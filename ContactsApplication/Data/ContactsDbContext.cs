using ContactsApplication.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactsApplication.Data
{
    public class ContactsDbContext : DbContext
    {
        public ContactsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
