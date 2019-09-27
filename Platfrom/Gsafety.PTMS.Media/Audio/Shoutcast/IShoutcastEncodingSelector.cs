using System;
using System.Text;

namespace Gsafety.PTMS.Media.Audio.Shoutcast
{
    public interface IShoutcastEncodingSelector
    {
        Encoding GetEncoding(Uri url);
    }
}
