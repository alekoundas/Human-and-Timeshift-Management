using Newtonsoft.Json;

namespace DataAccess.ViewModels.Validation
{
    public class ApiValidationResult
    {
        [JsonProperty(PropertyName = "employeeId")]
        public int EmployeeId { get; set; }

        [JsonProperty(PropertyName = "responseType")]
        public string ResponseType { get; set; }

        [JsonProperty(PropertyName = "responseValue")]
        public string ResponseValue { get; set; }
    }
}
