using System;
using System.Collections.Generic;

namespace Gsafety.PTMS.Media.Utility.RandomGenerators
{
    public class NormalDistribution
    {
        // http://en.wikipedia.org/wiki/Marsaglia_polar_method

        readonly float _mean;
        readonly IRandomGenerator _randomGenerator;
        readonly float _standardDeviation;
        float? _value;

        public NormalDistribution(IRandomGenerator randomGenerator, float mean, float standardDeviation)
        {
            if (randomGenerator == null)
                throw new ArgumentNullException("randomGenerator");
            if (standardDeviation <= 0)
                throw new ArgumentOutOfRangeException("standardDeviation");

            _randomGenerator = randomGenerator;
            _mean = mean;
            _standardDeviation = standardDeviation;
        }

        public float Next()
        {
            float result;

            if (_value.HasValue)
            {
                result = _value.Value;
                _value = null;
            }
            else
            {
                float d2;
                float u;
                float v;

                for (; ; )
                {
                    u = 2.0f * _randomGenerator.NextFloat() - 1.0f;
                    v = 2.0f * _randomGenerator.NextFloat() - 1.0f;

                    d2 = u * u + v * v;

                    if (d2 > 0.0f && d2 < 1.0f)
                        break;
                }

                var s = (float)Math.Sqrt(-2.0f * (float)Math.Log(d2) / d2);

                _value = v * s;
                result = u * s;
            }

            return result * _standardDeviation + _mean;
        }
    }

    public static class NormalDistributionExtensions
    {
        public static IEnumerable<float> AsEnumerable(this NormalDistribution normalDistribution)
        {
            for (; ; )
                yield return normalDistribution.Next();
        }
    }
}
