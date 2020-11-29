using DataAccess;
using DataAccess.Models.Entity;
using System.Collections.Generic;

namespace Bussiness.Service.ExcelServiceWorkers
{
    public class WorkPlaceExcelWorker : ExcelService<WorkPlace>
    {
        private BaseDatawork _baseDatawork { get; }

        public WorkPlaceExcelWorker(BaseDbContext BaseDbContext) : base(BaseDbContext)
        {
            _baseDatawork = new BaseDatawork(BaseDbContext);
        }

        public void CompleteValidations() { }

        public WorkPlaceExcelWorker ValidateUnique<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var title = (exportedInstance.GetType().GetProperty("Title"))
                    .GetValue(exportedInstance);

                var customerId = (int?)(exportedInstance.GetType().GetProperty("CustomerId"))
                    .GetValue(exportedInstance);

                hasValidationError = _baseDatawork.WorkPlaces
                    .Any(x => x.Title.Trim() == title.ToString().Trim() && x.CustomerId == customerId);

            }
            if (hasValidationError)
                error = "Title,CustomerId";

            return this;
        }

        public WorkPlaceExcelWorker ValidateRequired<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var title = (exportedInstance.GetType().GetProperty("Title"))
                    .GetValue(exportedInstance);

                var customerId = (int?)(exportedInstance.GetType().GetProperty("CustomerId"))
                    .GetValue(exportedInstance);

                if (title == "" || customerId == 0)
                    hasValidationError = true;
            }
            if (hasValidationError)
                error = "Title,CustomerId";

            return this;
        }
    }
}
