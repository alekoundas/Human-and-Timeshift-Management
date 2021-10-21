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
    public class ApplicationRoleDataTableWorker : DataTableService
    {
        private readonly SecurityDataWork _securityDatawork;
        private Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<ApplicationRole> _filter { get; set; } = PredicateBuilder.New<ApplicationRole>();


        public ApplicationRoleDataTableWorker(
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
            //_filter = _filter.And(GetSearchFilter());
        }

        private Func<IQueryable<ApplicationRole>, IOrderedQueryable<ApplicationRole>> SetOrderBy()
        {
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<ApplicationRole, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<ApplicationRole>();
            if (_datatable.Search.Value != null)
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "CreatedOn")
                        filter = filter.Or(x => x.Name.ToString().Contains(_datatable.Search.Value));
                    else if (column.Data == "UserName")
                        filter = filter.Or(x => x.Controller.Contains(_datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);
            return filter;
        }


        public async Task<ApplicationRoleDataTableWorker> UserEdit()
        {
            var includes = new List<Func<IQueryable<ApplicationRole>, IIncludableQueryable<ApplicationRole, object>>>();
            includes.Add(x => x.Include(y => y.UserRoles).ThenInclude(y => y.User));

            var entities = await _securityDatawork.ApplicationRoles
                .GetWithFilterQueryable(SetOrderBy(), _filter, includes, _pageSize, _pageIndex)
                .Select(x => new ApplicationRole
                {
                    Id= x.Id,
                    Controller = x.Controller,
                    Permition = x.Permition,
                    UserRoles = x.UserRoles.Select(y => new ApplicationUserRole
                    {
                        User = new ApplicationUser
                        {
                            Id = y.User.Id
                        }
                    }).ToList()
                })
                .ToListAsync();

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<ApplicationUser>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities.Select(x => x.Controller).Distinct())
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;
                dictionary.Add("Name", result);
                dictionary.Add("GreekName", result);
                dictionary.Add("View", dataTableHelper.GetButtonForRoles(result, "View", _datatable.UserId, entities));
                dictionary.Add("Edit", dataTableHelper.GetButtonForRoles(result, "Edit", _datatable.UserId, entities));
                dictionary.Add("Create", dataTableHelper.GetButtonForRoles(result, "Create", _datatable.UserId, entities));
                dictionary.Add("Deactivate", dataTableHelper.GetButtonForRoles(result, "Deactivate", _datatable.UserId, entities));
                dictionary.Add("Delete", dataTableHelper.GetButtonForRoles(result, "Delete", _datatable.UserId, entities));
                dictionary.Add("Import", dataTableHelper.GetButtonForRoles(result, "Import", _datatable.UserId, entities));
                dictionary.Add("Export", dataTableHelper.GetButtonForRoles(result, "Export", _datatable.UserId, entities));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _securityDatawork.ApplicationRoles.CountAllAsyncFiltered(_filter);

            return this;
        }
    }
}
