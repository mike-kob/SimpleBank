using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_server.Models
{
    public class DepositCard : ICard
    {
        private decimal _totalBalance;
        [Key]
        public long CardNum { get; set; }
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
        public decimal TotalBalance
        {
            get => UpdateBalance() ? Balance + Balance * Rate : Balance;
            set => _totalBalance = value;
        }
        [Required]
        public DateTime StartDeposit { get; set; }
        [Required]
        public DateTime EndDeposit { get => new DateTime(StartDeposit.Year + 1, StartDeposit.Month, StartDeposit.Day, StartDeposit.Hour, StartDeposit.Minute, StartDeposit.Second); }
        [Required]
        public bool Commission { get; set; }
        [Required]
        public decimal PercentIfWithdraw { get; } = 0.01m;
        [ForeignKey("Id")]
        public User CardUser { get; set; }
        public int Id { get; set; }

        private bool UpdateBalance()
        {
            return EndDeposit <= DateTime.Now;
        }
    }
}

