using Bussiness;
using Bussiness.Helpers;
using Bussiness.Service;
using Bussiness.SignalR.Hubs;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Libraries.Select2;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApplication.Api
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesApiController : ControllerBase
    {
        private BaseDbContext _context;
        private readonly BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        private readonly IHubContext<ConnectionHub> _notificationHubContext;
        public EmployeesApiController(
            BaseDbContext BaseDbContext,
            SecurityDbContext SecurityDbContext,
            IHubContext<ConnectionHub> notificationHubContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
            _notificationHubContext = notificationHubContext;
        }

        [HttpGet("testapi")]
        public async Task<ActionResult<Employee>> testapiAsync()
        {


            var sdsdsd = _notificationHubContext.Clients.All;
            await _notificationHubContext.Clients.All.SendAsync("sendToUser", "mpleeeeee", "skata");
            return Ok();
        }

        // GET: api/employees/deactivate/5
        [HttpGet("deactivate/{id}")]
        public async Task<ActionResult<DeactivateViewModel>> Deactivate(int id)
        {
            var response = new DeactivateViewModel();
            var employee = await _baseDataWork.Employees
                .FirstOrDefaultAsync(x => x.Id == id);

            employee.IsActive = !employee.IsActive;
            _baseDataWork.Update(employee);

            var status = await _baseDataWork.SaveChangesAsync();
            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Ο υπάλληλος " +
                    employee.FullName +
                    (employee.IsActive ? " ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : " ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία.";
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Ο υπάλληλος " +
                     employee.FullName +
                    " ΔΕΝ " +
                    (employee.IsActive ? "ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : "ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία";
            }

            response.ResponseTitle = "Αλλαγή κατάστασης υπαλλήλου ";
            response.Entity = employee;

            return Ok(response);
        }


        // DELETE: api/employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteViewModel>> Delete(int id)
        {
            var response = new DeleteViewModel();
            var employee = await _context
                .Employees
                .Include(x => x.EmployeeWorkPlaces)
                .Include(x => x.Leaves)
                .Include(x => x.WorkHours)
                .Include(x => x.RealWorkHours)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (employee == null)
                return NotFound();

            var employeeWorkplaces = employee.EmployeeWorkPlaces;
            var employeeLeaves = employee.Leaves;
            var employeeWorkHours = employee.WorkHours;
            var employeeRealWorkHours = employee.RealWorkHours;

            _baseDataWork.Employees.Remove(employee);
            var status = await _baseDataWork.SaveChangesAsync();

            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Ο υπάλληλος " +
                    employee.FullName +
                    " διαγράφηκε με επιτυχία." +
                    "Επίσης διαγράφηκαν για αυτον τον υπάλληλο: " +
                    " Άδειες:" + employeeLeaves.Count +
                    " Βάρδιες:" + employeeWorkHours.Count +
                    " Πραγματικές Βάρδιες:" + employeeRealWorkHours.Count +
                    "";
            }

            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Ο υπάλληλος " +
                    employee.FullName +
                    " ΔΕΝ διαγράφηκε!";
            }

            response.ResponseTitle = "Διαγραφή υπαλλήλου";
            response.Entity = employee;
            return response;
        }



        // GET: api/employees/getSelet2Option/id
        [HttpGet("getselect2option/{id}")]
        public async Task<ActionResult<Employee>> GetSelet2Option(int id)
        {
            var select2Helper = new Select2Helper();
            Expression<Func<Employee, bool>> filter = x => x.Id == id;

            var employees = (List<Employee>)await _baseDataWork.Employees
                     .GetPaggingWithFilter(null, filter, null, 1);

            return Ok(select2Helper.CreateEmployeesResponse(employees, false));
        }


        // POST: api/employees/select2
        [HttpPost("select2")]
        public async Task<ActionResult<Employee>> Select2([FromBody] Select2Get select2)
        {
            var employees = new List<Employee>();
            var select2Helper = new Select2Helper();
            var total = await _baseDataWork.Employees.CountAllAsync();
            var hasMore = (select2.Page * 10) < total;
            var filter = PredicateBuilder.New<Employee>();
            var parentFilter = filter;

            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<Employee>(HttpContext);

            if (!canShowDeactivated)
                filter = filter.And(x => x.IsActive == true);

            if (select2.TimeShiftId != null)
                filter = filter.And(x => x.EmployeeWorkPlaces
                .Any(y => y.WorkPlace.TimeShifts
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

        // POST: api/employees/select2
        [HttpPost("select2filtered")]
        public async Task<ActionResult<Employee>> Select2Filtered([FromBody] Select2FilteredGet select2)
        {
            var employees = new List<Employee>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<Employee>();

            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<Employee>(HttpContext);

            if (!canShowDeactivated)
                filter = filter.And(x => x.IsActive == true);

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
            var results = (await new DataTableService(datatable, _baseDataWork, HttpContext, _securityDatawork)
                .ConvertData<Employee>())
                .CompleteResponse<Employee>();

            return Ok(results);

        }
    }
}
