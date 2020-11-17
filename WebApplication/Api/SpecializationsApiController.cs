using Bussiness;
using Bussiness.Helpers;
using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Datatable;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication.Api
{
    [Route("api/specializations")]
    [ApiController]
    public class SpecializationsApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public SpecializationsApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // GET: api/specializations/deactivate/5
        [HttpGet("deactivate/{id}")]
        public async Task<ActionResult<DeactivateViewModel>> Deactivate(int id)
        {
            var response = new DeactivateViewModel();
            var specialization = await _baseDataWork.Specializations
                .FirstOrDefaultAsync(x => x.Id == id);

            specialization.IsActive = !specialization.IsActive;
            _baseDataWork.Update(specialization);

            var status = await _context.SaveChangesAsync();
            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Η ειδικότητα " +
                    specialization.Name +
                    (specialization.IsActive ? " ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : " ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία.";
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Η ειδικότητα " +
                     specialization.Name +
                    " ΔΕΝ " +
                    (specialization.IsActive ? "ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : "ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία";
            }

            response.ResponseTitle = "Αλλαγή κατάστασης ειδικότητας";
            response.Entity = specialization;

            return Ok(response);
        }

        // DELETE: api/specializations/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteViewModel>> DeleteSpecialization(int id)
        {
            var response = new DeleteViewModel();
            var specialization = await _context.Specializations.FindAsync(id);
            if (specialization == null)
                return NotFound();

            _context.Specializations.Remove(specialization);
            var status = await _context.SaveChangesAsync();

            if (status >= 1)
                response.ResponseBody = "Η ειδικότητα" +
                    specialization.Name +
                    " διαγράφηκε με επιτυχία";
            else
                response.ResponseBody = "Ωχ! Η ειδικότητα" +
                    specialization.Name +
                    " ΔΕΝ διαγράφηκε!";


            response.ResponseTitle = "Διαγραφή ειδικότητας";
            response.Entity = specialization;
            return response;
        }

        // GET: api/select2
        [HttpGet("select2")]
        public async Task<ActionResult<Specialization>> Select2(string search, int page)
        {
            var specializations = new List<Specialization>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<Specialization>();
            filter = filter.And(x => true);
            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<Specialization>(HttpContext);

            if (!canShowDeactivated)
                filter = filter.And(x => x.IsActive == true);

            if (string.IsNullOrWhiteSpace(search))
                specializations = await _baseDataWork.Specializations
                    .GetPaggingWithFilter(null, null, null, 10, page);
            else
            {
                filter = filter.And(x => x.Name.Contains(search));

                specializations = await _baseDataWork.Specializations
                    .GetPaggingWithFilter(null, filter, null, 10, page);
            }
            var total = await _baseDataWork.Specializations.CountAllAsyncFiltered(filter);
            var hasMore = (page * 10) < total;

            return Ok(select2Helper.CreateSpecializationResponse(specializations, hasMore));
        }

        // POST: api/datatable
        [HttpPost("datatable")]
        public async Task<ActionResult<Specialization>> Datatable([FromBody] Datatable datatable)
        {
            var results = (await new DataTableService(datatable, _baseDataWork, HttpContext)
                .ConvertData<Specialization>())
                .CompleteResponse<Specialization>();

            return Ok(results);
            //var pageSize = datatable.Length;
            //var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            //var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            //var orderDirection = datatable.Order[0].Dir;
            //var filter = PredicateBuilder.New<Specialization>();
            //filter = filter.And(GetSearchFilter(datatable));

            //var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<Specialization>(HttpContext);

            //if (!canShowDeactivated)
            //    filter = filter.And(x => x.IsActive == true);

            //var includes = new List<Func<IQueryable<Specialization>, IIncludableQueryable<Specialization, object>>>();
            //var specializations = new List<Specialization>();

            //if (datatable.Predicate == "SpecializationIndex")
            //{
            //    specializations = await _baseDataWork.Specializations
            //        .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            //}

            //var mapedData = MapResults(specializations, datatable);

            //var total = await _baseDataWork.Specializations.CountAllAsyncFiltered(filter);
            //var dataTableHelper = new DataTableHelper<ExpandoObject>();
            //return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        //protected IEnumerable<ExpandoObject> MapResults(IEnumerable<Specialization> results, Datatable datatable)
        //{
        //    var expandoObject = new ExpandoService();
        //    var dataTableHelper = new DataTableHelper<Specialization>();
        //    List<ExpandoObject> returnObjects = new List<ExpandoObject>();
        //    foreach (var specialization in results)
        //    {
        //        var expandoObj = expandoObject.GetCopyFrom<Specialization>(specialization);
        //        var dictionary = (IDictionary<string, object>)expandoObj;

        //        if (datatable.Predicate == "SpecializationIndex")
        //        {

        //            dictionary.Add("Buttons", dataTableHelper.GetButtons("Specialization", "Specializations", specialization.Id.ToString()));
        //            returnObjects.Add(expandoObj);
        //        }
        //    }

        //    return returnObjects;
        //}


        //private Func<IQueryable<Specialization>, IOrderedQueryable<Specialization>> SetOrderBy(string columnName, string orderDirection)
        //{
        //    if (columnName != "")
        //        return x => x.OrderBy(columnName + " " + orderDirection.ToUpper());
        //    else
        //        return null;
        //}
        //private Expression<Func<Specialization, bool>> GetSearchFilter(Datatable datatable)
        //{
        //    var filter = PredicateBuilder.New<Specialization>();
        //    if (datatable.Search.Value != null)
        //    {
        //        foreach (var column in datatable.Columns)
        //        {
        //            if (column.Data == "Name")
        //                filter = filter.Or(x => x.Name.Contains(datatable.Search.Value));
        //            if (column.Data == "Description")
        //                filter = filter.Or(x => x.Description.Contains(datatable.Search.Value));
        //        }

        //    }
        //    else
        //        filter = filter.And(x => true);

        //    return filter;
        //}



    }
}
