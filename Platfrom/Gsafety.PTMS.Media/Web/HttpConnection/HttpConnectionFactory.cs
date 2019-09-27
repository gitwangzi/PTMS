using System;
using Gsafety.PTMS.Media.Web.HttpConnection;

namespace Gsafety.PTMS.Media.Web.HttpConnectionReader
{
    public interface IHttpConnectionFactory
    {
        IHttpConnection CreateHttpConnection();
    }

    public class HttpConnectionFactory : IHttpConnectionFactory
    {
        readonly Func<IHttpConnection> _httpConnectionFactory;

        public HttpConnectionFactory(Func<IHttpConnection> httpConnectionFactory)
        {
            if (null == httpConnectionFactory)
                throw new ArgumentNullException("httpConnectionFactory");

            _httpConnectionFactory = httpConnectionFactory;
        }


        public IHttpConnection CreateHttpConnection()
        {
            return _httpConnectionFactory();
        }
    }
}
