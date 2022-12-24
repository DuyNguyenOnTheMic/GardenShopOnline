using GardenShopOnline.Hubs;
using GardenShopOnline.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace GardenShopOnline.Controllers
{
    public class ContactController : Controller
    {
        private readonly BonsaiGardenEntities db = new BonsaiGardenEntities();

        // GET: Contact
        [HttpGet]
        public ActionResult Index()
        {
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
            ChatHub.Send("blabla", message);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}