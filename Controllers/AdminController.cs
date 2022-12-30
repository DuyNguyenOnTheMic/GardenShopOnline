using System.Web.Mvc;

namespace GardenShopOnline.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Index
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "AspNetUsers");
            }
            return View();
        }
    }
}