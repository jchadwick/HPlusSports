using HPlusSports.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HPlusSports
{
    public class Application : System.Web.HttpApplication
    {
        public static ShoppingCart Cart
        {
            get
            {
                var context = new HPlusSportsDbContext();
                var userId = HttpContext.Current.User.Identity.Name;
                var cart = context.ShoppingCarts.FirstOrDefault(x => x.UserId == userId);
                return cart ?? new ShoppingCart { UserId = userId };
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            HPlusSportsDbContext.Initialize();
        }
    }
}
