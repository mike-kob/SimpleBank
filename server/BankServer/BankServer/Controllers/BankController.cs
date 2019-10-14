using Bank_server.Models;
using BankServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankServer.Controllers
{
    [Route("api")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly BankServerContext _context;

        public BankController(BankServerContext context)
        {
            _context = context;
        }

        [HttpGet("balance/{id}")]
        public async Task<ActionResult> GetBalanceDepositCard(long id)
        {

            if (await _context.CheckingCard.SingleOrDefaultAsync(m => m.CardNum == id) != null)
            {
                var card = await _context.CheckingCard.SingleOrDefaultAsync(m => m.CardNum == id);
                return new OkObjectResult(new BalanceCard { Ok = true, CardNum = id, Balance = card.Balance });
            }
            else if (await _context.DepositCard.SingleOrDefaultAsync(m => m.CardNum == id) != null)
            {
                var card = await _context.DepositCard.SingleOrDefaultAsync(m => m.CardNum == id);
                return new OkObjectResult(new BalanceCard { Ok = true, CardNum = id, Balance = card.Balance });
            }
            else if (await _context.CreditCard.SingleOrDefaultAsync(m => m.CardNum == id) != null)
            {
                var card = await _context.CreditCard.SingleOrDefaultAsync(m => m.CardNum == id);
                return new OkObjectResult(new BalanceCard { Ok = true, CardNum = id, Balance = card.Balance });
            }
            return new OkObjectResult(new BalanceCard { Ok = false, CardNum = id, Balance = 0 });
        }


        public class CardExists
        {
            public bool Ok { get; set; }
            public long CardNum { get; set; }
            public bool IsValid { get; set; }
        }
        public class BalanceCard
        {
            public bool Ok { get; set; }
            public long CardNum { get; set; }
            public decimal Balance { get; set; }
        }
    }
}
