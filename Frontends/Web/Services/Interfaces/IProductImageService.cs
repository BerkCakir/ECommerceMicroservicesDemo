using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.ProductImage;

namespace Web.Services.Interfaces
{
    public interface IProductImageService
    {
        Task<ProductImageViewModel> UploadImage(IFormFile image);

        Task<bool> DeleteImage(string imageUrl);
    }
}
