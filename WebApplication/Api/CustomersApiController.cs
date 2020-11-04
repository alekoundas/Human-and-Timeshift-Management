﻿using Bussiness;
using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Datatable;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication.Utilities;

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

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            return customer;
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
                return BadRequest();

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
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

            _context.WorkHours.RemoveRange(customerTimeShiftsWorHours);
            _context.RealWorkHours.RemoveRange(customerTimeShiftsRealWorHours);
            _context.TimeShifts.RemoveRange(customerTimeShifts);
            _context.HourRestrictions.RemoveRange(customerWorkPlacesWorkHourRestrictionsHourRestictions);
            _context.WorkPlaceHourRestrictions.RemoveRange(customerWorkPlacesWorkHourRestrictions);
            _context.Contacts.RemoveRange(customerContacts);
            _context.WorkPlaces.RemoveRange(customerWorkPlaces);
            _context.EmployeeWorkPlaces.RemoveRange(customerWorkPlacesEmployeeWorkPlaces);
            _context.Customers.Remove(customer);

            var status = await _context.SaveChangesAsync();

            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Ο πελάτης " +
                    customer.ΙdentifyingΝame +
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
                    customer.ΙdentifyingΝame +
                    " ΔΕΝ διαγράφηκε!";
            }


            response.ResponseTitle = "Διαγραφή πελάτη";
            response.Entity = customer;
            return response;
        }

        // GET: api/customers/select2
        [HttpGet("select2")]
        public async Task<ActionResult<Customer>> select2(string search, int page)
        {

            var customers = new List<Customer>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<Customer>();
            filter = filter.And(x => true);

            if (string.IsNullOrWhiteSpace(search))
                customers = await _baseDataWork.Customers
                    .GetPaggingWithFilter(null, null, null, 10, page);
            else
            {
                filter = filter.And(x => x.ΙdentifyingΝame.Contains(search));

                customers = await _baseDataWork.Customers
                    .GetPaggingWithFilter(null, filter, null, 10, page);
            }
            var total = await _baseDataWork.Customers.CountAllAsyncFiltered(filter);
            var hasMore = (page * 10) < total;

            return Ok(select2Helper.CreateCustomersResponse(customers, hasMore));
        }

        // POST: api/customers/datatable
        [HttpPost("datatable")]
        public async Task<ActionResult<Customer>> Datatable([FromBody] Datatable datatable)
        {
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var orderDirection = datatable.Order[0].Dir;
            var filter = PredicateBuilder.New<Customer>();
            filter = filter.And(GetSearchFilter(datatable));

            var includes = new List<Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>>>();
            var customers = new List<Customer>();

            if (datatable.Predicate == "CustomerIndex")
            {
                customers = await _baseDataWork.Customers
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection),
                        filter, includes, pageSize, pageIndex);
            }


            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            var mapedData = await MapResults(customers, datatable);

            var total = await _baseDataWork.Customers.CountAllAsyncFiltered(filter);
            return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        protected async Task<IEnumerable<ExpandoObject>> MapResults(IEnumerable<Customer> results, Datatable datatable)
        {
            var expandoObject = new ExpandoCopier();
            var dataTableHelper = new DataTableHelper<Customer>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var result in results)
            {
                var expandoObj = expandoObject.GetCopyFrom<Customer>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                if (datatable.Predicate == "CustomerIndex")
                {
                    dictionary.Add("Buttons", dataTableHelper.GetButtons("Customer", "Customers", result.Id.ToString()));
                    returnObjects.Add(expandoObj);
                }
            }

            return returnObjects;
        }

        private Func<IQueryable<Customer>, IOrderedQueryable<Customer>> SetOrderBy(string columnName, string orderDirection)
        {
            if (columnName != "")
                return x => x.OrderBy(columnName + " " + orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<Customer, bool>> GetSearchFilter(Datatable datatable)
        {
            var filter = PredicateBuilder.New<Customer>();
            if (datatable.Search.Value != null)
            {
                foreach (var column in datatable.Columns)
                {
                    if (column.Data == "ΙdentifyingΝame")
                        filter = filter.Or(x => x.ΙdentifyingΝame.Contains(datatable.Search.Value));
                    if (column.Data == "Profession")
                        filter = filter.Or(x => x.Profession.Contains(datatable.Search.Value));
                    if (column.Data == "Address")
                        filter = filter.Or(x => x.Address.Contains(datatable.Search.Value));
                    if (column.Data == "PostalCode")
                        filter = filter.Or(x => x.PostalCode.Contains(datatable.Search.Value));
                    if (column.Data == "DOY")
                        filter = filter.Or(x => x.DOY.Contains(datatable.Search.Value));
                    if (column.Data == "AFM")
                        filter = filter.Or(x => x.AFM.Contains(datatable.Search.Value));
                }

            }
            else
                filter = filter.And(x => true);

            return filter;
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
