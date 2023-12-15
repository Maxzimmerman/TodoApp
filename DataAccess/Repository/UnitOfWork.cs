using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.DataAccess.data;
using Todo.DataAccess.Repository.IRepository;

namespace Todo.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext _context;
        public IApplicationUserRepository _applicationUserRepository;
        public IApplicationUserRepository ApplicationUserRepository
        {
            get { return _applicationUserRepository; }
            set { _applicationUserRepository = value; }
        }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ApplicationUserRepository = new ApplicationUserRepository(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
