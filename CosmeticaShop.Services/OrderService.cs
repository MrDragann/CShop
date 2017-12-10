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
using CosmeticaShop.IServices.Models.Responses;

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
                    var user = db.Users.FirstOrDefault(x => x.Id == userId);
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
                            Address = "",
                            Amount = (product.Price - product.Discount) * quantity
                        };
                        db.OrderHeaders.Add(order);
                        db.SaveChanges();              
                    }
                    db.OrderProducts.Add(new OrderProduct()
                    {
                        OrderId = order.Id,
                        ProductId = product.Id,
                        Price = product.Price,
                        Quantity = quantity,
                        Discount = product.Discount,
                        Amount = (product.Price - product.Discount) * quantity,
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

        #endregion

        #region [ Административная часть ]




        #endregion
    }
}
