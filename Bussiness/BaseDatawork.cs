using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Business.Repository;
using Business.Repository.Interface;
using Bussiness.Repository;
using Bussiness.Repository.Base;
using Bussiness.Repository.Base.Interface;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.Leaves;
using Microsoft.EntityFrameworkCore;

namespace Bussiness
{
    public class BaseDatawork : IBaseDatawork
    {
        private readonly BaseDbContext _dbcontext;


        public IEmployeeRepository Employees { get; private set; }
        public ISpecializationRepository Specializations { get; private set; }
        public ICompanyRepository Companies { get; private set; }
        public ICustomerRepository Customers { get; private set; }
        public IWorkplaceRepository WorkPlaces { get; private set; }
        public IWorkHourRepository WorkHours { get; private set; }
        public IRealWorkHourRepository RealWorkHours { get; private set; }
        public ITimeShiftRepository TimeShifts { get; private set; }
        public IEmployeeWorkPlaceRepository EmployeeWorkPlaces { get; private set; }
        public ILeaveRepository Leaves { get; private set; }
        public ILeaveTypeRepository LeaveTypes { get; private set; }

        public BaseDatawork(BaseDbContext baseDbContext)
        {
            _dbcontext = baseDbContext;
            Employees = new EmployeeRepository(_dbcontext);
            Specializations = new SpecializationRepository(_dbcontext);
            Companies = new CompanyRepository(_dbcontext);
            Customers = new CustomerRepository(_dbcontext);
            WorkPlaces = new WorkPlaceRepository(_dbcontext);
            WorkHours = new WorkHourRepository(_dbcontext);
            RealWorkHours = new RealWorkHourRepository(_dbcontext);
            TimeShifts = new TimeShiftRepository(_dbcontext);
            EmployeeWorkPlaces = new EmployeeWorkPlaceRepository(_dbcontext);
            Leaves = new LeaveRepository(_dbcontext);
            LeaveTypes = new LeaveTypeRepository(_dbcontext);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbcontext.SaveChangesAsync();
        }

        public void Update<TEntity>(TEntity model)
        {

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


            await _dbcontext.WorkHours.Include(x=>x.Employee)
                .Where(WorkHourFilter)
                .Where(x => x.Employee.Id == employeeId)?
                .ForEachAsync(y =>
                    response.Add(new ApiLeavesHasOverlapResponse
                    {
                        Id = y.Id,
                        EmployeeId =y.Employee.Id,
                        GivenEmployeeId = employeeId,
                        TypeOf = y.IsDayOff?"DayOff":"WorkHour"
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

        private Func<T, bool> GetDateOverlapFilter<T>(DateTime startOn, DateTime endOn) =>
             x =>
                 ((DateTime)x.GetType().GetProperty("StartOn").GetValue(x) <= startOn && startOn <= (DateTime)x.GetType().GetProperty("EndOn").GetValue(x)) ||
                     ((DateTime)x.GetType().GetProperty("StartOn").GetValue(x) <= endOn && endOn <= (DateTime)x.GetType().GetProperty("EndOn").GetValue(x)) ||
                     (startOn < (DateTime)x.GetType().GetProperty("StartOn").GetValue(x) && (DateTime)x.GetType().GetProperty("EndOn").GetValue(x) < endOn);

        private Func<T, bool> GetFilter<T>(int employeeId) =>
            x => (int)x.GetType().GetProperty("Employee").PropertyType.GetProperty("Id").GetValue(x) == employeeId;
    }
}
