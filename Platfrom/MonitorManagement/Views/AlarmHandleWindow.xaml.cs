using Gsafety.PTMS.Video.Args;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.PTMS.Share;
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
using Gsafety.PTMS.BasicPage.VideoDisplay;
using System.Reflection;

namespace Gsafety.Ant.Monitor.Views
{
    public partial class AlarmHandleWindow : ChildWindow
    {
        private VehicleAlarmServiceClient vehicleAlarmServiceClient = null;
        public string AlarmId { get; set; }
        public DateTime CurrentDate
        {
            get
            {
                return txtDateTime.SelectedDate.Value;
            }
            set
            {
                txtDateTime.SelectedDate = value;
            }
        }
        public bool? IsChecked
        {
            get
            {
                return cbAlarm.IsChecked;
            }
            set
            {
                cbAlarm.IsChecked = value;
            }
        }
        public string VehicleId
        {
            get
            {
                return txtCarNumber.Text;
            }
            set
            {
                txtCarNumber.Text = value;
            }
        }
        public Nullable<DateTime> AlarmTime
        {
            get
            {
                return DateTime.Parse(txtAlarmTime.Text);
            }
            set
            {
                txtAlarmTime.Text = value.ToString();
            }
        }
        public Gsafety.PTMS.ServiceReference.VehicleAlarmService.AlarmInfoEx ValarmInfo { get; set; }
        private bool ifhandle = false;

        public AlarmHandleWindow()
        {
            InitializeComponent();
            try
            {
                vehicleAlarmServiceClient = ServiceClientFactory.Create<VehicleAlarmServiceClient>();
                vehicleAlarmServiceClient.HandleAlarmCompleted += vehicleAlarmServiceClient_HandleAlarmCompleted;
            }
            catch (Exception ex)
            {

            }
            this.txtDisposer.Text = ApplicationContext.Instance.AuthenticationInfo.Account;
            CurrentDate = DateTime.Now;
        }

        void vehicleAlarmServiceClient_HandleAlarmCompleted(object sender, HandleAlarmCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.DialogResult = true;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

