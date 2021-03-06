﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.Data.Models
{
    /// <summary>
    /// Таблица заказов
    /// </summary>
    public class OrderHeader
    {
        #region [ Свойства ]

        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Ид пользователя
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Ид купона
        /// </summary>
        public int? CouponId { get; set; }
        /// <summary>
        /// JSon строка содержащая  информацию о купоне
        /// </summary>
        public string CouponJson { get; set; }
        /// <summary>
        /// Дата создание заказа
        /// </summary>
        public DateTime DateCreate { get; set; }
        /// <summary>
        /// Статус заказа - EnumStatusOrder
        /// </summary>
        public int Status { get; set; }
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

        #endregion

        #region [ Связанные объекты ]

        /// <summary>
        /// Связь с купоном
        /// </summary>
        public Coupon Coupon { get; set; }

        /// <summary>
        /// Связь с таблицей товаров заказов
        /// </summary>
        public List<OrderProduct> OrderProducts { get; set; }
        /// <summary>
        /// Заказчик
        /// </summary>
        public User User { get; set; }

        #endregion
    }
}
