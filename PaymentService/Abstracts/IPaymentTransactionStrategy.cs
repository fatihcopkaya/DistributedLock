using PaymentService.Models;

namespace PaymentService.Abstracts
{
    public interface IPaymentTransactionStrategy
    {
        Task Execute(PaymentTransaction transaction);
    }
}
