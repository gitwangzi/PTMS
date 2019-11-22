using System.Text;
using Gsafety.PTMS.Media.Utility.TextEncodings;

namespace Gsafety.PTMS.Media.Web.HttpConnection
{
    public interface IHttpEncoding
    {
        Encoding HeaderDecoding { get; }
        Encoding HeaderEncoding { get; }
    }

    public sealed class HttpEncoding : IHttpEncoding
    {
        readonly Encoding _decoding;
        readonly Encoding _encoding;

        public HttpEncoding(ISmEncodings encodings)
        {
            _encoding = encodings.AsciiEncoding;
            _decoding = encodings.Latin1Encoding;
        }


        public Encoding HeaderDecoding
        {
            get { return _decoding; }
        }

        public Encoding HeaderEncoding
        {
            get { return _encoding; }
        }
    }
}
