using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank_server.Models
{
    public class CheckingCard : ICard
    {
        public CheckingCard(string cardNum, string pin, decimal balance, User cardUser, DateTime dateCreated)
        {
            CardNum = cardNum;
            Pin = pin;
            Balance = balance;
            CardUser = cardUser;
            DateCreated = dateCreated;
        }

        public string CardNum { get; set; }
        public string Pin { get; set; }
        public decimal Balance { get; set; }
        public User CardUser { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
