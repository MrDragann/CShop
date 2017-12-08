using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CosmeticaShop.IServices.Models.Brand
{
    public class BrandModel
    {
        #region [ Свойства ]

        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Url бренда
        /// </summary>
        public string KeyUrl { get; set; }

        /// <summary>
        /// Видимость
        /// </summary>
        public bool IsActive { get; set; }

        public string SeoKeywords { get; set; }

        public string SeoDescription { get; set; }

        public string PhotoUrl { get; set; }

        public HttpPostedFileBase PhotoFile { get; set; }

        #endregion
    }
}