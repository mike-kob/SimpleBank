using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankServer.Data;
using BankServer.Models;
using Microsoft.EntityFrameworkCore;
using Timer = System.Timers.Timer;

namespace UpdateDatabase
{
    class Program
    {
        private static DbContextOptionsBuilder<BankServerContext> _optionsBuilder;
        private static BankServerContext _context;
        static void Main(string[] args)
        {
            _optionsBuilder = new DbContextOptionsBuilder<BankServerContext>();
            _optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BankServer;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            //_optionsBuilder.UseSqlServer("Server=MICHAEL-PC\\SQLEXPRESS;Database=bankDB_6;Trusted_Connection=True;MultipleActiveResultSets=true");
            using (_context = new BankServerContext(_optionsBuilder.Options))
            {
                var creditCards = _context.CreditCard;
                var depositCards = _context.DepositCard;
                HandleTimer(depositCards, creditCards);
                var timer = new Timer(1000 * 86400);
                timer.Elapsed += async (sender, e) => await HandleTimer(depositCards, creditCards);
                timer.Start();
                Console.ReadKey();
            }
        }

        private static Task HandleTimer(IQueryable<DepositCard> depositCards, IQueryable<CreditCard> creditCards)
        {
            if (depositCards.LongCount() > 0)
            {
                Console.WriteLine($"deposit cards were updated at {DateTime.Now.ToString(CultureInfo.CurrentCulture)}");
                UpdateDeposit(depositCards);
                _context.SaveChangesAsync();
            }
            if (creditCards.LongCount() > 0)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"credit cards were updated at {DateTime.Now.ToString(CultureInfo.CurrentCulture)}");
                UpdateCredit(creditCards);
                _context.SaveChangesAsync();
            }
            return Task.CompletedTask;
        }



        private static void UpdateDeposit(IEnumerable<DepositCard> cards)
        {
            foreach (var card in cards)
            {
                if (!card.UpdateBalance()) continue;
                card.TotalBalance = card.Balance + card.Balance * card.Rate;
            }
        }
        private static void UpdateCredit(IEnumerable<CreditCard> cards)
        {
            foreach (var card in cards)
            {
                if (!card.IsInLimit) continue;
                if (card.Balance < 0) continue;
                if (!(card.EndLimit < DateTime.Now)) continue;
                if (card.MinSum != null)
                {
                    var days = (DateTime.Now - card.EndLimit).Value.Days;
                    card.OwnMoney -= card.MinSum.Value * card.PercentIfDelay * days;
                }
            }
        }
    }
}
