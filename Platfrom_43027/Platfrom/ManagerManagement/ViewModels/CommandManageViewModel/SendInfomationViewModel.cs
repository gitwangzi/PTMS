/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6109ae3e-4261-4dfb-a916-81d0c934b9a2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
/////    Project Description:    
/////             Class Name: TemperatureAddRuleViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/30 17:51:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/30 17:51:14
/////            Modified by:
/////   Modified Description: 
/////======================================================================
/////          Modified Time: 2014/11/16 09:11:56
/////            Modified by: GJSY(zhoudd)
/////   Modified Description:  
/////======================================================================
/////          Modified Time: 2014/11/17 10:08:09
/////            Modified by: GJSY(zhoudd)
/////   Modified Description:  
/////======================================================================
using Gsafety.PTMS.Manager.Models;
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
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Reflection;

namespace Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
{
    [ExportAsViewModel(ManagerName.SendInfomationVM)]
    public class SendInfomationViewModel : BaseEntityViewModel
    {
        #region Field

        CommandManageServiceClient addCommandManageService = null;
        CommandManageServiceClient editCommandManageService = null;
        Gsafety.PTMS.ServiceReference.CommandManageService.AbnormalDoorRuleInfo CurrentRuleInfo;
        private bool _IsCanCommit = false;
        private string _Title;
        private bool _EnableEdit = true;
        private string _ruleName;
        private Visibility _AddOrEditButtonVisible = Visibility.Collapsed;
        private Visibility _SettingButtonVisible = Visibility.Collapsed;
        private Visibility _DetailButtonVisible = Visibility.Collapsed;
        private string _speed;
        string _UserDescription = string.Empty;
        ObservableCollection<SelectInfoModel> selectModels;
        private Gsafety.PTMS.BasicPage.VehicleSelect.VehicleSelectViewModelold _VehicleSelectVM;

        #endregion

        #region Command

        public ICommand ReturnCommand { get; private set; }
        public ICommand FinishCommand { get; private set; }
        public ICommand SaveSettingCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand SettingCommand { get; private set; }

        #endregion

        #region Property
        public ObservableCollection<SelectInfoModel> SelectModels
        {
            get { return selectModels; }
            set { selectModels = value; }
        }

        public ViewParameters CurrentAction { get; set; }

        public Gsafety.PTMS.BasicPage.VehicleSelect.VehicleSelectViewModelold VehicleSelectVM
        {
            get { return _VehicleSelectVM; }
            set { _VehicleSelectVM = value; }
        }

        public Visibility AddOrEditButtonVisible
        {
            get
            {
                return _AddOrEditButtonVisible;
            }
            set
            {
                _AddOrEditButtonVisible = value;
                RaisePropertyChanged("AddOrEditButtonVisible");
            }
        }
        public Visibility SettingButtonVisible
        {
            get
            {
                return _SettingButtonVisible;
            }
            set
            {
                _SettingButtonVisible = value;
                RaisePropertyChanged("SettingButtonVisible");
            }
        }
        public Visibility DetailButtonVisible
        {
            get
            {
                return _DetailButtonVisible;
            }
            set
            {
                _DetailButtonVisible = value;
                RaisePropertyChanged("DetailButtonVisible");
            }
        }

