using BankServer.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_server.Models
{
    public class DepositCard : ICard
    {
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
        public decimal TotalBalance { get; set; }
        [Required]
        public DateTime StartDeposit { get; set; }
        [Required]
        public DateTime EndDeposit { get => new DateTime(StartDeposit.Year + 1, StartDeposit.Month, StartDeposit.Day, StartDeposit.Hour, StartDeposit.Minute, StartDeposit.Second); }
        [Required]
        public decimal Commission  { get; } = 0.01m;
        [ForeignKey("Id")]
        public User CardUser { get; set; }
        public int Id { get; set; }
        [ForeignKey("TxnId")]
        public Transaction Txn { get; set; }
        public int TxnId { get; set; }

        public bool UpdateBalance()
        {
            return EndDeposit <= DateTime.Now;
        }
    }
}

