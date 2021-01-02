using Bussiness;
using Bussiness.Helpers;
using Bussiness.Service;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Libraries.Select2;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace WebApplication.Api
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersApiController : ControllerBase
    {

        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public CustomersApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // GET: api/customers/deactivate/5
        [HttpGet("deactivate/{id}")]
        public async Task<ActionResult<DeactivateViewModel>> Deactivate(int id)
        {
            var response = new DeactivateViewModel();
            var customer = await _baseDataWork.Customers
                .FirstOrDefaultAsync(x => x.Id == id);

            customer.IsActive = !customer.IsActive;
            _baseDataWork.Update(customer);

            var status = await _baseDataWork.SaveChangesAsync();
            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Ο πελάτης " +
                    customer.IdentifyingName +
                    (customer.IsActive ? " ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : " ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία.";
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Ο πελάτης " +
                     customer.IdentifyingName +
                    " ΔΕΝ " +
                    (customer.IsActive ? "ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : "ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία";
            }

            response.ResponseTitle = "Αλλαγή κατάστασης πελάτη";
            response.Entity = customer;

            return Ok(response);
        }

        // DELETE: api/customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteViewModel>> DeleteCustomer(int id)
        {
            var response = new DeleteViewModel();
            var customer = await _context.Customers
                .Include(x => x.Contacts)
                .Include(x => x.WorkPlaces).ThenInclude(x => x.WorkPlaceHourRestrictions).ThenInclude(x => x.HourRestrictions)
                .Include(x => x.WorkPlaces).ThenInclude(x => x.EmployeeWorkPlaces)
                .Include(x => x.WorkPlaces).ThenInclude(x => x.TimeShifts).ThenInclude(x => x.RealWorkHours)
                .Include(x => x.WorkPlaces).ThenInclude(x => x.TimeShifts).ThenInclude(x => x.WorkHours)
                .FirstOrDefaultAsync(x => x.Id == id);

            var customerWorkPlaces = customer.WorkPlaces;
            var customerWorkPlacesEmployeeWorkPlaces = customer.WorkPlaces.SelectMany(x => x.EmployeeWorkPlaces);
            var customerContacts = customer.Contacts;
            var customerWorkPlacesWorkHourRestrictions = customer.WorkPlaces.SelectMany(x => x.WorkPlaceHourRestrictions);
            var customerWorkPlacesWorkHourRestrictionsHourRestictions = customer.WorkPlaces.SelectMany(x => x.WorkPlaceHourRestrictions.SelectMany(y => y.HourRestrictions));
            var customerTimeShifts = customer.WorkPlaces.SelectMany(x => x.TimeShifts);
            var customerTimeShiftsRealWorHours = customer.WorkPlaces.SelectMany(x => x.TimeShifts.SelectMany(y => y.RealWorkHours));
            var customerTimeShiftsWorHours = customer.WorkPlaces.SelectMany(x => x.TimeShifts.SelectMany(y => y.WorkHours));

            if (customer == null)
                return NotFound();

            _context.Customers.Remove(customer);

            var status = await _baseDataWork.SaveChangesAsync();

            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Ο πελάτης " +
                    customer.IdentifyingName +
                    " διαγράφηκε με επιτυχία" +
                    "Επίσης διαγράφηκαν για αυτον τον πελάτη: " +
                    " Πόστα:" + customerWorkPlaces.Count().ToString() +
                    " Επαφές:" + customerContacts.Count().ToString() +
                    " Περιορισμοί πόστων:" + customerWorkPlacesWorkHourRestrictions.Count().ToString() +
                    " Χρονοδιαγράμματα:" + customerTimeShifts.Count().ToString() +
                    " Βάρδιες:" + customerTimeShiftsWorHours.Count().ToString() +
                    " Πραγματικές Βάρδιες:" + customerTimeShiftsRealWorHours.Count().ToString();
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Ο πελάτης " +
                    customer.IdentifyingName +
                    " ΔΕΝ διαγράφηκε!";
            }

            response.ResponseTitle = "Διαγραφή πελάτη";
            response.Entity = customer;
            return response;
        }

        // POST: api/customers/select2
        [HttpPost("select2")]
        public async Task<ActionResult<Employee>> Select2([FromBody] Select2 select2)
        {

            var customers = new List<Customer>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<Customer>();
            filter = filter.And(x => true);
            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<Customer>(HttpContext);

            if (!canShowDeactivated)
                filter = filter.And(x => x.IsActive == true);

            if (select2.FromEntityId != 0)
                filter = filter.And(x => x.Id == select2.FromEntityId);

            if (!string.IsNullOrWhiteSpace(select2.Search))
                filter = filter.And(x => x.IdentifyingName.Contains(select2.Search));

            customers = await _baseDataWork.Customers
                .GetPaggingWithFilter(null, filter, null, 10, select2.Page);

            var total = await _baseDataWork.Customers.CountAllAsyncFiltered(filter);
            var hasMore = (select2.Page * 10) < total;

            return Ok(select2Helper.CreateCustomersResponse(customers, hasMore));
        }

        // POST: api/customers/datatable
        [HttpPost("datatable")]
        public async Task<ActionResult<Customer>> Datatable([FromBody] Datatable datatable)
        {
            var results = (await new DataTableService(datatable, _baseDataWork, HttpContext)
                .ConvertData<Customer>())
                .CompleteResponse<Customer>();

            return Ok(results);

        }
    }
}
