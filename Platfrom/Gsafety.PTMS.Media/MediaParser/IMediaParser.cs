using System;
using System.Collections.Generic;
using Gsafety.PTMS.Media.Buffering;
using Gsafety.PTMS.Media.Metadata;
using Gsafety.PTMS.Media.TransportStream.TsParser;

namespace Gsafety.PTMS.Media.MediaParser
{
    public interface IMediaParser : IDisposable
    {
        ICollection<IMediaParserMediaStream> MediaStreams { get; }
        bool EnableProcessing { get; set; }
        TimeSpan StartPosition { get; set; }
        event EventHandler ConfigurationComplete;
        void ProcessEndOfData();
        void ProcessData(byte[] buffer, int offset, int length);
        void FlushBuffers();
        void Initialize(IBufferingManager bufferingManager, Action<IProgramStreams> programStreamsHandler = null);

        void InitializeStream(IStreamMetadata streamMetadata);
        void StartSegment(ISegmentMetadata segmentMetadata);
        void SetTrackMetadata(ITrackMetadata trackMetadata);
    }
}
