using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Gsafety.PTMS.Share;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Jounce.Core.View;
using Jounce.Regions.Core;
using Gsafety.PTMS.ServiceReference.VehicleStatusService;
using Gsafety.PTMS.SecuritySuite.Models;
using Gsafety.PTMS.ServiceReference.MessageService;
using System.Reflection;
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;

namespace Gsafety.PTMS.SecuritySuite.Views
{
    public partial class InfoSuiteStatus : ChildWindow
    {
        #region Fields

        private string _MDVRCoreSN = string.Empty;
        VehicleStatusServiceClient client = null;
        private string SuiteInfoID = string.Empty;
        #endregion

        #region Attributes

        public DateTime CurrentTime { get; set; }
        public string VehicleId { get; set; }
        public string SuiteId { get; set; }
        public short CurrentStatus { get; set; }
        public short ChangedStatus { get; set; }
        public string userInfo { get; set; }
        public string changeReason { get; set; }
        public Action<bool> ResultAction;
        public Action<bool> AbnoramlToRepairResultAction;

        #endregion


        public InfoSuiteStatus(SuiteStatusControl suiteStatus)
        {
            try
            {
                InitializeComponent();
                this.UserInfo.Text = ApplicationContext.Instance.AuthenticationInfo.UserName;
                this.userInfo = this.UserInfo.Text.ToString();
                CurrentTime = DateTime.Now;
                SuiteInfoID = suiteStatus.Suite_info_id;
                this.DataContext = this;//bingding page to code      
                client = ServiceClientFactory.Create<VehicleStatusServiceClient>();
                client.RunningToAbnoramlCompleted += VehicleStatusServiceClient_RunningToAbnoramlCompleted;
                client.AbnoramlToRunningCompleted += VehicleStatusServiceClient_AbnoramlToRunningCompleted;
                client.AbnoramlToRepairCompleted += VehicleStatusServiceClient_AbnoramlToRepairCompleted;
                client.RepairToInitialCompleted += VehicleStatusServiceClient_RepairToInitialCompleted;
                this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }

        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        public void QueryAction(string vehicleID, string suiteID, short current, short change, string mdvrCoreSN)
        {
            try
            {
                this.VehicleId = vehicleID;
                this.SuiteId = suiteID;
                this.CurrentStatus = current;
                this.ChangedStatus = change;
                this._MDVRCoreSN = mdvrCoreSN;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SuiteMangementInfo suiteStatusToDB = new SuiteMangementInfo();
                suiteStatusToDB.CurrentStatus = this.CurrentStatus;
                suiteStatusToDB.StatusChange = this.ChangedStatus;
                suiteStatusToDB.changeReason = this.changeReason;
                suiteStatusToDB.VehicleID = this.VehicleId;
                suiteStatusToDB.SuiteID = this.SuiteId;
                suiteStatusToDB.SuiteINFOID = SuiteInfoID;
                suiteStatusToDB.UserInfo = this.userInfo;
                switch (this.CurrentStatus)
                {
                    case (int)DeviceSuiteStatus.Running:
                        {
                            client.RunningToAbnoramlAsync(suiteStatusToDB);
                        }
                        break;
                    case (int)DeviceSuiteStatus.Abnormal:
                        if (this.ChangedStatus == (int)DeviceSuiteStatus.Maintenance)
                        {
                            client.AbnoramlToRepairAsync(suiteStatusToDB);
                            Gsafety.PTMS.ServiceReference.MessageService.DeviceMaintain maintaindevice = new Gsafety.PTMS.ServiceReference.MessageService.DeviceMaintain();
                            maintaindevice.MdvrCoreId = this._MDVRCoreSN;
                            maintaindevice.MaintainTime = DateTime.Now;
                            ApplicationContext.Instance.MessageManager.SendDeviceMaintainMessage(maintaindevice);
                        }
                        else if (this.ChangedStatus == (int)DeviceSuiteStatus.Running)
                        {
                            client.AbnoramlToRunningAsync(suiteStatusToDB);
                        }
                        break;
                    case (int)DeviceSuiteStatus.Maintenance:
                        {
                            client.RepairToInitialAsync(suiteStatusToDB);
                        }
                        break;
                }
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        void VehicleStatusServiceClient_RunningToAbnoramlCompleted(object sender, RunningToAbnoramlCompletedEventArgs e)
        {
            try
            {
                if (e.Result == true)
                {
                    ResultAction(true);
                }
                else
                {
                    ResultAction(false);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        void VehicleStatusServiceClient_AbnoramlToRepairCompleted(object sender, AbnoramlToRepairCompletedEventArgs e)
        {
            try
            {
                if (e.Result == true)
                {
                    ResultAction(true);
                }
                else
                {
                    ResultAction(true);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        void VehicleStatusServiceClient_AbnoramlToRunningCompleted(object sender, AbnoramlToRunningCompletedEventArgs e)
        {
            try
            {
                if (e.Result == true)
                {
                    ResultAction(true);
                }
                else
                {
                    ResultAction(false);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }
        void VehicleStatusServiceClient_RepairToInitialCompleted(object sender, RepairToInitialCompletedEventArgs e)
        {
            try
            {
                if (e.Result == true)
                {
                    ResultAction(true);
                }
                else
                {
                    ResultAction(false);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }
    }
}

