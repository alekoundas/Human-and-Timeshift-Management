using Bussiness.Service.ExcelServiceWorkers;
using DataAccess;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Bussiness.Service
{
    public class ExcelService<TEntity>
    {
        private BaseDatawork _baseDataWork;
        private BaseDbContext _baseDbContext;
        private List<string> Errors;
        private ExcelWorksheet _worksheet;

        private ExcelPackage ExcelPackage { get; set; }

        private List<TEntity> _exportedInstances;
        public ExcelService(BaseDbContext BaseDbContext)
        {
            this.Errors = new List<string>();
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _baseDbContext = BaseDbContext;
        }

        public ExcelService<TEntity> CreateNewExcel(string fileName)
        {

            this.ExcelPackage = new ExcelPackage();
            this.ExcelPackage.Workbook.Properties.Author = "PureMethod";
            this.ExcelPackage.Workbook.Properties.Title = fileName;
            this.ExcelPackage.Workbook.Properties.Subject = fileName + "δεδομένα απο την βάση";
            this.ExcelPackage.Workbook.Properties.Created = DateTime.Now;

            return this;
        }

        public async Task<ExcelService<TEntity>> AddSheetAsync(List<string> colTitles, string dataWorkEntity = null)
        {
            var colCount = 1;
            var rowCount = 1;
            _worksheet = this.ExcelPackage.Workbook.Worksheets.Add(nameof(TEntity).ToString());

            //Add the headers
            foreach (var colTitle in colTitles)
            {
                _worksheet.Cells[rowCount, colCount++].Value = colTitle;
                if (colTitle.EndsWith("Id"))
                    await GetLookUpValueAsync(colTitle, colCount - 1);
                else if (colTitle.Contains("IsActive"))
                    GetIsActive(colTitle, colCount - 1);
            }

            if (dataWorkEntity != null)
            {

                //ex _dataWork.propertyValue
                var propertyValue = _baseDataWork
                    .GetType()
                    .GetProperty(dataWorkEntity)
                    .GetValue(_baseDataWork);

                //ex _dataWork.propertyValue.GetAllAsync();
                var method = (Task)propertyValue
                    .GetType()
                    .GetMethod("GetAllAsync")
                    .Invoke(propertyValue, null);

                //ex await _dataWork.propertyValue.GetAllAsync();
                await method.ConfigureAwait(false);


                //ex var results = await _dataWork.propertyValue.GetAllAsync();
                List<TEntity> results = (List<TEntity>)method
                    .GetType()
                    .GetProperty("Result")
                    .GetValue(method);

                //Add rows with data (if any)
                foreach (var result in results)
                {
                    rowCount++;
                    colCount = 1;
                    foreach (var colTitle in colTitles)
                        _worksheet.Cells[rowCount, colCount++].Value =
                            result.GetType().GetProperty(colTitle).GetValue(result);
                }
            }

            //worksheet.HeaderFooter.OddFooter.InsertPicture(
            // new FileInfo(Path.Combine("wwwroot","img", "Leave.png")),
            // PictureAlignment.Right);

            //Make sheet cells even
            _worksheet.Cells[1, 1, rowCount, colCount].AutoFitColumns();
            return this;
        }

        public ExcelPackage CompleteExcel(out List<string> Errors)
        {
            Errors = this.Errors;
            return this.ExcelPackage;
        }



        public List<TEntity> RetrieveExtractedData(out List<string> Errors)
        {
            Errors = this.Errors;
            return _exportedInstances;
        }

        public async Task<ExcelService<TEntity>> ExtractDataFromExcel(IFormFile ImportExcel)
        {
            _exportedInstances = new List<TEntity>();
            var genericEntityInstance = (TEntity)Activator.CreateInstance(typeof(TEntity));
            using (MemoryStream stream = new MemoryStream())
            {
                await ImportExcel.CopyToAsync(stream);
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {

                    var worksheet = excelPackage.Workbook.Worksheets[0];
                    //foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        genericEntityInstance = (TEntity)Activator.CreateInstance(typeof(TEntity));

                        //TODO:convert to arrow
                        void AddPropertyValueToInstance<TValue>(TEntity instance, TValue value, string propertyName)
                        {
                            instance.GetType().GetProperty(propertyName).SetValue(instance, value, null);
                        }



                        for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                        {

                            var propertyName = worksheet.Cells[1, j].Value.ToString();

                            if (worksheet.Cells[1, j].Value.ToString().EndsWith("Id"))//Filter relation
                            {
                                if (await GetIdForExtractedEntity(worksheet, i, j) != 0)
                                {
                                    var propertyValue = await GetIdForExtractedEntity(worksheet, i, j);

                                    AddPropertyValueToInstance(genericEntityInstance, propertyValue, propertyName);
                                }
                            }
                            else if (worksheet.Cells[1, j].Value.ToString().Contains("IsActive"))
                            {
                                var propertyValue = GetBoolValue(worksheet, i, j);

                                AddPropertyValueToInstance(genericEntityInstance, propertyValue, propertyName);
                            }
                            else if (genericEntityInstance.GetType().GetProperty(worksheet.Cells[1, j].Value.ToString()).PropertyType.Name == "DateTime")
                            {
                                var value = worksheet.Cells[i, j].Value?.ToString();
                                var setValue = default(DateTime);

                                if (value?.Length > 0)
                                    DateTime.TryParse(value, out setValue);

                                AddPropertyValueToInstance(genericEntityInstance, setValue, propertyName);
                            }
                            else if (genericEntityInstance.GetType().GetProperty(worksheet.Cells[1, j].Value.ToString()).PropertyType.Name == "Int32")
                            {
                                var value = worksheet.Cells[i, j].Value?.ToString();
                                var setValue = default(Int32);

                                if (value?.Length > 0)
                                    Int32.TryParse(value, out setValue);

                                AddPropertyValueToInstance(genericEntityInstance, setValue, propertyName);
                            }
                            else if (genericEntityInstance.GetType().GetProperty(worksheet.Cells[1, j].Value.ToString()).PropertyType.Name == "Decimal")
                            {
                                var value = worksheet.Cells[i, j].Value?.ToString();
                                var setValue = default(Decimal);

                                if (value?.Length > 0)
                                    Decimal.TryParse(value, out setValue);

                                AddPropertyValueToInstance(genericEntityInstance, setValue, propertyName);
                            }
                            else if (genericEntityInstance.GetType().GetProperty(worksheet.Cells[1, j].Value.ToString()).PropertyType.Name == "String")
                                AddPropertyValueToInstance(genericEntityInstance, worksheet.Cells[i, j].Value?.ToString(), propertyName);
                        }

                        //Add Audit fields non existant in excel column
                        AddPropertyValueToInstance(genericEntityInstance, DateTime.Now, "CreatedOn");
                        AddPropertyValueToInstance(genericEntityInstance, HttpAccessorService.GetLoggeInUser_FullName, "CreatedBy_FullName");
                        AddPropertyValueToInstance(genericEntityInstance, HttpAccessorService.GetLoggeInUser_Id, "CreatedBy_Id");

                        _exportedInstances.Add(genericEntityInstance);
                    }
                }
            }
            return this;
        }

        public ExcelService<TEntity> ValidateExtractedData()
        {
            string uniqueError = "";
            string requiredError = "";
            switch (typeof(TEntity).Name)
            {
                case "Company":
                    new CompanyExcelWorker(_baseDbContext)
                      .ValidateUnique(_exportedInstances, out uniqueError)
                      .ValidateRequired(_exportedInstances, out requiredError)
                      .CompleteValidations();
                    break;
                case "Customer":
                    new CustomerExcelWorker(_baseDbContext)
                       .ValidateUnique(_exportedInstances, out uniqueError)
                       .ValidateRequired(_exportedInstances, out requiredError)
                       .CompleteValidations();
                    break;
                case "WorkPlace":
                    new WorkPlaceExcelWorker(_baseDbContext)
                        .ValidateUnique(_exportedInstances, out uniqueError)
                        .ValidateRequired(_exportedInstances, out requiredError)
                        .CompleteValidations();
                    break;
                case "Employee":
                    new EmployeeExcelWorker(_baseDbContext)
                        .ValidateUnique(_exportedInstances, out uniqueError)
                        .ValidateRequired(_exportedInstances, out requiredError)
                        .CompleteValidations();
                    break;
                case "Specialization":
                    new SpecializationExcelWorker(_baseDbContext)
                        .ValidateUnique(_exportedInstances, out uniqueError)
                        .ValidateRequired(_exportedInstances, out requiredError)
                        .CompleteValidations();
                    break;
                case "LeaveType":
                    new LeaveTypeExcelWorker(_baseDbContext)
                        .ValidateUnique(_exportedInstances, out uniqueError)
                        .ValidateRequired(_exportedInstances, out requiredError)
                        .CompleteValidations();
                    break;
                case "Contract":
                    new ContractExcelWorker(_baseDbContext)
                        .ValidateUnique(_exportedInstances, out uniqueError)
                        .ValidateRequired(_exportedInstances, out requiredError)
                        .CompleteValidations();
                    break;
                case "ContractType":
                    new ContractTypeExcelWorker(_baseDbContext)
                        .ValidateUnique(_exportedInstances, out uniqueError)
                        .ValidateRequired(_exportedInstances, out requiredError)
                        .CompleteValidations();
                    break;
                case "ContractMembership":
                    new ContractMembershipExcelWorker(_baseDbContext)
                        .ValidateUnique(_exportedInstances, out uniqueError)
                        .ValidateRequired(_exportedInstances, out requiredError)
                        .CompleteValidations();
                    break;
                default:
                    AddError("error", "error");
                    break;
            }
            if (uniqueError.Length > 0)
                AddError("ValidationUnique", uniqueError);
            if (requiredError.Length > 0)
                AddError("ValidationRequired", requiredError);

            return this;
        }


        private bool GetBoolValue(ExcelWorksheet worksheet, int row, int column)
        {
            var boolValue = false;
            var excelCellValue = worksheet.Cells[row, column].Value?.ToString();
            if (excelCellValue == "Ναί")
                boolValue = true;
            return boolValue;
        }

        private void GetIsActive(string colTitle, int colCount)
        {
            var colData = new List<string> { "Ναί", "Όχι" };
            var dd = _worksheet.Cells[2, colCount, 50000, colCount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            dd.AllowBlank = false;
            if (colData.Count > 0)
                foreach (var response in colData)
                    dd.Formula.Values.Add(response);
        }

        private async Task GetLookUpValueAsync(string colTitle, int colCount)
        {
            var colData = new List<string>();

            var lookupSheet = this.ExcelPackage.Workbook.Worksheets.Add("Lookup_" + colTitle);

            //lookupSheet.Cells["B1"].Value = "Here we have to add long text";
            //lookupSheet.Cells["B2"].Value = "All list values combined have to have more then 255 chars";
            //lookupSheet.Cells["B3"].Value = "more text 1 more text more text more text";
            //lookupSheet.Cells["B4"].Value = "more text 2 more text more text more text";
            //lookupSheet.Cells["B5"].Value = "more text 2 more text more text more textmore text 2 more text more text more textmore text 2 more text more text more textmore text 2 more text more text more textmore text 2 more text more text more textmore text 2 more text more text more textmore text 2 more text more text more textmore";

            //var val = ws.DataValidations.AddListValidation("A2");
            //val.Formula.ExcelFormula = "B$1:B$5";



            if (colTitle == "CompanyId")
                colData = await _baseDataWork.Companies.SelectAllAsync(x => "[VatNumber]:" + x.VatNumber + "_[Title]:" + x.Title);
            else if (colTitle == "CustomerId")
                colData = await _baseDataWork.Customers.SelectAllAsync(x => "[VatNumber]:" + x.VatNumber + "_[IdentifyingName]:" + x.IdentifyingName);
            else if (colTitle == "EmployeeId")
                colData = await _baseDataWork.Employees.SelectAllAsync(x => "[VatNumber]:" + x.VatNumber + "_[FullName]:" + x.FullName);
            else if (colTitle == "SpecializationId")
                colData = await _baseDataWork.Specializations.SelectAllAsync(x => "[Name]:" + x.Name);
            else if (colTitle == "LeaveTypeId")
                colData = await _baseDataWork.LeaveTypes.SelectAllAsync(x => "[Name]:" + x.Name);
            else if (colTitle == "WorkPlaceId")
                colData = await _baseDataWork.WorkPlaces.SelectAllAsync(x => "[Title]:" + x.Title + "_[CustomerVatNumber]:" + x.Customer.VatNumber);
            else if (colTitle == "ContractId")
                colData = await _baseDataWork.Contracts.SelectAllAsync(x => "[Title]:" + x.Title);
            else if (colTitle == "ContractTypeId")
                colData = await _baseDataWork.ContractTypes.SelectAllAsync(x => "[Name]:" + x.Name);
            else if (colTitle == "ContractMembershipId")
                colData = await _baseDataWork.ContractMemberships.SelectAllAsync(x => "[Name]:" + x.Name);

            if (colData.Count == 0)
                AddError("LookupEmpty", colTitle);

            for (int i = 0; i < colData.Count; i++)
                lookupSheet.Cells[i + 1, 1].Value = colData[i];


            var excelDataValidationList = _worksheet.Cells[2, colCount, 50000, colCount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            excelDataValidationList.Formula.ExcelFormula = "Lookup_" + colTitle + "!A1:A" + colData.Count;
            excelDataValidationList.AllowBlank = false;
            excelDataValidationList.ShowErrorMessage = true;
            excelDataValidationList.ShowInputMessage = true;
        }

        private async Task<int> GetIdForExtractedEntity(ExcelWorksheet worksheet, int row, int column)
        {
            int? id = 0;
            var excelColumnName = worksheet.Cells[1, column].Value.ToString();
            var entityName = excelColumnName.Remove(excelColumnName.Length - 2);
            var excelCellValue = worksheet.Cells[row, column].Value?.ToString();

            if (entityName == "Company")
            {
                var VatNumber = excelCellValue?.Split("_")[0]?.Split(":")[1];
                if (VatNumber != null)
                    id = (await _baseDataWork.Companies
                        .FirstAsync(x => x.VatNumber == VatNumber))?
                        .Id;
            }
            if (entityName == "Customer")
            {
                var VatNumber = excelCellValue?.Split("_")[0].Split(":")[1];
                var identifyingName = excelCellValue?.Split("_")[1].Split(":")[1];
                if (VatNumber != null && identifyingName != null)
                    id = (await _baseDataWork.Customers
                    .FirstAsync(x => x.VatNumber == VatNumber && x.IdentifyingName == identifyingName))?
                    .Id;
            }
            if (entityName == "Employee")
            {
                var VatNumber = excelCellValue?.Split("_")[0]?.Split(":")[1];
                if (VatNumber != null)
                    id = (await _baseDataWork.Employees
                    .FirstAsync(x => x.VatNumber == VatNumber))?
                    .Id;
            }
            if (entityName == "Specialization")
            {
                var name = excelCellValue?.Split("_")[0]?.Split(":")[1];
                if (name != null)
                    id = (await _baseDataWork.Specializations
                        .FirstAsync(x => x.Name == name))?
                        .Id;
            }
            if (entityName == "LeaveType")
            {
                var name = excelCellValue?.Split("_")[0]?.Split(":")[1];
                if (name != null)
                    id = (await _baseDataWork.LeaveTypes
                    .FirstAsync(x => x.Name == name))?
                    .Id;
            }
            if (entityName == "WorkPlace")
            {
                var title = excelCellValue?.Split("_")[0]?.Split(":")[1];
                var customerVatNumber = excelCellValue?.Split("_")[1]?.Split(":")[1];
                if (title != null && customerVatNumber != null)
                    id = (await _baseDataWork.WorkPlaces
                    .FirstAsync(x => x.Title == title && x.Customer.VatNumber == customerVatNumber))?
                    .Id;
            }
            if (entityName == "Contract")
            {
                var title = excelCellValue?.Split("_")[0]?.Split(":")[1];
                if (title != null)
                    id = (await _baseDataWork.Contracts
                    .FirstAsync(x => x.Title == title))?
                    .Id;
            }
            if (entityName == "ContractType")
            {
                var name = excelCellValue?.Split("_")[0]?.Split(":")[1];
                if (name != null)
                    id = (await _baseDataWork.ContractTypes
                    .FirstAsync(x => x.Name == name))?
                    .Id;
            }
            if (entityName == "ContractMembership")
            {
                var name = excelCellValue?.Split("_")[0]?.Split(":")[1];
                if (name != null)
                    id = (await _baseDataWork.ContractMemberships
                        .FirstAsync(x => x.Name == name))?
                        .Id;
            }

            if (id == null)
            {
                AddError(entityName, "NullEntityIdFromDb");
                return 0;
            }

            return (int)id;
        }

        private void AddError(string type, string colName)
        {
            var message = $"Βρέθηκε πρόβλημα στην κολώνα {colName}";

            if (type == "ValidationUnique")
                message += " πρεπει να υπάρχει μοναδικότητα!";
            if (type == "ValidationRequired")
                message += " πρεπει να είναι συμπληρομένο!";
            if (type == "LookupEmpty")
                message += " όπου δεν βρεθηκαν δεδομένα για την δημιουργεία λιστών!";
            if (type == "NullEntityIdFromDb")
                message += " όπου δεν βρεθηκαν δεδομενα των Lookup στην βάση!";
            if (type == "error")
                message = "Error που δεν μπορεσε να διαχειριστεί!";

            this.Errors.Add(message);
        }
    }
}
