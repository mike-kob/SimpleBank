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
            public string Token { get; set; }
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
                return new OkObjectResult(new BalanceCard { Ok = true, CardNum = id, Balance = card.TotalBalance });
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
                    card.Pin = ComputeSha256Hash(newPin);
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
                        return new OkObjectResult(new { Ok = true, Allowed = true, TxnId = transaction.TxnId });
                    }
                }

                if (_context.CreditCard.FirstOrDefault(c => c.CardNum == cardNum) != null)
                {
                    var card = await _context.CreditCard.FirstOrDefaultAsync(c => c.CardNum == cardNum);
                    var transaction = new Transaction
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
                        return new OkObjectResult(new { Ok = true, Allowed = true, transaction.TxnId });
                    }

                }

                if (_context.DepositCard.FirstOrDefault(c => c.CardNum == cardNum) != null)
                {
                    var card = _context.DepositCard.FirstOrDefault(c => c.CardNum == cardNum);
                    var transaction = new Transaction
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
                        return new OkObjectResult(new { Ok = true, Allowed = true, Errors = new[] { "Discarding all benefits" } });
                    }

                    if (card.TotalBalance < amount)
                    {
                        await ConfirmWithdraw(card.CardNum, transaction.TxnId, false);
                        return new OkObjectResult(new { Ok = true, Allowed = false, Errors = new[] { "Not enough money" } });
                    }

                    return new OkObjectResult(new { Ok = true, Allowed = true });

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
        public async Task<ActionResult> ConfirmWithdraw(string cardNum = null, int? txnId = null, bool success = false, int amount = 0, string errors = null)
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
                amount = Convert.ToInt32(myObject.amount);
                if (!IsAuthenticated(cardNum))
                {
                    return Unauthorized();
                }
            }

            try
            {
                var transaction = await _context.Transaction.FirstOrDefaultAsync(txn => txn.TxnId == txnId);
                if (transaction == null)
                {
                    return new OkObjectResult(new { Ok = false, Errors = new[] { "Not found" } });
                }
                transaction.Success = success;
                if (success)
                {
                    var card = await _context.Card.FirstOrDefaultAsync(c => c.CardNum == cardNum);
                    switch (card)
                    {
                        case CheckingCard checkingCard:
                            checkingCard.Balance -= amount;
                            break;
                        case DepositCard depositCard:
                            depositCard.Balance -= amount;
                            break;
                        case CreditCard creditCard:
                            creditCard.OwnMoney -= amount;
                            creditCard.IsInLimit = creditCard.OwnMoney < 0;
                            if (creditCard.IsInLimit)
                            {
                                creditCard.MinSum = -creditCard.OwnMoney;
                                creditCard.LimitWithdrawn = DateTime.Now;
                            }
                            break;
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                await _context.SaveChangesAsync();
                var conf = new ConfirmWithdrawResult { Ok = false, Errors = new List<string> { exc.Message } };
                return new OkObjectResult(conf);
            }
            return new OkObjectResult(new ConfirmWithdrawResult { Ok = false });


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
            { Amount = amount, DatetimeOfTxn = DateTime.Now, TypeOfTxn = 1 };
            await _context.SaveChangesAsync();
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
                    {
                        checkCard.Balance -= amount;
                    }
                    else
                    {
                        transaction.Success = false;
                        await _context.SaveChangesAsync();
                        return new OkObjectResult(new { Ok = false, Errors = new[] { "Not enough money" } });
                    }
                }
                else if (cardFrom is DepositCard)
                {
                    transaction.Success = false;
                    await _context.SaveChangesAsync();
                    return new OkObjectResult(new { Ok = false, Errors = new[] { "Cannot transfer from Deposit card" } });
                }
                else if (cardFrom is CreditCard creditCard)
                {
                    if (creditCard.Balance >= amount)
                    {
                        creditCard.OwnMoney -= amount;
                        creditCard.IsInLimit = creditCard.OwnMoney < 0;
                        if (creditCard.IsInLimit)
                        {
                            creditCard.MinSum = -creditCard.OwnMoney;
                            creditCard.LimitWithdrawn = DateTime.Now;
                        }
                    }
                    else
                    {
                        transaction.Success = false;
                        await _context.SaveChangesAsync();
                        return new OkObjectResult(new { Ok = false, Errors = new[] { "Not enough money" } });
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
            string hashedPin = ComputeSha256Hash(pin);
            Card card;
            if (_context.Card.FirstOrDefault(c => c.CardNum == cardNum) != null)
            {
                card = _context.Card.FirstOrDefault(c => c.CardNum == cardNum);
                if (card.Pin == hashedPin)
                {
                    var now = DateTime.Now;
                    //var jwt = new JwtSecurityToken(
                    //issuer: AuthOptions.ISSUER,
                    //audience: AuthOptions.AUDIENCE,
                    //notBefore: now,
                    //signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                    //var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                    var encodedJwt = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    Token token = new Token
                    { CardNum = cardNum, CardToken = encodedJwt, Create = DateTime.Now };
                    _context.Token.Add(token);
                    _context.SaveChanges();
                    return new OkObjectResult(new Log { Ok = true, Token = encodedJwt });
                }
                return new OkObjectResult(new Log { Ok = false });
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