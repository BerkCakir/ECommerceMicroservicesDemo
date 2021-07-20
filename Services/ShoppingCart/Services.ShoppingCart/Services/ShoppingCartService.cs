using Services.ShoppingCart.Dtos;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.ShoppingCart.Services
{
    public class ShoppingCartService: IShoppingCartService
    {
        private readonly RedisService _redisService;

        public ShoppingCartService(RedisService redisService)
        {
            this._redisService = redisService;
        }

        public async Task<Response<bool>> Delete(string userId)
        {
            var status = await _redisService.GetDb().KeyDeleteAsync(userId);
            if (status)
            {
                return Response<bool>.Success(204);
            }

            return Response<bool>.Fail("Shopping cart not found", 404);

        }

        public async Task<Response<ShoppingCartDto>> GetShoppingCart(string userId)
        {
            var item = await _redisService.GetDb().StringGetAsync(userId);
            if (String.IsNullOrEmpty(item))
            {
                return Response<ShoppingCartDto>.Fail("Shopping cart not found", 404);
            }
            return Response<ShoppingCartDto>.Success(JsonSerializer.Deserialize<ShoppingCartDto>(item), 200);
        }

        public async Task<Response<bool>> SaveOrUpdate(ShoppingCartDto shoppingCartDto)
        {
            var status = await _redisService.GetDb().StringSetAsync(shoppingCartDto.UserId, JsonSerializer.Serialize(shoppingCartDto));

            if(status)
            {
                return Response<bool>.Success(204);
            }

            return Response<bool>.Fail("Shopping cart cannot be modified",500);

        }
    }
}
