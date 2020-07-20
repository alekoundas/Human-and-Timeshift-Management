using System;
using System.Collections.Generic;
using System.Text;
using Bussiness.Repository.Interface;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.RealWorkHours;

namespace Bussiness.Repository.Base.Interface
{
    public interface IRealWorkHourRepository : IBaseRepository<RealWorkHour>
    {
        public bool IsDateOverlaping(RealWorkHourApiViewModel realWorkHour, int employeeId);
    }
}
