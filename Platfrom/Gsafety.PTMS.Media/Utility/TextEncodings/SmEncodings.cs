using Gsafety.PTMS.Media.Common.Loggers;
using System;
using System.Diagnostics;
using System.Text;

namespace Gsafety.PTMS.Media.Utility.TextEncodings
{
    public interface ISmEncodings
    {
        Encoding AsciiEncoding { get; }
        Encoding Latin1Encoding { get; }
    }

    public class SmEncodings : ISmEncodings
    {
        internal static readonly Encoding Latin1;
        internal static readonly Encoding Ascii;

        static SmEncodings()
        {
            Ascii = GetAsciiEncoding();
            Latin1 = GetLatin1Encoding();
        }

        public Encoding Latin1Encoding
        {
            get { return Latin1; }
        }

        public Encoding AsciiEncoding
        {
            get { return Ascii; }
        }

        static Encoding GetLatin1Encoding()
        {
            var decoding = GetEncoding("Windows-1252");

            if (null != decoding)
                return decoding;

            decoding = GetEncoding("iso-8859-1");
            if (null != decoding)
                return decoding;

            return new Windows1252Encoding();
        }

        static Encoding GetAsciiEncoding()
        {
            var encoding = GetEncoding("us-ascii");

            if (null != encoding)
                return encoding;

            return new AsciiEncoding();
        }

        static Encoding GetEncoding(string name)
        {
            try
            {
                return Encoding.GetEncoding(name);
            }
            catch (Exception)
            {
                LoggerInstance.Debug("Unable to get " + name + " encoding");
            }

            return null;
        }
    }
}
