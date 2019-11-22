using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Media.Common.Loggers
{
    public class LoggerInstance
    {
        private object _locker = new object();

        public static ILogging Logger
        {
            get
            {
                return _logger;
            }
            set
            {
                _logger = value;
            }
        }

        private static ILogging _logger = new DebugLogger();

        public static void Debug(string format, params object[] message)
        {
            if (_logger == null)
            {
                return;
            }
            lock (_logger)
            {

                _logger.Debug(string.Format(format, message));
            }
        }

        public static void Debug(string message)
        {
            if (_logger == null)
            {
                return;
            }
            lock (_logger)
            {

                _logger.Debug(message);
            }
        }

        public static void Info(string message)
        {
            if (_logger == null)
            {
                return;
            }
            lock (_logger)
            {

                _logger.Info(message);
            }
        }

        public static void Error(string message)
        {
            if (_logger == null)
            {
                return;
            }
            lock (_logger)
            {

                _logger.Error(message);
            }
        }

        public static void Exception(Exception ex)
        {
            if (_logger == null)
            {
                return;
            }
            lock (_logger)
            {
                _logger.Exception(ex);

            }
        }

        public static void Exception(string ex)
        {
            if (_logger == null)
            {
                return;
            }
            lock (_logger)
            {
                _logger.Exception(ex);

            }
        }
    }
}
