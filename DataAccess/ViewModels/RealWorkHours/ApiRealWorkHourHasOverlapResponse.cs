using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models;
using Newtonsoft.Json;

namespace DataAccess.ViewModels.RealWorkHours
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