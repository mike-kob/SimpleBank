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
        public decimal Balance => OwnMoney + Limit;
        public decimal PercentIfDelay { get; } = 0.01m;
        public bool IsInLimit { get; set; }
        public DateTime? LimitWithdrawn { get; set; }
        public DateTime? EndLimit { get => EndLimitDate(); }
        public decimal? MinSum { get; set; }

        [ForeignKey("Id")]
        public User CardUser { get; set; }
        public int Id { get; set; }
                
        public DateTime? EndLimitDate()
        {
            if (LimitWithdrawn!=null)
            {
                var y = LimitWithdrawn.Value.Year;
                bool isLeap = (y % 4 == 0 && y % 100 != 0) || (y % 400 == 0);
                var month = (LimitWithdrawn.Value.Month + 1) % 12;
                var day = 0;
                switch (month)
                {
                    case 1: case 3: case 5:
                    case 7: case 8:
                    case 10:
                        day = 31;
                        break;
                    case 12:
                        day = 31;
                        y += 1;
                        break;
                    case 4: case 6:
                    case 9:
                    case 11:
                        day = 30;
                        break;
                    case 2:
                        day = isLeap ? 29 : 28;
                        break;
                }
                return new DateTime(y, month, day);
            }
            else 
                return null;
        }
    }
}
