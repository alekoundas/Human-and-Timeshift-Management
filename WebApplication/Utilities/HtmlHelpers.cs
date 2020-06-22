using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WebApplication.Utilities
{

    public static class HtmlHelpers
    {
        public static IHtmlContent HelloWorldHTMLString(this IHtmlHelper htmlHelper)
            => new HtmlString("<strong>Hello World2</strong>");

        public static String HelloWorldString(this IHtmlHelper htmlHelper)
            => "<strong>Hello World1</strong>";


        public static IHtmlContent LinkButton(this IHtmlHelper html, string controller, string action, string buttonName, string area = "")
        {
            var arealink = @"href=/" + area + "/" + controller + "/" + action;
            var link = "href=/" + controller + "/" + action;
            if (!string.IsNullOrEmpty(area))
                return new HtmlString("<li class=''><a " + arealink + " ><i class='fa fa-circle-o'></i> " + buttonName + " </a></li>");
            return new HtmlString("<li class=''><a " + link + " ><i class='fa fa-circle-o'></i> " + buttonName + " </a></li>");

        }
    }
}
