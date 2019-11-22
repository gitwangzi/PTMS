using System;

namespace Gsafety.PTMS.Media.Utility.RandomGenerators
{
    public class XorShift1024Star : IRandomGenerator<ulong>, IRandomGenerator<uint>
    {
        // http://arxiv.org/abs/1402.6246
        const float FloatScale = (1.0f / uint.MaxValue);
        const double DoubleScale = (1.0 / ulong.MaxValue);
        readonly IPlatformServices _platformServices;
        readonly ulong[] _s = new ulong[16];
        int _p;
        uint? _stored;

        public XorShift1024Star(IPlatformServices platformServices)
        {
            if (null == platformServices)
                throw new ArgumentNullException("platformServices");

            _platformServices = platformServices;

            Reseed();
        }

        uint IRandomGenerator<uint>.Next()
        {
            if (_stored.HasValue)
            {
                var stored = _stored.Value;

                _stored = null;

                return stored;
            }

            var n = Next();

            _stored = (uint)(n >> 32);

            return (uint)n;
        }

        public void Reseed()
        {
            _p = 0;

            _platformServices.GetSecureRandom(_s);
        }

        public ulong Next()
        {
            var s0 = _s[_p];
            var s1 = _s[_p = (_p + 1) & 15];

            s1 ^= s1 << 31; // a
            s1 ^= s1 >> 11; // b
            s0 ^= s0 >> 30; // c

            return (_s[_p] = s0 ^ s1) * 1181783497276652981L;
        }

        public void GetBytes(byte[] buffer, int offset, int count)
        {
            for (; ; )
            {
                var v = Next();

                for (var j = 0; j < sizeof(ulong); ++j)
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
            return ((IRandomGenerator<uint>)this).Next() * FloatScale;
        }

        public double NextDouble()
        {
            return Next() * DoubleScale;
        }
    }
}
