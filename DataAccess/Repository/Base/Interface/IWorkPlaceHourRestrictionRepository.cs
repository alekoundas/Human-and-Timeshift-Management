using DataAccess.Repository.Interface;
using DataAccess.Models.Entity;

namespace DataAccess.Repository.Base.Interface
{
    public interface IWorkPlaceHourRestrictionRepository : IBaseRepository<WorkPlaceHourRestriction>
    {
        bool ValidateMaxHours(int timeShiftId, int year, int month, int day, double secondsToSubmit);
        string GetDayMaxTime(int timeShiftId, int year, int month, int day);
        string GetDayRealWorkHoursTime(int timeShiftId, int year, int month, int day);
    }
}
