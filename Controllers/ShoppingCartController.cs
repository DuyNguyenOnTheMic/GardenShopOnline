using GardenShopOnline.Models;
using System.Linq;
using System.Web.Mvc;

namespace GardenShopOnline.Controllers
{
    public class ShoppingCartController : Controller
    {
        readonly BonsaiGardenEntities db = new BonsaiGardenEntities();

        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModels
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }

        //
        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            // Retrieve the product from the database
            var addedproduct = db.Products
                .Single(product => product.ID == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(HttpContext);

            cart.AddToCart(addedproduct);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(HttpContext);

            // Get the name of the product to display confirmation
            string productName = db.Carts
                .Single(item => item.RecordID == id).Product.Name;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModels
            {
                Message = productName + " đã được xoá khỏi giỏ hàng.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }

        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}