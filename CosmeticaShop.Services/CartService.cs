using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;
using CosmeticaShop.Data;
using CosmeticaShop.Data.Models;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Order;
using CosmeticaShop.IServices.Models.Product;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.Services.Static;

namespace CosmeticaShop.Services
{
    public class CartService : ICartService
    {
        /// <summary>
        /// Получить корзину пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<OrderProductsModel> GetCart(Guid userId)
        {
            using (var db = new DataContext())
            {
                var products = db.Products.Include(x => x.Brand).ToList();
                var order = db.OrderHeaders.Include(x => x.OrderProducts).FirstOrDefault(x => x.UserId == userId && x.Status == (int)EnumStatusOrder.Cart);
                if (order == null)
                    return new List<OrderProductsModel>();
                foreach (var product in order.OrderProducts)
                {
                    product.Product = products.FirstOrDefault(x => x.Id == product.ProductId);
                    if (product.Product != null)
                    {
                        product.Product.PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, product.Product.Id.ToString());
                    }

                }
                var productsCart = order.OrderProducts.Select(ConvertToProductModel).ToList();

                return productsCart;
            }
        }
        /// <summary>
        /// Получение данных о корзине из куки
        /// </summary>
        /// <returns>ModelCarts.</returns>
        public List<OrderProductsModel> GetCookieCart()
        {
            HttpCookie cookieReq = HttpContext.Current.Request.Cookies["UserCart"];
            if (string.IsNullOrWhiteSpace(cookieReq?.Values["productId"]))
            {
                return new List<OrderProductsModel>();
            }
            var cookieProducts = cookieReq.Values["productId"].Split(',').Select(int.Parse).ToList();
            var cookieQuantity = cookieReq.Values["quantity"].Split(',').Select(int.Parse).ToList();

            List<OrderProductsModel> cookieCart = new List<OrderProductsModel>();
            for (int i = 0; i < cookieProducts.Count; i++)
            {
                cookieCart.Add(new OrderProductsModel { ProductId = cookieProducts[i], Quantity = cookieQuantity[i] });
            }
            using (var db = new DataContext())
            {
                var products = db.Products.Include(x => x.Brand).ToList();
                foreach (var cartModel in cookieCart)
                {
                    var product = products.FirstOrDefault(x => x.Id == cartModel.ProductId);
                    if (product != null)
                    {
                        cartModel.Product = new ProductBaseModel()
                        {
                            Price = product.Price,
                            BrandName = product.Brand?.Name,
                            DiscountPercent = product.Discount,
                            DiscountPrice = CalculationService.GetDiscountPrice(product.Price, product.Discount),
                            Name = product.Name,
                            Id = product.Id,
                            PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product,
                                cartModel.ProductId.ToString())
                        };
                        cartModel.Price = product.Price;
                        cartModel.Discount = product.Discount;
                        cartModel.Amount = CalculationService.GetDiscountPrice(product.Price, product.Discount) * cartModel.Quantity;
                    }

                }
            }
            return cookieCart;
        }

        /// <summary>
        /// Дополнить желаемое из куки
        /// </summary>
        /// <returns></returns>
        public void ComplementCart(Guid userId)
        {
            using (var db = new DataContext())
            {
                var cart = db.OrderHeaders.Include(x => x.OrderProducts).FirstOrDefault(x => x.UserId == userId && x.Status == (int)EnumStatusOrder.Cart);
                HttpCookie cookieReq = HttpContext.Current.Request.Cookies["UserCart"];
                if (cookieReq == null || string.IsNullOrWhiteSpace(cookieReq.Values["productId"]))
                {
                    if (cart != null)
                    {
                        HttpCookie aCookie = new HttpCookie("UserCart");
                        var products = cart.OrderProducts.ToList();
                        var cooikeIds = string.Join(",", products.Select(x => x.Product));
                        var quantities = string.Join(",", products.Select(x => x.Quantity));
                        aCookie.Values["productId"] = cooikeIds;
                        aCookie.Values["quantity"] = quantities;
                        aCookie.Expires = DateTime.Now.AddDays(30);
                        HttpContext.Current.Response.Cookies.Add(aCookie);
                    }
                }
                else
                {
                    if (cart != null)
                    {
                        var cookieProducts = cookieReq.Values["productId"].Split(',').Select(int.Parse).ToList();
                        var cookieQuantity = cookieReq.Values["quantity"].Split(',').Select(int.Parse).ToList();
                        var products = db.Products.ToList();
                        List<OrderProduct> cookieCart = new List<OrderProduct>();
                        for (int i = 0; i < cookieProducts.Count(); i++)
                        {
                            if (cart.OrderProducts.Any(x => x.ProductId == cookieProducts[i]))
                                continue;
                            var product = products.FirstOrDefault(x => x.Id == cookieProducts[i]);
                            if (product != null)
                            {
                                var productIndex = cookieQuantity.FindIndex(x => x == cookieProducts[i]);

                                cookieCart.Add(new OrderProduct
                                {
                                    ProductId = cookieProducts[i],
                                    Price = product.Price,
                                    Discount = product.Discount,
                                    Amount = CalculationService.GetDiscountPrice(product.Price, product.Discount) * cookieQuantity[productIndex],
                                    Quantity = cookieQuantity[productIndex]
                                });
                            }
                        }
                        db.OrderProducts.AddRange(cookieCart);
                        db.SaveChanges();
                        foreach (var product in cart.OrderProducts)
                        {
                            for (int i = 0; i < cookieProducts.Count(); i++)
                            {
                                if (cookieProducts.Contains(product.ProductId))
                                    continue;
                                cookieReq.Values["productId"] += "," + product.ProductId;
                                cookieReq.Values["quantity"] += "," + product.Quantity;
                            }
                        }
                        HttpContext.Current.Response.Cookies.Add(cookieReq);
                    }
                }
            }
        }

        /// <summary>
        /// Удалить товар из корзины
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="productId">Ид товара</param>
        /// <returns></returns>
        public BaseResponse DeleteProduct(Guid userId, int productId)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var order = db.OrderHeaders.Include(x => x.OrderProducts).FirstOrDefault(x => x.UserId == userId && x.Status == (int)EnumStatusOrder.Cart);
                    if (order == null)
                    {
                        HttpCookie cookieReq = HttpContext.Current.Request.Cookies["User"];
                        if (!string.IsNullOrWhiteSpace(cookieReq?.Value))
                        {                            
                            userId = Guid.Parse(cookieReq.Value);
                            order = db.OrderHeaders.Include(x => x.OrderProducts).FirstOrDefault(x => x.UserId == userId && x.Status == (int)EnumStatusOrder.Cart);
                            if(order == null)
                                return new BaseResponse(EnumResponseStatus.Error, "Корзина не найдена");
                        }
                       
                    }
                    var product = order.OrderProducts.FirstOrDefault(x => x.ProductId == productId);
                    if (product == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Товар не найден");
                    db.OrderProducts.Remove(product);
                    order.Amount -= product.Amount * product.Quantity;
                    db.SaveChanges();
                    CookieDeleteProduct(productId);
                    return new BaseResponse(EnumResponseStatus.Success);
                }
            }
            catch (Exception e)
            {
                return new BaseResponse(EnumResponseStatus.Exception, e.Message);
            }

        }
        /// <summary>
        /// Удалить товар из куки корзины 
        /// </summary>
        /// <param name="productId">Ид товара</param>
        /// <returns></returns>
        public void CookieDeleteProduct(int productId)
        {
            HttpCookie cookieReq = HttpContext.Current.Request.Cookies["UserCart"];
            if (cookieReq != null)
            {
                var cookieProducts = cookieReq.Values["productId"].Split(',').Select(int.Parse).ToList();
                var cookieQuantity = cookieReq.Values["quantity"].Split(',').Select(int.Parse).ToList();
                var productIndex = cookieProducts.FindIndex(x => x == productId);
                cookieProducts.Remove(productId);
                cookieQuantity.RemoveAt(productIndex);
                cookieReq.Values["productId"] = string.Join(",", cookieProducts.ToArray());
                cookieReq.Values["quantity"] = string.Join(",", cookieQuantity.ToArray());
                //aCookie.Expires = DateTime.Now.AddDays(7);
                HttpContext.Current.Response.Cookies.Add(cookieReq);
            }           
        }
        #region Конвертация

        public OrderProductsModel ConvertToProductModel(OrderProduct m)
        {
            var product = new ProductBaseModel();
            if (m.Product != null)
            {
                product = ConvertToProductBaseModel(m.Product);
            }
            return new OrderProductsModel
            {
                Amount = m.Amount,
                ProductId = m.ProductId,
                Price = m.Price,
                IsDelete = m.IsDelete,
                OrderId = m.OrderId,
                Quantity = m.Quantity,
                Id = m.Id,
                Discount = m.Discount,
                Product = product
            };
        }
        private ProductBaseModel ConvertToProductBaseModel(Product m)
        {
            return new ProductBaseModel
            {
                Id = m.Id,
                Name = m.Name,
                BrandName = m.Brand?.Name,
                Price = m.Price,
                DiscountPercent = m.Discount,
                PhotoUrl = m.PhotoUrl,
                DiscountPrice = CalculationService.GetDiscountPrice(m.Price, m.Discount)
            };
        }
        #endregion
    }
}
