using Newtonsoft.Json;
using System;

namespace DataAccess.ViewModels
{
    public class RealWorkHourEdit
    {

        [JsonProperty(PropertyName = "realworkHourId", Required = Required.Default)]
        public int RealworkHourId { get; set; }

        [JsonProperty(PropertyName = "startOn", Required = Required.Default)]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "EndOn", Required = Required.Default)]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "Comments")]
        public string Comments { get; set; }
    }
}
