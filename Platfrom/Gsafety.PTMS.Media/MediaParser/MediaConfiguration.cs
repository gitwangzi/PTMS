using System;
using System.Collections.Generic;

namespace Gsafety.PTMS.Media.MediaParser
{
    public interface IMediaConfiguration
    {
        TimeSpan? Duration { get; }
        IMediaParserMediaStream Audio { get; }
        IMediaParserMediaStream Video { get; }

        List<IMediaParserMediaStream> AlternateStreams { get; }
    }

    public class MediaConfiguration : IMediaConfiguration
    {
        public List<IMediaParserMediaStream> AlternateStreams { get; set; }

        public TimeSpan? Duration { get; set; }

        public IMediaParserMediaStream Audio { get; set; }

        public IMediaParserMediaStream Video { get; set; }
    }
}
