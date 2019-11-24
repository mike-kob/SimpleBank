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
            _optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BankServer;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=true");
            //_optionsBuilder.UseSqlServer("Server=MICHAEL-PC\\SQLEXPRESS;Database=bankDB_6;Trusted_Connection=True;MultipleActiveResultSets=true");
            using (_context = new BankServerContext(_optionsBuilder.Options))
            {
                var creditCards = _context.CreditCard;
                var depositCards = _context.DepositCard;
                var transactions = _context.Transaction;
                HandleTimer(depositCards, creditCards, transactions);
                var timer = new Timer(1000 * 5);
                //var timer = new Timer(1000 * 86400);
                timer.Elapsed += async (sender, e) => await HandleTimer(depositCards, creditCards, transactions);
                timer.Start();
                Console.ReadKey();
            }
        }

        private static Task HandleTimer(IQueryable<DepositCard> depositCards, IQueryable<CreditCard> creditCards, IQueryable<Transaction> transactions)
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
                UpdateCredit(creditCards, transactions);
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
        private static void UpdateCredit(IEnumerable<CreditCard> cards, IEnumerable<Transaction> transactions)
        {
            foreach (var card in cards)
            {
                if (!card.IsInLimit) continue;
                if (card.Balance < 0) continue;
                if (!(card.EndLimit < DateTime.Now)) continue;
                var days = (DateTime.Now - card.EndLimit).Value.Days;
                var lastTransaction = transactions.Where(x => x.TypeOfTxn == 2 && x.CardSenderNum == card.CardNum).OrderByDescending(x=>x.DatetimeOfTxn).FirstOrDefault();
                if (lastTransaction != null)
                {
                    days = (DateTime.Now - lastTransaction.DatetimeOfTxn).Days;
                }
                if (card.MinSum != null)
                {
                    var withdrawalSum = card.MinSum.Value * card.PercentIfDelay * days;
                    var transaction = new Transaction
                    {
                        TypeOfTxn = 2,
                        Amount = withdrawalSum,
                        DatetimeOfTxn = DateTime.Now,
                        CardSenderNum = card.CardNum
                    };
                    _context.Transaction.Add(transaction);
                    _context.SaveChangesAsync();
                    card.OwnMoney -= withdrawalSum;
                }
            }
        }
    }
}
