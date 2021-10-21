using Bussiness.Helpers;
using Bussiness.Service.DataTableServiceWorkers;
using DataAccess;
using DataAccess.Libraries.Datatable;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace Bussiness.Service
{
    public class DataTableService
    {
        public Datatable _datatable { get; }
        private SecurityDataWork _securityDataWork { get; }
        private BaseDatawork _baseDatawork { get; }
        private HttpContext _httpContext { get; }
        private Object _currentWorker { get; set; }

        protected int _pageSize { get => _datatable.Length; }
        protected int _pageIndex { get => (int)Math.Ceiling((decimal)(_datatable.Start / _datatable.Length) + 1); }
        protected string _columnName { get => _datatable.Columns[_datatable.Order[0].Column].Data; }
        protected string _orderDirection { get => _datatable.Order[0].Dir; }

        public DataTableService(
            Datatable datatable,
            BaseDatawork baseDatawork,
            HttpContext httpContext,
            SecurityDataWork securityDataWork = null)
        {
            _datatable = datatable;
            _baseDatawork = baseDatawork;
            _httpContext = httpContext;

            if (securityDataWork != null)
            {
                _securityDataWork = securityDataWork;

            }
        }

        public async Task<DataTableService> ConvertData<TEntity>(string nonDbModelName = "")
        {
            //if input is string it means that cant get model type from reflection
            //so string is nessecery
            if (nonDbModelName != "")

                switch (nonDbModelName)
                {
                    case "Projection":
                        _currentWorker = new ProjectionDataTableWorker(_datatable, _baseDatawork, _httpContext);
                        break;
                    default:
                        break;
                }
            else
                switch (typeof(TEntity).Name)
                    {
                    case "LogType":
                        _currentWorker = new LogTypeDataTableWorker(_datatable, _baseDatawork, _httpContext, _securityDataWork);
                        break;
                    case "Log":
                        _currentWorker = new LogDataTableWorker(_datatable, _baseDatawork, _httpContext, _securityDataWork);
                        break;
                    case "LogEntity":
                        _currentWorker = new LogEntityDataTableWorker(_datatable, _baseDatawork, _httpContext, _securityDataWork);
                        break;
                    case "ApplicationUser":
                        _currentWorker = new ApplicationUserDataTableWorker(_datatable, _baseDatawork, _httpContext, _securityDataWork);
                        break;
                    case "ApplicationRole":
                        _currentWorker = new ApplicationRoleDataTableWorker(_datatable, _baseDatawork, _httpContext, _securityDataWork);
                        break;
                    case "Company":
                        _currentWorker = new CompanyDataTableWorker(_datatable, _baseDatawork, _httpContext);
                        break;
                    case "Contract":
                        _currentWorker = new ContractDataTableWorker(_datatable, _baseDatawork, _httpContext);
                        break;
                    case "ContractType":
                        _currentWorker = new ContractTypeDataTableWorker(_datatable, _baseDatawork, _httpContext);
                        break;
                    case "ContractMembership":
                        _currentWorker = new ContractMembershipDataTableWorker(_datatable, _baseDatawork, _httpContext);
                        break;
                    case "Employee":
                        _currentWorker = new EmployeeDataTableWorker(_datatable, _baseDatawork, _securityDataWork, _httpContext);
                        break;
                    case "Customer":
                        _currentWorker = new CustomerDataTableWorker(_datatable, _baseDatawork, _httpContext);
                        break;
                    case "Leave":
                        _currentWorker = new LeaveDataTableWorker(_datatable, _baseDatawork, _httpContext);
                        break;
                    case "LeaveType":
                        _currentWorker = new LeaveTypeDataTableWorker(_datatable, _baseDatawork, _httpContext);
                        break;
                    case "Specialization":
                        _currentWorker = new SpecializationDataTableWorker(_datatable, _baseDatawork, _httpContext);
                        break;
                    case "TimeShift":
                        _currentWorker = new TimeShiftDataTableWorker(_datatable, _baseDatawork, _httpContext);
                        break;
                    case "WorkPlaceHourRestriction":
                        _currentWorker = new WorkPlaceHourRestrictionDataTableWorker(_datatable, _baseDatawork, _httpContext);
                        break;
                    case "WorkPlace":
                        _currentWorker = new WorkPlaceDataTableWorker(_datatable, _baseDatawork, _httpContext);
                        break;
                    case "RealWorkHour":
                        _currentWorker = new RealWorkHourDataTableWorker(_datatable, _baseDatawork, _httpContext);
                        break;
                    case "Projection":
                        _currentWorker = new RealWorkHourDataTableWorker(_datatable, _baseDatawork, _httpContext);
                        break;
                    default:
                        break;
                }

            var task = (Task)_currentWorker.GetType()
            .GetMethod(_datatable.Predicate)?
            .Invoke(_currentWorker, null);

            await task.ConfigureAwait(false);

            return this;
        }

        public DatatableResponse<ExpandoObject> CompleteResponse<TEntity>()
        {
            var dataTableHelper = new DataTableHelper<ExpandoObject>();
            var mappedData = (List<ExpandoObject>)_currentWorker.GetType()
                .GetProperty("EntitiesMapped").GetValue(_currentWorker, null);

            var total = (int)_currentWorker.GetType()
                .GetProperty("EntitiesTotal").GetValue(_currentWorker, null);

            return dataTableHelper.CreateResponse(_datatable, mappedData, total);
        }
    }
}
