using GardenShopOnline.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GardenShopOnline.Controllers
{
    [Authorize]
    public class CustomerOrdersController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly BonsaiGardenEntities db = new BonsaiGardenEntities();

        public CustomerOrdersController()
        {
        }

        public CustomerOrdersController(ApplicationUserManager userManager)
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

        // GET: CustomerOrders
        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult Index()
        {
            Session["pills-create"] = "active";
            Session["pills-confirm"] = "";
            Session["pills-sent"] = "";
            Session["pills-create-show"] = "active show";
            Session["pills-confirm-show"] = "";
            Session["pills-sent-show"] = "";
            var customerOrders = db.CustomerOrders.Include(c => c.AspNetUser);
            return View(customerOrders.ToList());
        }

        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult OrrderList(DateTime? date_start, DateTime? date_end)
        {

            if (date_start == null)
            {
                date_start = DateTime.Now.AddDays((-DateTime.Now.Day) + 1);
            }
            if (date_end == null)
            {
                date_end = DateTime.Now.AddMonths(1).AddDays(-(DateTime.Now.Day));
            }
            var OrrderList = db.CustomerOrders.Where(i => i.DateCreated >= date_start && i.DateCreated <= date_end);
            return PartialView(OrrderList.ToList());
        }

        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult OrrderDetailsList(string order_id)
        {
            TempData["order_id"] = order_id;
            if (order_id != "#")
            {
                CustomerOrder order = db.CustomerOrders.Find(order_id);
                TempData["order_status"] = order.Status;
                if (order.PaidAdvance != null)
                {
                    TempData["order_paidAdvance"] = order.PaidAdvance;
                }
                TempData["order_total"] = order.Total;
            }
            var OrrderDetailsList = db.OrderDetails.Where(o => o.OrderID == order_id);
            return PartialView(OrrderDetailsList.ToList());
        }

        public ActionResult GetOrderData()
        {
            var userId = User.Identity.GetUserId();
            var query_order = db.CustomerOrders.Where(o => o.AccCustomerID == userId).OrderByDescending(o => o.DateCreated).ToList();
            return PartialView("_Order", query_order);
        }

        public ActionResult GetOrderDetails(string order_id)
        {
            var order = db.CustomerOrders.Find(order_id);
            var OrrderDetailsList = db.OrderDetails.Where(o => o.OrderID == order_id).ToList();
            ViewData["Id"] = order_id;
            ViewData["FullName"] = order.FullName;
            ViewData["Address"] = order.Address;
            ViewData["Phone"] = order.Phone;
            ViewData["Total"] = OrrderDetailsList.First().CustomerOrder.Total;
            return PartialView("_OrderDetails", OrrderDetailsList);
        }

        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult EditStatus_Order(CustomerOrder order)
        {
            CustomerOrder customerOrder = db.CustomerOrders.Find(order.ID);
            var user = UserManager.FindById(customerOrder.AccCustomerID);
            if (customerOrder.Status == 1)
            {
                Session["pills-create"] = "active";
                Session["pills-confirm"] = "";
                Session["pills-sent"] = "";

                Session["pills-create-show"] = "active show";
                Session["pills-confirm-show"] = "";
                Session["pills-sent-show"] = "";
            }
            else if (customerOrder.Status == 2)
            {
                Session["pills-create"] = "";
                Session["pills-confirm"] = "active";
                Session["pills-sent"] = "";

                Session["pills-create-show"] = "";
                Session["pills-confirm-show"] = "active show";
                Session["pills-sent-show"] = "";
            }
            else if (customerOrder.Status == 3)
            {
                Session["pills-create"] = "";
                Session["pills-confirm"] = "";
                Session["pills-sent"] = "active";

                Session["pills-create-show"] = "";
                Session["pills-confirm-show"] = "";
                Session["pills-sent-show"] = "active show";
            }
            customerOrder.Status = order.Status;
            if (order.Status == 5)
            {
                customerOrder.PaidAdvance = order.PaidAdvance;
                customerOrder.Note = order.Note;
            }
            db.Entry(customerOrder).State = EntityState.Modified;
            db.SaveChanges();
            UserManager.SendEmail(user.Id, "Your order " + customerOrder.ID + "has been updated", "Your order's status is currently now " + customerOrder.Status);
            return Json("EditStatus_Order", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Admin, Staff")]
        public JsonResult FindOrder(string order_id)
        {
            var emp = new CustomerOrder
            {
                ID = order_id

            };
            return Json(emp);
        }

        [CustomAuthorize(Roles = "Admin, Staff")]
        public ActionResult DeleteOrder(CustomerOrder order)
        {
            CustomerOrder customerOrder = db.CustomerOrders.Find(order.ID);

            if (customerOrder.Status == 1)
            {

                Session["pills-create"] = "active";
                Session["pills-confirm"] = "";
                Session["pills-sent"] = "";

                Session["pills-create-show"] = "active show";
                Session["pills-confirm-show"] = "";
                Session["pills-sent-show"] = "";
            }
            else if (customerOrder.Status == 2)
            {

                Session["pills-create"] = "";
                Session["pills-confirm"] = "active";
                Session["pills-sent"] = "";

                Session["pills-create-show"] = "";
                Session["pills-confirm-show"] = "active show";
                Session["pills-sent-show"] = "";
            }
            else if (customerOrder.Status == 3)
            {

                Session["pills-create"] = "";
                Session["pills-confirm"] = "";
                Session["pills-sent"] = "active";

                Session["pills-create-show"] = "";
                Session["pills-confirm-show"] = "";
                Session["pills-sent-show"] = "active show";
            }
            customerOrder.Status = 5;
            customerOrder.Reason = order.Reason;

            db.Entry(customerOrder).State = EntityState.Modified;
            db.SaveChanges();
            return Json("DeleteOrder", JsonRequestBehavior.AllowGet);
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
