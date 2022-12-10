using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GardenShopOnline.Models;

namespace GardenShopOnline.Controllers
{
    public class CategogiesController : Controller
    {
        private BonsalGardentEntities db = new BonsalGardentEntities();

        // GET: Categogies
        public ActionResult Index()
        {
            var Categogies = db.Categogies.Where(c => c.Status != 3).OrderByDescending(c => c.ID);

            return View(Categogies.ToList());
        }
        public ActionResult Create_Categogy(string name_Categogy)
        {           
            Categogy Categogy = new Categogy();
            Categogy.Name = name_Categogy;
            Categogy.Status = 1;
            db.Categogies.Add(Categogy);
            db.SaveChanges();
            Session["notification"] = "Thêm mới thành công!";
            return RedirectToAction("Index");
        }
        public ActionResult EditStatus_Category(Categogy categogys)
        {
            Categogy categories = db.Categogies.Find(categogys.ID);
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
        public JsonResult FindCategogy(int categogy_id)
        {
            Categogy categories = db.Categogies.Find(categogy_id);
            var emp = new Categogy();
            emp.ID = categogy_id;
            emp.Name = categories.Name;
            return Json(emp);
        }
        public JsonResult UpdateCategory(Categogy categorys)
        {
            Categogy categories = db.Categogies.Find(categorys.ID);
            categories.Name = categorys.Name;
            db.Entry(categories).State = EntityState.Modified;
            db.SaveChanges();
            string message = "Record Saved Successfully ";
            bool status = true;
            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete_Categogy(Categogy categorys)
        {
            Categogy categories = db.Categogies.Find(categorys.ID);
            categories.Status = 3;
            db.Entry(categories).State = EntityState.Modified;
            db.SaveChanges();
            return Json("Delete_Categogy", JsonRequestBehavior.AllowGet);
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
