using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using CosmeticaShop.Data;
using CosmeticaShop.Data.Models;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Order;
using CosmeticaShop.IServices.Models.Product;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.Services.Static;
using Newtonsoft.Json;

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
        /// <returns></returns>
        public BaseResponse AddProductInCart(int productId, Guid userId,int quantity)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var product = db.Products.FirstOrDefault(x => x.Id == productId);
                    if (product == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Товар не был найден");
                    var user = db.Users.Include(x=>x.UserAddress).FirstOrDefault(x => x.Id == userId);
                    if (user == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Пользователь не был найден");
                    var orders = db.OrderHeaders.Where(x => x.UserId == user.Id).ToList();
                    var order = orders.FirstOrDefault(x => x.Status == (int) EnumStatusOrder.Cart);
                    if (order == null)
                    {
                         order = new OrderHeader()
                        {
                            UserId = user.Id,
                            DateCreate = DateTime.Now,
                            Status = (int) EnumStatusOrder.Cart,
                            Address = JsonConvert.SerializeObject(new {Address = user.UserAddress.Address,City = user.UserAddress.City,Country = user.UserAddress.Country,Phone = user.UserAddress.Phone}),
                            Amount = CalculationService.GetDiscountPrice(product.Price, product.Discount) * quantity
                        };
                        db.OrderHeaders.Add(order);
                        db.SaveChanges();              
                    }
                    else
                    {
                        order.Amount+= CalculationService.GetDiscountPrice(product.Price, product.Discount) * quantity;
                    }
                    db.OrderProducts.Add(new OrderProduct()
                    {
                        OrderId = order.Id,
                        ProductId = product.Id,
                        Price = product.Price,
                        Quantity = quantity,
                        Discount = product.Discount,
                        Amount =  CalculationService.GetDiscountPrice(product.Price,product.Discount) * quantity,
                    });
                    db.SaveChanges();
                    return new BaseResponse(EnumResponseStatus.Success,"Товар успешно добавлен в корзину");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
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
                var user = db.Users.Include(x=>x.OrderHeaders).FirstOrDefault(x => x.Id == userId);
                return user.OrderHeaders.Where(x=> x.Status != (int)EnumStatusOrder.Cart).Select(ConvertToOrderHeaderModel).ToList();
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

        #endregion

        #region [ Административная часть ]




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
                Status = (EnumStatusOrder) m.Status,
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
                Quantity =m.Quantity,
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
