using Gsafety.PTMS.Media.Configuration;

namespace Gsafety.PTMS.Media.MediaParser
{
    public interface IMediaParserMediaStream
    {
        IConfigurationSource ConfigurationSource { get; }
        IStreamSource StreamSource { get; }
    }
}
