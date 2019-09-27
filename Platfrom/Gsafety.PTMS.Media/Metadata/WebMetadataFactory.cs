using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.Web;

namespace Gsafety.PTMS.Media.Metadata
{
    public interface IWebMetadataFactory
    {
        IStreamMetadata CreateStreamMetadata(WebResponse webResponse, ContentType contentType = null);
        ISegmentMetadata CreateSegmentMetadata(WebResponse webResponse, ContentType contentType = null);
    }

    public class WebMetadataFactory : IWebMetadataFactory
    {
        public IStreamMetadata CreateStreamMetadata(WebResponse webResponse, ContentType contentType = null)
        {
            var shoutcast = new ShoutcastHeaders(webResponse.RequestUri, webResponse.Headers);

            var streamMetadata = new StreamMetadata
            {
                Url = webResponse.RequestUri,
                ContentType = contentType ?? webResponse.ContentType,
                Bitrate = shoutcast.Bitrate,
                Description = shoutcast.Description,
                Genre = shoutcast.Genre,
                Name = shoutcast.Name,
                Website = shoutcast.Website
            };

            return streamMetadata;
        }

        public ISegmentMetadata CreateSegmentMetadata(WebResponse webResponse, ContentType contentType)
        {
            var shoutcast = new ShoutcastHeaders(webResponse.RequestUri, webResponse.Headers);

            if (shoutcast.MetaInterval > 0 || shoutcast.SupportsIcyMetadata)
            {
                var segmentMetadata = new ShoutcastSegmentMetadata
                {
                    Url = webResponse.RequestUri,
                    ContentType = contentType ?? webResponse.ContentType,
                    Length = webResponse.ContentLength,
                    IcyMetaInt = shoutcast.MetaInterval,
                    SupportsIcyMetadata = shoutcast.SupportsIcyMetadata
                };

                return segmentMetadata;
            }

            var streamMetadata = new SegmentMetadata
            {
                Url = webResponse.RequestUri,
                ContentType = contentType ?? webResponse.ContentType,
                Length = webResponse.ContentLength
            };

            return streamMetadata;
        }
    }
}
