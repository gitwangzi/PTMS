using Gsafety.PTMS.Media.Common.Loggers;
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

namespace Gsafety.PTMS.Share
{
    public class MediaPlayerLogger : ILogging
    {
        public void Debug(string message)
        {
            //System.Diagnostics.Debug.WriteLine(message);
            ApplicationContext.Instance.Logger.LogInforMession("VideoPlayer : ", message);
        }

        public void Error(string message)
        {
            ApplicationContext.Instance.Logger.LogError("VideoPlayer : ", message);
        }

        public void Exception(string ex)
        {
            ApplicationContext.Instance.Logger.LogError("VideoPlayer : ", ex);
        }

        public void Exception(Exception ex)
        {
            Exception(ex.ToString());
        }

        public void Info(string message)
        {
            ApplicationContext.Instance.Logger.LogInforMession("VideoPlayer : ", message);
        }

        public LoggerLevel Level
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
