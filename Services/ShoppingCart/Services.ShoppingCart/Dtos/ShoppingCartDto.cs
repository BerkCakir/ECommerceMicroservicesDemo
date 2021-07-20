using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.ShoppingCart.Dtos
{
    public class ShoppingCartDto
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public List<ShoppingCartItemDto> shoppingCartItems { get; set; }
        public decimal TotalPrice { get => shoppingCartItems.Sum(x => x.Price * x.Quantity); }
    }
}
