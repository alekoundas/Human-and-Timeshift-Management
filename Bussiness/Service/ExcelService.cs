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
        private List<string> Errors;
        private ExcelWorksheet _worksheet;

        private ExcelPackage ExcelPackage { get; set; }

        private List<TEntity> _exportedInstances;
        public ExcelService(BaseDbContext BaseDbContext)
        {
            this.Errors = new List<string>();
            _baseDataWork = new BaseDatawork(BaseDbContext);
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
                if (colTitle.Contains("Id"))
                    await GetLookUpAsync(colTitle, colCount - 1);
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

        public async Task<ExcelService<TEntity>> ValidateExtractedData()
        {
            var hasValidationError = false;
            switch (typeof(TEntity).Name)
            {
                case "Company":
                    foreach (var exportedInstance in _exportedInstances)
                    {
                        var afm = (exportedInstance.GetType().GetProperty("Afm"))
                            .GetValue(exportedInstance);

                        hasValidationError = _baseDataWork.Companies
                            .Any(x => x.Afm.Trim() == afm.ToString().Trim());

                        if (hasValidationError)
                            AddError("Afm", "Validation");
                    }
                    break;
                case "Customer":
                    foreach (var exportedInstance in _exportedInstances)
                    {
                        var AFM = (exportedInstance.GetType().GetProperty("AFM"))
                            .GetValue(exportedInstance);

                        hasValidationError = _baseDataWork.Customers
                            .Any(x => x.AFM.Trim() == AFM.ToString().Trim());

                        if (hasValidationError)
                            AddError("Afm", "Validation");
                    }
                    break;
                case "WorkPlace":
                    foreach (var exportedInstance in _exportedInstances)
                    {
                        var title = (exportedInstance.GetType().GetProperty("Title"))
                            .GetValue(exportedInstance);

                        var customerId = (int?)(exportedInstance.GetType().GetProperty("CustomerId"))
                            .GetValue(exportedInstance);

                        hasValidationError = _baseDataWork.WorkPlaces
                            .Any(x => x.Title.Trim() == title.ToString().Trim() && x.CustomerId == customerId);

                        if (hasValidationError)
                            AddError("Title και CustomerId", "Validation");
                    }
                    break;
                case "Employee":
                    foreach (var exportedInstance in _exportedInstances)
                    {
                        var Afm = (exportedInstance.GetType().GetProperty("Afm"))
                            .GetValue(exportedInstance);

                        hasValidationError = _baseDataWork.Employees
                            .Any(x => x.Afm.Trim() == Afm.ToString().Trim());

                        if (hasValidationError)
                            AddError("Afm", "Validation");
                    }
                    break;
                case "Specialization":
                    foreach (var exportedInstance in _exportedInstances)
                    {
                        var Name = (exportedInstance.GetType().GetProperty("Name"))
                            .GetValue(exportedInstance);

                        hasValidationError = _baseDataWork.Specializations
                            .Any(x => x.Name.Trim() == Name.ToString().Trim());

                        if (hasValidationError)
                            AddError("Name", "Validation");
                    }
                    break;
                case "LeaveType":
                    foreach (var exportedInstance in _exportedInstances)
                    {
                        var Name = (exportedInstance.GetType().GetProperty("Name"))
                            .GetValue(exportedInstance);

                        hasValidationError = _baseDataWork.LeaveTypes
                            .Any(x => x.Name.Trim() == Name.ToString().Trim());

                        if (hasValidationError)
                            AddError("Name", "Validation");
                    }
                    break;
                default:
                    AddError("error", "error");
                    break;
            }

            return this;
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
                    foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                        for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                        {
                            genericEntityInstance = (TEntity)Activator.CreateInstance(typeof(TEntity));

                            for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                                if (worksheet.Cells[1, j].Value.ToString().Contains("Id"))//Filter integers
                                    genericEntityInstance
                                        .GetType()
                                        .GetProperty(worksheet.Cells[1, j].Value.ToString())
                                        .SetValue(genericEntityInstance, await GetIdForEntity(worksheet, i, j), null);
                                else if (worksheet.Cells[1, j].Value.ToString().Contains("IsActive"))
                                    genericEntityInstance
                                        .GetType()
                                        .GetProperty(worksheet.Cells[1, j].Value.ToString())
                                        .SetValue(genericEntityInstance, GetBoolValue(worksheet, i, j), null);
                                else
                                    genericEntityInstance
                                        .GetType()
                                        .GetProperty(worksheet.Cells[1, j].Value.ToString())
                                        .SetValue(genericEntityInstance, worksheet.Cells[i, j].Value?.ToString(), null);

                            //Add CreatedOn extra non existant in excel column
                            genericEntityInstance
                                .GetType()
                                .GetProperty("CreatedOn")
                                .SetValue(genericEntityInstance, DateTime.Now, null);

                            _exportedInstances.Add(genericEntityInstance);
                        }
                }
            }
            return this;
        }
        private async Task<int> GetIdForEntity(ExcelWorksheet worksheet, int row, int column)
        {
            var id = 0;
            var excelColumnName = worksheet.Cells[1, column].Value.ToString();
            var entityName = excelColumnName.Remove(excelColumnName.Length - 2);
            var excelCellValue = worksheet.Cells[row, column].Value?.ToString();

            if (entityName == "Company")
            {
                var afm = excelCellValue.Split("_")[0].Split(":")[1];
                id = (await _baseDataWork.Companies
                    .FirstAsync(x => x.Afm == afm))
                    .Id;
            }
            if (entityName == "Customer")
            {
                var afm = excelCellValue.Split("_")[0].Split(":")[1];
                var identifyingName = excelCellValue.Split("_")[1].Split(":")[1];
                id = (await _baseDataWork.Customers
                    .FirstAsync(x => x.AFM == afm && x.ΙdentifyingΝame == identifyingName))
                    .Id;
            }
            if (entityName == "Employee")
            {
                var afm = excelCellValue.Split("_")[0].Split(":")[1];
                id = (await _baseDataWork.Employees
                    .FirstAsync(x => x.Afm == afm))
                    .Id;
            }
            if (entityName == "Specialization")
            {
                var name = excelCellValue.Split("_")[0].Split(":")[1];
                id = (await _baseDataWork.Specializations
                    .FirstAsync(x => x.Name == name))
                    .Id;
            }
            if (entityName == "LeaveType")
            {
                var name = excelCellValue.Split("_")[0].Split(":")[1];
                id = (await _baseDataWork.LeaveTypes
                    .FirstAsync(x => x.Name == name))
                    .Id;
            }

            if (id == 0)
                AddError(entityName, "NullEntityIdFromDb");

            return id;
        }

        private bool GetBoolValue(ExcelWorksheet worksheet, int row, int column)
        {
            var boolValue = false;
            var excelCellValue = worksheet.Cells[row, column].Value?.ToString();
            if (excelCellValue == "Ναί")
                boolValue = true;
            return boolValue;
        }


        private async Task GetIsActive(string colTitle, int colCount)
        {
            var colData = new List<string> { "Ναί", "Όχι" };
            var dd = _worksheet.Cells[2, colCount, 50000, colCount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            dd.AllowBlank = false;
            if (colData.Count > 0)
                foreach (var response in colData)
                    dd.Formula.Values.Add(response);
        }

        private async Task GetLookUpAsync(string colTitle, int colCount)
        {
            var colData = await GetLookUpDataAsync(colTitle);
            var dd = _worksheet.Cells[2, colCount, 50000, colCount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            dd.AllowBlank = false;
            if (colData.Count > 0)
                foreach (var response in colData)
                    dd.Formula.Values.Add(response);
        }

        private async Task<List<string>> GetLookUpDataAsync(string colTitle)
        {
            var response = new List<string>();
            if (colTitle == "CompanyId")
                response = await _baseDataWork.Companies.SelectAllAsync(x => "[Afm]:" + x.Afm + "_[Title]:" + x.Title);
            else if (colTitle == "CustomerId")
                response = await _baseDataWork.Customers.SelectAllAsync(x => "[AFM]:" + x.AFM + "_[ΙdentifyingΝame]:" + x.ΙdentifyingΝame);
            else if (colTitle == "EmployeeId")
                response = await _baseDataWork.Employees.SelectAllAsync(x => "[AFM]:" + x.Afm + "_[FullName]:" + x.FullName);
            else if (colTitle == "SpecializationId")
                response = await _baseDataWork.Specializations.SelectAllAsync(x => "[Name]:" + x.Name);
            else if (colTitle == "LeaveTypeId")
                response = await _baseDataWork.LeaveTypes.SelectAllAsync(x => "[Name]:" + x.Name);

            if (response.Count == 0)
                AddError(colTitle, "LookupEmpty");
            return response;
        }

        private void AddError(string colName, string type)
        {
            var message = $"Βρέθηκε πρόβλημα στην κολώνα {colName}";

            if (type == "Validation")
                message += "  πρεπει να υπάρχει μοναδικότητα!";
            if (type == "LookupEmpty")
                message += "όπου δεν βρεθηκαν δεδομένα για την δημιουργεία λιστών!";
            if (type == "NullEntityIdFromDb")
                message += "όπου δεν βρεθηκαν οι συγγεκριμένες εγγραφές στην βάση!";
            if (type == "error")
                message = "Error που δεν μπορεσε να διαχειριστεί!";

            this.Errors.Add(message);
        }
    }
}
