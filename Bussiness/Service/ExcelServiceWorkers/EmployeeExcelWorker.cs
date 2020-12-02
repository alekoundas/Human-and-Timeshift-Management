using DataAccess;
using DataAccess.Models.Entity;
using System;
using System.Collections.Generic;

namespace Bussiness.Service.ExcelServiceWorkers
{
    public class EmployeeExcelWorker : ExcelService<Employee>
    {
        private BaseDatawork _baseDatawork { get; }

        public EmployeeExcelWorker(BaseDbContext BaseDbContext) : base(BaseDbContext)
        {
            _baseDatawork = new BaseDatawork(BaseDbContext);
        }

        public void CompleteValidations() { }


        public EmployeeExcelWorker ValidateUnique<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var VatNumber = (exportedInstance.GetType().GetProperty("VatNumber"))
                    .GetValue(exportedInstance);

                hasValidationError = _baseDatawork.Employees
                     .Any(x => x.VatNumber.Trim() == VatNumber.ToString().Trim());
            }
            if (hasValidationError)
                error = "VatNumber";

            return this;
        }

        public EmployeeExcelWorker ValidateRequired<TEntity>(List<TEntity> exportedInstances, out string error)
        {
            var hasValidationError = false;
            error = "";
            foreach (var exportedInstance in exportedInstances)
            {
                var FirstName = (exportedInstance.GetType().GetProperty("FirstName"))
                    .GetValue(exportedInstance);
                var LastName = (exportedInstance.GetType().GetProperty("LastName"))
                   .GetValue(exportedInstance);
                var VatNumber = (exportedInstance.GetType().GetProperty("VatNumber"))
                   .GetValue(exportedInstance);
                var HireDate = (exportedInstance.GetType().GetProperty("HireDate"))
                   .GetValue(exportedInstance);

                hasValidationError = hasValidationError =
                    string.IsNullOrEmpty((string)FirstName) ||
                    string.IsNullOrEmpty((string)LastName) ||
                    string.IsNullOrEmpty((string)VatNumber) ||
                    (DateTime)HireDate == default(DateTime);
            }
            if (hasValidationError)
                error = "FirstName,LastName,VatNumber,HireDate";

            return this;
        }
    }
}
