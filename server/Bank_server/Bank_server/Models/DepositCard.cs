using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
        [Key]
        public string CardNum { get; set; }
        [Required]
        [MaxLength(4)]
        public string Pin { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        // initial sum
        [Required]
        public decimal Balance { get; set; }
        [Required]
        public decimal Rate { get; set; }
        [Required]
        public decimal TotalBalance => UpdateBalance() ? Balance + Balance * Rate : Balance;
        [Required]
        public DateTime StartDeposit { get; set; }
        [Required]
        public DateTime EndDeposit { get => new DateTime(StartDeposit.Year + 1, StartDeposit.Month, StartDeposit.Day, StartDeposit.Hour, StartDeposit.Minute, StartDeposit.Second); }
        //property to check  if user withdraw money before the end of the deposit
        [Required]
        public bool Commission { get; set; }
        [Required]
        public double PercentIfWithdraw { get; } = 1.0;
        [ForeignKey("Id")]
        public User CardUser { get; set; }
        public int Id { get; set; }


        private bool UpdateBalance()
        {
            //code to check if we need to add bonus
            return false;
        }
    }
}

