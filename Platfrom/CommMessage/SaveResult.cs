using System;

namespace Gsafety.Common.CommMessage
{
    public class SaveResultArgs : EventArgs
    {
        public bool Result { get; set; }

        public string Message { get; set; }
    }
}
