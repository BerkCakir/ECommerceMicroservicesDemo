using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Helpers
{
    public class ImageHelper
    {
        private readonly ServiceApiSettings _serviceApiSettings;

        public ImageHelper(IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public string GetProductImageUrl(string photoUrl)
        {
            return $"{_serviceApiSettings.ProductImagesUri}/images/{photoUrl}";
        }
    }
}
