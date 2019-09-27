using System;
using System.Diagnostics;
using Gsafety.PTMS.Media.Configuration;
using Gsafety.PTMS.Media.Pes;
using Gsafety.PTMS.Media.TransportStream.TsParser;
using Gsafety.PTMS.Media.TransportStream.TsParser.Utility;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media.Audio
{
    public abstract class AudioStreamHandler : PesStreamHandler
    {
        protected readonly IAudioConfigurator AudioConfigurator;
        protected readonly Action<TsPesPacket> NextHandler;
        readonly IAudioFrameHeader _frameHeader;
        readonly int _minimumPacketSize;
        readonly ITsPesPacketPool _pesPacketPool;
        protected AudioParserBase Parser;
        bool _isConfigured;

        protected AudioStreamHandler(PesStreamParameters parameters, IAudioFrameHeader frameHeader, IAudioConfigurator configurator, int minimumPacketSize)
            : base(parameters)
        {
            if (null == parameters)
                throw new ArgumentNullException("parameters");
            if (null == parameters.PesPacketPool)
                throw new ArgumentException("PesPacketPool cannot be null", "parameters");
            if (null == parameters.NextHandler)
                throw new ArgumentException("NextHandler cannot be null", "parameters");
            if (minimumPacketSize < 1)
                throw new ArgumentOutOfRangeException("minimumPacketSize", "minimumPacketSize must be positive: " + minimumPacketSize);
            if (null == frameHeader)
                throw new ArgumentNullException("frameHeader");

            _pesPacketPool = parameters.PesPacketPool;
            NextHandler = parameters.NextHandler;
            _frameHeader = frameHeader;
            AudioConfigurator = configurator;
            _minimumPacketSize = minimumPacketSize;
        }

        public override IConfigurationSource Configurator
        {
            get { return AudioConfigurator; }
        }

        public override TimeSpan? GetDuration(TsPesPacket packet)
        {
            if (packet.Duration.HasValue)
                return packet.Duration;

            var duration = TimeSpan.Zero;

            var length = packet.Length;
            var endOffset = packet.Index + length;
            int nextFrameOffset;
            var skipLength = 0;

            for (var i = packet.Index; i < endOffset; i += nextFrameOffset, length -= nextFrameOffset)
            {
                if (_frameHeader.Parse(packet.Buffer, i, length))
                {
                    duration += _frameHeader.Duration;

                    if (_frameHeader.HeaderOffset > 0)
                        LoggerInstance.Debug("AudioStreamHandler.GetDuration() skipping {0} bytes before frame", _frameHeader.HeaderOffset);

                    nextFrameOffset = _frameHeader.HeaderOffset + _frameHeader.FrameLength;
                    skipLength = 0;
                }
                else
                {
                    if (length > _frameHeader.HeaderOffset + _minimumPacketSize)
                    {
                        nextFrameOffset = _frameHeader.HeaderOffset + 1;
                        skipLength += nextFrameOffset;
                        continue;
                    }

                    LoggerInstance.Debug("AudioStreamHandler.GetDuration() unable to find frame, skipping {0} bytes", length + skipLength);
                    break;
                }
            }

            packet.Duration = duration;

            return duration;
        }

        public override void PacketHandler(TsPesPacket packet)
        {
            base.PacketHandler(packet);

            if (null == packet)
            {
                if (null != Parser)
                    Parser.FlushBuffers();

                if (null != NextHandler)
                    NextHandler(null);

                return;
            }

            if (null != Parser)
            {
                Parser.Position = packet.PresentationTimestamp;
                Parser.ProcessData(packet.Buffer, packet.Index, packet.Length);

                _pesPacketPool.FreePesPacket(packet);

                return;
            }

            //Reject garbage packet
            if (packet.Length < _minimumPacketSize)
            {
                _pesPacketPool.FreePesPacket(packet);
                return;
            }

            if (!_isConfigured)
            {
                if (_frameHeader.Parse(packet.Buffer, packet.Index, packet.Length, true))
                {
                    _isConfigured = true;
                    AudioConfigurator.Configure(_frameHeader);
                }
            }

            NextHandler(packet);
        }
    }
}
