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
        [Description("Principală")]
        Home = 1,
        /// <summary>
        /// Контакты
        /// </summary>
        [Description("Contacte")]
        Contacts = 2,
        /// <summary>
        /// О нас
        /// </summary>
        [Description("Despre noi")]
        About = 3,
        /// <summary>
        /// Использование куков
        /// </summary>
        [Description("Politica de utilizare Cookie-uri")]
        PoliticaCookie = 4,
        /// <summary>
        /// Оптовые продажи
        /// </summary>
        [Description("Vânzări angro")]
        VinzariAgro = 5,
        /// <summary>
        /// Реквизиты
        /// </summary>
        [Description("Rechizite")]
        Rechizite = 6,
        /// <summary>
        /// Доставка и возврат
        /// </summary>
        [Description("Livrare și returnare")]
        Livrare = 7,
        /// <summary>
        /// Политика конфиденциальности
        /// </summary>
        [Description("Politica de confidențialitate")]
        PrivacyPolicy = 8,
        /// <summary>
        /// Публичное предложение
        /// </summary>
        [Description("Oferta publică")]
        OfertaPublic = 9,
        /// <summary>
        /// Блог
        /// </summary>
        [Description("Blog")]
        Blog = 10,
        /// <summary>
        /// Товары
        /// </summary>
        [Description("Produse")]
        Products = 11,
    }
}
