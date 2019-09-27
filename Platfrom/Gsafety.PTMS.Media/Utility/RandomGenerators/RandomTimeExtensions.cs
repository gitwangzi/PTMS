using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility.RandomGenerators
{
    public static class RandomTimeExtensions
    {
        public static TimeSpan RandomTimeSpan(this IRandomGenerator<ulong> generator, TimeSpan minimum, TimeSpan maximum)
        {
            var range = maximum.Ticks - minimum.Ticks + 1;

            var random = generator.Next(range);

            return minimum + TimeSpan.FromTicks(random);
        }

        public static Task RandomDelay(this IRandomGenerator<ulong> generator, TimeSpan minimum, TimeSpan maximum, CancellationToken cancellationToken)
        {
            //return TaskExt.Delay(generator.RandomTimeSpan(minimum, maximum), cancellationToken);
            return TaskExt.Delay((int)generator.RandomTimeSpan(minimum, maximum).TotalMilliseconds, cancellationToken);
        }
    }
}
