using System;
using System.Text;

namespace Gsafety.PTMS.Media.Audio.Shoutcast
{
    public class Utf8ShoutcastEncodingSelector : IShoutcastEncodingSelector
    {
        public Encoding GetEncoding(Uri url)
        {
            return Encoding.UTF8;
        }
    }
}
