using System.Collections.Generic;
using Gsafety.PTMS.Media.Configuration;

namespace Gsafety.PTMS.Media.Metadata
{
    public interface IConfigurationMetadata
    {
        IAudioConfigurationSource Audio { get; }
        IVideoConfigurationSource Video { get; }

        List<IConfigurationSource> AlternateStreams { get; }
    }

    public class ConfigurationMetadata : IConfigurationMetadata
    {
        public IAudioConfigurationSource Audio { get; set; }
        public IVideoConfigurationSource Video { get; set; }

        public List<IConfigurationSource> AlternateStreams { get; set; }
    }
}
