using System.Web.Mvc;
using CosmeticaShop.IServices;
using CosmeticaShop.Services;
using CosmeticaShop.Web.Infrastructure;

namespace CosmeticaShop.Web.Controllers
{
    public class FavoritesController : BaseController
    {
        
        private readonly IWishService _wishService = new WishService();
        public ActionResult Index()
        {            
            var user = new WebUser();
            if (user.IsAuthorized)
            {
                var model = _wishService.GetWishs(user.UserId);
                return View(model);
            }
            var cookieModel = _wishService.GetCookieWishs();
            return View(cookieModel);
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
                var model = _wishService.DeleteWish(user.UserId,productId);
                return Json(model);
            }
            var cookieModel = _wishService.DeleteWish(null,productId);
            return Json(cookieModel);
        }
    }
}