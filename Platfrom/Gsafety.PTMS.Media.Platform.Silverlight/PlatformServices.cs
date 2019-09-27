using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Gsafety.PTMS.Media.Utility;

namespace Gsafety.PTMS.Media
{
    public class PlatformServices : IPlatformServices
    {
        readonly Stack<Random> _generators = new Stack<Random>();

        public double GetRandomNumber()
        {
            var random = GetRandom();

            var ret = random.NextDouble();

            FreeRandom(random);

            return ret;
        }

        public Stream Aes128DecryptionFilter(Stream stream, byte[] key, byte[] iv)
        {
            // CBC with PCKS #7 padding.  (Default for desktop and only supported values
            // for Silverlight/Phone.)
            var aes = new AesManaged
            {
                Key = key,
                IV = iv
            };

            return new CryptoStream(stream, aes.CreateDecryptor(), CryptoStreamMode.Read);
        }

        public void GetSecureRandom(byte[] bytes)
        {
#if WINDOWS_PHONE || SILVERLIGHT
            var rng = new RNGCryptoServiceProvider();
            {
#else
            using (var rng = RandomNumberGenerator.Create())
            {
#endif
                rng.GetBytes(bytes);
            }
        }

        Random GetRandom()
        {
            lock (_generators)
            {
                if (_generators.Count > 0)
                    return _generators.Pop();
            }

            var seed = new byte[sizeof(int)];

            GetSecureRandom(seed);

            return new Random(BitConverter.ToInt32(seed, 0));
        }

        void FreeRandom(Random random)
        {
            lock (_generators)
            {
                _generators.Push(random);
            }
        }
    }
}
