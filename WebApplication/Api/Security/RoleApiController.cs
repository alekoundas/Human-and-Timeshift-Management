using Bussiness;
using Bussiness.Helpers;
using Bussiness.Service;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Models.Identity;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

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

            var dataTableHelper = new DataTableHelper<ExpandoObject>();
            return Ok(dataTableHelper.CreateResponse(datatable, await mapedData, total));



        }
        protected async Task<IEnumerable<ExpandoObject>> MapResults(IEnumerable<string> results, Datatable datatable)
        {
            var expandoObject = new ExpandoService();
            var dataTableHelper = new DataTableHelper<string>();
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var result in results)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                var applicationRoles = await _securityDatawork.ApplicationRoles.GetAllAsync();
                var userRoles = _securityDatawork.ApplicationUserRoles.GetRolesFormUserId(datatable.UserId);
                if (datatable.Predicate == "UserEdit")
                {
                    dictionary.Add("Name", result);
                    dictionary.Add("GreekName", result);
                    dictionary.Add("View", dataTableHelper.GetButtonForRoles(result, "View", datatable.UserId, userRoles, applicationRoles));
                    dictionary.Add("Edit", dataTableHelper.GetButtonForRoles(result, "Edit", datatable.UserId, userRoles, applicationRoles));
                    dictionary.Add("Create", dataTableHelper.GetButtonForRoles(result, "Create", datatable.UserId, userRoles, applicationRoles));
                    dictionary.Add("Deactivate", dataTableHelper.GetButtonForRoles(result, "Deactivate", datatable.UserId, userRoles, applicationRoles));
                    dictionary.Add("Delete", dataTableHelper.GetButtonForRoles(result, "Delete", datatable.UserId, userRoles, applicationRoles));
                    returnObjects.Add(expandoObj);

                }
                else if (datatable.Predicate == "UserProfile")
                {
                    dictionary.Add("Name", result);
                    dictionary.Add("GreekName", result);
                    dictionary.Add("View", dataTableHelper.GetButtonForRoles(result, "View", datatable.UserId, userRoles, applicationRoles));
                    dictionary.Add("Edit", dataTableHelper.GetButtonForRoles(result, "Edit", datatable.UserId, userRoles, applicationRoles));
                    dictionary.Add("Create", dataTableHelper.GetButtonForRoles(result, "Create", datatable.UserId, userRoles, applicationRoles));
                    dictionary.Add("Deactivate", dataTableHelper.GetButtonForRoles(result, "Deactivate", datatable.UserId, userRoles, applicationRoles));
                    dictionary.Add("Delete", dataTableHelper.GetButtonForRoles(result, "Delete", datatable.UserId, userRoles, applicationRoles));
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
