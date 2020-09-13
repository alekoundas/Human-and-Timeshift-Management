using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.Models.Datatable;
using System.Dynamic;
using Bussiness.Service;
using WebApplication.Utilities;
using Bussiness;
using Bussiness.Repository.Security.Interface;
using System.Linq.Expressions;
using DataAccess.Models.Select2;
using LinqKit;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Dynamic.Core;

namespace WebApplication.Api
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;

        public EmployeesApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }



        // POST: api/employees
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            var employeeWorkplaces = _baseDataWork.EmployeeWorkPlaces
                .Where(x => x.EmployeeId == id).ToList();

            _baseDataWork.EmployeeWorkPlaces.RemoveRange(employeeWorkplaces);
            _baseDataWork.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return employee;
        }



        // GET: api/employees/getSelet2Option/id
        [HttpGet("getselect2option/{id}")]
        public async Task<ActionResult<Employee>> GetSelet2Option(int id)
        {
            var select2Helper = new Select2Helper();
            Expression<Func<Employee, bool>> filter = x => x.Id == id;

            var employees = (List<Employee>)await _baseDataWork.Employees
                     .GetPaggingWithFilter(null, filter, null, 1);

            return Ok(select2Helper.CreateEmployeesResponse(employees,false));
        }


        // GET: api/employees/select2
        [HttpPost("select2")]
        public async Task<ActionResult<Employee>> Select2([FromBody] Select2Get select2)
        {
            var employees = new List<Employee>();
            var select2Helper = new Select2Helper();
            var total = await _baseDataWork.Employees.CountAllAsync();
            var hasMore = (select2.Page * 10) < total;
            var filter = PredicateBuilder.New<Employee>();
            var parentFilter = filter;

            if (select2.TimeShiftId != null)
                filter = filter.And(x => x.EmployeeWorkPlaces
                .Any(y => y.WorkPlace.TimeShift
                    .Any(z => z.Id == select2.TimeShiftId)));

            if (select2.ExistingIds?.Count > 0)
                foreach (var employeeId in select2.ExistingIds)
                    filter = filter.And(x => x.Id != employeeId);

            if (select2.Search != null)
                filter = filter.And(x =>
                   x.EmployeeWorkPlaces.Any(y =>
                       x.FirstName.Contains(select2.Search) ||
                       x.LastName.Contains(select2.Search)
                    )
                );

            if (parentFilter == filter)
                employees = (List<Employee>)await _baseDataWork.Employees
                  .GetPaggingWithFilter(null, null, null, 10, select2.Page);
            else
                employees = (List<Employee>)await _baseDataWork.Employees
              .GetPaggingWithFilter(null, filter, null, 10, select2.Page);

            return Ok(select2Helper.CreateEmployeesResponse(employees, hasMore));
        }

        // GET: api/employees/select2
        [HttpPost("select2filtered")]
        public async Task<ActionResult<Employee>> Select2Filtered([FromBody] Select2FilteredGet select2)
        {
            var employees = new List<Employee>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<Employee>();

            if (select2.ExistingIds?.Count > 0)
                foreach (var employeeId in select2.ExistingIds)
                    filter = filter.And(x => x.Id != employeeId);


            if (select2.TimeShiftId != 0 && !string.IsNullOrWhiteSpace(select2.Search))
            {
                var timeshift = await _baseDataWork.TimeShifts
                    .FirstOrDefaultAsync(x => x.Id == select2.TimeShiftId);

                filter = filter.And(x =>
                   x.EmployeeWorkPlaces.Any(y =>
                   y.WorkPlaceId == timeshift.WorkPlaceId) &&
                   (
                       x.FirstName.Contains(select2.Search) ||
                       x.LastName.Contains(select2.Search)
                   )
                );
                if (select2.ExistingIds.Count > 0)
                    employees = (List<Employee>)await _baseDataWork.Employees
                      .GetPaggingWithFilter(null, filter, null, 10, select2.Page);
            }
            else if (select2.TimeShiftId != 0 && string.IsNullOrWhiteSpace(select2.Search))
            {
                var timeshift = await _baseDataWork.TimeShifts
                    .FirstOrDefaultAsync(x => x.Id == select2.TimeShiftId);

                filter = filter.And(x => x.EmployeeWorkPlaces
                    .Any(y => y.WorkPlaceId == timeshift.WorkPlaceId));

                employees = (List<Employee>)await _baseDataWork.Employees
                  .GetPaggingWithFilter(null, filter, null, 10, select2.Page);
            }
            else if (!string.IsNullOrWhiteSpace(select2.Search))
            {
                filter = filter.And(x =>
                    x.FirstName.Contains(select2.Search) ||
                    x.LastName.Contains(select2.Search) ||
                    x.ErpCode.ToString().Contains(select2.Search));

                employees = (List<Employee>)await _baseDataWork.Employees
               .GetPaggingWithFilter(null, filter, null, 10, select2.Page);
            }
            else
            {
                employees = (List<Employee>)await _baseDataWork.Employees
                    .GetPaggingWithFilter(null, null, null, 10, select2.Page);
            }

            var total = await _baseDataWork.Employees.CountAllAsyncFiltered(filter);
            var hasMore = (select2.Page * 10) < total;
            return Ok(select2Helper.CreateEmployeesResponse(employees, hasMore));
        }

        // POST: api/employees/datatable
        [HttpPost("datatable")]
        public async Task<ActionResult<Employee>> Datatable([FromBody] Datatable datatable)
        {

            var total = await _baseDataWork.Employees.CountAllAsync();
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var orderDirection = datatable.Order[0].Dir;
            var filter = PredicateBuilder.New<Employee>();
            filter = filter.And(x => true);

            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.Specialization));

            var employees = new List<Employee>();

            if (string.IsNullOrWhiteSpace(datatable.Predicate))
                employees = await _baseDataWork.Employees
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), null, includes, pageSize, pageIndex);

            if (datatable.Predicate == "EmployeeIndex")
            {
                includes.Add(x => x.Include(y => y.Company));
                employees = await _baseDataWork.Employees
                      .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), null, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "CompanyEdit")
            {
                includes.Add(x => x.Include(y => y.Company));

                filter = filter.And(x =>
                     x.CompanyId == datatable.GenericId || x.CompanyId == null);

                employees = await _baseDataWork.Employees
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "WorkPlaceEdit")
            {
                includes.Add(x => x.Include(y => y.Company));

                filter = filter.And(x => x.Company.Customers
                .Any(y => y.WorkPlaces
                .Any(z => z.Id == datatable.GenericId)));

                employees = await _baseDataWork.Employees
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "WorkPlaceDetail")
            {
                includes.Add(x => x.Include(y => y.Company));

                filter = filter.And(x => x.Company.Customers
                .Any(y => y.WorkPlaces
                .Any(z => z.Id == datatable.GenericId)));

                employees = await _baseDataWork.Employees
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            }
          
            if (datatable.Predicate == "TimeShiftEdit")
            {
                var timeShift = await _baseDataWork.TimeShifts
                    .FirstOrDefaultAsync(x => x.Id == datatable.GenericId);

                filter = filter.And(x => x.EmployeeWorkPlaces
                    .Any(y => y.WorkPlaceId == timeShift.WorkPlaceId));

                employees = await _baseDataWork.Employees
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection),
                    filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "TimeShiftDetail")
            {
                var timeShift = await _baseDataWork.TimeShifts
                    .FirstOrDefaultAsync(x => x.Id == datatable.GenericId);

                filter = filter.And(x => x.EmployeeWorkPlaces
                     .Any(y => y.WorkPlaceId == timeShift.WorkPlaceId));

                employees = await _baseDataWork.Employees
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection),
                    filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "RealWorkHourIndex")
            {
                if (datatable.GenericId != 0)
                {
                    var timeShift = await _baseDataWork.TimeShifts
                        .FirstOrDefaultAsync(x => x.Id == datatable.GenericId);

                    filter = filter.And(x => x.EmployeeWorkPlaces
                        .Any(y => y.WorkPlaceId == timeShift.WorkPlaceId));
                }

                employees = await _baseDataWork.Employees
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection),
                            filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "RealWorkHourCurrentDay")
            {
                includes.Add(x => x.Include(y => y.RealWorkHours));

                var today = DateTime.Now;
                filter = filter.And(x => x.RealWorkHours
                    .Any(y => y.StartOn.Date == today.Date));

                employees = await _baseDataWork.Employees
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection),
                    filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "ProjectionDifference")
            {
                employees = await _baseDataWork.Employees
                    .ProjectionDifference(SetOrderBy(columnName, orderDirection),
                    datatable.StartOn, datatable.EndOn,
                    datatable.GenericId, pageSize, pageIndex);
            }
            if (datatable.Predicate == "ProjectionConcentric")
            {

                if (datatable.GenericId != 0)
                    filter = filter.And(x => x.EmployeeWorkPlaces
                        .Any(y => y.WorkPlaceId == datatable.GenericId));

                employees = await _baseDataWork.Employees
                     .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection),
                     filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "ProjectionRealWorkHoursAnalytically")
            {
                if (datatable.GenericId != 0)
                    filter = filter.And(x => x.EmployeeWorkPlaces
                        .Any(y => y.WorkPlaceId == datatable.GenericId));

                employees = await _baseDataWork.Employees
                     .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection),
                     filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "ProjectionRealWorkHoursAnalyticallySum")
            {
                if (datatable.GenericId != 0)
                    filter = filter.And(x => x.EmployeeWorkPlaces
                        .Any(y => y.WorkPlaceId == datatable.GenericId));

                employees = await _baseDataWork.Employees
                     .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection),
                     filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "ProjectionPresenceDaily")
            {
                if (datatable.GenericId != 0)
                    filter = filter.And(x => x.EmployeeWorkPlaces
                        .Any(y => y.WorkPlaceId == datatable.GenericId));

                employees = await _baseDataWork.Employees
                     .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection),
                     filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "ProjectionRealWorkHoursSpecific")
            {
                if (datatable.GenericId != 0)
                    filter = filter.And(x => x.EmployeeWorkPlaces
                        .Any(y => y.WorkPlaceId == datatable.GenericId));

                employees = await _baseDataWork.Employees
                     .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection),
                     filter, includes, pageSize, pageIndex);
            }
            var mapedData = MapResults(employees, datatable);

            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            return Ok(dataTableHelper.CreateResponse(datatable, await mapedData, total));
        }

        protected async Task<IEnumerable<ExpandoObject>> MapResults(IEnumerable<Employee> results, Datatable datatable)
        {
            var expandoObject = new ExpandoCopier();
            var dataTableHelper = new DataTableHelper<Employee>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var employee in results)
            {
                var expandoObj = expandoObject.GetCopyFrom<Employee>(employee);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("ScpecializationName", employee.Specialization.Name);

                if (datatable.Predicate == "EmployeeIndex")
                {
                    if (employee.Company != null)
                        dictionary.Add("CompanyTitle", employee.Company.Title);

                    dictionary.Add("Buttons", dataTableHelper.GetButtons(
                        "Employee", "Employees", employee.Id.ToString()));

                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "CompanyEdit")
                {
                    var apiUrl = UrlHelper.EmployeeCompany(employee.Id, datatable.GenericId);

                    if (employee.Company != null)
                    {
                        dictionary.Add("CompanyTitle", employee.Company.Title);
                        dictionary.Add("IsInCompany", dataTableHelper.GetToggle(
                            "Employee", apiUrl, "checked"));
                    }
                    else
                        dictionary.Add("IsInCompany", dataTableHelper.GetToggle(
                            "Employee", apiUrl, ""));

                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "WorkPlaceEdit")
                {
                    var apiUrl = UrlHelper.EmployeeWorkPlace(employee.Id, datatable.GenericId);

                    if (employee.Company != null)
                        dictionary.Add("CompanyTitle", employee.Company.Title);

                    if (_baseDataWork.EmployeeWorkPlaces
                        .Any(x => x.EmployeeId == employee.Id &&
                        x.WorkPlaceId == datatable.GenericId))
                    {
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "Employee", apiUrl, "checked"));
                    }
                    else
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "Employee", apiUrl, ""));

                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "WorkPlaceDetail")
                {
                    var apiUrl = UrlHelper.EmployeeWorkPlace(employee.Id, datatable.GenericId);

                    if (employee.Company != null)
                        dictionary.Add("CompanyTitle", employee.Company.Title);

                    if (_baseDataWork.EmployeeWorkPlaces
                        .Any(x => x.EmployeeId == employee.Id &&
                        x.WorkPlaceId == datatable.GenericId))
                    {
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "Employee", apiUrl, "checked", true));
                    }
                    else
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "Employee", apiUrl, "", true));

                    returnObjects.Add(expandoObj);
                }
            
                else if (datatable.Predicate == "TimeShiftEdit")
                {
                    var timeshift = await _baseDataWork.TimeShifts
                        .FirstOrDefaultAsync(x => x.Id == datatable.GenericId);

                    for (int i = 1; i <= DateTime.DaysInMonth(timeshift.Year, timeshift.Month); i++)
                        dictionary.Add("Day" + i,
                            dataTableHelper.GetTimeShiftEditCellBodyWorkHoursAsync(_baseDataWork,
                                i, datatable, employee.Id));

                    dictionary.Add("ToggleSlider", dataTableHelper
                        .GetEmployeeCheckbox(datatable, employee.Id));

                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "TimeShiftDetail")
                {
                    var timeshift = await _baseDataWork.TimeShifts
                        .FirstOrDefaultAsync(x => x.Id == datatable.GenericId);

                    for (int i = 1; i <= DateTime.DaysInMonth(timeshift.Year, timeshift.Month); i++)
                        dictionary.Add("Day" + i,
                            dataTableHelper.GetTimeShiftEditCellBodyWorkHoursAsync(_baseDataWork,
                                i, datatable, employee.Id));

                    dictionary.Add("ToggleSlider", dataTableHelper
                        .GetEmployeeCheckbox(datatable, employee.Id));

                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "RealWorkHourIndex")
                {
                    var compareMonth = 0;
                    var compareYear = 0;

                    if (datatable.SelectedMonth == null || datatable.SelectedYear == null)
                    {
                        var timeShift = await _baseDataWork.TimeShifts.FirstOrDefaultAsync(x => x.Id == datatable.GenericId);
                        compareMonth = timeShift.Month;
                        compareYear = timeShift.Year;
                    }
                    else
                    {
                        compareMonth = (int)datatable.SelectedMonth;
                        compareYear = (int)datatable.SelectedYear;
                    }
                    for (int i = 1; i <= DateTime.DaysInMonth(compareYear, compareMonth); i++)
                            dictionary.Add("Day" + i,
                                await dataTableHelper.GetTimeShiftEditCellBodyRealWorkHoursAsync(_baseDataWork,
                                    i, datatable, employee.Id));

                    dictionary.Add("ToggleSlider", dataTableHelper
                        .GetEmployeeCheckbox(datatable, employee.Id));

                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "RealWorkHourCurrentDay")
                {
                    dictionary.Add("StartOn", String.Join<string>("</br>",
                        employee.RealWorkHours.Select(x => x.StartOn.ToString())));

                    dictionary.Add("EndOn", String.Join<string>("</br>",
                        employee.RealWorkHours.Select(x => x.EndOn.ToString())));

                    dictionary.Add("ToggleSlider", dataTableHelper
                        .GetEmployeeCheckbox(datatable, employee.Id));

                    dictionary.Add("Buttons", dataTableHelper
                        .GetCurrentDayButtons(employee));

                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "ProjectionDifference")
                {
                    if (datatable.FilterByRealWorkHour == true ||
                        (datatable.FilterByRealWorkHour == false && datatable.FilterByWorkHour == false))
                        if (employee.RealWorkHours.Count() > 0)
                            foreach (var realWorkHour in employee.RealWorkHours)
                            {
                                expandoObj = expandoObject.GetCopyFrom<Employee>(employee);
                                dictionary = (IDictionary<string, object>)expandoObj;
                                dictionary.Add("ScpecializationName", employee.Specialization.Name);
                                dictionary.Add("RealWorkHourDate", realWorkHour.StartOn + " - " + realWorkHour.EndOn);
                                returnObjects.Add(expandoObj);

                            }

                    if (datatable.FilterByWorkHour == true ||
                        (datatable.FilterByRealWorkHour == false && datatable.FilterByWorkHour == false))
                        if (employee.WorkHours.Count() > 0)
                            foreach (var workHour in employee.WorkHours)
                            {
                                expandoObj = expandoObject.GetCopyFrom<Employee>(employee);
                                dictionary = (IDictionary<string, object>)expandoObj;
                                dictionary.Add("ScpecializationName", employee.Specialization.Name);
                                dictionary.Add("WorkHourDate", workHour.StartOn + " - " + workHour.EndOn);
                                returnObjects.Add(expandoObj);
                            }

                }
                else if (datatable.Predicate == "ProjectionConcentric")
                {
                    var totalSeconds = await _baseDataWork.RealWorkHours
                            .GetEmployeeTotalSecondsFromRange(employee.Id, datatable.StartOn, datatable.EndOn);

                    var totalSecondsDay = await _baseDataWork.RealWorkHours
                           .GetEmployeeTotalSecondsDayFromRange(employee.Id, datatable.StartOn, datatable.EndOn);

                    var totalSecondsNight = await _baseDataWork.RealWorkHours
                            .GetEmployeeTotalSecondsNightFromRange(employee.Id, datatable.StartOn, datatable.EndOn);

                    if (datatable.ShowHoursInPercentage)
                    {
                        dictionary.Add("TotalHours", totalSeconds / 60 / 60);
                        dictionary.Add("TotalHoursDay", totalSecondsDay / 60 / 60);
                        dictionary.Add("TotalHoursNight", totalSecondsNight / 60 / 60);
                    }
                    else
                    {
                        dictionary.Add("TotalHours", ((int)totalSeconds / 60 / 60).ToString() + ":" + ((int)totalSeconds / 60 % 60).ToString());
                        dictionary.Add("TotalHoursDay", ((int)totalSecondsDay / 60 / 60).ToString() + ":" + ((int)totalSecondsDay / 60 % 60).ToString());
                        dictionary.Add("TotalHoursNight", ((int)totalSecondsNight / 60 / 60).ToString() + ":" + ((int)totalSecondsNight / 60 % 60).ToString());
                    }

                    //if ((datatable.EndOn.Date - datatable.StartOn.Date).TotalDays == 0.0)
                    //{
                    //    var daySeconds = await _baseDataWork.RealWorkHours
                    //             .GetEmployeeTotalSecondsForDay(employee.Id, datatable.StartOn);

                    //    var nightSeconds = await _baseDataWork.RealWorkHours
                    //        .GetEmployeeTotalSecondsForNight(employee.Id, datatable.StartOn);
                    //    if (datatable.ShowHoursInPercentage)
                    //    {
                    //        dictionary.Add("Day_0", daySeconds / 60 / 60);
                    //        dictionary.Add("Night_0", nightSeconds / 60 / 60);
                    //    }
                    //    else
                    //    {
                    //        dictionary.Add("Day_0", ((int)daySeconds / 60 / 60).ToString() + ":" + ((int)daySeconds / 60 % 60).ToString());
                    //        dictionary.Add("Night_0", ((int)nightSeconds / 60 / 60).ToString() + ":" + ((int)nightSeconds / 60 % 60).ToString());
                    //    }
                    //}
                    //else
                    //    for (int i = 0; i <= (datatable.EndOn.Date - datatable.StartOn.Date).TotalDays; i++)
                    //    {
                    //        var compareDate = new DateTime(datatable.StartOn.AddDays(i).Ticks);
                    //        var daySeconds = await _baseDataWork.RealWorkHours
                    //               .GetEmployeeTotalSecondsForDay(employee.Id, compareDate);

                    //        var nightSeconds = await _baseDataWork.RealWorkHours
                    //            .GetEmployeeTotalSecondsForNight(employee.Id, compareDate);
                    //        if (datatable.ShowHoursInPercentage)
                    //        {
                    //            dictionary.Add("Day_" + i, daySeconds / 60 / 60);
                    //            dictionary.Add("Night_" + i, nightSeconds / 60 / 60);
                    //        }
                    //        else
                    //        {
                    //            dictionary.Add("Day_" + i, ((int)daySeconds / 60 / 60).ToString() + ":" + ((int)daySeconds / 60 % 60).ToString());
                    //            dictionary.Add("Night_" + i, ((int)nightSeconds / 60 / 60).ToString() + ":" + ((int)nightSeconds / 60 % 60).ToString());
                    //        }
                    //    }
                    returnObjects.Add(expandoObj);

                }
                else if (datatable.Predicate == "ProjectionRealWorkHoursAnalytically")
                {
                    if ((datatable.EndOn.Date - datatable.StartOn.Date).TotalDays == 0.0)
                    {
                        dictionary.Add("Day_0", await dataTableHelper
                               .GetProjectionRealWorkHoursAnalyticallyCellBodyAsync(
                               _baseDataWork, datatable.StartOn, datatable, employee.Id));
                    }
                    else
                        for (int i = 0; i <= (datatable.EndOn.Date - datatable.StartOn.Date).TotalDays; i++)
                        {
                            var compareDate = new DateTime(datatable.StartOn.AddDays(i).Ticks);
                            dictionary.Add("Day_" + i, await dataTableHelper
                                .GetProjectionRealWorkHoursAnalyticallyCellBodyAsync(
                                _baseDataWork, compareDate, datatable, employee.Id));
                        }
                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "ProjectionRealWorkHoursAnalyticallySum")
                {
                    var totalSeconds = await _baseDataWork.RealWorkHours
                            .GetEmployeeTotalSecondsFromRange(employee.Id, datatable.StartOn, datatable.EndOn);


                    if ((datatable.EndOn.Date - datatable.StartOn.Date).TotalDays == 0.0)
                    {
                        var daySeconds = await _baseDataWork.RealWorkHours
                                 .GetEmployeeTotalSecondsForDay(employee.Id, datatable.StartOn);

                        var nightSeconds = await _baseDataWork.RealWorkHours
                            .GetEmployeeTotalSecondsForNight(employee.Id, datatable.StartOn);

                        if (datatable.ShowHoursInPercentage)
                            dictionary.Add("Day_0", daySeconds / 60 / 60);
                        else
                            dictionary.Add("Day_0", ((int)daySeconds / 60 / 60).ToString() + ":" + ((int)daySeconds / 60 % 60).ToString());
                    }
                    else
                        for (int i = 0; i <= (datatable.EndOn.Date - datatable.StartOn.Date).TotalDays; i++)
                        {
                            var compareDate = new DateTime(datatable.StartOn.AddDays(i).Ticks);
                            var daySeconds = await _baseDataWork.RealWorkHours
                                   .GetEmployeeTotalSecondsForDay(employee.Id, compareDate);

                            var nightSeconds = await _baseDataWork.RealWorkHours
                                .GetEmployeeTotalSecondsForNight(employee.Id, compareDate);
                            if (datatable.ShowHoursInPercentage)
                                dictionary.Add("Day_" + i, daySeconds / 60 / 60);
                            else
                                dictionary.Add("Day_" + i, ((int)daySeconds / 60 / 60).ToString() + ":" + ((int)daySeconds / 60 % 60).ToString());
                        }
                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "ProjectionPresenceDaily")
                {
                    var realWorkHours = await _baseDataWork.RealWorkHours
                            .GetCurrentAssignedOnCell(DateTime.Now, employee.Id);

                    var todayCell = "";
                    foreach (var realWorkHour in realWorkHours)
                        todayCell += "<p style='white-space:nowrap;'>" +
                            realWorkHour.StartOn.ToShortTimeString() +
                            " - " +
                            realWorkHour.EndOn.ToShortTimeString() +
                            "</p></br>";
                    dictionary.Add("Today", todayCell);

                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "ProjectionRealWorkHoursSpecific")
                {

                    for (int i = 0; i < datatable.SpecificDates?.Count(); i++)
                    {
                        var realWorkHours = await _baseDataWork.RealWorkHours
                          .GetCurrentAssignedOnCell(datatable.SpecificDates[i], employee.Id);
                        var dayCell = "";
                        foreach (var realWorkHour in realWorkHours)
                            dayCell += "<p style='white-space:nowrap;'>" +
                                realWorkHour.StartOn.ToShortTimeString() +
                                " - " +
                                realWorkHour.EndOn.ToShortTimeString() +
                                "</p></br>";

                        dictionary.Add("Day_" + i, dayCell);

                    }

                    returnObjects.Add(expandoObj);
                }

            }
            return returnObjects;
        }

        private Func<IQueryable<Employee>, IOrderedQueryable<Employee>> SetOrderBy(string columnName, string orderDirection)
        {
            if (columnName == "WorkHourDate")
                return x => x.OrderBy(y => y.WorkHours.OrderBy(z => z.StartOn));
            else if (columnName == "RealWorkHourDate")
                return x => x.OrderBy(y => y.RealWorkHours.OrderBy(z => z.StartOn));
            else if (columnName != "")
                return x => x.OrderBy(columnName + " " + orderDirection.ToUpper());
            else
                return null;
        }

    }
}
