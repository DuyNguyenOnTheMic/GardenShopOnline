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
    public class BankPaymentsController : Controller
    {
        private BonsaiGardenEntities db = new BonsaiGardenEntities();
        public BankPaymentsController()
        {
            ViewBag.isCreate = false;
        }
        // GET: BankPayments
        public ActionResult Index()
        {
            return View(db.BankPayments.ToList());
        }
        public ActionResult BankPaymentList()
        {
            return PartialView(db.BankPayments.Where(c => c.Status != 3).ToList().OrderByDescending(c => c.ID));

        }

        // GET: BankPayments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankPayment bankPayment = db.BankPayments.Find(id);
            if (bankPayment == null)
            {
                return HttpNotFound();
            }
            return View(bankPayment);
        }

        // GET: BankPayments/Create
        public ActionResult Create()
        {
            ViewBag.isCreate = true;
            return View("Form");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Bank,AccountNumber,AccountName,Image,Branch,Status")] BankPayment bankPayment, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                bankPayment.Status = 1;
                string filename = Path.GetFileName(Image.FileName);
                string _filename = DateTime.Now.ToString("yymmssfff") + filename;

                string extension = Path.GetExtension(Image.FileName);

                string path = Path.Combine(Server.MapPath("~/assets/images/"), _filename);

                bankPayment.Image = _filename;
                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                {
                    if (Image.ContentLength <= 4000000)
                    {
                        db.BankPayments.Add(bankPayment);

                        if (db.SaveChanges() > 0)
                        {
                            Image.SaveAs(path);

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
            ViewBag.isCreate = true;
            return View("Form", bankPayment);
        }

        // GET: BankPayments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankPayment bankPayment = db.BankPayments.Find(id);
            if (bankPayment == null)
            {
                return HttpNotFound();
            }
            return View("Form",bankPayment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Bank,AccountNumber,AccountName,Image,Branch,Status")] BankPayment bankPayment, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string _filename = DateTime.Now.ToString("yymmssfff") + filename;

                    string extension = Path.GetExtension(file.FileName);

                    string path = Path.Combine(Server.MapPath("~/assets/images/"), _filename);

                    bankPayment.Image = _filename;

                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (file.ContentLength <= 4000000)
                        {
                            db.Entry(bankPayment).State = EntityState.Modified;
                            string oldImgPath = Request.MapPath(bankPayment.Image);

                            if (db.SaveChanges() > 0)
                            {
                                file.SaveAs(path);
                                if (System.IO.File.Exists(oldImgPath))
                                {
                                    System.IO.File.Delete(oldImgPath);
                                }
                                Session["notification"] = "Cập nhật thành công!";
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
                db.Entry(bankPayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bankPayment);
        }

        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult Delete_BankPayment(BankPayment bankPayment)
        {
            bool status = true;
            try
            {
                BankPayment item = db.BankPayments.Find(bankPayment.ID);
                db.BankPayments.Remove(item);
                db.SaveChanges();
            }
            catch
            {
                status = false;
            }

            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
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
