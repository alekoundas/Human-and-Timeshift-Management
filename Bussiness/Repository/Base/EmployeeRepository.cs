using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Business.Repository.Interface;
using DataAccess.Models;
using Bussiness.Repository;
using DataAccess.Models.Entity;

namespace Business.Repository
{
    public class EmployeeRepository : BaseRepository<Employee> ,IEmployeeRepository
    {
        public EmployeeRepository(BaseDbContext dbContext) :base (dbContext)
        {
        }
        public BaseDbContext dbContext { get { return dbContext as BaseDbContext; } }

        
    }
}
