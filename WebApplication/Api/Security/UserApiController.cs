using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Business.Repository;
using Bussiness;
using Bussiness.Service;
using DataAccess;
using DataAccess.Models;
using DataAccess.Models.Datatable;
using DataAccess.Models.Entity;
using DataAccess.Models.Identity;
using DataAccess.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication.Utilities;
using System.Linq.Dynamic.Core;

namespace WebApplication.Api.Security
{
    [Route("api/users/")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SecurityDataWork _securityDatawork;
        public UserApiController(UserManager<ApplicationUser> userManager,
          SecurityDbContext securityDbContext)
        {
            _securityDatawork = new SecurityDataWork(securityDbContext);
            _userManager = userManager;
        }

        [HttpPost("datatable")]
        public async Task<ActionResult<ApplicationUser>> DataTable([FromBody] Datatable datatable)
        {
            var total = _securityDatawork.ApplicationUsers.CountAll();
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var orderDirection = datatable.Order[0].Dir;

            var applicationUsers = new List<ApplicationUser>();

            //TODO: order by
            if (datatable.Predicate == "UserIndex")
            {
                applicationUsers = await _securityDatawork.ApplicationUsers.GetWithPagging(SetOrderBy(columnName, orderDirection), pageSize, pageIndex);

            }

            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            var mapedData = MapResults(applicationUsers, datatable);

            return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }


        protected IEnumerable<ExpandoObject> MapResults(IEnumerable<ApplicationUser> results, Datatable datatable)
        {
            var expandoObject = new ExpandoCopier();
            var dataTableHelper = new DataTableHelper<ApplicationUser>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var user in results)
            {
                var expandoObj = expandoObject.GetCopyFrom<ApplicationUser>(user);
                var dictionary = (IDictionary<string, object>)expandoObj;

                if (datatable.Predicate == "UserIndex")
                {
                    dictionary.Add("Buttons", dataTableHelper.GetButtons("User", "Users", user.Id));
                    returnObjects.Add(expandoObj);
                }
            }

            return returnObjects;
        }

        private Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> SetOrderBy(string columnName, string orderDirection)
        {
            if (columnName != "")
                return x => x.OrderBy(columnName + " " + orderDirection.ToUpper());
            else
                return null;
        }

    }
}
