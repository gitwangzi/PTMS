using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media.Web.WebRequestReader
{
    public static class HttpWebRequestExtensions
    {
        static readonly byte[] NoData = new byte[0];

        public static async Task<byte[]> ReadAsByteArrayAsync(this HttpWebResponse response, CancellationToken cancellationToken)
        {
            var contentLength = response.ContentLength;

            if (0 == contentLength)
                return NoData;

            if (contentLength > 2 * 1024 * 1024)
            {
                throw new WebException("Too much data for GetByteArrayAsync: " + contentLength);
            }

            using (var buffer = contentLength > 0 ? new MemoryStream((int)contentLength) : new MemoryStream())
            {
                using (var stream = response.GetResponseStream())
                {
                    //stream.CopyTo(buffer, 4096)
                    await Task.Factory.StartNew(() => { stream.CopyTo(buffer, 4096); }, cancellationToken);
                }

                return buffer.ToArray();
            }
        }

        public static async Task<HttpWebResponse> SendAsync(this HttpWebRequest request, CancellationToken cancellationToken)
        {
            var task = Task<System.Net.WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null);

            using (cancellationToken.Register(r => ((WebRequest)r).Abort(), request, false))
            {
                try
                {
                    var t = task.ConfigureAwait(false);
                    task.ContinueWith(a => { LoggerInstance.Debug("SendAsync End"); });
                    return (HttpWebResponse)await t;
                }
                catch (WebException ex)
                {
                    if (cancellationToken.IsCancellationRequested && ex.Status == WebExceptionStatus.RequestCanceled)
                        throw new OperationCanceledException(ex.Message, ex, cancellationToken);

                    throw;
                }
            }
        }
    }
}
