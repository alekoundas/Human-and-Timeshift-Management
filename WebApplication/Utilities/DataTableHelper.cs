using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Business.Repository.Interface;
using Bussiness;
using Bussiness.Repository.Security.Interface;
using DataAccess.Models.Datatable;
using DataAccess.Models.Entity;
using LinqKit;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;

namespace WebApplication.Utilities
{
    public class DataTableHelper<TEntity>
    {
        private IHttpContextAccessor _httpContext;
        private ISecurityDatawork _securityDatawork;
        //private IBaseDatawork _baseDatawork;
        public DataTableHelper(SecurityDataWork securityDatawork/*, BaseDatawork baseDatawork*/)
        {
            _securityDatawork = securityDatawork;
            //_baseDatawork = baseDatawork;
        }

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
            var stringToReturn = "";
            _httpContext = new HttpContextAccessor();
            var currentUserRoles = _httpContext.HttpContext.User.Claims
                .Select(x => x.Value).ToList();

            if (currentUserRoles.Contains(baseRole + "_View"))
                stringToReturn += ViewButton(baseRole, id);

            if (currentUserRoles.Contains(baseRole + "_Edit"))
                stringToReturn += EditButton(baseRole, id);

            if (currentUserRoles.Contains(baseRole + "_Delete"))
                stringToReturn += DeleteButton(apiController, id);

            return stringToReturn;
        }

        public async Task<string> GetButtonForRoles(string controller, string permition, string userId)
        {
            var stringToReturn = "";
            if (controller != null)
            {

                var isDisabled = true;
                var roles = await _securityDatawork.ApplicationUserRoles.GetRolesFormUserId(userId);

                if (roles.Any(x => x.Name == "User_Edit"))
                    isDisabled = false;

                if (permition == "View")
                {
                    var roleId = _securityDatawork.ApplicationRoles.SingleOrDefault(x => x.Controller == controller && x.Permition == permition)?.Id;
                    if (roleId != null)
                        stringToReturn += roles.Any(x => x.Controller == controller && x.Permition == permition)
                        ? RoleCheckbox(true, userId, roleId, isDisabled)
                            : RoleCheckbox(false, userId, roleId, isDisabled);
                }

                if (permition == "Edit")
                {
                    var roleId = _securityDatawork.ApplicationRoles.SingleOrDefault(x => x.Controller == controller && x.Permition == permition)?.Id;
                    if (roleId != null)
                        stringToReturn += roles.Any(x => x.Controller == controller && x.Permition == permition)
                            ? RoleCheckbox(true, userId, roleId, isDisabled)
                                : RoleCheckbox(false, userId, roleId, isDisabled);
                }

                if (permition == "Create")
                {
                    var roleId = _securityDatawork.ApplicationRoles.SingleOrDefault(x => x.Controller == controller && x.Permition == permition)?.Id;
                    if (roleId != null)
                        stringToReturn += roles.Any(x => x.Controller == controller && x.Permition == permition)
                        ? RoleCheckbox(true, userId, roleId, isDisabled)
                            : RoleCheckbox(false, userId, roleId, isDisabled);
                }

                if (permition == "Delete")
                {
                    var roleId = _securityDatawork.ApplicationRoles.SingleOrDefault(x => x.Controller == controller && x.Permition == permition)?.Id;
                    if (roleId != null)
                        stringToReturn += roles.Any(x => x.Controller == controller && x.Permition == permition)
                        ? RoleCheckbox(true, userId, roleId, isDisabled)
                            : RoleCheckbox(false, userId, roleId, isDisabled);
                }


            }

            return stringToReturn;
        }

        private static string ViewButton(string controller, string id)
           => @"<a href='/" + controller + "/Details/" + id + "'><i class='fa fa-eye'></i></a>";

        private static string EditButton(string controller, string id)
            => @"<a href='/" + controller + "/Edit/" + id + "'><i class='fa fa-pencil-square-o'></i></a>";

        private static string DeleteButton(string controller, string id)
            => @"<a ><i urlAttr='/api/" + controller + "/" + id + "' class='fa fa-trash-o DatatableDeleteButton' ></i></a>";



        public string RoleCheckbox(bool isChecked, string userId, string roleId, bool isDisabled = false)
          =>
              "<div class='input-group-prepend'>" +
                "<div class='input-group-text'>" +
                    "<input " +
                        "type='checkbox' " +
                        "aria-label='Checkbox for following text input'" +
                        "class='PermitionCheckbox'" +
                        "UserId='" + userId + "'" +
                        "RoleId='" + roleId + "'" +
                        (isChecked == true ? " checked " : "") +
                        (isDisabled == true ? " disabled " : "") +
                    ">" +
                "</div>" +
              "</div>";



