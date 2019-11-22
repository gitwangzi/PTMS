using System;
using System.Collections.Generic;

namespace Gsafety.PTMS.Media.Mmreg
{
    // // See MPEGLAYER3WAVEFORMAT in Microsoft's mmreg.h
    class MpegLayer3WaveFormat : WaveFormatEx
    {
        [Flags]
        public enum Flags : uint
        {
            PaddingIso = 0x00000000,
            PaddingOn = 0x00000001,
            PaddingOff = 0x00000002,
        }
        public enum Id : ushort
        {
            Unkown = 0,
            Mpeg = 1,
            ConstantFrameSize = 2
        }

        const int MpegLayer3WfxExtraBytes = 12;

#pragma warning disable 649
        public uint fdwFlags;
#pragma warning restore 649
        public ushort nBlockSize;
#pragma warning disable 649
        public ushort nCodecDelay;
#pragma warning restore 649
        public ushort nFramesPerBlock = 1;
        public ushort wID = (ushort)Id.Mpeg;

        public MpegLayer3WaveFormat()
        {
            wFormatTag = (ushort)WaveFormatTag.MpegLayer3;
        }

        public override ushort cbSize
        {
            get { return (ushort)(base.cbSize + MpegLayer3WfxExtraBytes); }
        }

        public override void ToBytes(IList<byte> buffer)
        {
            base.ToBytes(buffer);

            buffer.AddLe(wID);
            buffer.AddLe(fdwFlags);
            buffer.AddLe(nBlockSize);
            buffer.AddLe(nFramesPerBlock);
            buffer.AddLe(nCodecDelay);
        }
    }
}
