using DataAccess.Libraries.Datatable;
using DataAccess.Models.Entity;
using DataAccess.Models.Security;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Bussiness.Helpers
{
    public class DataTableHelper<TEntity>
    {
        private IHttpContextAccessor _httpContext;

        public DatatableResponse<TEntity> CreateResponse(Datatable dataTable, IEnumerable<TEntity> model, int total, int filteredCount = -1)
        {
            if (filteredCount == -1)
                filteredCount = total;
            return new DatatableResponse<TEntity>()
            {
                data = model,
                draw = dataTable.Draw,
                recordsFiltered = filteredCount,
                recordsTotal = total
            };
        }

        public string GetButtons(string baseRole, string apiController, string id)
        {
            _httpContext = new HttpContextAccessor();

            var stringToReturn = "<div style='width:100%;'>";
            //stringToReturn += "<div style='width: 30%; float: left;'>&nbsp</div>";

            var currentUserRoles = _httpContext.HttpContext.User.Claims
                .Select(x => x.Value).ToList();

            if (currentUserRoles.Contains(baseRole + "_View"))
                stringToReturn += "<div style='width: 20%; float: left;'>" +
                    ViewButton(baseRole, id) +
                    "</div>";

            if (currentUserRoles.Contains(baseRole + "_Edit"))
                stringToReturn += "<div style='width: 20%; float: left;'>" +
                    EditButton(baseRole, id) +
                    "</div>";

            if (currentUserRoles.Contains(baseRole + "_Deactivate"))
                stringToReturn += "<div style='width: 20%; float: left;'>" +
                    DeactivateButton(apiController, id) +
                    "</div>";

            if (currentUserRoles.Contains(baseRole + "_Delete"))
                stringToReturn += "<div style='width: 20%; float: left;'>" +
                    DeleteButton(apiController, id) +
                    "</div>";

            //stringToReturn += "<div style='width: 30%; float: left;'>&nbsp</div>";
            stringToReturn += "</div>";
            return stringToReturn;
        }

        public string GetButtonForRoles(string controller, string permition, string userId, List<ApplicationRole> userRoles, List<ApplicationRole> applicationRoles)
        {
            var stringToReturn = "";
            var isDisabled = true;

            //1.Check if loggedin user can edit roles
            _httpContext = new HttpContextAccessor();
            var currentUserRoles = _httpContext.HttpContext.User.Claims
                   .Select(x => x.Value).ToList();

            if (currentUserRoles.Contains("User_Edit"))
                isDisabled = false;

            var roleId = applicationRoles.FirstOrDefault(x =>
                x.Controller == controller && x.Permition == permition)?.Id;

            //2.Check if user from id has specific role
            var isChecked = userRoles
                .Any(x => x.Controller == controller && x.Permition == permition);

            if (roleId != null)
                stringToReturn += RoleCheckbox(isChecked, userId, roleId, isDisabled);

            return stringToReturn;
        }

        private static string ViewButton(string controller, string id)
           => @"<div style='width:20px; height:20px;'><a href='/" + controller + "/Details/" + id + "'><i class='fa fa-eye'></i></a></div>";

        private static string EditButton(string controller, string id)
            => @"<div style='width:20px; height:20px;'><a href='/" + controller + "/Edit/" + id + "'><i class='fa fa-pencil-square-o'></i></a></div>";

        private static string DeactivateButton(string controller, string id)
            => @"<div style='width:20px; height:20px;'><a ><i urlAttr='/api/" + controller + "/" + "deactivate" + "/" + id + "' class='fa fa-ban DatatableDeactivateButton' ></i></a></div>";

        private static string DeleteButton(string controller, string id)
            => @"<div style='width:20px; height:20px;'><a ><i urlAttr='/api/" + controller + "/" + id + "' class='fa fa-trash-o DatatableDeleteButton' ></i></a></div>";

        public string RoleCheckbox(bool isChecked, string userId, string roleId, bool isDisabled = false)
          =>
              "<div class='input-group-prepend'>" +
                "<div class='input-group-text'>" +
                    "<input " +
                        "type='checkbox' " +
                        "aria-label='Checkbox for following text input'" +
                        "class='ToggleSliders'" +
                        "UserId='" + userId + "'" +
                        "RoleId='" + roleId + "'" +
                        (isChecked == true ? " checked " : "") +
                        (isDisabled == true ? " disabled " : "") +
                    ">" +
                "</div>" +
              "</div>";

        public string GetToggle(string baseRole, string apiUrl, string toggleState, bool isDisabled = false)
        {
            var stringToReturn = "";
            _httpContext = new HttpContextAccessor();
            var currentUserRoles = _httpContext.HttpContext.User.Claims
                .Select(x => x.Value).ToList();

            if (currentUserRoles.Contains(baseRole + "_Edit") && isDisabled == false)
                stringToReturn += EditToggle(toggleState, apiUrl);

            else if (currentUserRoles.Contains(baseRole + "_View"))
                stringToReturn += ViewToggle(toggleState, apiUrl);

            return stringToReturn;
        }

        private static string ViewToggle(string toggleState, string link)
           => @"<input urlAttr='" + link + "' class='ToggleSliders disabled' type='checkbox' data-onstyle='success' disabled " + toggleState + ">";

        private static string EditToggle(string toggleState, string link)
            => @"<input urlAttr='" + link + "' class='ToggleSliders' type='checkbox' data-onstyle='success' " + toggleState + ">";

        public string GetTimeShiftEditCellBodyWorkHours(int dayOfMonth, Datatable datatable, Employee employee)
        {
            var currentDate = new DateTime(
                datatable.TimeShiftYear,
                datatable.TimeShiftMonth,
                dayOfMonth);

            var otherWorkHours = employee.WorkHours.Where(x =>
                x.TimeShiftId != datatable.FilterByTimeShiftId &&
                x.StartOn.Year == datatable.TimeShiftYear &&
                x.StartOn.Month == datatable.TimeShiftMonth &&
                x.StartOn.Day == dayOfMonth)
            .ToList();


            var cellWorkHours = employee.WorkHours.Where(x =>
               x.TimeShiftId == datatable.FilterByTimeShiftId &&
               x.StartOn.Year == datatable.TimeShiftYear &&
               x.StartOn.Month == datatable.TimeShiftMonth &&
               x.StartOn.Day == dayOfMonth)
            .ToList();


            var cellLeaves = employee.Leaves.Where(x =>
                   x.StartOn.Year == datatable.TimeShiftYear &&
                   x.StartOn.Month == datatable.TimeShiftMonth &&
                   x.StartOn.Day <= dayOfMonth)
                .ToList();

            var strToReturn = "<div style='width:120px; white-space: nowrap;'>";

            if (cellLeaves.Count() > 0)
            {
                if (datatable.MakePrintable)
                    return "<div style='width:110px; white-space: nowrap;'>" +
                                "<center><p><b>ΚΑ</b></p></center>" +
                            "</div>";
                else
                    return "<div style='width:110px; white-space: nowrap;'>" +
                                "<center><p><b>Άδεια</b></p></center>" +
                            "</div>";
            }
            else if (cellWorkHours.Count() == 0)
            {
                if (otherWorkHours.Count() == 0)
                {
                    if (datatable.MakePrintable)

                        strToReturn += "<div style='width:110px; white-space: nowrap;'>" +
                                            "<center><p><b>Ρ</b></p></center>" +
                                        "</div>";
                    else
                        strToReturn += "<div style='width:110px; white-space: nowrap;'>" +
                                            "<center><p><b>Ρεπό</b></p></center>" +
                                        "</div>";
                }
                else
                {
                    if (datatable.MakePrintable)
                        strToReturn += "<div style='width:110px; white-space: nowrap;'>" +
                                            "<center><p><b></b></p></center>" +
                                        "</div>";
                    else
                        strToReturn += "<div style='width:110px; white-space: nowrap;'>" +
                                            "<center><p><b>Δηλώθηκε <br>σε άλλο <br>πόστο</b></p></center>" +
                                        "</div>";
                }
            }
            else
                foreach (var cellWorkHour in cellWorkHours)
                {
                    var celStartOn = cellWorkHour.StartOn;
                    var celEndOn = cellWorkHour.EndOn;

                    //build cell html for current date 
                    //start on
                    if (datatable.ShowHoursIn24h)
                        if (datatable.MakePrintable)
                        {
                            strToReturn += "<div style='width:18px;display:block; float: left;'> ";
                            strToReturn += celStartOn.ToString("HH");
                        }
                        else
                        {
                            strToReturn += "<div style='width:38px;display:block; float: left;'> ";
                            strToReturn += celStartOn.ToString("HH:mm");
                        }
                    else
                    {
                        strToReturn += "<div style='width:50px;display:block; float: left;'> ";
                        strToReturn += celStartOn.ToShortTimeString();
                    }

                    // "-" or "--"
                    strToReturn += "</div>";
                    if (datatable.MakePrintable)
                    {
                        strToReturn += "<div style='width:12px;display:block; float: left;'> ";
                        strToReturn += "--";
                    }
                    else
                    {
                        strToReturn += "<div style='width:8px;display:block; float: left;'> ";
                        strToReturn += "-";
                    }
                    strToReturn += "</div>";

                    //End on
                    if (datatable.ShowHoursIn24h)
                    {
                        strToReturn += "<div style='width:38px; display:block; float: left;'>";
                        if (datatable.MakePrintable)
                            strToReturn += celEndOn.ToString("HH");
                        else
                            strToReturn += celEndOn.ToString("HH:mm");
                    }
                    else
                    {
                        strToReturn += "<div style='width:50px; display:block; float: left;'>";
                        strToReturn += celEndOn.ToShortTimeString();
                    }

                    strToReturn += "</div>";
                }

            strToReturn += "</div>";
            strToReturn += "</div>";

            _httpContext = new HttpContextAccessor();
            var currentUserRoles = _httpContext.HttpContext.User.Claims
                .Select(x => x.Value).ToList();

            //if not in Details view
            if (datatable.Predicate != "TimeShiftDetail")
            {
                if (currentUserRoles.Contains("TimeShift_Edit"))
                    strToReturn += FaIconAdd(dayOfMonth, "", employee.Id);

                if (currentUserRoles.Contains("TimeShift_Edit"))
                    if (cellWorkHours.Count() > 0)
                        strToReturn += FaIconEdit(dayOfMonth, "green", employee.Id, datatable.GenericId);
            }

            return strToReturn;
        }

        public string GetTimeShiftEditCellBodyRealWorkHours(int compareMonth, int compareYear,
            int compareDay, Datatable datatable, Employee employee)
        {
            var cellRealWorkHours = employee.RealWorkHours.Where(x =>
                        x.TimeShiftId == datatable.GenericId &&
                        x.StartOn.Year == compareYear &&
                        x.StartOn.Month == compareMonth &&
                        x.StartOn.Day == compareDay)
                       .ToList();

            var cellLeaves = employee.Leaves.Where(x =>
                   x.StartOn.Year == compareYear &&
                   x.StartOn.Month == compareMonth &&
                   (x.StartOn.Day <= compareDay && compareDay <= x.EndOn.Day))//&&
                                                                              //x.EmployeeId == employeeId)
                .ToList();

            var strToReturn = "<div style='width:110px; white-space: nowrap;'>";

            if (cellLeaves.Count() > 0)
                return "<div style='width:110px; white-space: nowrap;'>" +
                 "<center><p><b>Άδεια</b></p></center>" +
                  "</div>";
            else
            {
                foreach (var cellWorkHour in cellRealWorkHours)
                {
                    var celStartOn = cellWorkHour.StartOn;
                    var celEndOn = cellWorkHour.EndOn;

                    //build cell html for current date 
                    //start on
                    if (datatable.ShowHoursIn24h)
                    {
                        strToReturn += "<div style='width:38px;display:block; float: left;'> ";
                        strToReturn += celStartOn.ToString("HH:mm");
                    }
                    else
                    {
                        strToReturn += "<div style='width:50px;display:block; float: left;'> ";
                        strToReturn += celStartOn.ToShortTimeString();
                    }

                    // "-" or "--"
                    strToReturn += "</div>";
                    strToReturn += "<div style='width:8px;display:block; float: left;'> ";
                    strToReturn += "-";
                    strToReturn += "</div>";

                    //End on
                    if (datatable.ShowHoursIn24h)
                    {
                        strToReturn += "<div style='width:38px; display:block; float: left;'>";
                        strToReturn += celEndOn.ToString("HH:mm");
                    }
                    else
                    {
                        strToReturn += "<div style='width:50px; display:block; float: left;'>";
                        strToReturn += celEndOn.ToShortTimeString();
                    }

                    strToReturn += "</div>";
                }
                strToReturn += "</div>";


                //var currentDate = new DateTime(compareYear, 1, 1);
                //var currentDate2 = new DateTime(compareYear, 12, 12);
                //var currentDate = new DateTime(compareYear, compareMonth, compareDay);
                //var publicHolidays = DateSystem.GetPublicHoliday(currentDate, currentDate2, CountryCode.GR);
                //if (publicHolidays.Count() > 0)
                //    strToReturn += publicHolidays.FirstOrDefault();


                _httpContext = new HttpContextAccessor();
                var currentUserRoles = _httpContext.HttpContext.User.Claims
                    .Select(x => x.Value).ToList();

                if (datatable.GenericId != 0)
                {
                    if (true)
                    {

                        if (currentUserRoles.Contains("RealWorkHour_Create"))
                            strToReturn += FaIconAdd(compareDay, "", employee.Id,
                                compareMonth, compareYear);

                        if (currentUserRoles.Contains("RealWorkHour_Edit"))
                            if (cellRealWorkHours.Count() > 0)
                                strToReturn += FaIconEdit(compareDay, "green", employee.Id,
                                       datatable.GenericId, compareMonth, compareYear);
                    }
                }
                return strToReturn;
            }
        }

        public string GetProjectionRealWorkHoursAnalyticallyCellBody(
            ICollection<RealWorkHour> realWorkHours)
        {
            var cellBody = "";
            foreach (var realWorkHour in realWorkHours)
            {
                cellBody += "<p white-space: nowrap;'>" +
                    realWorkHour.StartOn.ToShortTimeString() +
                    " - " +
                    realWorkHour.EndOn.ToShortTimeString() +
                    "</p></br>";
            }

            return cellBody;
        }
        public string GetProjectionTimeShiftSuggestionCellBody(
            WorkHour workHour)
        {
            return "<p white-space: nowrap;'>" +
                workHour.StartOn.ToString() +
                " - " +
                workHour.EndOn.ToString() +
                "</p></br>";
        }

        private static string FaIconEdit(int dayOfMonth, string color, int employeeid, int timeshiftid, int month = 0, int year = 0)
          => @"<i class='fa fa-pencil hidden faIconEdit'   timeshiftid='" + timeshiftid + "' employeeid='" + employeeid + "' cellColor='" + color + "'  dayOfMonth = '" + dayOfMonth + "' Month = '" + month + "' Year = '" + year + "'></i>";

        private static string FaIconAdd(int dayOfMonth, string color, int employeeid, int month = 0, int year = 0)
            => @"<i class='fa fa-plus hidden faIconAdd' employeeid='" + employeeid + "' cellColor=''" + color + "'' dayOfMonth = '" + dayOfMonth + "' Month = '" + month + "' Year = '" + year + "'></i>";

        public string GetEmployeeCheckbox(Datatable datatable, int employeeId)
            => @"<input " +
                "class='ToggleSliders'" +
                "type='checkbox'" +
                "data-onstyle='success'" +
                "employeeId=" + employeeId +
              ">";

        public string GetCheckbox(int entityId, bool state)
            => @"<input " +
                "class='ToggleSliders'" +
                "type='checkbox'" +
                "data-onstyle='success'" +
                "entityId='" + entityId + "'" +
                (state ? "checked" : "") +
              ">";

        public string GetCurrentDayButtons(RealWorkHour realWorkHour)
           => CurrentDayFaIconEdit(realWorkHour.Id);

        private static string CurrentDayFaIconEdit(int realWorkHourId)
           => @"<i class='fa fa-pencil faIconEdit' realworkhourid='" + realWorkHourId + "'></i>";


        public string GetProjectionDifferenceWorkHourLink(int id, string value)
        {
            return RedirectButton("TimeShift/Edit/" + id, value);
        }
        public string GetProjectionDifferenceRealWorkHourLink(int id, string value)
        {
            return RedirectButton("RealWorkHour/Index", value);
        }

        private string RedirectButton(string url, string value)
             => @"<a href='/" + url + "'><div style='width:100%; height:100%;'>" + value + "</div></a>";
    }
}
