using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Repository.Interface;
using DataAccess.Models.Entity;

namespace Business.Repository.Interface
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<List<Employee>> ProjectionDifference(
            Func<IQueryable<Employee>, IOrderedQueryable<Employee>> orderingInfo,
            DateTime startOn,
            DateTime endOn,
            int workPlaceId = 0,
            int pageSize = 10,
            int pageIndex = 1);
    }
}