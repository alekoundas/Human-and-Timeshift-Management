using System;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication.Utilities
{
    public static class HtmlHelper
    {
        private static IHttpContextAccessor httpContext;

        public static IHtmlContent LinkButton(this IHtmlHelper html, string controller, string action, string buttonName, string permition = "")
        {
            var link = "href=/" + controller + "/" + action;
            if (IsOkToShow(permition))
                return new HtmlString("<li class=''><a " + link + " ><i class='fa fa-circle-o'></i> " + buttonName + " </a></li>");

            return new HtmlString("");
        }

        public static IHtmlContent ButtonCreateNew(this IHtmlHelper html, string controller)
        {
            var link = "href=/" + controller + "/Create";
            if (IsOkToShow(controller + "_Create"))
                return new HtmlString("<a " + link + " class='button'> <button class='btn btn-primary'>Προσθήκη</button> </a>");

            return new HtmlString("");
        }

        public static IHtmlContent ButtonBackToList(this IHtmlHelper html, string controller)
        {
            var link = "href=/" + controller + "/Index";
            if (IsOkToShow(controller + "_View"))
                return new HtmlString("<a " + link + " class='button'> <button class='btn btn-primary'>Πίσω στην λίστα</button> </a>");

            return new HtmlString("");
        }

        public static IHtmlContent ButtonGoToEdit(this IHtmlHelper html, string controller, int id)
        {
            var link = "href=/" + controller + "/Edit/" + id;
            if (IsOkToShow(controller + "_Edit"))
                return new HtmlString("<a " + link + " class='button'> <button class='btn btn-primary'>Επεξεργασία</button> </a>");

            return new HtmlString("");
        }

        public static IHtmlContent ButtonImport(this IHtmlHelper html, string controller)
        {
            var onClick = "onclick = \"$('#ImportModal').modal('show')\"";
            if (IsOkToShow(controller + "_Create"))
                return new HtmlString("<a " + onClick + " class='button'> <button class='btn btn-primary'>Import</button> </a>");

            return new HtmlString("");
        }

        public static bool HasAnyChildButton(this IHtmlHelper html, string controller)
        {
            if (IsOkToShow(controller + "_View") || IsOkToShow(controller + "_Create"))
                return true;

            return false;
        }

        private static bool IsOkToShow(string permition)
        {
            httpContext = new HttpContextAccessor();

            if (!String.IsNullOrEmpty(permition))
                return httpContext.HttpContext.User.Claims.Any(x =>
                x.Value.Split("_")[0] == permition.Split("_")[0] &&
                x.Value.Split("_")[1] == permition.Split("_")[1]);
            return true;
        }

    }
}
