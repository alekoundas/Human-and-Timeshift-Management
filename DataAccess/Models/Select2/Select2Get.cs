using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DataAccess.Models.Select2
{
    public class Select2Get
    {
        [JsonProperty(PropertyName = "search", Required = Required.Default)]
        public string Search { get; set; }

        [JsonProperty(PropertyName = "page", Required = Required.Default)]
        public int Page { get; set; }

        [JsonProperty(PropertyName = "existingIds", Required = Required.Default)]
        public List<int> ExistingIds { get; set; }

    }
}
