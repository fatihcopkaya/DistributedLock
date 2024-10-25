using PaymentService.Abstracts;
using PaymentService.Enums;
using PaymentService.Strategies;

namespace PaymentService.Factories
{
    public  class Factory(IServiceProvider serviceProvider) : IFactory
    {
        public  IPaymentTransactionStrategy CreatePaymentTransactionStrategy(TransactionTypes transactionTypes)
        {
            return transactionTypes switch
            {
                TransactionTypes.IsSale => serviceProvider.GetRequiredService<SaleStrategy>(),
                TransactionTypes.IsCancel => serviceProvider.GetRequiredService<CancelStrategy>(),
                TransactionTypes.IsRefund => serviceProvider.GetRequiredService<RefundStrategy>(),
                _ => throw new ArgumentException("Invalid transaction type", nameof(transactionTypes))

            };
        }
    }
}
