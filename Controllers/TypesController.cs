using GardenShopOnline.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GardenShopOnline.Controllers
{
    [CustomAuthorize(Roles = "Admin, Staff")]
    public class TypesController : Controller
    {
        private readonly BonsaiGardenEntities db = new BonsaiGardenEntities();

        // GET: Types
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _TypeList()
        {
            var types = db.Types.Where(c => c.Status != 3).OrderByDescending(c => c.ID);

            return PartialView(types.ToList());
        }

        public JsonResult Create_Type(string name)
        {
            string message = "";
            bool status = true;
            int check = db.Types.Where(c => c.Name == name).Count();
            if (check > 0)
            {
                status = false;
                message = "Type name already exists";
            }
            else
            {
                Models.Type Type = new Models.Type
                {
                    Name = name,
                    Status = 1
                };
                db.Types.Add(Type);
                db.SaveChanges();
                message = "Created successfully";
                status = true;
            }
            return Json(new { status, message }, JsonRequestBehavior.AllowGet);
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
            string message = "";
            bool status = true;
            int check = db.Types.Where(c => c.Name == Type.Name).Count();
            if (check > 0)
            {
                status = false;
                message = "Type name already exists";
            }
            else
            {
                Models.Type Types = db.Types.Find(Type.ID);
                Types.Name = Type.Name;
                db.Entry(Types).State = EntityState.Modified;
                db.SaveChanges();
                message = "Record Saved Successfully ";
                status = true;
            }
            return Json(new { status, message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete_Type(Models.Type Type)
        {
            bool status = true;
            try
            {
                Models.Type Types = db.Types.Find(Type.ID);
                db.Types.Remove(Types);
                db.SaveChanges();
            }
            catch (System.Exception)
            {
                status = false;
            }

            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
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
