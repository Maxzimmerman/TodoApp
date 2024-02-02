using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.ModelsIn.ViewModels;

namespace Todo.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetOne(int id);
        void AddAsync(RegisterViewModel registerViewModel);
        void Remove(T entity);
    }
}
