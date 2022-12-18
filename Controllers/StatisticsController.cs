using GardenShopOnline.Models;
using System;
using System.Collections.Generic;
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
            var lastWeekDay = DateTime.Today.AddDays(-6);
            var countList = new List<int>();
            for (DateTime date = lastWeekDay; date <= DateTime.Today; date = date.AddDays(1))
            {
                countList.Add(db.CustomerOrders.Count(o => DbFunctions.TruncateTime(o.DateCreated) == date));
            }
            return Json(countList, JsonRequestBehavior.AllowGet);
        }
    }
}