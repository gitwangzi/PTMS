using System;
using System.Threading;
using Gsafety.PTMS.Media.TransportStream.TsParser.Utility;

namespace Gsafety.PTMS.Media.TransportStream.TsParser
{
    public class TsPesPacket
    {
        internal BufferInstance BufferEntry;

        public byte[] Buffer
        {
            get { return BufferEntry.Buffer; }
        }

        public int Index;
        public int Length;
        public TimeSpan PresentationTimestamp;
        public TimeSpan? DecodeTimestamp;
        public TimeSpan? Duration;

        public void Clear()
        {
            Index = Length = 0;
            PresentationTimestamp = TimeSpan.Zero;
            DecodeTimestamp = null;
            Duration = null;
        }

#if DEBUG
        static int _packetCount;
        public readonly int PacketId = Interlocked.Increment(ref _packetCount);
#endif

        public override string ToString()
        {
#if DEBUG
            return string.Format("Packet({0}) index {1} length {2} duration {3} timestamp {4}/{5} buffer {6}",
                PacketId, Index, Length, Duration, PresentationTimestamp, DecodeTimestamp, BufferEntry);
#else
            return string.Format("Packet index {0} length {1} duration {2} timestamp {3}/{4} buffer {5}",
                Index, Length, Duration, PresentationTimestamp, DecodeTimestamp, BufferEntry);
#endif
        }
    }
}
