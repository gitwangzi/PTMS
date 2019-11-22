using System;
using Gsafety.PTMS.Media.Metadata;

namespace Gsafety.PTMS.Media.Audio.Shoutcast
{
    public interface IShoutcastMetadataFilterFactory
    {
        IAudioParser Create(ISegmentMetadata segmentMetadata, IAudioParser audioParser, Action<ITrackMetadata> reportMetadata, int interval);
    }

    public class ShoutcastMetadataFilterFactory : IShoutcastMetadataFilterFactory
    {
        readonly IShoutcastEncodingSelector _shoutcastEncodingSelector;

        public ShoutcastMetadataFilterFactory(IShoutcastEncodingSelector shoutcastEncodingSelector)
        {
            if (null == shoutcastEncodingSelector)
                throw new ArgumentNullException("shoutcastEncodingSelector");

            _shoutcastEncodingSelector = shoutcastEncodingSelector;
        }

        public IAudioParser Create(ISegmentMetadata segmentMetadata, IAudioParser audioParser, Action<ITrackMetadata> reportMetadata, int interval)
        {
            var encoding = _shoutcastEncodingSelector.GetEncoding(segmentMetadata.Url);

            return new ShoutcastMetadataFilter(audioParser, reportMetadata, interval, encoding);
        }
    }
}
