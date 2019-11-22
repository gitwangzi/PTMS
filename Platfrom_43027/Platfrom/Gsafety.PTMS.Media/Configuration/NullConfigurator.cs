using System;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.Metadata;

namespace Gsafety.PTMS.Media.Configuration
{
    public sealed class NullConfigurator : IConfigurationSource
    {
        public string CodecPrivateData
        {
            get { return null; }
        }

        public string Name
        {
            get { return "Unknown"; }
        }

        public string StreamDescription
        {
            get { return null; }
        }

        public int? Bitrate
        {
            get { return null; }
        }

        public ContentType ContentType
        {
            get { return ContentTypes.Binary; }
        }

        public IMediaStreamMetadata MediaStreamMetadata
        {
            get { return null; }
        }

        public bool IsConfigured
        {
            get { return true; }
        }

#pragma warning disable 67
        // The event is required to implement the interface.
        public event EventHandler ConfigurationComplete;
#pragma warning restore 67
    }
}
