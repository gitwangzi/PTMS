using System.Diagnostics;

namespace Gsafety.PTMS.Media.AAC
{
    public static class AacAudioSpecificConfig
    {
        /// <summary>
        ///     AAC Main isn't really used anymore.  Sometimes the encoder may
        ///     say "1" when it really should say "2" (AAC LC) or "5" (HE AAC).
        ///     See http://en.wikipedia.org/wiki/MPEG-4_Part_3#MPEG-4_Audio_Object_Types
        /// </summary>
        public static int? RemapObjectType1 { get; set; }

        public static byte[] DefaultAudioSpecificConfigFactory(AacFrameHeader aacFrameHeader)
        {
            var objectType = aacFrameHeader.Profile + 1;

            if (1 == objectType && RemapObjectType1.HasValue)
            {
                objectType = RemapObjectType1.Value;
            }

            return new[]
                   {
                       (byte) ((objectType << 3) | ((aacFrameHeader.FrequencyIndex >> 1) & 0x07)),
                       (byte) ((aacFrameHeader.FrequencyIndex << 7) | (aacFrameHeader.ChannelConfig << 3))
                   };
        }
    }
}
