using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Web.Models.ShoppingCard;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient _httpClient;
        private readonly IDiscountService _discountService;

        public ShoppingCartService(HttpClient httpClient, IDiscountService discountService)
        {
            _httpClient = httpClient;
            _discountService = discountService;
        }

        public async Task AddItem(ShoppingCardItemViewModel shoppingCardItemViewModel)
        {
            var shoppingCart = await Get();

            if (shoppingCart != null)
            {
                if (!shoppingCart.ShoppingCartItems.Any(x => x.ProductId == shoppingCardItemViewModel.ProductId))
                {
                    shoppingCart.ShoppingCartItems.Add(shoppingCardItemViewModel);
                }
            }
            else
            {
                shoppingCart = new ShoppingCartViewModel();

                shoppingCart.ShoppingCartItems.Add(shoppingCardItemViewModel);
            }

            await SaveOrUpdate(shoppingCart);
        }

        public async Task<bool> ApplyDiscount(string discountCode)
        {
            await CancelApplyDiscount();

            var shoppingCart = await Get();
            if (shoppingCart == null)
            {
                return false;
            }

            var hasDiscount = await _discountService.GetDiscount(discountCode);
            if (hasDiscount == null)
            {
                return false;
            }

            shoppingCart.ApplyDiscount(hasDiscount.Code, hasDiscount.Rate);
            await SaveOrUpdate(shoppingCart);
            return true;
        }

        public async Task<bool> CancelApplyDiscount()
        {
            var shoppingCart = await Get();

            if (shoppingCart == null || shoppingCart.DiscountCode == null)
            {
                return false;
            }

            shoppingCart.CancelDiscount();
            await SaveOrUpdate(shoppingCart);
            return true;
        }

        public async Task<bool> Delete()
        {
            var result = await _httpClient.DeleteAsync("shoppingcarts");

            return result.IsSuccessStatusCode;
        }

        public async Task<ShoppingCartViewModel> Get()
        {
            var response = await _httpClient.GetAsync("shoppingcarts");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var basketViewModel = await response.Content.ReadFromJsonAsync<Response<ShoppingCartViewModel>>();

            return basketViewModel.Data;
        }

        public async Task<bool> RemoveItem(string productId)
        {
            var shoppingCart = await Get();

            if (shoppingCart == null)

            {
                return false;
            }

            var deleteItem = shoppingCart.ShoppingCartItems.FirstOrDefault(x => x.ProductId == productId);

            if (deleteItem == null)
            {
                return false;
            }

            var deleteResult = shoppingCart.ShoppingCartItems.Remove(deleteItem);

            if (!deleteResult)
            {
                return false;
            }

            if (!shoppingCart.ShoppingCartItems.Any())
            {
                shoppingCart.DiscountCode = null;
            }

            return await SaveOrUpdate(shoppingCart);
        }

        public async Task<bool> SaveOrUpdate(ShoppingCartViewModel shoppingCartViewModel)
        {
            var response = await _httpClient.PostAsJsonAsync<ShoppingCartViewModel>("shoppingcarts", shoppingCartViewModel);

            return response.IsSuccessStatusCode;
        }
    }
}
