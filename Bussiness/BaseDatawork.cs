using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Business.Repository;
using Business.Repository.Interface;
using Bussiness.Repository.Base;
using Bussiness.Repository.Base.Interface;
using DataAccess;
using DataAccess.Models.Entity;
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
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbcontext.SaveChangesAsync();
        }

        public void Update<TEntity>(TEntity model)
        {

            _dbcontext.Entry(model).State = EntityState.Modified;
        }


    }
}
