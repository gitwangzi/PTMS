using Gsafety.PTMS.Manager.Models;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Reflection;
namespace Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
{
    [ExportAsViewModel(ManagerName.ConfigRulesDetailViewModel)]
    public class ConfigRulesDetailVm : BaseEntityViewModel
    {
        CommandManageServiceClient config;
        #region
        public IActionCommand ReturnCommand { get; set; }
        public IActionCommand GpsDefaultCommand { get; set; }
        public IActionCommand AlarmDefaultCommand { get; set; }
        public IActionCommand AbnormalDoorCommand { get; set; }
        public IActionCommand TemperatureCommand { get; set; }
        AlarmSettingRules toDefaultAlarm = new AlarmSettingRules();
        GpsSettingInfo toDefaultGps = new GpsSettingInfo();
        AbnormalDoorRuleInfo toDefaultAbnormalDoor = new AbnormalDoorRuleInfo();
        TemperatureRuleInfo toDefaultTemperature = new TemperatureRuleInfo();
        public Collection<ConfigDetail> configDataList { get; set; }
        public Collection<GpsSettingInfo> ConfiggpsList { get; set; }
        public Collection<AbnormalDoorRuleInfo> ConfigabnormalDoorList { get; set; }
        public Collection<AlarmSettingRules> ConfigAlarmSettingList { get; set;}
        public Collection<TemperatureRuleInfo> ConfigTemperatureList { get; set; }
        BaseEnumInfo _currentSettingtype=new BaseEnumInfo();
        ConfigDetail configDetails = new ConfigDetail();      
        private string _Vehcile_ID;
        public string Vehcile_ID
        {
            get { return _Vehcile_ID; }
            set
            {
                _Vehcile_ID = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Vehcile_ID));
            }
        }
        private string _SuiteID;
        public string SuiteID
        {
            get { return _SuiteID; }
            set
            {
                _SuiteID = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SuiteID));
            }
        }

        private string _MANAGER_HighTemperature;
        public string MANAGER_HighTemperature
        {
            get { return _MANAGER_HighTemperature; }
            set
            {
                _MANAGER_HighTemperature = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_HighTemperature));
            }
        }

        private string _MANAGER_LowTemperatrue;
        public string MANAGER_LowTemperatrue
        {
            get { return _MANAGER_LowTemperatrue; }
            set
            {
                _MANAGER_LowTemperatrue = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_LowTemperatrue));
            }
        }

        private string _MANAGER_TemperaturDiscription;
        public string MANAGER_TemperaturDiscription
        {
            get { return _MANAGER_TemperaturDiscription; }
            set
            {
                _MANAGER_TemperaturDiscription = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_TemperaturDiscription));
            }
        }

        private string _MANAGER_Temperature_RuleName;
        public string MANAGER_Temperature_RuleName
        {
            get { return _MANAGER_Temperature_RuleName; }
            set
            {
                _MANAGER_Temperature_RuleName = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_Temperature_RuleName));
            }
        }

        private string _MANAGER_TemperatureToVehcileCount;
        public string MANAGER_TemperatureToVehcileCount
        {
            get { return _MANAGER_TemperatureToVehcileCount; }
            set
            {
                _MANAGER_TemperatureToVehcileCount = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_TemperatureToVehcileCount));
            }
        }

        
        private string _AbnormalDoor_Rule_Name;
        public string MANAGER_AbnormalDoor_Rule_Name
        {
            get { return _AbnormalDoor_Rule_Name; }
            set
            {
                _AbnormalDoor_Rule_Name = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_AbnormalDoor_Rule_Name));
            }
        }

        private string _MANAGER_AbnormalDoorSpeed;
        public string MANAGER_AbnormalDoorSpeed
        {
            get { return _MANAGER_AbnormalDoorSpeed; }
            set
            {
                _MANAGER_AbnormalDoorSpeed = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_AbnormalDoorSpeed));
            }
        }
        private string _MANAGER_AbnormalDoorToVehcileCount;
        public string MANAGER_AbnormalDoorToVehcileCount
        {
            get { return _MANAGER_AbnormalDoorToVehcileCount; }
            set
            {
                _MANAGER_AbnormalDoorToVehcileCount = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_AbnormalDoorToVehcileCount));
            }
        }

        private string _MANAGER_AbnormalDescription;
        public string MANAGER_AbnormalDescription
        {
            get
            { return _MANAGER_AbnormalDescription; }
            set
            {
                _MANAGER_AbnormalDescription = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_AbnormalDescription));            
            }       
        }

        private string _MANAGER_Alarm_RuleName;
        public string MANAGER_Alarm_RuleName
        {
            get
            { return _MANAGER_Alarm_RuleName; }
            set
            {
                _MANAGER_Alarm_RuleName = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_Alarm_RuleName));
            }
        }

        private string _MANAGER_AlarmButtonTime;
        public string MANAGER_AlarmButtonTime
        {
            get
            { return _MANAGER_AlarmButtonTime; }
            set
            {
                _MANAGER_AlarmButtonTime = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_AlarmButtonTime));
            }
        }

        private string _MANAGER_AlarmDescription;
        public string MANAGER_AlarmDescription
        {
            get
            { return _MANAGER_AlarmDescription; }
            set
            {
                _MANAGER_AlarmDescription = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_AlarmDescription));
            }
        }

        private string _MANAGER_AlarmNormalTime;
        public string MANAGER_AlarmNormalTime
        {
            get
            { return _MANAGER_AlarmNormalTime; }
            set
            {
                _MANAGER_AlarmNormalTime = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_AlarmNormalTime));
            }
        }

        private string _MANAGER_AlarmToVehcileCount;
        public string MANAGER_AlarmToVehcileCount
        {
            get
            { return _MANAGER_AlarmToVehcileCount; }
            set
            {
                _MANAGER_AlarmToVehcileCount = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_AlarmToVehcileCount));
            }
        }

        private string _MANAGER_GpsRuleName;
        public string MANAGER_GpsRuleName
        {
            get { return _MANAGER_GpsRuleName; }
            set
            {
                _MANAGER_GpsRuleName = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_GpsRuleName));
            }
        }

        private string _MANAGER_GpsIfMonitor;
        public string MANAGER_GpsIfMonitor
        {
            get { return _MANAGER_GpsIfMonitor; }
            set
            {
                _MANAGER_GpsIfMonitor = value;
                if (!string.IsNullOrEmpty(value))
                {
                    if (int.Parse(value) == 1)
                    {
                        _MANAGER_GpsIfMonitor = ApplicationContext.Instance.StringResourceReader.GetString("Real");
                    }
                    if (int.Parse(value) == 0)
                    {
                        _MANAGER_GpsIfMonitor = ApplicationContext.Instance.StringResourceReader.GetString("NotTrue");
                    }
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_GpsIfMonitor));
            }
        }
        private string _MANAGER_GpsUploadCount;
        public string MANAGER_GpsUploadCount
        {
            get { return _MANAGER_GpsUploadCount; }
            set
            {
                _MANAGER_GpsUploadCount = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_GpsUploadCount));
            }
        }

        private string _MANAGER_GpsUploadType;
        public string MANAGER_GpsUploadType
        {
            get { return _MANAGER_GpsUploadType; }
            set
            {
                _MANAGER_GpsUploadType = value;
                if (!string.IsNullOrEmpty(value))
                { 
                    switch (value)
                    {
                        case "0":
                            _MANAGER_GpsUploadType = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_GpsSetting_SendType_DistanceValue");
                        break;
                        case "1":
                        _MANAGER_GpsUploadType = ApplicationContext.Instance.StringResourceReader.GetString("Rpt_Alarm_Time");
                        break;
                        case "2":
                        _MANAGER_GpsUploadType = ApplicationContext.Instance.StringResourceReader.GetString("Manager_MexUpload");
                        break;

                    }
                
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_GpsUploadType));
            }
        }

        private string _MANAGER_GpsDistance;
        public string MANAGER_GpsDistance
        {
            get { return _MANAGER_GpsDistance; }
            set
            {
                _MANAGER_GpsDistance = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_GpsDistance));
            }
        }

        private string _MANAGER_GpsTime;
        public string MANAGER_GpsTime
        {
            get { return _MANAGER_GpsTime; }
            set
            {
                _MANAGER_GpsTime = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_GpsTime));
            }
        }

        private string _MANAGER_GpsToVehcileCount;
        public string MANAGER_GpsToVehcileCount
        {
            get { return _MANAGER_GpsToVehcileCount; }
            set
            {
                _MANAGER_GpsToVehcileCount = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_GpsToVehcileCount));
            }
        }
        public string _MANAGER_GpsNote;
        public string MANAGER_GpsNote
        {
            get { return _MANAGER_GpsNote; }
            set
            {
                _MANAGER_GpsNote = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_GpsNote));
            }
        }


        #endregion

        public ConfigRulesDetailVm()
        {
            try
            {
                config = ServiceClientFactory.Create<CommandManageServiceClient>();
                ReturnCommand = new ActionCommand<object>(obj => Return());
                GpsDefaultCommand = new ActionCommand<object>(obj => GpsDefault());
                AlarmDefaultCommand = new ActionCommand<object>(obj => AlarmDefault());
                AbnormalDoorCommand = new ActionCommand<object>(obj => AbnormalDoorDefault());
                TemperatureCommand = new ActionCommand<object>(obj => TemperatureDefault());
                config.ConfigGpsCompleted += config_ConfigGpsCompleted;
                config.ConfigAbnormalDoorCompleted += config_ConfigAbnormalDoorCompleted;
                config.ConfigAlarmSettingsCompleted += config_ConfigAlarmSettingsCompleted;
                config.ConfigTemperatureSettingsCompleted += config_ConfigTemperatureSettingsCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                configDetails = viewParameters["ConfigRulesDetailView"] as Gsafety.PTMS.ServiceReference.CommandManageService.ConfigDetail;
                InitialPage(configDetails);

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        public void Publish(string name)
        {
          try
            {
                EventAggregator.Publish(new ViewNavigationArgs(ManagerName.ConfigInfoView));
            }
          catch (Exception ex)
            {
                throw;
            }        
        }

        private void Return()
        {
            Publish("ConfigInfoView");
            ToEmpty();
        }

        private void ToEmpty()
        {
            Vehcile_ID = string.Empty;
            SuiteID = string.Empty;
            MANAGER_HighTemperature = string.Empty;
            MANAGER_LowTemperatrue = string.Empty;
            MANAGER_TemperaturDiscription = string.Empty;
            MANAGER_Temperature_RuleName = string.Empty;
            MANAGER_AbnormalDoor_Rule_Name = string.Empty;
            MANAGER_AbnormalDoorSpeed = string.Empty;
            MANAGER_AbnormalDescription = string.Empty;
            MANAGER_Alarm_RuleName = string.Empty;
            MANAGER_AlarmButtonTime = string.Empty;
            MANAGER_AlarmNormalTime = string.Empty;
            MANAGER_AlarmDescription = string.Empty;
            MANAGER_GpsRuleName = string.Empty;
            MANAGER_GpsIfMonitor = string.Empty;
            MANAGER_GpsUploadCount = string.Empty;
            MANAGER_GpsDistance = string.Empty;
            MANAGER_GpsTime = string.Empty;
            MANAGER_GpsNote = string.Empty;
            MANAGER_GpsToVehcileCount = string.Empty;
            MANAGER_AbnormalDoorToVehcileCount = string.Empty;
            MANAGER_AlarmToVehcileCount = string.Empty;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MANAGER_Alarm_RuleName));
        }
        private void GpsDefault()
        {

            if (!string.IsNullOrEmpty(configDetails.Gps_RuleName))
            {
                config.GetDefaultGpsInfoAsync();
                config.GetDefaultGpsInfoCompleted += config_GetDefaultGpsInfoCompleted;
            }
          
           
        }

        private void AlarmDefault()
        {
            if (!string.IsNullOrEmpty(configDetails.Alarm_RuleName))
            {
                config.GetDefaultAlarmInfoAsync();
                config.GetDefaultAlarmInfoCompleted += config_GetDefaultAlarmInfoCompleted;
            }
        }

        private void AbnormalDoorDefault()
        {
            if (!string.IsNullOrEmpty(configDetails.AbnormalDoor_ruleName))
            {
                config.GetDefaultAbnormalDoorInfoAsync();
                config.GetDefaultAbnormalDoorInfoCompleted += config_GetDefaultAbnormalDoorInfoCompleted;
            }
        }

        private void TemperatureDefault()
        {
            if (!string.IsNullOrEmpty(configDetails.Temperature_ruleName))
            {
                config.GetDefaultTemperatureInfoAsync();
                config.GetDefaultTemperatureInfoCompleted += config_GetDefaultTemperatureInfoCompleted;
            }
        }

        private void config_GetDefaultGpsInfoCompleted(object sender, GetDefaultGpsInfoCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    IEnumerable<GpsSettingInfo> toDefaultGpsEnum = e.Result.Result;
                    IList<GpsSettingInfo> toDefaultGpsList;
                    toDefaultGpsList = (IList<GpsSettingInfo>)toDefaultGpsEnum;
                    foreach (var item in toDefaultGpsList)
                    {
                        toDefaultGps.Gps_RuleID = configDetails.Gps_RuleID;
                        toDefaultGps.Gps_RuleName = item.Gps_RuleName;
                        toDefaultGps.Gps_IfMonitor = item.Gps_IfMonitor;
                        toDefaultGps.Gps_Distance = item.Gps_Distance;
                        toDefaultGps.Gps_Time = item.Gps_Time;
                        toDefaultGps.Gps_UploadSum = item.Gps_UploadSum;
                        toDefaultGps.Gps_UploadType = item.Gps_UploadType;
                        toDefaultGps.Gps_IsDefault = 0;
                        toDefaultGps.Gps_Valid = item.Gps_Valid;
                        toDefaultGps.Gps_CreateTime = DateTime.Now;
                        toDefaultGps.Gps_Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                    }
                    //config.ModifyGpsSettingsAsync(toDefaultGps);
                    //config.ModifyGpsSettingsCompleted += config_ModifyGpsSettingsCompleted;
                    Gsafety.PTMS.ServiceReference.TrafficManageService.SelectInfoModel selectModel = new Gsafety.PTMS.ServiceReference.TrafficManageService.SelectInfoModel();
                    ServiceReference.MessageService.GpsSendUpModel model = new ServiceReference.MessageService.GpsSendUpModel();
                    model.Value = new ObservableCollection<Gsafety.PTMS.ServiceReference.MessageService.SelectInfoModel>();
                    selectModel.Code = configDetails.MDVR_ID;
                    selectModel.Type = Gsafety.PTMS.ServiceReference.TrafficManageService.SettingType.Vehicle;
                    model.Setting = new SettingGpsSendUpCMD();
                    model.Setting.SendTime = DateTime.Now;
                    model.Setting.RuleName = toDefaultGps.Gps_RuleID;
                    model.Setting.IsUsing = (int)toDefaultGps.Gps_IfMonitor;
                    model.Setting.SendNum = toDefaultGps.Gps_UploadSum;
                    model.Setting.DistanceValue = toDefaultGps.Gps_Distance;
                    model.Setting.TimeValue = toDefaultGps.Gps_Time;
                    model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                    ApplicationContext.Instance.MessageManager.SendSettingGpsCMD(model);
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void config_GetDefaultAlarmInfoCompleted(object sender, GetDefaultAlarmInfoCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    IEnumerable<AlarmSettingRules> toDefaultAlarmEnum = e.Result.Result;
                    IList<AlarmSettingRules> toDefaultAlarmList;
                    toDefaultAlarmList = (IList<AlarmSettingRules>)toDefaultAlarmEnum;
                    foreach (var item in toDefaultAlarmList)
                    {
                        toDefaultAlarm.Alarm_RuleID = configDetails.Alarm_RuleID;
                        toDefaultAlarm.Alarm_RuleName = item.Alarm_RuleName;
                        toDefaultAlarm.Alarm_Normal = item.Alarm_Normal;
                        toDefaultAlarm.Alarm_ButtonTime = item.Alarm_ButtonTime;
                        toDefaultAlarm.Alarm_CreateTime = DateTime.Now;
                        toDefaultAlarm.Alarm_IsDefault = 0;
                        toDefaultAlarm.Alarm_Valid = item.Alarm_Valid;
                        toDefaultAlarm.Alarm_Creator = item.Alarm_Creator;
                    }
                    //config.ModifyAlarmSettingsAsync(toDefaultAlarm);
                    //config.ModifyAlarmSettingsCompleted += config_ModifyAlarmSettingsCompleted;
                    Gsafety.PTMS.ServiceReference.TrafficManageService.SelectInfoModel selectModel = new Gsafety.PTMS.ServiceReference.TrafficManageService.SelectInfoModel();
                    ServiceReference.MessageService.OneKeyAlarmSendUpModel model = new ServiceReference.MessageService.OneKeyAlarmSendUpModel();
                    model.DelayAlarmSetting = new SettingDelayAlarmCMD();
                    model.Value = new ObservableCollection<Gsafety.PTMS.ServiceReference.MessageService.SelectInfoModel>();
                    selectModel.Code = configDetails.MDVR_ID;
                    selectModel.Type = Gsafety.PTMS.ServiceReference.TrafficManageService.SettingType.Vehicle;
                    model.Setting = new SettingOneKeyAlarmCMD();
                    model.Setting.RuleName = toDefaultAlarm.Alarm_RuleName;
                    model.Setting.SendTime = DateTime.Now;
                    model.OperationType = RuleOperationType.Default;
                    model.Setting.SendValue = (short)toDefaultAlarm.Alarm_ButtonTime;
                    model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                    model.DelayAlarmSetting.SendValue = (short)toDefaultAlarm.Alarm_ButtonTime;
                    model.DelayAlarmSetting.OneKeyDelayTime = (short)toDefaultAlarm.Alarm_Normal;
                    model.DelayAlarmSetting.SendTime = DateTime.Now;
                    model.DelayAlarmSetting.RuleID = toDefaultAlarm.Alarm_RuleID;
                    model.DelayAlarmSetting.SendTime = DateTime.Now;
                    ApplicationContext.Instance.MessageManager.SendSettingOneKeyAlarmUploadCMD(model);
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void config_GetDefaultAbnormalDoorInfoCompleted(object sender, GetDefaultAbnormalDoorInfoCompletedEventArgs e)
        {
            try
            {
                IEnumerable<AbnormalDoorRuleInfo> toDefaultAbnormalDoorEnum = e.Result.Result;
                IList<AbnormalDoorRuleInfo> toDefaultAbnormalDoorList;
                toDefaultAbnormalDoorList = (IList<AbnormalDoorRuleInfo>)toDefaultAbnormalDoorEnum;
                foreach (var item in toDefaultAbnormalDoorList)
                {
                    toDefaultAbnormalDoor.ID = configDetails.AbnormalDoor_ruleID;
                    toDefaultAbnormalDoor.RuleName = item.RuleName;
                    toDefaultAbnormalDoor.Speed = item.Speed;
                    toDefaultAbnormalDoor.UsingCount = item.UsingCount;
                    toDefaultAbnormalDoor.IsDefault = false;
                    toDefaultAbnormalDoor.ValID = false;
                    toDefaultAbnormalDoor.UserDescription = item.UserDescription;
                    toDefaultAbnormalDoor.Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                    toDefaultAbnormalDoor.CreateTime = DateTime.Now;
                }
                //config.UpdateExistAbnormalDoorRuleAsync(toDefaultAbnormalDoor);
                //config.UpdateExistAbnormalDoorRuleCompleted+=config_UpdateExistAbnormalDoorRuleCompleted;     
                AbnormalDoorRuleInfo abnormalDoorInfo = new AbnormalDoorRuleInfo();
                Gsafety.PTMS.ServiceReference.TrafficManageService.SelectInfoModel selectModel = new Gsafety.PTMS.ServiceReference.TrafficManageService.SelectInfoModel();
                ServiceReference.MessageService.AbnormalDoorSendUpModel model = new ServiceReference.MessageService.AbnormalDoorSendUpModel();
                model.Value = new ObservableCollection<Gsafety.PTMS.ServiceReference.MessageService.SelectInfoModel>();
                selectModel.Code = configDetails.MDVR_ID;
                selectModel.Type = Gsafety.PTMS.ServiceReference.TrafficManageService.SettingType.Vehicle;
                model.Setting = new SettingAbnormalDoorCMD();
                model.Setting.SendTime = DateTime.Now;
                model.OperationType = RuleOperationType.Default;
                model.Setting.SendValue = abnormalDoorInfo.Speed.ToString();
                model.Setting.RuleName = abnormalDoorInfo.ID;
                ApplicationContext.Instance.MessageManager.SendAbnormalDoorSettingCMD(model);
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void config_GetDefaultTemperatureInfoCompleted(object sender, GetDefaultTemperatureInfoCompletedEventArgs e)
        {
            try
            {
                IEnumerable<TemperatureRuleInfo> toDefaultTemperatureEnum = e.Result.Result;
                IList<TemperatureRuleInfo> toDefaultTemperatureList;
                toDefaultTemperatureList = (IList<TemperatureRuleInfo>)toDefaultTemperatureEnum;
                foreach (var item in toDefaultTemperatureList)
                {
                    toDefaultTemperature.ID = configDetails.Temperature_ruleID;
                    toDefaultTemperature.RuleName = item.RuleName;
                    toDefaultTemperature.LowTemperature = item.LowTemperature;
                    toDefaultTemperature.HighTemperature = item.HighTemperature;
                    toDefaultTemperature.TemperatureType = item.TemperatureType;
                    toDefaultTemperature.UserDescription = item.UserDescription;
                    toDefaultTemperature.CreateTime = DateTime.Now;
                    toDefaultTemperature.Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                }
                //config.UpdateTemperatureRuleAsync(toDefaultTemperature);
                //config.UpdateTemperatureRuleCompleted+=config_UpdateTemperatureRuleCompleted;
                TemperatureRuleInfo temperatureInfo = new TemperatureRuleInfo();
                Gsafety.PTMS.ServiceReference.TrafficManageService.SelectInfoModel selectModel = new Gsafety.PTMS.ServiceReference.TrafficManageService.SelectInfoModel();
                ServiceReference.MessageService.TemperatureSendUpModel model = new ServiceReference.MessageService.TemperatureSendUpModel();
                model.Value = new ObservableCollection<Gsafety.PTMS.ServiceReference.MessageService.SelectInfoModel>();
                selectModel.Code = configDetails.MDVR_ID;
                selectModel.Type = Gsafety.PTMS.ServiceReference.TrafficManageService.SettingType.Vehicle;
                model.Setting = new SettingTemperatureCMD();
                model.Setting.SendTime = DateTime.Now;
                model.Setting.RuleName = toDefaultTemperature.ID;
                model.OperationType = RuleOperationType.Add;
                model.Setting.SendType = (TemperatureMarkType)_currentSettingtype.Code;
                if (temperatureInfo.SettingType == ServiceReference.CommandManageService.TemperatureSettingType.Enable)
                {
                    model.Setting.TemperatureType = toDefaultTemperature.TemperatureType;
                    model.Setting.MaxValue = toDefaultTemperature.HighTemperature;
                    model.Setting.MinValue = toDefaultTemperature.HighTemperature;
                }
                else
                {
                    model.Setting.MaxValue = null;
                    model.Setting.MinValue = null;
                    model.Setting.TemperatureType = null;
                }
                ApplicationContext.Instance.MessageManager.SendTemperatureSettingCMD(model);

                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        public void InitialPage(ConfigDetail configDetails)
        {
            try
            {
                MANAGER_GpsRuleName = configDetails.Gps_RuleName;
                MANAGER_AbnormalDoor_Rule_Name = configDetails.AbnormalDoor_ruleName;
                MANAGER_Alarm_RuleName = configDetails.Alarm_RuleName;
                MANAGER_Temperature_RuleName = configDetails.Temperature_ruleName;
                Vehcile_ID = configDetails.VehcileID;
                SuiteID = configDetails.Suite_ID;
                //config.ConfigInfoDetailAsync(configDetails.Gps_RuleName, configDetails.Temperature_ruleName, configDetails.Alarm_RuleName, configDetails.AbnormalDoor_ruleName);
                config.ConfigGpsAsync(configDetails.Gps_RuleName);
                config.ConfigAbnormalDoorAsync(configDetails.AbnormalDoor_ruleName);
                config.ConfigAlarmSettingsAsync(configDetails.Alarm_RuleName);
                config.ConfigTemperatureSettingsAsync(configDetails.Temperature_ruleName);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        //void config_ConfigInfoDetailCompleted(object sender, ConfigInfoDetailCompletedEventArgs e)
        //{
        //    try
        //    {
        //        configDataList = e.Result.Result;
        //        foreach (var items in configDataList)
        //        {
        //            MANAGER_GpsIfMonitor = items.Gps_IfMonitor.ToString();
        //            MANAGER_GpsUploadCount = items.Gps_UploadSum.ToString();
        //            MANAGER_GpsDistance = items.Gps_Distance.ToString();
        //        }
        //    }
        //    catch(Exception)
        //    {
        //        throw;
        //    }
        //}
        void config_ConfigGpsCompleted(object sender, ConfigGpsCompletedEventArgs e)
        {
            try
            {
                ConfiggpsList = e.Result.Result;
                foreach (var items in ConfiggpsList)
                {
                    MANAGER_GpsIfMonitor = items.Gps_IfMonitor.ToString();
                    MANAGER_GpsUploadCount = items.Gps_UploadSum.ToString();
                    MANAGER_GpsUploadType = items.Gps_UploadType.ToString();
                    MANAGER_GpsDistance = items.Gps_Distance.ToString();
                    MANAGER_GpsNote = items.Gps_Description;
                    MANAGER_GpsToVehcileCount = items.Gps_VehicleCount.ToString();
                }
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }        
        }
        void config_ConfigAbnormalDoorCompleted(object sender, ConfigAbnormalDoorCompletedEventArgs e)
        {
            try
            {
                ConfigabnormalDoorList = e.Result.Result;
                foreach (var items in ConfigabnormalDoorList)
                {
                    MANAGER_AbnormalDoorSpeed = items.Speed.ToString(); 
                    MANAGER_AbnormalDescription  =items.UserDescription;
                    MANAGER_AbnormalDoorToVehcileCount = items.UsingCount.ToString();
                }
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }        
        }
        void config_ConfigAlarmSettingsCompleted(object sender, ConfigAlarmSettingsCompletedEventArgs e)
        {
            try
            {
                ConfigAlarmSettingList = e.Result.Result;
                foreach (var items in ConfigAlarmSettingList)
                {
                    MANAGER_AlarmButtonTime =items.Alarm_ButtonTime.ToString();
                    MANAGER_AlarmNormalTime = items.Alarm_Normal.ToString();
                    MANAGER_AlarmDescription = items.Alarm_Description;
                    MANAGER_AlarmToVehcileCount = items.Alarm_VehcileCount.ToString();
                }
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }                
        }
        void config_ConfigTemperatureSettingsCompleted(object sender, ConfigTemperatureSettingsCompletedEventArgs e)
        {
            try
            {
                ConfigTemperatureList = e.Result.Result;
                foreach (var items in ConfigTemperatureList)
                {
                    MANAGER_HighTemperature = items.HighTemperature.ToString();
                    MANAGER_LowTemperatrue = items.LowTemperature.ToString();
                    MANAGER_TemperaturDiscription = items.UserDescription;
                    MANAGER_TemperatureToVehcileCount = items.UsingCount.ToString();
                }
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }        
        }

    }
}
