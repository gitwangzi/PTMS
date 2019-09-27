using System.Collections.Generic;

namespace Gsafety.PTMS.Media.Mmreg
{
    // See the note in http://msdn.microsoft.com/en-us/library/ff426928(v=VS.96).aspx
    // We should use RawAac1 (0xff) instead of ADTS (0x1610).
    public class HeAacWaveInfo : WaveFormatEx
    {
        public enum PayloadType : ushort
        {
            Raw = 0,
            ADTS = 1,
            ADIF = 2,
            LOAS = 3
        }

        public override ushort cbSize
        {
            get
            {
                var size = base.cbSize + 12;

                if (null != pbAudioSpecificConfig)
                    size += pbAudioSpecificConfig.Count;

                return (ushort)size;
            }
        }

        public uint dwReserved2;
        public ushort wAudioProfileLevelIndication = 0xFE;
        public ushort wPayloadType = (ushort)PayloadType.ADTS;
        public ushort wReserved1;
        public ushort wStructType;
        public ICollection<byte> pbAudioSpecificConfig;

        public HeAacWaveInfo()
        {
            wFormatTag = (ushort)WaveFormatTag.HeAac;

            wBitsPerSample = 16;
            nBlockAlign = 1;
        }

        public override void ToBytes(IList<byte> buffer)
        {
            base.ToBytes(buffer);

            buffer.AddLe(wPayloadType);
            buffer.AddLe(wAudioProfileLevelIndication);
            buffer.AddLe(wStructType);
            buffer.AddLe(wReserved1);
            buffer.AddLe(dwReserved2);

            if (null == pbAudioSpecificConfig || pbAudioSpecificConfig.Count <= 0)
                return;

            foreach (var b in pbAudioSpecificConfig)
                buffer.Add(b);
        }
    }
}
