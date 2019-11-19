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
    [Route("api/atm")]

    [ApiController]
    public class AtmController : ControllerBase
    {
        private readonly BankServerContext _context;

        public AtmController(BankServerContext context)
        {
            _context = context;
        }


        [HttpGet("remaining/{id}")]
        public async Task<ActionResult> GetRemaining(string id)
        {
            var atm = await _context.Atm.SingleOrDefaultAsync(m => m.AtmId == Int32.Parse(id));

            if (atm != null)
            {
                return new OkObjectResult(new { Ok = true, Remaining = atm.RemainingMoney });
            }

            return new OkObjectResult(new { Ok = false });
        }
        
        [HttpPost]
        [Route("withdraw")]
        public async Task<ActionResult> SetWithdrawn()
        {
            string body;
            using (var stream = new StreamReader(HttpContext.Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            var myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(body);
            int atmId = Int32.Parse(myObject["atmId"]);
            int amount = Int32.Parse(myObject["amount"]);

            var atm = await _context.Atm.SingleOrDefaultAsync(m => m.AtmId == atmId);

            if (atm != null)
            {
                atm.RemainingMoney -= amount;
                await _context.SaveChangesAsync();
                return new OkObjectResult(new { Ok = true, Remaining = atm.RemainingMoney });
            }

            return new OkObjectResult(new { Ok = false });
        }

    }
}