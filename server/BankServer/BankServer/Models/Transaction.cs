using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankServer.Models
{
    public class Transaction
    {
        [Key]       
        public int TxnId { get; set; }
        [Required]
        public int TypeOfTxn { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime DatetimeOfTxn { get; set; }
        [Required]
        public bool Success { get; set; }
        [ForeignKey("CardSenderNum")]
        public Card CardSender { get; set; }
        public string CardSenderNum { get; set; }
        [ForeignKey("CardReceiverNum")]
        public Card CardReceiver { get; set; }
        public string CardReceiverNum { get; set; }

    }
}
