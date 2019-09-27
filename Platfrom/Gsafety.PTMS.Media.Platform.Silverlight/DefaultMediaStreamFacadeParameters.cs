using System;

namespace Gsafety.PTMS.Media
{
    public static class DefaultMediaStreamFacadeParameters
    {
        public static Func<IMediaStreamFacadeBase> Factory =
            () =>
            {
                var mediaStreamFacade = new MediaStreamFacade();

                return mediaStreamFacade;
            };

        public static IMediaStreamFacade Create(this MediaStreamFacadeParameters parameters)
        {
            var factory = parameters.Factory ?? Factory;

            return (IMediaStreamFacade)factory();
        }
    }
}
