using System;

namespace Gsafety.PTMS.Media.MediaManager
{
    public class MediaManagerStateEventArgs : EventArgs
    {
        public readonly string Message;
        public readonly MediaManagerState State;

        public MediaManagerStateEventArgs(MediaManagerState state, string message = null)
        {
            State = state;
            Message = message;
        }
    }
}
