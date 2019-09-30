using Microsoft.EntityFrameworkCore;

namespace Bank_server.Models
{
    public class BankServerContext : DbContext
    {
        public BankServerContext (DbContextOptions<BankServerContext> options)
            : base(options)
        {
        }

        public DbSet<Bank_server.Models.User> User { get; set; }

        public DbSet<Bank_server.Models.CheckingCard> CheckingCard { get; set; }

        public DbSet<Bank_server.Models.CreditCard> CreditCard { get; set; }

        public DbSet<Bank_server.Models.DepositCard> DepositCard { get; set; }
    }
}
