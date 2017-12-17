using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Models.Product;

namespace CosmeticaShop.IServices.Models.Order
{
    public class OrderProductsModel
    {
        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ид заказа
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Ид товара
        /// </summary>
        public int ProductId { get; set; }
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
        /// Признак удаления
        /// </summary>
        public DateTime? IsDelete { get; set; }
        /// <summary>
        /// Товар
        /// </summary>
        public ProductBaseModel Product { get; set; }
    }
}
