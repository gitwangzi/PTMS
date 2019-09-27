using Gsafety.Ant.Installation.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.ServiceReference.BscDevSuiteService;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 04849f7f-ad2b-44b6-9d22-f25d4731d197      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: InputDeviceInfoViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/16 16:18:11
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/16 16:18:11
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.InstallSuiteCheckVm)]
    public class InstallSuiteCheckViewModel : InstallSuiteViewModelBase
    {
        private string suitesn;
        public string SuiteSn
        {
            get { return suitesn; }
            set
            {
                suitesn = value;
                if (string.IsNullOrEmpty(suitesn))
                {
                    IsGetMessage = false;
                }
                else
                {
                    IsGetMessage = true;
                }

                RaisePropertyChanged(() => SuiteSn);
            }
        }

        private string protocol;
        public string Protocol
        {
            get { return protocol; }
            set
            {
                protocol = value;
                RaisePropertyChanged(() => Protocol);
            }
        }

        string organizationid = string.Empty;

        private DevSuite _CurrentModel = new DevSuite();
        public DevSuite CurrentModel
        {
            get
            {
                return _CurrentModel;
            }
            set
            {
                _CurrentModel = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentModel));
            }
        }

        private DevSuitePart _selecteditem = null;

        public DevSuitePart SelectedItem
        {
            get { return _selecteditem; }
            set
            {
                _selecteditem = value;
                RaisePropertyChanged(() => this.SelectedItem);
            }
        }

        VehicleServiceClient vehicleServiceClient = null;
        BscDevSuiteServiceClient suiteclient = null;

        IList<EnumInfos> categorys = null;
        public InstallSuiteCheckViewModel()
        {
            try
            {
                step = 2;
                vehicleServiceClient = ServiceClientFactory.Create<VehicleServiceClient>();
                suiteclient = ServiceClientFactory.Create<BscDevSuiteServiceClient>();
                ImageSource = "Step02.png";

                var adapter = new EnumAdapter<Gsafety.PTMS.Bases.Enums.ProtocolTypeEnum>();
                categorys = adapter.GetEnumInfos();

                vehicleServiceClient.CheckInstallVehicleForSuiteCompleted += vehicleServiceClient_CheckInstallVehicleForSuiteCompleted;
                deviceInstallServiceClient.SubmitForStep2Completed += deviceInstallServiceClient_SubmitForStep2Completed;
                suiteclient.GetDevSuiteBySuiteIDCompleted += suiteclient_GetBscDevSuiteBySuiteIDCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InputDeviceInfoViewModel()", ex);
            }
        }

        protected override void Get()
        {
            vehicleServiceClient.CheckInstallVehicleForSuiteAsync(InstallInfo.VehicleID, ApplicationContext.Instance.AuthenticationInfo.ClientID);
        }

        void vehicleServiceClient_CheckInstallVehicleForSuiteCompleted(object sender, CheckInstallVehicleForSuiteCompletedEventArgs e)
        {
            try
            {
                organizationid = e.Result.Result.OrganizationID;
                suiteclient.GetDevSuiteBySuiteIDAsync(SuiteSn);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vehicleServiceClient_CheckInstallVehicleCompleted", ex);
            }
        }

        void suiteclient_GetBscDevSuiteBySuiteIDCompleted(object sender, GetDevSuiteBySuiteIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.Result != null)
                    {
                        #region

                        CurrentModel = e.Result.Result;

                        if (CurrentModel.BscDevSuiteParts != null && CurrentModel.BscDevSuiteParts.Count > 0)
                        {
                            SelectedItem = CurrentModel.BscDevSuiteParts[0];
                            foreach (var item in CurrentModel.BscDevSuiteParts)
                            {
                                if (item.ProduceTime.HasValue)
                                    item.ProduceTime = item.ProduceTime.Value.ToLocalTime();

                                if (item.PartType.ToString() == "AlarmButton")
                                {
                                    item.ShowParttype = ApplicationContext.Instance.StringResourceReader.GetString("AlarmButton");
                                }
                                else if (item.PartType.ToString() == "Camera")
                                {
                                    item.ShowParttype = ApplicationContext.Instance.StringResourceReader.GetString("Camera");
                                }
                                else if (item.PartType.ToString() == "Screen")
                                {
                                    item.ShowParttype = ApplicationContext.Instance.StringResourceReader.GetString("Screen");
                                }

                                if (item.ProduceTime.HasValue)
                                {
                                    item.ProduceTime = item.ProduceTime.Value.ToLocalTime();
                                }
                            }
                        }

                        Protocol = categorys.FirstOrDefault(t => t.Value == (short)CurrentModel.Protocol).LocalizedString;
                        string showTip = string.Empty;

                        if (CurrentModel.Status == (short)DeviceSuiteStatus.Initial)
                        {
                            IsFinished = true;
                        }
                        else if (CurrentModel.Status == (short)DeviceSuiteStatus.Working)
                        {
                            IsFinished = false;
                            showTip = "ID_INSTALL_SuiteAlreadyInstalled";
                        }
                        else if (CurrentModel.Status == (short)DeviceSuiteStatus.Testing)
                        {
                            IsFinished = false;
                            showTip = "ID_INSTALL_SuiteTesting";
                        }
                        else if (CurrentModel.Status == (short)DeviceSuiteStatus.Running)
                        {
                            IsFinished = false;
                            showTip = "ID_INSTALL_SuiteAlreadyInstalled";
                        }
                        else if (CurrentModel.Status == (short)DeviceSuiteStatus.Abnormal)
                        {
                            IsFinished = false;
                            showTip = "ID_INSTALL_SuiteAbnormal";
                        }
                        else if (CurrentModel.Status == (short)DeviceSuiteStatus.WaitingMaintenance)
                        {
                            IsFinished = false;
                            showTip = "ID_INSTALL_SuiteWaitingMaintenance";
                        }
                        else if (CurrentModel.Status == (short)DeviceSuiteStatus.Maintenance)
                        {
                            IsFinished = false;
                            showTip = "ID_INSTALL_SuiteMaintenance";
                        }
                        else if (CurrentModel.Status == (short)DeviceSuiteStatus.Scrap)
                        {
                            IsFinished = false;
                            showTip = "ID_INSTALL_SuiteScrap";
                        }
                        else if (CurrentModel.Status == (short)DeviceSuiteStatus.History)
                        {
                            IsFinished = false;
                            showTip = "ID_INSTALL_SuiteHistory";
                        }
                        else
                        {
                            IsFinished = false;
                        }

                        if (IsFinished == false)
                        {
                            var result = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString(showTip), MessageDialogButton.Ok);
                            result.Closed += result_Closed;
                        }

                        #endregion
                    }
                    else
                    {
                        IsFinished = false;
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg), MessageDialogButton.Ok);
                    }
                }
                else
                {
                    IsFinished = false;
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), MessageDialogButton.Ok);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("securitySuiteServiceClient_GetSecuritySuiteBySuiteIdCompleted", ex);
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_GetSecuritySuiteInfoFailed"), MessageDialogButton.Ok);
            }
        }

        protected override void NextPage()
        {
            try
            {
                IsFinished = false;
                Step2Package package = new Step2Package();
                package.SuiteId = CurrentModel.SuiteInfoID;
                package.SuiteKey = CurrentModel.SuiteID;
                package.VehicleId = InstallInfo.VehicleID;
                package.MDVR_CORE_SN = CurrentModel.MdvrCoreSn;
                package.SuiteStatus = (int)DeviceSuiteStatus.Testing;
                package.InstallID = _InstallID;
                package.SelfInspectCheck = -1;
                package.AlarmCheck = -1;
                package.GpsCheck = -1;
                package.VideoCheck = -1;
                package.VideoQualityCheck = -1;
                package.IsSuccess = -1;
                package.SuiteStatus = (int)DeviceSuiteStatus.Working;
                package.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                package.Organization = organizationid;
                package.SuiteSwitchTime = DateTime.Now.ToUniversalTime();

                deviceInstallServiceClient.SubmitForStep2Async(package);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InputDeviceInfoViewModel NextPage", ex);
            }
        }

        void deviceInstallServiceClient_SubmitForStep2Completed(object sender, SubmitForStep2CompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    try
                    {
                        ApplicationContext.Instance.MessageClient.BeginInstallSuite(CurrentModel.MdvrCoreSn);
                    }
                    catch (Exception ex)
                    {
                        ApplicationContext.Instance.Logger.LogException("deviceInstallServiceClient_SubmitForStep2Completed", ex);
                    }
                    ResetData();
                    EventAggregator.Publish(new ViewNavigationArgs(InstallationName.InstallVehcileSuiteCheckV, new Dictionary<string, object>() { { "ID", _InstallID } }));
                }
                else
                {
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString(e.Result.Result), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.Result), MessageDialogButton.Ok);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }




        protected override void ResetData()
        {
            try
            {
                CurrentModel.SuiteID = string.Empty;
                CurrentModel.MdvrSn = string.Empty;
                CurrentModel.MdvrCoreSn = string.Empty;
                CurrentModel.UpsSn = string.Empty;
                CurrentModel.SdSn = string.Empty;
                CurrentModel.MdvrSim = string.Empty;
                CurrentModel.MdvrSimMobile = string.Empty;
                CurrentModel.SoftwareVersion = string.Empty;
                CurrentModel.Model = string.Empty;
                if (CurrentModel.BscDevSuiteParts != null)
                {
                    CurrentModel.BscDevSuiteParts.Clear();
                }


                organizationid = string.Empty;
                SuiteSn = string.Empty;
                Protocol = string.Empty;


                IsFinished = false;
                IsGetMessage = false;

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentModel));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InputDeviceInfoViewModel ResetData", ex);
            }
        }

        private void result_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    ResetData();
                }
            }
        }
    }
}
