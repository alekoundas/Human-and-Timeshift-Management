using DataAccess;
using DataAccess.Models.Entity;
using System.Collections.Generic;

namespace Bussiness.Service.ExcelServiceWorkers
{
    public class LeaveTypeExcelWorker : ExcelService<LeaveType>
    {
        private BaseDatawork _baseDatawork { get; }

        public LeaveTypeExcelWorker(BaseDbContext BaseDbContext) : base(BaseDbContext)
        {
            _baseDatawork = new BaseDatawork(BaseDbContext);
        }

        public void CompleteValidations() { }


        public LeaveTypeExcelWorker ValidateUnique<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var Name = (exportedInstance.GetType().GetProperty("Name"))
                    .GetValue(exportedInstance);

                hasValidationError = _baseDatawork.LeaveTypes
                    .Any(x => x.Name.Trim() == Name.ToString().Trim());
            }
            if (hasValidationError)
                error = "Name";

            return this;
        }

        public LeaveTypeExcelWorker ValidateRequired<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var Name = (exportedInstance.GetType().GetProperty("Name"))
                    .GetValue(exportedInstance);

                hasValidationError = string.IsNullOrEmpty((string)Name);
            }
            if (hasValidationError)
                error = "Name";

            return this;
        }
    }
}
