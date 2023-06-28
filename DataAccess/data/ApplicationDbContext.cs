using Microsoft.EntityFrameworkCore;
using Todo.Models;

namespace Todo.DataAccess.data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<TodoEntry> todos { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Priority> priorities { get; set; }
    }
}
