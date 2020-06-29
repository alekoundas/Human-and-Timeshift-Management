using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Business.Repository.Interface;
using DataAccess.Models;
using Bussiness.Repository;
using DataAccess.Models.Entity;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Business.Repository
{
    public class EmployeeRepository : BaseRepository<Employee> ,IEmployeeRepository
    {
        public EmployeeRepository(BaseDbContext dbContext) :base (dbContext)
        {
        }

        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }

      
    }
}
