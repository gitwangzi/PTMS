using System;
using Gsafety.PTMS.Media.Content;

namespace Gsafety.PTMS.Media.Metadata
{
    public interface IStreamMetadata
    {
        Uri Url { get; }
        ContentType ContentType { get; }

        int? Bitrate { get; }
        TimeSpan? Duration { get; }

        string Name { get; }
        string Description { get; }
        string Genre { get; }

        Uri Website { get; }
    }

    public class StreamMetadata : IStreamMetadata
    {
        public Uri Url { get; set; }
        public ContentType ContentType { get; set; }

        public int? Bitrate { get; set; }
        public TimeSpan? Duration { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }

        public Uri Website { get; set; }

        public override string ToString()
        {
            var name = string.IsNullOrWhiteSpace(Name) ? "{null}" : '"' + Name + '"';
            var url = null == Url ? "null" : Url.ToString();
            var type = null == ContentType ? "<unknown>" : ContentType.Name;

            return "Stream " + name + " <" + url + "> " + type;
        }
    }
}
