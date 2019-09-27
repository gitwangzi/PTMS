using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.PTMS.Manager.Models;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
{
    [ExportAsViewModel(ManagerName.ConfigInfoViewModel)]
    public class ConfigInfoViewModel : BaseViewModel
    {
        CommandManageServiceClient config;
        #region
        private int currentIndex = 1;
        public PagedServerCollection<ConfigInfo> ConfigInfoList { get; set; }
        public IActionCommand QueryCommand { get; set; }
        public IActionCommand LOOKCommand { get; set; }
        public IActionCommand DefaultCommand { get; set; }
        public AlarmSettingRules alarmInfo { get; set; }
        ConfigDetail configDetail = new ConfigDetail();
        public AbnormalDoorRuleInfo CurrentSettingInfo { get; set; }
        AlarmSettingRules toDefaultAlarm = new AlarmSettingRules();
        GpsSettingInfo toDefaultGps = new GpsSettingInfo();
        AbnormalDoorRuleInfo toDefaultAbnormalDoor = new AbnormalDoorRuleInfo();
        TemperatureRuleInfo toDefaultTemperature = new TemperatureRuleInfo();
        private ConfigInfo _configData;
        public ConfigInfo configData
        {
            get
            {
                return _configData;
            }
            set
            {
                _configData = value;
            }

        }


        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.ConfigInfoList != null)
                {
                    this.ConfigInfoList.PageSize = value;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageSizeValue));
            }
        }
        //private BaseEnumInfo _currentSettingtype;
        //public BaseEnumInfo CurrentSettingType
        //{
        //    get { return _currentSettingtype; }
        //    set
        //    {
        //        _currentSettingtype = value;
        //    }
        //}
        BaseEnumInfo CurrentSettingType = new BaseEnumInfo();
        public string VehicleID { get; set; }
        public string Suite_ID { get; set; }
        public List<int> PageSizeList { get; set; }
        #endregion

        public ConfigInfoViewModel()
        {
            try
            {
                config = ServiceClientFactory.Create<CommandManageServiceClient>();
                PageSizeList = new List<int> { 20, 40, 80 };
                PageSizeValue = PageSizeList[0];
                QueryCommand = new ActionCommand<object>(obj => Query());
                DefaultCommand = new ActionCommand<object>(obj => Default());
                LOOKCommand = new ActionCommand<object>(obj => Publish("ConfigRulesDetailView"));
                InitailPagedServerCollection();
                config.ConfigInfoCompleted += congfig_ConfigInfoCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void InitailPagedServerCollection()
        {
            try
            {
                ConfigInfoList = new PagedServerCollection<ConfigInfo>(new Action<int, int>(InvokServer));
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
                configDetail.Gps_RuleName = configData.Gps_RuleName;
                configDetail.AbnormalDoor_ruleName = configData.AbnormalDoor_RuleName;
                configDetail.Alarm_RuleName = configData.Alarm_RuleName;
                configDetail.Temperature_ruleName = configData.Temperature_RuleName;
                configDetail.VehcileID = configData.VehicleID;
                configDetail.Suite_ID = configData.SuiteID;
                configDetail.AbnormalDoor_ruleID = configData.AbnormalDoor_RuleID;
                configDetail.Alarm_RuleID = configData.Alarm_RuleID;
                configDetail.Gps_RuleID = configData.Gps_RuleID;
                configDetail.Temperature_ruleID = configData.Temperature_RuleID;
                configDetail.MDVR_ID = configData.MDVRID;
                EventAggregator.Publish(new ViewNavigationArgs(ManagerName.ConfigRulesDetailView, new Dictionary<string, object>() { { "action", name }, { name, configDetail } }));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void InvokServer(int pageIndex, int pageSize)
        {
            try
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                PagingInfo pageInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                config.ConfigInfoAsync(VehicleID, Suite_ID, pageInfo);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void congfig_ConfigInfoCompleted(object sender, ConfigInfoCompletedEventArgs e)
        {
            try
            {
                ConfigInfoList.loader_Finished(new PagedResult<ConfigInfo>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex
                });
                if (e.Result.Result.Count == 0)
                {
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("ConfigInfoViewModel", ex);
            }
        }

        private void Default()
        {
            if (MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_Rule_DelAllRelation"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                if (!string.IsNullOrEmpty(configData.Alarm_RuleName))
                {
                    config.GetDefaultAlarmInfoAsync();
                    config.GetDefaultAlarmInfoCompleted += config_GetDefaultAlarmInfoCompleted;
                }
                if (!string.IsNullOrEmpty(configData.Gps_RuleName))
                {
                    config.GetDefaultGpsInfoAsync();
                    config.GetDefaultGpsInfoCompleted += config_GetDefaultGpsInfoCompleted;
                }
                if (!string.IsNullOrEmpty(configData.AbnormalDoor_RuleName))
                {
                    config.GetDefaultAbnormalDoorInfoAsync();
                    config.GetDefaultAbnormalDoorInfoCompleted += config_GetDefaultAbnormalDoorInfoCompleted;
                }
                if (!string.IsNullOrEmpty(configData.Temperature_RuleName))
                {
                    config.GetDefaultTemperatureInfoAsync();
                    config.GetDefaultTemperatureInfoCompleted += config_GetDefaultTemperatureInfoCompleted;
                }
            }
        }
        private int IsOK = 0;
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
                        toDefaultAlarm.Alarm_RuleID = configData.Alarm_RuleID;
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
                    SelectInfoModel selectModel = new SelectInfoModel();
                    ServiceReference.MessageService.OneKeyAlarmSendUpModel model = new ServiceReference.MessageService.OneKeyAlarmSendUpModel();
                    model.DelayAlarmSetting = new SettingDelayAlarmCMD();
                    model.Value = new ObservableCollection<SelectInfoModel>();
                    selectModel.Code = configData.MDVRID;
                    selectModel.Type = SettingType.Vehicle;
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
                    IsOK++;
                    if (IsOK == 4)
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void Inital()
        {

        }
        //private void config_ModifyAlarmSettingsCompleted(object sender, ModifyAlarmSettingsCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Result.IsSuccess)
        //        {
        //        SelectInfoModel selectModel = new SelectInfoModel();
        //        ServiceReference.MessageService.OneKeyAlarmSendUpModel model = new ServiceReference.MessageService.OneKeyAlarmSendUpModel();
        //        model.DelayAlarmSetting = new SettingDelayAlarmCMD();
        //        model.Value = new ObservableCollection<SelectInfoModel>();
        //        selectModel.Code = configData.MDVRID;
        //        selectModel.Type = SettingType.Vehicle;
        //        model.Setting = new SettingOneKeyAlarmCMD();
        //        model.Setting.RuleName = toDefaultAlarm.Alarm_RuleName;
        //        model.Setting.SendTime = DateTime.Now;
        //        model.OperationType = RuleOperationType.Default;
        //        model.Setting.SendValue = (short)toDefaultAlarm.Alarm_ButtonTime;
        //        model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
        //        model.DelayAlarmSetting.SendValue = (short)toDefaultAlarm.Alarm_ButtonTime;
        //        model.DelayAlarmSetting.OneKeyDelayTime = (short)toDefaultAlarm.Alarm_Normal;
        //        model.DelayAlarmSetting.SendTime = DateTime.Now;
        //        model.DelayAlarmSetting.RuleID = toDefaultAlarm.Alarm_RuleID;
        //        model.DelayAlarmSetting.SendTime = DateTime.Now;
        //        ApplicationContext.Instance.MessageManager.SendSettingOneKeyAlarmUploadCMD(model);            
        //        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
        //        }
        //        ConfigInfoList.MoveToPage(currentIndex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }        
        //}

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
                        toDefaultGps.Gps_RuleID = configData.Gps_RuleID;
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
                    SelectInfoModel selectModel = new SelectInfoModel();
                    ServiceReference.MessageService.GpsSendUpModel model = new ServiceReference.MessageService.GpsSendUpModel();
                    model.Value = new ObservableCollection<SelectInfoModel>();
                    selectModel.Code = configData.MDVRID;
                    selectModel.Type = SettingType.Vehicle;
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
                    IsOK++;
                    if (IsOK == 4)
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        //private void config_ModifyGpsSettingsCompleted(object sender, ModifyGpsSettingsCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Result.IsSuccess)
        //        {
        //            SelectInfoModel selectModel = new SelectInfoModel();
        //            ServiceReference.MessageService.GpsSendUpModel model = new ServiceReference.MessageService.GpsSendUpModel();
        //            model.Value = new ObservableCollection<SelectInfoModel>();
        //            selectModel.Code = configData.MDVRID;
        //            selectModel.Type = SettingType.Vehicle;
        //            model.Setting = new SettingGpsSendUpCMD();
        //            model.Setting.SendTime = DateTime.Now;
        //            model.Setting.RuleName = toDefaultGps.Gps_RuleID;
        //            model.Setting.IsUsing = (int)toDefaultGps.Gps_IfMonitor;
        //            model.Setting.SendNum = toDefaultGps.Gps_UploadSum;
        //            model.Setting.DistanceValue = toDefaultGps.Gps_Distance;
        //            model.Setting.TimeValue = toDefaultGps.Gps_Time;
        //            model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
        //            ApplicationContext.Instance.MessageManager.SendSettingGpsCMD(model);
        //            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
        //        }
        //        ConfigInfoList.MoveToPage(currentIndex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }      
        //}


        private void config_GetDefaultAbnormalDoorInfoCompleted(object sender, GetDefaultAbnormalDoorInfoCompletedEventArgs e)
        {
            try
            {
                IEnumerable<AbnormalDoorRuleInfo> toDefaultAbnormalDoorEnum = e.Result.Result;
                IList<AbnormalDoorRuleInfo> toDefaultAbnormalDoorList;
                toDefaultAbnormalDoorList = (IList<AbnormalDoorRuleInfo>)toDefaultAbnormalDoorEnum;
                foreach (var item in toDefaultAbnormalDoorList)
                {
                    toDefaultAbnormalDoor.ID = configData.AbnormalDoor_RuleID;
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
                SelectInfoModel selectModel = new SelectInfoModel();
                ServiceReference.MessageService.AbnormalDoorSendUpModel model = new ServiceReference.MessageService.AbnormalDoorSendUpModel();
                model.Value = new ObservableCollection<SelectInfoModel>();
                selectModel.Code = configData.MDVRID;
                selectModel.Type = SettingType.Vehicle;
                model.Setting = new SettingAbnormalDoorCMD();
                model.Setting.SendTime = DateTime.Now;
                model.OperationType = RuleOperationType.Default;
                model.Setting.SendValue = abnormalDoorInfo.Speed.ToString();
                model.Setting.RuleName = abnormalDoorInfo.ID;
                ApplicationContext.Instance.MessageManager.SendAbnormalDoorSettingCMD(model);
                IsOK++;
                if (IsOK == 4)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        //private void config_UpdateExistAbnormalDoorRuleCompleted(object sender, UpdateExistAbnormalDoorRuleCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Result.IsSuccess)
        //        {
        //            AbnormalDoorRuleInfo abnormalDoorInfo = new AbnormalDoorRuleInfo();
        //            SelectInfoModel selectModel = new SelectInfoModel();
        //            ServiceReference.MessageService.AbnormalDoorSendUpModel model = new ServiceReference.MessageService.AbnormalDoorSendUpModel();
        //            model.Value = new ObservableCollection<SelectInfoModel>();
        //            selectModel.Code = configData.MDVRID;
        //            selectModel.Type = SettingType.Vehicle;
        //            model.Setting = new SettingAbnormalDoorCMD();
        //            model.Setting.SendTime = DateTime.Now;
        //            model.OperationType = RuleOperationType.Default;
        //            model.Setting.SendValue = abnormalDoorInfo.Speed.ToString();
        //            model.Setting.RuleName = abnormalDoorInfo.ID;
        //            ApplicationContext.Instance.MessageManager.SendAbnormalDoorSettingCMD(model);
        //        }
        //        ConfigInfoList.MoveToPage(currentIndex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void config_GetDefaultTemperatureInfoCompleted(object sender, GetDefaultTemperatureInfoCompletedEventArgs e)
        {
            try
            {
                IEnumerable<TemperatureRuleInfo> toDefaultTemperatureEnum = e.Result.Result;
                IList<TemperatureRuleInfo> toDefaultTemperatureList;
                toDefaultTemperatureList = (IList<TemperatureRuleInfo>)toDefaultTemperatureEnum;
                foreach (var item in toDefaultTemperatureList)
                {
                    toDefaultTemperature.ID = configData.Temperature_RuleID;
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
                SelectInfoModel selectModel = new SelectInfoModel();
                ServiceReference.MessageService.TemperatureSendUpModel model = new ServiceReference.MessageService.TemperatureSendUpModel();
                model.Value = new ObservableCollection<SelectInfoModel>();
                selectModel.Code = configData.MDVRID;
                selectModel.Type = SettingType.Vehicle;
                model.Setting = new SettingTemperatureCMD();
                model.Setting.SendTime = DateTime.Now;
                model.Setting.RuleName = toDefaultTemperature.ID;
                model.OperationType = RuleOperationType.Add;
                model.Setting.SendType = (TemperatureMarkType)CurrentSettingType.Code;
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
                IsOK++;
                if (IsOK == 4)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        //private void config_UpdateTemperatureRuleCompleted(object sender, UpdateTemperatureRuleCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Result.IsSuccess)
        //        {
        //            TemperatureRuleInfo temperatureInfo = new TemperatureRuleInfo();
        //            SelectInfoModel selectModel = new SelectInfoModel();
        //            ServiceReference.MessageService.TemperatureSendUpModel model = new ServiceReference.MessageService.TemperatureSendUpModel();
        //            model.Value = new ObservableCollection<SelectInfoModel>();
        //            selectModel.Code = configData.MDVRID;
        //            selectModel.Type = SettingType.Vehicle;
        //            model.Setting = new SettingTemperatureCMD();
        //            model.Setting.SendTime = DateTime.Now;
        //            model.Setting.RuleName = temperatureInfo.ID;
        //            model.OperationType = RuleOperationType.Add;
        //            model.Setting.SendType = (TemperatureMarkType)_currentSettingtype.Code;
        //            if (temperatureInfo.SettingType == ServiceReference.CommandManageService.TemperatureSettingType.EnableAndSettingBehindValue)
        //            {
        //                model.Setting.TemperatureType = temperatureInfo.TemperatureType;
        //                model.Setting.MaxValue = temperatureInfo.HighTemperature;
        //                model.Setting.MinValue = temperatureInfo.HighTemperature;
        //            }
        //            else
        //            {
        //                model.Setting.MaxValue = null;
        //                model.Setting.MinValue = null;
        //                model.Setting.TemperatureType = null;
        //            }
        //            ApplicationContext.Instance.MessageManager.SendTemperatureSettingCMD(model);
        //        }
        //        ConfigInfoList.MoveToPage(currentIndex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void Query()
        {
            ConfigInfoList.MoveToFirstPage();
        }
    }
}