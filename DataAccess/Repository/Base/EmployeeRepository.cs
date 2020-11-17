using DataAccess.Models.Datatable;
using DataAccess.Models.Entity;
using DataAccess.Repository.Interface;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repository.Base
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
            Datatable datatable,
            Expression<Func<Employee, bool>> filter,
            int pageSize = 10,
            int pageIndex = 1)
        {
            var filterRealWorkHours = PredicateBuilder.New<RealWorkHour>();
            var filterOrRealWorkHours = PredicateBuilder.New<RealWorkHour>();
            var filterWorkHours = PredicateBuilder.New<WorkHour>();
            var filterOrWorkHours = PredicateBuilder.New<WorkHour>();

            filterOrRealWorkHours = filterOrRealWorkHours.Or(x => x.StartOn <= datatable.StartOn && datatable.StartOn <= x.EndOn);
            filterOrRealWorkHours = filterOrRealWorkHours.Or(x => x.StartOn <= datatable.EndOn && datatable.EndOn <= x.EndOn);
            filterOrRealWorkHours = filterOrRealWorkHours.Or(x => datatable.StartOn < x.StartOn && x.EndOn < datatable.EndOn);
            filterRealWorkHours = filterRealWorkHours.And(filterOrRealWorkHours);

            filterOrWorkHours = filterOrWorkHours.Or(x => x.StartOn <= datatable.StartOn && datatable.StartOn <= x.EndOn);
            filterOrWorkHours = filterOrWorkHours.Or(x => x.StartOn <= datatable.EndOn && datatable.EndOn <= x.EndOn);
            filterOrWorkHours = filterOrWorkHours.Or(x => datatable.StartOn < x.StartOn && x.EndOn < datatable.EndOn);
            filterWorkHours = filterWorkHours.And(filterOrWorkHours);

            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();

            includes.Add(x => x.Include(y => y.RealWorkHours).ThenInclude(z => z.TimeShift));
            includes.Add(x => x.Include(y => y.WorkHours).ThenInclude(z => z.TimeShift));
            includes.Add(x => x.Include(y => y.Specialization));

            //filter = filter.And(x =>
            //x.RealWorkHours.Any(filterOrRealWorkHours) || x.WorkHours.Any(filterOrWorkHours));

            filter = filter.And(x =>
                 x.RealWorkHours.Any(y =>
                       (y.StartOn <= datatable.StartOn && datatable.StartOn <= y.EndOn) ||
                       (y.StartOn <= datatable.EndOn && datatable.EndOn <= y.EndOn) ||
                       (datatable.StartOn < y.StartOn && y.EndOn < datatable.EndOn)) ||

                x.WorkHours.Any(y =>
                      (y.StartOn <= datatable.StartOn && datatable.StartOn <= y.EndOn) ||
                      (y.StartOn <= datatable.EndOn && datatable.EndOn <= y.EndOn) ||
                      (datatable.StartOn < y.StartOn && y.EndOn < datatable.EndOn)));


            var realWorkHours = Context.RealWorkHours//.Include(x => x.TimeShift)
                .Where(filterRealWorkHours) as IQueryable<RealWorkHour>;

            var workHours = Context.WorkHours.Include(x => x.TimeShift)
                .Where(filterWorkHours)
                .Where(x => x.IsDayOff == false)
                as IQueryable<WorkHour>;


            if (datatable.GenericId != 0)//workplaceId
            {
                workHours = workHours.Where(x => x.TimeShift.WorkPlaceId == datatable.GenericId);
                realWorkHours = realWorkHours.Where(x => x.TimeShift.WorkPlaceId == datatable.GenericId);
                filter = filter.And(x => x.EmployeeWorkPlaces.Any(y => y.WorkPlaceId == datatable.GenericId));
            }

            var qry = (IQueryable<Employee>)Context.Employees;

            foreach (var include in includes)
                qry = include(qry);


            if (orderingInfo != null)
                qry = orderingInfo(qry);

            qry = qry.Where(filter);
            var DbF = Microsoft.EntityFrameworkCore.EF.Functions;
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
                        (z.StartOn <= datatable.StartOn && datatable.StartOn <= z.EndOn) ||
                        (z.StartOn <= datatable.EndOn && datatable.EndOn <= z.EndOn) ||
                        (datatable.StartOn < z.StartOn && z.EndOn < datatable.EndOn))
                           .Where(y => !workHours.Any(z => z.StartOn == y.StartOn && z.EndOn == y.EndOn))
                           .Where(y => !workHours.Any(z => Math.Abs(DbF.DateDiffMinute(z.StartOn, y.StartOn)) <= datatable.FilterByOffsetMinutes && Math.Abs(DbF.DateDiffMinute(z.EndOn, y.EndOn)) <= datatable.FilterByOffsetMinutes))
                           .ToList(),
                WorkHours = x.WorkHours.Where(z =>
                        (z.StartOn <= datatable.StartOn && datatable.StartOn <= z.EndOn) ||
                        (z.StartOn <= datatable.EndOn && datatable.EndOn <= z.EndOn) ||
                        (datatable.StartOn < z.StartOn && z.EndOn < datatable.EndOn))
                           .Where(y => !realWorkHours.Any(z => z.StartOn == y.StartOn && z.EndOn == y.EndOn) && y.IsDayOff == false)
                           .Where(y => !realWorkHours.Any(z => Math.Abs(DbF.DateDiffMinute(z.StartOn, y.StartOn)) <= datatable.FilterByOffsetMinutes && Math.Abs(DbF.DateDiffMinute(z.EndOn, y.EndOn)) <= datatable.FilterByOffsetMinutes))

                           //.Where(y =>
                           //{
                           //    return !realWorkHours.Any(z =>
                           //    {
                           //        var aaa = System.Math.Abs(DbF.DateDiffMinute(z.StartOn, y.StartOn)) <= datatable.FilterByOffsetMinutes && System.Math.Abs(DbF.DateDiffMinute(z.EndOn, y.EndOn)) <= datatable.FilterByOffsetMinutes;
                           //        return aaa;
                           //    });
                           //})
                           //.Where(y => !realWorkHours.Any(z => z.StartOn.Subtract(y.StartOn).TotalMinutes <= datatable.FilterByOffsetMinutes && z.EndOn.Subtract(y.EndOn).TotalMinutes <= datatable.FilterByOffsetMinutes))
                           .ToList(),
                Specialization = x.Specialization
            });
            //qry = qry.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return qry.ToList();
            //return await qry.ToListAsync();
        }


    }
}
