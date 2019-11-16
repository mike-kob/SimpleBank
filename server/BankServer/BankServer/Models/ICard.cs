using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankServer.Models
{
    public abstract class Card
    {
        [Key]
        public string CardNum { get; set; }
        [Required]
        [MaxLength(4)]
        public string Pin { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [ForeignKey("UserId")]
        public User CardUser { get; set; }
        public int UserId { get; set; }
        [Required]
        public decimal Balance { get; }
    }
}
