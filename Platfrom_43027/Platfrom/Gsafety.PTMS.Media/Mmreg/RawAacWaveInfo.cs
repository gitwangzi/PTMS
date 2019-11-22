using System.Collections.Generic;

namespace Gsafety.PTMS.Media.Mmreg
{
    /// <summary>
    ///     Extend WaveFormatEx by appending an AudioSpecificConfig
    /// </summary>
    // http://wiki.multimedia.cx/index.php?title=MPEG-4_Audio#Audio_Specific_Config
    public class RawAacWaveInfo : WaveFormatEx
    {
        public ICollection<byte> pbAudioSpecificConfig;

        public RawAacWaveInfo()
        {
            wFormatTag = (ushort)WaveFormatTag.RawAac1;

            nBlockAlign = 4;
            wBitsPerSample = 16;
        }

        public override ushort cbSize
        {
            get
            {
                var size = base.cbSize;

                if (null != pbAudioSpecificConfig)
                    size += (ushort)pbAudioSpecificConfig.Count;

                return size;
            }
        }

        public override void ToBytes(IList<byte> buffer)
        {
            base.ToBytes(buffer);

            if (null == pbAudioSpecificConfig || pbAudioSpecificConfig.Count <= 0)
                return;

            foreach (var b in pbAudioSpecificConfig)
                buffer.Add(b);
        }
    }
}
