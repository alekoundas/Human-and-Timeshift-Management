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
using LinqKit;
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

        // POST: api/role/DataTable>
        [HttpPost("datatable")]
        public async Task<ActionResult<ApplicationRole>> DataTable([FromBody] Datatable datatable)
        {
            var total = await _securityDatawork.ApplicationRoles.CountAllAsync();
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var orderDirection = datatable.Order[0].Dir;
            var filter = PredicateBuilder.New<ApplicationRole>();
            filter = filter.And(x => true);

            var applicationRoles = new List<string>();


            //var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            //var mappedData = MapResults(applicationRoles, datatable.ApplicationUserId);




            if (datatable.Predicate == "UserEdit")
            {
                applicationRoles = await _securityDatawork.ApplicationRoles.GetAvailableControllers();
            }
            if (datatable.Predicate == "UserProfile")
            {
                applicationRoles = await _securityDatawork.ApplicationRoles.GetAvailableControllers();
            }


            var mapedData = MapResults(applicationRoles, datatable);

            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            return Ok(dataTableHelper.CreateResponse(datatable, await mapedData, total));



        }
        protected async Task<IEnumerable<ExpandoObject>> MapResults(IEnumerable<string> results, Datatable datatable)
        {
            var expandoObject = new ExpandoCopier();
            var dataTableHelper = new DataTableHelper<string>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var result in results)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;


                if (datatable.Predicate == "UserEdit")
                {
                    dictionary.Add("Name", result);
                    dictionary.Add("View", dataTableHelper.GetButtonForRoles(result, "View", datatable.UserId));
                    dictionary.Add("Edit", dataTableHelper.GetButtonForRoles(result, "Edit", datatable.UserId));
                    dictionary.Add("Create", dataTableHelper.GetButtonForRoles(result, "Create", datatable.UserId));
                    dictionary.Add("Delete", dataTableHelper.GetButtonForRoles(result, "Delete", datatable.UserId));
                    returnObjects.Add(expandoObj);

                }
                else if (datatable.Predicate == "UserProfile")
                {
                    dictionary.Add("Name", result);
                    dictionary.Add("View", dataTableHelper.GetButtonForRoles(result, "View", datatable.UserId));
                    dictionary.Add("Edit", dataTableHelper.GetButtonForRoles(result, "Edit", datatable.UserId));
                    dictionary.Add("Create", dataTableHelper.GetButtonForRoles(result, "Create", datatable.UserId));
                    dictionary.Add("Delete", dataTableHelper.GetButtonForRoles(result, "Delete", datatable.UserId));
                    returnObjects.Add(expandoObj);
                }
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
