using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPlusSports
{
  public class AllowPartialRenderingAttribute : System.Web.Mvc.ActionFilterAttribute
  {
    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
      if (filterContext.HttpContext.Request.IsAjaxRequest())
      {
        var result = filterContext.Result as ViewResult;

        if (result == null)
          return;

        filterContext.Result = new PartialViewResult
        {
          TempData = result.TempData,
          View = result.View,
          ViewData = result.ViewData,
          ViewEngineCollection = result.ViewEngineCollection,
          ViewName = result.ViewName,
        };
      }
    }
  }
}