using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.ShoppingCart.Dtos;
using Services.ShoppingCart.Services;
using Shared.ControllerBases;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : CustomControllerBase
    {

        private readonly IShoppingCartService _shoppingCartService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public ShoppingCartsController(IShoppingCartService shoppingCartService, ISharedIdentityService sharedIdentityService)
        {
            _shoppingCartService = shoppingCartService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetShoppingCart()
        {
            return CreateResult(await _shoppingCartService.GetShoppingCart(_sharedIdentityService.GetUserId));
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateShoppingCart(ShoppingCartDto shoppingCartDto)
        {
            shoppingCartDto.UserId = _sharedIdentityService.GetUserId;
            return CreateResult(await _shoppingCartService.SaveOrUpdate(shoppingCartDto));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteShoppingCart()
        {
            return CreateResult(await _shoppingCartService.Delete(_sharedIdentityService.GetUserId));
        }
    }
}
