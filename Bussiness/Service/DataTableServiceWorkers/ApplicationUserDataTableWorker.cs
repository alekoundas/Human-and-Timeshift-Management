using Bussiness.Helpers;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Models.Security;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bussiness.Service.DataTableServiceWorkers
{
    public class ApplicationUserDataTableWorker : DataTableService
    {
        private readonly SecurityDataWork _securityDatawork;
        private Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<ApplicationUser> _filter { get; set; } = PredicateBuilder.New<ApplicationUser>();


        public ApplicationUserDataTableWorker(
            Datatable datatable,
            BaseDatawork baseDatawork,
            HttpContext httpContext,
            SecurityDataWork securityDatawork)
            : base(datatable, baseDatawork, httpContext, securityDatawork)
        {
            _datatable = datatable;
            _httpContext = httpContext;
            _securityDatawork = securityDatawork;

            _filter = _filter.And(x => true);
            //Filters for everyone
            _filter = _filter.And(GetSearchFilter());
        }

        private Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> SetOrderBy()
        {
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<ApplicationUser, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<ApplicationUser>();
            if (_datatable.Search.Value != null)
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "CreatedOn")
                        filter = filter.Or(x => x.CreatedOn.ToString().Contains(_datatable.Search.Value));
                    else if (column.Data == "UserName")
                        filter = filter.Or(x => x.UserName.Contains(_datatable.Search.Value));
                    else if (column.Data == "Email")
                        filter = filter.Or(x => x.Email.Contains(_datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);
            return filter;
        }


        public async Task<ApplicationUserDataTableWorker> ApplicationUserIndex()
        {
            var includes = new List<Func<IQueryable<ApplicationUser>, IIncludableQueryable<ApplicationUser, object>>>();

            var entities = await _securityDatawork.ApplicationUsers
                .GetWithFilterQueryable(SetOrderBy(), _filter, includes, _pageSize, _pageIndex)
                .Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.Email
                })
                .ToListAsync();

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<ApplicationUser>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom(result);
                var dictionary = (IDictionary<string, object>)expandoObj;
                dictionary.Add("Buttons", dataTableHelper.GetButtons("User", "Users", result.Id));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _securityDatawork.ApplicationUsers.CountAllAsyncFiltered(_filter);

            return this;
        }
    }
}
