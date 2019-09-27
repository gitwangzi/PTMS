using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Gsafety.PTMS.Media
{
    public interface IMediaStreamConfiguration
    {
        IStreamSource VideoStreamSource { get; }
        IStreamSource AudioStreamSource { get; }

        ICollection<MediaStreamDescription> Descriptions { get; }
        IDictionary<MediaSourceAttributesKeys, string> Attributes { get; }

        TimeSpan? Duration { get; }
    }

    public class MediaStreamConfiguration : IMediaStreamConfiguration
    {
        public IStreamSource VideoStreamSource { get; set; }
        public IStreamSource AudioStreamSource { get; set; }
        public ICollection<MediaStreamDescription> Descriptions { get; set; }
        public IDictionary<MediaSourceAttributesKeys, string> Attributes { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
