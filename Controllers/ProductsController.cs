using GardenShopOnline.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constants = GardenShopOnline.Helpers.Constants;

namespace GardenShopOnline.Controllers
{
    public class ProductsController : Controller
    {
        private readonly BonsaiGardenEntities db = new BonsaiGardenEntities();
        public ProductsController()
        {
            ViewBag.isCreate = false;
        }
        // GET: Products
        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult AdminIndex()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Index(string searchKey, int? categoryId, int? typeId)
        {
            var dbProduct = db.Products;
            ViewData["Category"] = new SelectList(db.Categories.ToList(), "ID", "Name", categoryId);
            ViewData["Type"] = new SelectList(db.Types.ToList(), "ID", "Name", typeId);
            ViewData["TotalCount"] = dbProduct.Where(x => x.Status == 1).Count();
            var products = dbProduct.Where(x => x.Status == 1 && (string.IsNullOrEmpty(searchKey)
            || x.Name.ToLower().Contains(searchKey.ToLower()) || searchKey.ToLower().Contains(x.Name.ToLower()))
            && (!typeId.HasValue || x.TypeID == typeId)
            && (!categoryId.HasValue || x.CategoryID == categoryId));
            return View(products.ToList());
        }

        public ActionResult GetRelatedProducts(int productId, int typeId, int categoryId)
        {
            // Get related products list based on type and category
            return PartialView("_RelatedProducts", db.Products.Where(p => p.ID != productId && p.Status == 1 && (p.TypeID == typeId || p.CategoryID == categoryId)).ToList());
        }

        [CustomAuthorize(Roles = "Admin, Staff")]
        // sử dụng PartialView để có thể lọc sản phẩm mà không load lại toàn trang
        public ActionResult ProductList(int category_id, int type_id)
        {
            var links = from l in db.Products
                        where l.Category.Status != 3 && l.Type.Status != 3
                        select l;

            if (category_id != -1)
            {
                links = links.Where(p => p.CategoryID == category_id);
            }
            if (type_id != -1)
            {
                links = links.Where(p => p.TypeID == type_id);
            }
            return PartialView(links.Where(c => c.Status != 3).OrderByDescending(c => c.ID));

        }
        [CustomAuthorize(Roles = "Admin, Staff")]

        public ActionResult EditStatus_Product(Product product)
        {
            Product products = db.Products.Find(product.ID);
            if (products.Status == 1)
            {
                products.Status = 2;
            }
            else
            {
                products.Status = 1;
            }
            db.Entry(products).State = EntityState.Modified;
            db.SaveChanges();
            return Json("EditStatus_Product", JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult Delete_Product(Product product)
        {
            bool status = true;
            try
            {
                Product product1 = db.Products.Find(product.ID);
                var image = db.ImageProducts.Where(x => x.ProductID == product.ID);
                foreach (var item in image)
                {
                    db.ImageProducts.Remove(item);
                    string oldImgPath = Request.MapPath(item.Image);
                    if (System.IO.File.Exists(oldImgPath))
                    {
                        System.IO.File.Delete(oldImgPath);
                    }
                }
                db.Products.Remove(product1);
                db.SaveChanges();
            }
            catch
            {
                status = false;
            }

            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        // GET: Products1/Create
        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.TypeID = new SelectList(db.Types, "ID", "Name");
            ViewBag.isCreate = true;
            return View("Form");
        }

        [CustomAuthorize(Roles = "Admin, Staff")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CategoryID,TypeID,Name,Description,Image,DateCreated,DateUpdate,Status,Quantity")] Product product, HttpPostedFileBase[] Image, string priceProduct)
        {
            if (Image.Length <= 5)
            {
                if (ModelState.IsValid)
                {
                    decimal price = decimal.Parse(priceProduct.Replace(",", "").Replace(".", ""));
                    product.Status = 1;
                    product.Price = price;
                    db.Products.Add(product);
                    foreach (var item in Image)
                    {
                        string filename = Path.GetFileName(item.FileName);
                        string _filename = DateTime.Now.ToString("yymmssfff") + filename;

                        string extension = Path.GetExtension(item.FileName);

                        string path = Path.Combine(Server.MapPath("~/assets/images/"), _filename);

                        ImageProduct imageProduct = new ImageProduct();
                        imageProduct.ProductID = product.ID;
                        imageProduct.Image = _filename;
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {
                            if (item.ContentLength <= 4000000)
                            {
                                db.ImageProducts.Add(imageProduct);
                                if (db.SaveChanges() > 0)
                                {
                                    item.SaveAs(path);
                                }
                            }
                            else
                            {
                                ViewBag.msg = "Hình ảnh phải lớn hơn hoặc bằng 4MB!";
                            }
                        }
                        else
                        {
                            ViewBag.msg = "Định dạng file không hợp lệ!";
                        }
                    }
                    db.SaveChanges();
                    Session["notification"] = "Add succeeded!";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            ViewBag.TypeID = new SelectList(db.Types, "ID", "Name", product.TypeID);
            ViewBag.isCreate = true;
            return View("Form", product);
        }

        // GET: Products1/Edit/5
        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult Edit(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            ViewBag.TypeID = new SelectList(db.Types, "ID", "Name", product.TypeID);
            return View("Form", product);
        }

        [CustomAuthorize(Roles = "Admin, Staff")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CategoryID,TypeID,Name,Description,Price,Image,DateCreated,DateUpdate,Status,Quantity")] Product product, HttpPostedFileBase[] file, string priceProduct)
        {
            if (ModelState.IsValid)
            {
                decimal price = decimal.Parse(priceProduct.Replace(",", "").Replace(".", ""));
                product.Price = price;
                if (file.Length > 0 && file[0] != null)
                {
                    var image = db.ImageProducts.Where(x => x.ProductID == product.ID);
                    foreach (var item in image)
                    {
                        db.ImageProducts.Remove(item);
                        string oldImgPath = Request.MapPath(item.Image);
                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }
                    foreach (var item in file)
                    {
                        string filename = Path.GetFileName(item.FileName);
                        string _filename = DateTime.Now.ToString("yymmssfff") + filename;

                        string extension = Path.GetExtension(item.FileName);

                        string path = Path.Combine(Server.MapPath("~/assets/images/"), _filename);

                        ImageProduct imageProduct = new ImageProduct();
                        imageProduct.ProductID = product.ID;
                        imageProduct.Image = _filename;

                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {
                            if (item.ContentLength <= 4000000)
                            {
                                db.ImageProducts.Add(imageProduct);
                                item.SaveAs(path);
                            }
                            else
                            {
                                ViewBag.msg = "Hình ảnh phải lớn hơn hoặc bằng 4MB!";
                            }
                        }
                        else
                        {
                            ViewBag.msg = "Định dạng file không hợp lệ!";
                        }
                    }
                    db.Entry(product).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        Session["notification"] = "Update succeeded!";
                        return RedirectToAction("Index");
                    }
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            ViewBag.TypeID = new SelectList(db.Types, "ID", "Name", product.TypeID);
            return View(product);
        }

        // GET: Products/Details
        public ActionResult Details(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewData["CommentCount"] = db.CommentProducts.Where(c => c.ProductID == id && c.Status == 2).Count();
            return View(product);
        }

        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult Comment_product(int product_id, string content)
        {
            CommentProduct comment = new CommentProduct();
            comment.Content = content;
            comment.ProductID = product_id;
            comment.DateCreated = DateTime.Now;
            comment.UserID = User.Identity.GetUserId();
            comment.Status = 1;
            db.CommentProducts.Add(comment);
            db.SaveChanges();
            return Json("Comment_product", JsonRequestBehavior.AllowGet);
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
