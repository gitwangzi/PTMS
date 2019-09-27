using System;
using Gsafety.PTMS.Media.TransportStream.TsParser;
using Gsafety.PTMS.Media.TransportStream.TsParser.Utility;

namespace Gsafety.PTMS.Media.Buffering
{
    public sealed class NullBufferingManager : IBufferingManager
    {
        readonly ITsPesPacketPool _packetPool;

        public NullBufferingManager(ITsPesPacketPool packetPool)
        {
            if (null == packetPool)
                throw new ArgumentNullException("packetPool");

            _packetPool = packetPool;
        }

        public float? BufferingProgress
        {
            get { return null; }
        }

        public bool IsBuffering
        {
            get { return false; }
        }

        public IStreamBuffer CreateStreamBuffer(TsStreamType streamType)
        {
            return new StreamBuffer(streamType, _packetPool.FreePesPacket, this);
        }

        public void Flush()
        { }

        public bool IsSeekAlreadyBuffered(TimeSpan position)
        {
            return true;
        }

        public void Initialize(IQueueThrottling queueThrottling, Action reportBufferingChange)
        { }

        public void Shutdown(IQueueThrottling queueThrottling)
        { }

        public void Refresh()
        { }

        public void ReportExhaustion()
        { }

        public void ReportEndOfData()
        { }

        public void Dispose()
        { }
    }
}
