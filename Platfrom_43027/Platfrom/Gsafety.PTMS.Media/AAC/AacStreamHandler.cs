using Gsafety.PTMS.Media.Audio;
using Gsafety.PTMS.Media.TransportStream.TsParser;

namespace Gsafety.PTMS.Media.AAC
{
    public class AacStreamHandler : AudioStreamHandler
    {
        const int MinimumPacketSize = 7; // Well, it needs the ADTS header at least...

        public AacStreamHandler(PesStreamParameters parameters)
            : base(parameters, new AacFrameHeader(), new AacConfigurator(parameters.MediaStreamMetadata, parameters.StreamType.Description), MinimumPacketSize)
        {
            if (AacDecoderSettings.Parameters.UseParser)
                Parser = new AacParser(parameters.PesPacketPool, AudioConfigurator.Configure, NextHandler);
        }
    }
}
