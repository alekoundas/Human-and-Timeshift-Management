using DataAccess;
using DataAccess.Models.Entity;
using System.Collections.Generic;

namespace Bussiness.Service.ExcelServiceWorkers
{
    public class CustomerExcelWorker : ExcelService<Customer>
    {
        private BaseDatawork _baseDatawork { get; }

        public CustomerExcelWorker(BaseDbContext BaseDbContext) : base(BaseDbContext)
        {
            _baseDatawork = new BaseDatawork(BaseDbContext);
        }

        public void CompleteValidations() { }

        public CustomerExcelWorker ValidateUnique<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var VatNumber = (exportedInstance.GetType().GetProperty("VatNumber"))
                    .GetValue(exportedInstance);

                hasValidationError = _baseDatawork.Customers
                     .Any(x => x.VatNumber.Trim() == VatNumber.ToString().Trim());
            }
            if (hasValidationError)
                error = "VatNumber";

            return this;
        }

        public CustomerExcelWorker ValidateRequired<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var VatNumber = (exportedInstance.GetType().GetProperty("VatNumber"))
                    .GetValue(exportedInstance);
                var IdentifyingName = (exportedInstance.GetType().GetProperty("IdentifyingName"))
                   .GetValue(exportedInstance);

                hasValidationError = string.IsNullOrEmpty((string)VatNumber) || string.IsNullOrEmpty((string)IdentifyingName);
            }
            if (hasValidationError)
                error = "VatNumber,IdentifyingName";

            return this;
        }
    }
}
