using AutoMapper;
using MassTransit;
using MongoDB.Driver;
using Services.ProductCatalog.Dtos;
using Services.ProductCatalog.Models;
using Services.ProductCatalog.Settings;
using Shared.Dtos;
using Shared.Messages;
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
        private readonly IPublishEndpoint _publishEndpoint;
        public ProductService(IMapper mapper, IDatabaseSettings databaseSettings, IPublishEndpoint publishEndpoint)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var dataBase = client.GetDatabase(databaseSettings.DataBaseName);

            _productCollection = dataBase.GetCollection<Product>(databaseSettings.ProductCollectionName);
            _productCategoryCollection = dataBase.GetCollection<ProductCategory>(databaseSettings.ProductCategoryCollectionName);
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<Shared.Dtos.Response<List<ProductDto>>> GetAllAsync()
        {
            var products = await _productCollection.Find(p => true).ToListAsync();
            if (products.Any())
            {
                foreach (var product in products)
                {
                    product.ProductCategory = await _productCategoryCollection.Find<ProductCategory>(c => c.Id == product.ProductCategoryId).FirstAsync();
                }
                return Shared.Dtos.Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(products), 200);
            }

            return Shared.Dtos.Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(new List<Product>()), 200);
        }

        public async Task<Shared.Dtos.Response<ProductDto>> GetByIdAsync(string id)
        {
            var product = await _productCollection.Find<Product>(p => p.Id == id).FirstOrDefaultAsync();

            if (product != null)
            {
                product.ProductCategory = await _productCategoryCollection.Find<ProductCategory>(c => c.Id == product.ProductCategoryId).FirstAsync();
                return Shared.Dtos.Response<ProductDto>.Success(_mapper.Map<ProductDto>(product), 200);
            }

            return Shared.Dtos.Response<ProductDto>.Fail("Product Not Found", 404);
        }

        public async Task<Shared.Dtos.Response<List<ProductDto>>> GetAllByUserId(string createdUserId)
        {
            var products = await _productCollection.Find<Product>(p => p.CreatedUserId == createdUserId).ToListAsync();

            if (products.Any())
            {
                foreach (var product in products)
                {
                    product.ProductCategory = await _productCategoryCollection.Find<ProductCategory>(c => c.Id == product.ProductCategoryId).FirstAsync();
                }
                return Shared.Dtos.Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(products), 200);
            }

            return Shared.Dtos.Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(new List<Product>()), 200);
        }

        public async Task<Shared.Dtos.Response<ProductDto>> CreateAsync(ProductCreateDto productCreateDto)
        {
            var newProduct = _mapper.Map<Product>(productCreateDto);
            newProduct.CreatedDate = DateTime.Now;
            await _productCollection.InsertOneAsync(newProduct);

            return Shared.Dtos.Response<ProductDto>.Success(_mapper.Map<ProductDto>(newProduct), 200);

        }
        public async Task<Shared.Dtos.Response<ProductDto>> UpdateAsync(ProductUpdateDto productUpdateDto)
        {
            var updateProduct = _mapper.Map<Product>(productUpdateDto);
            var result = await _productCollection.FindOneAndReplaceAsync(p => p.Id == updateProduct.Id, updateProduct);

            if(result != null)
            {
                await _publishEndpoint.Publish<ProductNameChangedEvent>(new ProductNameChangedEvent
                { ProductId = productUpdateDto.Id, UpdatedName = productUpdateDto.Name });
                return Shared.Dtos.Response<ProductDto>.Success(null, 204);
            }

            return Shared.Dtos.Response<ProductDto>.Fail("Product Not Found", 404);

        }
        public async Task<Shared.Dtos.Response<ProductDto>> DeleteAsync(string id)
        {
            var result = await _productCollection.DeleteOneAsync(p => p.Id == id);

            if (result.DeletedCount > 0)
            {
                return Shared.Dtos.Response<ProductDto>.Success(null, 204);
            }

            return Shared.Dtos.Response<ProductDto>.Fail("Product Not Found", 404);

        }

    }
}
