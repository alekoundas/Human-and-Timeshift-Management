using DataAccess;
using DataAccess.Models.Entity;
using System.Collections.Generic;

namespace Bussiness.Service.ExcelServiceWorkers
{
    public class SpecializationExcelWorker : ExcelService<Specialization>
    {
        private BaseDatawork _baseDatawork { get; }

        public SpecializationExcelWorker(BaseDbContext BaseDbContext) : base(BaseDbContext)
        {
            _baseDatawork = new BaseDatawork(BaseDbContext);
        }

        public void CompleteValidations() { }


        public SpecializationExcelWorker ValidateUnique<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var Name = (exportedInstance.GetType().GetProperty("Name"))
                    .GetValue(exportedInstance);

                hasValidationError = _baseDatawork.Specializations
                    .Any(x => x.Name.Trim() == Name.ToString().Trim());
            }
            if (hasValidationError)
                error = "Name";

            return this;
        }

        public SpecializationExcelWorker ValidateRequired<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var Name = (exportedInstance.GetType().GetProperty("Name"))
                    .GetValue(exportedInstance);

                hasValidationError = (string)Name == "";
            }
            if (hasValidationError)
                error = "Name";

            return this;
        }
    }
}
