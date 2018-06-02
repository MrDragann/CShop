using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CosmeticaShop.IServices.Models.Blog
{
    public class BlogModel
    {
        public int Id { get; set; }

        public DateTime DateCreate { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string PreviewContent { get; set; }

        public string PhotoUrl { get; set; }

        public string KeyUrl { get; set; }

        public string SeoKeywords { get; set; }

        public string SeoDescription { get; set; }

        public bool IsActive { get; set; }

        public HttpPostedFileBase PhotoFile { get; set; }
    }
}
