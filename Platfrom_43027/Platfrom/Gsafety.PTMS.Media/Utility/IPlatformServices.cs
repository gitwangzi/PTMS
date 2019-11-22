using System;
using System.IO;

namespace Gsafety.PTMS.Media.Utility
{
    public interface IPlatformServices
    {
        // Only thin wrappers around platform services belong here.  Any application/library
        // services belong elsewhere (in some DI/IoC).

        /// <summary>
        ///     Returns a random number between 0.0 and 1.0.  It does not suffer from Random's slowly
        ///     changing default seed.
        /// </summary>
        /// <returns></returns>
        double GetRandomNumber();

        /// <summary>
        ///     Decrypt the given stream with AES-128 CBC and PKCS #7 padding.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        Stream Aes128DecryptionFilter(Stream stream, byte[] key, byte[] iv);

        /// <summary>
        ///     Get cryptographically secure random bytes.
        /// </summary>
        /// <param name="bytes"></param>
        void GetSecureRandom(byte[] bytes);
    }

    public static class PlatformServicesExtensions
    {
        public static void GetSecureRandom(this IPlatformServices platformServices, ulong[] output)
        {
            if (null == platformServices)
                throw new ArgumentNullException("platformServices");
            if (null == output)
                throw new ArgumentNullException("output");

            var bytes = new byte[output.Length * sizeof (ulong)];

            platformServices.GetSecureRandom(bytes);

            Buffer.BlockCopy(bytes, 0, output, 0, bytes.Length);
        }

        public static void GetSecureRandom(this IPlatformServices platformServices, uint[] output)
        {
            if (null == platformServices)
                throw new ArgumentNullException("platformServices");
            if (null == output)
                throw new ArgumentNullException("output");

            var bytes = new byte[output.Length * sizeof (uint)];

            platformServices.GetSecureRandom(bytes);

            Buffer.BlockCopy(bytes, 0, output, 0, bytes.Length);
        }
    }
}
