using System.Collections.Generic;

namespace Gsafety.PTMS.Media.Web
{
    public interface IWebReaderManagerParameters
    {
        IEnumerable<KeyValuePair<string, string>> DefaultHeaders { get; }
    }

    public class WebReaderManagerParameters : IWebReaderManagerParameters
    {
        public IEnumerable<KeyValuePair<string, string>> DefaultHeaders { get; set; }
    }

    public static class WebReaderManagerParametersExtensions
    {
        public static void SetParameter(this IMediaStreamFacadeBase mediaStreamFacade, IWebReaderManagerParameters parameters)
        {
            mediaStreamFacade.Builder.RegisterSingleton(parameters);
        }
    }
}
