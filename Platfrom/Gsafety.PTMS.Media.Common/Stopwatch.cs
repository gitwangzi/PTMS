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

namespace Gsafety.PTMS.Media.Common
{
    public class Stopwatch
    {
        private DateTime _starttime;
        private DateTime _endtime;

        public TimeSpan TotalTime
        {
            get
            {
                return (_endtime - _starttime);
            }
        }

        public Stopwatch()
        {

        }

        public void Start()
        {
            _starttime = DateTime.Now;
        }

        public void Stop()
        {
            _endtime = DateTime.Now;
        }
    }
}
