using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Enums;

namespace CosmeticaShop.IServices.Models.SitePage
{
    public class SitePageModel
    {
        /// <summary>
        /// Ид страницы (EnumSitePage)
        /// </summary>
        public EnumSitePage Id { get; set; }
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

        public string SeoKeywords { get; set; }

        public string SeoDescription { get; set; }
    }
}
