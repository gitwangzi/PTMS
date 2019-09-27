using System;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.Content;

namespace Gsafety.PTMS.Media.Web
{
    public interface IWebReaderManager
    {
        IWebReader CreateReader(Uri url, ContentKind contentKind, IWebReader parent = null, ContentType contentType = null);
        IWebCache CreateWebCache(Uri url, ContentKind contentKind, IWebReader parent = null, ContentType contentType = null);
        Task<ContentType> DetectContentTypeAsync(Uri url, ContentKind contentKind, CancellationToken cancellationToken, IWebReader parent = null);
    }

    public static class WebReaderManagerExtensions
    {
        public static IWebReader CreateRootReader(this IWebReaderManager webReaderManager, ContentKind contentKind, ContentType contentType = null)
        {
            return webReaderManager.CreateReader(null, contentKind, contentType: contentType);
        }

        public static IWebReader CreateRootReader(this IWebReaderManager webReaderManager, ContentType contentType = null)
        {
            return webReaderManager.CreateRootReader(ContentKind.Unknown, contentType);
        }
    }
}
