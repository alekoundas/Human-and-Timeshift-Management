using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bussiness.Repository.Interface;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.RealWorkHours;
using DataAccess.ViewModels.WorkHours;

namespace Bussiness.Repository.Base.Interface
{
    public interface IRealWorkHourRepository : IBaseRepository<RealWorkHour>
    {
        Task<List<RealWorkHour>> GetCurrentAssignedOnCell(int timeShiftId, int year, int month, int day, int employeeId);
        Task<List<RealWorkHour>> GetCurrentAssignedOnCellFilterByEmployeeIds(GetForEditCellWorkHoursApiViewModel viewModel);
        public bool IsDateOverlaping(ApiRealWorkHoursHasOverlapRange workHour, int employeeId);
        bool IsDateOvertime(ApiRealWorkHoursHasOvertimeRange workHour, int employeeId);
        bool AreDatesOverlaping(DateTime startOn, DateTime endOn, int employeeId);
        bool AreDatesOverlapingLeaves(DateTime startOn, DateTime endOn, int employeeId);
        bool AreDatesOverlapingDayOff(DateTime startOn, DateTime endOn, bool isDayOff, int employeeId);
        Task<double> GetEmployeeTotalSecondsForDay(int employeeId, DateTime compareDate);
        Task<double> GetEmployeeTotalSecondsForNight(int employeeId, DateTime compareDate);
        Task<List<RealWorkHour>> GetCurrentAssignedOnCell(DateTime compareDate, int employeeId);
        Task<double> GetEmployeeTotalSecondsFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
        Task<double> GetEmployeeTotalSecondsDayFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
        Task<double> GetEmployeeTotalSecondsNightFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
        Task<double> GetEmployeeTotalSecondsSaturdayDayFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
        Task<double> GetEmployeeTotalSecondsSaturdayNightFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
        Task<double> GetEmployeeTotalSecondsSundayDayFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
        Task<double> GetEmployeeTotalSecondsSundayNightFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
    }
}
