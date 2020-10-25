using DataAccess.Models;
using Newtonsoft.Json;

namespace DataAccess.ViewModels.HourRestrictions
{
    public class ApiHourRestrictionValidateResponse : BaseEntity
    {
        [JsonProperty(PropertyName = "errorType", Required = Required.Default)]
        public string ErrorType { get; set; }

        [JsonProperty(PropertyName = "errorValue", Required = Required.Default)]
        public string ErrorValue { get; set; }

    }
}
