using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.IServices.Enums
{
    /// <summary>
    /// Статус заказа
    /// </summary>
    public enum EnumStatusOrder
    {
        /// <summary>
        /// В корзине
        /// </summary>
        [Description("В корзине")]
        Cart = 0,
        /// <summary>
        /// В ожидании
        /// </summary>
        [Description("В ожидании")]
        Pending = 1,
        /// <summary>
        /// Обработка
        /// </summary>
        [Description("Обработка")]
        Processing = 2,
        /// <summary>
        /// Завершенный
        /// </summary>
        [Description("Завершенный")]
        Complete = 3,
        /// <summary>
        /// Отменен
        /// </summary>
        [Description("Отменен")]
        Canceled = 4

    }
}
