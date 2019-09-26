using System;

namespace Bank_server.Models
{
    public class DepositCard : ICard
    {
        public DepositCard(string cardNum, string pin, DateTime dateCreated, User cardUser, decimal balance, decimal rate, DateTime startDeposit)
        {
            CardNum = cardNum;
            Pin = pin;
            DateCreated = dateCreated;
            CardUser = cardUser;
            Balance = balance;
            Rate = rate;
            StartDeposit = startDeposit;
            Commission = false;
        }

        public string CardNum { get ; set ; }
        public string Pin { get ; set ; }
        public DateTime DateCreated { get; set; }
        public User CardUser { get; set; }
        // initial sum
        public decimal Balance { get; set; }
        public decimal Rate { get; set; }
        //sum with percents; is updated each ??
        public decimal TotalBalance => UpdateBalance() ? Balance + Balance * Rate : Balance;
        public DateTime StartDeposit { get; set; }
        public DateTime EndDeposit { get => new DateTime(StartDeposit.Year + 1, StartDeposit.Month, StartDeposit.Day, StartDeposit.Hour, StartDeposit.Minute, StartDeposit.Second); }
        //property to check  if user withdraw money before the end of the deposit
        public bool Commission { get; set; }
        public double PercentIfWithdraw { get; } = 1.0;
        private bool UpdateBalance()
        {
            //code to check if we need to add bonus
            return false;
        }
    }
}
