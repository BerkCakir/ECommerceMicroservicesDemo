using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Discounts;
using Web.Models.ShoppingCard;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IProductCatalogService _productCatalogService;
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartController(IProductCatalogService productCatalogService, IShoppingCartService shoppingCartService)
        {
            _productCatalogService = productCatalogService;
            _shoppingCartService = shoppingCartService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _shoppingCartService.Get());
        }

        public async Task<IActionResult> AddItem(string productId)
        {
            var product = await _productCatalogService.GetByProductIdAsync(productId);

            var item = new ShoppingCardItemViewModel { ProductId = product.Id, ProductName = product.Name, Price = product.Price, Quantity = 1 };

            await _shoppingCartService.AddItem(item);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> RemoveItem(string productId)
        {
            var result = await _shoppingCartService.RemoveItem(productId);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ApplyDiscount(DiscountApplyInput discountApplyInput)
        {
            if (!ModelState.IsValid)
            {
                TempData["discountError"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).First();
                return RedirectToAction(nameof(Index));
            }
            var discountStatus = await _shoppingCartService.ApplyDiscount(discountApplyInput.Code);

            TempData["discountStatus"] = discountStatus;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CancelApplyDiscount()
        {
            await _shoppingCartService.CancelApplyDiscount();
            return RedirectToAction(nameof(Index));
        }
    }
}
