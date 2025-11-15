using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PTXYZ_OvertimeApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
