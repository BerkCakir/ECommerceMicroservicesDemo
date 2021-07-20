using Services.ShoppingCart.Dtos;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.ShoppingCart.Services
{
    public interface IShoppingCartService
    {
        Task<Response<ShoppingCartDto>> GetShoppingCart(string userId);
        Task<Response<bool>> SaveOrUpdate(ShoppingCartDto shoppingCartDto);
        Task<Response<bool>> Delete(string userId);

    }
}
