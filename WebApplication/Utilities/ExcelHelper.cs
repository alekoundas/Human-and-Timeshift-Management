using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.Repository.Interface;
using Bussiness;
using DataAccess;
using DataAccess.Models.Entity;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;

namespace WebApplication.Utilities
{
    public class ExcelHelper
    {
        private BaseDatawork _baseDataWork;

        private ExcelPackage ExcelPackage { get; set; }
        public ExcelHelper(BaseDbContext BaseDbContext)
        {
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        public ExcelHelper CreateNewExcel(string fileName)
        {
            this.ExcelPackage = new ExcelPackage();
            this.ExcelPackage.Workbook.Properties.Author = "PureMethod";
            this.ExcelPackage.Workbook.Properties.Title = fileName;
            this.ExcelPackage.Workbook.Properties.Subject = fileName + " data exported from database";
            this.ExcelPackage.Workbook.Properties.Created = DateTime.Now;

            return this;
        }

        public ExcelHelper AddSheet<TEntity>(List<string> colTitles, List<TEntity> results = null)
        {
            var colCount = 1;
            var rowCount = 1;
            var worksheet = this.ExcelPackage.Workbook.Worksheets.Add(nameof(TEntity));

            //Add the headers
            foreach (var colTitle in colTitles)
            {
                worksheet.Cells[rowCount, colCount++].Value = colTitle;
                if (colTitle.Contains("Id"))
                    GetLookUpAsync(colTitle, worksheet, colCount-1);
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

        public ExcelPackage CompleteExcel()
        {
            return this.ExcelPackage;
        }
        private async Task GetLookUpAsync(string colTitle, ExcelWorksheet worksheet, int colCount)
        {
            var dd = worksheet.Cells[2, colCount, 1000, colCount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            dd.AllowBlank = false;
            foreach (var response in await GetLookUpDataAsync(colTitle))
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
            return response;
        }
    }
}
