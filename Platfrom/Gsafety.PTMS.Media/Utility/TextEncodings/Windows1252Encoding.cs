using System.Collections.Generic;
using System.Text;

namespace Gsafety.PTMS.Media.Utility.TextEncodings
{
    /// <summary>
    ///     A simplified CP1252 encoding that makes no attempt
    ///     to normalize, deal with combining characters, surrogates,
    ///     or such.
    /// </summary>
    public class Windows1252Encoding : Encoding
    {
        static readonly char[] CharLookup =
        {
            '\u20ac', '\ufffd', '\u201a', '\u0192',
            '\u201e', '\u2026', '\u2020', '\u2021',
            '\u02c6', '\u2030', '\u0160', '\u2039',
            '\u0152', '\ufffd', '\u017d', '\ufffd',
            '\ufffd', '\u2018', '\u2019', '\u201c',
            '\u201d', '\u2022', '\u2013', '\u2014',
            '\u02dc', '\u2122', '\u0161', '\u203a',
            '\u0153', '\ufffd', '\u017e', '\u0178'
        };

        static readonly Dictionary<char, byte> ByteLookup;
        static readonly int ByteLookupMax;

        static Windows1252Encoding()
        {
            ByteLookup = new Dictionary<char, byte>(CharLookup.Length);

            for (var i = 0; i < CharLookup.Length; ++i)
            {
                var c = CharLookup[i];

                if (EncodingHelpers.ReplacementCharacter == c)
                    continue;

                if (c > ByteLookupMax)
                    ByteLookupMax = c;

                ByteLookup[c] = (byte)(i + 0x80);
            }
        }

        public override int GetByteCount(char[] chars, int index, int count)
        {
            return count;
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            for (var i = 0; i < charCount; ++i)
            {
                var c = chars[i + charIndex];

                byte b;

                if (c < 0x80)
                    b = (byte)c;
                else if (c < 0xa0)
                {
                    if (!ByteLookup.TryGetValue(c, out b))
                        b = (byte)'?';
                }
                else if (c < 0x100)
                    b = (byte)c;
                else
                {
                    b = (byte)'?';
                }

                bytes[i + byteIndex] = b;
            }

            return charCount;
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return count;
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            for (var i = 0; i < byteCount; ++i)
            {
                var b = bytes[i + byteIndex];

                char c;

                if (b <= 0x80 || b >= 0xa0)
                    c = (char)b;
                else
                    c = CharLookup[b - 0x80];

                chars[i + charIndex] = c;
            }

            return byteCount;
        }

        public override int GetMaxByteCount(int charCount)
        {
            return charCount;
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount;
        }
    }
}
