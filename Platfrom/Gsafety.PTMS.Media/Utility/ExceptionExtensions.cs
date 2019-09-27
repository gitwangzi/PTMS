using System;
using System.Text;

namespace Gsafety.PTMS.Media.Utility
{
    public static class ExceptionExtensions
    {
        public static string ExtendedMessage(this Exception ex)
        {
            if (null == ex)
                throw new ArgumentNullException("ex");

            if (null == ex.InnerException)
            {
                var aggregateException = ex as AggregateException;

                if (null == aggregateException)
                    return string.IsNullOrEmpty(ex.Message) ? ex.GetType().FullName : ex.Message;
            }

            var sb = new StringBuilder();

            DumpException(sb, 0, ex);

            return sb.ToString();
        }

        static void DumpException(StringBuilder sb, int indent, AggregateException aggregateException)
        {
            AppendWithIndent(sb, indent, aggregateException);

            foreach (var ex in aggregateException.InnerExceptions)
                DumpException(sb, indent + 1, ex);
        }

        static void DumpException(StringBuilder sb, int indent, Exception exception)
        {
            var aggregateException = exception as AggregateException;

            if (null != aggregateException)
                DumpException(sb, indent, aggregateException);
            else
                AppendWithIndent(sb, indent, exception);
        }

        static void AppendWithIndent(StringBuilder sb, int indent, Exception exception)
        {
            sb.Append(' ', indent * 3);
            sb.AppendLine(exception.GetType().FullName + ": " + exception.Message);

            if (null != exception.InnerException)
                DumpException(sb, indent + 1, exception.InnerException);
        }
    }
}
