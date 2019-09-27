using Autofac;
using Gsafety.PTMS.Media.Web;
using Gsafety.PTMS.Media.Web.HttpConnection;
using Gsafety.PTMS.Media.Web.HttpConnectionReader;

namespace Gsafety.PTMS.Media
{
    public class HttpConnectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SocketWrapper>().As<ISocket>().ExternallyOwned();

            builder.RegisterType<HttpConnection>().As<IHttpConnection>().ExternallyOwned();
            builder.RegisterType<HttpConnectionFactory>().As<IHttpConnectionFactory>().SingleInstance();
            builder.RegisterType<HttpConnectionRequestFactory>().As<IHttpConnectionRequestFactory>().SingleInstance();
            builder.RegisterType<HttpConnectionRequestFactoryParameters>().As<IHttpConnectionRequestFactoryParameters>().SingleInstance();

            builder.RegisterType<HttpConnectionWebReaderManager>().As<IWebReaderManager>().SingleInstance();

            builder.RegisterType<HttpEncoding>().As<IHttpEncoding>().SingleInstance();
            builder.RegisterType<HttpHeaderSerializer>().As<IHttpHeaderSerializer>().SingleInstance();
            builder.RegisterType<UserAgentEncoder>().As<IUserAgentEncoder>().SingleInstance();
        }
    }
}
