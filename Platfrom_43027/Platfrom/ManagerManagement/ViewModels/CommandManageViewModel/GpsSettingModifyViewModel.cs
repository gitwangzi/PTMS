/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5c94d6f1-7075-46e9-838f-29a250ba06b6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
/////    Project Description:    
/////             Class Name: GpsSettingModifyViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/12 15:20:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/12 15:20:14
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using System.Collections.Generic;
using Gsafety.PTMS.Bases.Enums;
using Jounce.Core.View;
using System.Reflection;

namespace Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
{
    [ExportAsViewModel(ManagerName.GpsSettingModifyViewModel)]
    public class GpsSettingModifyViewModel : BaseEntityViewModel
    {
        CommandManageServiceClient gpsSettingAddClient;
        #region
        public ICommand ReturnCommand { get; private set; }
        public ICommand FinishCommand { get; private set; }
        public List<EnumModel> UploadTypeList { get; set; }
        public List<EnumModel> UploadUsingList { get; set; }
        public string Gps_Comments { get; set; }
        public string Title { get; private set; }
        GpsSettingInfo gpsAddInfo = new GpsSettingInfo();
        public GpsSettingInfo gpsSettingData { get; set; }
        private bool _IsEditDistance = false;
        public bool IsEditDistance
        {
            get { return _IsEditDistance; }
            set
            {
                _IsEditDistance = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsEditDistance));
            }
        }

        private bool _IsEditTime = false;
        public bool IsEditTime
        {
            get { return _IsEditTime; }
            set
            {
                _IsEditTime = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsEditTime));
            }
        }

        private bool _UploadEdit = false;
        public bool UploadEdit
        {
            get { return _UploadEdit; }
            set
            {
                _UploadEdit = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UploadEdit));
            }
        }
        private bool _IsSettingToVehicle = false;
        public bool IsSettingToVehcile
        {
            get { return _IsSettingToVehicle; }
            set
            {
                _IsSettingToVehicle = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsSettingToVehcile));
            }
        }
        private EnumModel _currentType;
        public EnumModel CurrentType
        {
            get
            {
                return _currentType;
            }
            set
            {
                _currentType = value;
                if (value.EnumValue == 0)
                {
                    Gps_TimeEnabled = false;
                    Gps_DistanceEnabled = true;
                }
                if (value.EnumValue == 1)
                {
                    Gps_DistanceEnabled = false;
                    Gps_TimeEnabled = true;
                }
                if (value.EnumValue == 2)
                {
                    Gps_DistanceEnabled = true;
                    Gps_TimeEnabled = true;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentType));
            }
        }

        private EnumModel _UsingType;
        public EnumModel UsingType
        {
            get
            {
                return _UsingType;
            }
            set
            {
                _UsingType = value;
                if (value.EnumValue == 1)
                {
                    UploadVisible = Visibility.Visible;
                }
                if (value.EnumValue == 0)
                {
                    UploadVisible = Visibility.Collapsed;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UsingType));
            }
        }
        private Visibility _UploadVisible;
        public Visibility UploadVisible
        {
            get
            {
                return _UploadVisible;
            }
            set
            {
                _UploadVisible = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UploadVisible));
            }
        }

        #endregion

        #region   property
        private int index;
        public int TabControlSelectIndex
        {
            get { return index; }
            set { index = value; }
        }

        private string _Gps_LoginName;
        public string Gps_LoginName
        {
            get { return _Gps_LoginName; }
            set
            {
                _Gps_LoginName = value == null ? null : value.Trim();
                ValidateGpsLoginName(ExtractPropertyName(() => Gps_LoginName), _Gps_LoginName);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_LoginName));              
            }
        }

        private void ValidateGpsLoginName(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MustFilled"));
            }
        }
        private string _Gps_UpLoadCount;
        public string Gps_UpLoadCount
        {
            get { return _Gps_UpLoadCount; }
            set
            {
                _Gps_UpLoadCount = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));              
            }
        }

        private string _Gps_Distance;
        public string Gps_Distance
        {
            get { return _Gps_Distance; }
            set
            {
                _Gps_Distance = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));            
            }
        }

        private string _Gps_Time;
        public string Gps_Time
        {
            get { return _Gps_Time; }
            set
            {
                _Gps_Time = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));              
            }
        }
        private bool ValidateGps(string prop, string value)
        {
         try
           {
              ClearErrors(prop);
              Match match = Regex.Match(value, @"^(0|[1-9]?[0-9]\d*|0)$");
              if (!match.Success)
               {
                  SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Number"));
                  return false;
               }
              else if (int.Parse(match.Value) > 65535 || int.Parse(match.Value) < 0)
               {
                  SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_ValiNumber"));
                  return false;
               }
               return true;
            }
            catch(Exception ex)
            {
              throw ex;
            }
        }
        bool ValidateNumbers(string prop, string value)
        {
            try
            {
                ClearErrors(prop);
                if (!Regex.Match(value, "^[0-9]*$").Success)
                {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Number"));
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }     
        private bool _Gps_DistanceEnabled = false;
        public bool Gps_DistanceEnabled
        {
            get { return _Gps_DistanceEnabled; }
            set
            {
                _Gps_DistanceEnabled = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_DistanceEnabled));
            }
        }

        private bool _Gps_TimeEnabled = false;
        public bool Gps_TimeEnabled
        {
            get { return _Gps_TimeEnabled; }
            set
            {
                _Gps_TimeEnabled = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_TimeEnabled));
            }
        }

        public void UpLoadValue()                  // 0:Distance  1:Time    2:Distance and Time
        {
            if (!string.IsNullOrEmpty(Gps_Time) && string.IsNullOrEmpty(Gps_Distance))
            {
                gpsAddInfo.Gps_UploadType = 1;
                gpsAddInfo.Gps_Time = short.Parse(Gps_Time);
            }
            if (!string.IsNullOrEmpty(Gps_Distance) && string.IsNullOrEmpty(Gps_Time))
            {
                gpsAddInfo.Gps_UploadType = 0;
                gpsAddInfo.Gps_Distance = short.Parse(Gps_Distance);
            }
            if (!string.IsNullOrEmpty(Gps_Distance) && !string.IsNullOrEmpty(Gps_Time))
            {
                gpsAddInfo.Gps_UploadType = 2;
                gpsAddInfo.Gps_Distance = short.Parse(Gps_Distance);
                gpsAddInfo.Gps_Time = short.Parse(Gps_Time);
            }
        }
        #endregion

        public GpsSettingModifyViewModel()
        {
            try
            {
                gpsSettingAddClient = ServiceClientFactory.Create<CommandManageServiceClient>();
                ReturnCommand = new ActionCommand<object>(obj => Return());
                FinishCommand = new ActionCommand<object>(obj => Finish());
                IsSettingToVehcile = false;
                getTypeUpload();
                getUsingUpload();
                gpsSettingAddClient.ModifyGpsSettingsCompleted += gpsSettingAddClient_ModifyGpsSettingsCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                Title = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_ModifyGpsSetting");
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                gpsSettingData = viewParameters["Modify"] as Gsafety.PTMS.ServiceReference.CommandManageService.GpsSettingInfo;
                InitialPage(gpsSettingData);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }
        public void InitialPage(GpsSettingInfo rulesInfo)
        {
            try
            {
                Gps_LoginName = gpsSettingData.Gps_RuleName;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_LoginName));
                Gps_UpLoadCount = gpsSettingData.Gps_UploadSum.ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                Gps_Time = gpsSettingData.Gps_Time.ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                Gps_Distance = gpsSettingData.Gps_Distance.ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                Gps_Comments = gpsSettingData.Gps_Description;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Comments));
                if (gpsSettingData.Gps_IfMonitor != null)
                {
                    usingType((short)gpsSettingData.Gps_IfMonitor);
                }
                if (gpsSettingData.Gps_UploadType == null)
                {
                    gpsSettingData.Gps_UploadType = short.Parse(CurrentType.EnumValue.ToString());
                    viewType((short)gpsSettingData.Gps_UploadType);
                }
                else
                {
                    viewType((short)gpsSettingData.Gps_UploadType);
                }
                UsingType = UsingType;
                CurrentType = CurrentType;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(ManagerName.GpsSettingView, new Dictionary<string, object>() { { "action", "return" } }));
            UploadEdit = false;
            toEmpty();
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UploadEdit));
        }
        private void Finish()
        {
            try
            {
                if (!string.IsNullOrEmpty(Gps_LoginName))
                {
                    if (UsingType.EnumValue == 1)
                    {
                       
                        if (CurrentType.EnumValue == 2)
                        {
                            if (ValidateGps(ExtractPropertyName(() => Gps_Time), _Gps_Time) && ValidateGps(ExtractPropertyName(() => Gps_Distance), _Gps_Distance) && ValidateGps(ExtractPropertyName(() => Gps_UpLoadCount), _Gps_UpLoadCount))
                            {
                                if (int.Parse(Gps_Time) > 1 && int.Parse(Gps_Time) < 65535 && int.Parse(Gps_Distance) > 1 && int.Parse(Gps_Distance) < 65535 && int.Parse(Gps_UpLoadCount) > 1 && int.Parse(Gps_UpLoadCount) < 65535)
                                {
                                    UpLoadValue();
                                    gpsAddInfo.Gps_RuleID = gpsSettingData.Gps_RuleID;
                                    gpsAddInfo.Gps_IfMonitor = 1;
                                    gpsAddInfo.Gps_Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                                    gpsAddInfo.Gps_RuleName = Gps_LoginName;
                                    gpsAddInfo.Gps_UploadSum = int.Parse(Gps_UpLoadCount);
                                    gpsAddInfo.Gps_Description = Gps_Comments;
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentType));
                                    gpsAddInfo.Gps_UploadType = (short)CurrentType.EnumValue;
                                    gpsSettingAddClient.ModifyGpsSettingsAsync(gpsAddInfo);
                                }
                                else
                                {
                                    ValidateGps(ExtractPropertyName(() => Gps_Time), _Gps_Time);
                                    ValidateGps(ExtractPropertyName(() => Gps_Distance), _Gps_Distance);
                                    ValidateGps(ExtractPropertyName(() => Gps_UpLoadCount), _Gps_UpLoadCount);
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                                }
                            }
                            else
                            {
                                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                            }
                        }
                        if (CurrentType.EnumValue == 1)
                        {
                            if (ValidateNumbers(ExtractPropertyName(() => Gps_Time), _Gps_Time) && ValidateGps(ExtractPropertyName(() => Gps_UpLoadCount), _Gps_UpLoadCount))
                            {
                                if (int.Parse(Gps_Time) > 1 && int.Parse(Gps_Time) < 65535 && int.Parse(Gps_UpLoadCount) > 1 && int.Parse(Gps_UpLoadCount) < 65535)
                                {
                                    UpLoadValue();
                                    gpsAddInfo.Gps_RuleID = gpsSettingData.Gps_RuleID;
                                    gpsAddInfo.Gps_IfMonitor = 1;
                                    gpsAddInfo.Gps_Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                                    gpsAddInfo.Gps_RuleName = Gps_LoginName;
                                    gpsAddInfo.Gps_UploadSum = int.Parse(Gps_UpLoadCount);
                                    gpsAddInfo.Gps_Description = Gps_Comments;
                                    gpsAddInfo.Gps_Distance = null;
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentType));
                                    gpsAddInfo.Gps_UploadType = (short)CurrentType.EnumValue;
                                    gpsSettingAddClient.ModifyGpsSettingsAsync(gpsAddInfo);
                                }
                                else
                                {
                                    ValidateGps(ExtractPropertyName(() => Gps_Time), _Gps_Time);
                                    ValidateGps(ExtractPropertyName(() => Gps_Distance), _Gps_Distance);
                                    ValidateGps(ExtractPropertyName(() => Gps_UpLoadCount), _Gps_UpLoadCount);
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                                }
                            }
                            else
                            {
                                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                            }
                        }
                        if (CurrentType.EnumValue == 0)
                        {
                            if (ValidateNumbers(ExtractPropertyName(() => Gps_Distance), _Gps_Distance) && ValidateGps(ExtractPropertyName(() => Gps_UpLoadCount), _Gps_UpLoadCount))
                            {
                                if (int.Parse(Gps_Distance) > 1 && int.Parse(Gps_Distance) < 65535 && int.Parse(Gps_UpLoadCount) > 1 && int.Parse(Gps_UpLoadCount) < 65535)
                                {
                                    UpLoadValue();
                                    gpsAddInfo.Gps_RuleID = gpsSettingData.Gps_RuleID;
                                    gpsAddInfo.Gps_IfMonitor = 1;
                                    gpsAddInfo.Gps_Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                                    gpsAddInfo.Gps_RuleName = Gps_LoginName;
                                    gpsAddInfo.Gps_Time = null;
                                    gpsAddInfo.Gps_UploadSum = int.Parse(Gps_UpLoadCount);
                                    gpsAddInfo.Gps_Description = Gps_Comments;
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentType));
                                    gpsAddInfo.Gps_UploadType = (short)CurrentType.EnumValue;
                                    gpsSettingAddClient.ModifyGpsSettingsAsync(gpsAddInfo);
                                }
                                else
                                {
                                    ValidateGps(ExtractPropertyName(() => Gps_Time), _Gps_Time);
                                    ValidateGps(ExtractPropertyName(() => Gps_Distance), _Gps_Distance);
                                    ValidateGps(ExtractPropertyName(() => Gps_UpLoadCount), _Gps_UpLoadCount);
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                                }
                            }
                            else
                            {
                                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                            }
                        }
                    }
                    else if (UsingType.EnumValue == 0)
                    {
                        gpsAddInfo.Gps_RuleID = gpsSettingData.Gps_RuleID;
                        gpsAddInfo.Gps_Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        gpsAddInfo.Gps_RuleName = Gps_LoginName;
                        gpsAddInfo.Gps_Description = Gps_Comments;
                        gpsAddInfo.Gps_IfMonitor = (short)UsingType.EnumValue;
                        gpsAddInfo.Gps_Time = null;
                        gpsAddInfo.Gps_Distance = null;
                        gpsAddInfo.Gps_UploadType = null;
                        gpsAddInfo.Gps_UploadSum = null;
                        //InitialPage(gpsAddInfo);
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UsingType));
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UploadVisible));
                        gpsSettingAddClient.ModifyGpsSettingsAsync(gpsAddInfo);
                    }

                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_SettingNameISNULL"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        void gpsSettingAddClient_ModifyGpsSettingsCompleted(object sender, ModifyGpsSettingsCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    this.Gps_DistanceEnabled = false;
                    this.Gps_TimeEnabled = false;
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Faild"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void toEmpty()
        {
            try
            {
                this.Gps_LoginName = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_LoginName));
                this.Gps_Time = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                this.Gps_Distance = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                this.Gps_UpLoadCount = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                this.Gps_Comments = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Comments));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void getUsingUpload()
        {
            UploadUsingList = new List<EnumModel>();
            foreach (var item in Enum.GetNames(typeof(GpsIfMonitor)))
            {
                int typeValue = (int)Enum.Parse(typeof(GpsIfMonitor), item, true);
                UploadUsingList.Add(new EnumModel { EnumName = item, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(item), EnumValue = typeValue });
            }
            UsingType = UploadUsingList.FirstOrDefault();
        }

        private void getTypeUpload()
        {
            UploadTypeList = new List<EnumModel>();
            foreach (var item in Enum.GetNames(typeof(GpsUploadType)))
            {
                int typeValue = (int)Enum.Parse(typeof(GpsUploadType), item, true);
                UploadTypeList.Add(new EnumModel { EnumName = item, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(item), EnumValue = typeValue });
            }
            CurrentType = UploadTypeList.FirstOrDefault();
        }
        private void viewType(int value)
        {
            CurrentType = UploadTypeList.Where(e => e.EnumValue == value).FirstOrDefault();
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentType));
        }
        private void usingType(int value)
        {
            UsingType = UploadUsingList.Where(e => e.EnumValue == value).FirstOrDefault();
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UsingType));
        }
    }
}

