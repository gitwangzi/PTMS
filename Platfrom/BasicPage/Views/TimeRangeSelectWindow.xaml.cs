using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;

namespace Gsafety.PTMS.BasicPage.Views
{
    public partial class TimeRangeSelectWindow : ChildWindow
    {
        public DateTime StartDateTime
        {
            get
            {
                return sliver.StartDateTime;
            }
        }
        public DateTime EndDateTime
        {
            get
            {
                return sliver.EndDateTime;
            }
        }

        public TimeRangeSelectWindow(DateTime minTime, DateTime maxTime)
        {
            InitializeComponent();

            sliver.SetBetweenSilverPropertys(Gsafety.Common.Controls.SilverSlecetCycleEnum.Date_Time, 1, maxTime, minTime);

            var period = TimeSpan.FromSeconds((maxTime - minTime).TotalSeconds / 4);

            sliver.StartDateTime = minTime.Add(period);
            sliver.EndDateTime = maxTime.Add(-period);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (EndDateTime <= StartDateTime)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("EndTimeBigThanStartTime"));
                return;
            }

            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

