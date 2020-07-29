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

namespace WebApplication.Api
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesApiController : ControllerBase
    {
        private  BaseDbContext _context;
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
            {
                return NotFound();
            }

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

            return Ok(select2Helper.CreateEmployeesResponse(employees));
        }

        // GET: api/employees/select2
        [HttpPost("select2")]
        public async Task<ActionResult<Employee>> Select2([FromBody] Select2Get select2)
        {
            var employees = new List<Employee>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<Employee>();

            if (select2.ExistingEmployees?.Count > 0)
                foreach (var employeeId in select2.ExistingEmployees)
                    filter = filter.And(x => x.Id != employeeId);



            if (select2.TimeShiftId != 0 && !string.IsNullOrWhiteSpace(select2.Search))
            {
                var timeshift = await _baseDataWork.TimeShifts.FirstOrDefaultAsync(x => x.Id == select2.TimeShiftId);
                filter = filter.And(x =>
                   x.EmployeeWorkPlaces.Any(y =>
                   y.WorkPlaceId == timeshift.WorkPlaceId) &&
                   (
                       x.FirstName.Contains(select2.Search) ||
                       x.LastName.Contains(select2.Search)
                   )
                );
                if (select2.ExistingEmployees.Count > 0)


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

            return Ok(select2Helper.CreateEmployeesResponse(employees));
        }

        // POST: api/employees/getdatatable
        [HttpPost("getdatatable")]
        public async Task<ActionResult<Employee>> GetDatatable([FromBody] Datatable datatable)
        {

            var total = await _baseDataWork.Employees.CountAllAsync();
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var isDescending = datatable.Order[0].Dir == "desc";

            var includes = new List<Expression<Func<Employee, object>>>();
            includes.Add(x => x.Specialization);

            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            var employees = new List<Employee>();

            if (string.IsNullOrWhiteSpace(datatable.Predicate))
            {
                employees = await _baseDataWork.Employees
                    .GetPaggingWithFilter(null, null, includes, pageSize, pageIndex);
            }

            if (datatable.Predicate == "CompanyEdit")
            {
                Expression<Func<Employee, bool>> filter =
                    (x => x.CompanyId == datatable.GenericId || x.CompanyId == null);
                employees = await _baseDataWork.Employees
                    .GetPaggingWithFilter(null, filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "WorkPlaceEdit")
            {
            includes.Add(x => x.Company);
                Expression<Func<Employee, bool>> filter = (x => x.Company.Customers
                .Any(y => y.WorkPlaces
                .Any(z => z.Id == datatable.GenericId)));

                employees = await _baseDataWork.Employees
                    .GetPaggingWithFilter(null, filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "TimeShiftEdit")
            {
                var timeShift = await _baseDataWork.TimeShifts.FirstOrDefaultAsync(x => x.Id == datatable.GenericId);

                Expression<Func<Employee, bool>> filter = x => x.EmployeeWorkPlaces
                    .Any(y => y.WorkPlaceId == timeShift.WorkPlaceId);

                employees = await _baseDataWork.Employees
                    .GetPaggingWithFilter(null, filter, includes, pageSize, pageIndex);
            } 
            if (datatable.Predicate == "RealWorkHourCurrentDay")
            {
                includes.Add(x => x.RealWorkHours);

                var today = DateTime.Now;
                Expression<Func<Employee, bool>> filter = x => x.RealWorkHours
                    .Any(y => y.StartOn.Date == today.Date);

                employees = await _baseDataWork.Employees
                    .GetPaggingWithFilter(null, filter, includes, pageSize, pageIndex);
            }

            var mapedData = MapResults(employees, datatable);

            return Ok(dataTableHelper.CreateResponse(datatable,await mapedData, total));
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

                if (string.IsNullOrWhiteSpace(datatable.Predicate))
                {
                    if (employee.Company != null)
                        dictionary.Add("CompanyTitle", employee.Company.Title);

                    dictionary.Add("Buttons", dataTableHelper.GetButtons(
                        "Employee", "Employees", employee.Id.ToString()));

                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "CompanyEdit")
                {
                    var apiUrl = UrlHelper.EmployeePerCompany(employee.Id, datatable.GenericId);

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
                    var apiUrl = UrlHelper.EmployeePerWorkPlace(employee.Id, datatable.GenericId);

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
                else if (datatable.Predicate == "TimeShiftEdit")
                {
                    var timeshift = await _baseDataWork.TimeShifts
                        .FirstOrDefaultAsync(x => x.Id == datatable.GenericId);

                    for (int i = 1; i <= DateTime.DaysInMonth(timeshift.Year, timeshift.Month); i++)
                        dictionary.Add("Day" + i,
                            dataTableHelper.GetHoverElementsAsync(_baseDataWork,
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
            }
            return returnObjects;
        }


        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
