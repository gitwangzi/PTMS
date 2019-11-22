using System;
using Gsafety.PTMS.Media.TransportStream.TsParser;

namespace Gsafety.PTMS.Media.Buffering
{
    public interface IBufferingManager : IDisposable
    {
        float? BufferingProgress { get; }
        bool IsBuffering { get; }
        IStreamBuffer CreateStreamBuffer(TsStreamType streamType);
        void Flush();
        bool IsSeekAlreadyBuffered(TimeSpan position);
        void Initialize(IQueueThrottling queueThrottling, Action reportBufferingChange);
        void Shutdown(IQueueThrottling queueThrottling);
        void Refresh();
        void ReportExhaustion();
        void ReportEndOfData();
    }

    public class BufferStatus
    {
        public int Size { get; set; }
        public TimeSpan? Newest { get; set; }
        public TimeSpan? Oldest { get; set; }
        public int PacketCount { get; set; }
        public bool IsDone { get; set; }
        public bool IsValid { get; set; }
        public bool IsMedia { get; set; }
    }

    public interface IBufferingQueue
    {
        void UpdateBufferStatus(BufferStatus bufferStatus);
        void Flush();
    }
}
