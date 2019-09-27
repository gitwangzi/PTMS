using System;
using System.Linq;
using System.Text;

namespace Gsafety.PTMS.Media.Web
{
    public static class Rfc2047Encoding
    {
        static readonly bool[] NeedsEncodingFlags = new bool[0x7f];

        static Rfc2047Encoding()
        {
            for (var c = (char)0; c < NeedsEncodingFlags.Length; ++c)
            {
                if (char.IsControl(c) || char.IsWhiteSpace(c)
                    || '=' == c || '"' == c)
                    NeedsEncodingFlags[c] = true;
            }
        }

        public static bool NeedsRfc2047Encoding(char value)
        {
            if (value >= NeedsEncodingFlags.Length)
                return true;

            return NeedsEncodingFlags[value];
        }

        public static bool NeedsRfc2047Encoding(string value)
        {
            return value.Cast<char>().Any(NeedsRfc2047Encoding);
        }

        public static string Rfc2047Encode(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (!NeedsRfc2047Encoding(value))
                return value;

            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(value));

            return "=?utf-8?B?" + encoded + "?=";
        }
    }
}
