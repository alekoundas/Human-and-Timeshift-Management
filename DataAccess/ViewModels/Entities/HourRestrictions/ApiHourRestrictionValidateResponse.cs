using DataAccess.Models.Audit;
using Newtonsoft.Json;

namespace DataAccess.ViewModels
{
    public class ApiHourRestrictionValidateResponse : BaseEntity
    {
        [JsonProperty(PropertyName = "errorType", Required = Required.Default)]
        public string ErrorType { get; set; }

        [JsonProperty(PropertyName = "errorValue", Required = Required.Default)]
        public string ErrorValue { get; set; }

    }
}
