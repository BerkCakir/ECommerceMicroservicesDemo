using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Web.Helpers;
using Web.Models;
using Web.Models.ProductCatalog;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class ProductCatalogService : IProductCatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly IProductImageService _imageService;
        private readonly ImageHelper _imageHelper;

        public ProductCatalogService(HttpClient httpClient, IProductImageService imageService, ImageHelper imageHelper)
        {
            _httpClient = httpClient;
            _imageService = imageService;
            _imageHelper = imageHelper;
        }

        public async Task<bool> AddProductAsync(ProductCreateInput productCreateInput)
        {
            var resultPhotoService = await _imageService.UploadImage(productCreateInput.ImageFormFile);

            if (resultPhotoService != null)
            {
                productCreateInput.Picture = resultPhotoService.Url;
            }

            var response = await _httpClient.PostAsJsonAsync<ProductCreateInput>("products", productCreateInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
            var response = await _httpClient.DeleteAsync($"products/{productId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<ProductCategoryViewModel>> GetAllProductCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("productcategories");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<ProductCategoryViewModel>>>();

            return responseSuccess.Data;
        }

        public async Task<List<ProductViewModel>> GetAllProductsAsync()
        {
            var response = await _httpClient.GetAsync("products");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<ProductViewModel>>>();
            responseSuccess.Data.ForEach(x =>
            {
                x.SavedPictureUrl = _imageHelper.GetProductImageUrl(x.Picture);
            });
            return responseSuccess.Data;
        }

        public async Task<List<ProductViewModel>> GetAllProductsByUserIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"products/GetAllByUserId/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<ProductViewModel>>>();

            responseSuccess.Data.ForEach(x =>
            {
                x.SavedPictureUrl = _imageHelper.GetProductImageUrl(x.Picture);
            });

            return responseSuccess.Data;
        }

        public async Task<ProductViewModel> GetByProductIdAsync(string productId)
        {
            var response = await _httpClient.GetAsync($"products/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<ProductViewModel>>();

            responseSuccess.Data.SavedPictureUrl = _imageHelper.GetProductImageUrl(responseSuccess.Data.Picture);

            return responseSuccess.Data;
        }

        public async Task<bool> UpdateProductAsync(ProductUpdateInput productUpdateInput)
        {
            var resultPhotoService = await _imageService.UploadImage(productUpdateInput.ImageFormFile);

            if (resultPhotoService != null)
            {
                await _imageService.DeleteImage(productUpdateInput.Picture);
                productUpdateInput.Picture = resultPhotoService.Url;
            }

            var response = await _httpClient.PutAsJsonAsync<ProductUpdateInput>("products", productUpdateInput);

            return response.IsSuccessStatusCode;
        }
    }
}
