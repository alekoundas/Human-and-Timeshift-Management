using DataAccess.Models.Entity;
using DataAccess.Repository.Base.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository.Base
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

        public async Task<List<Leave>> GetCurrentAssignedOnCell(int year, int month, int day, int employeeId)
        {
            return await Context.Leaves.Where(x =>
                  x.StartOn.Year == year &&
                  x.StartOn.Month == month &&
                  (x.StartOn.Day <= day && day <= x.EndOn.Day) &&
                  x.Employee.Id == employeeId)
                .ToListAsync();
        }
    }
}
