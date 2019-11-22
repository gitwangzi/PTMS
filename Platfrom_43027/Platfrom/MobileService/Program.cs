using MobileService.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace MobileService
{
    class Program
    {
        static void Main(string[] args)
        {
            string runmode = "WindowsService";
            if (System.Configuration.ConfigurationManager.AppSettings["RunMode"] != null)
            {
                runmode = System.Configuration.ConfigurationManager.AppSettings["RunMode"].ToString();
            }
            if (runmode == "Console")
            {
                string MobileServiceUrl = System.Configuration.ConfigurationManager.AppSettings["MobileServiceUrl"].ToString();

                HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(MobileServiceUrl);
                config.Routes.MapHttpRoute("DefaultAPI", "api/{controller}/{action}/{authCode}/{id}/", new
                {
                    authCode = RouteParameter.Optional,
                    id = RouteParameter.Optional
                });
                config.Routes.MapHttpRoute("Authenticate", "api/{controller}/{action}/{sim}/{vehiclenum}/{license}/{operationlicense}");
                config.Routes.MapHttpRoute("PageAPIWithType", "api/{controller}/{action}/{authCode}/{pageIndex}/{pageValue}/{starttime}/{endtime}/{type}");
                config.Routes.MapHttpRoute("PageAPI", "api/{controller}/{action}/{authCode}/{pageIndex}/{pageValue}/{starttime}/{endtime}");
                HttpSelfHostServer server = new HttpSelfHostServer(config);
                server.OpenAsync().Wait();
                Console.ReadLine();
                server.CloseAsync();
            }
            else if (runmode == "WindowsService")
            {
                ServiceBase[] ServicesToRun = new ServiceBase[] 
                { 
                    new MobileServiceBase()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
