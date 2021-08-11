using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class ServiceApiSettings
    {
        public string IdentityBaseUri { get; set; }
        public string GatewayBaseUri { get; set; }
        public string ProductImagesUri { get; set; }
        public ServiceApi ProductCatalog { get; set; }
        public ServiceApi ProductImage { get; set; }
        public ServiceApi ShoppingCart { get; set; }
        public ServiceApi Discount { get; set; }
        public ServiceApi Payment { get; set; }
        public ServiceApi Order { get; set; }
    }

    public class ServiceApi
    {
        public string Path { get; set; }
    }
}
