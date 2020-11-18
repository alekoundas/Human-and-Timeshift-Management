using DataAccess;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bussiness.Service
{
    public class ExcelService
    {
        private BaseDatawork _baseDataWork;
        private List<string> Errors;

        private ExcelPackage ExcelPackage { get; set; }
        public ExcelService(BaseDbContext BaseDbContext)
        {
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        public ExcelService CreateNewExcel(string fileName)
        {
            this.Errors = new List<string>();

            this.ExcelPackage = new ExcelPackage();
            this.ExcelPackage.Workbook.Properties.Author = "PureMethod";
            this.ExcelPackage.Workbook.Properties.Title = fileName;
            this.ExcelPackage.Workbook.Properties.Subject = fileName + "δεδομένα απο την βάση";
            this.ExcelPackage.Workbook.Properties.Created = DateTime.Now;

            return this;
        }

        public async Task<ExcelService> AddSheetAsync<TEntity>(List<string> colTitles, List<TEntity> results = null)
        {
            var colCount = 1;
            var rowCount = 1;
            var worksheet = this.ExcelPackage.Workbook.Worksheets.Add(nameof(TEntity));

            //Add the headers
            foreach (var colTitle in colTitles)
            {
                worksheet.Cells[rowCount, colCount++].Value = colTitle;
                if (colTitle.Contains("Id"))
                    await GetLookUpAsync(colTitle, worksheet, colCount - 1);
            }

            //Add rows with data
            if (results != null)
                foreach (var result in results)
                {
                    rowCount++;
                    colCount = 1;
                    foreach (var colTitle in colTitles)
                        worksheet.Cells[rowCount, colCount++].Value =
                            result.GetType().GetProperty(colTitle).GetValue(result);
                }


            //worksheet.HeaderFooter.OddFooter.InsertPicture(
            // new FileInfo(Path.Combine("wwwroot","img", "Leave.png")),
            // PictureAlignment.Right);

            //Make sheet cells even
            worksheet.Cells[1, 1, rowCount, colCount].AutoFitColumns();
            return this;
        }

        public ExcelPackage CompleteExcel(out List<string> Errors)
        {
            Errors = this.Errors;
            return this.ExcelPackage;
        }

        private async Task GetLookUpAsync(string colTitle, ExcelWorksheet worksheet, int colCount)
        {
            var colData = await GetLookUpDataAsync(colTitle);
            var dd = worksheet.Cells[2, colCount, 50000, colCount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            dd.AllowBlank = false;
            if (colData.Count > 0)
                foreach (var response in colData)
                    dd.Formula.Values.Add(response.ToString());
        }

        private async Task<List<int>> GetLookUpDataAsync(string colTitle)
        {
            var response = new List<int>();
            if (colTitle == "CompanyId")
                response = await _baseDataWork.Companies.SelectAllAsync(x => x.Id);
            else if (colTitle == "SpecializationId")
                response = await _baseDataWork.Specializations.SelectAllAsync(x => x.Id);
            else if (colTitle == "CustomerId")
                response = await _baseDataWork.Customers.SelectAllAsync(x => x.Id);

            if (response.Count == 0)
                this.Errors.Add(colTitle);
            return response;
        }
    }
}
