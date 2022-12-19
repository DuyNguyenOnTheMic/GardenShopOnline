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
    public class CustomerOrdersController : Controller
    {
        private BonsaiGardenEntities db = new BonsaiGardenEntities();

        // GET: CustomerOrders
        public ActionResult Index()
        {
            var customerOrders = db.CustomerOrders.Include(c => c.AspNetUser);
            return View(customerOrders.ToList());
        }
        public ActionResult OrrderList(DateTime? date_start, DateTime? date_end)
        {
            if (date_start == null)
            {
                date_start = DateTime.Now.AddDays((-DateTime.Now.Day) + 1);
            }
            if (date_end == null)
            {
                date_end = DateTime.Now.AddMonths(1).AddDays(-(DateTime.Now.Day));
            }
            var OrrderList = db.CustomerOrders.Where(i => i.DateCreated >= date_start && i.DateCreated <= date_end);
            return PartialView(OrrderList.ToList());
        }
        public ActionResult EditStatus_Order(CustomerOrder order)
        {
            CustomerOrder customerOrder = db.CustomerOrders.Find(order.ID);
            if (customerOrder.Status == 1)
            {
                customerOrder.Status = 2;
            }
            else if (customerOrder.Status == 2)
            {
                customerOrder.Status = 3;
            }
            else if (customerOrder.Status == 3)
            {
                customerOrder.Status = 4;
            }

            db.Entry(customerOrder).State = EntityState.Modified;
            db.SaveChanges();
            return Json("EditStatus_Order", JsonRequestBehavior.AllowGet);
        }
        // GET: CustomerOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrder customerOrder = db.CustomerOrders.Find(id);
            if (customerOrder == null)
            {
                return HttpNotFound();
            }
            return View(customerOrder);
        }

        // GET: CustomerOrders/Create
        public ActionResult Create()
        {
            ViewBag.AccCustomerID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: CustomerOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AccCustomerID,DateCreated,DateUpdated,DateDeleted,FullName,Address,Phone,Total")] CustomerOrder customerOrder)
        {
            if (ModelState.IsValid)
            {
                db.CustomerOrders.Add(customerOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccCustomerID = new SelectList(db.AspNetUsers, "Id", "Email", customerOrder.AccCustomerID);
            return View(customerOrder);
        }

        // GET: CustomerOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrder customerOrder = db.CustomerOrders.Find(id);
            if (customerOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccCustomerID = new SelectList(db.AspNetUsers, "Id", "Email", customerOrder.AccCustomerID);
            return View(customerOrder);
        }

        // POST: CustomerOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AccCustomerID,DateCreated,DateUpdated,DateDeleted,FullName,Address,Phone,Total")] CustomerOrder customerOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccCustomerID = new SelectList(db.AspNetUsers, "Id", "Email", customerOrder.AccCustomerID);
            return View(customerOrder);
        }

        // GET: CustomerOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrder customerOrder = db.CustomerOrders.Find(id);
            if (customerOrder == null)
            {
                return HttpNotFound();
            }
            return View(customerOrder);
        }

        // POST: CustomerOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerOrder customerOrder = db.CustomerOrders.Find(id);
            db.CustomerOrders.Remove(customerOrder);
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
