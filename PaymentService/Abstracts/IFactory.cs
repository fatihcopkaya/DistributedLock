using PaymentService.Enums;

namespace PaymentService.Abstracts
{
    public interface IFactory
    {
         IPaymentTransactionStrategy CreatePaymentTransactionStrategy(TransactionTypes transactionTypes);
    }
}
