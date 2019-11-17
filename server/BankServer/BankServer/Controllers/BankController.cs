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
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

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

        #region ReturnObjects

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
        public class Log
        {
            public bool Ok { get; set; }
            public string Token  { get; set; }
            public List<string> Errors { get; set; }
        }

        #endregion
        
        public class AuthOptions
        {
            public const string ISSUER = "Bankiri";
            public const string AUDIENCE = "http://localhost:51884/";
            const string KEY = "bbbbbbbbbbbbbbbb"; 
            public const int LIFETIME = 0;
            public static SymmetricSecurityKey GetSymmetricSecurityKey()
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
            }
        }

        [HttpGet("cardExists/{id}")]
        public async Task<ActionResult> GetCardExists(string id)
        {
            var card = await _context.Card.SingleOrDefaultAsync(m => m.CardNum == id);

            if (card != null)
            {
                return new OkObjectResult(new CardExists { Ok = true, CardNum = id, IsValid = true });
            }

            return new OkObjectResult(new CardExists { Ok = false, CardNum = id, IsValid = false });
        }

        [HttpGet("balance/{id}")]
        public async Task<ActionResult> GetBalanceCard(string id)
        {
            if (!IsAuthenticated(id))
            {
                return Unauthorized();
            }
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
            if (!IsAuthenticated(cardNum))
            {
                return Unauthorized();
            }
            try
            {
                Card card = _context.Card.FirstOrDefault(c => c.CardNum == cardNum);
                if (card == null)
                {
                    return new OkObjectResult(new { Ok = false, Allowed = false, Errors = new[] { "Card doesn't exist" } });
                }
                else
                {
                    card.Pin = newPin; //ComputeSha256Hash(newPin);
                    _context.SaveChanges();
                    return new OkObjectResult(new { Ok = true, Allowed = true });
                }
            }
            catch (ArgumentNullException)
            {
                return Unauthorized();
            }
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
            if (!IsAuthenticated(cardNum))
            {
                return Unauthorized();
            }

            try
            {
                if (_context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum) != null)
                {
                    var card = _context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum);
                    Transaction transaction = new Transaction
                    {
                        TypeOfTxn = 0,
                        Amount = amount,
                        DatetimeOfTxn = DateTime.Now,
                        CardSenderNum = card.CardNum
                    };
                    _context.Transaction.Add(transaction);
                    await _context.SaveChangesAsync();
                    if (card.Balance < amount)
                    {
                        await ConfirmWithdraw(card.CardNum, transaction.TxnId, false);
                        return new OkObjectResult(new { Ok = true, Allowed = false, Errors = new[] { "Not enough money" } });
                    }
                    else
                    {
                        return new OkObjectResult(new { Ok = true, Allowed = true, TxnId = transaction.TxnId});
                    }
                }

                if (_context.CreditCard.FirstOrDefault(c => c.CardNum == cardNum) != null)
                {
                    var card = await _context.CreditCard.FirstOrDefaultAsync(c => c.CardNum == cardNum);
                    Transaction transaction = new Transaction
                    {
                        TypeOfTxn = 0,
                        Amount = amount,
                        DatetimeOfTxn = DateTime.Now,
                        CardSenderNum = card.CardNum
                    };
                    _context.Transaction.Add(transaction);
                    await _context.SaveChangesAsync();
                    if (card.Balance < amount)
                    {
                        await ConfirmWithdraw(card.CardNum, transaction.TxnId, false);
                        new OkObjectResult(new { Ok = true, Allowed = false, Errors = new[] { "Not enough money" } });
                    }
                    else
                    {
                        return new OkObjectResult(new { Ok = true, Allowed = true, TxnId = transaction.TxnId });
                    }

                }

                if (_context.DepositCard.FirstOrDefault(c => c.CardNum == cardNum) != null)
                {
                    var card = _context.DepositCard.FirstOrDefault(c => c.CardNum == cardNum);
                    Transaction transaction = new Transaction
                    {
                        TypeOfTxn = 0,
                        Amount = amount,
                        DatetimeOfTxn = DateTime.Now,
                        CardSenderNum = card.CardNum
                    };
                    _context.Transaction.Add(transaction);
                    await _context.SaveChangesAsync();
                    if (card.EndDeposit >= DateTime.Now)
                    {
                        if (card.Balance < amount)
                        {
                            await ConfirmWithdraw(card.CardNum, transaction.TxnId, false);
                            return new OkObjectResult(new { Ok = true, Allowed = false, Errors = new[] { "Not enough money" } });
                        }
                        else
                        {
                            return new OkObjectResult(new { Ok = true, Allowed = true, Errors = new[] { "Discarding all benefits" } });
                        }
                    }
                    else
                    {
                        if (card.TotalBalance < amount)
                        {
                            await ConfirmWithdraw(card.CardNum, transaction.TxnId, false);
                            return new OkObjectResult(new { Ok = true, Allowed = false, Errors = new[] { "Not enough money" } });
                        }
                        else
                        {
                            return new OkObjectResult(new { Ok = true, Allowed = true});
                        }
                    }

                }
                
                return new OkObjectResult(new { Ok = false, Allowed = false, Errors = new[] { "Card not found" } });
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
        public async Task<ActionResult> ConfirmWithdraw(string cardNum = null, int? txnId = null, bool success = false, string errors = null)
        {
            if (cardNum == null || txnId == null)
            {
                string body;
                using (var stream = new StreamReader(HttpContext.Request.Body))
                {
                    body = await stream.ReadToEndAsync();
                }
                var myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(body);
                cardNum = Convert.ToString(myObject.cardNum);
                txnId = Convert.ToInt32(myObject.txnId);
                success = Convert.ToBoolean(myObject.success);
                if (!IsAuthenticated(cardNum))
                {
                    return Unauthorized();
                }
            }

            try
            {
                Transaction transaction = await _context.Transaction.FirstOrDefaultAsync(txn => txn.TxnId == txnId);
                if (transaction == null)
                {
                    return new OkObjectResult(new { Ok = false, Errors = new[] { "Not found" } });
                }
                transaction.Success = success;
                await _context.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                await _context.SaveChangesAsync();
                var conf = new ConfirmWithdrawResult { Ok = false, Errors = new List<string> { exc.Message } };
                return new OkObjectResult(conf);
            }
            return new OkObjectResult(new ConfirmWithdrawResult { Ok = false });



            //if (!result.Equals("finished")) return new OkObjectResult(new ConfirmWithdrawResult { Ok = false });
            //Transaction transaction = null;
            //try
            //{
            //    if (_context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum) != null)
            //    {
            //        var card = _context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum);
            //        transaction = _context.Transaction.FirstOrDefault(c => c.TxnId == txnId);
            //        card.Balance -= transaction.Amount;
            //        await _context.SaveChangesAsync();
            //        return new OkObjectResult(new ConfirmWithdrawResult { Ok = true });
            //    }
            //    else if (_context.CreditCard.FirstOrDefault(c => c.CardNum == cardNum) != null)
            //    {
            //        var card = await _context.CreditCard.FirstOrDefaultAsync(c => c.CardNum == cardNum);
            //        transaction = _context.Transaction.FirstOrDefault(c => c.TxnId == txnId);
            //        if (card.OwnMoney >= transaction.Amount)
            //        {
            //            card.OwnMoney -= transaction.Amount;
            //        }
            //        else if (card.Balance >= transaction.Amount)
            //        {
            //            var initBalance = card.OwnMoney;
            //            if (!card.IsInLimit)
            //            {
            //                card.Limit = transaction.Amount - card.OwnMoney;
            //                card.OwnMoney = 0;
            //                card.IsInLimit = true;
            //                card.LimitWithdrawn = DateTime.Now;
            //            }
            //            else if (card.IsInLimit && DateTime.Now <= card.EndLimit)
            //            {
            //                card.Limit = transaction.Amount - card.OwnMoney;
            //                card.OwnMoney = 0;
            //            }
            //            else if (card.IsInLimit && DateTime.Now > card.EndLimit)
            //            {
            //                var days = DateTime.Now.Subtract((DateTime)card.EndLimit).Days;
            //                var percents = days * card.PercentIfDelay * initBalance;
            //                card.Limit = transaction.Amount - card.OwnMoney - percents;
            //                card.OwnMoney = 0;
            //                if (card.Limit < 0) card.Limit = 0;
            //            }
            //        }
            //        await _context.SaveChangesAsync();
            //        return new OkObjectResult(new ConfirmWithdrawResult { Ok = true });
            //    }
            //    else if (_context.DepositCard.FirstOrDefault(c => c.CardNum == cardNum) != null)
            //    {
            //        var card = _context.DepositCard.FirstOrDefault(c => c.CardNum == cardNum);
            //        transaction = _context.Transaction.FirstOrDefault(c => c.TxnId == txnId);
            //        if (card.UpdateBalance())
            //        {
            //            if (card.Balance == card.TotalBalance)
            //            {
            //                card.Balance -= transaction.Amount;
            //                card.TotalBalance -= transaction.Amount;
            //            }
            //            else
            //            {
            //                card.Balance += card.Balance * card.Rate - transaction.Amount;
            //                card.TotalBalance = card.Balance;
            //            }
            //        }
            //        else
            //        {
            //            card.Rate = 0m;
            //            card.Balance -= (transaction.Amount + transaction.Amount * card.Commission);
            //            card.TotalBalance = card.TotalBalance;

            //        }
            //        await _context.SaveChangesAsync();
            //        return new OkObjectResult(new ConfirmWithdrawResult { Ok = true });
            //    }
            //}
            //catch (Exception exc)
            //{
            //    transaction.Success = false;
            //    await _context.SaveChangesAsync();
            //    var conf = new ConfirmWithdrawResult { Ok = false, Errors = new List<string> { exc.Message } };
            //    return new OkObjectResult(conf);
            //}
        }

        [HttpPost]
        [Route("~/api/transfer")]
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

            if (!IsAuthenticated(cardNumFrom))
            {
                return Unauthorized();
            }

            Transaction transaction = new Transaction
            { Amount = amount, DatetimeOfTxn = DateTime.Now, TypeOfTxn = 1};

            try
            {
                Card cardFrom, cardTo;
                if (_context.Card.FirstOrDefault(c => c.CardNum == cardNumFrom) != null)
                {
                    cardFrom = _context.Card.FirstOrDefault(c => c.CardNum == cardNumFrom);
                    transaction.CardSender = cardFrom;
                    transaction.CardSenderNum = cardFrom.CardNum;
                }
                else
                {
                    return new OkObjectResult(new { Ok = false, Errors = new[] { "Sender card not found" } });
                }

                if (_context.Card.FirstOrDefault(c => c.CardNum == cardNumTo) != null)
                {
                    cardTo = _context.Card.FirstOrDefault(c => c.CardNum == cardNumTo);
                    transaction.CardReceiver = cardTo;
                    transaction.CardReceiverNum = cardTo.CardNum;
                }
                else
                {
                    return new OkObjectResult(new { Ok = false, Errors = new[] { "Recepient card not found" } });
                }

                if (cardFrom is CheckingCard checkCard)
                {
                    if (checkCard.Balance >= amount)
                        checkCard.Balance -= amount;
                }
                else if (cardFrom is DepositCard depositCard)
                {
                    return new OkObjectResult(new { Ok = false, Errors = new[] { "Cannot transfer from Deposit card" } });
                    //if (depositCard.UpdateBalance())
                    //{
                    //    if (depositCard.Balance == depositCard.TotalBalance)
                    //    {
                    //        depositCard.Balance -= amount;
                    //        depositCard.TotalBalance -= amount;
                    //    }
                    //    else
                    //    {
                    //        depositCard.Balance += depositCard.Balance * depositCard.Rate - amount;
                    //        depositCard.TotalBalance = depositCard.Balance;
                    //    }
                    //}
                    //else
                    //{
                    //    depositCard.Rate = 0m;
                    //    depositCard.Balance -= (amount + amount * depositCard.Commission);
                    //    depositCard.TotalBalance = depositCard.TotalBalance;
                    //}
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
                            creditCard.OwnMoney -= amount;
                            creditCard.IsInLimit = creditCard.OwnMoney < 0;
                            if (creditCard.IsInLimit)
                            {
                                creditCard.LimitWithdrawn = DateTime.Now;
                            }
                            //var initBalance = creditCard.OwnMoney;
                            //if (!creditCard.IsInLimit)
                            //{
                            //    creditCard.OwnMoney -= amount;
                            //    creditCard.IsInLimit = true;
                            //    creditCard.LimitWithdrawn = DateTime.Now;
                            //}
                            //else if (creditCard.IsInLimit && DateTime.Now <= creditCard.EndLimit)
                            //{

                            //    creditCard.Limit = amount - creditCard.OwnMoney;
                            //    creditCard.OwnMoney = 0;
                            //}
                            //else if (creditCard.IsInLimit && DateTime.Now > creditCard.EndLimit)
                            //{

                            //    var days = DateTime.Now.Subtract((DateTime)creditCard.EndLimit).Days;
                            //    var percents = days * creditCard.PercentIfDelay * initBalance;
                            //    creditCard.Limit = amount - creditCard.OwnMoney - percents;
                            //    creditCard.OwnMoney = 0;
                            //}
                        }
                        else
                        {
                            transaction.Success = false;
                            await _context.SaveChangesAsync();
                            return new OkObjectResult(new { Ok = false, Errors = new[] { "Not enough money" } });
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
                return new OkObjectResult(new TransferMoney { Ok = true });
            }
            catch (Exception exc)
            {
                transaction.Success = false;
                await _context.SaveChangesAsync();
                return new OkObjectResult(new TransferMoney { Ok = false, Errors = new List<string> { exc.Message } });
            }

        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login()
        {
            var body = "";
            using (var stream = new StreamReader(HttpContext.Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            var myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(body);
            string cardNum = Convert.ToString(myObject.cardNum);
            string pin = Convert.ToString(myObject.pin);
            string hashedPin = pin; //ComputeSha256Hash(pin);
            Card card;
            if (_context.Card.FirstOrDefault(c => c.CardNum == cardNum) != null)
            {
                card = _context.Card.FirstOrDefault(c => c.CardNum == cardNum);
                if (card.Pin == pin)
                {
                    var now = DateTime.Now;
                    var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                    Token token = new Token
                    { CardNum = cardNum, CardToken = encodedJwt, Create = DateTime.Now };
                    _context.Token.Add(token);
                    _context.SaveChanges();
                    return new OkObjectResult(new Log { Ok = true, Token = encodedJwt });
                }
                return new OkObjectResult(new Log { Ok = false});
            }
            else
            {
                return new OkObjectResult(new Log { Ok = false });
            }
        }

        private bool IsAuthenticated(string cardNum)
        {
            string accessToken = Request.Headers["Authorization"][0];
            string[] parts = accessToken.Split(' ');
            if (parts[0] != "bearer")
            {
                return false;
            }
            Token token = _context.Token.FirstOrDefault(c => c.CardToken == parts[1]);
            if (token?.CardNum != cardNum)
            {
                return false;
            }
            if (AuthOptions.LIFETIME != 0 && (DateTime.Now - token.Create).TotalSeconds > AuthOptions.LIFETIME)
            {
                return false;
            }

            return true;
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}