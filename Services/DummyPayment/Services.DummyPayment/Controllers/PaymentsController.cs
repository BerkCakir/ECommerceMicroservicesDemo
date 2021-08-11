using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DummyPayment.Models;
using Shared.ControllerBases;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.DummyPayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : CustomControllerBase
    {
        [HttpPost]
        public IActionResult ReceivePayment(PaymentDto paymentDto)
        {
            return CreateResult(Response<NoContent>.Success(200));
        }
    }
}
