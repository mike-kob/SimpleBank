using System;

namespace Bank_server.Models
{
    public class CreditCard : ICard
    {
        public CreditCard(string cardNum, string pin, decimal balance, decimal limit, User cardUser)
        {
            CardNum = cardNum;
            Pin = pin;
            Balance = balance;
            Limit = limit;
            IsInLimit = false;
            LimitWithdrawn = null;
            EndLimit = null;
            IsLimitPaid = null;
            CardUser = cardUser;
        }

        public string CardNum { get; set; }
        public string Pin { get; set; }
        public decimal Balance { get; set; }       
        public decimal Limit { get; set; }
        public bool IsInLimit { get; set; }
        public DateTime? LimitWithdrawn { get; set; }
        public DateTime? EndLimit { get; set; }
        public bool? IsLimitPaid { get; set; }
        public User CardUser { get; set; }
    }
}
