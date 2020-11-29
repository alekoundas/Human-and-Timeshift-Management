using DataAccess;
using DataAccess.Models.Entity;
using System.Collections.Generic;

namespace Bussiness.Service.ExcelServiceWorkers
{
    public class CompanyExcelWorker : ExcelService<Company>
    {
        private BaseDatawork _baseDatawork { get; }

        public CompanyExcelWorker(BaseDbContext BaseDbContext) : base(BaseDbContext)
        {
            _baseDatawork = new BaseDatawork(BaseDbContext);
        }

        public void CompleteValidations() { }


        public CompanyExcelWorker ValidateUnique<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var VatNumber = (exportedInstance.GetType().GetProperty("VatNumber"))
                    .GetValue(exportedInstance);

                hasValidationError = _baseDatawork.Companies
                     .Any(x => x.VatNumber.Trim() == VatNumber.ToString().Trim());
            }
            if (hasValidationError)
                error = "VatNumber";

            return this;
        }

        public CompanyExcelWorker ValidateRequired<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var VatNumber = (exportedInstance.GetType().GetProperty("VatNumber"))
                    .GetValue(exportedInstance);

                hasValidationError = (int)VatNumber == 0;
            }
            if (hasValidationError)
                error = "VatNumber";

            return this;
        }
    }
}
