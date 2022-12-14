using GardenShopOnline.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GardenShopOnline.Controllers
{
    [CustomAuthorize(Roles = "Admin, Staff")]
    public class CategoriesController : Controller
    {
        private readonly BonsaiGardenEntities db = new BonsaiGardenEntities();

        // GET: Categories
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _CategoryList()
        {
            var categories = db.Categories.Where(c => c.Status != 3).OrderByDescending(c => c.ID);

            return PartialView(categories.ToList());
        }
        public JsonResult Create_Category(string name)
        {
            string message = "";
            bool status = true;
            int check = db.Categories.Where(c => c.Name == name).Count();
            if (check > 0)
            {
                status = false;
                message = "Category name already exists";
            }
            else
            {
                Category Category = new Category
                {
                    Name = name,
                    Status = 1
                };
                db.Categories.Add(Category);
                db.SaveChanges();
                message = "Created successfully";
                status = true;
            }

            return Json(new { status, message }, JsonRequestBehavior.AllowGet);
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
            string message = "";
            bool status = true;
            int check = db.Categories.Where(c => c.Name == categorys.Name).Count();
            if (check > 0)
            {
                status = false;
                message = "Category name already exists";
            }
            else
            {
                Category categories = db.Categories.Find(categorys.ID);
                categories.Name = categorys.Name;
                db.Entry(categories).State = EntityState.Modified;
                db.SaveChanges();
                message = "Record Saved Successfully ";
                status = true;
            }

            return Json(new { status, message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete_Category(Category categorys)
        {
            bool status = true; try
            {
                Category categories = db.Categories.Find(categorys.ID);
                db.Categories.Remove(categories);
                db.SaveChanges();
            }
            catch
            {
                status = false;
            }

            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCategory()
        {

            return Json(db.Categories.Where(c => c.Status == 1).OrderByDescending(c => c.ID).Select(x => new
            {
                categoryID = x.ID,
                categoryName = x.Name
            }).ToList(), JsonRequestBehavior.AllowGet);
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
