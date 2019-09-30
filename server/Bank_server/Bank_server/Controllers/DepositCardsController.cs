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
    public class DepositCardsController : ControllerBase
    {
        private readonly Bank_serverContext _context;

        public DepositCardsController(Bank_serverContext context)
        {
            _context = context;
        }

        // GET: api/DepositCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepositCard>>> GetDepositCard()
        {
            return await _context.DepositCard.ToListAsync();
        }

        // GET: api/DepositCards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepositCard>> GetDepositCard(string id)
        {
            var depositCard = await _context.DepositCard.FindAsync(id);

            if (depositCard == null)
            {
                return NotFound();
            }

            return depositCard;
        }

        // PUT: api/DepositCards/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepositCard(string id, DepositCard depositCard)
        {
            if (id != depositCard.CardNum)
            {
                return BadRequest();
            }

            _context.Entry(depositCard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepositCardExists(id))
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

        // POST: api/DepositCards
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<DepositCard>> PostDepositCard(DepositCard depositCard)
        {
            _context.DepositCard.Add(depositCard);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DepositCardExists(depositCard.CardNum))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDepositCard", new { id = depositCard.CardNum }, depositCard);
        }

        // DELETE: api/DepositCards/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DepositCard>> DeleteDepositCard(string id)
        {
            var depositCard = await _context.DepositCard.FindAsync(id);
            if (depositCard == null)
            {
                return NotFound();
            }

            _context.DepositCard.Remove(depositCard);
            await _context.SaveChangesAsync();

            return depositCard;
        }

        private bool DepositCardExists(string id)
        {
            return _context.DepositCard.Any(e => e.CardNum == id);
        }
    }
}
