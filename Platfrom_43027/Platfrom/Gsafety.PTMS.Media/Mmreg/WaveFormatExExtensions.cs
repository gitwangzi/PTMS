using System.Collections.Generic;
using System.Text;

namespace Gsafety.PTMS.Media.Mmreg
{
    public static class WaveFormatExExtensions
    {
        /// <summary>
        ///   Add "value" in little endian order.
        /// </summary>
        /// <param name="buffer"> </param>
        /// <param name="value"> </param>
        public static void AddLe(this IList<byte> buffer, ushort value)
        {
            buffer.Add((byte)(value & 0xff));
            buffer.Add((byte)(value >> 8));
        }

        /// <summary>
        ///   Add "value" in little endian order.
        /// </summary>
        /// <param name="buffer"> </param>
        /// <param name="value"> </param>
        public static void AddLe(this IList<byte> buffer, uint value)
        {
            AddLe(buffer, (ushort)value);
            AddLe(buffer, (ushort)(value >> 16));
        }

        public static string ToCodecPrivateData(this WaveFormatEx waveFormatEx)
        {
            var b = new List<byte>(18 + waveFormatEx.cbSize);

            waveFormatEx.ToBytes(b);

            var sb = new StringBuilder(b.Count * 2);

            foreach (var x in b)
                sb.Append(x.ToString("X2"));

            return sb.ToString();
        }
    }
}
