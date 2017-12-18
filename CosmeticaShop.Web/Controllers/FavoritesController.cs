using System.Web.Mvc;

namespace CosmeticaShop.Web.Controllers
{
    public class FavoritesController : BaseController
    {
        // GET: Favorites
        public ActionResult Index()
        {
            return View();
        }
    }
}