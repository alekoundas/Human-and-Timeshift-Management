using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Bussiness;
using Bussiness.Repository.Security.Interface;
using DataAccess.Models.Datatable;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;

namespace WebApplication.Utilities
{
    public class DataTableHelper<TEntity>
    {
        private IHttpContextAccessor _httpContext;
        private ISecurityDatawork _securityDatawork;
        public DataTableHelper(SecurityDataWork securityDatawork)
        {
            _securityDatawork = securityDatawork;
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

        static string ViewButton(string controller, string id)
           => @"<a href='/" + controller + "/Details/" + id + "'><i class='fa fa-eye'></i></a>";

        static string EditButton(string controller, string id)
            => @"<a href='/" + controller + "/Edit/" + id + "'><i class='fa fa-pencil-square-o'></i></a>";

        static string DeleteButton(string controller, string id)
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

        public string RoleEditCheckbox(bool isChecked, string userId, string roleId)
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

        public string RoleCreateCheckbox(bool isChecked, string userId, string roleId)
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

        public string RoleDeleteCheckbox(bool isChecked, string userId, string roleId)
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

        static string ViewToggle(string toggleState, string link)
           => @"<input urlAttr='" + link + "' class='ToggleSliders disabled' type='checkbox' data-onstyle='success' disabled " + toggleState + ">";

        static string EditToggle(string toggleState, string link)
            => @"<input urlAttr='" + link + "' class='ToggleSliders' type='checkbox' data-onstyle='success' " + toggleState + ">";

    }
}
