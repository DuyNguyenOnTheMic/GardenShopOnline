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
            return View(db.Categogies.ToList());
        }
        public ActionResult Create_Categogy(string name_Categogy)
        {           
            Categogy Categogy = new Categogy();
            Categogy.Name = name_Categogy;
           
            db.Categogies.Add(Categogy);
            db.SaveChanges();
            Session["notification"] = "Thêm mới thành công!";
            return RedirectToAction("Index");
        }

        // GET: Categogies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categogy categogy = db.Categogies.Find(id);
            if (categogy == null)
            {
                return HttpNotFound();
            }
            return View(categogy);
        }

        // GET: Categogies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categogies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] Categogy categogy)
        {
            if (ModelState.IsValid)
            {
                db.Categogies.Add(categogy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categogy);
        }

        // GET: Categogies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categogy categogy = db.Categogies.Find(id);
            if (categogy == null)
            {
                return HttpNotFound();
            }
            return View(categogy);
        }

        // POST: Categogies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] Categogy categogy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categogy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categogy);
        }

        // GET: Categogies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categogy categogy = db.Categogies.Find(id);
            if (categogy == null)
            {
                return HttpNotFound();
            }
            return View(categogy);
        }

        // POST: Categogies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categogy categogy = db.Categogies.Find(id);
            db.Categogies.Remove(categogy);
            db.SaveChanges();
            return RedirectToAction("Index");
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
