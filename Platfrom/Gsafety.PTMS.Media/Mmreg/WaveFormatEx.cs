using System.Collections.Generic;

namespace Gsafety.PTMS.Media.Mmreg
{
    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff538799(v=vs.85).aspx
    // Also see WAVEFORMATEX in Microsoft's mmreg.h
    public class WaveFormatEx
    {
    
        public enum WaveFormatTag : ushort
        {
            RawAac1 = 0x00ff,
            Mpeg = 0x0050,
            MpegLayer3 = 0x0055,
            FraunhoferIisMpeg2Aac = 0x0180,
            AdtsAac = 0x1600,
            RawAac = 0x1601,
            HeAac = 0x1610,
            Mpeg4Aac = 0xa106
        }

        public uint nAvgBytesPerSec;
        public ushort nBlockAlign;
        public ushort nChannels;
        public uint nSamplesPerSec;
        public ushort wBitsPerSample;
        public ushort wFormatTag;

        /// <summary>
        ///     The number of bytes after WAVEFORMATEX.
        /// </summary>
        public virtual ushort cbSize
        {
            get { return 0; }
        }

        public virtual void ToBytes(IList<byte> buffer)
        {
            buffer.AddLe(wFormatTag);
            buffer.AddLe(nChannels);
            buffer.AddLe(nSamplesPerSec);
            buffer.AddLe(nAvgBytesPerSec);
            buffer.AddLe(nBlockAlign);
            buffer.AddLe(wBitsPerSample);
            buffer.AddLe(cbSize);
        }
    }
}
