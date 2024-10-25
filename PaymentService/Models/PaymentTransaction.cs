using PaymentService.Enums;

namespace PaymentService.Models
{
    public class PaymentTransaction
    {
        public Guid Id { get; set; }
        public Guid? ReferanceTransactionId { get; set; }
        public TransactionTypes TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}
