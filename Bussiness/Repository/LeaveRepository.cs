using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Repository.Base.Interface;
using DataAccess;
using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Bussiness.Repository
{
    public class LeaveRepository : BaseRepository<Leave>, ILeaveRepository
    {
        public LeaveRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }

        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }

        public  List<Leave> GetCurrentAssignedOnCell( int year, int month, int day, int employeeId)
        {
            return  Context.Leaves.Where(x =>
                   x.StartOn.Year == year &&
                   x.StartOn.Month == month &&
                   (x.StartOn.Day <= day && day <= x.EndOn.Day) &&
                   x.Employee.Id == employeeId)
                .ToList();
        }
    }
}
