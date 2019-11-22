using System;
using System.Threading;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.Metadata;

namespace Gsafety.PTMS.Media.Configuration
{
    public class ConfiguratorBase : IConfigurationSource
    {
        readonly ContentType _contentType;
        readonly IMediaStreamMetadata _mediaStreamMetadata;
        int _isConfigured;

        protected ConfiguratorBase(ContentType contentType, IMediaStreamMetadata mediaStreamMetadata)
        {
            if (null == contentType)
                throw new ArgumentNullException("contentType");

            _contentType = contentType;
            _mediaStreamMetadata = mediaStreamMetadata;
        }

        public virtual string CodecPrivateData { get; protected set; }

        public string Name { get; protected set; }
        public string StreamDescription { get; protected set; }

        public int? Bitrate { get; protected set; }

        public bool IsConfigured
        {
            get { return 0 != _isConfigured; }
        }

        public ContentType ContentType
        {
            get { return _contentType; }
        }

        public IMediaStreamMetadata MediaStreamMetadata
        {
            get { return _mediaStreamMetadata; }
        }

        public event EventHandler ConfigurationComplete;

        protected void SetConfigured()
        {
            // Does ARM need memory barriers ("interlocked" is what's available in a PCL)?
            Interlocked.Exchange(ref _isConfigured, 1);

            var configurationComplete = ConfigurationComplete;

            if (null == configurationComplete)
                return;

            ConfigurationComplete = null;

            configurationComplete(this, EventArgs.Empty);
        }
    }
}
