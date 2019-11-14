using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankServer.Models
{
    [Table("DepositCard")]
    public class DepositCard : Card
    {
        // initial sum
        [Required]
        public new decimal Balance { get; set; }
        [Required]
        public decimal Rate { get; set; }
        [Required]
        public decimal TotalBalance { get; set; }
        [Required]
        public DateTime StartDeposit { get; set; }
        [Required]
        public DateTime EndDeposit => new DateTime(StartDeposit.Year + 1, StartDeposit.Month, StartDeposit.Day, StartDeposit.Hour, StartDeposit.Minute, StartDeposit.Second);

        [Required]
        public decimal Commission  { get; } = 0.01m;
              

        public bool UpdateBalance()
        {
            return EndDeposit <= DateTime.Now;
        }
    }
}

