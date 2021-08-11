using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.ProductCatalog
{
    public class ProductCreateInput
    {
        [Display(Name="Product Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        public string CreatedUserId { get; set; }
        public string Picture { get; set; }

        public ProductInventoryViewModel ProductInventory { get; set; }

        [Display(Name = "Product Category")]
        public string ProductCategoryId { get; set; }

        [Display(Name = "Product Image")]
        public IFormFile ImageFormFile{ get; set; }
    }
}
