using System;
using Gsafety.PTMS.Media.TransportStream.TsParser;

namespace Gsafety.PTMS.Media
{
    public interface IStreamSource
    {
        bool HasSample { get; }

        float? BufferingProgress { get; }

        bool IsEof { get; }
        TimeSpan? PresentationTimestamp { get; }

        TsPesPacket GetNextSample();

        void FreeSample(TsPesPacket packet);
        bool DiscardPacketsBefore(TimeSpan value);
    }
}
