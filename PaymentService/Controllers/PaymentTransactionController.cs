using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Abstracts;
using PaymentService.Models;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTransactionController(ICacheService cacheService,IFactory factory) : ControllerBase
    {
        [HttpPost("PaymentProccess")]
        public async Task<IActionResult> YourControllerMethod(PaymentTransaction paymentTransaction)
        {
            var strategy = factory.CreatePaymentTransactionStrategy(paymentTransaction.TransactionType);

            await strategy.Execute(paymentTransaction);

            return Ok();
        }
    }
}
