using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Repository.Interface;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.View;

namespace Bussiness.Repository.Base.Interface
{
   public interface IEmployeeWorkHourRepository: IBaseRepository<EmployeeWorkHour>
    {
        Task<bool> IsDateOverlaps(WorkHoursApiViewModel workHour);
    }
}
