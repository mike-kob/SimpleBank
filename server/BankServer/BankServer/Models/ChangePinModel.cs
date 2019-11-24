using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankServer.Models
{
    public class ChangePinModel
    {
        public string CardNum { get; set; }
        public string OldPin { get; set; }
        public string NewPin { get; set; }
    }
}
