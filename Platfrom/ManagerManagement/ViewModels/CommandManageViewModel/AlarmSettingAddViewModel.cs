using Gsafety.PTMS.BasicPage.VehicleSelect;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
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
    [ExportAsViewModel(ManagerName.AlarmSettingAddViewModel)]
    public class AlarmSettingAddViewModel:BaseEntityViewModel
    {
        CommandManageServiceClient alarmSettingAddClient ;
        #region properties and command       
        public string Alarm_Description { get; set; }
        AlarmSettingRules alarmAddRules = new AlarmSettingRules();
        public string Title { get; private set; }
        public AlarmSettingRules alarmData { get; set; }
        public ICommand ReturnCommand{ get;private set;}
        public ICommand FinshCommand { get;private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand SaveAndSettingCommand { get; private set; }
        public Visibility alarmSettingVisibility { get; set; }
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
                if (!string.IsNullOrEmpty(Alarm_RuleName) && !string.IsNullOrEmpty(Alarm_RuleName))
                {
                    FinishEnabled = true;
                }
                else
                    FinishEnabled = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
            }
        }

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

        public bool IsSettingToVehcile { get; set; }

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
                //if (!string.IsNullOrEmpty(Alarm_RuleName) && !string.IsNullOrEmpty(Alarm_RuleName))
                //{
                //    FinishEnabled = true;
                //}
                //else
                //    FinishEnabled = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
            }
        }

        static private int buttonTime=2;
        private string _Alarm_ButtonTime ;
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
        //private string pattern = @"^[0-9]*$";
        //private string param1 = null;
        //private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    Match m = Regex.Match(this.buttontime.Text, pattern);
        //    if (!m.Success)
        //    {
        //        this.buttontime.Text = param1;
        //        this.buttontime.SelectionStart = this.buttontime.Text.Length;
        //    }
        //    else
        //    {
        //        param1 = this.buttontime.Text;
        //    }
        //}

        private string _Alarm_Normal;
        public string Alarm_Normal
        {
            get { return _Alarm_Normal; }
            set
            {
                _Alarm_Normal = value;
                ValidateNumbersNormal(ExtractPropertyName(() => Alarm_Normal), _Alarm_Normal);                     
                if (!string.IsNullOrEmpty(Alarm_RuleName))
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
            if (string.IsNullOrEmpty(value))
            {
               
            }
            else if (!Regex.Match(value, "^[0-9]*$").Success)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Number"));
            }
            else if (Regex.Match(value, "^[0-9]*$").Success)
            {
                if (!(int.Parse(value) > 0 && int.Parse(value) < 60))
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_NumberIn0To60"));
            }
         }
      
        #endregion
        public AlarmSettingAddViewModel()
        {
            try
            {
                alarmSettingAddClient = ServiceClientFactory.Create<CommandManageServiceClient>();
                _VehicleSelectVM = new VehicleSelectViewModelold();
                IsSettingToVehcile = false;
                ReturnCommand = new ActionCommand<object>(obj => Return());
                FinshCommand = new ActionCommand<object>(obj => Finsh());
                ResetCommand = new ActionCommand<object>(obj => Reset());
                SaveAndSettingCommand = new ActionCommand<object>(obj => SaveAndSetting());
                alarmSettingAddClient.CheckAlarmExistCompleted += alarmSettingAddClient_CheckAlarmExistCompleted;
                alarmSettingAddClient.CheckAlarmidExistCompleted += alarmSettingAddClient_CheckAlarmidExistCompleted;
                alarmSettingAddClient.AlarmSettingAddCompleted += alarmSettingAddClient_AlarmSettingAddCompleted;
                //alarmSettingAddClient.ModifyAlarmSettingsCompleted += alarmSettingAddClient_ModifyAlarmSettingsCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object>viewParameters)
        {
            try
            {
                //_VehicleSelectVM.InitTree();
                base.ActivateView(viewName, viewParameters);
                action = viewParameters["action"].ToString();
                switch (action)
                {
                    case "Add":
                        alarmSettingVisibility = Visibility.Visible;
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => alarmSettingVisibility));
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_AlarmSettingAdd");
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                        IsRead = false;
                        Alarm_ButtonTime = buttonTime.ToString();
                        VehicleSelectVM.InitTree();
                        VehicleSelectVM.TreeViewVisible = Visibility.Visible;
                        toEmpty();
                        break;
                    case "ToVehicle":
                        alarmSettingVisibility = Visibility.Collapsed;
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => alarmSettingVisibility));
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Manager_toVehicle");
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                        IsRead = true;
                        VehicleSelectVM.InitTree();
                        VehicleSelectVM.TreeViewVisible = Visibility.Visible;
                        alarmData = viewParameters["ToVehicle"] as Gsafety.PTMS.ServiceReference.CommandManageService.AlarmSettingRules;
                        InitialPage(alarmData);
                        break;
                }
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
            EventAggregator.Publish(new ViewNavigationArgs(ManagerName.AlarmSettingView, new Dictionary<string, object> { { "action", "return" } }));
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
                IsSettingToVehcile = false;
                if (action == "Add")
                {
                    if((IsNumber(ExtractPropertyName(() => Alarm_ButtonTime), _Alarm_ButtonTime) && IsNumber(ExtractPropertyName(() => Alarm_Normal), _Alarm_Normal)))
                    {
                       if (!string.IsNullOrEmpty(Alarm_RuleName))
                       {
                           if (int.Parse(Alarm_ButtonTime) > 0 && int.Parse(Alarm_ButtonTime) < 20 && int.Parse(Alarm_Normal) > 0 && int.Parse(Alarm_Normal) < 60)
                           {
                               alarmSettingAddClient.CheckAlarmExistAsync(Alarm_RuleName);
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
              
                if (action == "Modify")
                {
                    if (!string.IsNullOrEmpty(Alarm_RuleName))
                    {
                        alarmSettingAddClient.CheckAlarmidExistAsync(Alarm_RuleName, alarmData.Alarm_RuleID);                                              
                    }
                    else if (string.IsNullOrEmpty(Alarm_RuleName))
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_SettingNameISNULL"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                }
             
                if (action == "ToVehicle")
                {
                    IsSettingToVehcile = true;
                    SelectModels = VehicleSelectVM.GetSelectModel();
                    if (SelectModels.Count != 0)
                    {
                        ServiceReference.MessageService.OneKeyAlarmSendUpModel model = new ServiceReference.MessageService.OneKeyAlarmSendUpModel();
                        model.DelayAlarmSetting = new SettingDelayAlarmCMD();
                        model.Value = new ObservableCollection<SelectInfoModel>();
                        model.Setting = new SettingOneKeyAlarmCMD();
                        model.Setting.RuleName = alarmData.Alarm_RuleName;
                        model.Setting.SendTime = DateTime.Now;
                        model.OperationType = RuleOperationType.Add;
                        model.Setting.SendValue =int.Parse(Alarm_ButtonTime);
                        model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        model.DelayAlarmSetting.SendValue = 2;
                        model.DelayAlarmSetting.OneKeyDelayTime = (int)alarmData.Alarm_Normal;
                        model.DelayAlarmSetting.SendTime = DateTime.Now;
                        model.DelayAlarmSetting.RuleID = alarmData.Alarm_RuleID;
                        model.DelayAlarmSetting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        model.Value = SelectModels;
                        model.DelayAlarmSetting.SendTime = DateTime.Now;
                        ApplicationContext.Instance.MessageManager.SendSettingOneKeyAlarmUploadCMD(model);
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);                   
                    }          
                }                             
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        public void SaveAndSetting()
        {
            try
            {
                IsSettingToVehcile = true;
                if (action == "Add")
                {
                    if (!string.IsNullOrEmpty(Alarm_RuleName))
                    {
                        alarmSettingAddClient.CheckAlarmExistAsync(Alarm_RuleName);                        
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_SettingNameISNULL"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                }               
                if (action == "ToVehicle")
                {
                    IsSettingToVehcile = true;
                    SelectModels = VehicleSelectVM.GetSelectModel();
                    if (SelectModels.Count != 0)
                    {
                        ServiceReference.MessageService.OneKeyAlarmSendUpModel model = new ServiceReference.MessageService.OneKeyAlarmSendUpModel();
                        model.DelayAlarmSetting = new SettingDelayAlarmCMD();
                        model.Value = new ObservableCollection<SelectInfoModel>();
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
                        model.Value = SelectModels;
                        ApplicationContext.Instance.MessageManager.SendSettingOneKeyAlarmUploadCMD(model);
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                }
              
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }        
        }
        public void Reset()
        {
            try
            {              
                if (action == "ToVehicle")
                {
                    VehicleSelectVM.InitTree();
                    VehicleSelectVM.TreeViewVisible = Visibility.Visible;
                }
                else if (action == "Add")
                {
                    toEmpty();
                    VehicleSelectVM.InitTree();
                    VehicleSelectVM.TreeViewVisible = Visibility.Visible;
                }                        
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void alarmSettingAddClient_CheckAlarmExistCompleted(object sender, CheckAlarmExistCompletedEventArgs e)
        {
            try
            {
                if (e.Result.Result==true)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Name_IsExist"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                else 
                {
                    if (action == "Add")
                    {
                        alarmAddRules.Alarm_RuleID = Guid.NewGuid().ToString();
                        alarmAddRules.Alarm_RuleName = Alarm_RuleName;
                        alarmAddRules.Alarm_ButtonTime = short.Parse(Alarm_ButtonTime);
                        alarmAddRules.Alarm_Normal = int.Parse(Alarm_Normal);
                        alarmAddRules.Alarm_Description = Alarm_Description;
                        alarmAddRules.Alarm_Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        if (short.Parse(Alarm_ButtonTime) > 0 && short.Parse(Alarm_ButtonTime) < 20 && int.Parse(Alarm_Normal) > 0 && int.Parse(Alarm_Normal) < 60)
                        {
                            alarmSettingAddClient.AlarmSettingAddAsync(alarmAddRules);
                        }
                        else
                        {
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_ButtonTime));
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Alarm_Normal)); 
                           MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                        }
                    }         
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }
        void alarmSettingAddClient_CheckAlarmidExistCompleted(object sender, CheckAlarmidExistCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Name_IsExist"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                else
                {
                    //if (action == "Modify")
                    //{
                    //    alarmAddRules.Alarm_RuleID = alarmData.Alarm_RuleID;
                    //    alarmAddRules.Alarm_RuleName = Alarm_RuleName;
                    //    alarmAddRules.Alarm_ButtonTime = short.Parse(Alarm_ButtonTime);
                    //    alarmAddRules.Alarm_Normal = int.Parse(Alarm_Normal);
                    //    alarmAddRules.Alarm_Description = Alarm_Description;
                    //    alarmAddRules.Alarm_Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                    //    alarmSettingAddClient.ModifyAlarmSettingsAsync(alarmAddRules);   
                    //}
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        void alarmSettingAddClient_AlarmSettingAddCompleted(object sender,AlarmSettingAddCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    SelectModels = VehicleSelectVM.GetSelectModel();
                    if (SelectModels.Count != 0 && IsSettingToVehcile)
                    {
                        ServiceReference.MessageService.OneKeyAlarmSendUpModel model = new ServiceReference.MessageService.OneKeyAlarmSendUpModel();
                        model.DelayAlarmSetting = new SettingDelayAlarmCMD();
                        model.Value = new ObservableCollection<SelectInfoModel>();
                        model.Setting = new SettingOneKeyAlarmCMD();
                        model.Setting.RuleName = alarmAddRules.Alarm_RuleID;
                        model.Setting.SendTime = DateTime.Now;
                        model.Setting.SendValue = (int)alarmAddRules.Alarm_ButtonTime;
                        model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        model.DelayAlarmSetting.SendValue = buttonTime;
                        model.DelayAlarmSetting.OneKeyDelayTime = (int)alarmAddRules.Alarm_Normal;
                        model.DelayAlarmSetting.SendTime = DateTime.Now;
                        model.DelayAlarmSetting.RuleID = alarmAddRules.Alarm_RuleID;
                        model.DelayAlarmSetting.SendTime = DateTime.Now;
                        model.DelayAlarmSetting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        model.Value = SelectModels;
                        ApplicationContext.Instance.MessageManager.SendSettingOneKeyAlarmUploadCMD(model);
                        toEmpty();
                    }
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    finishEnabled = false;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
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
            this.Alarm_RuleName = string.Empty;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => this.Alarm_RuleName));
            this.Alarm_ButtonTime = buttonTime.ToString();
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => this.Alarm_ButtonTime));
            this.Alarm_Normal = string.Empty;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => this.Alarm_Normal));
            this.Alarm_Description = string.Empty;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => this.Alarm_Description));
        }
    }
}
