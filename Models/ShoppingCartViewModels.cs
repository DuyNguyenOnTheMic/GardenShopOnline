using GardenShopOnline.Models;
using System.Collections.Generic;

namespace GardenShopOnline.ViewModels
{
    public class ShoppingCartViewModels
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}