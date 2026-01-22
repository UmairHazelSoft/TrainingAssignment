using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Training_Assignment.Models;

namespace Training_Assignment.Data
{
    /// <summary>
    /// Entity Framework Core database context.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Users table.
        /// </summary>
        public DbSet<User> Users { get; set; }
    }
}
