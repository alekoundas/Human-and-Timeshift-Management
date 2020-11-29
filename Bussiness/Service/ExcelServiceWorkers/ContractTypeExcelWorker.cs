using DataAccess;
using DataAccess.Models.Entity;
using System.Collections.Generic;

namespace Bussiness.Service.ExcelServiceWorkers
{
    public class ContractTypeExcelWorker : ExcelService<ContractType>
    {
        private BaseDatawork _baseDatawork { get; }

        public ContractTypeExcelWorker(BaseDbContext BaseDbContext) : base(BaseDbContext)
        {
            _baseDatawork = new BaseDatawork(BaseDbContext);
        }

        public void CompleteValidations() { }


        public ContractTypeExcelWorker ValidateUnique<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var Name = (exportedInstance.GetType().GetProperty("Name"))
                    .GetValue(exportedInstance);

                hasValidationError = _baseDatawork.ContractTypes
                    .Any(x => x.Name.Trim() == Name.ToString().Trim());
            }
            if (hasValidationError)
                error = "Name";

            return this;
        }

        public ContractTypeExcelWorker ValidateRequired<TEntity>(List<TEntity> exportedInstances, out string error)
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
