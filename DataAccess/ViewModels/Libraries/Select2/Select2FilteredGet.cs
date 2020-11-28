using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DataAccess.Libraries.Select2
{
    public class Select2FilteredGet
    {
        [JsonProperty(PropertyName = "search", Required = Required.Default)]
        public string Search { get; set; }

        [JsonProperty(PropertyName = "page", Required = Required.Default)]
        public int Page { get; set; }

        [JsonProperty(PropertyName = "timeShiftId")]
        public int TimeShiftId { get; set; }

        [JsonProperty(PropertyName = "existingIds")]
        public List<int> ExistingIds { get; set; }

    }
}
