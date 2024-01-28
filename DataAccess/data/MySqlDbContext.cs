using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Todo.DataAccess.data
{
    public class MySqlDbContext : ApplicationDbContext
    {
        public MySqlDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
