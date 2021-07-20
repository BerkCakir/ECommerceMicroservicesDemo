using AutoMapper;
using MongoDB.Driver;
using Services.ProductCatalog.Dtos;
using Services.ProductCatalog.Models;
using Services.ProductCatalog.Settings;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.ProductCatalog.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IMongoCollection<ProductCategory> _productCategoryCollection;
        private readonly IMapper _mapper;

        public ProductCategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            //MongoDB #2 usage
            var client = new MongoClient(databaseSettings.ConnectionString);
            var dataBase = client.GetDatabase(databaseSettings.DataBaseName);

            _productCategoryCollection = dataBase.GetCollection<ProductCategory>(databaseSettings.ProductCategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<List<ProductCategoryDto>>> GetAllAsync()
        {
            var productCategories = await _productCategoryCollection.Find(c => true).ToListAsync();
            return Response<List<ProductCategoryDto>>.Success(_mapper.Map<List<ProductCategoryDto>>(productCategories), 200); //AutoMapper #4 usage
        }
        public async Task<Response<ProductCategoryDto>> GetByIdAsync(string id)
        {
            var productCategory = await _productCategoryCollection.Find<ProductCategory>(x => x.Id == id).FirstOrDefaultAsync();

            if(productCategory != null)
            {
                return Response<ProductCategoryDto>.Success(_mapper.Map<ProductCategoryDto>(productCategory), 200);
            }

            return Response<ProductCategoryDto>.Fail("Product Category Not Found", 404);
        }
        public async Task<Response<ProductCategoryDto>> CreateAsync(ProductCategoryDto productCategoryDto)
        {
            var productCategory = _mapper.Map<ProductCategory>(productCategoryDto);
            await _productCategoryCollection.InsertOneAsync(productCategory);
            return Response<ProductCategoryDto>.Success(_mapper.Map<ProductCategoryDto>(productCategory), 200);
        }
    }
}
