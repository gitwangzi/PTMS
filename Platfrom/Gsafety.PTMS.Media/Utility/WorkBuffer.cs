using System;
using System.Threading;
using Gsafety.PTMS.Media.Metadata;

namespace Gsafety.PTMS.Media.Utility
{
    public class WorkBuffer
    {
        const int DefaultBufferSize = 174 * 188; // Almost 32768 and saves some cycles having to rebuffer partial packets

        public readonly byte[] Buffer;
        public int Length;
        public ISegmentMetadata Metadata;

#if DEBUG
        static int _sequenceCounter;

        public readonly int Sequence = Interlocked.Increment(ref _sequenceCounter);
        public int ReadCount;
#endif

        public WorkBuffer()
            : this(DefaultBufferSize)
        { }

        public WorkBuffer(int bufferSize)
        {
            if (bufferSize < 1)
                throw new ArgumentException("The buffer size must be positive", "bufferSize");

            Buffer = new byte[bufferSize];
        }

        public override string ToString()
        {
#if DEBUG
            return string.Format("WorkBuffer({0}) count {1} length {2}/{3}", Sequence, ReadCount, Length, Buffer.Length);
#else
            return string.Format("WorkBuffer length {0}/{1}", Length, Buffer.Length);
#endif
        }
    }
}
