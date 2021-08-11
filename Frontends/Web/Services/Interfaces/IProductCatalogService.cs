using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.ProductCatalog;

namespace Web.Services.Interfaces
{
    public interface IProductCatalogService
    {
        Task<List<ProductViewModel>> GetAllProductsAsync();
        Task<List<ProductCategoryViewModel>> GetAllProductCategoriesAsync();
        Task<List<ProductViewModel>> GetAllProductsByUserIdAsync(string userId);
        Task <ProductViewModel> GetByProductIdAsync(string productId);
        Task<bool> AddProductAsync(ProductCreateInput productCreateInput);
        Task<bool> UpdateProductAsync(ProductUpdateInput productUpdateInput);
        Task<bool> DeleteProductAsync(string productId);


    }
}
