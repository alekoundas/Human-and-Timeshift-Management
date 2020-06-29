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
            {
                return NotFound();
            }

            var employeeWorkplaces =  _baseDataWork.EmployeeWorkPlaces
                .Where(x => x.EmployeeId == id).ToList();
            _baseDataWork.EmployeeWorkPlaces.RemoveRange(employeeWorkplaces);
            _baseDataWork.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return employee;
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
            includes.Add(x => x.Company);

            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            var applicationUsers = new List<Employee>();

            if (string.IsNullOrWhiteSpace(datatable.Predicate))
            {
                applicationUsers = await _baseDataWork.Employees.GetPaggingWithFilter(null, null, includes, pageSize, pageIndex);
            }

            if (datatable.Predicate == "CompanyEdit")
            {
                Expression<Func<Employee, bool>> filter = (x => x.CompanyId == datatable.GenericId || x.CompanyId == null);
                applicationUsers = await _baseDataWork.Employees.GetPaggingWithFilter(null, filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "WorkPlaceEdit")
            {
                Expression<Func<Employee, bool>> filter = (x => x.Company.Customers.Any(y => y.WorkPlaces.Any(z => z.Id == datatable.GenericId)));
                applicationUsers = await _baseDataWork.Employees.GetPaggingWithFilter(null, filter, includes, pageSize, pageIndex);
            }

            var mapedData = MapResults(applicationUsers, datatable.Predicate, datatable.GenericId);

            return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        protected IEnumerable<ExpandoObject> MapResults(IEnumerable<Employee> results, string datatablePredicate, int genericId = 0)
        {
            var expandoObject = new ExpandoCopier();
            var dataTableHelper = new DataTableHelper<Employee>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var result in results)
            {


                var expandoObj = expandoObject.GetCopyFrom<Employee>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("ScpecializationName", result.Specialization.Name);


                if (string.IsNullOrWhiteSpace(datatablePredicate))
                {
                    if (result.Company != null)
                        dictionary.Add("CompanyTitle", result.Company.Title);

                    dictionary.Add("Buttons", dataTableHelper.GetButtons(
                        "Employee", "Employees", result.Id.ToString()));

                    returnObjects.Add(expandoObj);
                }
                else if (datatablePredicate == "CompanyEdit")
                {
                    var apiUrl = UrlHelper.EmployeePerCompany(result.Id, genericId);

                    if (result.Company != null)
                    {
                        dictionary.Add("CompanyTitle", result.Company.Title);
                        dictionary.Add("IsInCompany", dataTableHelper.GetToggle(
                            "Employee", apiUrl, "checked"));
                    }
                    else
                        dictionary.Add("IsInCompany", dataTableHelper.GetToggle(
                            "Employee", apiUrl, ""));

                    returnObjects.Add(expandoObj);
                }
                else if (datatablePredicate == "WorkPlaceEdit")
                {
                    var apiUrl = UrlHelper.EmployeePerWorkPlace(result.Id, genericId);

                    if (result.Company != null)
                        dictionary.Add("CompanyTitle", result.Company.Title);

                    if (_baseDataWork.EmployeeWorkPlaces
                        .Any(x => x.EmployeeId == result.Id && x.WorkPlaceId == genericId))
                    {
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "Employee", apiUrl, "checked"));
                    }
                    else
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "Employee", apiUrl, ""));

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
