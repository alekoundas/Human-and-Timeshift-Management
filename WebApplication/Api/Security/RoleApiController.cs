using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Bussiness;
using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Datatable;
using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Utilities;

namespace WebApplication.Api.Security
{
    [Route("api/role")]
    [ApiController]
    public class RoleApiController : ControllerBase
    {

        private readonly SecurityDataWork _securityDatawork;
        public RoleApiController(SecurityDbContext securityDbContext)
        {
            _securityDatawork = new SecurityDataWork(securityDbContext);
        }

        // GET: api/role/get>
        [HttpPost("get")]
        public async Task<ActionResult<ApplicationRole>> Get([FromBody] Datatable datatable)
        {
            var total = _securityDatawork.ApplicationUsers.CountAll();

            var applicationRoles = await _securityDatawork.ApplicationRoles.GetAvailableControllers();

            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            var mappedData = MapResults(applicationRoles, datatable.ApplicationUserId);

            return Ok(dataTableHelper.CreateResponse(datatable, mappedData, total));
        }
        protected IEnumerable<ExpandoObject> MapResults(IEnumerable<string> results, string userId)
        {
            var expandoObject = new ExpandoCopier();
            var dataTableHelper = new DataTableHelper<string>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var result in results)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;
                dictionary.Add("Name", result);
                dictionary.Add("View", dataTableHelper.GetButtonForRoles(result, "View", userId));
                dictionary.Add("Edit", dataTableHelper.GetButtonForRoles(result, "Edit", userId));
                dictionary.Add("Create", dataTableHelper.GetButtonForRoles(result, "Create", userId));
                dictionary.Add("Delete", dataTableHelper.GetButtonForRoles(result, "Delete", userId));
                returnObjects.Add(expandoObj);
            }

            return returnObjects;
        }




        // GET api/<RoleDataController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RoleDataController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RoleDataController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RoleDataController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
