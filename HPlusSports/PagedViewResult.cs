using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HPlusSports
{
  public class PagedResults : ActionFilterAttribute
  {
    public const int DefaultPageSize = 8;

    static int? ToInt(string value)
    {
      return int.TryParse(value, out int intValue) ? (int?)intValue : null;
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
      var request = filterContext.HttpContext.Request;
      var ViewData = filterContext.Controller.ViewData;

      var model = (filterContext.Result as ViewResultBase)?.Model as IEnumerable<object>;

      if (model == null)
        return;

      var resultsCount = model.Count();
      var pageSize = ToInt(request["count"]).GetValueOrDefault(DefaultPageSize);
      var currentPage = ToInt(request["page"]).GetValueOrDefault(1);
      var pageCount = resultsCount / pageSize + (resultsCount % pageSize > 0 ? 1 : 0);
      var previousPage = (currentPage - 1 > 0) ? currentPage - 1 : (int?)null;
      var nextPage = (currentPage + 1 <= pageCount) ? currentPage + 1 : (int?)null;

      var collection = 
        model
          .Skip((currentPage - 1) * pageSize)
          .Take(pageSize);

      ViewData["PageSize"] = pageSize;
      ViewData["ResultsCount"] = resultsCount;
      ViewData["CurrentPage"] = currentPage;
      ViewData["PageCount"] = pageCount;
      ViewData["PreviousPage"] = previousPage;
      ViewData["NextPage"] = nextPage;
    }
  }
}