        public string RuleName
        {
            get { return _ruleName; }
            set
            {
                _ruleName = value;
                RaisePropertyChanged("RuleName");
                ValidateRuleName();
            }
        }
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                RaisePropertyChanged("Title");
            }
        }
        public string Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                RaisePropertyChanged("Speed");
                ValidateInputeSpeed(ExtractPropertyName(() => Speed), _speed);
            }
        }
        public string UserDescription
        {
            get { return _UserDescription; }
            set
            {
                _UserDescription = value;
                RaisePropertyChanged("UserDescription");
            }
        }

        public bool IsEditSaveSetting { get; set; }
        public bool IsAddSaveSetting { get; set; }
        public bool EnableEdit
        {
            get { return _EnableEdit; }
            set
            {
                _EnableEdit = value;
                RaisePropertyChanged("EnableEdit");
            }
        }
        public bool IsCanCommit
        {
            get { return _IsCanCommit; }
            set
            {
                _IsCanCommit = value;
                RaisePropertyChanged("IsCanCommit");
            }
        }

        #endregion

        #region Ctor
        public SendInfomationViewModel()
        {
            try
            {
                //addCommandManageService = ServiceClientFactory.Create<CommandManageServiceClient>();
                //editCommandManageService = ServiceClientFactory.Create<CommandManageServiceClient>();

                _VehicleSelectVM = new Gsafety.PTMS.BasicPage.VehicleSelect.VehicleSelectViewModelold();
                _VehicleSelectVM.InitTree();
                //CurrentRuleInfo = new AbnormalDoorRuleInfo();

                ReturnCommand = new ActionCommand<object>((obj) => Return());
                SaveSettingCommand = new ActionCommand<object>(obj => SaveSetting());
                ResetCommand = new ActionCommand<object>(obj => Reset());
                SettingCommand = new ActionCommand<object>(obj => OnSetting());

                //addCommandManageService.AddAbnormalDoorRuleCompleted += addCommandManageService_AddAbnormalDoorRuleCompleted;
                //addCommandManageService.IsExistAbnormalDoorRuleCompleted += addCommandManageService_IsExistAbnormalDoorRuleCompleted;
                //addCommandManageService.UpdateExistAbnormalDoorRuleCompleted += addCommandManageService_UpdateExistAbnormalDoorRuleCompleted;

                //editCommandManageService.UpdateExistAbnormalDoorRuleCompleted += editCommandManageService_UpdateExistAbnormalDoorRuleCompleted;
                //editCommandManageService.IsExistAbnormalDoorRuleForUpdateCompleted += editCommandManageService_IsExistAbnormalDoorRuleForUpdateCompleted;
                //editCommandManageService.GetAllVehicleRuleRelationCompleted += editCommandManageService_GetAllVehicleRuleRelationCompleted;


                //CurrentAction = new ViewParameters();
                //IsEditSaveSetting = false;
                //IsAddSaveSetting = false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        #endregion

        #region Wcf Event

        #region Add Event
        private void addCommandManageService_UpdateExistAbnormalDoorRuleCompleted(object sender, UpdateExistAbnormalDoorRuleCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    CurrentAction.IsRefresh = false;
                    throw e.Error;
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("更新失败！"), ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Setting_Tip"), MessageBoxButton.OK);
                }
                else
                {
                    CurrentAction.IsRefresh = true;
                    if (IsEditSaveSetting)
                    {
                        ServiceReference.MessageService.AbnormalDoorSendUpModel model = new ServiceReference.MessageService.AbnormalDoorSendUpModel();
                        model.Value = new ObservableCollection<SelectInfoModel>();
                        model.Setting = new SettingAbnormalDoorCMD();
                        model.Setting.SendTime = DateTime.Now;
                        model.OperationType = RuleOperationType.Add;
                        model.Setting.SendValue = CurrentRuleInfo.Speed.ToString();
                        model.Setting.RuleName = CurrentRuleInfo.ID;
                        model.Value = SelectModels;
                        model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        ApplicationContext.Instance.MessageManager.SendAbnormalDoorSettingCMD(model);
                    }
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("更新成功！"), ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Setting_Tip"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void addCommandManageService_IsExistAbnormalDoorRuleCompleted(object sender, IsExistAbnormalDoorRuleCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    CurrentAction.IsRefresh = false;
                    throw e.Error;
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("添加失败！"), ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Setting_Tip"), MessageBoxButton.OK);
                }
                if (e.Result.IsSuccess)
                {
                    if (!e.Result.Result)
                    {
                        CurrentRuleInfo.Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        CurrentRuleInfo.RuleName = RuleName;
                        CurrentRuleInfo.Speed = int.Parse(Speed);
                        CurrentRuleInfo.UserDescription = UserDescription;
                        CurrentRuleInfo.ID = Guid.NewGuid().ToString();
                        addCommandManageService.AddAbnormalDoorRuleAsync(CurrentRuleInfo);
                    }
                    else
                    {
                        SetError("RuleName", ApplicationContext.Instance.StringResourceReader.GetString("名称已经存在"));
                        IsCanCommit = !HasErrors;

                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void addCommandManageService_AddAbnormalDoorRuleCompleted(object sender, AddAbnormalDoorRuleCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    CurrentAction.IsRefresh = false;
                    throw e.Error;
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("添加失败！"), ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Setting_Tip"), MessageBoxButton.OK);
                }
                if (e.Result.IsSuccess)
                {
                    if (e.Result.Result)
                    {
                        CurrentAction.IsRefresh = true;
                        if (IsAddSaveSetting)
                        {
                            ServiceReference.MessageService.AbnormalDoorSendUpModel model = new ServiceReference.MessageService.AbnormalDoorSendUpModel();
                            model.Value = new ObservableCollection<SelectInfoModel>();
                            model.Setting = new SettingAbnormalDoorCMD();
                            model.Setting.SendTime = DateTime.Now;
                            model.OperationType = RuleOperationType.Add;
                            model.Setting.SendValue = CurrentRuleInfo.Speed.ToString();
                            model.Setting.RuleName = CurrentRuleInfo.ID;
                            model.Value = SelectModels;
                            model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                            ApplicationContext.Instance.MessageManager.SendAbnormalDoorSettingCMD(model);
                        }
                        Reset();
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Setting_AddSuccessful"), ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Setting_Tip"), MessageBoxButton.OK);
                    }
                    else
                    {
                        CurrentAction.IsRefresh = false;
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Setting_AddFailed"), ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Setting_Tip"), MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        void editCommandManageService_GetAllVehicleRuleRelationCompleted(object sender, GetAllVehicleRuleRelationCompletedEventArgs e)
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
        #endregion

        #region Edit Event
        private void editCommandManageService_IsExistAbnormalDoorRuleForUpdateCompleted(object sender, IsExistAbnormalDoorRuleForUpdateCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    CurrentAction.IsRefresh = false;
                    throw e.Error;
                }
                else
                {
                    if (e.Result.IsSuccess)
                    {
                        if (e.Result.Result)
                        {
                            SetError("RuleName", ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Name_IsExist"));
                            IsCanCommit = !HasErrors;
                        }
                        else
                        {
                            CurrentRuleInfo.Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                            CurrentRuleInfo.RuleName = RuleName;
                            CurrentRuleInfo.Speed = int.Parse(Speed);
                            CurrentRuleInfo.UserDescription = UserDescription;
                            editCommandManageService.UpdateExistAbnormalDoorRuleAsync(CurrentRuleInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void editCommandManageService_UpdateExistAbnormalDoorRuleCompleted(object sender, UpdateExistAbnormalDoorRuleCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    CurrentAction.IsRefresh = false;
                    throw e.Error;
                }
                else
                {
                    if (e.Result.Result)
                    {
                        CurrentAction.IsRefresh = true;
                        if (IsEditSaveSetting)
                        {
                            ServiceReference.MessageService.AbnormalDoorSendUpModel model = new ServiceReference.MessageService.AbnormalDoorSendUpModel();
                            model.Value = new ObservableCollection<SelectInfoModel>();
                            model.Setting = new SettingAbnormalDoorCMD();
                            model.Setting.SendTime = DateTime.Now;
                            model.OperationType = RuleOperationType.Add;
                            model.Setting.SendValue = CurrentRuleInfo.Speed.ToString();
                            model.Setting.RuleName = CurrentRuleInfo.ID;
                            model.Value = SelectModels;
                            ApplicationContext.Instance.MessageManager.SendAbnormalDoorSettingCMD(model);
                        }
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Edit_Success"), ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Setting_Tip"), MessageBoxButton.OK);
                    }
                    else
                    {
                        CurrentAction.IsRefresh = false;
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Edit_Faild"), ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Setting_Tip"), MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        #endregion

        #endregion

        #region Command
        public void Return()
        {
            try
            {
                CurrentAction.Action = ActionType.Back;
                EventAggregator.Publish(new ViewNavigationArgs(ManagerName.AbnormalDoorSettingView, new Dictionary<string, object>() { { "Parameters", CurrentAction } }));

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void SaveSetting()
        {
            try
            {
                SelectModels = VehicleSelectVM.GetSelectModel();
                if (selectModels.Count <= 0)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Select_Vhicles"), ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Setting_Tip"), MessageBoxButton.OK);
                    return;
                }
                if (!HasErrors)
                {
                    switch (CurrentAction.Action)
                    {
                        case ActionType.Add:
                            IsEditSaveSetting = false;
                            IsAddSaveSetting = true;
                            addCommandManageService.IsExistAbnormalDoorRuleAsync(RuleName);
                            break;
                        case ActionType.Edit:
                            IsEditSaveSetting = true;
                            IsAddSaveSetting = false;
                            editCommandManageService.IsExistAbnormalDoorRuleForUpdateAsync(RuleName, CurrentRuleInfo.ID);
                            break;
                        case ActionType.Delete:
                            break;
                        case ActionType.Detail:
                            break;
                        case ActionType.Back:
                            break;
                        case ActionType.SettingToVehicle:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void OnSetting()
        {
            try
            {
                SelectModels = VehicleSelectVM.GetSelectModel();
                if (selectModels.Count <= 0)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Select_Vhicles"), ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Setting_Tip"), MessageBoxButton.OK);
                    return;
                }
                ServiceReference.MessageService.SendInfomationModel model = new ServiceReference.MessageService.SendInfomationModel();
                model.Value = new ObservableCollection<SelectInfoModel>();
                model.Setting = new SendInfomationCMD();
                model.OperationType = RuleOperationType.Add;
                model.Setting.SendTime = DateTime.Now;
                model.Value = SelectModels;
                model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;

                model.Setting.DispatchType = SendInfomationType.Car;
                model.Setting.DisplayPosition = new ObservableCollection<DisplayPositionType>();
                model.Setting.DisplayPosition.Add(DisplayPositionType.CarAfterbody);
                model.Setting.DisplayPosition.Add(DisplayPositionType.CarBroadside);
                model.Setting.ManualControl = false;
                model.Setting.SendContent = UserDescription;

                model.Setting.DisplayTime = -1;

                //CurrentAction.IsRefresh = true;
                ApplicationContext.Instance.MessageManager.SendInfomationCMD(model);
                if (string.IsNullOrEmpty(model.Setting.SendContent))
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("PleaseInputSendInfo"), ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Setting_Tip"), MessageBoxButton.OK);
                }
                else
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_CmdSendSuccessful"), ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Setting_Tip"), MessageBoxButton.OK);

               
            }
            catch (Exception ex)
            {
                CurrentAction.IsRefresh = true;
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        protected override void OnCommitted()
        {
            try
            {
                IsAddSaveSetting = false;
                IsEditSaveSetting = false;
                switch (CurrentAction.Action)
                {
                    case ActionType.Add:
                        addCommandManageService.IsExistAbnormalDoorRuleAsync(RuleName);
                        break;
                    case ActionType.Edit:
                        editCommandManageService.IsExistAbnormalDoorRuleForUpdateAsync(RuleName, CurrentRuleInfo.ID);
                        break;
                    case ActionType.Delete:
                        break;
                    case ActionType.Detail:
                        break;
                    case ActionType.Back:
                        break;
                    case ActionType.SettingToVehicle:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        #endregion

        #region ValidateInput
        private void ValidateRuleName()
        {
            var propName = ExtractPropertyName(() => RuleName);
            ClearErrors(propName);
            ValidateNotNull(propName, _ruleName);
        }
        private bool ValidateNotNull(string prop, string value)
        {
            bool isCheckedPass = true;
            if (string.IsNullOrEmpty(value))
            {
                isCheckedPass = false;
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("ValueNotNull"));
            }
            IsCanCommit = !HasErrors;
            return isCheckedPass;
        }
        private void ValidateInputeSpeed(string prop, string value)
        {
            ClearErrors(prop);
            if (ValidateNotNull(prop, value))
            {
                Match match = Regex.Match(value, @"^(0|[1-9]?[0-9]\d*|0)$");
                if (!match.Success)
                {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Speed_Validate"));
                }
                else
                {
                    if (int.Parse(match.Value) > 50)
                    {
                        SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Speed_Validate"));
                    }
                }
            }
            IsCanCommit = !HasErrors;
        }
        protected override void ValidateAll()
        {
            ValidateRuleName();
            ValidateInputeSpeed(ExtractPropertyName(() => Speed), _speed);
        }

        #endregion

        #region Method
        void Reset()
        {
            try
            {
                EnableEdit = true;
                CurrentRuleInfo = new AbnormalDoorRuleInfo();
                RuleName = string.Empty;
                Speed = string.Empty;
                UserDescription = string.Empty;
                VehicleSelectVM.InitTree();
                VehicleSelectVM.TreeViewVisible = Visibility.Visible;
                ValidateAll();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void AddOrEditInit(object obj, bool enableInput, bool isShowTree)
        {
            try
            {
                AbnormalDoorRuleInfo parameters = obj as AbnormalDoorRuleInfo;
                CurrentRuleInfo = parameters;
                RuleName = CurrentRuleInfo.RuleName;
                Speed = CurrentRuleInfo.Speed.ToString();
                UserDescription = CurrentRuleInfo.UserDescription;
                ValidateAll();
                EnableEdit = enableInput;
                if (isShowTree)
                {
                    VehicleSelectVM.TreeViewVisible = Visibility.Visible;
                }
                else
                {
                    VehicleSelectVM.TreeViewVisible = Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Override
        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            //try
            //{
            //    IsEditSaveSetting = false;
            //    IsAddSaveSetting = false;
            SettingButtonVisible = Visibility.Visible;
            //    AddOrEditButtonVisible = Visibility.Collapsed;
            //    DetailButtonVisible = Visibility.Collapsed;
            //    base.ActivateView(viewName, viewParameters);
            //    CurrentAction = new ViewParameters();
            //    if (viewParameters.Keys.Contains("Parameters"))
            //    {
            //        CurrentAction = viewParameters["Parameters"] as ViewParameters;
            //    }

            //    switch (CurrentAction.Action)
            //    {
            //        case ActionType.Add:
            //            Title = ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_AbnormalDoor_Add");
            //            AddOrEditButtonVisible = Visibility.Visible;
            //            Reset();
            //            break;
            //        case ActionType.Edit:
            //            Title = ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_AbnorrmalDoor_Update");
            //            AddOrEditButtonVisible = Visibility.Visible;
            //            VehicleSelectVM.InitTree();
            //            AddOrEditInit(CurrentAction.Parameters, true, true);
            //            break;
            //        case ActionType.Delete:
            //            break;
            //        case ActionType.Detail:
            //            DetailButtonVisible = Visibility.Visible;
            //            AddOrEditInit(CurrentAction.Parameters, false, false);
            //            break;
            //        case ActionType.Back:
            //            break;
            //        case ActionType.SettingToVehicle:
            //            Title = ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_AbnorrmalDoor_SetVehicle");
            //            SettingButtonVisible = Visibility.Visible;
            //            if (CurrentAction.Parameters != null)
            //            {
            //                //取得所有和当前规则关联的车辆
            //                //然后在初始化到树里
            //                AddOrEditInit(CurrentAction.Parameters, false, true);
            //                editCommandManageService.GetAllVehicleRuleRelationAsync(CurrentRuleInfo.ID, Gsafety.PTMS.ServiceReference.CommandManageService.RuleType.AbnormalDoor);
            //                VehicleSelectVM.InitTree();

            //            }
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            //}
        }
        #endregion
    }
}
