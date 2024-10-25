using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService.Context
{
    public class PaymentServiceDbContext : DbContext
    {
        public PaymentServiceDbContext(DbContextOptions options) : base(options)
        {
        }
        DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    }
}
