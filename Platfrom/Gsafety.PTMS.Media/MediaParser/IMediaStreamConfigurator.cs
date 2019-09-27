using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.MediaManager;
using Gsafety.PTMS.Media.Utility;

namespace Gsafety.PTMS.Media.MediaParser
{
    public interface IMediaStreamConfigurator : IDisposable
    {
        TimeSpan? TotalTime { get; set; }
        TimeSpan? SeekTarget { get; set; }
        IMediaManager MediaManager { get; set; }

        void Initialize();

        Task<TMediaStreamSource> CreateMediaStreamSourceAsync<TMediaStreamSource>(CancellationToken cancellationToken)
            where TMediaStreamSource : class;

        Task PlayAsync(IMediaConfiguration configuration, CancellationToken cancellationToken);
        Task CloseAsync();

        void ReportError(string message);
        void CheckForSamples();

        void ValidateEvent(MediaStreamFsm.MediaEvent mediaEvent);
    }

    public static class MediaStreamSourceExtensions
    {
        public static Task PlayAsync(this IMediaStreamConfigurator mediaStreamConfigurator,
            IEnumerable<IMediaParserMediaStream> mediaParserMediaStreams, TimeSpan? duration, CancellationToken cancellationToken)
        {
            return mediaStreamConfigurator.PlayAsync(mediaParserMediaStreams.CreateMediaConfiguration(duration), cancellationToken);
        }
    }
}
