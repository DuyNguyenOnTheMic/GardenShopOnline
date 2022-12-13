using GardenShopOnline.Models;
using System;
using System.Web.Mvc;

namespace GardenShopOnline.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        readonly BonsaiGardenEntities db = new BonsaiGardenEntities();
        const string PromoCode = "FREE";

        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new CustomerOrder();
            TryUpdateModel(order);

            try
            {
                if (string.Equals(values["PromoCode"], PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;

                    //Save Order
                    db.CustomerOrders.Add(order);
                    db.SaveChanges();
                    //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);

                    return RedirectToAction("Complete",
                        new { id = order.ID });
                }
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }
    }
}