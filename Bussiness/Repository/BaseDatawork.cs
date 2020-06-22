using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using DataAccess.Models;
using Business.Repository.Interface;

namespace Business.Repository
{
    public class BaseDatawork: IBaseDatawork
    {
        public IEmployeeRepository Employees { get; private set; }
        public BaseDatawork(BaseDbContext dbContext)
        {
            Employees = new EmployeeRepository(dbContext);
        }
    }
}

