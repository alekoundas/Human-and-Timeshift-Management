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

            var roles = await _securityDatawork.ApplicationUserRoles.GetRolesFormUserId(userId);

            if (permition == "View")
            {
                var roleId = _securityDatawork.ApplicationRoles.SingleOrDefault(x => x.Controller == controller && x.Permition == permition).Id;
                stringToReturn += roles.Any(x => x.Controller == controller && x.Permition == permition)
                    ? RoleViewCheckbox(true, userId, roleId)
                    : RoleViewCheckbox(false, userId, roleId);
            }

            if (permition == "Edit")
            {
                var roleId = _securityDatawork.ApplicationRoles.SingleOrDefault(x => x.Controller == controller && x.Permition == permition).Id;
                stringToReturn += roles.Any(x => x.Controller == controller && x.Permition == permition)
                    ? RoleEditCheckbox(true, userId, roleId)
                        : RoleEditCheckbox(false, userId, roleId);
            }

            if (permition == "Create")
            {
                var roleId = _securityDatawork.ApplicationRoles.SingleOrDefault(x => x.Controller == controller && x.Permition == permition).Id;
                stringToReturn += roles.Any(x => x.Controller == controller && x.Permition == permition)
                    ? RoleEditCheckbox(true, userId, roleId)
                        : RoleEditCheckbox(false, userId, roleId);
            }

            if (permition == "Delete")
            {
                var roleId = _securityDatawork.ApplicationRoles.SingleOrDefault(x => x.Controller == controller && x.Permition == permition).Id;
                stringToReturn += roles.Any(x => x.Controller == controller && x.Permition == permition)
                    ? RoleDeleteCheckbox(true, userId, roleId)
                        : RoleDeleteCheckbox(false, userId, roleId);
            }



            return stringToReturn;
        }

        private static string ViewButton(string controller, string id)
           => @"<a href='/" + controller + "/Details/" + id + "'><i class='fa fa-eye'></i></a>";

        private static string EditButton(string controller, string id)
            => @"<a href='/" + controller + "/Edit/" + id + "'><i class='fa fa-pencil-square-o'></i></a>";

        private static string DeleteButton(string controller, string id)
            => @"<a ><i urlAttr='/api/" + controller + "/" + id + "' class='fa fa-trash-o DatatableDeleteButton' ></i></a>";

        public string RoleViewCheckbox(bool isChecked, string userId, string roleId)
          =>
              "<div class='input-group-prepend'>" +
                "<div class='input-group-text'>" +
                    "<input " +
                        "type='checkbox' " +
                        "aria-label='Checkbox for following text input'" +
                        "id='PermitionRoleCheckbox_View'class='PermitionCheckbox'" +
                        "UserId='" + userId + "'" +
                        "RoleId='" + roleId + "'" +
                        (isChecked == true ? "checked" : "") +
                    ">" +
                "</div>" +
              "</div>";

        private string RoleEditCheckbox(bool isChecked, string userId, string roleId)
            =>
              "<div class='input-group-prepend'>" +
                "<div class='input-group-text'>" +
                    "<input " +
                        "type='checkbox' " +
                        "aria-label='Checkbox for following text input'" +
                        "id='PermitionRoleCheckbox_Edit'class='PermitionCheckbox'" +
                        "UserId='" + userId + "'" +
                        "RoleId='" + roleId + "'" +
                        (isChecked == true ? "checked" : "") +
                    ">" +
                "</div>" +
              "</div>";

        private string RoleCreateCheckbox(bool isChecked, string userId, string roleId)
            =>
              "<div class='input-group-prepend'>" +
                "<div class='input-group-text'>" +
                    "<input " +
                        "type='checkbox' " +
                        "aria-label='Checkbox for following text input'" +
                        "id='PermitionRoleCheckbox_Create'class='PermitionCheckbox'" +
                        "UserId='" + userId + "'" +
                        "RoleId='" + roleId + "'" +
                        (isChecked == true ? "checked" : "") +
                    ">" +
                "</div>" +
              "</div>";

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




        public string GetToggle(string baseRole, string apiUrl, string toggleState)
        {
            var stringToReturn = "";
            _httpContext = new HttpContextAccessor();
            var currentUserRoles = _httpContext.HttpContext.User.Claims
                .Select(x => x.Value).ToList();

            if (currentUserRoles.Contains(baseRole + "_Edit"))
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
        public string GetHoverElementsAsync(BaseDatawork baseDatawork, int dayOfMonth, Datatable datatable, int employeeId)
        {
            var skata = baseDatawork.Employees.Where(x =>
                x.Id == employeeId &&
                x.WorkHours.Any(y =>
                    y.TimeShiftId == datatable.GenericId &&
                    y.StartOn.Year == datatable.TimeShiftYear &&
                    y.StartOn.Month == datatable.TimeShiftMonth &&
                    (
                        y.StartOn.Day == dayOfMonth ||
                        y.EndOn.Day == dayOfMonth ||
                        (y.StartOn.Day < dayOfMonth && dayOfMonth < y.EndOn.Day)
                     )));

            //TODO :Async
            var cellWorkHours = baseDatawork.WorkHours.GetCurrentAssignedOnCell(
                datatable.GenericId,
                datatable.TimeShiftYear,
                datatable.TimeShiftMonth,
                dayOfMonth,
                employeeId);

            if (cellWorkHours.Count() > 0)
            {
                var sdafasdf = 234;
            }

            //if (skata)
            //    return FaIconEdit(dayOfMonth, "green", employeeId, datatable.GenericId) + FaIconAdd(dayOfMonth, "green", employeeId);
            //return FaIconAdd(dayOfMonth, "", employeeId);

            var startTimeSpan = String.Join("",
                cellWorkHours.Select(x =>
                    SpanTimeValue(x.StartOn.ToShortTimeString())));

            var endTimeSpan = String.Join("",
                cellWorkHours.Select(x =>
                    SpanTimeValue(x.EndOn.ToShortTimeString())));

            return "<div style='width:110px; white-space: nowrap;'>" +
                      "<div style='width:50px;display:block;  float: left;'>" +
                      (!String.IsNullOrEmpty(startTimeSpan) ? "<span>Έναρξη</span></br>" : "") +
                      startTimeSpan +
                      "</div>" +
                      "<div style='width:50px; display:block;  float: right; '>" +
                      (!String.IsNullOrEmpty(endTimeSpan) ? "<span>Λήξη</span></br>" : "") +
                      endTimeSpan +
                    "</div>" +
                     (cellWorkHours.Count() > 0 ? FaIconEdit(dayOfMonth, "green", employeeId,
                        datatable.GenericId) :
                        "") +
                     FaIconAdd(dayOfMonth, "", employeeId);

        }

        private static string SpanTimeValue(string time)
            => @"<span>" + time + "</span></br>";
        private static string FaIconEdit(int dayOfMonth, string color, int employeeid, int timeshiftid)
          => @"<i class='fa fa-pencil hidden faIconEdit'   timeshiftid='" + timeshiftid + "' employeeid='" + employeeid + "' cellColor='" + color + "' dayOfMonth = '" + dayOfMonth + "'></i>";

        private static string FaIconAdd(int dayOfMonth, string color, int employeeid)
            => @"<i class='fa fa-plus hidden faIconAdd' employeeid='" + employeeid + "' cellColor=''" + color + "'' dayOfMonth = '" + dayOfMonth + "'></i>";

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
           => @"<i class='fa fa-pencil hidden faIconEdit' employeeid='" + employeeId + "'></i>";









    }
}
