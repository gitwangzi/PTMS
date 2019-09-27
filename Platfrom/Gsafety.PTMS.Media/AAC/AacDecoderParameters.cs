using System;
using System.Collections.Generic;

namespace Gsafety.PTMS.Media.AAC
{
    public class AacDecoderParameters
    {
        public enum WaveFormatEx
        {
            RawAac,
            HeAac
        };

        Func<AacFrameHeader, ICollection<byte>> _audioSpecificConfigFactory;

        bool _useParser = true;

        public AacDecoderParameters()
        {
            //ConfigurationFormat = WaveFormatEx.HeAac;
            ConfigurationFormat = WaveFormatEx.RawAac;
            UseRawAac = true;
        }

        public bool UseParser
        {
            get { return _useParser || UseRawAac; }
            set { _useParser = value; }
        }

        public bool UseRawAac { get; set; }

        public WaveFormatEx ConfigurationFormat { get; set; }

        public Func<AacFrameHeader, ICollection<byte>> AudioSpecificConfigFactory
        {
            get
            {
                if (null == _audioSpecificConfigFactory)
                    return AacAudioSpecificConfig.DefaultAudioSpecificConfigFactory;

                return _audioSpecificConfigFactory;
            }
            set
            {
                _audioSpecificConfigFactory = value;
            }
        }

        /// <summary>
        ///     Optional CodecPrivateData factory.  If set, AudioSpecificConfigFactory will
        ///     be ignored.
        /// </summary>
        public Func<AacFrameHeader, string> CodecPrivateDataFactory { get; set; }
    }
}
