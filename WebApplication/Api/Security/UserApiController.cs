using Bussiness;
using Bussiness.Helpers;
using Bussiness.Service;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Libraries.Select2;
using DataAccess.Models.Security;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace WebApplication.Api.Security
{
    [Route("api/users")]
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

        [HttpPost("resetpassword/{userId}")]
        public async Task<ActionResult<ApplicationUser>> ResetPassword(string userId)
        {
            var user = await _securityDatawork.ApplicationUsers
                   .FirstOrDefaultAsync(x => x.Id == userId);


            user.HasToChangePassword = true;

            var status = await _userManager.UpdateAsync(user);
            //if (!status.Succeeded)
            //{
            //}

            return Ok(new { });
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

            var dataTableHelper = new DataTableHelper<ExpandoObject>();
            var mapedData = MapResults(applicationUsers, datatable);

            return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        // DELETE: api/specializations/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteViewModel>> Delete(string id)
        {
            var response = new DeleteViewModel();
            var user = await _securityDatawork.ApplicationUsers
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound();

            _securityDatawork.ApplicationUsers.Remove(user);
            var status = await _securityDatawork.SaveChangesAsync();

            if (status >= 1)
                response.ResponseBody = "Ο χρήστης" +
                    user.FirstName + " " +
                    user.LastName +
                    " διαγράφηκε με επιτυχία";
            else
                response.ResponseBody = "Ωχ! Ο χρήστης" +
                    user.FirstName + " " +
                    user.LastName +
                    " ΔΕΝ διαγράφηκε!";


            response.ResponseTitle = "Διαγραφή χρήστη";
            response.Entity = user;
            return response;
        }

        protected IEnumerable<ExpandoObject> MapResults(IEnumerable<ApplicationUser> results, Datatable datatable)
        {
            var expandoObject = new ExpandoService();
            var dataTableHelper = new DataTableHelper<ApplicationUser>();
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


        // POST: api/users/select2
        [HttpPost("select2")]
        public async Task<ActionResult<ApplicationUser>> Select2([FromBody] Select2 select2)
        {
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<ApplicationUser>();
            filter = filter.And(x => true);

            if (select2.FromEntityIdString?.Length > 0)
                filter = filter.And(x => x.Id == select2.FromEntityIdString);

            if (select2.ExistingIdsString?.Count > 0)
                foreach (var id in select2.ExistingIdsString)
                    filter = filter.And(x => x.Id != id);

            if (!string.IsNullOrWhiteSpace(select2.Search))
                filter = filter.And(x => x.FirstName.Contains(select2.Search) || x.LastName.Contains(select2.Search));

            var entities = await _securityDatawork.ApplicationUsers
                .GetPaggingWithFilter(null, filter, null, 10, select2.Page);

            var total = await _securityDatawork.ApplicationUsers.CountAllAsyncFiltered(filter);
            var hasMore = (select2.Page * 10) < total;

            return Ok(select2Helper.CreateUsersResponse(entities, hasMore));
        }

    }
}
