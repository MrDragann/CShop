using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.IServices.Enums
{
    /// <summary>
    /// Статусы пользователей
    /// </summary>
    public enum EnumStatusUser
    {
        /// <summary>
        /// Новый
        /// </summary>
        [Description("Новый")]
        New = 1,
        /// <summary>
        /// Подтвержден
        /// </summary>
        [Description("Подтвержден")]
        Active = 2,
        /// <summary>
        /// Заблокирован
        /// </summary>
        [Description("Заблокирован")]
        Locked = 3,
        /// <summary>
        /// Неавторизованный пользователь
        /// </summary>
        [Description("Неавторизованный")]
        Unauthorized = 4
    }
}
