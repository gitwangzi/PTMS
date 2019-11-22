using System;
using System.Net;

namespace Gsafety.PTMS.Media.Web.HttpConnectionReader
{
    public interface IHttpConnectionRequestFactoryParameters
    {
        Uri Proxy { get; }
        CookieContainer Cookies { get; }
    }

    public class HttpConnectionRequestFactoryParameters : IHttpConnectionRequestFactoryParameters
    {
        public Uri Proxy { get; set; }
        public CookieContainer Cookies { get; set; }
    }

    public static class HttpCOnnectionRequestFactoryParametersExtensions
    {
        public static void SetParameter(this IMediaStreamFacadeBase mediaStreamFacade, IHttpConnectionRequestFactoryParameters parameters)
        {
            mediaStreamFacade.Builder.RegisterSingleton(parameters);
        }
    }
}
