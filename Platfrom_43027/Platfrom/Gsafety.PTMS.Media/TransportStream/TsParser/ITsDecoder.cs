using Gsafety.PTMS.Media.Metadata;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Media.TransportStream.TsParser
{
    public interface ITsDecoder : IDisposable
    {
        bool EnableProcessing { get; set; }
        void ParseEnd();
        void Parse(byte[] buffer, int offset, int length);
        void Initialize(Func<TsStreamType, uint, IMediaStreamMetadata, TsPacketizedElementaryStream> pesStreamFactory, Action<IProgramStreams> programStreamsHandler);
        void FlushBuffers();

        void UnregisterHandler(uint pid);
        void RegisterHandler(uint pid, Action<TsPacket> add);
        TsPacketizedElementaryStream CreateStream(TsStreamType streamType, uint pid, IMediaStreamMetadata mediaStreamMetadata);
    }
}
