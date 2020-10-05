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
using LinqKit;

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
            Expression<Func<Employee, bool>> filter,
            int workPlaceId = 0,
            int pageSize = 10, 
            int pageIndex = 1)
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();

            includes.Add(x => x.Include(y => y.RealWorkHours).ThenInclude(z => z.TimeShift));
            includes.Add(x => x.Include(y => y.WorkHours).ThenInclude(z => z.TimeShift));
            includes.Add(x => x.Include(y => y.Specialization));

            filter = filter.And(x =>
                   x.RealWorkHours.Any(y =>
                         (y.StartOn <= startOn && startOn <= y.EndOn) ||
                         (y.StartOn <= endOn && endOn <= y.EndOn) ||
                         (startOn < y.StartOn && y.EndOn < endOn)) ||

                  x.WorkHours.Any(y =>
                        (y.StartOn <= startOn && startOn <= y.EndOn) ||
                        (y.StartOn <= endOn && endOn <= y.EndOn) ||
                        (startOn < y.StartOn && y.EndOn < endOn)));


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


            if (workPlaceId != 0)
            {
                workHours.Where(x => x.TimeShift.WorkPlaceId == workPlaceId);
                realWorkHours.Where(x => x.TimeShift.WorkPlaceId == workPlaceId);
                filter = filter.And(x => x.EmployeeWorkPlaces.Any(y => y.WorkPlaceId == workPlaceId));
            }

            var qry = (IQueryable<Employee>)Context.Employees;

            foreach (var include in includes)
                qry = include(qry);


            if (orderingInfo != null)
                qry = orderingInfo(qry);

            qry = qry.Where(filter);

            qry = qry.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            qry = qry.Select(x => new Employee
            {
                Id = x.Id,
                Afm = x.Afm,
                DateOfBirth = x.DateOfBirth,
                ErpCode = x.ErpCode,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                RealWorkHours = x.RealWorkHours.Where(z =>
                        (z.StartOn <= startOn && startOn <= z.EndOn) ||
                        (z.StartOn <= endOn && endOn <= z.EndOn) ||
                        (startOn < z.StartOn && z.EndOn < endOn))
                            .Where(y => !workHours.Any(z => z.StartOn == y.StartOn && z.EndOn == y.EndOn))
                            .ToList(),
                WorkHours = x.WorkHours.Where(z =>
                        (z.StartOn <= startOn && startOn <= z.EndOn) ||
                        (z.StartOn <= endOn && endOn <= z.EndOn) ||
                        (startOn < z.StartOn && z.EndOn < endOn))
                            .Where(y => !realWorkHours.Any(z => z.StartOn == y.StartOn && z.EndOn == y.EndOn) && y.IsDayOff == false)
                            .ToList(),
                Specialization = x.Specialization
            });


            return await qry.ToListAsync();
        }
      

    }
}
