using DataAccess;
using DataAccess.Models.Entity;
using System.Collections.Generic;

namespace Bussiness.Service.ExcelServiceWorkers
{
    public class ContractExcelWorker : ExcelService<Contract>
    {
        private BaseDatawork _baseDatawork { get; }

        public ContractExcelWorker(BaseDbContext BaseDbContext) : base(BaseDbContext)
        {
            _baseDatawork = new BaseDatawork(BaseDbContext);
        }

        public void CompleteValidations() { }


        public ContractExcelWorker ValidateUnique<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var title = (exportedInstance.GetType().GetProperty("Title"))
                    .GetValue(exportedInstance);

                hasValidationError = _baseDatawork.Contracts
                    .Any(x => x.Title.Trim() == title.ToString().Trim());
            }
            if (hasValidationError)
                error = "Title";

            return this;
        }

        public ContractExcelWorker ValidateRequired<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var Title = (exportedInstance.GetType().GetProperty("Title"))
                    .GetValue(exportedInstance);

                hasValidationError = string.IsNullOrEmpty((string)Title);
            }
            if (hasValidationError)
                error = "Title";

            return this;
        }
    }
}
