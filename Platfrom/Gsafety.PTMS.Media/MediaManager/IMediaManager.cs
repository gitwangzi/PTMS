using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.MediaParser;

namespace Gsafety.PTMS.Media.MediaManager
{
    public interface IMediaManager : IDisposable
    {
        TimeSpan? SeekTarget { get; set; }
        MediaManagerState State { get; }

        /// <summary>
        ///     Force the source to be considered <see cref="Gsafety.PTMS.Media.Content.ContentType" />.
        ///     The type will be detected if null.
        /// </summary>
        /// <seealso cref="Gsafety.PTMS.Media.Content.ContentTypes" />
        ContentType ContentType { get; set; }

        /// <summary>
        ///     Force the source's stream to be considered <see cref="Gsafety.PTMS.Media.Content.ContentType" />.
        ///     The type will be detected if null.
        /// </summary>
        /// <seealso cref="Gsafety.PTMS.Media.Content.ContentTypes" />
        ContentType StreamContentType { get; set; }

        Task PlayingTask { get; }

        Task<IMediaStreamConfigurator> OpenMediaAsync(ICollection<Uri> source, CancellationToken cancellationToken);
        Task StopMediaAsync(CancellationToken cancellationToken);
        Task CloseMediaAsync();

        Task<TimeSpan> SeekMediaAsync(TimeSpan position);

        event EventHandler<MediaManagerStateEventArgs> OnStateChange;
    }
}
