using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Enums;

namespace CosmeticaShop.IServices.Models.Order
{
    public class OrderHeaderModel
    {
        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Ид пользователя
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Дата создание заказа
        /// </summary>
        public DateTime DateCreate { get; set; }
        /// <summary>
        /// Статус заказа - EnumStatusOrder
        /// </summary>
        public EnumStatusOrder Status { get; set; }
        /// <summary>
        /// JSon строка содержащая  адрес заказа
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Признак удаления
        /// </summary>
        public DateTime? IsDelete { get; set; }

        /// <summary>
        /// Связь с таблицей товаров заказов
        /// </summary>
        public List<OrderProductsModel> OrderProducts { get; set; }
    }
}
