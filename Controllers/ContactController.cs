using GardenShopOnline.Hubs;
using GardenShopOnline.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GardenShopOnline.Controllers
{
    public class ContactController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly BonsaiGardenEntities db = new BonsaiGardenEntities();

        public ContactController()
        {
        }

        public ContactController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        public ActionResult Customer()
        {
            var currentUserId = "7d920d3d-74f1-4a72-bbce-e2301afaf321";
            var query_userChat = db.Messages.Where(m => m.FromUserId != currentUserId).GroupBy(m => m.FromUserId).ToList();
            return View(query_userChat);
        }

        [HttpGet]
        public ActionResult GetData(string fromUserId)
        {
            var toUserId = UserManager.FindByEmail("bonsaigarden6@gmail.com").Id;
            ViewData["fromUserId"] = fromUserId;
            ViewData["toUserId"] = toUserId;
            var query_message = db.Messages.Where(m => m.FromUserId == fromUserId && m.ToUserId == toUserId).OrderByDescending(m => m.ID).ToList();
            return PartialView("_Chat", query_message);
        }

        // GET: Contact
        [HttpGet]
        public ActionResult Index()
        {
            var fromUserId = User.Identity.GetUserId();
            var toUserId = UserManager.FindByEmail("bonsaigarden6@gmail.com").Id;
            ViewData["fromUserId"] = fromUserId;
            ViewData["toUserId"] = toUserId;
            var query_message = db.Messages.Where(m => m.FromUserId == fromUserId && m.ToUserId == toUserId).OrderByDescending(m => m.ID).ToList();
            return View(query_message);
        }

        [HttpPost]
        public JsonResult SendMessage(string message, string fromUserId, string toUserId, HttpPostedFileBase file)
        {
            // Send message to user   
            Message ms = new Message(); 
            ms.FromUserId = fromUserId;
            ms.ToUserId = toUserId;
            ms.Status = 1;
            ms.DateCreated = DateTime.Now;
            if (file != null)
            {                             
                string filename = Path.GetFileName(file.FileName);
                string _filename = DateTime.Now.ToString("yymmssfff") + filename;

                string extension = Path.GetExtension(file.FileName);

                string path = Path.Combine(Server.MapPath("~/assets/images/"), _filename);
                ms.Type = 2;
                ms.Message1 = _filename;
                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                {
                    if (file.ContentLength <= 4000000)
                    {
                        db.Messages.Add(ms);

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
            }
            else 
            {
                ms.Type = 1;
                ms.Message1 = message;
                db.Messages.Add(ms);
            }
            string mess = ms.Message1;
            db.SaveChanges();
            ChatHub.Send("blabla", message);
            return Json(new { success = true, message = mess, time = ms.DateCreated.ToString("HH:mm") }, JsonRequestBehavior.AllowGet);
        }
    }
}