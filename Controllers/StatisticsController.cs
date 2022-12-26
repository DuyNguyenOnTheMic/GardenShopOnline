using GardenShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        [HttpGet]
        public JsonResult GetOrderData(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date;
            var countList = new List<Tuple<int, int>>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var orderCount = db.CustomerOrders.Count(o => DbFunctions.TruncateTime(o.DateCreated) == date);
                var accountCount = db.AspNetUsers.Count(a => DbFunctions.TruncateTime(a.DateCreated) == date);
                countList.Add(Tuple.Create(orderCount, accountCount));
            }
            return Json(countList, JsonRequestBehavior.AllowGet);
        }
    }
}