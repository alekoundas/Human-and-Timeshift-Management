using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repository.Interface;
using DataAccess.Models.Datatable;
using DataAccess.Models.Entity;

namespace DataAccess.Repository.Interface
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<List<Employee>> ProjectionDifference(
           Func<IQueryable<Employee>, IOrderedQueryable<Employee>> orderingInfo,
            Datatable datatable,
            Expression<Func<Employee, bool>> filter,
            int pageSize = 10,
            int pageIndex = 1);
    }
}