using System;

namespace Bank_server.Models
{
    public class CreditCard : ICard
    {
        public CreditCard(string cardNum, string pin, User cardUser, DateTime dateCreated, decimal ownMoney, decimal limit)
        {
            CardNum = cardNum;
            Pin = pin;
            CardUser = cardUser;
            DateCreated = dateCreated;
            OwnMoney = ownMoney;
            Limit = limit;
            Balance = OwnMoney + Limit;
            IsInLimit = false;
            LimitWithdrawn = null;
            EndLimit = null;
            IsLimitPaid = null;
        }

        public string CardNum { get; set; }
        public string Pin { get; set; }
        public User CardUser { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal OwnMoney { get; set; }
        public decimal Limit { get; set; }
        public decimal Balance { get; }
        public double PercentIfDelay { get; } = 1.0;
        public bool IsInLimit { get; set; }
        public DateTime? LimitWithdrawn { get; set; }
        public DateTime? EndLimit { get; set; }
        public bool? IsLimitPaid { get; set; }
        
        public DateTime? EndLimitDate()
        {
            //TO DO
            //you can replenish the account without any percent until calculated date - end of the next month
            return null;
        }
    }
}
