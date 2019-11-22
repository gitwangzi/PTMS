using System;

namespace Gsafety.PTMS.Media.TransportStream.TsParser.Utility
{
    public interface IBufferPool : IDisposable
    {
        BufferInstance Allocate(int minSize);
        void Free(BufferInstance bufferInstance);
    }
}
