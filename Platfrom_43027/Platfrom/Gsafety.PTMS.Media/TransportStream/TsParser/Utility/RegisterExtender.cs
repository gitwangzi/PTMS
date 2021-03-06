using System;
using System.Diagnostics;

namespace Gsafety.PTMS.Media.TransportStream.TsParser.Utility
{
    /// <summary>
    ///     Extend a slow-changing integer register from n bits (n &lt; 64) to 64 bits.  The
    ///     register must change by less than 2 ^ (n - 1) per iteration.
    /// </summary>
    sealed class RegisterExtender
    {
        readonly int _width;
        ulong _value;

        public RegisterExtender(ulong initialValue, int actualWidth)
        {
            if (actualWidth < 2 || actualWidth > 63)
                throw new ArgumentOutOfRangeException("actualWidth", "actualWidth must be between 2 and 63, inclusive.");

            _value = initialValue;
            _width = actualWidth;
        }

        public ulong Extend(ulong value)
        {
            var wrapThreshold = (1L << (_width - 1));
            var period = (1UL << _width);
            var mask = ~(period - 1);

            Debug.Assert(0 == (value & mask));

            value |= _value & mask;

            // Check to see if we have wrapped (either up or down).
            if (Math.Abs((long)(value - _value)) > wrapThreshold)
            {
                var adjustedPts = value;

                if (adjustedPts > _value)
                    adjustedPts = value - period;
                else
                    adjustedPts = value + period;

                var adjustedPtsError = Math.Abs((long)(adjustedPts - _value));
                var ptsError = Math.Abs((long)(value - _value));

                // Pick the value closest to the old one.
                if (adjustedPtsError <= ptsError)
                    value = adjustedPts;

                Debug.Assert(Math.Abs((long)(value - _value)) <= wrapThreshold);
            }

            return _value = value;
        }
    }
}
