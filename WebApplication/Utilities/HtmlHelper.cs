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

        public static IHtmlContent LinkButton(this IHtmlHelper html, string controller, string action, string buttonName)
        {
            var link = "href=/" + controller + "/" + action;
            return new HtmlString("<li class=''><a " + link + " ><i class='fa fa-circle-o'></i> " + buttonName + " </a></li>");

        }

        public static IHtmlContent ButtonPrimary(this IHtmlHelper html)
        {
            httpContext = new HttpContextAccessor();
            var UserRoles = httpContext.HttpContext.User.Claims.Select(x=>x.Value).ToList();
            return new HtmlString("<li class=''><a  ><i class='fa fa-circle-o'></i>  </a></li>");

        }
    }
}
