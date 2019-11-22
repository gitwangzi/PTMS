using System;

namespace Gsafety.PTMS.Media.Common.Loggers
{
    public class DebugLogger : ILogging
    {
        public DebugLogger()
        {
            Level = LoggerLevel.ALL;
        }

        public void Debug(string message)
        {
            if (Level > LoggerLevel.DEBUG)
            {
                return;
            }
            message = message ?? string.Empty;
            System.Diagnostics.Debug.WriteLine("Debug : " + message);
        }

        public void Info(string message)
        {
            if (Level > LoggerLevel.INFO)
            {
                return;
            }

            message = message ?? string.Empty;
            System.Diagnostics.Debug.WriteLine("Info : " + message);
        }

        public void Error(string message)
        {
            if (Level > LoggerLevel.ERROR)
            {
                return;
            }

            message = message ?? string.Empty;
            System.Diagnostics.Debug.WriteLine("Error : " + message);
        }

        public void Exception(Exception ex)
        {
            Exception(ex.ToString());
        }

        public LoggerLevel Level { get; set; }

        #region ILogging 成员


        public void Exception(string ex)
        {
            if (Level > LoggerLevel.EXCEPTION)
            {
                return;
            }

            if (ex == null)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine("Exception : " + ex);
        }

        #endregion
    }
}
