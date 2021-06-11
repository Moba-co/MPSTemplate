using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Moba.Common.DTOs;

namespace Moba.Common.Extensions.HtmlHelpers
{
    public static class HtmlExtension
    {
        /// <summary>
        /// for add active class to a element aht is active
        /// </summary>
        /// <param name="htmlHelper" type="extension for IHtmlHelper"></param>
        /// <param name="controller" type="string"></param>
        /// <param name="activeText"></param>
        /// <returns> active string </returns>
        public static string IsActive(this IHtmlHelper htmlHelper, string controller,string activeText = "active")
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var routeController = routeData.Values["controller"].ToString();

            var returnActive = (controller == routeController);

            return returnActive ? activeText : "";
        }

        public static string IsActive(this IHtmlHelper htmlHelper, string controller, string action,string activeText = "active")
        {
            var routeData = htmlHelper.ViewContext.RouteData;
            var routeAction = routeData.Values["action"].ToString();
            var routeController = routeData.Values["controller"].ToString();

            var returnActive = (controller == routeController && (action == routeAction || routeAction == "Details"));

            return returnActive ? activeText : "";
        }
        
        public static List<SelectListItem> ToAspSelectList(this List<GeneralSelectListItem> list)
        {
            return list?.Select(s => new SelectListItem
            {
                Selected = s.Selected,
                Text = s.Text,
                Value = s.Value
            })?.ToList() ?? new List<SelectListItem>();
        }
    }
}