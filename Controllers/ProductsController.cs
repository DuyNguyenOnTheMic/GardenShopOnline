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
    public class ProductsController : Controller
    {
        private BonsaiGardenEntities db = new BonsaiGardenEntities();

        // GET: Products
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProductList( int category_id)
        {
            var links = from l in db.Products
                        where  l.Category.Status != 3
                        select l;
           
            if (category_id != -1)
            {
                links = links.Where(p => p.CategoryID == category_id);
            }
            return PartialView(links.Where(c => c.Status != 3).OrderByDescending(c => c.ID));

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
