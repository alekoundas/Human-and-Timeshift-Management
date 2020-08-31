using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;

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
            if (IsOkToShow(controller+"_Create"))
                return new HtmlString("<a " + link + " class='button'> <button class='btn btn-primary'>Προσθήκη</button> </a>");

            return new HtmlString("");
        }


          public static IHtmlContent ButtonBackToList(this IHtmlHelper html, string controller)
        {
            var link = "href=/" + controller + "/Index";
            if (IsOkToShow(controller+"_View"))
                return new HtmlString("<a " + link + " class='button'> <button class='btn btn-primary'>Πίσω στην λίστα</button> </a>");

            return new HtmlString("");
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
