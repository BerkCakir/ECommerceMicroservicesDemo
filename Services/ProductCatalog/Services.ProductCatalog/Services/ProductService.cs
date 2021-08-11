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
    public class ProductService : IProductService
    {

        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMongoCollection<ProductCategory> _productCategoryCollection;
        private readonly IMapper _mapper;

        public ProductService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var dataBase = client.GetDatabase(databaseSettings.DataBaseName);

            _productCollection = dataBase.GetCollection<Product>(databaseSettings.ProductCollectionName);
            _productCategoryCollection = dataBase.GetCollection<ProductCategory>(databaseSettings.ProductCategoryCollectionName);
            _mapper = mapper;
        }
        public async Task<Response<List<ProductDto>>> GetAllAsync()
        {
            var products = await _productCollection.Find(p => true).ToListAsync();
            if (products.Any())
            {
                foreach (var product in products)
                {
                    product.ProductCategory = await _productCategoryCollection.Find<ProductCategory>(c => c.Id == product.ProductCategoryId).FirstAsync();
                }
                return Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(products), 200);
            }

            return Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(new List<Product>()), 200);
        }

        public async Task<Response<ProductDto>> GetByIdAsync(string id)
        {
            var product = await _productCollection.Find<Product>(p => p.Id == id).FirstOrDefaultAsync();

            if (product != null)
            {
                product.ProductCategory = await _productCategoryCollection.Find<ProductCategory>(c => c.Id == product.ProductCategoryId).FirstAsync();
                return Response<ProductDto>.Success(_mapper.Map<ProductDto>(product), 200);
            }

            return Response<ProductDto>.Fail("Product Not Found", 404);
        }

        public async Task<Response<List<ProductDto>>> GetAllByUserId(string createdUserId)
        {
            var products = await _productCollection.Find<Product>(p => p.CreatedUserId == createdUserId).ToListAsync();

            if (products.Any())
            {
                foreach (var product in products)
                {
                    product.ProductCategory = await _productCategoryCollection.Find<ProductCategory>(c => c.Id == product.ProductCategoryId).FirstAsync();
                }
                return Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(products), 200);
            }

            return Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(new List<Product>()), 200);
        }

        public async Task<Response<ProductDto>> CreateAsync(ProductCreateDto productCreateDto)
        {
            var newProduct = _mapper.Map<Product>(productCreateDto);
            newProduct.CreatedDate = DateTime.Now;
            await _productCollection.InsertOneAsync(newProduct);

            return Response<ProductDto>.Success(_mapper.Map<ProductDto>(newProduct), 200);

        }
        public async Task<Response<ProductDto>> UpdateAsync(ProductUpdateDto productUpdateDto)
        {
            var updateProduct = _mapper.Map<Product>(productUpdateDto);
            var result = await _productCollection.FindOneAndReplaceAsync(p => p.Id == updateProduct.Id, updateProduct);

            if(result != null)
            {
                return Response<ProductDto>.Success(null, 204);
            }


            return Response<ProductDto>.Fail("Product Not Found", 404);

        }
        public async Task<Response<ProductDto>> DeleteAsync(string id)
        {
            var result = await _productCollection.DeleteOneAsync(p => p.Id == id);

            if (result.DeletedCount > 0)
            {
                return Response<ProductDto>.Success(null, 204);
            }

            return Response<ProductDto>.Fail("Product Not Found", 404);

        }

    }
}
