using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.Metadata;

namespace Gsafety.PTMS.Media.Configuration
{
    public abstract class VideoConfigurator : ConfiguratorBase, IVideoConfigurationSource
    {
        protected VideoConfigurator(string fourCc, ContentType contentType, IMediaStreamMetadata mediaStreamMetadata)
            : base(contentType, mediaStreamMetadata)
        {
            VideoFourCc = fourCc;
        }

        public int? Height { get; protected set; }
        public int? Width { get; protected set; }

        public int? FrameRateNumerator { get; protected set; }

        public int? FrameRateDenominator { get; protected set; }

        public string VideoFourCc { get; protected set; }
    }
}
