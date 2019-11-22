namespace Gsafety.PTMS.Media.Web
{
    public static class UserAgentExtensions
    {
        public static void SetParameter(this IMediaStreamFacadeBase mediaStreamFacade, IUserAgent userAgent)
        {
            mediaStreamFacade.Builder.RegisterSingleton(userAgent);
        }
    }
}
