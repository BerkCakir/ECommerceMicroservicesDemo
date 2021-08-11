using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.ProductCatalog;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductCatalogService _productCatalogService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public ProductsController(IProductCatalogService productCatalogService, ISharedIdentityService sharedIdentityService)
        {
            _productCatalogService = productCatalogService;
            _sharedIdentityService = sharedIdentityService;
        }
       public async Task<IActionResult> Index()
        {
            return View(await _productCatalogService.GetAllProductsByUserIdAsync(_sharedIdentityService.GetUserId));
        }
        public async Task<IActionResult> Create()
        {
            var categories = await _productCatalogService.GetAllProductCategoriesAsync();

            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateInput productCreateInput)
        {
            var categories = await _productCatalogService.GetAllProductCategoriesAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");
            if (!ModelState.IsValid)
            {
                return View();
            }
            productCreateInput.CreatedUserId = _sharedIdentityService.GetUserId;

            await _productCatalogService.AddProductAsync(productCreateInput);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string id)
        {
            var course = await _productCatalogService.GetByProductIdAsync(id);
            var categories = await _productCatalogService.GetAllProductCategoriesAsync();

            if (course == null)
            {
                RedirectToAction(nameof(Index));
            }
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", course.Id);
            ProductUpdateInput courseUpdateInput = new()
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Price = course.Price,
                ProductInventory = course.ProductInventory,
                ProductCategoryId = course.ProductCategoryId,
                CreatedUserId = course.CreatedUserId,
                Picture = course.Picture
            };

            return View(courseUpdateInput);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateInput productUpdateInput)
        {
            var categories = await _productCatalogService.GetAllProductCategoriesAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", productUpdateInput.Id);
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _productCatalogService.UpdateProductAsync(productUpdateInput);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _productCatalogService.DeleteProductAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
