using AutoMapper;
using Services.ProductCatalog.Dtos;
using Services.ProductCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.ProductCatalog.Mapping
{
    public class GenericMapping:Profile //Automapper #3 generate this class from Profile to use Automapper
    {
        public GenericMapping()
        {
            CreateMap<ProductCategory, ProductCategoryDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductInventory, ProductInventoryDto>().ReverseMap();

            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<Product, ProductUpdateDto>().ReverseMap();

        }
    }
}
