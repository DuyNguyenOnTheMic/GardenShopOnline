using GardenShopOnline.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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
            var userId = User.Identity.GetUserId();
            var user = db.AspNetUsers.Find(userId);
            ViewData["FullName"] = user.FullName;
            ViewData["Address"] = user.Address;
            ViewData["Phone"] = user.PhoneNumber;
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
                var cart = ShoppingCart.GetCart(HttpContext);
                //Check product quantity and order quantity
                string check = cart.checkOrder(order);
                if (check != "")
                {
                    //Display error when order quantity exceeds product quantity
                    ViewBag.Error = check;
                    return View(order);
                }
                else
                {
                    order.ID = "#" + DateTime.Now.ToString("yyMMddHHmmssff");
                    order.AccCustomerID = User.Identity.GetUserId();
                    order.DateCreated = DateTime.Now;
                    order.Status = 1;

                    //Save Order
                    db.CustomerOrders.Add(order);
                    db.SaveChanges();
                    //Process the order
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

        //
        // GET: /Checkout/Complete
        public ActionResult Complete(string id)
        {
            var userId = User.Identity.GetUserId();
            // Validate customer owns this order
            bool isValid = db.CustomerOrders.Any(
                o => o.ID == id &&
                o.AccCustomerID == userId);

            if (isValid)
            {
                ViewData["OrderId"] = id;
                return View();
            }
            else
            {
                return View("Error");
            }
        }
    }
}