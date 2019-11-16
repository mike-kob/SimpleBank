using BankServer.Models;
using Microsoft.EntityFrameworkCore;

namespace BankServer.Data
{
    public class BankServerContext : DbContext
    {
        public BankServerContext (DbContextOptions<BankServerContext> options)
            : base(options)
        {
        }

        public DbSet<Transaction> Transaction { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<CheckingCard> CheckingCard { get; set; }

        public DbSet<CreditCard> CreditCard { get; set; }

        public DbSet<DepositCard> DepositCard { get; set; }
        public DbSet<Atm> Atm { get; set; }

    }
}
