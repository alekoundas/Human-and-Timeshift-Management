using DataAccess.Libraries.Datatable;
using DataAccess.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repository.Interface
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
    }
}
