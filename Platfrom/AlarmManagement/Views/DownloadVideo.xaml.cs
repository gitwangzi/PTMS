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
using Gsafety.PTMS.Share;
using Gsafety.Common.CommMessage;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Gsafety.PTMS.Alarm.Views
{
    public partial class DownloadVideo : ChildWindow
    {
        private VideoDownLoadArgs m_VideoDownLoadArgs;
        private string _vehicleId;
        private string _mdvrId;
         
        public VideoDownLoadArgs videoDownLoadArgs
        {
            get { return m_VideoDownLoadArgs; }
            set { m_VideoDownLoadArgs = value; }
        }
        public DownloadVideo(string vehicleId,string mdvrId,DateTime startTime,DateTime endTime)
        {
            InitializeComponent();
            this._vehicleId = vehicleId;
            this._mdvrId = mdvrId;

            StartTime.SelectedDate = startTime.Date;
            StartShortTime.Value = startTime;
            EndTime.SelectedDate = endTime.Date;
            EndShortTime.Value = endTime;
            VehicleId.Text = vehicleId;

        }

        #region event
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

            this.DialogResult = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            m_VideoDownLoadArgs = new VideoDownLoadArgs();
            m_VideoDownLoadArgs.CarNo = _vehicleId;
            if (RB_QueryServerFileList.IsChecked == true)
            {
                m_VideoDownLoadArgs.ArgType = QueryType.QueryServerFileList;
            }
            else if (RB_QueryServerDownloadFileList.IsChecked == true)
            {
                m_VideoDownLoadArgs.ArgType = QueryType.QueryServerDownloadFileList;
            }
            else
            {
                m_VideoDownLoadArgs.ArgType = QueryType.QueryMdvrFileList;
            }
            if (StartTime.SelectedDate.HasValue)
            {
                m_VideoDownLoadArgs.StartTime = DateTime.Parse(StartTime.SelectedDate.Value.Date.ToShortDateString() + " " + StartShortTime.Value.Value.ToLongTimeString());
            }
            if (EndTime.SelectedDate.HasValue)
            {
                m_VideoDownLoadArgs.EndTime = DateTime.Parse(EndTime.SelectedDate.Value.Date.ToShortDateString() + " " + EndShortTime.Value.Value.ToLongTimeString());
            }
            m_VideoDownLoadArgs.MdvrCoreSn = _mdvrId;

            if ((this.StartTime.SelectedDate == null || this.EndTime.SelectedDate == null || !StartShortTime.Value.HasValue || !EndShortTime.Value.HasValue || (m_VideoDownLoadArgs.StartTime > m_VideoDownLoadArgs.EndTime)))
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_DownVideoConditionDetail"));
                return;
            }

            this.DialogResult = true;
        }

        private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DialogResult == true &&
                (this.StartTime.SelectedDate == null || this.EndTime.SelectedDate == null))
            {
                e.Cancel = true;
                ChildWindow cw = new ChildWindow();
                cw.Content = ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_DownVideoConditionDetail");
                cw.Show();
            }
        }
        #endregion
    }
}

