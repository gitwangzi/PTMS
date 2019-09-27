using System;
using Gsafety.PTMS.Media.Content;

namespace Gsafety.PTMS.Media.Metadata
{
    public interface ISegmentMetadata
    {
        Uri Url { get; }
        ContentType ContentType { get; }

        long? Length { get; }
    }

    public class SegmentMetadata : ISegmentMetadata
    {
        public Uri Url { get; set; }
        public ContentType ContentType { get; set; }

        public long? Length { get; set; }

        public override string ToString()
        {
            var url = null == Url ? "null" : Url.ToString();
            var type = null == ContentType ? "<unknown>" : ContentType.Name;

            return "Segment <" + url + "> " + type;
        }
    }
}
