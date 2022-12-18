using GardenShopOnline.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;

namespace GardenShopOnline.Controllers
{
    public class StatisticsController : Controller
    {
        readonly BonsaiGardenEntities db = new BonsaiGardenEntities();

        // GET: Statistics/Order
        public ActionResult Order()
        {
            return View();
        }

        public JsonResult GetOrderData()
        {
            return Json(db.CustomerOrders.Count(o => DbFunctions.TruncateTime(o.DateCreated) == DateTime.Today), JsonRequestBehavior.AllowGet);
        }
    }
}