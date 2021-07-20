using Services.ProductCatalog.Dtos;
using Services.ProductCatalog.Models;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.ProductCatalog.Services
{
    public interface IProductCategoryService
    {
         Task<Response<List<ProductCategoryDto>>> GetAllAsync();

        Task<Response<ProductCategoryDto>> GetByIdAsync(string id);

        Task<Response<ProductCategoryDto>> CreateAsync(ProductCategoryDto productCategoryDto);

    }
}
