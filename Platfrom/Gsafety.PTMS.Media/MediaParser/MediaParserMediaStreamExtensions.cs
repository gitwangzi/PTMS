using System;
using System.Collections.Generic;
using Gsafety.PTMS.Media.Configuration;

namespace Gsafety.PTMS.Media.MediaParser
{
    public static class MediaParserMediaStreamExtensions
    {
        public static IMediaConfiguration CreateMediaConfiguration(this IEnumerable<IMediaParserMediaStream> mediaParserMediaStreams, TimeSpan? duration)
        {
            var configuration = new MediaConfiguration
            {
                Duration = duration
            };

            List<IMediaParserMediaStream> alternateStreams = null;

            foreach (var mediaStream in mediaParserMediaStreams)
            {
                var configurationSource = mediaStream.ConfigurationSource;

                var video = configurationSource as IVideoConfigurationSource;

                if (null != video && null == configuration.Video)
                {
                    configuration.Video = mediaStream;

                    continue;
                }

                var audio = configurationSource as IAudioConfigurationSource;

                if (null != audio && null == configuration.Audio)
                {
                    configuration.Audio = mediaStream;

                    continue;
                }

                if (null == alternateStreams)
                    alternateStreams = new List<IMediaParserMediaStream>();

                alternateStreams.Add(mediaStream);
            }

            configuration.AlternateStreams = alternateStreams;

            return configuration;
        }
    }
}
