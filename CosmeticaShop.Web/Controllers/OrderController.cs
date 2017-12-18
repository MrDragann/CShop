using System.Web.Mvc;

namespace CosmeticaShop.Web.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
    }
}