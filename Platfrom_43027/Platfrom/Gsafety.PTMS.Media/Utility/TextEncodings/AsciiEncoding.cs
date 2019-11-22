using System.Text;

namespace Gsafety.PTMS.Media.Utility.TextEncodings
{
    /// <summary>
    ///     A simplified ASCII encoding that makes no attempt
    ///     to normalize, deal with combining characters, surrogates,
    ///     or such.
    /// </summary>
    public class AsciiEncoding : Encoding
    {
        public override int GetByteCount(char[] chars, int index, int count)
        {
            return count;
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            for (var i = 0; i < charCount; ++i)
            {
                var c = chars[i + charIndex];

                var b = (byte)(c <= 127 ? c : '?');

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

                chars[i + charIndex] = b <= 127 ? (char)b : EncodingHelpers.ReplacementCharacter;
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
