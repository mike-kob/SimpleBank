using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bank_server.Models;

namespace Bank_server.Models
{
    public class Bank_serverContext : DbContext
    {
        public Bank_serverContext (DbContextOptions<Bank_serverContext> options)
            : base(options)
        {
        }

        public DbSet<Bank_server.Models.User> User { get; set; }

        public DbSet<Bank_server.Models.CheckingCard> CheckingCard { get; set; }

        public DbSet<Bank_server.Models.CreditCard> CreditCard { get; set; }

        public DbSet<Bank_server.Models.DepositCard> DepositCard { get; set; }
    }
}
