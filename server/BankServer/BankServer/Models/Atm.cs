using System.ComponentModel.DataAnnotations;

namespace BankServer.Models
{
    public class Atm
    {
        [Key]
        public int AtmId { get; set; }
        [Required]
        public decimal RemainingMoney { get; set; }
    }
}
