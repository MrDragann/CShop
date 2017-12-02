using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.IServices.Enums
{
    /// <summary>
    /// Список ролей пользователей
    /// </summary>
    public enum EnumUserRole
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        [Description("Пользователь")]
        User = 1,
        /// <summary>
        /// Администратор
        /// </summary>
        [Description("Администратор")]
        Admin = 2
    }
}
