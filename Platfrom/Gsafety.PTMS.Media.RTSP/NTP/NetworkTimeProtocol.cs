﻿#region Copyright
/*
This file came from Managed Media Aggregation, You can always find the latest version @ https://net7mma.codeplex.com/
  
 Julius.Friedman@gmail.com / (SR. Software Engineer ASTI Transportation Inc. http://www.asti-trans.com)

Permission is hereby granted, free of charge, 
 * to any person obtaining a copy of this software and associated documentation files (the "Software"), 
 * to deal in the Software without restriction, 
 * including without limitation the rights to :
 * use, 
 * copy, 
 * modify, 
 * merge, 
 * publish, 
 * distribute, 
 * sublicense, 
 * and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * 
 * JuliusFriedman@gmail.com should be contacted for further details.

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
 * 
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
 * TORT OR OTHERWISE, 
 * ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 * v//
 */
#endregion

using Gsafety.PTMS.Media.RTSP.Common;
using Gsafety.PTMS.Media.RTSP.Common.Extensions.TimeSpan;
namespace Gsafety.PTMS.Media.RTSP.Ntp
{
    /// <summary>
    /// Contains logic useful for calculating values which correspond to the <see href="http://en.wikipedia.org/wiki/Network_Time_Protocol"> Network Time Protocol </see>
    /// </summary>
    public class NetworkTimeProtocol
    {
        //Should all probably be DateTimeOffset

        /// <summary>
        /// Converts specified DateTime value to short NPT time.
        /// </summary>
        /// <param name="value">DateTime value to convert.</param>
        /// <returns>Returns NPT value.</returns>
        /// <notes>
        /// In some fields where a more compact representation is
        /// appropriate, only the middle 32 bits are used; that is, the low 16
        /// bits of the integer part and the high 16 bits of the fractional part.
        /// The high 16 bits of the integer part must be determined independently.
        /// </notes>
        public static uint DateTimeToNptTimestamp32(ref System.DateTime value) { return (uint)((DateTimeToNptTimestamp(ref value) << Binary.BitsPerShort) & uint.MaxValue); }

        public static uint DateTimeToNptTimestamp32(System.DateTime value) { return DateTimeToNptTimestamp32(ref value); }

        //Error	44	Type 'Media.Ntp.NetworkTimeProtocol' already defines a member called 'DateTimeToNptTimestamp' with the same parameter types
        //public static long DateTimeToNptTimestamp(System.DateTime value) { return (long)DateTimeToNptTimestamp(ref value); }

        public static ulong DateTimeToNptTimestamp(System.DateTime value) { return DateTimeToNptTimestamp(ref value); }

        /// <summary>
        /// Converts specified DateTime value to long NPT time.
        /// </summary>
        /// <param name="value">DateTime value to convert. This value must be in local time.</param>
        /// <returns>Returns NPT value.</returns>
        /// <notes>
        /// Wallclock time (absolute date and time) is represented using the
        /// timestamp format of the Network Time Protocol (NPT), which is in
        /// seconds relative to 0h UTC on 1 January 1900 [4].  The full
        /// resolution NPT timestamp is a 64-bit unsigned fixed-point number with
        /// the integer part in the first 32 bits and the fractional part in the
        /// last 32 bits. In some fields where a more compact representation is
        /// appropriate, only the middle 32 bits are used; that is, the low 16
        /// bits of the integer part and the high 16 bits of the fractional part.
        /// The high 16 bits of the integer part must be determined independently.
        /// </notes>
        public static ulong DateTimeToNptTimestamp(ref System.DateTime value)
        {
            System.DateTime baseDate = value >= UtcEpoch2036 ? UtcEpoch2036 : UtcEpoch1900;

            System.TimeSpan elapsedTime = value > baseDate ? value.ToUniversalTime() - baseDate.ToUniversalTime() : baseDate.ToUniversalTime() - value.ToUniversalTime();

            //(uint)Common.Extensions.DateTime.DateTimeExtensions.Microseconds(value)

            return ((ulong)(elapsedTime.Ticks / System.TimeSpan.TicksPerSecond) << Binary.BitsPerInteger) | (uint)(elapsedTime.Ticks / TimeSpanExtensions.MicrosecondsPerMillisecond);
        }        

        public static System.DateTime NptTimestampToDateTime(ref ulong nptTimestamp) { return NptTimestampToDateTime((uint)((nptTimestamp >> Binary.BitsPerInteger) & uint.MaxValue), (uint)(nptTimestamp & uint.MaxValue)); }

        public static System.DateTime NptTimestampToDateTime(ulong ntpTimestamp) { return NptTimestampToDateTime(ref ntpTimestamp); }

        public static System.DateTime NptTimestampToDateTime(ref uint seconds, ref uint fractions, System.DateTime? epoch = null)
        {
            //Convert to ticks
            ulong ticks = (ulong)((seconds * System.TimeSpan.TicksPerSecond) + ((fractions * System.TimeSpan.TicksPerSecond) / 0x100000000L));

            //System.DateTime.SpecifyKind( , System.DateTimeKind.Utc);
            return epoch.HasValue ? epoch.Value + System.TimeSpan.FromTicks((long)ticks) : 
                    /*seconds > 0 && */(seconds & 0x80000000L) == 0 ? 
                        UtcEpoch2036 + System.TimeSpan.FromTicks((long)ticks) :
                            UtcEpoch1900 + System.TimeSpan.FromTicks((long)ticks);
        }

        public static System.DateTime NptTimestampToDateTime(uint seconds, uint fractions, System.DateTime? epoch = null) { return NptTimestampToDateTime(ref seconds, ref fractions, epoch); }

        public static System.DateTime NptTimestampToDateTime(int seconds, int fractions, System.DateTime? epoch = null)
        {
            uint sec = (uint)seconds, frac = (uint)fractions; 
            
            return NptTimestampToDateTime(ref sec, ref frac, epoch);
        }

        /// <summary>
        /// The seconds difference in seconds between NTP Time and Unix Time.
        /// </summary>
        public const long NtpUnixDifferenceSeconds = 2208988800;

        //When the First Epoch will wrap (The real Y2k)
        public static System.DateTime UtcEpoch2036 = new System.DateTime(2036, 2, 7, 6, 28, 16, System.DateTimeKind.Utc);

        public static System.DateTime UtcEpoch1900 = new System.DateTime(1900, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

        public static System.DateTime UtcEpoch1970 = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
    }
}