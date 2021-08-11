using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.ProductCatalog
{
    public class ProductViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string CreatedUserId { get; set; }

        public string Picture { get; set; }

        public ProductInventoryViewModel ProductInventory { get; set; }

        public string ProductCategoryId { get; set; }
        public ProductCategoryViewModel ProductCategory { get; set; }


        public string SavedPictureUrl { get; set; }

        public string ShortDescription
        {
            get => Description.Length > 100 ? Description.Substring(0, 100) + "..." : Description;
        }
    }
}
