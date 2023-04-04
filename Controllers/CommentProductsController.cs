using GardenShopOnline.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Constants = GardenShopOnline.Helpers.Constants;


namespace GardenShopOnline.Controllers
{
    public class CommentProductsController : Controller
    {
        private readonly BonsaiGardenEntities db = new BonsaiGardenEntities();

        // GET: CommentProducts
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Index()
        {

            var commentProducts = db.CommentProducts.Include(c => c.Product);
            return View(commentProducts.ToList());
        }

        public ActionResult CommentProduct(int product_id)
        {
            var commentProducts = db.CommentProducts.Include(c => c.Product).Where(c => c.ProductID == product_id && c.Status == Constants.APPROVED_COMMENT);
            return PartialView(commentProducts.ToList());
        }

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult CommentProductList()
        {
            var commentProducts = db.CommentProducts.Include(c => c.Product);
            return PartialView(commentProducts.ToList());
        }

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult EditStatus_Comment(CommentProduct cmt)
        {
            CommentProduct comment = db.CommentProducts.Find(cmt.ID);
            comment.Status = cmt.Status;
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();
            return Json("EditStatus_Order", JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult ReplyComment(CommentProduct cmt)
        {
            CommentProduct comment = new CommentProduct
            {
                Content = cmt.Content,
                ProductID = cmt.ProductID,
                DateCreated = DateTime.Now,
                UserID = User.Identity.GetUserId(),
                Reply_coment = cmt.Reply_coment,
                Status = Constants.APPROVED_COMMENT
            };
            db.CommentProducts.Add(comment);
            db.SaveChanges();
            CommentProduct commentProduct = db.CommentProducts.Find(cmt.Reply_coment);
            commentProduct.Status = Constants.APPROVED_COMMENT;
            db.Entry(commentProduct).State = EntityState.Modified;
            db.SaveChanges();
            return Json("ReplyComment", JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin, Staff")]
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
