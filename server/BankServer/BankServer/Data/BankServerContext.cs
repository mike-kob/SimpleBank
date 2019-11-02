using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bank_server.Models;
using BankServer.Models;

namespace BankServer.Models
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

        public DbSet<BankServer.Models.Transaction> Transaction { get; set; }
    }
}
