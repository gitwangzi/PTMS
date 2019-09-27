/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 09f0dcfa-5ed4-4803-87b7-0d3ae65dad77      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: DeviceSelftestViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/17 15:32:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/17 15:32:14
/////            Modified by:
/////   Modified Description: 
/////======================================================================
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
using System.Linq;
using System.Collections.Generic;
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;
using Gsafety.PTMS.ServiceReference.WorkingSuiteService;
using Gsafety.PTMS.Installation.Models;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System.Reflection;

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.DeviceSelftestVm)]
    public class DeviceSelftestViewModel : InstallViewModelBase
    {

        private SelfCheckInfor _SelfChackInfors = new SelfCheckInfor();
        public SelfCheckInfor SelfChackInfors
        {
            get { return _SelfChackInfors; }
            set
            {
                _SelfChackInfors = value;
                IsMaintenance = _SelfChackInfors.Check();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => { RaisePropertyChanged(() => SelfChackInfors); ChangeStyle(); });
            }
        }
        public SelfInspectInfo _SelfInspectInfo { get; private set; }

        SecuritySuiteServiceClient securitySuiteServiceClient = null;
        WorkingSuiteServiceClient workingSuiteServiceClient = null;

        public DeviceSelftestViewModel()
        {            
            try
            {
                step = 4;
                securitySuiteServiceClient = ServiceClientFactory.Create<SecuritySuiteServiceClient>();
                workingSuiteServiceClient = ServiceClientFactory.Create<WorkingSuiteServiceClient>();
                ImageSource = "Step04.png";
                deviceInstallServiceClient.GetSelfInspectCompleted += deviceinstallserviceClient_GetSelfInspectCompleted;
                deviceInstallServiceClient.GetInstallationResultCompleted += deviceinstallserviceClient_GetInstallationResultCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DeviceSelftestViewModel()", ex);
            }          
        }


        protected override void Get()
        {
            deviceInstallServiceClient.GetInstallationResultAsync(_InstallID);
        }

        bool InspectCheck = false;

        void deviceinstallserviceClient_GetInstallationResultCompleted(object sender, GetInstallationResultCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    if (e.Result.Result.Audit != null)
                    {
                        deviceInstallServiceClient.GetSelfInspectAsync(e.Result.Result.Audit.SelfInspectId);
                        if (e.Result.Result.Audit.SelfInspectCheck == 1)
                        {
                            InspectCheck = true;
                        }
                        else
                        {
                            InspectCheck = false;
                        }
                    }
                    else
                    {
                        InspectCheck = false;
                        IsMaintenance = true;
                        IsFinished = false;
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_GetInstallationAuditFailed"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                    }
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("deviceinstallserviceClient_GetInstallationResultCompleted", ex);
            }
        }

        void deviceinstallserviceClient_GetSelfInspectCompleted(object sender, GetSelfInspectCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    if (e.Result.Result != null)
                    {

                        _SelfInspectInfo = e.Result.Result;
                        SelfCheckInfor info = new SelfCheckInfor();
                        info.Channel1 = _SelfInspectInfo.Channel1;
                        info.Channel2 = _SelfInspectInfo.Channel2;
                        info.Channel3 = _SelfInspectInfo.Channel3;
                        info.Channel4 = _SelfInspectInfo.Channel4;
                        info.DeviceSN = _SelfInspectInfo.MdvrCoreId;
                        info.CheckTime = _SelfInspectInfo.InspectTime.ToString();
                        info.RecHDD = _SelfInspectInfo.RecSD;
                        info.Sensor1 = _SelfInspectInfo.Sensor1;
                        info.Sensor2 = _SelfInspectInfo.Sensor2;
                        info.Sensor3 = _SelfInspectInfo.Sensor3;
                        info.StandbyPower = _SelfInspectInfo.StandbyPower;
                        info.Module3G = _SelfInspectInfo.Module3G;
                        info.CurInTemperature = _SelfInspectInfo.CurInTemperature;
                        info.SIM = _SelfInspectInfo.SimCard;
                        info.CurVoltage = _SelfInspectInfo.CurVoltage;
                        info.GPSINFO = _SelfInspectInfo.GpsInfo;
                        info.SdCapacity = _SelfInspectInfo.SdCapacity;
                        SelfChackInfors = info;

                        if (InspectCheck)
                        {
                            IsFinished = true;
                            IsMaintenance = false;
                        }
                    }
                    else
                    {
                        IsFinished = false;
                        IsMaintenance = true;
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_GetSelfInspectFailed"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("deviceinstallserviceClient_GetSelfInspectCompleted", ex);
            }
        }

        protected override void Quit()
        {
            var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_IfQuitInstall"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                ResetData();
                EventAggregator.Publish(new ViewNavigationArgs(InstallationName.DeviceInstallV));
            }
        }

        protected override void GoNextPage()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.CheckDeviceFunctionV, new Dictionary<string, object>() { { "ID", _InstallID } }));
        }

        protected override void NextPage()
        {
            try
            {
                //TODO
                InstallationInfo installDetailModel = new InstallationInfo()
                {
                    /// ID
                    Id = _InstallID,
                    /// The current installation steps
                    CheckStep = 4
                };
                deviceInstallServiceClient.UpdateInstallationAsync(installDetailModel);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DeviceSelftestViewModel Nextpage",ex);
            }
        }

        private void ChangeStyle()
        {
            if (this.SelfChackInfors == null)
            {
                return;
            }
            var view = Router.ViewQuery("DeviceSelftest") as Gsafety.PTMS.Installation.Views.InstallInitiateSuiteView;

            //textbox
            foreach (var prop in SelfChackInfors.GetType().GetProperties().Where(x => x.PropertyType == typeof(string)))
            {
                var propValue = Convert.ToString(prop.GetValue(this.SelfChackInfors, null));
                if (string.IsNullOrWhiteSpace(propValue))
                {
                    continue;
                }
                var textBox = view.FindName(prop.Name) as System.Windows.Controls.TextBlock;
                if (textBox != null)
                {
                    textBox.Foreground = new SolidColorBrush(GetUIColorRole(prop.Name, propValue));
                }
            }

        }


        private Color GetUIColorRole(string propName, string propValue)
        {
            var result = Color.FromArgb(255, 255, 255, 255);

            if (propName.EndsWith("_Result"))
            {
                if (propValue == ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Success"))
                {
                    result = Color.FromArgb(255, 0, 190, 0);
                }
                else if (propValue == ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Falid"))
                {
                    result = Color.FromArgb(255, 255, 0, 0);
                }
            }
            else
            {
                if (propName.StartsWith("Sensor"))
                {
                    if (propValue == "OK")
                    {
                        result = Color.FromArgb(255, 0, 0, 255);
                    }
                    else if (propValue == ApplicationContext.Instance.StringResourceReader.GetString("SelfCheck_NoInformation"))
                    {
                        result = Color.FromArgb(255, 0, 0, 0);
                    }
                    else
                    {
                        result = Color.FromArgb(255, 255, 128, 64);
                    }

                }
            }

            return result;


        }

        protected override void ResetData()
        {
            try
            {
                SelfChackInfors.DeviceSN = string.Empty;
                SelfChackInfors.DeviceSN_Result = string.Empty;
                SelfChackInfors.CheckTime = string.Empty;
                SelfChackInfors.CheckTime_Result = string.Empty;
                SelfChackInfors.RecHDD = string.Empty;
                SelfChackInfors.RecHDD_Result = string.Empty;
                SelfChackInfors.Sensor1 = string.Empty;
                SelfChackInfors.Sensor1_Result = string.Empty;
                SelfChackInfors.Sensor2 = string.Empty;
                SelfChackInfors.Sensor2_Result = string.Empty;
                SelfChackInfors.Sensor3 = string.Empty;
                SelfChackInfors.Sensor3_Result = string.Empty;
                SelfChackInfors.StandbyPower = string.Empty;
                SelfChackInfors.StandbyPower_Result = string.Empty;
                SelfChackInfors.Module3G = string.Empty;
                SelfChackInfors.Module3G_Result = string.Empty;
                SelfChackInfors.Channel1 = string.Empty;
                SelfChackInfors.Channel1_Result = string.Empty;
                SelfChackInfors.Channel2 = string.Empty;
                SelfChackInfors.Channel2_Result = string.Empty;
                SelfChackInfors.Channel3 = string.Empty;
                SelfChackInfors.Channel3_Result = string.Empty;
                SelfChackInfors.Channel4 = string.Empty;
                SelfChackInfors.Channel4_Result = string.Empty;
                SelfChackInfors.CurInTemperature = string.Empty;
                SelfChackInfors.CurInTemperature_Result = string.Empty;
                SelfChackInfors.SIM = string.Empty;
                SelfChackInfors.SIM_Result = string.Empty;
                SelfChackInfors.CurVoltage = string.Empty;
                SelfChackInfors.CurVoltage_Result = string.Empty;
                SelfChackInfors.GPSINFO = string.Empty;
                SelfChackInfors.GPSINFO_Result = string.Empty;
                SelfChackInfors.SdCapacity = string.Empty;
                SelfChackInfors.SdCapacity_Result = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelfChackInfors));
                IsFinished = false;
                IsMaintenance = false;
                IsGetMessage = true;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DeviceSelftestViewModel ResetData", ex);
            }
        }
    }
}
