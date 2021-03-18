using Bussiness.Service.ExcelServiceWorkers;
using DataAccess;
using DataAccess.Models.Entity;
using LinqKit;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bussiness.Service
{
    public class ExcelService<TEntity>
    {
        private BaseDatawork _baseDataWork;
        private BaseDbContext _baseDbContext;
        private List<string> Errors;
        private ExcelWorksheet _worksheet;
        private Expression<Func<Employee, bool>> _employeeFilter = PredicateBuilder.New<Employee>(true);
        private Expression<Func<TimeShift, bool>> _timeShiftFilter = PredicateBuilder.New<TimeShift>(true);


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

        public ExcelService<TEntity> AddLookupFilter(Expression<Func<Employee, bool>> predicate)
        {
            _employeeFilter = predicate;
            return this;
        }
        public ExcelService<TEntity> AddLookupFilter(Expression<Func<TimeShift, bool>> predicate)
        {
            _timeShiftFilter = predicate;
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
                if (colTitle.Contains("Day_"))
                {
                    SetColValidation(colTitle, colCount, "Time");
                    _worksheet.Cells[rowCount, colCount++].Value = colTitle;
                    SetColValidation(colTitle, colCount, "Time");
                    _worksheet.Cells[rowCount, colCount++].Value = colTitle;
                    _worksheet.Cells[rowCount, colCount - 2, rowCount, colCount - 1].Merge = true;

                }
                else
                {
                    var propertyType = typeof(TEntity).GetProperty(colTitle).PropertyType.Name;

                    if (colTitle.EndsWith("Id"))
                        await GetLookUpValueAsync(colTitle, colCount);
                    else if (colTitle.Contains("IsActive"))
                        GetIsActive(colTitle, colCount);
                    else if (propertyType == "DateTime")
                        SetColValidation(colTitle, colCount, propertyType);
                    //else if (propertyType == "Nullable`1")//Datetime?
                    //    break;

                    _worksheet.Cells[rowCount, colCount++].Value = colTitle;
                }
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

        public async Task<ExcelService<TEntity>> ExtractDataDaysFromExcel(IFormFile ImportExcel)
        {
            var timeshift = new TimeShift();
            _exportedInstances = new List<TEntity>();
            var genericEntityInstance = (TEntity)Activator.CreateInstance(typeof(TEntity));
            using (MemoryStream stream = new MemoryStream())
            {
                await ImportExcel.CopyToAsync(stream);
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    var worksheet = excelPackage.Workbook.Worksheets[0];
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
                            if (propertyName.Contains("Day_"))
                            {
                                var startOn = worksheet.Cells[i, j].Value?.ToString();
                                var endOn = worksheet.Cells[i, ++j].Value?.ToString();
                                var setStartOn = default(DateTime);
                                var setEndOn = default(DateTime);

                                if (startOn != null && endOn != null)
                                {
                                    DateTime.TryParse(startOn, out setStartOn);
                                    DateTime.TryParse(endOn, out setEndOn);

                                    setStartOn = new DateTime(timeshift.Year, timeshift.Month, Int32.Parse(propertyName.Split("_")[1]), setStartOn.TimeOfDay.Hours, setStartOn.TimeOfDay.Minutes, 0);
                                    setEndOn = new DateTime(timeshift.Year, timeshift.Month, Int32.Parse(propertyName.Split("_")[1]), setEndOn.TimeOfDay.Hours, setEndOn.TimeOfDay.Minutes, 0);

                                    var currentDayEntityInstance = (TEntity)Activator.CreateInstance(typeof(TEntity));
                                    var genericInstanceProperties = genericEntityInstance.GetType().GetProperties();
                                    var currentDayInstanceProperties = currentDayEntityInstance.GetType().GetProperties();

                                    //Property coppier
                                    foreach (var genericProperty in genericInstanceProperties)
                                    {
                                        foreach (var currentDayProperty in currentDayInstanceProperties)
                                        {
                                            if (genericProperty.Name == currentDayProperty.Name && genericProperty.PropertyType == currentDayProperty.PropertyType)
                                            {
                                                currentDayProperty.SetValue(currentDayEntityInstance, genericProperty.GetValue(genericEntityInstance));
                                                break;
                                            }
                                        }
                                    }


                                    AddPropertyValueToInstance(currentDayEntityInstance, setStartOn, "StartOn");
                                    AddPropertyValueToInstance(currentDayEntityInstance, setEndOn, "EndOn");

                                    //Add Audit fields non existant in excel column
                                    AddPropertyValueToInstance(currentDayEntityInstance, DateTime.Now, "CreatedOn");
                                    AddPropertyValueToInstance(currentDayEntityInstance, HttpAccessorService.GetLoggeInUser_FullName, "CreatedBy_FullName");
                                    AddPropertyValueToInstance(currentDayEntityInstance, HttpAccessorService.GetLoggeInUser_Id, "CreatedBy_Id");

                                    _exportedInstances.Add(currentDayEntityInstance);
                                }
                            }
                            else
                            {
                                if (worksheet.Cells[1, j].Value.ToString().EndsWith("Id"))//Filter relation
                                {
                                    if (await GetIdForExtractedEntity(worksheet, i, j) != 0)
                                    {
                                        var propertyValue = await GetIdForExtractedEntity(worksheet, i, j);
                                        if (propertyName == "TimeShiftId")
                                            timeshift = await _baseDataWork.TimeShifts.FirstOrDefaultAsync(x => x.Id == (int)propertyValue);
                                        AddPropertyValueToInstance(genericEntityInstance, propertyValue, propertyName);
                                    }
                                }
                                else if (genericEntityInstance.GetType().GetProperty(worksheet.Cells[1, j].Value.ToString()).PropertyType.Name == "String")
                                    AddPropertyValueToInstance(genericEntityInstance, worksheet.Cells[i, j].Value?.ToString(), propertyName);
                            }
                        }
                    }
                }
            }
            return this;
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
            string hoursInTimeshiftError = "";
            string eployeeInTimeshiftError = "";
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
                case "RealWorkHour":
                    new RealWorkHourExcelWorker(_baseDbContext)
                        .ValidateHoursInTimeshift(_exportedInstances, out hoursInTimeshiftError)
                        .ValidateEmployeeInTimeshift(_exportedInstances, out eployeeInTimeshiftError)
                        .CompleteValidations();
                    break;
                case "WorkHour":
                    new WorkHourExcelWorker(_baseDbContext)
                        .ValidateHoursInTimeshift(_exportedInstances, out hoursInTimeshiftError)
                        .ValidateEmployeeInTimeshift(_exportedInstances, out eployeeInTimeshiftError)
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
            if (hoursInTimeshiftError.Length > 0)
                AddError("ValidateHoursInTimeshift", hoursInTimeshiftError);
            if (eployeeInTimeshiftError.Length > 0)
                AddError("ValidateEmployeeInTimeshift", eployeeInTimeshiftError);

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

        private void SetColValidation(string colTitle, int colCount, string propertyType)
        {
            var excelDateTimeValidation = _worksheet.Cells[2, colCount, 50000, colCount];
            switch (propertyType)
            {
                case "Time":
                    var timeValidation = excelDateTimeValidation.DataValidation.AddTimeDataValidation();

                    timeValidation.ShowErrorMessage = true;
                    //Minimum allowed time
                    timeValidation.Formula.Value = new ExcelTime(0.0M);
                    //Maximum allowed time
                    timeValidation.Formula2.Value = new ExcelTime(0.999305555555556M);
                    timeValidation.AllowBlank = true;
                    timeValidation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    timeValidation.ErrorTitle = "Αδύνατη εισαγωγή ώρας";
                    timeValidation.Error = "Η ώρα πρεπει να ειναι της μορφής: hh:mm (πχ 21:20) ";

                    timeValidation.ShowInputMessage = true;
                    timeValidation.Prompt = "Η ώρα πρεπει να ειναι της μορφής: hh:mm (πχ 21:20)";
                    timeValidation.PromptTitle = "Εισαγωγή ώρας";
                    break;

                case "DateTime":
                    var dateTimeValidation = excelDateTimeValidation.DataValidation.AddDateTimeDataValidation();

                    //Minimum allowed date
                    dateTimeValidation.Formula.Value = new DateTime(2000, 1, 1, 0, 0, 0);
                    //Maximum allowed date
                    dateTimeValidation.Formula2.Value = new DateTime(2100, 1, 1, 0, 0, 0);
                    dateTimeValidation.AllowBlank = true;
                    dateTimeValidation.ShowErrorMessage = true;
                    dateTimeValidation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    dateTimeValidation.ErrorTitle = "Αδύνατη εισαγωγή ημερομηνίας";
                    dateTimeValidation.Error = "Η ημερομηνία πρεπει να ειναι της μορφής: d/m/y h:m (πχ 1/1/2001 21:20) ";

                    dateTimeValidation.ShowInputMessage = true;
                    dateTimeValidation.Prompt = "Η ημερομηνία πρεπει να ειναι της μορφής: d/m/y h:m (πχ 1/1/2001 21:20)";
                    dateTimeValidation.PromptTitle = "Εισαγωγή ημερομηνίας";
                    break;

                default:
                    break;
            }

        }

        private async Task GetLookUpValueAsync(string colTitle, int colCount)
        {
            var colData = new List<string>();
            var lookupSheet = this.ExcelPackage.Workbook.Worksheets.Add("Lookup_" + colTitle);

            if (colTitle == "CompanyId")
                colData = await _baseDataWork.Companies.SelectAllAsync(x => "[VatNumber]:" + x.VatNumber + "_[Title]:" + x.Title);
            else if (colTitle == "CustomerId")
                colData = await _baseDataWork.Customers.SelectAllAsync(x => "[VatNumber]:" + x.VatNumber + "_[IdentifyingName]:" + x.IdentifyingName);
            else if (colTitle == "EmployeeId")
                colData = await _baseDataWork.Employees.SelectAllAsyncFiltered(_employeeFilter, x => "[VatNumber]:" + x.VatNumber + "_[FullName]:" + x.FullName);
            else if (colTitle == "SpecializationId")
                colData = await _baseDataWork.Specializations.SelectAllAsync(x => "[Name]:" + x.Name);
            else if (colTitle == "LeaveTypeId")
                colData = await _baseDataWork.LeaveTypes.SelectAllAsync(x => "[Name]:" + x.Name);
            else if (colTitle == "TimeShiftId")
                colData = await _baseDataWork.TimeShifts.SelectAllAsyncFiltered(_timeShiftFilter, x => "[Id]:" + x.Id + "_[Title]:" + x.Title + "_[WorkPlace]:" + x.WorkPlace.Title);
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
            excelDataValidationList.Formula.ExcelFormula = "Lookup_" + colTitle + "!A1:A1000";
            excelDataValidationList.AllowBlank = false;

            excelDataValidationList.ShowErrorMessage = true;
            excelDataValidationList.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            excelDataValidationList.ErrorTitle = "Αδύνατη εισαγωγή";
            //excelDataValidationList.Error = "Η ημερομηνία πρεπει να ειναι της μορφής: d/m/y h:m (πχ 1/1/2001 21:20) ";

            excelDataValidationList.ShowInputMessage = true;
            //excelDataValidationList.Prompt = "Η ημερομηνία πρεπει να ειναι της μορφής: d/m/y h:m (πχ 1/1/2001 21:20)";
            excelDataValidationList.PromptTitle = "Εισαγωγή";
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
            if (entityName == "TimeShift")
            {
                Int32.TryParse(excelCellValue?.Split("_")[0]?.Split(":")[1], out var timeShiftId);
                if (timeShiftId != 0)
                    id = (await _baseDataWork.TimeShifts
                    .FirstAsync(x => x.Id == timeShiftId))?
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
