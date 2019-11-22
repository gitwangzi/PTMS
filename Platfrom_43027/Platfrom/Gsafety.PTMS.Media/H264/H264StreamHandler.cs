using System;
using Gsafety.PTMS.Media.Configuration;
using Gsafety.PTMS.Media.Pes;
using Gsafety.PTMS.Media.TransportStream.TsParser;
using Gsafety.PTMS.Media.TransportStream.TsParser.Utility;

namespace Gsafety.PTMS.Media.H264
{
    public sealed class H264StreamHandler : PesStreamHandler
    {
        readonly H264Configurator _configurator;
        readonly Action<TsPesPacket> _nextHandler;
        readonly NalUnitParser _parser;
        readonly ITsPesPacketPool _pesPacketPool;
        readonly RbspDecoder _rbspDecoder = new RbspDecoder();
        INalParser _currentParser;
        bool _isConfigured;

        public H264StreamHandler(PesStreamParameters parameters)
            : base(parameters)
        {
            if (null == parameters)
                throw new ArgumentNullException("parameters");
            if (null == parameters.PesPacketPool)
                throw new ArgumentException("PesPacketPool cannot be null", "parameters");
            if (null == parameters.NextHandler)
                throw new ArgumentException("NextHandler cannot be null", "parameters");

            _pesPacketPool = parameters.PesPacketPool;
            _nextHandler = parameters.NextHandler;
            _configurator = new H264Configurator(parameters.MediaStreamMetadata, parameters.StreamType.Description);

            _parser = new NalUnitParser(ResolveHandler);
        }

        public override IConfigurationSource Configurator
        {
            get { return _configurator; }
        }

        NalUnitParser.ParserStateHandler ResolveHandler(byte arg)
        {
            var nalUnitType = (NalUnitType)(arg & 0x1f);

            switch (nalUnitType)
            {
                case NalUnitType.Sps:
                    _rbspDecoder.CompletionHandler = _configurator.ParseSpsBytes;
                    _currentParser = _rbspDecoder;
                    break;
                case NalUnitType.Pps:
                    _rbspDecoder.CompletionHandler = _configurator.ParsePpsBytes;
                    _currentParser = _rbspDecoder;
                    break;
                case NalUnitType.Slice:
                case NalUnitType.Dpa:
                case NalUnitType.Idr:
                    _rbspDecoder.CompletionHandler = _configurator.ParseSliceHeader;
                    _currentParser = _rbspDecoder;
                    break;
                //case NalUnitType.Aud:
                //    _rbspDecoder.CompletionHandler =_configurator.ParseAud;
                //    _currentParser = _rbspDecoder;
                //    break;
                case NalUnitType.Sei:
                    _rbspDecoder.CompletionHandler = _configurator.ParseSei;
                    _currentParser = _rbspDecoder;
                    break;
                default:
                    _currentParser = null;
                    return null;
            }

            if (null == _currentParser)
                return null;

            return _currentParser.Parse;
        }

        public override void PacketHandler(TsPesPacket packet)
        {
            base.PacketHandler(packet);

            if (null == packet)
            {
                _nextHandler(null);

                return;
            }

            // Reject garbage packet
            if (packet.Length < 1)
            {
                _pesPacketPool.FreePesPacket(packet);

                return;
            }

            if (!_isConfigured)
            {
                _parser.Reset();

                _parser.Parse(packet.Buffer, packet.Index, packet.Length);

                _isConfigured = _configurator.IsConfigured;
            }

            _nextHandler(packet);
        }
    }
}
