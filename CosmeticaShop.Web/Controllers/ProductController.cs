using System.Linq;
using System.Web.Mvc;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Pagination;
using CosmeticaShop.Services;
using CosmeticaShop.Web.Infrastructure;
using CosmeticaShop.Web.Models;
using CosmeticaShop.IServices.Models.Product;

namespace CosmeticaShop.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService = new ProductService();
        private readonly IOrderService _orderService = new OrderService();
        private readonly ICategoryService _categoryService = new CategoryService();
        /// <summary>
        /// Страница с списоком товаров
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? page, ProductFilterModel request)
        {
            const int take = 21;
            var products = _productService.GetProducts(request);
            if (!page.HasValue)
                page = 1;
            var model = new ProductsViewModel()
            {
                
                Products = products,
                Brands = _productService.GetAllBrandsBase(),
                Pagination = new PaginationModel(take)
                {
                Count = products.Count,
                Skip = ((int)page - 1) * take,
                Take = take,
                PageNumber = (int)page,
                PageSize = products.Count / take + 1
            },
                Filter = request,
                Categories = _categoryService.GetCategories()
            };
            model.Products = model.Products.Skip(model.Pagination.Skip).Take(model.Pagination.Take).ToList();
            return View(model);
        }
        /// <summary>
        /// Добавить товар в корзину
        /// </summary>
        /// <param name="productId">Ид товара</param>
        /// <param name="quantity">Количество товара</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddInCart(int productId,int quantity)
        {
            var userId = new WebUser().UserId;
            var response = _orderService.AddProductInCart(productId, userId, quantity);
            return Json(response);
        }
        /// <summary>
        /// Добавить товар в желаемое
        /// </summary>
        /// <param name="productId">Ид товара</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddInWish(int productId)
        {
            var userId = new WebUser().UserId;
            var response = _productService.AddProductInWish(productId, userId);
            return Json(response);
        }
        /// <summary>
        /// Детальная страница товара
        /// </summary>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            var model = _productService.GetProduct(id);
            SetSitePageSettings(model.Name,model.SeoKeywords,model.SeoDescription);
            return View(model);
        }
    }
}