using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankServer.Models
{
    public abstract class Card
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CardNum { get; set; }
        public string Pin { get; set; }
        public DateTime DateCreated { get; set; }
        public User CardUser { get; set; }
        public decimal Balance { get; }
    }
}
