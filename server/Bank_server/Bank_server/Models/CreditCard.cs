using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
        [Key]
        public string CardNum { get; set; }
        [Required]
        [MaxLength(4)]
        public string Pin { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public decimal OwnMoney { get; set; }
        [Required]
        public decimal Limit { get; set; }
        [Required]
        public decimal Balance { get; }
        [Required]
        public double PercentIfDelay { get; } = 1.0;
        [Required]
        public bool IsInLimit { get; set; }
        [Required]
        public DateTime? LimitWithdrawn { get; set; }
        [Required]
        public DateTime? EndLimit { get; set; }
        [Required]
        public bool? IsLimitPaid { get; set; }

        [ForeignKey("Id")]
        public User CardUser { get; set; }
        public int Id { get; set; }

        //
        public DateTime? EndLimitDate()
        {
            //TO DO
            //you can replenish the account without any percent until calculated date - end of the next month
            return null;
        }
    }
}
