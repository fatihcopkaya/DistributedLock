using Microsoft.Extensions.Configuration;
using PaymentService.Abstracts;
using Polly;

namespace PaymentService.Services
{
    public class RetryService(IConfiguration configuration) : IRetryService
    {
        public async Task<TResult> ExecuteWithRetryAsync<TResult>(Func<Task<TResult>> action, TResult fallbackValue, Func<Task> onSuccessAction = null)
        {
            int totalTime = 0;
            Random rnd = new Random();
            int initialDelay = rnd.Next(50, 100);

            var retryPolicy = Policy<TResult>
                    .HandleResult(result => result.Equals(false))
                    .WaitAndRetryAsync(
                    retryCount: int.Parse(configuration.GetSection("Polly")["RetryCount"]),
                    retryAttempt =>
                    {                        
                        int randomAdditionalDelay = rnd.Next(50, 100); 
                        int currentDelay = initialDelay + randomAdditionalDelay;
                        return TimeSpan.FromMilliseconds(currentDelay);
                    },
                    onRetry: (result, sleepDuration, retryCount, context) =>
                    {
                        totalTime += (int)sleepDuration.TotalMilliseconds;
                    });

            var fallbackPolicy = Policy<TResult>
                    .HandleResult(result => result.Equals(false))
                    .FallbackAsync(fallbackValue, onFallbackAsync: (result, context) =>
                    {
                        Console.WriteLine($"Fallback executed. Returning fallback value: {fallbackValue} on time {totalTime} ");
                        throw new Exception("Operation failed");
                    });

            var policyWrap = Policy.WrapAsync(fallbackPolicy, retryPolicy);

            var result = await policyWrap.ExecuteAsync(action);

            if (!result.Equals(fallbackValue))
            {
                if (onSuccessAction != null)
                {
                    await onSuccessAction();
                }
            }

            return result;
        }
    }
}
