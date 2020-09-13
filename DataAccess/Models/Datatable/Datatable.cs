using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace DataAccess.Models.Datatable
{
    public class Datatable
    {
        [JsonPropertyName("columns")]
        public List<Column> Columns { get; set; }

        [JsonPropertyName("culture")]
        public string Culture { get; set; }

        [JsonPropertyName("draw")]
        public int Draw { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonPropertyName("start")]
        public int Start { get; set; }

        [JsonPropertyName("search")]
        public Search Search { get; set; }

        [JsonPropertyName("tableLVL")]
        public int TableLVL { get; set; }

        [JsonPropertyName("order")]
        public List<Order> Order { get; set; }

        [JsonPropertyName("searchValue")]
        public string SearchValue { get; set; }





        [JsonPropertyName("predicate")]
        public string Predicate { get; set; }

        [JsonPropertyName("applicationUserId")]
        public string ApplicationUserId { get; set; }

        [JsonPropertyName("genericId")]
        public int GenericId { get; set; }
       

        //TimeShift

        [JsonPropertyName("timeShiftYear")]
        public int TimeShiftYear { get; set; }
        [JsonPropertyName("timeShiftMonth")]
        public int TimeShiftMonth { get; set; }

        //Difference

        [JsonProperty(PropertyName = "startOn")]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "endOn")]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "filterByWorkHour")]
        public bool FilterByWorkHour { get; set; }
        [JsonProperty(PropertyName = "filterByRealWorkHour")]
        public bool FilterByRealWorkHour { get; set; }

        //Concentric
        [JsonProperty(PropertyName = "showHoursInPercentage")]
        public bool ShowHoursInPercentage { get; set; }

        //RealWorkHoursSpecificDates
        [JsonProperty(PropertyName = "specificDates")]
        public List<DateTime> SpecificDates { get; set; }

        //RealWorkHour Index
        [JsonProperty(PropertyName = "selectedYear")]
        public int? SelectedYear { get; set; }

        [JsonProperty(PropertyName = "selectedMonth")]
        public int? SelectedMonth { get; set; }
    }
}
