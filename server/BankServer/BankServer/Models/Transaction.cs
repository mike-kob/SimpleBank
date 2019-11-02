using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankServer.Models
{
    public class Transaction
    {
        [Key]       
        public int TxnId { get; set; }
        [Required]
        public int TypeOfTxn { get; set; }
        [Required]
        public long CardSender { get; set; }

        public long CardReceiver { get; set; }
        [Required]
        public double amount { get; set; }
        [Required]
        public DateTime DatetimeOfTxn { get; set; }
        [Required]
        public bool Success { get; set; }
    }
}
