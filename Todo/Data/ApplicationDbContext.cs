﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.ModelsIn;

namespace Todo.Data
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
        public DbSet<UserProject> projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoEntry>().Property(m => m.ProjectId).IsRequired(false);
            base.OnModelCreating(modelBuilder);
        }
    }
}
