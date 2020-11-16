using Bussiness.Helpers;
using DataAccess.Models.Datatable;
using DataAccess.Models.Entity;
using LinqKit;
using Microsoft.AspNetCore.Http;
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
    public class SpecializationDataTableWorker : DataTableService
    {
        private BaseDatawork _baseDatawork { get; }
        private Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<Specialization> _filter { get; set; } = PredicateBuilder.New<Specialization>();


        public SpecializationDataTableWorker(Datatable datatable, BaseDatawork baseDatawork, HttpContext httpContext)
            : base(datatable, baseDatawork, httpContext)
        {
            _datatable = datatable;
            _httpContext = httpContext;
            _baseDatawork = baseDatawork;

            //Filters for everyone
            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<Specialization>(_httpContext);

            if (!canShowDeactivated)
                _filter = _filter.And(x => x.IsActive == true);
            _filter = _filter.And(GetSearchFilter());
        }

        private Func<IQueryable<Specialization>, IOrderedQueryable<Specialization>> SetOrderBy()
        {
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<Specialization, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<Specialization>();
            if (_datatable.Search.Value != null)
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "Name")
                        filter = filter.Or(x => x.Name.Contains(_datatable.Search.Value));
                    if (column.Data == "Description")
                        filter = filter.Or(x => x.Description.Contains(_datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);
            return filter;
        }


        public async Task<SpecializationDataTableWorker> SpecializationIndex()
        {
            var includes = new List<Func<IQueryable<Specialization>, IIncludableQueryable<Specialization, object>>>();

            var entities = await _baseDatawork.Specializations
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Specialization>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Specialization>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("Buttons", dataTableHelper
                    .GetButtons("Specialization", "Specializations", result.Id.ToString()));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Specializations.CountAllAsyncFiltered(_filter);

            return this;
        }
    }
}
