namespace Gsafety.PTMS.Media.Utility
{
    public static class ByteConversion
    {
        const double ToMiB = 1.0 / (1024 * 1024);

        public static double BytesToMiB(this long value)
        {
            return value * ToMiB;
        }

        public static double BytesToMiB(this ulong value)
        {
            return value * ToMiB;
        }

        public static double? BytesToMiB(this long? value)
        {
            return value * ToMiB;
        }

        public static double? BytesToMiB(this ulong? value)
        {
            return value * ToMiB;
        }
    }
}
