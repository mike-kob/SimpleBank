using BankServer.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_server.Models
{
    public class CheckingCard:ICard
    {
        [Key]
        public long CardNum { get; set; }
        [Required]
        [MaxLength(4)]
        public string Pin { get; set; }
        [Required]
        public decimal Balance { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [ForeignKey("Id")]
        public User CardUser { get; set; }
        public int Id { get; set; }
        [ForeignKey("TxnId")]
        public Transaction Txn { get; set; }
        public int TxnId { get; set; }

    }
}
