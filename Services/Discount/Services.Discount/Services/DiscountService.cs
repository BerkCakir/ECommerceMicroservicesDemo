using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<Models.Discount>> Delete(int id)
        {
            var status = await _dbConnection.ExecuteAsync("DELETE FROM discount WHERE id=@Id", new { Id = id});
            if (status <= 0)
            {
                return Response<Models.Discount>.Fail("No discount found", 500);
            }
            return Response<Models.Discount>.Success(204);
        }

        public async Task<Response<List<Models.Discount>>> GetAll()
        {
            var discounts = await _dbConnection.QueryAsync<Models.Discount>("Select * from discount");
            return Response<List<Models.Discount>>.Success(discounts.ToList(), 200);
        }

        public async Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("Select * from discount where userId = @UserId AND code = @Code", 
                new { UserId = userId, Code=code })).SingleOrDefault();
            if (discount == null)
            {
                return Response<Models.Discount>.Fail("No discount found", 404);

            }
            return Response<Models.Discount>.Success(discount, 200);
        }

        public async Task<Response<Models.Discount>> GetById(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("Select * from discount where id = @Id", new { Id = id })).SingleOrDefault();
            if (discount == null)
            {
                return Response<Models.Discount>.Fail("No discount found", 404);

            }
            return Response<Models.Discount>.Success(discount, 200);
        }

        public async Task<Response<Models.Discount>> Save(Models.Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("INSERT INTO discount(userid,rate,code) VALUES(@UserId,@Rate,@Code)", discount);
            if(status <= 0)
            {
                return Response<Models.Discount>.Fail("An error occured", 500);
            }
            return Response<Models.Discount>.Success(204);
        }

        public async Task<Response<Models.Discount>> Update(Models.Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("UPDATE discount SET userid=@UserId,code=@Code,rate=@Rate WHERE id=@Id",
                new { Id = discount.Id, UserId = discount.UserId, Code = discount.Code, Rate = discount.Rate });
            if (status <= 0)
            {
                return Response<Models.Discount>.Fail("No discount found", 404);
            }
            return Response<Models.Discount>.Success(204);
        }
    }
}
