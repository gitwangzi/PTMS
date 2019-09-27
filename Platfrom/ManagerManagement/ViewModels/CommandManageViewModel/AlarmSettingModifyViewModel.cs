/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: c1ec8f4a-cd9f-4535-a7a3-cf7ccd82a90a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
/////    Project Description:    
/////             Class Name: AlarmSettingModifyViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/12 17:32:04
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/12 17:32:04
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
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.Share;
using Jounce.Framework.ViewModel;
using System.Text.RegularExpressions;
using Jounce.Framework.Command;
using Jounce.Core.View;
using System.Collections.Generic;
using Jounce.Core.ViewModel;
using System.Reflection;

namespace Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
{
     [ExportAsViewModel(ManagerName.AlarmSettingModifyViewModel)]
    public class AlarmSettingModifyViewModel : BaseEntityViewModel
    {
        CommandManageServiceClient alarmSettingAddClient;
        #region
        public string Alarm_Description { get; set; }
        public string Title { get; private set; }
        public AlarmSettingRules alarmData { get; set; }
        public ICommand ReturnCommand { get; private set; }
        public ICommand FinshCommand { get; private set; }
        AlarmSettingRules alarmAddRules = new AlarmSettingRules();
        private bool _IsRead;
        public bool IsRead
        {
            get { return _IsRead; }
            set
            {
                _IsRead = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsRead));
            }
        }
        private string _Alarm_RuleName;
        public string Alarm_RuleName
        {
            get { return _Alarm_RuleName; }
            set
            {
                _Alarm_RuleName = value == null ? null : value.Trim();
                ValidateGpsLoginName(ExtractPropertyName(() => Alarm_RuleName), _Alarm_RuleName);               
                if (!string.IsNullOrEmpty(Alarm_RuleName))
                {
                    FinishEnabled = true;
                }
                else
                    FinishEnabled = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
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
        private bool finishEnabled = false;
        public bool FinishEnabled
        {
            get { return finishEnabled; }
            set
            {
                finishEnabled = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
            }
        }
        private string _Alarm_Normal;
        public string Alarm_Normal
        {
            get { return _Alarm_Normal; }
            set
            {
                _Alarm_Normal = value;
                ValidateNumbersNormal(ExtractPropertyName(() => Alarm_Normal), _Alarm_Normal);                          
                if (!string.IsNullOrEmpty(Alarm_RuleName) && !string.IsNullOrEmpty(Alarm_RuleName))
                {
                    FinishEnabled = true;
                }
                else
                    FinishEnabled = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_Normal));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
            }
        }
        void ValidateNumbersNormal(string prop, string value)
        {
            ClearErrors(prop);
           if (!Regex.Match(value, "^[0-9]*$").Success)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Number"));
            }
            else if (Regex.Match(value, "^[0-9]*$").Success)
            {
                if (!(int.Parse(value) > 0 && int.Parse(value) < 60))
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_NumberIn0To60"));
            }
        }
       
        static private int buttonTime = 2;
        private string _Alarm_ButtonTime = buttonTime.ToString();
        public string Alarm_ButtonTime
        {
            get
            {
                return _Alarm_ButtonTime;
            }
            set
            {
                _Alarm_ButtonTime = value;
                ValidateNumbers(ExtractPropertyName(() => Alarm_ButtonTime), _Alarm_ButtonTime);                             
                if (!string.IsNullOrEmpty(Alarm_RuleName) && !string.IsNullOrEmpty(Alarm_RuleName))
                {
                    FinishEnabled = true;
                }
                else
                    FinishEnabled = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_ButtonTime));
            }
        }
        void ValidateNumbers(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MustFilled"));
            }
            else if (!Regex.Match(value, "^[0-9]*$").Success)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Number"));
            }
            else if (Regex.Match(value, "^[0-9]*$").Success)
            {
                if (!(int.Parse(value) > 0 && int.Parse(value) < 20))
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_NumberIn0To20"));
            }
        }
        #endregion

        public AlarmSettingModifyViewModel()
        {
            try
            {
                alarmSettingAddClient = ServiceClientFactory.Create<CommandManageServiceClient>();
                ReturnCommand = new ActionCommand<object>(obj => Return());
                FinshCommand = new ActionCommand<object>(obj => Finsh());
                alarmSettingAddClient.ModifyAlarmSettingsCompleted += alarmSettingAddClient_ModifyAlarmSettingsCompleted;
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
                Title = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_ModifyAlarmSeting");
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                IsRead = false;
                base.ActivateView(viewName, viewParameters);
                alarmData = viewParameters["Modify"] as Gsafety.PTMS.ServiceReference.CommandManageService.AlarmSettingRules;
                InitialPage(alarmData);
                //switch (action)
                //{             
                //    case "Modify":
                //        Title = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_ModifyAlarmSeting");
                //        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                //        IsRead = false;                  
                //        alarmData = viewParameters["Modify"] as Gsafety.PTMS.ServiceReference.CommandManageService.AlarmSettingRules;
                //        InitialPage(alarmData);
                //        break;               
                //}
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        public void InitialPage(AlarmSettingRules alarmData)
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
        public void Return()
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
        private bool IsNumber(string prop, string value)
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
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void Finsh()
        {
            try
            {
                if ((IsNumber(ExtractPropertyName(() => Alarm_ButtonTime), _Alarm_ButtonTime) && IsNumber(ExtractPropertyName(() => Alarm_Normal), _Alarm_Normal)))
                {
                    if (!string.IsNullOrEmpty(Alarm_RuleName))
                    {
                        if (int.Parse(Alarm_ButtonTime) > 0 && int.Parse(Alarm_ButtonTime) < 20 && int.Parse(Alarm_Normal) > 0 && int.Parse(Alarm_Normal) < 60)
                        {
                            alarmAddRules.Alarm_RuleID = alarmData.Alarm_RuleID;
                            alarmAddRules.Alarm_RuleName = Alarm_RuleName;
                            alarmAddRules.Alarm_ButtonTime = short.Parse(Alarm_ButtonTime);
                            alarmAddRules.Alarm_Normal = int.Parse(Alarm_Normal);
                            alarmAddRules.Alarm_Description = Alarm_Description;
                            alarmAddRules.Alarm_Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                            alarmSettingAddClient.ModifyAlarmSettingsAsync(alarmAddRules);
                        }
                        else
                        {
                            ValidateNumbers(ExtractPropertyName(() => Alarm_ButtonTime), _Alarm_ButtonTime);
                            ValidateNumbersNormal(ExtractPropertyName(() => Alarm_Normal), _Alarm_Normal);
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_ButtonTime));
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_Normal));
                            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                        }
                    }
                    else if (string.IsNullOrEmpty(Alarm_RuleName))
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_SettingNameISNULL"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        void alarmSettingAddClient_ModifyAlarmSettingsCompleted(object sender, ModifyAlarmSettingsCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
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
    }
}
