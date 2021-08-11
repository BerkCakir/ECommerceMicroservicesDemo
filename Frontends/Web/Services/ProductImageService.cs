using Microsoft.AspNetCore.Http;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Web.Models.ProductImage;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly HttpClient _httpClient;

        public ProductImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> DeleteImage(string imageUrl)
        {
            var response = await _httpClient.DeleteAsync($"images?imageUrl={imageUrl}");
            return response.IsSuccessStatusCode;
        }

        public async Task<ProductImageViewModel> UploadImage(IFormFile image)
        {
            if (image == null || image.Length <= 0)
            {
                return null;
            }
            var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(image.FileName)}";

            using var ms = new MemoryStream();

            await image.CopyToAsync(ms);

            var multipartContent = new MultipartFormDataContent();

            multipartContent.Add(new ByteArrayContent(ms.ToArray()), "image", fileName);

            var response = await _httpClient.PostAsync("images", multipartContent);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<ProductImageViewModel>>();

            return responseSuccess.Data;
        }
    }
}
