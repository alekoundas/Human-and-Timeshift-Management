using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Business.Repository.Interface;
using DataAccess.Models;
using Bussiness.Repository;

namespace Business.Repository
{
    public class EmployeeRepository : Repository<Employee> ,IEmployeeRepository
    {
        public EmployeeRepository(BaseDbContext dbContext) :base (dbContext)
        {
        }
    }
}
