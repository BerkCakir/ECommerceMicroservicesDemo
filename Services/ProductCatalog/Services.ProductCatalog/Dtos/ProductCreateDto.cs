using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.ProductCatalog.Dtos
{
    public class ProductCreateDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string CreatedUserId { get; set; }
        public string Picture { get; set; }

        public ProductInventoryDto ProductInventory { get; set; }

        public string ProductCategoryId { get; set; }

    }
}
