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
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var timeShiftId = (int)exportedInstance.GetType().GetProperty("TimeShiftId")
                    .GetValue(exportedInstance);

                var startOn = (DateTime)exportedInstance.GetType().GetProperty("StartOn")
                    .GetValue(exportedInstance);

                hasValidationError = !_baseDatawork.TimeShifts.Any(x =>
                    x.Id == timeShiftId &&
                    x.Month == startOn.Month &&
                    x.Year == startOn.Year);
            }
            if (hasValidationError)
                error = "StartOn";

            return this;
        }

        public WorkHourExcelWorker ValidateSameTimeshift<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            error = "";
            var hashTimeshiftIds = new HashSet<int>();
            foreach (var exportedInstance in exportedInstances)
            {
                var timeShiftId = (int)exportedInstance.GetType().GetProperty("TimeShiftId")
                    .GetValue(exportedInstance);

                hashTimeshiftIds.Add(timeShiftId);
            }

            if (hashTimeshiftIds.Count == 1)
                error = "TimeShiftId";

            return this;
        }

        public WorkHourExcelWorker ValidateEmployeeInTimeshift<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var timeShiftId = (int)exportedInstance.GetType().GetProperty("TimeShiftId")
                    .GetValue(exportedInstance);

                var employeeId = (int)exportedInstance.GetType().GetProperty("EmployeeId")
                    .GetValue(exportedInstance);

                hasValidationError = !_baseDatawork.TimeShifts.Any(x =>
                    x.Id == timeShiftId &&
                    x.WorkPlace.EmployeeWorkPlaces.Any(y => y.EmployeeId == employeeId));
            }
            if (hasValidationError)
                error = "EmployeeId";

            return this;
        }
    }
}
