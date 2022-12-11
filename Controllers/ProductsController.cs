using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GardenShopOnline.Models;

namespace GardenShopOnline.Controllers
{
    public class ProductsController : Controller
    {
        private BonsaiGardenEntities db = new BonsaiGardenEntities();

        // GET: Products
        public ActionResult Index()
        {
            return View();
        }

        // sử dụng PartialView để có thể lọc sản phẩm mà không load lại toàn trang
        public ActionResult ProductList( int category_id)
        {
            var links = from l in db.Products
                        where  l.Category.Status != 3
                        select l;
           
            if (category_id != -1)
            {
                links = links.Where(p => p.CategoryID == category_id);
            }
            return PartialView(links.Where(c => c.Status != 3).OrderByDescending(c => c.ID));

        }
        [HttpPost]
        public ActionResult Create_Product(string name_product, 
           int quantity,  int CategoryDropdown,
           string price, string description, HttpPostedFileBase file)
        {
            Product product = new Product();

            product.Name = name_product;
            product.Quantity = quantity;
            product.CategoryID = CategoryDropdown;
            product.Price = int.Parse(price.Replace(",", ""));
            product.Description = description;
            product.Status = 1;
            string filename = Path.GetFileName(file.FileName);
            string _filename = DateTime.Now.ToString("yymmssfff") + filename;

            string extension = Path.GetExtension(file.FileName);

            string path = Path.Combine(Server.MapPath("~/assets/images/"), _filename);

            product.Image = "~/assets/images/" + _filename;
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
