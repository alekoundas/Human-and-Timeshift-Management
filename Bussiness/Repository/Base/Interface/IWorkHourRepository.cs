using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Repository.Interface;
using DataAccess.Models.Entity;

namespace Bussiness.Repository.Base.Interface
{
    public interface IWorkHourRepository : IBaseRepository<WorkHour>
    {
        Task<List<WorkHour>> GetCurrentAssignedOnCell(int timeShiftId, int year, int month, int day, int employeeId);
    }
}
