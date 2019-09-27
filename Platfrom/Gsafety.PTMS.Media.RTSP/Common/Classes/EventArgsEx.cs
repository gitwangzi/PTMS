using System;
using System.Collections.Generic;

namespace Gsafety.PTMS.Media.RTSP.Common
{
    //Only used by ITransactionResult which should be removed eventually.
    public delegate void EventHandlerEx<in T>(object sender, T t);
}
