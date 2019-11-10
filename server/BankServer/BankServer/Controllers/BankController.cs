using Bank_server.Models;
using BankServer.Models;
using Microsoft.AspNetCore.Http;
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
        public class Withdraw
        {
            public bool Ok { get; set; }
            public bool Allowed { get; set; }
            public List<string> Errors { get; set; }
        }
        public class ConfirmWithdraw
        {
            public bool Ok { get; set; }
            public List<string> Errors { get; set; }
        }
        public class Transfer
        {
            public bool Ok { get; set; }
            public List<string> Errors { get; set; }
        }
        public class Start
        {
            public bool Ok { get; set; }
            public List<string> Errors { get; set; }
        }
        [HttpPost]
        [Route("~/api/startSession")]
        public ActionResult StartSession()
        {
            return new OkObjectResult(new Start { Ok = true });

        [HttpPost]
        [Route("~/api/СheckingWithdraw")]

        public async Task<ActionResult<CheckingCard>> СheckingWithdraw()
        {
            var body = "";
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            dynamic myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(body);
            long cardNum = Convert.ToInt64(myObject.num);
            decimal amount = Convert.ToDecimal(myObject.amount);
            try
            {
                var card = _context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum);
                var transaction = new Transaction
                {
                    TypeOfTxn = 0,
                    CardSender = card.CardNum,
                    Amount = amount,
                    DatetimeOfTxn = DateTime.Now
                };
                if (card.Balance >= amount)
                {
                    transaction.Success = true;
                    return await CheckingConfirmWithdraw(card.CardNum, "finished");
                }
                else
                {
                    return new OkObjectResult(new Withdraw { Ok = true, Allowed = false }); ;
                }
            }
            catch (ArgumentNullException)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("~/api/DepositWithdraw")]
        public async Task<ActionResult<DepositCard>> DepositWithdraw()
        {
            var body = "";
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            dynamic myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(body);
            long cardNum = Convert.ToInt64(myObject.num);
            decimal amount = Convert.ToDecimal(myObject.amount);
            try
            {
                var card = await _context.DepositCard.FirstOrDefaultAsync(c => c.CardNum == cardNum);
                if (card.TotalBalance >= amount)
                {
                    return await DepositConfirmWithdraw(card.CardNum, "finished");
                }
                else
                {
                    return new OkObjectResult(new Withdraw { Ok = true, Allowed = false });
                }
            }
            catch (ArgumentNullException)
            {
                return Unauthorized();
            }
            catch (AccessViolationException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("~/api/CreditWithdraw")]
        public async Task<ActionResult<CreditCard>> CreditWithdraw()
        {
            var body = "";
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            dynamic myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(body);
            long cardNum = Convert.ToInt64(myObject.num);
            decimal amount = Convert.ToDecimal(myObject.amount);
            try
            {
                var card = await _context.CreditCard.FirstOrDefaultAsync(c => c.CardNum == cardNum);
                if (card.OwnMoney >= amount || card.Balance >= amount)
                {
                    return await CreditConfirmWithdraw(card.CardNum, "finished");
                }
                else
                {
                    return new OkObjectResult(new Withdraw { Ok = true, Allowed = false }); ;
                }
            }
            catch (ArgumentNullException)
            {
                return Unauthorized();
            }
            catch (AccessViolationException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<CheckingCard>> CheckingConfirmWithdraw(long cardNum, string finished, string errors = null)
        {
            if (finished.Equals("finished"))
            {
                Transaction transaction = null;
                try
                {
                    var card = _context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum);
                    transaction = _context.Transaction.FirstOrDefault(c => c.CardSender == cardNum);
                    card.Balance -= transaction.Amount;
                    await _context.SaveChangesAsync();
                    return new OkObjectResult(new ConfirmWithdraw { Ok = true });
                }
                catch (Exception exc)
                {
                    transaction.Success = false;
                    await _context.SaveChangesAsync();
                    var conf = new ConfirmWithdraw { Ok = false };
                    conf.Errors = new List<string>
                    {
                        exc.Message
                    };
                    return new OkObjectResult(conf);
                }
            }
            return new OkObjectResult(new ConfirmWithdraw { Ok = false });
        }

        public async Task<ActionResult<DepositCard>> DepositConfirmWithdraw(long cardNum, string finished, string errors = null)
        {
            if (finished.Equals("finished"))
            {
                Transaction transaction = null;
                try
                {
                    var card = _context.DepositCard.FirstOrDefault(c => c.CardNum == cardNum);
                    if (card.UpdateBalance())
                    {
                        if (card.Balance == card.TotalBalance)
                        {
                            card.Balance -= transaction.Amount;
                            card.TotalBalance -= transaction.Amount;
                        }
                        else
                        {
                            card.Balance += card.Balance * card.Rate - transaction.Amount;
                            card.TotalBalance = card.Balance;
                        }
                    }
                    else
                    {
                        card.Rate = 0m;
                        card.Balance -= (transaction.Amount + transaction.Amount * card.Commission);
                        card.TotalBalance = card.TotalBalance;

                    }
                    await _context.SaveChangesAsync();
                    return new OkObjectResult(new ConfirmWithdraw { Ok = true });
                }
                catch (Exception exc)
                {
                    transaction.Success = false;
                    await _context.SaveChangesAsync();
                    var conf = new ConfirmWithdraw { Ok = false };
                    conf.Errors = new List<string>
                    {
                        exc.Message
                    };
                    return new OkObjectResult(conf);
                }
            }
            return new OkObjectResult(new ConfirmWithdraw { Ok = false });
        }
        public async Task<ActionResult<CreditCard>> CreditConfirmWithdraw(long cardNum, string finished, string errors = null)
        {
            if (finished.Equals("finished"))
            {
                Transaction transaction = null;
                try
                {
                    var card = await _context.CreditCard.FirstOrDefaultAsync(c => c.CardNum == cardNum);
                    if (card.OwnMoney >= transaction.Amount)
                    {
                        card.OwnMoney -= transaction.Amount;
                    }
                    else if (card.Balance >= transaction.Amount)
                    {
                        var initBalance = card.OwnMoney;
                        if (!card.IsInLimit)
                        {
                            card.Limit = transaction.Amount - card.OwnMoney;
                            card.OwnMoney = 0;
                            card.IsInLimit = true;
                            card.LimitWithdrawn = DateTime.Now;
                        }
                        else if (card.IsInLimit && DateTime.Now <= card.EndLimit)
                        {
                            card.Limit = transaction.Amount - card.OwnMoney;
                            card.OwnMoney = 0;
                        }
                        else if (card.IsInLimit && DateTime.Now > card.EndLimit)
                        {
                            var days = DateTime.Now.Subtract((DateTime)card.EndLimit).Days;
                            var percents = days * card.PercentIfDelay * initBalance;
                            card.Limit = transaction.Amount - card.OwnMoney - percents;
                            card.OwnMoney = 0;
                            if (card.Limit < 0) card.Limit = 0;
                        }
                    }
                    await _context.SaveChangesAsync();
                    return new OkObjectResult(new ConfirmWithdraw { Ok = true });
                }
                catch (Exception exc)
                {
                    transaction.Success = false;
                    await _context.SaveChangesAsync();
                    var conf = new ConfirmWithdraw { Ok = false };
                    conf.Errors = new List<string>
                    {
                        exc.Message
                    };
                    return new OkObjectResult(conf);
                }
            }
            return new OkObjectResult(new ConfirmWithdraw { Ok = false });
        }
        public async Task<ActionResult<CheckingCard>> CheckingTransfer()
        {
            var body = "";
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            dynamic myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(body);
            long cardNumFrom = Convert.ToInt64(myObject.numFrom);
            long cardNumTo = Convert.ToInt64(myObject.numTo);
            decimal amount = Convert.ToDecimal(myObject.amount);

            try
            {
                if (_context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNumFrom).Balance >= amount)
                {
                    _context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNumFrom).Balance -= amount;
                    _context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNumTo).Balance += amount;
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

            return new OkObjectResult(new Transfer { Ok = true });
        }

        public async Task<ActionResult<DepositCard>> DepositTransfer()
        {
            var body = "";
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            dynamic myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(body);
            long cardNumFrom = Convert.ToInt64(myObject.numFrom);
            long cardNumTo = Convert.ToInt64(myObject.numTo);
            decimal amount = Convert.ToDecimal(myObject.amount);

            try
            {
                if (_context.DepositCard.FirstOrDefault(c => c.CardNum == cardNumFrom).TotalBalance >= amount)
                {
                    var card = _context.DepositCard.FirstOrDefault(c => c.CardNum == cardNumFrom);
                    if (card.UpdateBalance())
                    {
                        if (card.Balance == card.TotalBalance)
                        {
                            card.Balance -= amount;
                            card.TotalBalance -= amount;
                        }
                        else
                        {
                            card.Balance += card.Balance * card.Rate - amount;
                            card.TotalBalance = card.Balance;
                        }
                    }
                    else
                    {
                        card.Rate = 0m;
                        card.Balance -= (amount + amount * card.Commission);
                        card.TotalBalance = card.TotalBalance;
                    }
                    _context.DepositCard.FirstOrDefault(c => c.CardNum == cardNumTo).Balance += amount;
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

            return new OkObjectResult(new Transfer { Ok = true });
        }


        public async Task<ActionResult<CreditCard>> CreditTransfer()
        {
            var body = "";
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            dynamic myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(body);
            long cardNumFrom = Convert.ToInt64(myObject.numFrom);
            long cardNumTo = Convert.ToInt64(myObject.numTo);
            decimal amount = Convert.ToDecimal(myObject.amount);

            try
            {
                var card = await _context.CreditCard.FirstOrDefaultAsync(c => c.CardNum == cardNumFrom);
                if (card.OwnMoney >= amount)
                {
                    card.OwnMoney -= amount;
                }
                else if (card.Balance >= amount)
                {
                    var initBalance = card.OwnMoney;
                    if (!card.IsInLimit)
                    {
                        card.Limit = amount - card.OwnMoney;
                        card.OwnMoney = 0;
                        card.IsInLimit = true;
                        card.LimitWithdrawn = DateTime.Now;
                    }
                    else if (card.IsInLimit && DateTime.Now <= card.EndLimit)
                    {
                        card.Limit = amount - card.OwnMoney;
                        card.OwnMoney = 0;
                    }
                    else if (card.IsInLimit && DateTime.Now > card.EndLimit)
                    {
                        var days = DateTime.Now.Subtract((DateTime)card.EndLimit).Days;
                        var percents = days * card.PercentIfDelay * initBalance;
                        card.Limit = amount - card.OwnMoney - percents;
                        card.OwnMoney = 0;
                    }
                    var cardTo = _context.CreditCard.FirstOrDefault(c => c.CardNum == cardNumTo);
                    cardTo.OwnMoney += amount;
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
            return new OkObjectResult(new Transfer { Ok = true });
        }
    }
}
