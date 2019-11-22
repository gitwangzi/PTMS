using System.Collections.Generic;
using Gsafety.PTMS.Media.Configuration;

namespace Gsafety.PTMS.Media.Metadata
{
    public interface IMediaStreamMetadata
    {
        string Language { get; }
    }

    public class MediaStreamMetadata : IMediaStreamMetadata
    {
        public string Language { get; set; }
    }
}
