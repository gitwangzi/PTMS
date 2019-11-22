using System;
using Gsafety.PTMS.Media.TransportStream.TsParser;

namespace Gsafety.PTMS.Media
{
    public interface ITsTimestamp
    {
        TimeSpan StartPosition { get; set; }
        TimeSpan? Offset { get; }
        void Flush();
        bool ProcessPackets();
        void RegisterMediaStream(MediaStream mediaStream, Func<TsPesPacket, TimeSpan?> getDuration);
    }
}
