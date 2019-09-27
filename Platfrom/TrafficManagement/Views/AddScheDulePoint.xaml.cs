using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Traffic.Models;
using Gsafety.Common.CommMessage;
using Jounce.Core.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Traffic.Views
{
    public partial class AddScheDulePoint : ChildWindow
    {
        /// <summary>
        /// 
        /// </summary>
        [Import]
        public IEventAggregator _EventAggregator { get; set; }
        public AddScheDulePoint()
        {
            CompositionInitializer.SatisfyImports(this);
            InitializeComponent();
        }
        public string _curScheDuleID = "";
        public StopScheDule _curSchedule = null;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(_curScheDuleID))
            {
                this.DialogResult = false;
                return;
            }
            //short nNum = 0;
            //try
            //{
            //    nNum = Convert.ToInt16(this.PointNum.Value);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_InValidData_SchedulePointNum"));
            //    ApplicationContext.Instance.Logger.LogError("AddScheDulePoint", ex);
            //    return;
            //}
            //if (nNum <= 0 || nNum > 100)
            //{
            //    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_InValidData_SchedulePointNum"));
            //    return;
            //}
            DateTime dt = DateTime.Now;
            try
            {
                dt = Convert.ToDateTime(this.DueTime.Value);
                dt = Convert.ToDateTime(dt.ToString("HH:mm:ss"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_InValidData_ScheduleDueTime"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageBoxButton.OK);
                ApplicationContext.Instance.Logger.LogError("AddScheDulePoint", ex);
                return;
            }
            if (string.IsNullOrEmpty(this.tbStopScheDulePointAddress.Text.Trim()))
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_InValidData_ScheduleAddress"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageBoxButton.OK);
                return;
            }
            if (this.tbStopScheDulePointAddress.Text.Trim().Length > ConstDefineModel.NameMaxLength)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_Input_NameTip"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageBoxButton.OK);
                return;
            }
            _addPointInfo = new StopScheDulePoint();
            _addPointInfo.Due_Time = dt.ToString("HH:mm:ss");
            _addPointInfo.ID = Guid.NewGuid().ToString();
            _addPointInfo.IsmarkStopGraphic = false;
            _addPointInfo.Location = this.tbStopScheDulePointAddress.Text.Trim();

            _EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = true });
            TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
            //trafficServiceClient.GetAtLastScheDulePointCompleted += trafficServiceClient_GetAtLastScheDulePointCompleted;
            //trafficServiceClient.GetAtLastScheDulePointAsync(_curScheDuleID);
            //trafficServiceClient.GetMaxScheDulePointNumCompleted += trafficServiceClient_GetMaxScheDulePointNumCompleted;
            //trafficServiceClient.GetMaxScheDulePointNumAsync(_curScheDuleID);

        }
        /// <summary>
        /// Get the last control point information (number, time, etc.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void trafficServiceClient_GetAtLastScheDulePointCompleted(object sender, GetAtLastScheDulePointCompletedEventArgs e)
        //{
        //    _EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
        //    if (e.Error == null && e.Result.IsSuccess == true)
        //    {
        //        StopScheDulePoint lastPt  = e.Result.Result;
        //        if (lastPt == null)
        //        {
        //            _addPointInfo.Point_NUM = 1;
        //        }
        //        else
        //        {
        //            _addPointInfo.Point_NUM = (short)(lastPt.Point_NUM + 1);
        //            //Effectiveness judgment time
        //            try
        //            {
        //                DateTime dt = Convert.ToDateTime(lastPt.Due_Time);
        //                DateTime dtNew = Convert.ToDateTime(_addPointInfo.Due_Time);
        //                if (dt.TimeOfDay >= dtNew.TimeOfDay)
        //                {
        //                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Invalid_Time"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageBoxButton.OK);
        //                    return;
        //                }
        //                if (_curSchedule!=null && ((dtNew - dt).TotalSeconds <= _curSchedule.Tolerance))
        //                {
        //                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Invalid_Time"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageBoxButton.OK);
        //                    return;
        //                }
        //            }
        //            catch(Exception ex)
        //            {
        //                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Invalid_Time"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageBoxButton.OK);
        //                ApplicationContext.Instance.Logger.LogError("trafficServiceClient_GetMaxScheDulePointNumCompleted", ex);
        //                return;
        //            }
        //        }
        //        //_addPointInfo.Point_NUM = (short)(nMax + 1);
        //        this.DialogResult = true;
        //    }
        //    else
        //    {
        //        ApplicationContext.Instance.Logger.LogError("trafficServiceClient_GetMaxScheDulePointNumCompleted", e.Error);
        //        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_GetMaxScheDulePointNumFaild"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageBoxButton.OK);
        //    }
        //}
        /// <summary>
        /// max
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void trafficServiceClient_GetMaxScheDulePointNumCompleted(object sender, GetMaxScheDulePointNumCompletedEventArgs e)
        //{
        //    _EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
        //    if (e.Error == null && e.Result.IsSuccess == true)
        //    {
        //        short nMax = e.Result.Result;
        //        _addPointInfo.Point_NUM = (short)(nMax + 1);
        //        this.DialogResult = true;
        //    }
        //    else
        //    {
        //        ApplicationContext.Instance.Logger.LogError("trafficServiceClient_GetMaxScheDulePointNumCompleted", e.Error);
        //        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_GetMaxScheDulePointNumFaild"));
        //    }
        //}


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void DueTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Ctrl_CanNotUsed"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageBoxButton.OK);
            }
            base.OnKeyDown(e);
            e.Handled = true;
        }
    }
}

