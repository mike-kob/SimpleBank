using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_server.Models
{
    public class CreditCard : ICard
    {
        [Key]
        public long CardNum { get; set; }
        [Required]
        [MaxLength(4)]
        public string Pin { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public decimal OwnMoney { get; set; }
        [Required]
        public decimal Limit { get; set; }
        [Required]
        public decimal Balance { get; }
        public double PercentIfDelay { get; } = 1.0;
        public bool IsInLimit { get; set; }
        public DateTime? LimitWithdrawn { get; set; }
        public DateTime? EndLimit { get; set; }
        public bool? IsLimitPaid { get; set; }

        [ForeignKey("Id")]
        public User CardUser { get; set; }
        public int Id { get; set; }

        //
        public DateTime? EndLimitDate()
        {
            //TO DO
            //you can replenish the account without any percent until calculated date - end of the next month
            return null;
        }
    }
}
