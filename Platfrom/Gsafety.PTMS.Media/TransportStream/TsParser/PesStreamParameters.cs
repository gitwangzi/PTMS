using System;
using Gsafety.PTMS.Media.Metadata;
using Gsafety.PTMS.Media.TransportStream.TsParser.Utility;

namespace Gsafety.PTMS.Media.TransportStream.TsParser
{
    public class PesStreamParameters
    {
        readonly ITsPesPacketPool _pesPacketPool;

        public PesStreamParameters(ITsPesPacketPool pesPacketPool)
        {
            if (null == pesPacketPool)
                throw new ArgumentNullException("pesPacketPool");

            _pesPacketPool = pesPacketPool;
        }

        public uint Pid { get; set; }
        public TsStreamType StreamType { get; set; }
        public Action<TsPesPacket> NextHandler { get; set; }

        public ITsPesPacketPool PesPacketPool
        {
            get { return _pesPacketPool; }
        }

        public IMediaStreamMetadata MediaStreamMetadata { get; set; }
    }
}
