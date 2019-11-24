using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankServer.Models
{
    public class WithdrawModel
    {
        public string CardNum { get; set; }
        public decimal Amount { get; set; }
    }
}
