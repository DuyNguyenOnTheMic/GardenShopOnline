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
            ShoppingCart.GetCart(HttpContext);
            ViewBag.Catgory =db.Categories.ToList();
            ViewBag.Type = db.Types.ToList();
            return View(db.Products.Where(p => p.Status == 1).ToList());
        }
        public ActionResult _ProductList(int categoryId, int typeId)
        {
            var links = from l in db.Products
                        where l.Category.Status != 3 && l.Type.Status != 3
                        select l;

            if (categoryId != -1)
            {
                links = links.Where(p => p.CategoryID == categoryId);
            }
            if (typeId != -1)
            {
                links = links.Where(p => p.TypeID == typeId);
            }
            return PartialView(links.Where(c => c.Status != 3).OrderByDescending(c => c.ID));

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}