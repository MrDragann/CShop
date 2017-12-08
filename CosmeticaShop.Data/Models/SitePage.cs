using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.Data.Models.Seo;

namespace CosmeticaShop.Data.Models
{
    /// <summary>
    /// Страницы сайта
    /// </summary>
    public class SitePage:SeoTags
    {
        /// <summary>
        /// Ид страницы (EnumSitePage)
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Заголовок страницы
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Контент страницы
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Дополнительный контент
        /// </summary>
        public string ExtraContent { get; set; }
    }
}
