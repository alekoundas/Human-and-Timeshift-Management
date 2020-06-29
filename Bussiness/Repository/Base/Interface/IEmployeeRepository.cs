using System;
using System.Collections.Generic;
using System.Text;
using Bussiness.Repository.Interface;
using DataAccess.Models.Entity;

namespace Business.Repository.Interface
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
    }
}