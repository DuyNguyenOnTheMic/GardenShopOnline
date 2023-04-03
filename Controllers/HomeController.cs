﻿using GardenShopOnline.Models;
using System.Linq;
using System.Web.Mvc;

namespace GardenShopOnline.Controllers
{
    public class HomeController : Controller
    {
        readonly BonsaiGardenEntities db = new BonsaiGardenEntities();

        [HttpGet]
        public ActionResult Index()
        {
            ShoppingCart.GetCart(HttpContext);
            return View();
        }

        [HttpGet]
        public ActionResult CategoryList()
        {
            var categories = db.Categories.Where(c => c.Status != 3).OrderByDescending(c => c.ID);
            return PartialView("_CategoryList", categories.ToList());
        }

        [HttpGet]
        public ActionResult ProductList(int? categoryId, int? typeId)
        {
            var links = from l in db.Products select l;

            if (categoryId != null)
            {
                links = links.Where(p => p.CategoryID == categoryId);
            }
            if (typeId != null)
            {
                links = links.Where(p => p.TypeID == typeId);
            }
            return PartialView("_ProductList", links.Where(c => c.Status == 1).Take(10).OrderByDescending(c => c.ID));
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