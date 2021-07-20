using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.ProductCatalog.Dtos;
using Services.ProductCatalog.Services;
using Shared.ControllerBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.ProductCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : CustomControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoriesController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _productCategoryService.GetAllAsync();

            return CreateResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _productCategoryService.GetByIdAsync(id);

            return CreateResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCategoryDto productCategoryDto)
        {
            var response = await _productCategoryService.CreateAsync(productCategoryDto);

            return CreateResult(response);
        }
    }
}
