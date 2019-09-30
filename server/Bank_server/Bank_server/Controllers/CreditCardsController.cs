using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bank_server.Models;

namespace Bank_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardsController : ControllerBase
    {
        private readonly Bank_serverContext _context;

        public CreditCardsController(Bank_serverContext context)
        {
            _context = context;
        }

        // GET: api/CreditCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreditCard>>> GetCreditCard()
        {
            return await _context.CreditCard.ToListAsync();
        }

        // GET: api/CreditCards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CreditCard>> GetCreditCard(string id)
        {
            var creditCard = await _context.CreditCard.FindAsync(id);

            if (creditCard == null)
            {
                return NotFound();
            }

            return creditCard;
        }

        // PUT: api/CreditCards/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCreditCard(string id, CreditCard creditCard)
        {
            if (id != creditCard.CardNum)
            {
                return BadRequest();
            }

            _context.Entry(creditCard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CreditCardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CreditCards
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<CreditCard>> PostCreditCard(CreditCard creditCard)
        {
            _context.CreditCard.Add(creditCard);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CreditCardExists(creditCard.CardNum))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCreditCard", new { id = creditCard.CardNum }, creditCard);
        }

        // DELETE: api/CreditCards/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CreditCard>> DeleteCreditCard(string id)
        {
            var creditCard = await _context.CreditCard.FindAsync(id);
            if (creditCard == null)
            {
                return NotFound();
            }

            _context.CreditCard.Remove(creditCard);
            await _context.SaveChangesAsync();

            return creditCard;
        }

        private bool CreditCardExists(string id)
        {
            return _context.CreditCard.Any(e => e.CardNum == id);
        }
    }
}
