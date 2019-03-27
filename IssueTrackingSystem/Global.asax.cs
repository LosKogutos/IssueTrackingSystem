using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;

namespace IssueTrackingSystem
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "Id", "UserName", autoCreateTables: true);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

        }
    }
}
