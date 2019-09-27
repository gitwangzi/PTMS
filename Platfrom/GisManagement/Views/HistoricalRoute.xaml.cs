/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4d258f0a-1ff8-4272-a1ea-4b6a2e744d36      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZANGDS
/////                 Author: TEST(zangds)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Monitor.Views
/////    Project Description:    
/////             Class Name: HistoricalRoute
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/9 13:53:25
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/9 13:53:25
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Gsafety.Common;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.Share;
using Gsafety.Common.Controls;


namespace GisManagement.Views
{
    public partial class HistoricalRoute : ChildWindow
    {
        public ObservableCollection<PredefinedColor> PreDefinedColors = null;
        public HisTraceArgs HistraceArgs { get; private set; }
        public HistoricalRoute(string VehicleNo, HisGPSDataType GPSDataType, bool IsEnabled_VehicleNo, DateTime StartTimeData, DateTime EndTimeData)
            : this(VehicleNo, IsEnabled_VehicleNo)
        {
            StartTimeData = StartTimeData.ToLocalTime();
            EndTimeData = EndTimeData.ToLocalTime();

            StartTime.SelectedDate = StartTimeData.Date;
            StartShortTime.Value = StartTimeData;
            EndTime.SelectedDate = EndTimeData.Date;
            EndShortTime.Value = EndTimeData;
            OKButton.IsEnabled = (VehicleId.Text != "");
        }
        public HistoricalRoute(string VehicleNo, bool IsEnabled_VehicleNo)
        {
            InitializeComponent();
            Initialize_Colors();
            RouteColor.DataContext = PreDefinedColors;
            DateTime CurrtTime = DateTime.Now;
            StartTime.SelectedDate = CurrtTime.Date;
            StartShortTime.Value = CurrtTime.AddHours(-1);
            EndTime.SelectedDate = CurrtTime.Date;
            EndShortTime.Value = CurrtTime;
            HistraceArgs = new HisTraceArgs();
            //HistraceArgs.GpsDataType = GPSDataType;// HisGPSDataType.MonitorGPS;
            VehicleId.IsEnabled = IsEnabled_VehicleNo;
            //if (IsEnabled_VehicleNo)
            //{
            //    Rb_AlarmGPS.Visibility = System.Windows.Visibility.Visible;
            //    Rb_MonitorGPS.Visibility = System.Windows.Visibility.Visible;
            //}
            VehicleId.Text = VehicleNo;

            OKButton.IsEnabled = (VehicleId.Text != "");
        }
        private void Initialize_Colors()
        {
            if (PreDefinedColors != null)
            {
                return;
            }

            PreDefinedColors = PredefinedColors.PredefinedColorCollection;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt = StartTime.DisplayDate;
            if (DateTime.Now - dt > new TimeSpan(365, 0, 0, 0))
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("History_BadInfo"), MessageDialogButton.Ok);
                return;
            }


            if (!StartTime.SelectedDate.HasValue)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_DownVideoConditionDetail"), MessageDialogButton.Ok);
                return;
            }
            else
            {
                HistraceArgs.StartTime = DateTime.Parse(StartTime.SelectedDate.Value.Date.ToShortDateString() + " " + StartShortTime.Value.Value.ToLongTimeString());
            }


            if (!EndTime.SelectedDate.HasValue)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_DownVideoConditionDetail"), MessageDialogButton.Ok);
                return;
            }
            else
            {
                HistraceArgs.EndTime = DateTime.Parse(EndTime.SelectedDate.Value.Date.ToShortDateString() + " " + EndShortTime.Value.Value.ToLongTimeString());
            }

            if (this.StartTime.SelectedDate == null || this.EndTime.SelectedDate == null || this.RouteColor == null || HistraceArgs.EndTime < HistraceArgs.StartTime)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_DownVideoConditionDetail"), MessageDialogButton.Ok);
                return;
            }

            HistraceArgs.Op = HisTraceOption.Add;
            HistraceArgs.CarNo = VehicleId.Text.Trim();
            HistraceArgs.LineColor = (RouteColor.SelectedValue as PredefinedColor).Value;
            this.DialogResult = true;
        }

        private void VehicleID_Changed(object sender, System.EventArgs e)
        {
            OKButton.IsEnabled = (VehicleId.Text != "");
        }
    }
}

