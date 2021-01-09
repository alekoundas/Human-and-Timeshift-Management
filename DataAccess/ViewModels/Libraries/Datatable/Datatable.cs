using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataAccess.Libraries.Datatable
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

        [JsonPropertyName("genericId")]
        public int GenericId { get; set; }


        //User
        [JsonPropertyName("userId")]
        public string UserId { get; set; }


        //TimeShift

        [JsonPropertyName("timeShiftYear")]
        public int TimeShiftYear { get; set; }

        [JsonPropertyName("timeShiftMonth")]
        public int TimeShiftMonth { get; set; }

        [JsonPropertyName("makePrintable")]
        public bool MakePrintable { get; set; }

        [JsonPropertyName("showHoursIn24h")]
        public bool ShowHoursIn24h { get; set; }


        //Difference

        [JsonProperty(PropertyName = "startOn")]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "endOn")]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "filterByWorkHour")]
        public bool FilterByWorkHour { get; set; }

        [JsonProperty(PropertyName = "filterByRealWorkHour")]
        public bool FilterByRealWorkHour { get; set; }

        [JsonProperty(PropertyName = "filterByOffsetMinutes")]
        public int? FilterByOffsetMinutes { get; set; }

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

        //WorkPlace Edit
        [JsonProperty(PropertyName = "filterByIncludedEmployees")]
        public bool FilterByIncludedEmployees { get; set; }

        //Customer Edit
        [JsonProperty(PropertyName = "filterByIncludedCustomer")]
        public bool FilterByIncludedCustomer { get; set; }

        //Employee Edit
        [JsonProperty(PropertyName = "filterByIncludedWorkPlaces")]
        public bool FilterByIncludedWorkPlaces { get; set; }

        [JsonProperty(PropertyName = "filterByWorkPlaceId")]
        public int FilterByWorkPlaceId { get; set; }
        [JsonProperty(PropertyName = "filterByEmployeeId")]
        public int FilterByEmployeeId { get; set; }

        [JsonProperty(PropertyName = "filterByTimeShift")]
        public int FilterByTimeShift { get; set; }

        [JsonProperty(PropertyName = "filterByMonth")]
        public int FilterByMonth { get; set; }

        [JsonProperty(PropertyName = "filterByYear")]
        public int FilterByYear { get; set; }

    }
}
