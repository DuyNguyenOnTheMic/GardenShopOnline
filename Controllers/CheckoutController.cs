using GardenShopOnline.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GardenShopOnline.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        readonly BonsaiGardenEntities db = new BonsaiGardenEntities();

        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(CustomerOrder order)
        {
            TryUpdateModel(order);

            try
            {
                order.AccCustomerID = User.Identity.GetUserId();
                order.DateCreated = DateTime.Now;

                //Save Order
                db.CustomerOrders.Add(order);
                db.SaveChanges();
                //Process the order
                var cart = ShoppingCart.GetCart(HttpContext);
                cart.CreateOrder(order);

                return RedirectToAction("Complete",
                    new { id = order.ID });
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }

        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            var userId = User.Identity.GetUserId();
            // Validate customer owns this order
            bool isValid = db.CustomerOrders.Any(
                o => o.ID == id &&
                o.AccCustomerID == userId);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}