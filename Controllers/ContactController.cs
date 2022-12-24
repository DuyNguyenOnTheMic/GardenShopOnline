using GardenShopOnline.Hubs;
using GardenShopOnline.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
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
        public ActionResult Customer(string fromUserId)
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
        public JsonResult SendMessage(string message, string fromUserId, string toUserId)
        {
            // Send message to user
            Message ms = new Message()
            {
                FromUserId = fromUserId,
                ToUserId = toUserId,
                Message1 = message,
                Status = 1,
                DateCreated = DateTime.Now
            };
            db.Messages.Add(ms);
            db.SaveChanges();
            ChatHub.Send("blabla", message);
            return Json(new { success = true, time = ms.DateCreated.ToString("HH:mm") }, JsonRequestBehavior.AllowGet);
        }
    }
}