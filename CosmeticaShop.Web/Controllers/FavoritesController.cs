using System.Web.Mvc;
using CosmeticaShop.IServices;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.Services;
using CosmeticaShop.Web.Infrastructure;
using CosmeticaShop.Web.Models;

namespace CosmeticaShop.Web.Controllers
{
    public class FavoritesController : BaseController
    {

        private readonly IWishService _wishService = new WishService();
        private readonly IProductService _productService = new ProductService();
        public ActionResult Index()
        {
            var user = new WebUser();
            var model = new WishViewModel
            {
                Recommends = _productService.GetRecomendProducts(4)
            };
            if (user.IsAuthorized)
            {
                model.Wishes = _wishService.GetWishs(user.UserId);
            }
            else
                model.Wishes = _wishService.GetCookieWishs();
            return View(model);
        }

        /// <summary>
        /// Удалить товар из желаемого
        /// </summary>
        /// <param name="productId">Ид товара</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteWish(int productId)
        {
            var user = new WebUser();
            if (user.IsAuthorized)
            {
                var model = _wishService.DeleteWish(user.UserId, productId);
                return Json(model);
            }
            var cookieModel = _wishService.DeleteWish(null, productId);
            return Json(cookieModel);
        }
    }
}