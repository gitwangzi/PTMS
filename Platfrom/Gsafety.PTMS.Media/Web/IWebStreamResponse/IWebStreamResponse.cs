using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Media.Web
{
    public interface IWebStreamResponse : IDisposable
    {
        bool IsSuccessStatusCode { get; }
        Uri ActualUrl { get; }
        int HttpStatusCode { get; }
        long? ContentLength { get; }
        void EnsureSuccessStatusCode();
        Task<Stream> GetStreamAsync(CancellationToken cancellationToken);
    }
}
