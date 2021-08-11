using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.ShoppingCard
{
    public class ShoppingCardItemViewModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        private decimal? DiscountedPrice { get; set; }
        public decimal GetCurrentPrice
        {
            get => DiscountedPrice != null ? DiscountedPrice.Value : Price;
        }
        public void ApplyDiscount(decimal discountedPrice)
        {
            DiscountedPrice = discountedPrice;
        }
    }
}
