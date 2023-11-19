using ContactsApplication.Data;
using ContactsApplication.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        public readonly ContactsDbContext contactsDbContext;

        public ContactsController(ContactsDbContext contactsDbContext)
        {
            this.contactsDbContext = contactsDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            // Get the contacts from the database
            return Ok(await contactsDbContext.Contacts.ToListAsync());
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetContactById")]
        public async Task<IActionResult> GetContactById([FromRoute] Guid id)
        {
            //await contactsDbContext.Contacts.FirstOrDefaultAsync(x => x.Id == id);
            var contact = await contactsDbContext.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(Contact contact)
        {
            contact.Id = Guid.NewGuid();

            await contactsDbContext.Contacts.AddAsync(contact);
            await contactsDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContactById), new { id = contact.Id }, contact);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, [FromBody] Contact updatedContact)
        {
            var existingContact = await contactsDbContext.Contacts.FindAsync(id);
            if (existingContact == null)
            {
                return NotFound();
            }
            existingContact.Name = updatedContact.Name;
            existingContact.Phone = updatedContact.Phone;
            existingContact.JobTitle = updatedContact.JobTitle;
            existingContact.BirthDate = updatedContact.BirthDate;

            await contactsDbContext.SaveChangesAsync();

            return Ok(existingContact);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var existingContact = await contactsDbContext.Contacts.FindAsync(id);
            if (existingContact == null)
            {
                return NotFound();
            }

            contactsDbContext.Contacts.Remove(existingContact);
            await contactsDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
