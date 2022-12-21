﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GardenShopOnline.Models
{
    public partial class ShoppingCart
    {
        readonly BonsaiGardenEntities db = new BonsaiGardenEntities();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Product product, int? quantity)
        {
            // Get the matching cart and product instances
            var cartItem = db.Carts.SingleOrDefault(
                c => c.ID == ShoppingCartId
                && c.ProductID == product.ID);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    ProductID = product.ID,
                    ID = ShoppingCartId,
                    Count = quantity == null ? 1 : (int)quantity,
                    Subtotal = product.Price
                };
                db.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count = quantity == null ? cartItem.Count + 1 : cartItem.Count + (int)quantity;
                cartItem.Subtotal = product.Price * cartItem.Count;
            }
            // Save changes
            db.SaveChanges();
        }

        public int DecreaseQuantity(int id)
        {
            // Get the cart
            var cartItem = db.Carts.Single(
                cart => cart.ID == ShoppingCartId
                && cart.RecordID == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    cartItem.Subtotal = cartItem.Product.Price * cartItem.Count;
                    itemCount = (int)cartItem.Count;
                }
                else
                {
                    db.Carts.Remove(cartItem);
                }
                // Save changes
                db.SaveChanges();
            }
            return itemCount;
        }

        public int RemoveProduct(int id)
        {
            // Get the cart
            var cartItem = db.Carts.Where(
                cart => cart.ID == ShoppingCartId
                && cart.RecordID == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                itemCount = GetCount();
                db.Carts.RemoveRange(cartItem);
                // Save changes
                db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(
                cart => cart.ID == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }
            // Save changes
            db.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(
                cart => cart.ID == ShoppingCartId).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in db.Carts
                          where cartItems.ID == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public decimal GetItemTotal(int id)
        {
            // Get the matching cart and product instances
            var cartItem = db.Carts.SingleOrDefault(
                c => c.ID == ShoppingCartId
                && c.ProductID == id);

            // Get item total
            var total = decimal.Zero;
            if (cartItem != null)
            {
                total = cartItem.Subtotal;
            }
            return total;
        }

        public decimal GetTotal()
        {
            // Multiply product price by count of that product to get 
            // the current price for each of those products in the cart
            // sum all product price totals to get the cart total
            decimal? total = (from cartItems in db.Carts
                              where cartItems.ID == ShoppingCartId
                              select (int?)cartItems.Subtotal).Sum();

            return total ?? decimal.Zero;
        }

        public string CreateOrder(CustomerOrder order)
        {
            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductID = item.ProductID,
                    OrderID = order.ID,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Count
                };
                db.OrderDetails.Add(orderDetail);
            }

            // Save the order
            db.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.ID;
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = db.Carts.Where(
                c => c.ID == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.ID = userName;
            }
            db.SaveChanges();
        }
    }
}