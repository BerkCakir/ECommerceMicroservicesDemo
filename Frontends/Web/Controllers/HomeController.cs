using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web.Exceptions;
using Web.Models;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductCatalogService _productCatalogService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IProductCatalogService productCatalogService, ILogger<HomeController> logger)
        {
            _productCatalogService = productCatalogService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productCatalogService.GetAllProductsAsync());
        }
        public async Task<IActionResult> Detail(string id)
        {
            return View(await _productCatalogService.GetByProductIdAsync(id));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (errorFeature != null && errorFeature.Error is UnAuthorizeException)
            {
                return RedirectToAction(nameof(AuthController.Logout), "Auth");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
