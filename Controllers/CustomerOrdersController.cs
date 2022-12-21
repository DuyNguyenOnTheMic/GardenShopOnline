using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GardenShopOnline.Models;

namespace GardenShopOnline.Controllers
{
    public class CustomerOrdersController : Controller
    {
        private BonsaiGardenEntities db = new BonsaiGardenEntities();

        // GET: CustomerOrders
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
        public ActionResult EditStatus_Order(CustomerOrder order)
        {
            CustomerOrder customerOrder = db.CustomerOrders.Find(order.ID);
            if (customerOrder.Status == 1)
            {
                customerOrder.Status = 2;
                Session["pills-create"] = "active";
                Session["pills-confirm"] = "";
                Session["pills-sent"] = "";

                Session["pills-create-show"] = "active show";
                Session["pills-confirm-show"] = "";
                Session["pills-sent-show"] = "";
            }
            else if (customerOrder.Status == 2)
            {
                customerOrder.Status = 3;
                Session["pills-create"] = "";
                Session["pills-confirm"] = "active";
                Session["pills-sent"] = "";

                Session["pills-create-show"] = "";
                Session["pills-confirm-show"] = "active show";
                Session["pills-sent-show"] = "";
            }
            else if (customerOrder.Status == 3)
            {
                customerOrder.Status = 4;
                Session["pills-create"] = "";
                Session["pills-confirm"] = "";
                Session["pills-sent"] = "active";

                Session["pills-create-show"] = "";
                Session["pills-confirm-show"] = "";
                Session["pills-sent-show"] = "active show";
            }


            db.Entry(customerOrder).State = EntityState.Modified;
            db.SaveChanges();
            return Json("EditStatus_Order", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult FindOrder(string order_id)
        {
            var emp = new CustomerOrder
            {
                ID = order_id
             
            };
            return Json(emp);
        }
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
