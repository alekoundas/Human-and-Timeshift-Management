using Newtonsoft.Json;
using System;

namespace DataAccess.ViewModels.Entities.Amendment
{
    public class ApiAmendmentDelete
    {

        [JsonProperty(PropertyName = "realWorkHourId", Required = Required.Default)]
        public int? RealWorkHourId { get; set; }

        [JsonProperty(PropertyName = "amendmentId", Required = Required.Default)]
        public int? AmendmentId { get; set; }
    }
}
