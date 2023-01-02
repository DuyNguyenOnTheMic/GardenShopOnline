using GardenShopOnline.Hubs;
using GardenShopOnline.Models;
using Microsoft.Ajax.Utilities;
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
        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult Customer()
        {
            var currentUserId = UserManager.FindByEmail("bonsaigarden6@gmail.com").Id;
            var query_userChat = db.Messages.Where(m => m.FromUserId != currentUserId).GroupBy(m => m.FromUserId).ToList();
            return View(query_userChat);
        }

        [HttpGet]
        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult GetData(string fromUserId)
        {
            var toUserId = UserManager.FindByEmail("bonsaigarden6@gmail.com").Id;
            ViewData["fromUserId"] = fromUserId;
            ViewData["toUserId"] = toUserId;
            ViewData["userEmail"] = UserManager.FindById(fromUserId).Email;
            var query_message = db.Messages.Where(m => (m.FromUserId == fromUserId && m.ToUserId == toUserId) || (m.FromUserId == toUserId && m.ToUserId == fromUserId)).OrderByDescending(m => m.ID).ToList();
            return PartialView("_Chat", query_message);
        }

        // GET: Contact
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var fromUserId = User.Identity.GetUserId();
            var toUserId = UserManager.FindByEmail("bonsaigarden6@gmail.com").Id;
            ViewData["fromUserId"] = fromUserId;
            ViewData["toUserId"] = toUserId;
            var query_message = db.Messages.Where(m => (m.FromUserId == fromUserId && m.ToUserId == toUserId) || (m.FromUserId == toUserId && m.ToUserId == fromUserId)).OrderByDescending(m => m.ID).ToList();
            return View(query_message);
        }

        [HttpPost]
        [Authorize]
        public JsonResult SendMessage(string message, string fromUserId, string toUserId, string connectionId, HttpPostedFileBase file)
        {
            string img = "";
            // Send message to user   
            Message ms = new Message
            {
                FromUserId = fromUserId,
                ToUserId = toUserId,
                Status = 1,
                DateCreated = DateTime.Now
            };
            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);
                string _filename = DateTime.Now.ToString("yymmssfff") + filename;

                string extension = Path.GetExtension(file.FileName);

                string path = Path.Combine(Server.MapPath("~/assets/images/"), _filename);
                ms.Type = 2;
                ms.Image = _filename;
                ms.Message1 = message;
                img = _filename;
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
            string Img = ms.Image;
            var time = ms.DateCreated.ToString("HH:mm");

            db.SaveChanges();
            ChatHub.Send(time, message, connectionId, fromUserId, toUserId, Img);
            return Json(new { success = true, message = mess, img, time }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Admin, Staff")]
        public JsonResult UpdateState(string fromUserId, string toUserId)
        {
            db.Messages.Where(m => ((m.FromUserId == fromUserId && m.ToUserId == toUserId) || (m.FromUserId == toUserId && m.ToUserId == fromUserId)) && m.DateViewed == null).ForEach(m => m.DateViewed = DateTime.Now);
            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}