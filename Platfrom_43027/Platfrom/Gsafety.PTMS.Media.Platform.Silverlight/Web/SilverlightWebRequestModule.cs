using Autofac;
using Gsafety.PTMS.Media.Web;
using Gsafety.PTMS.Media.Web.WebRequestReader;

namespace Gsafety.PTMS.Media
{
    public class SilverlightWebRequestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SilverlightHttpWebRequests>()
                .As<IHttpWebRequests>()
                .SingleInstance();

            builder.RegisterType<HttpWebRequestWebReaderManager>()
                .As<IWebReaderManager>()
                .SingleInstance();
        }
    }
}
