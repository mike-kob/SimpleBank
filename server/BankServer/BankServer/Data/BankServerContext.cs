using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bank_server.Models;

namespace BankServer.Models
{
    public class BankServerContext : DbContext
    {
        public BankServerContext (DbContextOptions<BankServerContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        public DbSet<CheckingCard> CheckingCard { get; set; }

        public DbSet<CreditCard> CreditCard { get; set; }

        public DbSet<DepositCard> DepositCard { get; set; }
    }
}
