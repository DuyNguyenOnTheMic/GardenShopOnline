using GardenShopOnline.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GardenShopOnline.Controllers
{
    public class TypesController : Controller
    {
        private readonly BonsaiGardenEntities db = new BonsaiGardenEntities();

        // GET: Types
        public ActionResult Index()
        {
            return View(db.Types.Where(c => c.Status != 3).OrderByDescending(c => c.ID).ToList());
        }

        public ActionResult Create_Type(string name_Type)
        {
            Models.Type Type = new Models.Type
            {
                Name = name_Type,
                Status = 1
            };
            db.Types.Add(Type);
            db.SaveChanges();
            Session["notification"] = "Thêm mới thành công!";
            return RedirectToAction("Index");
        }
        public ActionResult EditStatus_Type(Models.Type Types)
        {
            Models.Type Type = db.Types.Find(Types.ID);
            if (Type.Status == 1)
            {
                Type.Status = 2;
            }
            else
            {
                Type.Status = 1;
            }
            db.Entry(Type).State = EntityState.Modified;
            db.SaveChanges();
            return Json("EditStatus_Type", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult FindType(int category_id)
        {
            Models.Type Types = db.Types.Find(category_id);
            var emp = new Models.Type
            {
                ID = category_id,
                Name = Types.Name
            };
            return Json(emp);
        }
        public JsonResult UpdateType(Models.Type Type)
        {
            Models.Type Types = db.Types.Find(Type.ID);
            Types.Name = Type.Name;
            db.Entry(Types).State = EntityState.Modified;
            db.SaveChanges();
            string message = "Record Saved Successfully ";
            bool status = true;
            return Json(new { status, message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete_Type(Models.Type Type)
        {
            Models.Type Types = db.Types.Find(Type.ID);
            Types.Status = 3;
            db.Entry(Types).State = EntityState.Modified;
            db.SaveChanges();
            return Json("Delete_Type", JsonRequestBehavior.AllowGet);
        }
        public ActionResult getType()
        {

            return Json(db.Types.Where(c => c.Status == 1).OrderByDescending(c => c.ID).Select(x => new
            {
                TypeID = x.ID,
                TypeName = x.Name
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
