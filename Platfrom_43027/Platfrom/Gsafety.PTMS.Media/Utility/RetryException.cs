using System;
using System.Collections.Generic;

namespace Gsafety.PTMS.Media.Utility
{
    public class RetryException : AggregateException
    {
        public RetryException(string message, IEnumerable<Exception> exceptions)
            : base(message, exceptions)
        { }
    }
}
