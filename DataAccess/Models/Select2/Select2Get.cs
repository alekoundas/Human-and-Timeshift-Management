using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataAccess.Models.Select2
{
    public class Select2Get
    {
        [JsonProperty(PropertyName = "search")]
        public string Search { get; set; }

        [JsonProperty(PropertyName = "page", Required = Required.Default)]
        public int Page { get; set; }

        [JsonProperty(PropertyName = "timeShiftId")]
        public int? TimeShiftId { get; set; }

        [JsonProperty(PropertyName = "existingIds")]
        public List<int> ExistingIds { get; set; }

    }
}
