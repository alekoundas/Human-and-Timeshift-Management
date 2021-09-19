using DataAccess;
using DataAccess.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bussiness.Service.ExcelServiceWorkers
{
    public class WorkHourExcelWorker : ExcelService<RealWorkHour>
    {
        private BaseDatawork _baseDatawork { get; }

        public WorkHourExcelWorker(BaseDbContext BaseDbContext) : base(BaseDbContext)
        {
            _baseDatawork = new BaseDatawork(BaseDbContext);
        }

        public void CompleteValidations() { }


        public WorkHourExcelWorker ValidateHoursInTimeshift<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var delegateGetter_StartOn = (Func<TEntity, DateTime>)Delegate.CreateDelegate(typeof(Func<TEntity, DateTime>), null, typeof(TEntity).GetProperty("StartOn").GetGetMethod());
            var delegateGetter_TimeShiftId = (Func<TEntity, int>)Delegate.CreateDelegate(typeof(Func<TEntity, int>), null, typeof(TEntity).GetProperty("TimeShiftId").GetGetMethod());

            error = "";
            var timeShift = new TimeShift();
            foreach (var exportedInstance in exportedInstances)
            {
                var timeShiftId = delegateGetter_TimeShiftId(exportedInstance);
                var startOn = delegateGetter_StartOn(exportedInstance);

                if (timeShift.Id == 0)
                    timeShift = _baseDatawork.TimeShifts.Query.First(x => x.Id == timeShiftId);

                if (startOn.Month != timeShift.Month ||
                    startOn.Year != timeShift.Year ||
                    timeShiftId != timeShift.Id)
                {

                    error = "StartOn";
                    return this;
                }
            }

            return this;
        }

        public WorkHourExcelWorker ValidateSameTimeshift<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var delegateGetter_TimeShiftId = (Func<TEntity, int>)Delegate.CreateDelegate(typeof(Func<TEntity, int>), null, typeof(TEntity).GetProperty("TimeShiftId").GetGetMethod());


            error = "";
            var hashTimeshiftIds = new HashSet<int>();
            foreach (var exportedInstance in exportedInstances)
            {
                var timeShiftId = delegateGetter_TimeShiftId(exportedInstance);
                hashTimeshiftIds.Add(timeShiftId);
            }

            if (hashTimeshiftIds.Count != 1)
                error = "TimeShiftId";

            return this;
        }

        public WorkHourExcelWorker ValidateEmployeeInTimeshift<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var delegateGetter_TimeShiftId = (Func<TEntity, int>)Delegate.CreateDelegate(typeof(Func<TEntity, int>), null, typeof(TEntity).GetProperty("TimeShiftId").GetGetMethod());
            var delegateGetter_EmployeeId = (Func<TEntity, int>)Delegate.CreateDelegate(typeof(Func<TEntity, int>), null, typeof(TEntity).GetProperty("EmployeeId").GetGetMethod());

            var timeshiftId = delegateGetter_TimeShiftId(exportedInstances[1]);
            var hasValidationError = false;
            var employeeIds = new List<int>();
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {

                var employeeId = delegateGetter_EmployeeId(exportedInstance);
                employeeIds.Add(employeeId);

            }

            employeeIds = employeeIds.Distinct().ToList();


            foreach (var id in employeeIds)
            {
                if (!_baseDatawork.Employees.Any(x => x.Id == id && x.EmployeeWorkPlaces.Any(y => y.WorkPlace.TimeShifts.Any(z => z.Id == timeshiftId))))
                {
                    error = "EmployeeId";
                    return this;
                }
            }


            //hasValidationError =!_baseDatawork.Employees.Query
            //    .Where(x => x.EmployeeWorkPlaces.Any(y => y.WorkPlace.TimeShifts.Any(z => z.Id == timeshiftId)))
            //    .All(x => employeeIds.Contains(x.Id));

            //if (hasValidationError)
            //error = "EmployeeId";

            return this;
        }
    }
}
