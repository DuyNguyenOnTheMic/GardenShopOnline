using GardenShopOnline.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GardenShopOnline.Controllers
{
    public class CategogiesController : Controller
    {
        private readonly BonsaiGardenEntities db = new BonsaiGardenEntities();

        // GET: Categories
        public ActionResult Index()
        {
            var Categogies = db.Categories.Where(c => c.Status != 3).OrderByDescending(c => c.ID);

            return View(Categogies.ToList());
        }
        public ActionResult Create_Category(string name_Category)
        {
            Category Category = new Category
            {
                Name = name_Category,
                Status = 1
            };
            db.Categories.Add(Category);
            db.SaveChanges();
            Session["notification"] = "Thêm mới thành công!";
            return RedirectToAction("Index");
        }
        public ActionResult EditStatus_Category(Category Categorys)
        {
            Category categories = db.Categories.Find(Categorys.ID);
            if (categories.Status == 1)
            {
                categories.Status = 2;
            }
            else
            {
                categories.Status = 1;
            }
            db.Entry(categories).State = EntityState.Modified;
            db.SaveChanges();
            return Json("EditStatus_Category", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult FindCategory(int Category_id)
        {
            Category categories = db.Categories.Find(Category_id);
            var emp = new Category
            {
                ID = Category_id,
                Name = categories.Name
            };
            return Json(emp);
        }
        public JsonResult UpdateCategory(Category categorys)
        {
            Category categories = db.Categories.Find(categorys.ID);
            categories.Name = categorys.Name;
            db.Entry(categories).State = EntityState.Modified;
            db.SaveChanges();
            string message = "Record Saved Successfully ";
            bool status = true;
            return Json(new { status, message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete_Category(Category categorys)
        {
            Category categories = db.Categories.Find(categorys.ID);
            categories.Status = 3;
            db.Entry(categories).State = EntityState.Modified;
            db.SaveChanges();
            return Json("Delete_Category", JsonRequestBehavior.AllowGet);
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
