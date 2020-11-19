using DataAccess.Models.Entity;
using DataAccess.Repository.Base;
using DataAccess.Repository.Base.Interface;
using DataAccess.Repository.Interface;
using DataAccess.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BaseDatawork : IBaseDatawork
    {
        private readonly BaseDbContext _dbcontext;


        public ILeaveRepository Leaves { get; private set; }
        public ICompanyRepository Companies { get; private set; }
        public IWorkHourRepository WorkHours { get; private set; }
        public ICustomerRepository Customers { get; private set; }
        public IEmployeeRepository Employees { get; private set; }
        public IWorkplaceRepository WorkPlaces { get; private set; }
        public ITimeShiftRepository TimeShifts { get; private set; }
        public ILeaveTypeRepository LeaveTypes { get; private set; }
        public IRealWorkHourRepository RealWorkHours { get; private set; }
        public ISpecializationRepository Specializations { get; private set; }
        public IHourRestrictionRepository HourRestrictions { get; private set; }
        public IEmployeeWorkPlaceRepository EmployeeWorkPlaces { get; private set; }
        public IWorkPlaceHourRestrictionRepository WorkPlaceHourRestrictions { get; private set; }

        public BaseDatawork(BaseDbContext baseDbContext)
        {
            _dbcontext = baseDbContext;
            Leaves = new LeaveRepository(_dbcontext);
            Companies = new CompanyRepository(_dbcontext);
            Customers = new CustomerRepository(_dbcontext);
            Employees = new EmployeeRepository(_dbcontext);
            WorkHours = new WorkHourRepository(_dbcontext);
            WorkPlaces = new WorkPlaceRepository(_dbcontext);
            LeaveTypes = new LeaveTypeRepository(_dbcontext);
            TimeShifts = new TimeShiftRepository(_dbcontext);
            RealWorkHours = new RealWorkHourRepository(_dbcontext);
            Specializations = new SpecializationRepository(_dbcontext);
            HourRestrictions = new HourRestrictionRepository(_dbcontext);
            EmployeeWorkPlaces = new EmployeeWorkPlaceRepository(_dbcontext);
            WorkPlaceHourRestrictions = new WorkPlaceHourRestrictionRepository(_dbcontext);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbcontext.SaveChangesAsync();
        }

        public void Update<TEntity>(TEntity model)
        {
            _dbcontext.Entry(model).State = EntityState.Modified;
        }

        public void UpdateRange<TEntity>(List<TEntity> models)
        {
            foreach (var model in models)
                _dbcontext.Entry(model).State = EntityState.Modified;
        }

        public async Task<List<ApiLeavesHasOverlapResponse>> DateHasOverlap(
            DateTime startOn, DateTime endOn, int employeeId)
        {
            var response = new List<ApiLeavesHasOverlapResponse>();
            Expression<Func<Leave, bool>> leaveFilter = x =>
                  ((x.StartOn <= startOn && startOn <= x.EndOn) ||
                      (x.StartOn <= endOn && endOn <= x.EndOn) ||
                      (startOn < x.StartOn && x.EndOn < endOn));

            Expression<Func<WorkHour, bool>> WorkHourFilter = x =>
                ((x.StartOn <= startOn && startOn <= x.EndOn) ||
                (x.StartOn <= endOn && endOn <= x.EndOn) ||
                (startOn < x.StartOn && x.EndOn < endOn));


            await _dbcontext.WorkHours.Include(x => x.Employee)
                .Where(WorkHourFilter)
                .Where(x => x.Employee.Id == employeeId)?
                .ForEachAsync(y =>
                    response.Add(new ApiLeavesHasOverlapResponse
                    {
                        Id = y.Id,
                        EmployeeId = y.Employee.Id,
                        GivenEmployeeId = employeeId,
                        TypeOf = y.IsDayOff ? "DayOff" : "WorkHour"
                    })
                );

            await _dbcontext.Leaves.Include(x => x.Employee)
                .Where(leaveFilter)
                .Where(x => x.Employee.Id == employeeId)?
                .ForEachAsync(y =>
                    response.Add(new ApiLeavesHasOverlapResponse
                    {
                        Id = y.Id,
                        EmployeeId = y.Employee.Id,
                        GivenEmployeeId = employeeId,
                        TypeOf = "Leave"
                    })
                );

            return response;
        }
    }
}
