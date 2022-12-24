using GardenShopOnline.Hubs;
using GardenShopOnline.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
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

        // GET: Contact
        [HttpGet]
        public ActionResult Index()
        {
            var adminUser = UserManager.FindByEmail("bonsaigarden6@gmail.com").Id;
            ViewData["ToUserId"] = adminUser;
            return View();
        }

        [HttpPost]
        public JsonResult SendMessage(string message, string toUserId)
        {
            // Send message to user
            var fromUserId = User.Identity.GetUserId();
            db.Messages.Add(new Message()
            {
                FromUserId = fromUserId,
                ToUserId = toUserId,
                Message1 = message,
                Status = 1,
                DateCreated = DateTime.Now
            });
            db.SaveChanges();
            ChatHub.Send("blabla", message);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}