using Bank_server.Models;
using BankServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BankServer.Controllers
{
    public class Withdraw
    {
        public long num { get; set; }
        public decimal amount { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly BankServerContext _context;

        public BankController(BankServerContext context)
        {
            _context = context;
        }


        [HttpGet("balance/{id}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<decimal>> GetCheckingCard(string id)
        {

            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var checkCard = await _context.CheckingCard
                    .SingleOrDefaultAsync(m => m.CardNum == Convert.ToInt64(id));

                return checkCard.Balance;
            }
            catch (Exception)
            {
                return Forbid();
            }
        }
        
        [HttpPost]
        [Route("~/api/СheckingWithdraw")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
                if (_context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum).Balance >= amount)
                {
                    _context.CheckingCard.FirstOrDefault(c => c.CardNum == cardNum).Balance -= amount;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Forbid();
                }
            }
            catch(ArgumentNullException)
            {
                return Unauthorized();
            }
            return Ok("allowed");
            //return await ConfirmWithdraw(cardNum, "finished");
        }

        [HttpPost]
        [Route("~/api/DepositWithdraw")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
                    if (DateTime.Now < card.EndDeposit)
                    {
                        card.Rate = 0m;
                        card.Commission = true;
                        card.Balance -= amount *= card.PercentIfWithdraw;
                    }
                    else
                    {
                        card.TotalBalance -= amount;
                    }
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
            catch (AccessViolationException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok("allowed");
            //return await ConfirmWithdraw(cardNum, "finished");
        }

        [HttpPost]
        [Route("~/api/CreditWithdraw")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DepositCard>> CreditWithdraw()
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
                if (card.OwnMoney >= amount)
                {
                    card.OwnMoney -= amount;
                }
                else if (card.Balance >= amount)
                {
                    var initBalance = card.OwnMoney;
                     if  (!card.IsInLimit)
                     {
                        card.Limit = amount - card.OwnMoney;
                        card.OwnMoney = 0;
                        card.IsInLimit = true;
                        card.IsLimitPaid = false;
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
            catch (AccessViolationException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok("allowed");
            //return await ConfirmWithdraw(cardNum, "finished");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<CheckingCard>> CheckingConfirmWithdraw(long cardNum, string finished, string errors = null)
        {
            ////?????
            if (finished.Equals("finished"))
                await _context.SaveChangesAsync();
            return Ok();
        }

        public async Task<ActionResult<DepositCard>> DepositConfirmWithdraw(long cardNum, string finished, string errors = null)
        {
            ////?????
            if (finished.Equals("finished"))
                await _context.SaveChangesAsync();
            return Ok();
        }
        public async Task<ActionResult<CreditCard>> CreditConfirmWithdraw(long cardNum, string finished, string errors = null)
        {
            ////?????
            if (finished.Equals("finished"))
                await _context.SaveChangesAsync();
            return Ok();
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

            return Ok();
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
                    if (DateTime.Now < card.EndDeposit)
                    {
                        card.Rate = 0m;
                        card.Commission = true;
                        card.Balance -= amount *= card.PercentIfWithdraw;
                    }
                    else
                    {
                        card.TotalBalance -= amount;
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

            return Ok();
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
                        card.IsLimitPaid = false;
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

            return Ok();
        }
    }
}
