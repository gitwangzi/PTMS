using System;

namespace Gsafety.PTMS.Media.Audio
{
    public interface IAudioParser : IDisposable
    {
        TimeSpan StartPosition { get; set; }
        TimeSpan? Position { get; set; }
        void FlushBuffers();
        void ProcessData(byte[] buffer, int offset, int length);
    }
}
