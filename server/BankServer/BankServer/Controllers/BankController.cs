using BankServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BankServer.Data;

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

        public class CardExists
        {
            public bool Ok { get; set; }
            public string CardNum { get; set; }
            public bool IsValid { get; set; }
        }
        public class BalanceCard
        {
            public bool Ok { get; set; }
            public string CardNum { get; set; }
            public decimal Balance { get; set; }
        }
        public class BalanceCreditCard : BalanceCard
        {
            public decimal Limit { get; set; }
            public decimal OwnMoney { get; set; }
        }
        public class WithdrawResult
        {
            public bool Ok { get; set; }
            public bool Allowed { get; set; }
            public List<string> Errors { get; set; }
        }
        public class ConfirmWithdrawResult
        {
            public bool Ok { get; set; }
            public List<string> Errors { get; set; }
        }
        public class TransferMoney
        {
            public bool Ok { get; set; }
            public List<string> Errors { get; set; }
        }
        public class Start
        {
            public bool Ok { get; set; }
            public List<string> Errors { get; set; }
        }

        [HttpGet("cardExists/{id}")]
        public async Task<ActionResult> GetCardExists(string id)
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
            return new OkObjectResult(new CardExists { Ok = false, CardNum = id, IsValid = false });
        }

        [HttpGet("balance/{id}")]
        public async Task<ActionResult> GetBalanceCard(string id)
        {

            if (await _context.CheckingCard.SingleOrDefaultAsync(m => m.CardNum == id) != null)
            {
                var card = await _context.CheckingCard.SingleOrDefaultAsync(m => m.CardNum == id);
                return new OkObjectResult(new BalanceCard { Ok = true, CardNum = id, Balance = card.Balance });
            }
            else if (await _context.DepositCard.SingleOrDefaultAsync(m => m.CardNum == id) != null)
            {
                var card = await _context.DepositCard.SingleOrDefaultAsync(m => m.CardNum == id);
                var balanceCard = new BalanceCard { Ok = true, CardNum = id, Balance = card.Balance };
                if (card.UpdateBalance())
                {
                    if (card.Balance != card.TotalBalance)
                    {
                        card.Balance += card.Balance * card.Rate;
                        card.TotalBalance = card.Balance;
                        balanceCard.Balance = card.Balance;
                    }
                }
                else
                {
                    balanceCard.Balance = card.Balance;
                }
                return new OkObjectResult(balanceCard);
            }
            else if (await _context.CreditCard.SingleOrDefaultAsync(m => m.CardNum == id) != null)
            {
                var card = await _context.CreditCard.SingleOrDefaultAsync(m => m.CardNum == id);
                return new OkObjectResult(new BalanceCreditCard { Ok = true, CardNum = id, OwnMoney = card.OwnMoney, Limit = card.Limit, Balance = card.Balance });
            }
            return new OkObjectResult(new BalanceCard { Ok = false, CardNum = id, Balance = 0 });
        }

        [HttpPut("changePin")]
        public async Task<ActionResult<CheckingCard>> PutChangePinChecking()
        {
            string body;
            using (var stream = new StreamReader(HttpContext.Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            var myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(body);
            string cardNum = myObject["cardNum"];
            string oldPin = myObject["oldPin"];
            string newPin = myObject["newPin"];

            try
            {
                Card card = _context.Card.FirstOrDefault(c => c.CardNum == cardNum);
                if (card == null)
                {
                    return new OkObjectResult(new { Ok = false, Allowed = false, Errors = new[] { "Card doesn't exist" } });
                }
                else if (card.Pin == oldPin)
                {
                    card.Pin = newPin;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return new OkObjectResult(new { Ok = false, Allowed = false, Errors = new[] { "Old PIN code is incorrect" } });
                }
            }
            catch (ArgumentNullException)
            {
                return Unauthorized();
            }
            return new OkObjectResult(new { Ok = true, Allowed = true });
        }


        [HttpPost]
        [Route("~/api/startSession")]
        public ActionResult StartSession()
        {
            return new OkObjectResult(new Start { Ok = true });
        }

        [HttpPost]
        [Route("~/api/withdraw")]

        public async Task<ActionResult> Withdraw()
        {
            string body;
            using (var stream = new StreamReader(HttpContext.Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            var myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(body);
            string cardNum = Convert.ToString(myObject.cardNum);
            decimal amount = Convert.ToDecimal(myObject.amount);
            try
            {
                if (_context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum) != null)
                {
                    var card = _context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum);
                    _context.Transaction.Add(new Transaction
                    {
                        TypeOfTxn = 0,
                        Amount = amount,
                        DatetimeOfTxn = DateTime.Now,
                        CardSenderNum = card.CardNum
                    });
                    await _context.SaveChangesAsync();
                    if (card.Balance >= amount)
                    {
                        return await ConfirmWithdraw(card.CardNum, _context.Transaction.Last().TxnId, "finished");
                    }
                    return new OkObjectResult(new WithdrawResult { Ok = true, Allowed = false });
                }

                if (_context.CreditCard.FirstOrDefault(c => c.CardNum == cardNum) != null)
                {
                    var card = await _context.CreditCard.FirstOrDefaultAsync(c => c.CardNum == cardNum);
                    _context.Transaction.Add(new Transaction
                    {
                        TypeOfTxn = 0,
                        Amount = amount,
                        DatetimeOfTxn = DateTime.Now,
                        CardSenderNum = card.CardNum
                    });
                    await _context.SaveChangesAsync();
                    if (card.OwnMoney >= amount || card.Balance >= amount)
                    {
                        return await ConfirmWithdraw(card.CardNum, _context.Transaction.Last().TxnId, "finished");
                    }
                    return new OkObjectResult(new WithdrawResult { Ok = true, Allowed = false });

                }
                if (_context.DepositCard.FirstOrDefault(c => c.CardNum == cardNum) != null)
                {
                    var card = _context.DepositCard.FirstOrDefault(c => c.CardNum == cardNum);
                    _context.Transaction.Add(new Transaction
                    {
                        TypeOfTxn = 0,
                        Amount = amount,
                        DatetimeOfTxn = DateTime.Now,
                        CardSenderNum = card.CardNum
                    });
                    await _context.SaveChangesAsync();
                    if (card.TotalBalance >= amount)
                    {
                        return await ConfirmWithdraw(card.CardNum, _context.Transaction.Last().TxnId, "finished");
                    }

                    return new OkObjectResult(new WithdrawResult { Ok = true, Allowed = false });
                }
                return new OkObjectResult(new WithdrawResult { Ok = false, Allowed = false });
            }
            catch (Exception exc)
            {
                var res = new WithdrawResult { Ok = false, Allowed = false, Errors = new List<string> { exc.Message } };
                return new ObjectResult(res);
            }
        }

        [HttpPost]
        [Route("~/api/confirmWithdraw")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ConfirmWithdraw(string cardNum, int txnId, string finished, string errors = null)
        {
            if (!finished.Equals("finished")) return new OkObjectResult(new ConfirmWithdrawResult { Ok = false });
            Transaction transaction = null;
            try
            {
                if (_context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum) != null)
                {
                    var card = _context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum);
                    transaction = _context.Transaction.FirstOrDefault(c => c.TxnId == txnId);
                    card.Balance -= transaction.Amount;
                    await _context.SaveChangesAsync();
                    return new OkObjectResult(new ConfirmWithdrawResult { Ok = true });
                }
                else if (_context.CreditCard.FirstOrDefault(c => c.CardNum == cardNum) != null)
                {
                    var card = await _context.CreditCard.FirstOrDefaultAsync(c => c.CardNum == cardNum);
                    transaction = _context.Transaction.FirstOrDefault(c => c.TxnId == txnId);
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
                    return new OkObjectResult(new ConfirmWithdrawResult { Ok = true });
                }
                else if (_context.DepositCard.FirstOrDefault(c => c.CardNum == cardNum) != null)
                {
                    var card = _context.DepositCard.FirstOrDefault(c => c.CardNum == cardNum);
                    transaction = _context.Transaction.FirstOrDefault(c => c.TxnId == txnId);
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
                    return new OkObjectResult(new ConfirmWithdrawResult { Ok = true });
                }
            }
            catch (Exception exc)
            {
                transaction.Success = false;
                await _context.SaveChangesAsync();
                var conf = new ConfirmWithdrawResult { Ok = false, Errors = new List<string> { exc.Message } };
                return new OkObjectResult(conf);
            }

            return new OkObjectResult(new ConfirmWithdrawResult { Ok = false });
        }


        [HttpPost]
        [Route("~/api/Transfer")]
        public async Task<ActionResult<Card>> Transfer()
        {
            var body = "";
            using (var stream = new StreamReader(HttpContext.Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            var myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(body);
            string cardNumFrom = Convert.ToString(myObject.cardNumFrom);
            string cardNumTo = Convert.ToString(myObject.cardNumTo);
            decimal amount = Convert.ToDecimal(myObject.amount);
            Transaction transaction = null;
            try
            {
                Card cardFrom, cardTo;
                if (_context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNumFrom) != null)
                {
                    cardFrom = _context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNumFrom);
                }
                else if (_context.CreditCard.FirstOrDefault(c => c.CardNum == cardNumFrom) != null)
                {
                    cardFrom = _context.CreditCard.FirstOrDefault(c => c.CardNum == cardNumFrom);
                }
                else if (_context.DepositCard.FirstOrDefault(c => c.CardNum == cardNumFrom) != null)
                {
                    cardFrom = _context.DepositCard.FirstOrDefault(c => c.CardNum == cardNumFrom);
                }
                else
                {
                    return new OkObjectResult(new TransferMoney { Ok = false });
                }

                if (_context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNumTo) != null)
                {
                    cardTo = _context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNumTo);
                }
                else if (_context.CreditCard.FirstOrDefault(c => c.CardNum == cardNumTo) != null)
                {
                    cardTo = _context.CreditCard.FirstOrDefault(c => c.CardNum == cardNumTo);
                }
                else if (_context.DepositCard.FirstOrDefault(c => c.CardNum == cardNumTo) != null)
                {
                    cardTo = _context.DepositCard.FirstOrDefault(c => c.CardNum == cardNumTo);
                }
                else
                {
                    return new OkObjectResult(new TransferMoney { Ok = false });
                }

                transaction = new Transaction
                { Amount = amount, CardSenderNum = cardNumFrom, CardReceiverNum = cardNumTo };

                if (cardFrom is CheckingCard checkCard)
                {
                    if (checkCard.Balance >= amount)
                        checkCard.Balance -= amount;
                }
                else if (cardFrom is DepositCard depositCard)
                {
                    if (depositCard.UpdateBalance())
                    {
                        if (depositCard.Balance == depositCard.TotalBalance)
                        {
                            depositCard.Balance -= amount;
                            depositCard.TotalBalance -= amount;
                        }
                        else
                        {
                            depositCard.Balance += depositCard.Balance * depositCard.Rate - amount;
                            depositCard.TotalBalance = depositCard.Balance;
                        }
                    }
                    else
                    {
                        depositCard.Rate = 0m;
                        depositCard.Balance -= (amount + amount * depositCard.Commission);
                        depositCard.TotalBalance = depositCard.TotalBalance;
                    }
                }
                else
                {
                    var creditCard = (CreditCard)cardFrom;
                    if (creditCard != null)

                    {
                        if (creditCard.OwnMoney >= amount)
                        {
                            creditCard.OwnMoney -= amount;
                        }
                        else if (creditCard.Balance >= amount)
                        {
                            var initBalance = creditCard.OwnMoney;
                            if (!creditCard.IsInLimit)
                            {
                                creditCard.Limit = amount - creditCard.OwnMoney;
                                creditCard.OwnMoney = 0;
                                creditCard.IsInLimit = true;
                                creditCard.LimitWithdrawn = DateTime.Now;
                            }
                            else if (creditCard.IsInLimit && DateTime.Now <= creditCard.EndLimit)
                            {
                                creditCard.Limit = amount - creditCard.OwnMoney;
                                creditCard.OwnMoney = 0;
                            }
                            else if (creditCard.IsInLimit && DateTime.Now > creditCard.EndLimit)
                            {
                                var days = DateTime.Now.Subtract((DateTime)creditCard.EndLimit).Days;
                                var percents = days * creditCard.PercentIfDelay * initBalance;
                                creditCard.Limit = amount - creditCard.OwnMoney - percents;
                                creditCard.OwnMoney = 0;
                            }
                        }
                    }
                }

                switch (cardTo)
                {
                    case CheckingCard checkingCard:
                        checkingCard.Balance += amount;
                        break;
                    case DepositCard depositCard:
                        depositCard.Balance += amount;
                        break;
                    case CreditCard creditCard:
                        creditCard.OwnMoney += amount;
                        break;
                }

                transaction.Success = true;
                await _context.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                transaction.Success = false;
                return new OkObjectResult(new TransferMoney { Ok = false, Errors = new List<string> { exc.Message } });
            }
            return new OkObjectResult(new TransferMoney { Ok = true });
        }


    }
}
