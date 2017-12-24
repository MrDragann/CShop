using System;
using System.Collections.Generic;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Models.User;

namespace CosmeticaShop.IServices.Models.Order.Admin
{
    public class OrderHeaderModel
    {
        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Дата создание заказа
        /// </summary>
        public DateTime DateCreate { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Статус заказа - EnumStatusOrder
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Наименование статуса заказа
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// Адрес заказа
        /// </summary>
        public AddressModel Address { get; set; }
        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Связь с таблицей товаров заказов
        /// </summary>
        public List<OrderProductModel> OrderProducts { get; set; }
    }
}