        //public string RoleViewCheckbox(bool isChecked, string userId, string roleId)
        //  =>
        //      "<div class='input-group-prepend'>" +
        //        "<div class='input-group-text'>" +
        //            "<input " +
        //                "type='checkbox' " +
        //                "aria-label='Checkbox for following text input'" +
        //                "id='PermitionRoleCheckbox_View'class='PermitionCheckbox'" +
        //                "UserId='" + userId + "'" +
        //                "RoleId='" + roleId + "'" +
        //                (isChecked == true ? "checked" : "") +
        //            ">" +
        //        "</div>" +
        //      "</div>";

        //private string RoleEditCheckbox(bool isChecked, string userId, string roleId)
        //    =>
        //      "<div class='input-group-prepend'>" +
        //        "<div class='input-group-text'>" +
        //            "<input " +
        //                "type='checkbox' " +
        //                "aria-label='Checkbox for following text input'" +
        //                "id='PermitionRoleCheckbox_Edit'class='PermitionCheckbox'" +
        //                "UserId='" + userId + "'" +
        //                "RoleId='" + roleId + "'" +
        //                (isChecked == true ? "checked" : "") +
        //            ">" +
        //        "</div>" +
        //      "</div>";

        //private string RoleCreateCheckbox(bool isChecked, string userId, string roleId)
        //    =>
        //      "<div class='input-group-prepend'>" +
        //        "<div class='input-group-text'>" +
        //            "<input " +
        //                "type='checkbox' " +
        //                "aria-label='Checkbox for following text input'" +
        //                "id='PermitionRoleCheckbox_Create'class='PermitionCheckbox'" +
        //                "UserId='" + userId + "'" +
        //                "RoleId='" + roleId + "'" +
        //                (isChecked == true ? "checked" : "") +
        //            ">" +
        //        "</div>" +
        //      "</div>";

        private string RoleDeleteCheckbox(bool isChecked, string userId, string roleId)
             =>
               "<div class='input-group-prepend'>" +
                 "<div class='input-group-text'>" +
                     "<input " +
                         "type='checkbox' " +
                         "aria-label='Checkbox for following text input'" +
                         "id='PermitionRoleCheckbox_Delete'class='PermitionCheckbox'" +
                         "UserId='" + userId + "'" +
                         "RoleId='" + roleId + "'" +
                         (isChecked == true ? "checked" : "") +
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


