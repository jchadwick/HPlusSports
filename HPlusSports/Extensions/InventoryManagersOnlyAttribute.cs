using HPlusSports.Models;

namespace HPlusSports.Extensions
{
  public class InventoryManagersOnlyAttribute : System.Web.Mvc.AuthorizeAttribute
  {
    public InventoryManagersOnlyAttribute()
    {
      Roles = UserRoles.Admin;
    }
  }
}