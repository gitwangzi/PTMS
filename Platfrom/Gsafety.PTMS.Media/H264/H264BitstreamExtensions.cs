namespace Gsafety.PTMS.Media.H264
{
    static class H264BitstreamExtensions
    {
        public static uint ReadUe(this H264Bitstream h264Bitstream)
        {
            var zeros = 0;

            for (; ; )
            {
                var b = h264Bitstream.ReadBits(1);

                if (0 != b)
                    break;

                ++zeros;
            }

            if (0 == zeros)
                return 0;

            var u = h264Bitstream.ReadBits(zeros);

            return (1u << zeros) - 1 + u;
        }

        public static int ReadSe(this H264Bitstream h264Bitstream)
        {
            var u = h264Bitstream.ReadUe();

            if (u < 2)
                return (int)u;

            var n = (int)(u >> 1);

            if (0 == (u & 1))
                return -n;

            return n;
        }

        public static int ReadSignedBits(this H264Bitstream h264Bitstream, int count)
        {
            var n = h264Bitstream.ReadBits(count);

            var leadingBits = 32 - count;
            var sn = (int)(n << leadingBits);

            sn >>= leadingBits;

            return sn;
        }

        public static uint ReadFfSum(this H264Bitstream h264Bitstream)
        {
            var sum = 0u;

            for (; ; )
            {
                var b = h264Bitstream.ReadBits(8);

                sum += b;

                if (b != 0xff)
                    return sum;
            }
        }
    }
}
