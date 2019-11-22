using System.Diagnostics;
using System.Threading;
using Gsafety.PTMS.Media.TransportStream.TsParser.Utility;

namespace Gsafety.PTMS.Media.Utility
{
    sealed class PoolBufferInstance : BufferInstance
    {
#if BUFFER_POOL_STATS
        static int _bufferEntryCount;
        readonly int _bufferEntryId = Interlocked.Increment(ref _bufferEntryCount);
#endif
        int _allocationCount;

        public PoolBufferInstance(int size)
            : base(new byte[size])
        { }

        public override void Reference()
        {
            Debug.Assert(_allocationCount >= 0);

            Interlocked.Increment(ref _allocationCount);
        }

        public override bool Dereference()
        {
            Debug.Assert(_allocationCount > 0);

            return 0 == Interlocked.Decrement(ref _allocationCount);
        }

        public override string ToString()
        {
#if BUFFER_POOL_STATS
            return string.Format("Buffer({0}) {1} bytes {2} refs", _bufferEntryId, Buffer.Length, _allocationCount);
#else
            return string.Format("Buffer {0} bytes {1} refs", Buffer.Length, _allocationCount);
#endif
        }
    }
}
