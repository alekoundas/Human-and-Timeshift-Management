using DataAccess.Models.Entity;
using DataAccess.Repository.Interface;
using DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository.Base.Interface
{
    public interface IRealWorkHourRepository : IBaseRepository<RealWorkHour>
    {
        Task<List<RealWorkHour>> GetCurrentAssignedOnCell(int timeShiftId, int year, int month, int day, int employeeId);
        Task<List<RealWorkHour>> GetCurrentAssignedOnCellFilterByEmployeeIds(GetForEditCellWorkHoursApiViewModel viewModel);
        public bool IsDateOverlaping(ApiRealWorkHoursHasOverlapRange workHour, int employeeId);
        bool IsDateOvertime(ApiRealWorkHoursHasOvertimeRange workHour, int employeeId);
        bool AreDatesOverlaping(DateTime startOn, DateTime endOn, int employeeId);
        bool AreDatesOverlapingLeaves(DateTime startOn, DateTime endOn, int employeeId);
        double GetEmployeeTotalSecondsForDay(int employeeId, DateTime compareDate);
        double GetEmployeeTotalSecondsForNight(int employeeId, DateTime compareDate);
        Task<List<RealWorkHour>> GetCurrentAssignedOnCell(DateTime compareDate, int employeeId);
        double GetEmployeeTotalSecondsFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
        double GetEmployeeTotalSecondsDayFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
        double GetEmployeeTotalSecondsNightFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
        double GetEmployeeTotalSecondsSaturdayDayFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
        double GetEmployeeTotalSecondsSaturdayNightFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
        double GetEmployeeTotalSecondsSundayDayFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
        double GetEmployeeTotalSecondsSundayNightFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
    }
}
