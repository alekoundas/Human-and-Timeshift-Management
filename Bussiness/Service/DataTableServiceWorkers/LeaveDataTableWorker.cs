using Bussiness.Helpers;
using DataAccess.Models.Datatable;
using DataAccess.Models.Entity;
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
    public class LeaveDataTableWorker : DataTableService
    {
        private BaseDatawork _baseDatawork { get; }
        private Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<Leave> _filter { get; set; } = PredicateBuilder.New<Leave>();


        public LeaveDataTableWorker(Datatable datatable, BaseDatawork baseDatawork, HttpContext httpContext)
            : base(datatable, baseDatawork, httpContext)
        {
            _datatable = datatable;
            _httpContext = httpContext;
            _baseDatawork = baseDatawork;

            //Filters for everyone
            _filter = _filter.And(GetSearchFilter());
        }

        private Func<IQueryable<Leave>, IOrderedQueryable<Leave>> SetOrderBy()
        {
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<Leave, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<Leave>();
            if (_datatable.Search.Value != null)
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "StartOn")
                        filter = filter.Or(x => x.StartOn.ToString().Contains(_datatable.Search.Value));
                    if (column.Data == "EndOn")
                        filter = filter.Or(x => x.EndOn.ToString().Contains(_datatable.Search.Value));
                    if (column.Data == "EmployeeFullName")
                        filter = filter.Or(x => x.Employee.FirstName.Contains(_datatable.Search.Value) ||
                            x.Employee.LastName.Contains(_datatable.Search.Value));
                    if (column.Data == "LeaveTypeName")
                        filter = filter.Or(x => x.LeaveType.Name.Contains(_datatable.Search.Value));
                    if (column.Data == "ApprovedBy")
                        filter = filter.Or(x => x.ApprovedBy.Contains(_datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);
            return filter;
        }


        public async Task<LeaveDataTableWorker> LeaveIndex()
        {
            var includes = new List<Func<IQueryable<Leave>, IIncludableQueryable<Leave, object>>>();
            includes.Add(x => x.Include(y => y.LeaveType));
            includes.Add(x => x.Include(y => y.Employee));

            var entities = await _baseDatawork.Leaves
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Leave>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Leave>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("EmployeeFullName", result.Employee.FullName);
                dictionary.Add("LeaveTypeName", result.LeaveType.Name);
                dictionary.Add("Buttons", dataTableHelper
                    .GetButtons("Leave", "Leaves", result.Id.ToString()));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Leaves.CountAllAsyncFiltered(_filter);

            return this;
        }
    }
}
