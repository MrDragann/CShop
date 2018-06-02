using System.Collections.Generic;
using CosmeticaShop.IServices.Models.Blog;
using CosmeticaShop.IServices.Models.Product;

namespace CosmeticaShop.Web.Models
{
    public class BlogDetailViewModel
    {
        public BlogDetailViewModel()
        {
            Blog = new BlogModel();
            Blogs = new List<BlogModel>();
            Products = new List<ProductBaseModel>();
        }

        public BlogModel Blog { get; set; }

        /// <summary>
        /// Статьи
        /// </summary>
        public List<BlogModel> Blogs { get; set; }

        /// <summary>
        /// Товары
        /// </summary>
        public List<ProductBaseModel> Products { get; set; }
    }
}
