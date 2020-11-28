using Bussiness;
using Bussiness.Helpers;
using Bussiness.Service;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication.Api
{
    [Route("api/leavetypes")]
    [ApiController]
    public class LeaveTypeApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public LeaveTypeApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }
        // GET: api/leavetypes/deactivate/5
        [HttpGet("deactivate/{id}")]
        public async Task<ActionResult<DeactivateViewModel>> Deactivate(int id)
        {
            var response = new DeactivateViewModel();
            var leavetype = await _baseDataWork.LeaveTypes
                .FirstOrDefaultAsync(x => x.Id == id);

            leavetype.IsActive = !leavetype.IsActive;
            _baseDataWork.Update(leavetype);

            var status = await _baseDataWork.SaveChangesAsync();
            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Ο τύπος άδειας " +
                    leavetype.Name +
                    (leavetype.IsActive ? " ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : " ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία.";
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Ο τύπος άδειας " +
                     leavetype.Name +
                    " ΔΕΝ " +
                    (leavetype.IsActive ? "ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : "ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία";
            }

            response.ResponseTitle = "Αλλαγή κατάστασης τύπου άδειας";
            response.Entity = leavetype;

            return Ok(response);
        }

        // DELETE: api/leavetypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LeaveType>> DeleteLeaveType(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType == null)
                return NotFound();

            _context.LeaveTypes.Remove(leaveType);
            await _baseDataWork.SaveChangesAsync();

            return leaveType;
        }


        // GET: api/Select2
        [HttpGet("select2")]
        public async Task<ActionResult<LeaveType>> Select2(string search, int page)
        {
            var specializations = new List<LeaveType>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<LeaveType>();
            filter = filter.And(x => true);

            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<LeaveType>(HttpContext);

            if (!canShowDeactivated)
                filter = filter.And(x => x.IsActive == true);

            if (string.IsNullOrWhiteSpace(search))
                specializations = await _baseDataWork.LeaveTypes
                    .GetPaggingWithFilter(null, null, null, 10, page);
            else
            {
                filter = filter.And(x => x.Name.Contains(search));

                specializations = await _baseDataWork.LeaveTypes
                    .GetPaggingWithFilter(null, filter, null, 10, page);
            }
            var total = await _baseDataWork.LeaveTypes.CountAllAsyncFiltered(filter);
            var hasMore = (page * 10) < total;

            return Ok(select2Helper.CreateLeaveTypeResponse(specializations, hasMore));
        }

        // POST: api/datatable
        [HttpPost("datatable")]
        public async Task<ActionResult<LeaveType>> Datatable([FromBody] Datatable datatable)
        {
            var results = (await new DataTableService(datatable, _baseDataWork, HttpContext)
                .ConvertData<LeaveType>())
                .CompleteResponse<LeaveType>();

            return Ok(results);
            //var pageSize = datatable.Length;
            //var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            //var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            //var orderDirection = datatable.Order[0].Dir;
            //var filter = PredicateBuilder.New<LeaveType>();
            //filter = filter.And(GetSearchFilter(datatable));

            //var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<LeaveType>(HttpContext);

            //if (!canShowDeactivated)
            //    filter = filter.And(x => x.IsActive == true);

            //var includes = new List<Func<IQueryable<LeaveType>, IIncludableQueryable<LeaveType, object>>>();
            //var leaveTypes = new List<LeaveType>();

            //if (datatable.Predicate == "LeaveTypeIndex")
            //{
            //    leaveTypes = await _baseDataWork.LeaveTypes
            //        .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            //}

            //var mapedData = MapResults(leaveTypes, datatable);

            //var total = await _baseDataWork.LeaveTypes.CountAllAsyncFiltered(filter);
            //var dataTableHelper = new DataTableHelper<ExpandoObject>();
            //return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        //protected IEnumerable<ExpandoObject> MapResults(IEnumerable<LeaveType> results, Datatable datatable)
        //{
        //    var expandoObject = new ExpandoService();
        //    var dataTableHelper = new DataTableHelper<LeaveType>();
        //    List<ExpandoObject> returnObjects = new List<ExpandoObject>();
        //    foreach (var leaveTypes in results)
        //    {
        //        var expandoObj = expandoObject.GetCopyFrom<LeaveType>(leaveTypes);
        //        var dictionary = (IDictionary<string, object>)expandoObj;

        //        if (datatable.Predicate == "LeaveTypeIndex")
        //        {
        //            dictionary.Add("Buttons", dataTableHelper.GetButtons("LeaveType", "LeaveTypes", leaveTypes.Id.ToString()));
        //            returnObjects.Add(expandoObj);
        //        }
        //    }

        //    return returnObjects;
        //}


        //private Func<IQueryable<LeaveType>, IOrderedQueryable<LeaveType>> SetOrderBy(string columnName, string orderDirection)
        //{
        //    if (columnName != "")
        //        return x => x.OrderBy(columnName + " " + orderDirection.ToUpper());
        //    else
        //        return null;
        //}
        //private Expression<Func<LeaveType, bool>> GetSearchFilter(Datatable datatable)
        //{
        //    var filter = PredicateBuilder.New<LeaveType>();
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
