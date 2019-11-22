using System;
using System.Collections.Generic;
using System.Diagnostics;
using Gsafety.PTMS.Media.Configuration;
using Gsafety.PTMS.Media.MediaParser;
using Gsafety.PTMS.Media.TransportStream.TsParser;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media
{
    public sealed class MediaStream : IMediaParserMediaStream, IDisposable
    {
        readonly IConfigurationSource _configurator;
        readonly Action<TsPesPacket> _freePacket;
        readonly List<TsPesPacket> _packets = new List<TsPesPacket>();
        readonly IStreamBuffer _streamBuffer;

        public MediaStream(IConfigurationSource configurator, IStreamBuffer streamBuffer, Action<TsPesPacket> freePacket)
        {
            if (null == streamBuffer)
                throw new ArgumentNullException("streamBuffer");
            if (null == freePacket)
                throw new ArgumentNullException("freePacket");

            _configurator = configurator;
            _streamBuffer = streamBuffer;
            _freePacket = freePacket;
        }

        public ICollection<TsPesPacket> Packets
        {
            get { return _packets; }
        }

        public void Dispose()
        {
            Flush();
        }

        public IConfigurationSource ConfigurationSource
        {
            get { return _configurator; }
        }

        public IStreamSource StreamSource
        {
            get { return _streamBuffer; }
        }

        public void Flush()
        {
            if (_packets.Count <= 0)
                return;

            foreach (var packet in _packets)
            {
                if (null == packet)
                    continue;

                _freePacket(packet);
            }

            _packets.Clear();
        }

        public void EnqueuePacket(TsPesPacket packet)
        {
            _packets.Add(packet);
        }

        public bool PushPackets()
        {
            if (_packets.Count <= 0)
                return false;

            if (!_streamBuffer.TryEnqueue(_packets))
            {
                //LoggerInstance.Debug("MediaStream.PushPackets() the stream buffer was not ready to accept the packets: " + _streamBuffer);
                return false;
            }
            else
            {
                //LoggerInstance.Debug("MediaStream.PushPackets() count {0} buffer: {1}", _packets.Count, _streamBuffer);
            }

            _packets.Clear();

            return true;
        }
    }
}
