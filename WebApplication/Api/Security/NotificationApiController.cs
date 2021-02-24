using Bussiness;
using Bussiness.Helpers;
using Bussiness.Service;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Models.Security;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Api.Security
{

    [Route("api/Notifications")]
    [ApiController]
    public class NotificationApiController : ControllerBase
    {

        private readonly SecurityDataWork _securityDatawork;
        public NotificationApiController(SecurityDbContext securityDbContext)
        {
            _securityDatawork = new SecurityDataWork(securityDbContext);
        }

        // POST: api/Notification/DataTable>
        [HttpPost("datatable")]
        public async Task<ActionResult<ApplicationRole>> DataTable([FromBody] Datatable datatable)
        {
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var orderDirection = datatable.Order[0].Dir;

            var includes = new List<Func<IQueryable<Notification>, IIncludableQueryable<Notification, object>>>();

            var filter = PredicateBuilder.New<Notification>();
            filter = filter.And(x => true);


            if (datatable.Predicate == "NotificationIndex")
            {
                var loggedInUserId = HttpAccessorService.GetLoggeInUser_Id;
                filter = filter.And(x => x.ApplicationUserId == loggedInUserId);
            }


            var entities = await _securityDatawork.Notifications
                .GetPaggingWithFilter(x => x.OrderBy(y => y.CreatedOn), filter, includes, pageSize, pageIndex);

            var mapedData = await MapResults(entities, datatable);

            var dataTableHelper = new DataTableHelper<ExpandoObject>();
            var total = _securityDatawork.Notifications.Count(filter);
            return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }
        protected async Task<IEnumerable<ExpandoObject>> MapResults(List<Notification> results, Datatable datatable)
        {
            var dataTableHelper = new DataTableHelper<string>();
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var result in results)
            {
                var expandoObj = new ExpandoService().GetCopyFrom(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("Buttons", "");
                returnObjects.Add(expandoObj);
            }

            return returnObjects;
        }
    }
}
