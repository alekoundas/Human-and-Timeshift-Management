using Newtonsoft.Json;

namespace DataAccess.ViewModels
{
    public class ApiRealWorkHourHasOverlapResponse
    {

        [JsonProperty(PropertyName = "employeeId", Required = Required.Default)]
        public int EmployeeId { get; set; }

        [JsonProperty(PropertyName = "errorType", Required = Required.Default)]
        public string ErrorType { get; set; }

        [JsonProperty(PropertyName = "errorValue", Required = Required.Default)]
        public string ErrorValue { get; set; }

    }
}
