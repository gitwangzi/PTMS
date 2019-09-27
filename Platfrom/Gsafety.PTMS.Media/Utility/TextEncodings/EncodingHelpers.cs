using System.Collections.Generic;

namespace Gsafety.PTMS.Media.Utility.TextEncodings
{
    public static class EncodingHelpers
    {
        public const char ReplacementCharacter = '\ufffd';

        public static bool HasSurrogate(char[] chars, int charIndex, int charCount)
        {
            for (var i = charIndex; i < charCount + charIndex; ++i)
            {
                if (char.IsSurrogate(chars[i]))
                    return true;
            }

            return false;
        }

        public static IEnumerable<int> CodePoints(char[] chars, int charIndex, int charCount)
        {
            char? highSurrogate = null;

            for (var i = charIndex; i < charCount + charIndex; ++i)
            {
                var c = chars[i];

                if (highSurrogate.HasValue)
                {
                    if (char.IsSurrogatePair(highSurrogate.Value, c))
                        //yield return char.ConvertToUtf32(highSurrogate.Value, c);
                        yield return CharExt.ConvertToUtf32(highSurrogate.Value, c);
                    else
                        yield return '?';

                    highSurrogate = null;
                }
                else
                {
                    if (char.IsSurrogate(c))
                        highSurrogate = c;
                    else
                        yield return c;
                }
            }
        }
    }
}
