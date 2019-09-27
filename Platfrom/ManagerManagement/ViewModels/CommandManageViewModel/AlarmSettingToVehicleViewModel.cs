/////Copyright (C) Gsafety 2015 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4e06c567-035c-4e90-8760-3dd3b438d400      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
/////    Project Description:    
/////             Class Name: AlarmSettingToVehicleViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2015/1/13 16:30:31
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2015/1/13 16:30:31
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
using Jounce.Core.ViewModel;
using Jounce.Framework.ViewModel;
using Jounce.Framework.Command;
using Jounce.Core.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Gsafety.PTMS.BasicPage.VehicleSelect;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using System.Reflection;

namespace Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
{
    [ExportAsViewModel(ManagerName.AlarmSettingToVehicleViewModel)]
    public class AlarmSettingToVehicleViewModel : BaseEntityViewModel
    {
        private CommandManageServiceClient addCommandManageService = null;

        #region 
        public ICommand SetCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand ReturnCommand { get; set; }
        public AlarmSettingRules alarmData { get; set; }
        public string Title { get; set; }
        ObservableCollection<SelectInfoModel> selectModels;
        public ObservableCollection<SelectInfoModel> SelectModels
        {
            get { return selectModels; }
            set { selectModels = value; }
        }

        private VehicleSelectViewModelold _VehicleSelectVM;
        public VehicleSelectViewModelold VehicleSelectVM
        {
            get { return _VehicleSelectVM; }
            set { _VehicleSelectVM = value; }
        }
        private string _Alarm_RuleName;
        public string Alarm_RuleName
        {
            get { return _Alarm_RuleName; }
            set
            {
                _Alarm_RuleName = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_RuleName));
            }
        }
        private string _Alarm_ButtonTime;
        public string Alarm_ButtonTime
        {
            get
            {
                return _Alarm_ButtonTime;
            }
            set
            {
                _Alarm_ButtonTime = value;               
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_ButtonTime));
            }
        }
        private string _Alarm_Normal;
        public string Alarm_Normal
        {
            get
            {
                return _Alarm_Normal;
            }
            set
            {
                _Alarm_Normal = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_Normal));
            }
        }
        private string _Alarm_Description;
        public string Alarm_Description
        {
            get
            {
                return _Alarm_Description;
            }
            set
            {
                _Alarm_Description = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_Description));
            }
        }
        #endregion

        public AlarmSettingToVehicleViewModel()
        {
            try
            {
                addCommandManageService = ServiceClientFactory.Create<CommandManageServiceClient>();

                _VehicleSelectVM = new VehicleSelectViewModelold();
                SetCommand = new ActionCommand<object>(obj => Set());
                ResetCommand = new ActionCommand<object>(obj => Reset());
                ReturnCommand = new ActionCommand<object>(obj => Return());
                addCommandManageService.GetAllVehicleRuleRelationCompleted += addCommandManageService_GetAllVehicleRuleRelationCompleted;

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void addCommandManageService_GetAllVehicleRuleRelationCompleted(object sender, GetAllVehicleRuleRelationCompletedEventArgs e)
        {
            if (e.Result.IsSuccess)
            {
                if (e.Result.Result != null)
                {
                    VehicleRuleRelation result = e.Result.Result;
                    VehicleSelectVM.InitTree(result.Vehicles);
                }
            }
        }
          private string action;
          protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
          {
              try
              {
                  _VehicleSelectVM.InitTree();
                  base.ActivateView(viewName, viewParameters);
                  Title = ApplicationContext.Instance.StringResourceReader.GetString("Manager_toVehicle");
                  Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                  alarmData = viewParameters["ToVehicle"] as Gsafety.PTMS.ServiceReference.CommandManageService.AlarmSettingRules;
                  addCommandManageService.GetAllVehicleRuleRelationAsync(alarmData.Alarm_RuleID, Gsafety.PTMS.ServiceReference.CommandManageService.RuleType.OneKeyAlarm);
                  VehicleSelectVM.InitTree();
                  VehicleSelectVM.TreeViewVisible = Visibility.Visible;
                  InitialPage(alarmData);
              }
              catch (Exception ex)
              {
                  ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
              }
          }
        public void InitialPage(AlarmSettingRules alarmData)
        {
            try
            {
                Alarm_RuleName = alarmData.Alarm_RuleName;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_RuleName));
                Alarm_ButtonTime = alarmData.Alarm_ButtonTime.ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_ButtonTime));
                Alarm_Normal = alarmData.Alarm_Normal.ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_Normal));
                Alarm_Description = alarmData.Alarm_Description;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_Description));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void Set()
        {
            try
            {
                SelectModels = VehicleSelectVM.GetSelectModel();
                if (SelectModels.Count != 0)
                {
                    ServiceReference.MessageService.OneKeyAlarmSendUpModel model = new ServiceReference.MessageService.OneKeyAlarmSendUpModel();
                    model.DelayAlarmSetting = new SettingDelayAlarmCMD();
                    model.Value = new ObservableCollection<SelectInfoModel>();
                    model.Value = selectModels;
                    model.Setting = new SettingOneKeyAlarmCMD();
                    model.Setting.RuleName = alarmData.Alarm_RuleID;
                    model.Setting.SendTime = DateTime.Now;
                    model.OperationType = RuleOperationType.Add;
                    model.Setting.SendValue = int.Parse(Alarm_ButtonTime);
                    model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                    model.DelayAlarmSetting.SendValue = 2;
                    model.DelayAlarmSetting.OneKeyDelayTime = (int)alarmData.Alarm_Normal;
                    model.DelayAlarmSetting.SendTime = DateTime.Now;
                    model.DelayAlarmSetting.RuleID = alarmData.Alarm_RuleID;
                    model.DelayAlarmSetting.SendTime = DateTime.Now;
                    model.DelayAlarmSetting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                    ApplicationContext.Instance.MessageManager.SendSettingOneKeyAlarmUploadCMD(model);
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void Reset()
        {
            try
            {
                VehicleSelectVM.InitTree();
                VehicleSelectVM.TreeViewVisible = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void Return()
        {
            try
            {
                EventAggregator.Publish(new ViewNavigationArgs(ManagerName.AlarmSettingView, new Dictionary<string, object> { { "action", "return" } }));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
