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

        [HttpGet]
        public JsonResult GetOrderData(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date;
            var countList = new List<int>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                countList.Add(db.CustomerOrders.Count(o => DbFunctions.TruncateTime(o.DateCreated) == date));
            }
            return Json(countList, JsonRequestBehavior.AllowGet);
        }
    }
}