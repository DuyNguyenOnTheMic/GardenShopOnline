using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GardenShopOnline.Models;
using Microsoft.AspNet.Identity;

namespace GardenShopOnline.Controllers
{
    public class CommentProductsController : Controller
    {
        private BonsaiGardenEntities db = new BonsaiGardenEntities();

        // GET: CommentProducts
        public ActionResult Index()
        {
            
            var commentProducts = db.CommentProducts.Include(c => c.Product);
            return View(commentProducts.ToList());
        }
        public ActionResult CommentProduct(int product_id)
        {
            var commentProducts = db.CommentProducts.Include(c => c.Product).Where(c => c.ProductID == product_id &&c.Status == 2);
            return PartialView(commentProducts.ToList());
        }
        public ActionResult CommentProductList()
        {
            var commentProducts = db.CommentProducts.Include(c => c.Product);
            return PartialView(commentProducts.ToList());
        }
        public ActionResult EditStatus_Comment(CommentProduct cmt)
        {
            CommentProduct comment = db.CommentProducts.Find(cmt.ID);
            if (cmt.Status == 2)
            {
                comment.Status = 2;

            }
            else if (cmt.Status == 3)
            {
                comment.Status = 3;

            }
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();
            return Json("EditStatus_Order", JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReplyComment(CommentProduct cmt)
        {
                CommentProduct comment = new CommentProduct();
                comment.Content = cmt.Content;
                comment.ProductID = cmt.ProductID;
                comment.DateCreated = DateTime.Now;
                comment.UserID = User.Identity.GetUserId();
                comment.Reply_coment = cmt.Reply_coment;
                comment.Status = 2;
                db.CommentProducts.Add(comment);
            db.SaveChanges();
            CommentProduct commentProduct = db.CommentProducts.Find(cmt.Reply_coment);
            commentProduct.Status = 2;
            db.Entry(commentProduct).State = EntityState.Modified;
            db.SaveChanges();
                return Json("ReplyComment", JsonRequestBehavior.AllowGet);         
        }
        // GET: CommentProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentProduct commentProduct = db.CommentProducts.Find(id);
            if (commentProduct == null)
            {
                return HttpNotFound();
            }
            return View(commentProduct);
        }

        // GET: CommentProducts/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            return View();
        }

        // POST: CommentProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Content,AccManagerID,Answer,ProductID,AccCustomerID,DateCreated,Approval")] CommentProduct commentProduct)
        {
            if (ModelState.IsValid)
            {
                db.CommentProducts.Add(commentProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", commentProduct.ProductID);
            return View(commentProduct);
        }

        // GET: CommentProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentProduct commentProduct = db.CommentProducts.Find(id);
            if (commentProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", commentProduct.ProductID);
            return View(commentProduct);
        }

        // POST: CommentProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Content,AccManagerID,Answer,ProductID,AccCustomerID,DateCreated,Approval")] CommentProduct commentProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commentProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", commentProduct.ProductID);
            return View(commentProduct);
        }

        // GET: CommentProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentProduct commentProduct = db.CommentProducts.Find(id);
            if (commentProduct == null)
            {
                return HttpNotFound();
            }
            return View(commentProduct);
        }

        // POST: CommentProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CommentProduct commentProduct = db.CommentProducts.Find(id);
            db.CommentProducts.Remove(commentProduct);
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
