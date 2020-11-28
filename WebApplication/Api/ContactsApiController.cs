using Bussiness;
using DataAccess;
using DataAccess.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public ContactsApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // PUT: api/ContactsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(int id, Contact contact)
        {
            if (id != contact.Id)
                return BadRequest();

            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _baseDataWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/ContactsApi
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _baseDataWork.SaveChangesAsync();

            return CreatedAtAction("GetContact", new { id = contact.Id }, contact);
        }

        // DELETE: api/ContactsApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Contact>> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
                return NotFound();

            _context.Contacts.Remove(contact);
            await _baseDataWork.SaveChangesAsync();

            return contact;
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
