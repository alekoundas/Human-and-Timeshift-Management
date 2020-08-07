﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAccess.Models.Entity;
using Newtonsoft.Json;

namespace DataAccess.ViewModels
{
    public class TimeShiftHasOverlapViewModel
    {
        [JsonProperty(PropertyName = "startOn", Required = Required.Default)]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "endOn", Required = Required.Default)]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "timeShiftId", Required = Required.Default)]
        public int TimeShiftId { get; set; }
    }
}