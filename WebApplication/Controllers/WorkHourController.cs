using Bussiness.Helpers;
using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Entity;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    public class WorkHourController : MasterController
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public WorkHourController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }



        public async Task<ActionResult> DownloadExcelTemplate(int? id)
        {
            var timeshift = await _baseDataWork.TimeShifts.FirstOrDefaultAsync(x => x.Id == id);
            var daysInMonth = DateTime.DaysInMonth(timeshift.Year, timeshift.Month);
            var excelColumns = new List<string>(new string[] {
                "Excel_Properties",
                "Excel_Values",
                "EmployeeId",
                "TimeShiftId"
            });
            var cultureInfo = new CultureInfo("el-GR");
            for (int i = 1; i <= daysInMonth; i++)
            {
                var date = new DateTime(
                    timeshift.Year,
                    timeshift.Month,
                    i,
                    0, 0, 0);
                var dayStr = date.ToString("ddd,d-MMM", cultureInfo);

                excelColumns.Add(dayStr);
            }
            var timeShiftFilter = PredicateBuilder.New<TimeShift>(true);
            timeShiftFilter = timeShiftFilter.And(x => x.Id == id);

            var employeeFilter = PredicateBuilder.New<Employee>();
            employeeFilter = employeeFilter
                .And(x => x.EmployeeWorkPlaces.Any(y => y.WorkPlace.TimeShifts.Any(z => z.Id == id)));

            var excelPackage = (await (new ExcelService<WorkHour>(_context)
                .AddLookupFilter(employeeFilter)
                .AddLookupFilter(timeShiftFilter)
               .CreateNewExcel("WorkHours"))
               .AddSheetDatesAsync(excelColumns))
               .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "WorkHours.xlsx");
            else
                TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

            return View();
        }

        public async Task<ActionResult> DownloadExcelWithData(int? id)
        {
            var timeshift = await _baseDataWork.TimeShifts.FirstOrDefaultAsync(x => x.Id == id);
            var daysInMonth = DateTime.DaysInMonth(timeshift.Year, timeshift.Month);
            var excelColumns = new List<string>(new string[] {
                "Excel_Properties",
                "Excel_Values",
                "EmployeeId",
                "TimeShiftId"
            });

            var cultureInfo = new CultureInfo("el-GR");
            for (int i = 1; i <= daysInMonth; i++)
            {
                var date = new DateTime(
                    timeshift.Year,
                    timeshift.Month,
                    i,
                    0, 0, 0);

                excelColumns.Add(date.ToString("ddd,d-MMM", cultureInfo));
            }

            var timeShiftFilter = PredicateBuilder.New<TimeShift>(true);
            timeShiftFilter = timeShiftFilter.And(x => x.Id == id);

            var employeeFilter = PredicateBuilder.New<Employee>();
            employeeFilter = employeeFilter
                .And(x => x.EmployeeWorkPlaces.Any(y => y.WorkPlace.TimeShifts.Any(z => z.Id == id)));

            var includes = new List<Func<IQueryable<WorkHour>, IIncludableQueryable<WorkHour, object>>>();
            includes.Add(x => x.Include(y => y.Employee));
            includes.Add(x => x.Include(y => y.TimeShift).ThenInclude(y => y.WorkPlace));


            var excelPackage = (await (new ExcelService<WorkHour>(_context)
                .AddLookupFilter(employeeFilter)
                .AddLookupFilter(timeShiftFilter)
                .AddExpressionTreeFilter(ExpressionTreeHelper.WherePropertyEquals<WorkHour>("TimeShiftId", id.ToString()))
                .AddExpressionTreeIncludes(includes)
             .CreateNewExcel("WorkHours"))
             .AddSheetDatesAsync(excelColumns, "WorkHours"))
             .CompleteExcel(out var errors);

            if (errors.Count == 0)
                using (var package = excelPackage)
                    return File(package.GetAsByteArray(), XlsxContentType, "WorkHours.xlsx");
            else
                TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

            return View();
        }

        public async Task<ActionResult> Import(IFormFile ImportExcel)
        {
            if (ImportExcel == null)
                TempData["StatusMessage"] = "Ωχ! Φαίνεται πως δεν δόθηκε αρχείο Excel.";
            else
            {
                var realWorkHours = (await (new ExcelService<WorkHour>(_context)
                    .ExtractDataDaysFromExcel(ImportExcel)))
                    .ValidateExtractedData()
                    .RetrieveExtractedData(out var errors);

                if (errors.Count == 0)
                {

                    _baseDataWork.WorkHours.RemoveRange(_baseDataWork.WorkHours
                        .Where(x => x.TimeShiftId == realWorkHours[0].TimeShiftId).ToList());

                    var status_delete = await _baseDataWork.SaveChangesAsync();

                    _baseDataWork.WorkHours.AddRange(realWorkHours);
                    var status = await _baseDataWork.SaveChangesAsync();

                    if (status > 0)
                        TempData["StatusMessage"] = realWorkHours.Count +
                    " εγγραφές προστέθηκαν με επιτυχία";
                    else
                        TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";
                }
                else
                    TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);
            }

            return RedirectToAction("index", "TimeShift");
        }
    }
}
