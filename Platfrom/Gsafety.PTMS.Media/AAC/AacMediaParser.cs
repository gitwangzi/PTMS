using Gsafety.PTMS.Media.Audio;
using Gsafety.PTMS.Media.Audio.Shoutcast;
using Gsafety.PTMS.Media.Metadata;
using Gsafety.PTMS.Media.TransportStream.TsParser;
using Gsafety.PTMS.Media.TransportStream.TsParser.Utility;

namespace Gsafety.PTMS.Media.AAC
{
    public sealed class AacMediaParser : AudioMediaParser<AacParser, AacConfigurator>
    {
        static readonly TsStreamType StreamType = TsStreamType.FindStreamType(TsStreamType.AacStreamType);

        public AacMediaParser(ITsPesPacketPool pesPacketPool, IShoutcastMetadataFilterFactory shoutcastMetadataFilterFactory, IMetadataSink metadataSink)
            : base(StreamType, new AacConfigurator(null), pesPacketPool, shoutcastMetadataFilterFactory, metadataSink)
        {
            Parser = new AacParser(pesPacketPool, Configurator.Configure, SubmitPacket);
        }
    }
}
