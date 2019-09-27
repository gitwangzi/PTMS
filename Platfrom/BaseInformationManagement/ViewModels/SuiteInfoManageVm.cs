/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: fdd1112e-305b-4eb7-85fb-6a94e0363360      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.ViewModels
/////    Project Description:    
/////             Class Name: SuiteManageVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/22 8:43:21
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/22 8:43:21
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.ObjectModel;
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
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using System.Text.RegularExpressions;

namespace Gsafety.PTMS.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.SuiteInfoManageVm)]
    public class SuiteInfoManageVm : BaseInfoViewModelBase
    {
        private SecuritySuiteServiceClient suiteclient = ServiceClientFactory.Create<SecuritySuiteServiceClient>();
        private DeviceSuite InitialSecuritySuite { get; set; }
        public DeviceSuite CurrentSecuritySuite { get; set; }
        public EnumModel CurrentDeviceType { get; set; }
        public List<EnumModel> DeviceTypeList { get; set; }

      
        private void InitialDeviceType()
        {
            DeviceTypeList = new List<EnumModel>();
            Enum.GetNames(typeof(Gsafety.PTMS.ServiceReference.SecuritySuiteService.VehicleTypeEnum)).ToList().ForEach(x =>
            {
                EnumModel item = new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                DeviceTypeList.Add(item);
            });
        }
        //private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {

            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();

            switch (action)
            {
                case "view":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_LookSuite");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                    IsView = Visibility .Visible;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsView));
                    IsReadOnly = true;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    KeyIsReadOnly = true;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => KeyIsReadOnly));
                    InitialSecuritySuite = viewParameters["view"] as DeviceSuite;
                    break;
                case "update":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_UpdateSuite");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                    IsView = Visibility .Collapsed;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsView));
                    IsReadOnly = false;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    KeyIsReadOnly = true;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => KeyIsReadOnly));
                    InitialSecuritySuite = viewParameters["update"] as DeviceSuite;
                    break;
                case "add":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_AddSuite");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                    IsView = Visibility.Collapsed;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsView));
                    IsReadOnly = false;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    KeyIsReadOnly = false;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => KeyIsReadOnly));
                    break;
                default:
                    break;
            }
            Reset();
        }

        private int CallBackCount;
        private int OperateCount;
        private bool isDataExisted;
        public SuiteInfoManageVm()
        {
            try
            {
                Action<dynamic> CheckSecuritySuiteExistBySuiteID_CallBack = (e) =>
                {
                    try
                    {
                        var prop = ExtractPropertyName(() => SuiteId);
                        ClearErrors(prop);
                        if (e.Result != null && e.Result.Result)
                        {
                            SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DataExisted"));//数据已存在
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SuiteId));
                            isDataExisted = true;
                            System.Threading.Interlocked.Decrement(ref CallBackCount);
                        }
                        else
                        {
                            System.Threading.Interlocked.Increment(ref CallBackCount);
                        }
                    }
                    catch (Exception ex)
                    {
                        ApplicationContext.Instance.Logger.LogException("CheckSecuritySuiteExistBySuiteID_CallBack", ex);
                    }
                    OperateData();
                };
                Action<dynamic> CheckSecuritySuiteExistByMdvrCoreId_CallBack = (e) =>
                {
                    try
                    {
                        var prop = ExtractPropertyName(() => MdvrCoreId);
                        ClearErrors(prop);
                        if (e.Result != null && e.Result.Result)
                        {
                            SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DataExisted"));//数据已存在
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MdvrCoreId));
                            isDataExisted = true;
                            System.Threading.Interlocked.Decrement(ref CallBackCount);
                        }
                        else
                        {
                            System.Threading.Interlocked.Increment(ref CallBackCount);
                        }
                    }
                    catch (Exception ex)
                    {
                        ApplicationContext.Instance.Logger.LogException("CheckSecuritySuiteExistByMdvrCoreId_CallBack", ex);
                    }
                    OperateData();
                };
                Action<dynamic> CheckSecuritySuiteExistByMdvrId_CallBack = (e) =>
                {
                    try
                    {
                        var prop = ExtractPropertyName(() => MdvrId);
                        ClearErrors(prop);
                        if (e.Result != null && e.Result.Result)
                        {
                            SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DataExisted"));//数据已存在
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MdvrId));
                            isDataExisted = true;
                            System.Threading.Interlocked.Decrement(ref CallBackCount);
                        }
                        else
                        {
                            System.Threading.Interlocked.Increment(ref CallBackCount);
                        }
                    }
                    catch (Exception ex)
                    {
                        ApplicationContext.Instance.Logger.LogException("CheckSecuritySuiteExistByMdvrId_CallBack", ex);
                    }
                    OperateData();
                };
                suiteclient.CheckSecuritySuiteExistBySuiteIDCompleted += (s, e) => CheckSecuritySuiteExistBySuiteID_CallBack(e);
                suiteclient.CheckSecuritySuiteExistByMdvrCoreIdCompleted += (s, e) => CheckSecuritySuiteExistByMdvrCoreId_CallBack(e);
                suiteclient.CheckSecuritySuiteExistByMdvrIdCompleted += (s, e) => CheckSecuritySuiteExistByMdvrId_CallBack(e);
                suiteclient.AddSecuritySuiteCompleted += SecuritySuiteClient_AddSecuritySuiteCompleted;
                suiteclient.UpdateSecuritySuiteCompleted += SecuritySuiteClient_UpdateSecuritySuiteCompleted;

                ReturnCommand = new ActionCommand<object>(obj => Return());
                ResetCommand = new ActionCommand<object>(obj => Reset());

                InitialDeviceType();

            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SuiteInfoManageVm()", ex);
            }
        }

        protected override void OnCommitted()
        {
            //if (ZZmatch(SuiteId) && ZZmatch(MdvrId) && ZZmatch(MdvrCoreId))
            //{
            CurrentSecuritySuite.DeviceType = (Gsafety.PTMS.ServiceReference.SecuritySuiteService.VehicleTypeEnum)Enum.Parse(typeof(Gsafety.PTMS.ServiceReference.SecuritySuiteService.VehicleTypeEnum), CurrentDeviceType.EnumName, true);

            CurrentSecuritySuite.SuiteId = SuiteId;
            CurrentSecuritySuite.MdvrId = MdvrId;
            CurrentSecuritySuite.MdvrCoreId = MdvrCoreId;

            CallBackCount = 0;
            OperateCount = 0;
            isDataExisted = false;
            if (action.Equals("update"))
            {
                Update();
            }
            else
            {
                Add();
            }
        }

        protected bool ZZmatch(string str)
        {
            Regex obj = new Regex("^[A-Za-z0-9]+$");
            return obj.IsMatch(str);
        }

        private void Update()
        {
            if (SuiteId != InitialSecuritySuite.SuiteId)
            {
                suiteclient.CheckSecuritySuiteExistBySuiteIDAsync(SuiteId);
                OperateCount++;
            }
            if (MdvrCoreId != InitialSecuritySuite.MdvrCoreId)
            {
                suiteclient.CheckSecuritySuiteExistByMdvrCoreIdAsync(MdvrCoreId);
                OperateCount++;
            }
            if (MdvrId != InitialSecuritySuite.MdvrId)
            {
                suiteclient.CheckSecuritySuiteExistByMdvrIdAsync(MdvrId);
                OperateCount++;
            }
            if (OperateCount == 0)
            {
                suiteclient.UpdateSecuritySuiteAsync(CurrentSecuritySuite);
            }
        }

        private void Add()
        {
            OperateCount = 3;
            suiteclient.CheckSecuritySuiteExistBySuiteIDAsync(SuiteId);
            suiteclient.CheckSecuritySuiteExistByMdvrCoreIdAsync(MdvrCoreId);
            suiteclient.CheckSecuritySuiteExistByMdvrIdAsync(MdvrId);
        }

        private void OperateData()
        {
            if (CallBackCount == OperateCount && !isDataExisted)
            {
                switch (action)
                {
                    case "update":
                        suiteclient.UpdateSecuritySuiteAsync(CurrentSecuritySuite);
                        break;
                    case "add":
                        CurrentSecuritySuite.InstallStatus = InstallStatusType.UnInstall;
                        suiteclient.AddSecuritySuiteAsync(CurrentSecuritySuite);
                        break;
                    default:
                        break;
                }
            }
        }

        void SecuritySuiteClient_AddSecuritySuiteCompleted(object sender, AddSecuritySuiteCompletedEventArgs e)
        {
            try
            {
                if (e.Result == null || !e.Result.IsSuccess)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_AddSucess"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    Reset();
                    Return();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SecuritySuiteClient_AddSecuritySuiteCompleted", ex);
            }
        }

        void SecuritySuiteClient_UpdateSecuritySuiteCompleted(object sender, UpdateSecuritySuiteCompletedEventArgs e)
        {
            try
            {
                if (e.Result == null || !e.Result.IsSuccess)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_UpdateSucess"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                Return();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SecuritySuiteClient_UpdateSecuritySuiteCompleted", ex);
            }
        }

        protected override void Reset()
        {
            switch (action)
            {
                case "view":
                case "update":
                    CurrentSecuritySuite = BaseInformationCommon.Clone(InitialSecuritySuite);
                    SuiteId = CurrentSecuritySuite.SuiteId;
                    MdvrId = CurrentSecuritySuite.MdvrId;
                    MdvrCoreId = CurrentSecuritySuite.MdvrCoreId;
                    CurrentDeviceType = DeviceTypeList.FirstOrDefault(x => x.EnumName == CurrentSecuritySuite.DeviceType.ToString());
                    break;
                case "add":
                    CurrentSecuritySuite = new DeviceSuite();
                    CurrentSecuritySuite.status = DeviceSuiteStatus.Initial;
                    SuiteId = string.Empty;
                    MdvrId = string.Empty;
                    MdvrCoreId = string.Empty;
                    CurrentDeviceType = DeviceTypeList[1];
                    break;
                default:
                    break;
            }

            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentSecuritySuite));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SuiteId));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MdvrId));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MdvrCoreId));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentDeviceType));

        }

        private void Return()
        {       
            EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.SuiteInfoV, new Dictionary<string, object>() { { "action", "return" } }));
        }

        #region

        private string suiteId;
        public string SuiteId
        {
            get { return suiteId; }
            set
            {
                suiteId = value == null ? null : value.Trim();
                ValidateRequiredField(ExtractPropertyName(() => SuiteId), suiteId);
            }
        }

        private string mdvrId;
        public string MdvrId
        {
            get { return mdvrId; }
            set
            {
                mdvrId = value == null ? null : value.Trim();
                ValidateRequiredField(ExtractPropertyName(() => MdvrId), mdvrId);
            }
        }

        private string mdvrCoreId;
        public string MdvrCoreId
        {
            get { return mdvrCoreId; }
            set
            {
                mdvrCoreId = value == null ? null : value.Trim();
                ValidateMdvrCoreId(ExtractPropertyName(() => MdvrCoreId), mdvrCoreId);
            }
        }

        private void ValidateRequiredField(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MustFilled"));//必填字段
            }
        }

        private void ValidateMdvrCoreId(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MustFilled"));
            }
            if (!string.IsNullOrEmpty(value) && value.Length!=10)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_CoreRule"));//格式非法
            }
        }
        #endregion
    }
}
