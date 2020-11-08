using Bussiness.Repository.Interface;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bussiness.Repository.Base.Interface
{
    public interface IWorkHourRepository : IBaseRepository<WorkHour>
    {
        Task<List<WorkHour>> GetCurrentAssignedOnCell(int timeShiftId, int? year, int? month, int day, int employeeId);
        Task<List<WorkHour>> GetCurrentDayOffAssignedOnCell(DateTime compareDate, int employeeId);
        bool IsDateOverlaping(WorkHourApiViewModel workHour, int employeeId);
        bool IsDateOverlaping(ApiRealWorkHoursHasOverlapRange workHour, int employeeId);
        bool IsDateOvertime(WorkHourHasOvertimeRange workHour, int employeeId);
        bool HasExactDate(WorkHourApiViewModel workHour);
        Task<List<WorkHour>> GetCurrentAssignedOnCellFilterByEmployeeIds(GetForEditCellWorkHoursApiViewModel viewModel);
    }
}
