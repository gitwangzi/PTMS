using System;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.Builder;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.MediaManager;

namespace Gsafety.PTMS.Media
{
    public interface IMediaStreamFacadeBase : IDisposable
    {
        /// <summary>
        ///     Force the source to be considered <see cref="Gsafety.PTMS.Media.Content.ContentType" />.
        ///     The type will be detected if null. Set this value before calling CreateMediaStreamSourceAsync().
        ///     If an M3U8 playlist contains MP3 segments, then this should be set to M3U8.
        /// </summary>
        /// <seealso cref="Gsafety.PTMS.Media.Content.ContentTypes" />
        ContentType ContentType { get; set; }

        /// <summary>
        ///     Force the source's stream to be considered <see cref="Gsafety.PTMS.Media.Content.ContentType" />.
        ///     The type will be detected if null. Set this value before calling CreateMediaStreamSourceAsync().
        ///     If an M3U8 playlist contains MP3 segments, then this should be set to MP3.
        /// </summary>
        /// <seealso cref="Gsafety.PTMS.Media.Content.ContentTypes" />
        ContentType StreamContentType { get; set; }

        TimeSpan? SeekTarget { get; set; }
        MediaManagerState State { get; }
        IBuilder<IMediaManager> Builder { get; }
        bool IsDisposed { get; }

        Task PlayingTask { get; }

        event EventHandler<MediaManagerStateEventArgs> StateChange;

        Task StopAsync(CancellationToken cancellationToken);
        Task CloseAsync();
    }

    public interface IMediaStreamFacadeBase<TMediaStreamSource> : IMediaStreamFacadeBase
    where TMediaStreamSource : class
    {
        Task<TMediaStreamSource> CreateMediaStreamSourceAsync(Uri source, CancellationToken cancellationToken);
    }
}
