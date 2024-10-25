using PaymentService.Abstracts;
using PaymentService.Context;
using PaymentService.Models;

namespace PaymentService.Strategies
{
    public class CancelStrategy(ICacheService cacheService, IRetryService retryService,PaymentServiceDbContext dbContext) : IPaymentTransactionStrategy
    {
        public async Task Execute(PaymentTransaction transaction)
        {
         
            try
            {
                var IsComplete = await retryService.ExecuteWithRetryAsync(async () =>
                {
                    var result = await cacheService.GetAsync<string>(transaction.ReferanceTransactionId.ToString());
                    return string.IsNullOrEmpty(result);
                },
                fallbackValue: false, onSuccessAction: async () =>
                {
                    await cacheService.SetAsync(transaction.ReferanceTransactionId.ToString(), "RefundOperationIsInProcess", config =>
                    {
                        config.AbsoluteExpiration = 2;
                        config.SlidingExpiration = 1;
                    });
                });
                await dbContext.AddAsync(transaction);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await cacheService.RemoveAsync(transaction.ReferanceTransactionId.ToString());
            }




        }
    }
}
