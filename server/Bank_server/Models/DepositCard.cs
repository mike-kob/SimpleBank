using System;

namespace Bank_server.Models
{
    public class DepositCard : ICard
    {
        public DepositCard(string cardNum, string pin, decimal balance, decimal rate, decimal sumWithBonus, DateTime startDeposit, bool commission, User cardUser)
        {
            CardNum = cardNum;
            Pin = pin;
            Balance = balance;
            Rate = rate;
            SumWithBonus = sumWithBonus;
            StartDeposit = startDeposit;
            Commission = commission;
            CardUser = cardUser;
        }

        public string CardNum { get ; set ; }
        public string Pin { get ; set ; }
        public decimal Balance { get ; set ; }
        public decimal Rate { get; set; }
        public decimal SumWithBonus { get; set; }
        public DateTime StartDeposit { get; set; }
        public DateTime EndDeposit { get => new DateTime(StartDeposit.Year + 1, StartDeposit.Month, StartDeposit.Day, StartDeposit.Hour, StartDeposit.Minute, StartDeposit.Second); }
        public bool Commission { get; set; }
        public User CardUser { get ; set ; }

    }
}
