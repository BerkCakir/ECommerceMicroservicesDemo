using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Orders;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderService _orderService;
        public OrderController(IShoppingCartService shoppingCartService, IOrderService orderService)
        {
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Checkout()
        {
            var shoppingCart = await _shoppingCartService.Get();

            ViewBag.shoppingCart = shoppingCart;
            return View(new CheckOutInfoInput());
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckOutInfoInput checkoutInfoInput)
        {
            // var orderStatus = await _orderService.CreateOrder(checkoutInfoInput);

            var orderSuspend = await _orderService.SuspendOrder(checkoutInfoInput);
            if (!orderSuspend.IsSuccessful)
            {
                var shoppingCart = await _shoppingCartService.Get();

                ViewBag.shoppingCart = shoppingCart;

                ViewBag.error = orderSuspend.Error;

                return View();
            }
            // return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = orderStatus.OrderId });
            return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = new Random().Next(1, 1000) });
        }
        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
        public async Task<IActionResult> CheckoutHistory()
        {
            return View(await _orderService.GetOrder());
        }
    }
}
