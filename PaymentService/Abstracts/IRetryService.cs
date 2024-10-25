namespace PaymentService.Abstracts
{
    public interface IRetryService
    {
        Task<TResult> ExecuteWithRetryAsync<TResult>(Func<Task<TResult>> action, TResult fallbackValue, Func<Task> onSuccessAction = null);
    }
}
