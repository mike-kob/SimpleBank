using Bank_server.Models;
using BankServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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

        [HttpGet("cardExists/{id}")]
        public async Task<ActionResult> GetCardExists(long id)
        {

            if (await _context.CheckingCard.SingleOrDefaultAsync(m => m.CardNum == id) != null)
            {
                var card = await _context.CheckingCard.SingleOrDefaultAsync(m => m.CardNum == id);
                return new OkObjectResult(new CardExists { Ok = true, CardNum = id, IsValid = true });
            }
            else if (await _context.DepositCard.SingleOrDefaultAsync(m => m.CardNum == id) != null)
            {
                var card = await _context.DepositCard.SingleOrDefaultAsync(m => m.CardNum == id);
                return new OkObjectResult(new CardExists { Ok = true, CardNum = id, IsValid = true });
            }
            else if (await _context.CreditCard.SingleOrDefaultAsync(m => m.CardNum == id) != null)
            {
                var card = await _context.CreditCard.SingleOrDefaultAsync(m => m.CardNum == id);
                return new OkObjectResult(new CardExists { Ok = true, CardNum = id, IsValid = true });
            }
            return new OkObjectResult(new BalanceCard { Ok = false, CardNum = id, Balance = 0 });
        }

        [HttpGet("balance/{id}")]
        public async Task<ActionResult> GetBalanceCard(long id)
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

        [HttpPut("changePin")]
        public async Task<ActionResult<CheckingCard>> PutChangePinChecking()
        {
            var body = "";
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            dynamic myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(body);
            long cardNum = Convert.ToInt64(myObject.num);
            string oldPin = Convert.ToString(myObject.old);
            string newPin = Convert.ToString(myObject.newp);

            try
            {
                if (_context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum).Pin==oldPin)
                {
                    _context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum).Pin=newPin;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Forbid();
                }
            }
            catch (ArgumentNullException)
            {
                return Unauthorized();
            }
            return Ok("allowed");
        }

        [HttpPut("changePin")]
        public async Task<ActionResult<DepositCard>> PutChangePinDeposit()
        {
            var body = "";
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            dynamic myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(body);
            long cardNum = Convert.ToInt64(myObject.num);
            string oldPin = Convert.ToString(myObject.old);
            string newPin = Convert.ToString(myObject.newp);

            try
            {
                if (_context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum).Pin == oldPin)
                {
                    _context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum).Pin = newPin;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Forbid();
                }
            }
            catch (ArgumentNullException)
            {
                return Unauthorized();
            }
            return Ok("allowed");
        }

        [HttpPut("changePin")]
        public async Task<ActionResult<CreditCard>> PutChangePinCredit()
        {
            var body = "";
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            dynamic myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(body);
            long cardNum = Convert.ToInt64(myObject.num);
            string oldPin = Convert.ToString(myObject.old);
            string newPin = Convert.ToString(myObject.newp);

            try
            {
                if (_context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum).Pin == oldPin)
                {
                    _context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum).Pin = newPin;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Forbid();
                }
            }
            catch (ArgumentNullException)
            {
                return Unauthorized();
            }
            return Ok("allowed");
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
