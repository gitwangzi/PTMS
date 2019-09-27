using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using System;
using System.Collections.Generic;
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
    [ExportAsViewModel(ManagerName.DetailAlarmSettingsViewModel)]
    public class DetailsAlarmSettingsVm : BaseEntityViewModel
    {
        #region
        public ICommand ReturnCommand { get; private set; }
        public string Title { get; private set; }
        public AlarmSettingRules alarmSettingData { get; set; }
        public string Alarm_RuleName { get; set; }
        public string Alarm_ButtonTime { get; set; }
        public string Alarm_Normal { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateTime { get; set; }
        public string RefreshTime { get; set; }
        public string Alarm_Comments { get; set; }
        #endregion
        public DetailsAlarmSettingsVm()
        {
            try
            {
                ReturnCommand = new ActionCommand<object>(obj => Return());
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                Title = ApplicationContext.Instance.StringResourceReader.GetString("Look");
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                alarmSettingData = viewParameters["Detail"] as Gsafety.PTMS.ServiceReference.CommandManageService.AlarmSettingRules;
                InitialPage(alarmSettingData);
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
                EventAggregator.Publish(new ViewNavigationArgs(ManagerName.AlarmSettingView, new Dictionary<string, object>() { }));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void InitialPage(AlarmSettingRules alarmSettingData)
        {
            try
            {
                Alarm_RuleName = alarmSettingData.Alarm_RuleName;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_RuleName));
                Alarm_ButtonTime = alarmSettingData.Alarm_ButtonTime.ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_ButtonTime));
                Alarm_Normal = alarmSettingData.Alarm_Normal.ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_Normal));
                Creator = alarmSettingData.Alarm_Creator;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Creator));
                CreateTime = alarmSettingData.Alarm_CreateTime;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CreateTime));
                Alarm_Comments = alarmSettingData.Alarm_Description;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_Comments));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
