using BaseLib.Model;
using BaseLib.ViewModels;
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
    [ExportAsViewModel(ManagerName.AlarmSettingViewModel)]
    public class AlarmSettingVm:BaseViewModel
    {
        CommandManageServiceClient alarmSettingClient;
        #region properties and Command
        int currentIndex = 1;
        public IActionCommand QueryCommand { get; set; }
        public IActionCommand AddCommand { get; set; }
        public IActionCommand ModifyCommand { get; set; }
        public IActionCommand DeleteCommand { get; set; }
        public IActionCommand DetailCommand { get; set; }
        public IActionCommand DefaultCommand { get; set; }
        public IActionCommand ToVehicleCommand { get; set; }
        public PagedServerCollection<AlarmSettingRules> alarmSettingList{get;set;}
        AlarmSettingRules defaultAlarm = new AlarmSettingRules();
        public string ruleName { get; set; }       
        public int? buttonTime { get; set; }
        public int? overSpeed { get; set; }
        public int? normalSpeed { get; set; }
        public DateTime? createTime { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get
            {
                return pageSizeValue;
            }
            set
            {
                pageSizeValue = value;
                if (this.alarmSettingList != null)
                {
                    this.alarmSettingList.PageSize = value;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageSizeValue));
            }
        }
        private AlarmSettingRules _alarmSettingData;
        public AlarmSettingRules alarmSettingData
        {
            get 
            {
                return _alarmSettingData;
            }
            set
            {
                _alarmSettingData = value;
            }
        }

        public List<int> PageSizeList{ get;set; }

        #endregion
       
        public AlarmSettingVm()
        {
            try
            {
                alarmSettingClient = ServiceClientFactory.Create<CommandManageServiceClient>();
                alarmSettingClient.AlarmSettingCompleted += alarmSettingClient_AlarmSettingCompleted;
                alarmSettingClient.DeleteAlarmSettingCompleted += alarmSettingClient_DeleteAlarmSettingCompleted;
                PageSizeList = new List<int> { 20, 40, 80 };
                QueryCommand = new ActionCommand<object>(obj => Query());
                AddCommand = new ActionCommand<object>(obj => Publish("Add"));
                ModifyCommand = new ActionCommand<object>(obj => Modify("Modify"));
                DetailCommand = new ActionCommand<object>(obj => Detail("Detail"));
                DeleteCommand = new ActionCommand<object>(obj => Delete());
                DefaultCommand = new ActionCommand<object>(obj => Default());
                ToVehicleCommand = new ActionCommand<object>(obj => ToVehicle("ToVehicle"));  
                PageSizeValue = PageSizeList[0]; 
                InitPagedServerCollection();
                
               
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void InitPagedServerCollection()
        {
            alarmSettingList = new PagedServerCollection<AlarmSettingRules>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);             
                alarmSettingClient.AlarmSettingAsync(ruleName, new PagingInfo { PageIndex = pageIndex, PageSize = pageSize });
            });           
        }  

        private void Publish(string name)
        {
            EventAggregator.Publish(new ViewNavigationArgs(ManagerName.AlarmSettingAddView, new Dictionary<string, object>() { { "action", name }, { name, alarmSettingData } }));
        }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                if (viewParameters.Count!=0 && viewParameters["action"].ToString()=="return")
                {
                    alarmSettingList.MoveToPage(currentIndex);
                }              
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void alarmSettingClient_AlarmSettingCompleted(object sender,AlarmSettingCompletedEventArgs e)
        {
            try
            {
                alarmSettingList.loader_Finished(new PagedResult<AlarmSettingRules>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex
                });            
                if (e.Result.Result.Count==0)
                {
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AlarmSettingVm", ex); ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }       
        }
        void alarmSettingClient_DeleteAlarmSettingCompleted(object sender, DeleteAlarmSettingCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    alarmSettingList.ToPage(currentIndex);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void Detail(string name)
        {
            EventAggregator.Publish(new ViewNavigationArgs(ManagerName.DetailAlarmSettingsView, new Dictionary<string, object>() { { name, alarmSettingData } }));
        }

        public void Delete()
        {
            try
            {
                if (alarmSettingData.Alarm_VehcileCount > 0)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Used"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OKCancel);
                }
                else
                {
                    var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ConfirmDelete"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        try
                        {
                            alarmSettingClient.DeleteAlarmSettingAsync(alarmSettingData.Alarm_RuleName);
                        }
                        catch (Exception ex)
                        {
                            ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void Default()
        {
            try
            {
                if (MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_Rule_DelAllRelation"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    alarmSettingClient.GetDefaultAlarmSettingAsync();
                    alarmSettingClient.GetDefaultAlarmSettingCompleted += alarmSettingClient_GetDefaultAlarmSettingCompleted;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        public void alarmSettingClient_GetDefaultAlarmSettingCompleted( object sender, GetDefaultAlarmSettingCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    if (e.Result.Result != null)
                    {
                        ServiceReference.MessageService.OneKeyAlarmSendUpModel model = new ServiceReference.MessageService.OneKeyAlarmSendUpModel();
                        model.DelayAlarmSetting = new SettingDelayAlarmCMD();
                        model.Value = new ObservableCollection<SelectInfoModel>();
                        model.Setting = new SettingOneKeyAlarmCMD();
                        model.Setting.RuleName = alarmSettingData.Alarm_RuleID;
                        model.Setting.SendTime = DateTime.Now;
                        model.OperationType = RuleOperationType.Default;
                        model.Setting.SendValue = (short)e.Result.Result.Alarm_ButtonTime;
                        model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        model.DelayAlarmSetting.SendValue = 2;
                        model.DelayAlarmSetting.OneKeyDelayTime = (int)e.Result.Result.Alarm_Normal;
                        model.DelayAlarmSetting.SendTime = DateTime.Now;
                        model.DelayAlarmSetting.RuleID = alarmSettingData.Alarm_RuleID;
                        model.DelayAlarmSetting.SendTime = DateTime.Now;
                        model.DelayAlarmSetting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        ApplicationContext.Instance.MessageManager.SendSettingOneKeyAlarmUploadCMD(model);
                        alarmSettingList.MoveToPage(currentIndex);              
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("FAIL"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                }
                else
                {
                    throw e.Error;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void config_ModifyAlarmSettingsCompleted(object sender, ModifyAlarmSettingsCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    ServiceReference.MessageService.OneKeyAlarmSendUpModel model = new ServiceReference.MessageService.OneKeyAlarmSendUpModel();
                    model.DelayAlarmSetting = new SettingDelayAlarmCMD();
                    model.Value = new ObservableCollection<SelectInfoModel>();
                    model.Setting = new SettingOneKeyAlarmCMD();
                    model.Setting.RuleName = defaultAlarm.Alarm_RuleID;
                    model.Setting.SendTime = DateTime.Now;
                    model.OperationType = RuleOperationType.Default;
                    model.Setting.SendValue = (short)defaultAlarm.Alarm_ButtonTime;
                    model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                    model.DelayAlarmSetting.SendValue = 2;
                    model.DelayAlarmSetting.OneKeyDelayTime = (int)defaultAlarm.Alarm_Normal;
                    model.DelayAlarmSetting.SendTime = DateTime.Now;
                    model.DelayAlarmSetting.RuleID = defaultAlarm.Alarm_RuleID;
                    model.DelayAlarmSetting.SendTime = DateTime.Now;
                    model.DelayAlarmSetting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                    ApplicationContext.Instance.MessageManager.SendSettingOneKeyAlarmUploadCMD(model);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }

        public void ToVehicle(string name)
        {
            try
            {
                EventAggregator.Publish(new ViewNavigationArgs(ManagerName.AlarmSettingToVehicleView, new Dictionary<string, object>() { { name, alarmSettingData } }));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }  
        private void Modify(string name)
        {
            try
            {
                if (alarmSettingData.Alarm_VehcileCount > 0)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Used"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OKCancel);
                }
                else
                {
                    EventAggregator.Publish(new ViewNavigationArgs(ManagerName.AlarmSettingModifyView, new Dictionary<string, object>() { { name, alarmSettingData } }));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        public void Query()
        {
            try
            {
                alarmSettingList.MoveToFirstPage();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
