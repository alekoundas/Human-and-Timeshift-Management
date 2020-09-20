using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity;
using WebApplication.Utilities;
using Bussiness;
using System.Dynamic;
using Bussiness.Service;
using DataAccess.Models.Datatable;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Dynamic.Core;
using LinqKit;

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
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
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
            var total = await _baseDataWork.Customers.CountAllAsync();
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var orderDirection = datatable.Order[0].Dir;

            var includes = new List<Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>>>();
            var customers = new List<Customer>();

            if (string.IsNullOrWhiteSpace(datatable.Predicate))
            {

                customers = await _baseDataWork.Customers.GetPaggingWithFilter(null, null, null, pageSize, pageIndex);
            }

            if (datatable.Predicate == "CustomerIndex")
            {
                customers = await _baseDataWork.Customers
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), null, includes, pageSize, pageIndex);
            }


            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            var mapedData = MapResults(customers);

            return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        protected IEnumerable<ExpandoObject> MapResults(IEnumerable<Customer> results)
        {
            var expandoObject = new ExpandoCopier();
            var dataTableHelper = new DataTableHelper<Customer>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var result in results)
            {
                var expandoObj = expandoObject.GetCopyFrom<Customer>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;
                dictionary.Add("Buttons", dataTableHelper.GetButtons("Customer", "Customers", result.Id.ToString()));
                returnObjects.Add(expandoObj);
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

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
