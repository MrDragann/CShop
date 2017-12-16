using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CosmeticaShop.Data;
using CosmeticaShop.Data.Models;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Order;
using CosmeticaShop.IServices.Models.Product;
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
                var products = db.Products.Include(x=>x.Brand).ToList();
                var order = db.OrderHeaders.Include(x => x.OrderProducts).FirstOrDefault(x => x.UserId == userId && x.Status == (int)EnumStatusOrder.Cart);
                if(order==null)
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
                //todo:вынести в функцию
                DiscountPrice = Math.Floor(m.Price - (m.Price * m.Discount / 100))
            };
        }
        #endregion
    }
}
