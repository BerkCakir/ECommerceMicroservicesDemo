using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.ShoppingCard
{
    public class ShoppingCartViewModel
    {
        public ShoppingCartViewModel()
        {
            shoppingCartItems = new List<ShoppingCardItemViewModel>();
        }

        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public int? DiscountRate { get; set; }

        private List<ShoppingCardItemViewModel> shoppingCartItems;
        public decimal TotalPrice { get => shoppingCartItems.Sum(x => x.GetCurrentPrice * x.Quantity); }
        public bool HasDiscount
        {
            get => !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue;
        }

        public void CancelDiscount()
        {
            DiscountCode = null;
            DiscountRate = null;
        }

        public void ApplyDiscount(string code, int rate)
        {
            DiscountCode = code;
            DiscountRate = rate;
        }

        public List<ShoppingCardItemViewModel> ShoppingCartItems
        {
            get
            {
                if (HasDiscount)
                {
                    //Örnek kurs fiyat 100 TL indirim %10
                    shoppingCartItems.ForEach(x =>
                    {
                        var discountPrice = x.Price * ((decimal)DiscountRate.Value / 100);
                        x.ApplyDiscount(Math.Round(x.Price - discountPrice, 2));
                    });
                }
                return shoppingCartItems;
            }
            set
            {
                shoppingCartItems = value;
            }
        }
    }
}
