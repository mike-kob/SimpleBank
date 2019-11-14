using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankServer.Models
{
    [Table("CheckingCard")]
    public class CheckingCard : Card
    {
        [Required]
        public new decimal Balance { get; set; }
    }
}
