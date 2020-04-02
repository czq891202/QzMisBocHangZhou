using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace QzMisBocHangZhou.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LoadLog4netConfig();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        private void LoadLog4netConfig()
        {
            string filePath = Server.MapPath("~/App_Data/log4net.config");
            var fileInfo = new FileInfo(filePath);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(fileInfo);
        }
    }
}
