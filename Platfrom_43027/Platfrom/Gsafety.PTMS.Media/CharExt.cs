using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Media
{
    public static class CharExt
    {
        // The starting codepoint for Unicode plane 1.  Plane 1 contains 0x010000 ~ 0x01ffff.
        internal const int UNICODE_PLANE01_START = 0x10000;

        internal const char HIGH_SURROGATE_START = '\ud800';
        internal const char HIGH_SURROGATE_END = '\udbff';
        internal const char LOW_SURROGATE_START = '\udc00';
        internal const char LOW_SURROGATE_END = '\udfff';

        [Pure]
        public static bool IsHighSurrogate(char c)
        {
            return ((c >= HIGH_SURROGATE_START) && (c <= HIGH_SURROGATE_END));
        }

        [Pure]
        public static bool IsLowSurrogate(char c)
        {
            return ((c >= LOW_SURROGATE_START) && (c <= LOW_SURROGATE_END));
        }

        public static int ConvertToUtf32(char highSurrogate, char lowSurrogate)
        {
            if (!IsHighSurrogate(highSurrogate))
            {
                throw new ArgumentOutOfRangeException("highSurrogate", "ArgumentOutOfRange_InvalidHighSurrogate");
            }
            if (!IsLowSurrogate(lowSurrogate))
            {
                throw new ArgumentOutOfRangeException("lowSurrogate", "ArgumentOutOfRange_InvalidLowSurrogate");
            }
            Contract.EndContractBlock();
            return (((highSurrogate - HIGH_SURROGATE_START) * 0x400) + (lowSurrogate - LOW_SURROGATE_START) + UNICODE_PLANE01_START);
        }

        //public static int ConvertToUtf32(string s, int index)
        //{
        //    if (s == null)
        //    {
        //        throw new ArgumentNullException("s");
        //    }

        //    if (index < 0 || index >= s.Length)
        //    {
        //        throw new ArgumentOutOfRangeException("index", ("ArgumentOutOfRange_Index"));
        //    }
        //    Contract.EndContractBlock();
        //    // Check if the character at index is a high surrogate.
        //    int temp1 = (int)s[index] - HIGH_SURROGATE_START;
        //    if (temp1 >= 0 && temp1 <= 0x7ff)
        //    {
        //        // Found a surrogate char.
        //        if (temp1 <= 0x3ff)
        //        {
        //            // Found a high surrogate.
        //            if (index < s.Length - 1)
        //            {
        //                int temp2 = (int)s[index + 1] - LOW_SURROGATE_START;
        //                if (temp2 >= 0 && temp2 <= 0x3ff)
        //                {
        //                    // Found a low surrogate.
        //                    return ((temp1 * 0x400) + temp2 + UNICODE_PLANE01_START);
        //                }
        //                else
        //                {
        //                    throw new ArgumentException("Argument_InvalidHighSurrogate", "s");
        //                }
        //            }
        //            else
        //            {
        //                // Found a high surrogate at the end of the string.
        //                throw new ArgumentException("Argument_InvalidHighSurrogate", "s");
        //            }
        //        }
        //        else
        //        {
        //            // Find a low surrogate at the character pointed by index.
        //            throw new ArgumentException("Argument_InvalidLowSurrogate", "s");
        //        }
        //    }
        //    // Not a high-surrogate or low-surrogate. Genereate the UTF32 value for the BMP characters.
        //    return ((int)s[index]);
        //}
    }
}
