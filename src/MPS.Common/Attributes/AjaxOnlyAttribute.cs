using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moba.Common.Extenstions;

namespace Moba.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.IsAjax())
            {
                base.OnActionExecuting(context);
            }
            else
            {
                context.Result = new NotFoundResult();
            }
        }
    }
}