using System;

namespace Gsafety.PTMS.Media.Utility.RandomGenerators
{
    public class SystemRandomGenerator : IRandomGenerator
    {
        readonly IPlatformServices _platformServices;
        Random _random;

        public SystemRandomGenerator(IPlatformServices platformServices)
        {
            if (null == platformServices)
                throw new ArgumentNullException("platformServices");

            _platformServices = platformServices;

            Reseed();
        }

        public void GetBytes(byte[] buffer, int offset, int count)
        {
            if (null == buffer)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || offset >= buffer.Length)
                throw new ArgumentOutOfRangeException("offset");
            if (count < 1 || count + offset > buffer.Length)
                throw new ArgumentOutOfRangeException("count");

            if (0 == offset && buffer.Length == offset)
            {
                _random.NextBytes(buffer);

                return;
            }

            var bytes = new byte[count];

            _random.NextBytes(bytes);

            Array.Copy(bytes, 0, buffer, offset, count);
        }

        public float NextFloat()
        {
            return (float)_random.NextDouble();
        }

        public double NextDouble()
        {
            return _random.NextDouble();
        }

        public void Reseed()
        {
            var seed = new byte[sizeof(int)];

            _platformServices.GetSecureRandom(seed);

            _random = new Random(BitConverter.ToInt32(seed, 0));
        }
    }
}
