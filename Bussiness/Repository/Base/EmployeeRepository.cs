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
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Business.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }

        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }
        public async Task<List<Employee>> ProjectionDifference(
       Func<IQueryable<Employee>, IOrderedQueryable<Employee>> orderingInfo,
       DateTime startOn,
       DateTime endOn,
       int pageSize = 10,
       int pageIndex = 1)
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();

            includes.Add(x => x.Include(y => y.RealWorkHours).ThenInclude(z => z.TimeShift));
            includes.Add(x => x.Include(y => y.WorkHours).ThenInclude(z => z.TimeShift));
            includes.Add(x => x.Include(y => y.Specialization));

            Expression<Func<Employee, bool>> filter = x =>
                    (x.RealWorkHours.Any(y =>
                          (y.StartOn <= startOn && startOn <= y.EndOn) ||
                          (y.StartOn <= endOn && endOn <= y.EndOn) ||
                          (startOn < y.StartOn && y.EndOn < endOn)) ||

                    (x.WorkHours.Any(y =>
                         (y.StartOn <= startOn && startOn <= y.EndOn) ||
                         (y.StartOn <= endOn && endOn <= y.EndOn) ||
                         (startOn < y.StartOn && y.EndOn < endOn)))
                 );

            IQueryable<RealWorkHour> realWorkHours = (IQueryable<RealWorkHour>)Context.RealWorkHours.Where(x =>
                        (x.StartOn <= startOn && startOn <= x.EndOn) ||
                        (x.StartOn <= endOn && endOn <= x.EndOn) ||
                        (startOn < x.StartOn && x.EndOn < endOn)
                        );

            IQueryable<WorkHour> workHours = (IQueryable<WorkHour>)Context.WorkHours.Where(x =>
                   ((x.StartOn <= startOn && startOn <= x.EndOn) ||
                   (x.StartOn <= endOn && endOn <= x.EndOn) ||
                   (startOn < x.StartOn && x.EndOn < endOn)) &&
                   x.IsDayOff == false
                   );

            var qry = (IQueryable<Employee>)Context.Employees;

            foreach (var include in includes)
                qry = include(qry);

            qry = qry.Where(filter);

            if (orderingInfo != null)
                qry = orderingInfo(qry);

            qry = qry.Select(x => new Employee
            {
                Id = x.Id,
                Afm = x.Afm,
                DateOfBirth = x.DateOfBirth,
                ErpCode = x.ErpCode,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                RealWorkHours = x.RealWorkHours.Where(y => !workHours.Any(z => z.StartOn == y.StartOn && z.EndOn == y.EndOn)).ToList(),
                WorkHours = x.WorkHours.Where(y => !realWorkHours.Any(z => z.StartOn == y.StartOn && z.EndOn == y.EndOn) && y.IsDayOff == false).ToList(),
                Specialization = x.Specialization
            })/*.Where(x => x.RealWorkHours.Any()&& x.WorkHours.Any() )*/;

            qry = qry.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return await qry.ToListAsync();
        }

    }
}
