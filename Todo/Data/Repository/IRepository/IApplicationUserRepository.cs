using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.ModelsIn;
using Todo.ModelsIn.ViewModels;

namespace Todo.Data.Repository.IRepository
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