        public string GetTableCellBody(BaseDatawork baseDatawork, int dayOfMonth, Datatable datatable, int employeeId)
        {
            return "";
        }
        public async Task<string> GetTimeShiftEditCellBodyWorkHoursAsync(BaseDatawork baseDatawork, int dayOfMonth, Datatable datatable, int employeeId)
        {
            var strToReturn = "";

            var cellWorkHours = await baseDatawork.WorkHours.GetCurrentAssignedOnCell(
                datatable.GenericId,
                datatable.TimeShiftYear,
                datatable.TimeShiftMonth,
                dayOfMonth,
                employeeId);

            var cellLeaves = await baseDatawork.Leaves.GetCurrentAssignedOnCell(
               datatable.TimeShiftYear,
               datatable.TimeShiftMonth,
               dayOfMonth,
               employeeId);

            if (cellWorkHours.Any(x => x.IsDayOff))
                return
                    "<div style='width:110px; white-space: nowrap;'>" +
                    "<center><p><b>Ρεπό</b></p></center>" +
                     "</div>";
            else if (cellLeaves.Count() > 0)
                return
                    "<div style='width:110px; white-space: nowrap;'>" +
                    "<center><p><b>Άδεια</b></p></center>" +
                     "</div>";
            else
            {
                foreach (var cellWorkHour in cellWorkHours)
                {
                    var celStartOn = cellWorkHour.StartOn;
                    if (celStartOn.Day != dayOfMonth)
                        celStartOn = new DateTime(
                            cellWorkHour.StartOn.Year,
                            cellWorkHour.StartOn.Month,
                            cellWorkHour.StartOn.Day,
                            0,
                            0,
                            0,
                            0);

                    var celEndOn = cellWorkHour.EndOn;
                    if (celEndOn.Day != dayOfMonth)
                        celEndOn = new DateTime(
                                cellWorkHour.EndOn.Year,
                                cellWorkHour.EndOn.Month,
                                cellWorkHour.EndOn.Day,
                                23,
                                59,
                                0,
                                0);
                    strToReturn += "<div style='width:110px; white-space: nowrap;'><div style = 'width:50px;display:block;  float: left;' > " +
                        celStartOn.ToShortTimeString() +
                  "</div>";

                    strToReturn += " <div style = 'width:50px; display:block;  float: right; ' >" +
                        celEndOn.ToShortTimeString() +
                     "</div></div>";

                }


                _httpContext = new HttpContextAccessor();
                var currentUserRoles = _httpContext.HttpContext.User.Claims
                    .Select(x => x.Value).ToList();

                strToReturn += "</div>";

                if (currentUserRoles.Contains("TimeShift_View"))
                    strToReturn += FaIconAdd(dayOfMonth, "", employeeId);

                if (currentUserRoles.Contains("TimeShift_Edit"))
                    if (cellWorkHours.Count() > 0)
                        strToReturn += FaIconEdit(dayOfMonth, "green", employeeId, datatable.GenericId);

                return strToReturn;
            }

        }
        public async Task<string> GetTimeShiftEditCellBodyRealWorkHoursAsync(BaseDatawork baseDatawork, int dayOfMonth, Datatable datatable, int employeeId)
        {
            var compareMonth = 0;
            var compareYear = 0;
            var strToReturn = "";

            if (datatable.SelectedMonth == null || datatable.SelectedYear == null)
            {
                var timeShift = await baseDatawork.TimeShifts.FirstOrDefaultAsync(x => x.Id == datatable.GenericId);
                compareMonth = timeShift.Month;
                compareYear = timeShift.Year;
            }
            else
            {
                compareMonth = (int)datatable.SelectedMonth;
                compareYear = (int)datatable.SelectedYear;
            }

            var cellRealWorkHours = await baseDatawork.RealWorkHours.GetCurrentAssignedOnCell(
              datatable.GenericId,
              compareYear,
              compareMonth,
              dayOfMonth,
              employeeId);

            var cellLeaves = await baseDatawork.Leaves.GetCurrentAssignedOnCell(
               compareYear,
               compareMonth,
               dayOfMonth,
               employeeId);


            if (cellLeaves.Count() > 0)
                return
                    "<div style='width:110px; white-space: nowrap;'>" +
                        "<center><p><b>Άδεια</b></p></center>" +
                     "</div>";
            else
            {
                foreach (var cellWorkHour in cellRealWorkHours)
                {
                    var celStartOn = cellWorkHour.StartOn;
                    if (celStartOn.Day != dayOfMonth)
                        celStartOn = new DateTime(
                            cellWorkHour.StartOn.Year,
                            cellWorkHour.StartOn.Month,
                            cellWorkHour.StartOn.Day,
                            0,
                            0,
                            0,
                            0);

                    var celEndOn = cellWorkHour.EndOn;
                    if (celEndOn.Day != dayOfMonth)
                        celEndOn = new DateTime(
                                cellWorkHour.EndOn.Year,
                                cellWorkHour.EndOn.Month,
                                cellWorkHour.EndOn.Day,
                                23,
                                59,
                                0,
                                0);
                    strToReturn += "<div style='width:110px; white-space: nowrap;'><div style = 'width:50px;display:block;  float: left;' > " +
                        celStartOn.ToShortTimeString() +
                  "</div>";

                    strToReturn += " <div style = 'width:50px; display:block;  float: right; ' >" +
                        celEndOn.ToShortTimeString() +
                     "</div></div>";
                }
                //strToReturn += (datatable.GenericId != 0 ?
                //        (cellRealWorkHours.Count() > 0 ?
                //            FaIconEdit(dayOfMonth, "green", employeeId,
                //                datatable.GenericId, compareMonth,
                //                compareYear) +
                //            FaIconAdd(dayOfMonth, "", employeeId,
                //                compareMonth, compareYear)
                //            :
                //            FaIconAdd(dayOfMonth, "", employeeId,
                //               compareMonth, compareYear))
                //        :
                //        "");

                //return strToReturn;



                _httpContext = new HttpContextAccessor();
                var currentUserRoles = _httpContext.HttpContext.User.Claims
                    .Select(x => x.Value).ToList();

                if (datatable.GenericId != 0)
                {
                    if (currentUserRoles.Contains("TimeShift_View"))
                        strToReturn += FaIconAdd(dayOfMonth, "", employeeId,
                            compareMonth, compareYear);

                    if (currentUserRoles.Contains("TimeShift_Edit"))
                        if (cellRealWorkHours.Count() > 0)
                            strToReturn += FaIconEdit(dayOfMonth, "green", employeeId,
                                   datatable.GenericId, compareMonth, compareYear);
                }
                return strToReturn;

            }
        }

        public async Task<string> GetProjectionRealWorkHoursAnalyticallyCellBodyAsync(BaseDatawork baseDatawork, DateTime compareDate, Datatable datatable, int employeeId)
        {
            var cellRealWorkHours = await baseDatawork.RealWorkHours
                .GetCurrentAssignedOnCell(compareDate, employeeId);

            var cellBody = "";

            foreach (var realWorkHour in cellRealWorkHours)
            {
                cellBody += "<p white-space: nowrap;'>" +
                    realWorkHour.StartOn.ToShortTimeString() +
                    " - " +
                    realWorkHour.EndOn.ToShortTimeString() +
                    "</p></br>";
            }

            return cellBody;
        }

        private static string SpanTimeValue(string time)
            => @"<span>" + time + "</span></br>";
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



        public string GetCurrentDayButtons(Employee employee)
           => CurrentDayFaIconEdit(employee.Id);

        private static string CurrentDayFaIconEdit(int employeeId)
           => @"<i class='fa fa-pencil faIconEdit' employeeid='" + employeeId + "'></i>";

    }
}
