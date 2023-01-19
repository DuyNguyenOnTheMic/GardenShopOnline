using GardenShopOnline.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GardenShopOnline.Controllers
{
    public class ProductsController : Controller
    {
        private readonly BonsaiGardenEntities db = new BonsaiGardenEntities();

        // GET: Products
        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult Index()
        {
            return View();
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
        [HttpPost]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "Admin, Staff")]

        public ActionResult Create_Product(string name_product,
           int quantity, int CategoryDropdown, int TypeDropdown,
           string price, string description, HttpPostedFileBase file)
        {
            Product product = new Product();

            product.Name = name_product;
            product.Quantity = quantity;
            product.CategoryID = CategoryDropdown;
            product.TypeID = TypeDropdown;
            product.Price = int.Parse(price.Replace(",", ""));
            product.Description = description;
            product.Status = 1;
            string filename = Path.GetFileName(file.FileName);
            string _filename = DateTime.Now.ToString("yymmssfff") + filename;

            string extension = Path.GetExtension(file.FileName);

            string path = Path.Combine(Server.MapPath("~/assets/images/"), _filename);

            product.Image = _filename;
            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
            {
                if (file.ContentLength <= 4000000)
                {
                    db.Products.Add(product);

                    if (db.SaveChanges() > 0)
                    {
                        file.SaveAs(path);

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
            db.SaveChanges();
            Session["notification"] = "Thêm mới thành công!";
            return RedirectToAction("Index");
        }
        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult Delete_Product(Product product)
        {
            Product product1 = db.Products.Find(product.ID);
            product1.Status = 3;
            db.Entry(product1).State = EntityState.Modified;
            db.SaveChanges();
            return Json("Delete_Product", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [CustomAuthorize(Roles = "Admin, Staff")]
        public JsonResult FindProduct(int Product_id)
        {
            Product product = db.Products.Find(Product_id);
            var emp = new Product();
            emp.ID = Product_id;
            emp.Name = product.Name;
            emp.Quantity = product.Quantity;
            emp.Price = product.Price;
            emp.CategoryID = product.CategoryID;
            emp.Image = product.Image;
            emp.Description = product.Description;
            return Json(emp);
        }
        [HttpPost]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult UpdateProduct(int Product_id, string name_product,
           int quantity, int CategoryDropdown, int TypeDropdown,
           string price, string description, HttpPostedFileBase file)
        {
            Product product = db.Products.Find(Product_id);
            Session["imgPath"] = product.Image;
            product.Name = name_product;
            product.CategoryID = CategoryDropdown;
            product.TypeID = TypeDropdown;
            product.Price = int.Parse(price.Contains('.') ? price.Replace(".", "") : price.Replace(",", ""));
            product.Quantity = quantity;
            product.Description = description;
            product.DateUpdate = DateTime.Now;

            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);
                string _filename = DateTime.Now.ToString("yymmssfff") + filename;

                string extension = Path.GetExtension(file.FileName);

                string path = Path.Combine(Server.MapPath("~/assets/images/"), _filename);

                product.Image = _filename;

                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                {
                    if (file.ContentLength <= 4000000)
                    {
                        db.Entry(product).State = EntityState.Modified;
                        string oldImgPath = Request.MapPath(Session["imgPath"].ToString());

                        if (db.SaveChanges() > 0)
                        {
                            file.SaveAs(path);
                            if (System.IO.File.Exists(oldImgPath))
                            {
                                System.IO.File.Delete(oldImgPath);
                            }
                            return RedirectToAction("Index");
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
            else
            {
                product.Image = Session["imgPath"].ToString();
            }

            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            Session["notification"] = "Cập nhật thành công!";
            return RedirectToAction("Index");
        }

        // GET: Products1/Create
        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            return View();
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
