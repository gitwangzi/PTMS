using System;

namespace Gsafety.PTMS.Media.Utility.RandomGenerators
{
    public static class RandomGeneratorExtensions
    {
        public static void GetBytes(this IRandomGenerator randomGenerator, byte[] buffer)
        {
            randomGenerator.GetBytes(buffer, 0, buffer.Length);
        }

        public static int Next(this IRandomGenerator<uint> randomGenerator, int lessThan)
        {
            if (lessThan <= 0)
                return 0;

            var mask = BitTwiddling.PowerOf2Mask((uint)lessThan);

            for (; ; )
            {
                var v = (int)(randomGenerator.Next() & mask);

                if (v < lessThan)
                    return v;
            }
        }

        public static long Next(this IRandomGenerator<ulong> randomGenerator, long lessThan)
        {
            if (lessThan <= 0)
                return 0;

            var mask = BitTwiddling.PowerOf2Mask((ulong)lessThan);

            for (; ; )
            {
                var v = (long)(randomGenerator.Next() & mask);

                if (v < lessThan)
                    return v;
            }
        }

        public static int NextInt(this IRandomGenerator randomGenerator)
        {
            var uintGen = randomGenerator as IRandomGenerator<uint>;

            if (null != uintGen)
                return unchecked((int)uintGen.Next());

            var ulongGen = randomGenerator as IRandomGenerator<ulong>;

            if (null != ulongGen)
                return unchecked((int)ulongGen.Next());

            return unchecked((int)(-randomGenerator.NextDouble() * int.MinValue));
        }

        public static double NextExponential(this IRandomGenerator randomGenerator, double lambda)
        {
            return -Math.Log(randomGenerator.NextDouble()) / lambda;
        }
    }
}
