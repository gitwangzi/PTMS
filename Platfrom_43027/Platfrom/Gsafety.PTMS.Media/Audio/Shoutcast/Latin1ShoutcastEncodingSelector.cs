using System;
using System.Text;
using Gsafety.PTMS.Media.Utility.TextEncodings;

namespace Gsafety.PTMS.Media.Audio.Shoutcast
{
    public class Latin1ShoutcastEncodingSelector : IShoutcastEncodingSelector
    {
        readonly Encoding _latin1;

        public Latin1ShoutcastEncodingSelector(ISmEncodings encodings)
        {
            if (null == encodings)
                throw new ArgumentNullException("encodings");

            _latin1 = encodings.Latin1Encoding;
        }

        public Encoding GetEncoding(Uri url)
        {
            return _latin1;
        }

    }
}
