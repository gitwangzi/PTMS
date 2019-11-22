using Gsafety.PTMS.Media.Utility;

namespace Gsafety.PTMS.Media.AAC
{
    public static class AacDecoderSettings
    {
        static readonly ResettableParameters<AacDecoderParameters> AacDecoderParameters = new ResettableParameters<AacDecoderParameters>();

        public static AacDecoderParameters Parameters
        {
            get { return AacDecoderParameters.Parameters; }
            set { AacDecoderParameters.Parameters = value; }
        }
    }
}
