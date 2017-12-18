using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.IServices.Enums
{
    public enum EnumSitePage
    {
        /// <summary>
        /// Главная
        /// </summary>
        [Description("Главная")]
        Home = 1,
        /// <summary>
        /// Список желаемого
        /// </summary>
        [Description("Список желаемого")]
        WishList = 2,
        /// <summary>
        /// Контакты
        /// </summary>
        [Description("Контакты")]
        Contacts = 3,
        /// <summary>
        /// Корзина
        /// </summary>
        [Description("Корзина")]
        Cart = 4,
        /// <summary>
        /// О нас
        /// </summary>
        [Description("О нас")]
        About = 5,
        /// <summary>
        /// Оформление заказа
        /// </summary>
        [Description("Оформление заказа")]
        Ordering = 6,
        /// <summary>
        /// Помощь
        /// </summary>
        [Description("Помощь")]
        Help = 7,
        /// <summary>
        /// Профиль пользователя
        /// </summary>
        [Description("Профиль пользователя")]
        UserProfile = 8
    }
}
