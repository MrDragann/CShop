using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Requests;
using CosmeticaShop.Services;
using CosmeticaShop.Web.Models;

namespace CosmeticaShop.Web.Controllers
{
    public class BlogController : BaseController
    {
        #region [ Сервисы ]

        private readonly IBlogService _blogService = new BlogService();
        private readonly IProductService _productService = new ProductService();
        #endregion

        // GET: Blog
        public ActionResult Index(PaginationRequest request)
        {
            SetSitePageSettings(EnumSitePage.Blog);
            var model = _blogService.GetBlogPostList(request);
            return View(model.Items);
        }

        public ActionResult Detail(string keyUrl)
        {
            var blogModel = _blogService.GetBlogPostDetail(keyUrl);
            if (!blogModel.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            var model = new BlogDetailViewModel
            {
                Blog = blogModel.Value,
                Products = _productService.GetRecomendProducts(4),
                Blogs = _blogService.GetRecentBlogs(blogModel.Value.Id)
            };
            SetSitePageSettings(blogModel.Value.Title, blogModel.Value.SeoKeywords, blogModel.Value.SeoDescription);
            return View(model);
        }
    }
}