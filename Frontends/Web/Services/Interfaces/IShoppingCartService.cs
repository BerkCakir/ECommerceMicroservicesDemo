using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.ShoppingCard;

namespace Web.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Task<bool> SaveOrUpdate(ShoppingCartViewModel shoppingCartViewModel);

        Task<ShoppingCartViewModel> Get();

        Task<bool> Delete();

        Task AddItem(ShoppingCardItemViewModel shoppingCardItemViewModel);

        Task<bool> RemoveItem(string courseId);

        Task<bool> ApplyDiscount(string discountCode);

        Task<bool> CancelApplyDiscount();
    }
}
