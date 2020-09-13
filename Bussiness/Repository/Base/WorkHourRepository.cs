﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Repository.Base.Interface;
using DataAccess;
using DataAccess.Models.Datatable;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.WorkHours;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Bussiness.Repository.Base
{
    public class WorkHourRepository : BaseRepository<WorkHour>, IWorkHourRepository
    {
        public WorkHourRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }

        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }

        public List<WorkHour> GetCurrentAssignedOnCell(int timeShiftId, int? year, int? month, int day, int employeeId)
        {
            return Context.WorkHours.Where(x =>
                   x.TimeShiftId == timeShiftId &&
                   x.StartOn.Year == year &&
                   x.StartOn.Month == month &&
                   (x.StartOn.Day <= day && day <= x.EndOn.Day) &&
                   x.Employee.Id == employeeId)
                .ToList();
        }
        public async Task<List<WorkHour>> GetCurrentAssignedOnCellFilterByEmployeeIds(GetForEditCellWorkHoursApiViewModel viewModel)
        {
            var filter = PredicateBuilder.New<WorkHour>();

            foreach (var employeeId in viewModel.EmployeeIds)
                filter = filter.Or(x => x.EmployeeId == employeeId);

            return await Context.WorkHours.Where(filter)
                .Where(x =>
                   x.TimeShiftId == viewModel.TimeShiftId && (
                   x.StartOn.Day == viewModel.CellDay ||
                   x.EndOn.Day == viewModel.CellDay))
                .ToListAsync();
        }

        public bool IsDateOverlaping(WorkHoursApiViewModel workHour, int employeeId)
        {
            return Context.WorkHours.Where(x =>
                  (x.StartOn <= workHour.StartOn && workHour.StartOn <= x.EndOn) ||
                  (x.StartOn <= workHour.EndOn && workHour.EndOn <= x.EndOn))
                    .Any(y => y.Employee.Id == employeeId);
        }
        public bool IsDateOverlaping(HasOverlapRangeWorkHoursApiViewModel workHour, int employeeId)
        {
            if (workHour.IsEdit)
                return Context.WorkHours.Where(x =>
                (x.StartOn != workHour.ExcludeStartOn && x.EndOn != workHour.ExcludeEndOn))
                        .Where(x =>
                      (x.StartOn <= workHour.StartOn && workHour.StartOn <= x.EndOn) ||
                      (x.StartOn <= workHour.EndOn && workHour.EndOn <= x.EndOn) ||
                      (workHour.StartOn < x.StartOn && x.EndOn < workHour.EndOn))
                        .Any(y => y.Employee.Id == employeeId);
            else
                return Context.WorkHours.Where(x =>
                (x.StartOn <= workHour.StartOn && workHour.StartOn <= x.EndOn) ||
                (x.StartOn <= workHour.EndOn && workHour.EndOn <= x.EndOn) ||
                (workHour.StartOn < x.StartOn && x.EndOn < workHour.EndOn))
                  .Any(y => y.Employee.Id == employeeId);
        }

        public bool HasExactDate(WorkHoursApiViewModel workHour)
        {
            return Context.WorkHours.Any(x =>
                  x.TimeShiftId == workHour.TimeShiftId &&
                  x.StartOn == workHour.StartOn && workHour.EndOn == x.EndOn);
        }
      

    }
}
