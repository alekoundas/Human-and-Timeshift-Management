using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Repository.Interface;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.RealWorkHours;
using DataAccess.ViewModels.WorkHours;

namespace Bussiness.Repository.Base.Interface
{
    public interface IRealWorkHourRepository : IBaseRepository<RealWorkHour>
    {
        public Task<List<RealWorkHour>> GetCurrentAssignedOnCell(int timeShiftId, int year, int month, int day, int employeeId);
        public Task<List<RealWorkHour>> GetCurrentAssignedOnCellFilterByEmployeeIds(GetForEditCellWorkHoursApiViewModel viewModel);
        public bool IsDateOverlaping(HasOverlapRangeWorkHoursApiViewModel workHour, int employeeId);
        public bool AreDatesOverlaping(ApiRealWorkHourHasOverlap realWorkHour, int employeeId);
        public bool AreDatesOverlapingLeaves(ApiRealWorkHourHasOverlap realWorkHour, int employeeId);
        public bool AreDatesOverlapingDayOff(ApiRealWorkHourHasOverlap realWorkHour, int employeeId);
        public Task<double> GetEmployeeTotalSecondsForDay(int employeeId, DateTime compareDate);
        public Task<double> GetEmployeeTotalSecondsForNight(int employeeId, DateTime compareDate);
        public Task<List<RealWorkHour>> GetCurrentAssignedOnCell(DateTime compareDate, int employeeId);
        public Task<double> GetEmployeeTotalSecondsFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
        public Task<double> GetEmployeeTotalSecondsDayFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
        public Task<double> GetEmployeeTotalSecondsNightFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0);
    }
}
