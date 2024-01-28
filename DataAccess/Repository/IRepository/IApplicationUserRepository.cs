using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Models;
using Todo.Models.ViewModels;

namespace Todo.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository
    {
        void Update(ApplicationUser applicationUser);
        ApplicationUser GetOne(int id);
        async Task<IdentityResult> AddAsync(RegisterViewModel registerViewModel)
        {
            throw new Exception();
        }
        void Remove(ApplicationUser entity);
    }
}
