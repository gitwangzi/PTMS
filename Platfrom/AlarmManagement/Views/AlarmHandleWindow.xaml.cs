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

namespace Gsafety.PTMS.Alarm.Views
{
    public partial class AlarmHandleWindow : ChildWindow
    {
        private VehicleAlarmServiceClient vehicleAlarmServiceClient = null;

  
        public string AlarmId { get; set; }
        public DateTime CurrentDate { get; set; }
        public bool IsChecked { get; set; }
        public string VehicleId { get; set; }
        public string MDVR_CoreID { get; set; }
        public Nullable<DateTime> AlarmTime { get; set; }
        public Gsafety.PTMS.ServiceReference.VehicleAlarmService.AlarmInfo ValarmInfo { get; set; }
        private bool ifhandle = false;
 
        public Action<bool, int> ResultAction;

        public AlarmHandleWindow()
        {
            InitializeComponent();
            try
            {
                vehicleAlarmServiceClient = ServiceClientFactory.Create<VehicleAlarmServiceClient>();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            this.txtDisposer.Text = ApplicationContext.Instance.AuthenticationInfo.UserName;
            CurrentDate = DateTime.Now;
            //vehicleAlarmServiceClient.AddAlarmTreatmentCompleted += vehicleAlarmServiceClient_AddAlarmTreatmentCompleted;
            vehicleAlarmServiceClient.FinishFakeAlarmCompleted += vehicleAlarmServiceClient_FinishAlarmCompleted;
            vehicleAlarmServiceClient.FinishTrueAlarmCompleted += vehicleAlarmServiceClient_FinishAlarmCompleted;
            vehicleAlarmServiceClient.IfAlarmDetailCompleted += vehicleAlarmServiceClient_IfAlarmDetailCompleted;
            this.DataContext = this;
        }
   
        void vehicleAlarmServiceClient_IfAlarmDetailCompleted(object sender, IfAlarmDetailCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            ifhandle = e.Result.Result;
        }

        void vehicleAlarmServiceClient_FinishAlarmCompleted(object sender, FinishFakeAlarmCompletedEventArgs e)
        {
            if (e.Result.Result)
            {
                ResultAction(true,1);
            }
            else
            {
                ResultAction(false,1);
            }
        }

        void vehicleAlarmServiceClient_FinishAlarmCompleted(object sender, FinishTrueAlarmCompletedEventArgs e)
        {
            if (e.Result.Result)
            {
                ResultAction(true, 1);
            }
            else
            {
                ResultAction(false, 1);
            }
        }
  
        public void QueryAction(string alarmId, string vehicleId, string mdvr_CoreID, Nullable<DateTime> alarmTime, Gsafety.PTMS.ServiceReference.VehicleAlarmService.AlarmInfo vehiclealarmInfo)
        {
            this.AlarmId = alarmId;
            this.VehicleId = vehicleId;
            this.AlarmTime = alarmTime;
            this.MDVR_CoreID = mdvr_CoreID;
            this.ValarmInfo = vehiclealarmInfo;
            vehicleAlarmServiceClient.IfAlarmDetailAsync(alarmId);
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ifhandle)
            {
                ApplicationContext.Instance.MessageManager.SendCompleteAlarmMessage(new Gsafety.PTMS.ServiceReference.MessageService.CompleteAlarm() { MdvrCoreId = MDVR_CoreID, CompleteTime = DateTime.Now, AlarmTime = AlarmTime.Value });
                ResultAction(true, 1);
            }
            else
            {
                if (!this.IsChecked)
                {
                    vehicleAlarmServiceClient.FinishFakeAlarmAsync(AlarmId, DateTime.Now, this.txtNote.Text.Trim(), ApplicationContext.Instance.AuthenticationInfo.UserName);
                    ApplicationContext.Instance.MessageManager.SendCompleteAlarmMessage(new Gsafety.PTMS.ServiceReference.MessageService.CompleteAlarm() { MdvrCoreId = MDVR_CoreID, CompleteTime = DateTime.Now, AlarmTime = AlarmTime.Value });
                }
                else
                {
                    if (ApplicationContext.Instance.MessageManager.ForwardingAlarmMessageToARADS(ValarmInfo))
                    vehicleAlarmServiceClient.FinishTrueAlarmAsync(AlarmId, DateTime.Now, this.txtNote.Text.Trim(), ApplicationContext.Instance.AuthenticationInfo.UserName, true);
                    else
                    vehicleAlarmServiceClient.FinishTrueAlarmAsync(AlarmId, DateTime.Now, this.txtNote.Text.Trim(), ApplicationContext.Instance.AuthenticationInfo.UserName, false);
                }
            }
            this.DialogResult = true;
        }
        
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        
        private void UnHandedAlarmVedio1Command(object sender, RoutedEventArgs e)
        {
            AlarmVideoArgs args = new AlarmVideoArgs();
            args.ChannelId = 0;
            args.IsAutoPlay = true;
            args.MdvrId = this.MDVR_CoreID;
            args.CarNo = this.VehicleId;
            args.Ifdivmenu = true;
            Util.OpenVideoPage(args, 400, 300,new bool[]{false});
        }
        
        private void UnHandedAlarmVedio2Command(object sender, RoutedEventArgs e)
        {
            AlarmVideoArgs args = new AlarmVideoArgs();
            args.ChannelId = 1;
            args.IsAutoPlay = true;
            args.MdvrId = this.MDVR_CoreID;
            args.CarNo = this.VehicleId;
            args.Ifdivmenu = true;
            Util.OpenVideoPage(args, 400, 300,new bool[]{false});
        }

        private void UnHandedAlarmVedio3Command(object sender, RoutedEventArgs e)
        {
            AlarmVideoArgs args = new AlarmVideoArgs();
            args.ChannelId = 2;
            args.IsAutoPlay = true;
            args.MdvrId = this.MDVR_CoreID;
            args.CarNo = this.VehicleId;
            args.Ifdivmenu = true;
            Util.OpenVideoPage(args, 400, 300, new bool[] { false });
        }

        private void UnHandedAlarmVedio4Command(object sender, RoutedEventArgs e) 
        {
            AlarmVideoArgs args = new AlarmVideoArgs();
            args.ChannelId = 3;
            args.IsAutoPlay = true;
            args.MdvrId = this.MDVR_CoreID;
            args.CarNo = this.VehicleId;
            args.Ifdivmenu = true;
            Util.OpenVideoPage(args, 400, 300, new bool[] { false });
        }
    }
}

