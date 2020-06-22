using System;
using System.Collections.Generic;
using System.Text;
using Business.Repository;
using Business.Repository.Interface;
using DataAccess;

namespace Bussiness
{
    public class BaseDatawork : IBaseDatawork
    {
        private readonly BaseDbContext _baseDbContext;


        public IEmployeeRepository Employees { get; private set; }

        public BaseDatawork(BaseDbContext baseDbContext)
        {
            _baseDbContext = baseDbContext;
            Employees = new EmployeeRepository(_baseDbContext);
        }
    }
}
