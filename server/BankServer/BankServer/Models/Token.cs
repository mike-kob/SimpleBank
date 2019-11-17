using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankServer.Models
{
    public class Token
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string CardNum { get; set; }
        [Required]
        public string CardToken { get; set; }
        [Required]
        public DateTime Create { get; set; }

    }
}
