using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankServer.Models
{
    public class TransferModel
    {
        public string CardNumFrom { get; set; }
        public string CardNumTo { get; set; }
        public decimal Amount { get; set; }
    }
}
