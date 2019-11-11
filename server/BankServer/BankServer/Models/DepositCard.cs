using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankServer.Models
{
    [Table("DepositCard")]
    public class DepositCard : Card
    {
        [Key]
        public string CardNum { get; set; }
        [Required]
        [MaxLength(4)]
        public string Pin { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
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
        [ForeignKey("UserId")]
        public User CardUser { get; set; }
        public int UserId { get; set; }
        

        public bool UpdateBalance()
        {
            return EndDeposit <= DateTime.Now;
        }
    }
}

