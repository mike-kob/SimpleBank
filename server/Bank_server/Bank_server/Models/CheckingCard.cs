using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bank_server.Models
{
    public class CheckingCard:ICard
    {
        public CheckingCard(string cardNum, string pin, decimal balance, User cardUser, DateTime dateCreated)
        {
            CardNum = cardNum;
            Pin = pin;
            Balance = balance;
            CardUser = cardUser;
            DateCreated = dateCreated;
        }
        [Key]
        public string CardNum { get; set; }
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
        
    }
}
