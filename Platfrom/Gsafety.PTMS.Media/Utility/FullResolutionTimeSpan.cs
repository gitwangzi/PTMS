using System;

namespace Gsafety.PTMS.Media.Utility
{
    public static class FullResolutionTimeSpan
    {
        /// <summary>
        ///     Like TimeSpan.FromSeconds(), but rounds to the nearest 100ns tick instead of the nearest millisecond.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static TimeSpan FromSeconds(double seconds)
        {
            return new TimeSpan((long)Math.Round(TimeSpan.TicksPerSecond * seconds));
        }
    }
}
