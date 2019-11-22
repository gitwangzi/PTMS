using System;

namespace Gsafety.PTMS.Media.Utility.RandomGenerators
{
    public class Well512 : IRandomGenerator<uint>
    {
        //  http://lomont.org/Math/Papers/2008/Lomont_PRNG_2008.pdf

        const float FloatScale = 1.0f / uint.MaxValue;
        const double DoubleScale = 1.0 / ulong.MaxValue;
        readonly IPlatformServices _platformServices;
        readonly uint[] _state = new uint[16];
        uint _index;

        public Well512(IPlatformServices platformServices)
        {
            if (null == platformServices)
                throw new ArgumentNullException("platformServices");

            _platformServices = platformServices;

            Reseed();
        }

        public void Reseed()
        {
            _index = 0;

            _platformServices.GetSecureRandom(_state);
        }

        public uint Next()
        {
            var a = _state[_index];
            var c = _state[(_index + 13) & 15];

            var b = a ^ c ^ (a << 16) ^ (c << 15);

            c = _state[(_index + 9) & 15];

            c ^= (c >> 11);

            a = _state[_index] = b ^ c;

            var d = a ^ ((a << 5) & 0xDA442D24U);

            _index = (_index + 15) & 15;

            a = _state[_index];

            _state[_index] = a ^ b ^ d ^ (a << 2) ^ (b << 18) ^ (c << 28);

            return _state[_index];
        }

        public void GetBytes(byte[] buffer, int offset, int count)
        {
            for (; ; )
            {
                var v = Next();

                for (var j = 0; j < sizeof(uint); ++j)
                {
                    if (count <= 0)
                        return;

                    buffer[offset++] = (byte)v;
                    --count;
                    v >>= 8;
                }
            }
        }

        public float NextFloat()
        {
            return Next() * FloatScale;
        }

        public double NextDouble()
        {
            return (((ulong)Next() << 32) | Next()) * DoubleScale;
        }
    }
}
