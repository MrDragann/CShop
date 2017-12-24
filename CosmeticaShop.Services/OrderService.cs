using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using CosmeticaShop.Data;
using CosmeticaShop.Data.Models;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Coupon;
using CosmeticaShop.IServices.Models.Order;
using CosmeticaShop.IServices.Models.Product;
using CosmeticaShop.IServices.Models.Requests;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.User;
using CosmeticaShop.Services.Static;
using Newtonsoft.Json;
using OrderAdmin = CosmeticaShop.IServices.Models.Order.Admin;

namespace CosmeticaShop.Services
{
    public class OrderService : IOrderService
    {
        #region [ Публичная часть ]

        /// <summary>
        /// Добавить товар в корзину
        /// </summary>
        /// <param name="productId">Ид товара</param>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="quantity">Количество</param>
        /// <param name="isCookie">Добавление в корзину через куки</param>
        /// <returns></returns>
        public BaseResponse AddProductInCart(int productId, Guid userId, int quantity, bool isCookie)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var product = db.Products.FirstOrDefault(x => x.Id == productId);
                    if (product == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Товар не был найден");
                    var user = db.Users.Include(x => x.UserAddress).FirstOrDefault(x => x.Id == userId);
                    if (user == null && !isCookie)
                        return new BaseResponse(EnumResponseStatus.Error, "Пользователь не был найден");
                    if (user == null && isCookie)
                    {
                        user = new User
                        {
                            Id = userId,
                            Email = "unknown",
                            FirstName = "unknown",
                            LastName = "unknown",
                            PasswordHash = "unknown",
                            DateBirth = null,
                            ConfirmationToken = null,
                            Status = (int)EnumStatusUser.Unauthorized,
                            UserAddress = new UserAddress
                            {
                                Country = "unknown",
                                City = "unknown",
                                Address = "unknown",
                                Phone = "unknown"
                            }
                        };
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                    var orders = db.OrderHeaders.Include(x => x.OrderProducts).Where(x => x.UserId == user.Id).ToList();
                    var order = orders.FirstOrDefault(x => x.Status == (int)EnumStatusOrder.Cart);
                    if (order == null)
                    {
                        order = new OrderHeader()
                        {
                            UserId = user.Id,
                            DateCreate = DateTime.Now,
                            Status = (int)EnumStatusOrder.Cart,
                            Address = JsonConvert.SerializeObject(new { Address = user.UserAddress.Address, City = user.UserAddress.City, Country = user.UserAddress.Country, Phone = user.UserAddress.Phone }),
                            Amount = CalculationService.GetDiscountPrice(product.Price, product.Discount) * quantity,
                            OrderProducts = new List<OrderProduct>()
                        };
                        db.OrderHeaders.Add(order);
                        db.SaveChanges();
                    }
                    else
                    {
                        order.Amount += CalculationService.GetDiscountPrice(product.Price, product.Discount) * quantity;
                    }
                    if (order.OrderProducts.All(x => x.ProductId != product.Id))
                    {
                        db.OrderProducts.Add(new OrderProduct()
                        {
                            OrderId = order.Id,
                            ProductId = product.Id,
                            Price = product.Price,
                            Quantity = quantity,
                            Discount = product.Discount,
                            Amount = CalculationService.GetDiscountPrice(product.Price, product.Discount) * quantity,
                        });
                    }
                    else
                    {
                        var orderProduct = order.OrderProducts.FirstOrDefault(x => x.ProductId == product.Id);
                        if (orderProduct != null)
                        {
                            orderProduct.Quantity += quantity;
                            orderProduct.Amount = CalculationService.GetDiscountPrice(orderProduct.Price, orderProduct.Discount) * orderProduct.Quantity;
                        }
                    }
                    db.SaveChanges();
                    if (!isCookie)
                        AddProductInCoockieCart(productId, quantity,true);
                    return new BaseResponse(EnumResponseStatus.Success, "Товар успешно добавлен в корзину");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Добавить товар в корзину куки
        /// </summary>
        /// <param name="productId">Ид товара</param>  
        /// <param name="quantity">Количество товара</param>
        /// <param name="isAuth">Добавляет авторизованый пользователь?</param>
        /// <returns></returns>
        public BaseResponse AddProductInCoockieCart(int productId, int quantity, bool isAuth)
        {
            HttpCookie cookieUser = HttpContext.Current.Request.Cookies["User"];
            var userId = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(cookieUser?.Value))
            {
                HttpCookie aCookie = new HttpCookie("User")
                {
                    Value = userId.ToString(),
                    Expires = DateTime.Now.AddDays(30)
                };
                HttpContext.Current.Response.Cookies.Add(aCookie);
            }
            else
            {
                userId = Guid.Parse(cookieUser.Value);
            }
            var response = new BaseResponse(EnumResponseStatus.Success, "Успешно");
            if (!isAuth)
            {
                response = AddProductInCart(productId, userId, quantity, true);
            }
            if (response.IsSuccess)
            {
                HttpCookie cookieReq = HttpContext.Current.Request.Cookies["UserCart"];
                if (string.IsNullOrWhiteSpace(cookieReq?.Values["productId"]))
                {
                    HttpCookie aCookie = new HttpCookie("UserCart");

                    aCookie.Values["productId"] = productId.ToString();
                    aCookie.Values["quantity"] = quantity.ToString();

                    aCookie.Expires = DateTime.Now.AddDays(30);
                    HttpContext.Current.Response.Cookies.Add(aCookie);
                }
                else
                {
                    var cookieProducts = cookieReq.Values["productId"].Split(',').Select(int.Parse).ToList();
                    var cookieQuantity = cookieReq.Values["quantity"].Split(',').Select(int.Parse).ToList();
                    if (cookieProducts.Contains(productId))
                    {
                        var productIndex = cookieProducts.FindIndex(x => x == productId);
                        cookieQuantity[productIndex] += 1;
                        cookieReq.Values["quantity"] = string.Join(",", cookieQuantity.ToArray());
                        HttpContext.Current.Response.Cookies.Add(cookieReq);
                    }
                    else
                    {
                        cookieReq.Values["productId"] += "," + productId;
                        cookieReq.Values["quantity"] += "," + quantity;
                        HttpContext.Current.Response.Cookies.Add(cookieReq);
                    }
                }
                return new BaseResponse(EnumResponseStatus.Success, "Товар успешно добавлен в желаемое");
            }
            return response;
        }
        /// <summary>
        /// Добавить товар в корзину
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="productsOrder">Товары для заказа</param>
        /// <returns></returns>
        public BaseResponse<int> PreparationOrder(Guid userId, List<OrderProductsModel> productsOrder)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var products = db.Products.ToList();
                    var order = db.OrderHeaders.Include(x => x.OrderProducts).FirstOrDefault(x => x.UserId == userId && x.Status == (int)EnumStatusOrder.Cart);
                    if (order == null)
                    {
                        HttpCookie cookieReq = HttpContext.Current.Request.Cookies["User"];
                        if (!string.IsNullOrWhiteSpace(cookieReq?.Value))
                        {
                            userId = Guid.Parse(cookieReq.Value);
                            order = db.OrderHeaders.Include(x => x.OrderProducts).FirstOrDefault(x => x.UserId == userId && x.Status == (int)EnumStatusOrder.Cart);
                            if (order == null)
                                return new BaseResponse<int>(EnumResponseStatus.Error, "Корзина не найдена");
                        }
                    }
                    decimal amount = 0;
                    foreach (var item in order.OrderProducts)
                    {
                        var orderModel = productsOrder.FirstOrDefault(x => x.ProductId == item.ProductId);
                        if (orderModel == null)
                            return new BaseResponse<int>(EnumResponseStatus.Error, "Один из товаров заказа не был найден");
                        var product = products.FirstOrDefault(x => x.Id == orderModel.ProductId);
                        if (product == null)
                            return new BaseResponse<int>(EnumResponseStatus.Error, "Один из товаров  не был найден");
                        item.Quantity = orderModel.Quantity;
                        amount += CalculationService.GetDiscountPrice(product.Price, product.Discount) * orderModel.Quantity;
                        item.Amount = CalculationService.GetDiscountPrice(product.Price, product.Discount) * orderModel.Quantity;
                    }
                    order.Amount = amount;
                    db.SaveChanges();
                    return new BaseResponse<int>(EnumResponseStatus.Success, "Успешно", order.Id);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<int>(EnumResponseStatus.Exception, ex.Message);
            }
        }
        /// <summary>
        /// Получить историю заказов
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        public List<OrderHeaderModel> GetHistoryOrders(Guid userId)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.Include(x => x.OrderHeaders).FirstOrDefault(x => x.Id == userId);
                return user.OrderHeaders.Where(x => x.Status != (int)EnumStatusOrder.Cart).Select(ConvertToOrderHeaderModel).ToList();
            }
        }
        /// <summary>
        /// Получить историю заказов
        /// </summary>
        /// <param name="orderId">Ид заказа</param>
        public OrderHeaderModel GetOrder(int orderId)
        {
            using (var db = new DataContext())
            {
                var products = db.Products.ToList();
                var order = db.OrderHeaders.Include(x => x.OrderProducts).FirstOrDefault(x => x.Id == orderId);
                foreach (var product in order.OrderProducts)
                {
                    product.Product = products.FirstOrDefault(x => x.Id == product.ProductId);
                    if (product.Product != null)
                    {
                        product.Product.PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, product.Product.Id.ToString());
                    }

                }
                return ConvertToOrderHeaderModel(order);
            }
        }

        /// <summary>
        ///Добавить заказ
        /// </summary>
        /// <param name="orderId">Ид заказа</param>
        /// <param name="address">Адрес доставки</param>
        public BaseResponse AddOrder(int orderId, AddressModel address)
        {
            try
            {
                using (var db = new DataContext())
                {
                    //var products = db.Products.ToList();
                    var order = db.OrderHeaders.FirstOrDefault(x => x.Id == orderId);
                    if (order == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Заказ не найден");
                    if (order.Status != (int)EnumStatusOrder.Cart)
                        return new BaseResponse(EnumResponseStatus.Error, "Заказ уже был совершен");
                    order.Status = (int)EnumStatusOrder.Pending;
                    order.Address = JsonConvert.SerializeObject(address);
                    order.DateCreate = DateTime.Now;
                    db.SaveChanges();
                    HttpCookie cookieReq = HttpContext.Current.Request.Cookies["UserCart"];
                    if (cookieReq != null)
                    {
                        cookieReq.Values["productId"] = "";
                        cookieReq.Values["quantity"] = "";
                        cookieReq.Value = "";
                        cookieReq.Expires = DateTime.Now.AddDays(-1d);
                        HttpContext.Current.Response.Cookies.Add(cookieReq);
                    }
                    return new BaseResponse(EnumResponseStatus.Success, "Заказ успешно совершен");
                }
            }
            catch (Exception e)
            {
                return new BaseResponse(EnumResponseStatus.Exception, e.Message);
            }
        }

        #endregion

        #region [ Административная часть ]

        /// <summary>
        /// Получить список заказов
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PaginationResponse<OrderAdmin.OrderHeaderModel> GetFilteredOrders(PaginationRequest<BaseFilter> request)
        {
            using (var db = new DataContext())
            {
                var query = db.OrderHeaders.AsNoTracking().Include(x=>x.User.UserAddress)
                    .Where(x => !x.IsDelete.HasValue && x.Status!=(int)EnumStatusOrder.Cart)
                    .OrderByDescending(x => x.DateCreate) as IQueryable<OrderHeader>;

                //if (!string.IsNullOrEmpty(request.Filter.Term))
                //    query = query.Where(x => x.Code.ToLower().Contains(request.Filter.Term.ToLower()));

                var model = new PaginationResponse<OrderAdmin.OrderHeaderModel> { Count = query.Count() };

                query = request.Load(query);

                model.Items = query.ToList().Select(x => new OrderAdmin.OrderHeaderModel
                {
                    Id = x.Id,
                    DateCreate = x.DateCreate,
                    UserName = $"{x.User?.FirstName} {x.User?.LastName}",
                    StatusName = EnumService.GetEnumDescription((EnumStatusOrder)x.Status),
                    Amount = x.Amount
                }).ToList();
                return model;
            }
        }

        /// <summary>
        /// Получить модель заказа
        /// </summary>
        /// <param name="orderId">Ид заказа</param>
        /// <returns></returns>
        public BaseResponse<OrderAdmin.OrderHeaderModel> GetOrderHeaderModel(int orderId)
        {
            using (var db = new DataContext())
            {
                var order = db.OrderHeaders.Include(x => x.User.UserAddress)
                    .Where(x => x.Id == orderId).ToList().Select(x=>new OrderAdmin.OrderHeaderModel
                    {
                        Id = x.Id,
                        DateCreate = x.DateCreate,
                        UserName = $"{x.User?.FirstName} {x.User?.LastName}",
                        Status = x.Status,
                        StatusName = EnumService.GetEnumDescription((EnumStatusOrder)x.Status),
                        Amount = x.Amount,
                        Address = GetAddress(x.Address)
                    }).FirstOrDefault();
                if (order == null)
                    return new BaseResponse<OrderAdmin.OrderHeaderModel>(EnumResponseStatus.Error, "Заказ не найден");
                order.OrderProducts = GetOrderProducts(orderId, db);
                return new BaseResponse<OrderAdmin.OrderHeaderModel>(order);
            }
        }

        private AddressModel GetAddress(string address)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<AddressModel>(address);
                return model;
            }
            catch (Exception ex)
            {
                return new AddressModel();
            }
        }

        /// <summary>
        /// Получить список товаров заказа
        /// </summary>
        /// <param name="orderId">Ид заказа</param>
        /// <param name="db"></param>
        /// <returns></returns>
        private List<OrderAdmin.OrderProductModel> GetOrderProducts(int orderId, DataContext db)
        {
            var model = db.OrderProducts.Include(x => x.Product)
                .Where(x => x.OrderId == orderId && !x.IsDelete.HasValue).ToList()
                .Select(x => new OrderAdmin.OrderProductModel
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product?.Name,
                    PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, x.ProductId.ToString()),
                    Quantity = x.Quantity,
                    Price = x.Price,
                    Discount = x.Discount,
                    Amount = x.Amount
                }).ToList();
            return model;
        }

        /// <summary>
        /// Изменить статус заказу
        /// </summary>
        /// <param name="orderId">Ид заказа</param>
        /// <param name="status">Статус</param>
        /// <returns></returns>
        public BaseResponse ChangeOrderStatus(int orderId, int status)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var order = db.OrderHeaders.FirstOrDefault(x => x.Id == orderId);
                    if(order==null)
                        return new BaseResponse(EnumResponseStatus.Error,"Заказ не найден");
                    order.Status = status;
                    db.SaveChanges();
                    return new BaseResponse(EnumResponseStatus.Success,"Статус успешно изменен");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception,ex.Message);
            }
        }

        #endregion

        #region Конвертация

        public OrderHeaderModel ConvertToOrderHeaderModel(OrderHeader m)
        {
            return new OrderHeaderModel
            {
                Amount = m.Amount,
                Address = m.Address,
                DateCreate = m.DateCreate,
                IsDelete = m.IsDelete,
                UserId = m.UserId,
                Status = (EnumStatusOrder)m.Status,
                Id = m.Id,
                OrderProducts = m.OrderProducts?.Select(ConvertToOrderHeaderModel).ToList()
            };
        }

        public OrderProductsModel ConvertToOrderHeaderModel(OrderProduct m)
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
                //todo:вынести в функцию
                DiscountPrice = Math.Floor(m.Price - (m.Price * m.Discount / 100))
            };
        }
        #endregion
    }
}
