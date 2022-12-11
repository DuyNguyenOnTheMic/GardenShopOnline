using GardenShopOnline.Models;
using System.Linq;
using System.Web.Mvc;

namespace GardenShopOnline.Controllers
{
    public class HomeController : Controller
    {
        readonly BonsaiGardenEntities db = new BonsaiGardenEntities();
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }
    }
}