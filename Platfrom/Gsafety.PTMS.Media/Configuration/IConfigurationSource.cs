using System;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.Metadata;

namespace Gsafety.PTMS.Media.Configuration
{
    public interface IConfigurationSource
    {
        string CodecPrivateData { get; }
        string Name { get; }
        string StreamDescription { get; }
        int? Bitrate { get; }

        ContentType ContentType { get; }
        IMediaStreamMetadata MediaStreamMetadata { get; }

        bool IsConfigured { get; }
        event EventHandler ConfigurationComplete;
    }

    public static class ConfigurationSourceExtensions
    {
        public static string GetLanguage(this IConfigurationSource configurationSource)
        {
            var mediaStreamMetadata = configurationSource.MediaStreamMetadata;

            if (null == mediaStreamMetadata)
                return null;

            if (string.IsNullOrWhiteSpace(mediaStreamMetadata.Language))
                return null;

            return mediaStreamMetadata.Language;
        }
    }
}
