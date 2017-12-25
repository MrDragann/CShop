using System;
using CosmeticaShop.IServices.Models.Product;

namespace CosmeticaShop.IServices.Models.Order.Admin
{
    public class OrderProductModel
    {
        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Ид товара
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Наименование товара
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Url изображения
        /// </summary>
        public string PhotoUrl { get; set; }
        /// <summary>
        /// Количество
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Скидка на товар
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// Сумма для платежа
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Цена со скидкой
        /// </summary>
        public decimal DiscountPrice { get; set; }
    }
}
