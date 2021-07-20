using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.ProductImages.Dtos;
using Shared.ControllerBases;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.ProductImages.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : CustomControllerBase
    {
        [HttpPost ]
        public async Task<IActionResult> SaveImage(IFormFile image, CancellationToken cancellationToken)
        {
            if (image != null && image.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                await image.CopyToAsync(stream, cancellationToken);

                var returnPath = "images/" + image.FileName;

                ImageDto imageDto = new() { Url = returnPath };

                return CreateResult(Response<ImageDto>.Success(imageDto, 200));
            }
            return CreateResult(Response<ImageDto>.Fail("Empty image", 400));
        }
        public IActionResult DeleteImage(string url)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", url);
            if (!System.IO.File.Exists(path))
            {
                return CreateResult(Response<ImageDto>.Fail("Image not found", 400));

            }
            System.IO.File.Delete(path);
            return CreateResult(Response<ImageDto>.Success(null, 200));
        }
    }
}
