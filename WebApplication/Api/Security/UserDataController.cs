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

namespace WebApplication.Api.Security
{
    [Route("api/users/")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SecurityDataWork _securityDatawork;
        public UserDataController(UserManager<ApplicationUser> userManager,
          SecurityDbContext securityDbContext)
        {
            _securityDatawork = new SecurityDataWork(securityDbContext);
            _userManager = userManager;
        }

        [HttpPost("get")]
        public async Task<ActionResult<ApplicationUser>> Get([FromBody] Datatable datatable)
        {
            var total = _securityDatawork.ApplicationUsers.CountAll();
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var isDescending = datatable.Order[0].Dir == "desc";

            //TODO: order by
            var applicationUsers = await _securityDatawork.ApplicationUsers.GetWithPagging(null, pageSize, pageIndex);

            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            var mapedData = MapResults(applicationUsers);

            return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }


        protected IEnumerable<ExpandoObject> MapResults(IEnumerable<ApplicationUser> results)
        {
            var expandoObject = new ExpandoCopier();
            var dataTableHelper = new DataTableHelper<ApplicationUser>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var result in results)
            {
                var expandoObj = expandoObject.GetCopyFrom<ApplicationUser>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;
                dictionary.Add("Buttons", dataTableHelper.GetButtons("User","Users", result.Id));
                returnObjects.Add(expandoObj);
            }

            return returnObjects;
        }

    }
}
