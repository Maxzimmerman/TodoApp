using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.Models;

namespace Todo.DataAccess.data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<TodoEntry> todos { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Priority> priorities { get; set; }
        public DbSet<ApplicationUser> users { get; set; }
        public DbSet<Project> projects { get; set; }
    }
}
