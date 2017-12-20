using System.Web.Mvc;
using CosmeticaShop.IServices;
using CosmeticaShop.Services;
using CosmeticaShop.Web.Infrastructure;

namespace CosmeticaShop.Web.Controllers
{
    public class FavoritesController : BaseController
    {
        
        private static IWishService _wishService = new WishService();
        public ActionResult Index()
        {
            var userId = new WebUser().UserId;
            var model = _wishService.GetWishs(userId);
            return View(model);
        }
    }
}