using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;
using System.Web.Http;
namespace MobileService.Controller
{
    partial class MobileServiceBase : ServiceBase
    {
        HttpSelfHostServer host = null;
        public MobileServiceBase()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Diagnostics.Debugger.Launch();
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
            host = new HttpSelfHostServer(config);
            host.OpenAsync().Wait();
        }

        protected override void OnStop()
        {
            host.CloseAsync();
        }
    }
}
