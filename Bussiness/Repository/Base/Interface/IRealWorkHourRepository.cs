using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Repository.Interface;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.RealWorkHours;

namespace Bussiness.Repository.Base.Interface
{
    public interface IRealWorkHourRepository : IBaseRepository<RealWorkHour>
    {
        public bool AreDatesOverlaping(ApiRealWorkHourHasOverlap realWorkHour, int employeeId);
        public bool AreDatesOverlapingLeaves(ApiRealWorkHourHasOverlap realWorkHour, int employeeId);
        public bool AreDatesOverlapingDayOff(ApiRealWorkHourHasOverlap realWorkHour, int employeeId);
    }
}
