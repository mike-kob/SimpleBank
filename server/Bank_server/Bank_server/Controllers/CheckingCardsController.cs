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
    public class CheckingCardsController : ControllerBase
    {
        private readonly BankServerContext _context;

        public CheckingCardsController(BankServerContext context)
        {
            _context = context;
        }

        // GET: api/CheckingCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CheckingCard>>> GetCheckingCard()
        {
            return await _context.CheckingCard.ToListAsync();
        }

        // GET: api/CheckingCards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CheckingCard>> GetCheckingCard(string id)
        {
            var checkingCard = await _context.CheckingCard.FindAsync(id);

            if (checkingCard == null)
            {
                return NotFound();
            }

            return checkingCard;
        }

        // PUT: api/CheckingCards/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCheckingCard(string id, CheckingCard checkingCard)
        {
            if (id != checkingCard.CardNum)
            {
                return BadRequest();
            }

            _context.Entry(checkingCard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckingCardExists(id))
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

        // POST: api/CheckingCards
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<CheckingCard>> PostCheckingCard(CheckingCard checkingCard)
        {
            _context.CheckingCard.Add(checkingCard);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CheckingCardExists(checkingCard.CardNum))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCheckingCard", new { id = checkingCard.CardNum }, checkingCard);
        }

        // DELETE: api/CheckingCards/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CheckingCard>> DeleteCheckingCard(string id)
        {
            var checkingCard = await _context.CheckingCard.FindAsync(id);
            if (checkingCard == null)
            {
                return NotFound();
            }

            _context.CheckingCard.Remove(checkingCard);
            await _context.SaveChangesAsync();

            return checkingCard;
        }

        private bool CheckingCardExists(string id)
        {
            return _context.CheckingCard.Any(e => e.CardNum == id);
        }
    }
}
