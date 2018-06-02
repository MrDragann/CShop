using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.Data.Models.Seo;

namespace CosmeticaShop.Data.Models
{
    public class Blog : SeoTags
    {
        #region [ Свойства ]

        public int Id { get; set; }

        public DateTime DateCreate { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string PreviewContent { get; set; }

        public string PhotoUrl { get; set; }

        public string KeyUrl { get; set; }

        public bool IsActive { get; set; }

        public DateTime? IsDelete { get; set; }

        #endregion

    }
}
