using Gsafety.PTMS.Media.Utility;

namespace Gsafety.PTMS.Media
{
    public static class MediaStreamFacadeSettings
    {
        static readonly ResettableParameters<MediaStreamFacadeParameters> MediaStreamFacadeParameters = new ResettableParameters<MediaStreamFacadeParameters>();

        public static MediaStreamFacadeParameters Parameters
        {
            get { return MediaStreamFacadeParameters.Parameters; }
            set { MediaStreamFacadeParameters.Parameters = value; }
        }
    }
}
