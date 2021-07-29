using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Discount.Services
{
    public interface IDiscountService
    {
        Task<Response<List<Models.Discount>>> GetAll();

        Task<Response<Models.Discount>> GetById(int id);

        Task<Response<Models.Discount>> Save(Models.Discount discount);

        Task<Response<Models.Discount>> Update(Models.Discount discount);

        Task<Response<Models.Discount>> Delete(int id);

        Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId);
    }
}
