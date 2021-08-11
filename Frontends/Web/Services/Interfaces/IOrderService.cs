using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Orders;

namespace Web.Services.Interfaces
{
    public interface IOrderService
    {    
        Task<OrderCreatedViewModel> CreateOrder(CheckOutInfoInput checkoutInfoInput);

        Task SuspendOrder(CheckOutInfoInput checkoutInfoInput);

        Task<List<OrderViewModel>> GetOrder();
    }
}
