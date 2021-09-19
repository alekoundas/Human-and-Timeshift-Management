using Bussiness;
using Bussiness.Helpers;
using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.Models.Security;
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
        private readonly SecurityDataWork _securityDataWork;
        private BaseDatawork _baseDataWork;
        public WorkHourController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _securityDataWork = new SecurityDataWork(SecurityDbContext);
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
                var workHours = (await (new ExcelService<WorkHour>(_context)
                    .ExtractDataDaysFromExcel(ImportExcel)))
                    .ValidateExtractedData()
                    .RetrieveExtractedData(out var errors);

                var overlapWorkHours = _baseDataWork.WorkHours.Query
                    .Include(x => x.TimeShift).ThenInclude(x => x.WorkPlace)
                    .Include(x => x.Employee)
                    .Where(x => x.TimeShiftId != workHours.FirstOrDefault().TimeShiftId)
                    .Where(x => x.StartOn.Year == workHours.FirstOrDefault().StartOn.Year)
                    .Where(x => x.StartOn.Month == workHours.FirstOrDefault().StartOn.Month)
                    .Select(x => new WorkHour
                    {
                        StartOn = x.StartOn,
                        EndOn = x.StartOn,
                        EmployeeId = x.EmployeeId,
                        Employee = new Employee
                        {
                            FirstName = x.Employee.FirstName,
                            LastName = x.Employee.LastName
                        },
                        TimeShift = new TimeShift
                        {
                            Title = x.TimeShift.Title,
                            WorkPlace = new WorkPlace
                            {
                                Title = x.TimeShift.WorkPlace.Title
                            }

                        }
                    })
                    .ToList()
                    .Where(x => workHours.Any(y => y.EmployeeId == x.EmployeeId && x.StartOn.Date <= y.EndOn.Date && y.StartOn.Date <= x.EndOn.Date))
                    .ToList();

                if (errors.Count == 0 && overlapWorkHours.Count == 0)
                {

                    _baseDataWork.WorkHours.RemoveRange(_baseDataWork.WorkHours
                        .Where(x => x.TimeShiftId == workHours[0].TimeShiftId).ToList());

                    var status_delete = await _baseDataWork.SaveChangesAsync();

                    _baseDataWork.WorkHours.AddRange(workHours);
                    var status = await _baseDataWork.SaveChangesAsync();

                    if (status > 0)
                        TempData["StatusMessage"] = workHours.Count +
                    " εγγραφές προστέθηκαν με επιτυχία";
                    else
                        TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";
                }


                if (errors.Count > 0)
                    TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

                if (overlapWorkHours.Count > 0)
                {

                    var error_msg = new List<string>();
                    foreach (var workHour in overlapWorkHours)
                    {
                        error_msg.Add("<br> Βαρδια επικαλύπτεται απο \"" + workHour.StartOn.ToString() + " - " + workHour.EndOn.ToString() + "\" στο ποστο \"" + workHour.TimeShift.WorkPlace.Title + "\" υπάλληλος \"" + workHour.Employee.FullName+ "\"");
                    }

                    _securityDataWork.Notifications.Add(new Notification
                    {
                        ApplicationUserId = HttpAccessorService.GetLoggeInUser_Id,
                        Title = "Προβλημα εισαγωγής χρονοδιαγράμματος " + overlapWorkHours.First().TimeShift.Title,
                        Description = string.Join("", error_msg),
                        IsSeen = false,
                        CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                        CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                        CreatedOn = DateTime.Now
                    });
                    await _securityDataWork.SaveChangesAsync();
                }
            }

            return RedirectToAction("index", "TimeShift");
        }
    }
}
