using PaymentService.Abstracts;
using PaymentService.Context;
using PaymentService.Models;

namespace PaymentService.Strategies
{
    public class SaleStrategy(PaymentServiceDbContext dbContext) : IPaymentTransactionStrategy
    {
        public async Task Execute(PaymentTransaction transaction)
        {
            await dbContext.AddAsync(transaction);
            await dbContext.SaveChangesAsync();
        }
    }
}
