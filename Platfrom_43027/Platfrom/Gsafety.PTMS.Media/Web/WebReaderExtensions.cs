using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.Utility;

namespace Gsafety.PTMS.Media.Web
{
    public static class WebReaderExtensions
    {
        public static IWebReader CreateChild(this IWebReader webReader, Uri url, ContentKind contentKind, ContentType contentType = null)
        {
            return webReader.Manager.CreateReader(url, contentKind, webReader, contentType);
        }

        public static IWebCache CreateWebCache(this IWebReader webReader, Uri url, ContentKind contentKind, ContentType contentType = null)
        {
            return webReader.Manager.CreateWebCache(url, contentKind, webReader, contentType);
        }

        public static Task<ContentType> DetectContentTypeAsync(this IWebReader webReader, Uri url, ContentKind contentKind, CancellationToken cancellationToken)
        {
            return webReader.Manager.DetectContentTypeAsync(url, contentKind, cancellationToken, webReader);
        }

        public static async Task<TReturn> ReadStreamAsync<TReturn>(this IWebReader webReader, Uri url, IRetry retry,
            Func<Uri, Stream, TReturn> reader, CancellationToken cancellationToken)
        {
            for (; ; )
            {
                using (var response = await webReader.GetWebStreamAsync(url, true, cancellationToken))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var actualUrl = response.ActualUrl;

                        using (var stream = await response.GetStreamAsync(cancellationToken).ConfigureAwait(false))
                        using (var ms = new MemoryStream((int)(response.ContentLength ?? 4096)))
                        {
                            await stream.CopyToAsync(ms, 4096, cancellationToken).ConfigureAwait(false);

                            ms.Position = 0;

                            return reader(actualUrl, ms);
                        }
                    }

                    if (!RetryPolicy.IsRetryable((HttpStatusCode)response.HttpStatusCode))
                        response.EnsureSuccessStatusCode();

                    var canRetry =await retry.CanRetryAfterDelayAsync(cancellationToken);

                    if (!canRetry)
                        response.EnsureSuccessStatusCode();
                }
            }
        }

        public static async Task<TReturn> ReadStreamAsync<TReturn>(this IWebReader webReader, Uri url, Retry retry,
            Func<Uri, Stream, CancellationToken, Task<TReturn>> reader, CancellationToken cancellationToken)
        {
            for (; ; )
            {
                using (var response = await webReader.GetWebStreamAsync(url, false, cancellationToken))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var actualUrl = response.ActualUrl;

                        using (var stream = await response.GetStreamAsync(cancellationToken).ConfigureAwait(false))
                        {
                            return await reader(actualUrl, stream, cancellationToken).ConfigureAwait(false);
                        }
                    }

                    if (!RetryPolicy.IsRetryable((HttpStatusCode)response.HttpStatusCode))
                        response.EnsureSuccessStatusCode();

                    var canRetry = await retry.CanRetryAfterDelayAsync(cancellationToken)
                        .ConfigureAwait(false);

                    if (!canRetry)
                        response.EnsureSuccessStatusCode();
                }
            }
        }
    }
}
