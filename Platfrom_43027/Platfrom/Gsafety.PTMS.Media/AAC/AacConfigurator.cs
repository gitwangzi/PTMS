using System;
using Gsafety.PTMS.Media.Audio;
using Gsafety.PTMS.Media.Configuration;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.Metadata;
using Gsafety.PTMS.Media.Mmreg;

namespace Gsafety.PTMS.Media.AAC
{
    public sealed class AacConfigurator : ConfiguratorBase, IFrameParser, IAudioConfigurator
    {
        readonly AacFrameHeader _frameHeader = new AacFrameHeader();

        public AacConfigurator(IMediaStreamMetadata mediaStreamMetadata, string streamDescription = null)
            : base(ContentTypes.Aac, mediaStreamMetadata)
        {
            StreamDescription = streamDescription;
        }

        public AudioFormat Format
        {
            get { return AacDecoderSettings.Parameters.UseRawAac ? AudioFormat.AacRaw : AudioFormat.AacAdts; }
        }

        public int SamplingFrequency { get; private set; }

        public int Channels { get; private set; }

        public void Configure(IAudioFrameHeader frameHeader)
        {
            var aacFrameHeader = (AacFrameHeader)frameHeader;

            CodecPrivateData = BuildCodecPrivateData(aacFrameHeader);

            Name = frameHeader.Name;
            Channels = aacFrameHeader.ChannelConfig;
            SamplingFrequency = frameHeader.SamplingFrequency;

            SetConfigured();
        }

        public int FrameLength
        {
            get { return _frameHeader.FrameLength; }
        }

        public bool Parse(byte[] buffer, int index, int length)
        {
            if (!_frameHeader.Parse(buffer, index, length, true))
                return false;

            Configure(_frameHeader);

            return true;
        }

        static string BuildCodecPrivateData(AacFrameHeader aacFrameHeader)
        {
            var factory = AacDecoderSettings.Parameters.CodecPrivateDataFactory;

            if (null != factory)
                return factory(aacFrameHeader);

            WaveFormatEx w;

            var waveFormatEx = AacDecoderSettings.Parameters.ConfigurationFormat;

            switch (waveFormatEx)
            {
                case AacDecoderParameters.WaveFormatEx.RawAac:
                    if (!AacDecoderSettings.Parameters.UseRawAac)
                        throw new NotSupportedException("AacDecoderSettings.Parameters.UseRawAac must be enabled when using AacDecoderParameters.WaveFormatEx.RawAac");
                    w = new RawAacWaveInfo
                    {
                        nChannels = aacFrameHeader.ChannelConfig,
                        nSamplesPerSec = (uint)aacFrameHeader.SamplingFrequency,
                        nAvgBytesPerSec = (uint)(aacFrameHeader.Duration.TotalSeconds <= 0 ? 0 : aacFrameHeader.FrameLength / aacFrameHeader.Duration.TotalSeconds),
                        pbAudioSpecificConfig = aacFrameHeader.AudioSpecificConfig
                    };
                    break;
                case AacDecoderParameters.WaveFormatEx.HeAac:
                    w = new HeAacWaveInfo
                    {
                        wPayloadType = (ushort)(AacDecoderSettings.Parameters.UseRawAac ? HeAacWaveInfo.PayloadType.Raw : HeAacWaveInfo.PayloadType.ADTS),
                        nChannels = aacFrameHeader.ChannelConfig,
                        nSamplesPerSec = (uint)aacFrameHeader.SamplingFrequency,
                        pbAudioSpecificConfig = aacFrameHeader.AudioSpecificConfig
                    };
                    break;
                default:
                    throw new NotSupportedException("Unknown WaveFormatEx type: " + waveFormatEx);
            }

            return w.ToCodecPrivateData();
        }
    }
}
