using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data;
using Todo.Data.Repository.IRepository;
using Todo.ModelsIn.ViewModels;
using Todo.ModelsIn;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Todo.Data.Repository
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        private DbSet<ApplicationUser> _dbSet;
        private ApplicationDbContext context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public DbSet<ApplicationUser> DbSet
        {
            get { return _dbSet; }
            set { _dbSet = value; }
        }

        public ApplicationUserRepository(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager)
        {
            _db = applicationDbContext;
            this.DbSet = _db.Set<ApplicationUser>();
            _userManager = userManager;
        }

        public ApplicationUserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IdentityResult> AddAsync(RegisterViewModel registerViewModel)
        {
            var user = new ApplicationUser
            {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email,
                ApplicationUserName = registerViewModel.Name
            };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if(result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                return result;
            }
            else
            {
                return IdentityResult.Failed();
            }
        }

        public void Update(ApplicationUser applicationUser)
        {
            throw new NotImplementedException();
        }

        ApplicationUser IApplicationUserRepository.GetOne(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
