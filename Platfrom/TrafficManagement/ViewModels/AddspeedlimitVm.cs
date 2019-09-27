/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6ed88328-4c51-404d-9b91-7a65b103a9e0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.ViewModels
/////    Project Description:    
/////             Class Name: AddspeedlimitVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/2 8:37:54
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/2 8:37:54
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
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.Share;
using Jounce.Framework.Command;
using Jounce.Core.View;
using System.Collections.Generic;
using Gsafety.PTMS.BasicPage.VehicleSelect;
using System.Collections.ObjectModel;
using Jounce.Framework.ViewModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Gsafety.PTMS.Traffic.ViewModels
{
    [ExportAsViewModel(TrafficName.AddspeedlimitViewModel)]
    public class AddspeedlimitVm : BaseEntityViewModel
    {
        #region
        private string _RuleName;
        public string RuleName
        {
            get
            {
                return _RuleName;
            }
            set
            {
                _RuleName = value == null ? null : value.Trim();
                ValidateSpeedLoginName(ExtractPropertyName(() => RuleName), _RuleName);
                if (!string.IsNullOrEmpty(RuleName))
                {
                    FinishEnabled = true;
                }
                else
                    FinishEnabled = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RuleName));
            }
        }

        private void ValidateSpeedLoginName(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MustFilled"));
            }
        }

        private string _MaxSpeed;
        public string MaxSpeed
        {
            get
            {
                return _MaxSpeed;
            }
            set
            {
                _MaxSpeed = value;
                ValidateInputeSpeed(ExtractPropertyName(() => MaxSpeed), _MaxSpeed);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MaxSpeed));

            }
        }

        private void ValidateInputeSpeed(string prop, string value)
        {
            ClearErrors(prop);
            Match match = Regex.Match(value, @"^(0|[1-9]?[0-9]\d*|0)$");
            if (!match.Success)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Number"));
            }
            else if (int.Parse(match.Value) > 201 || int.Parse(match.Value) < 1)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_InputValidMaxSpeed"));
            }
        }

        private string _Duration;
        public string Duration
        {
            get
            {
                return _Duration;
            }
            set
            {
                _Duration = value;
                ValidateInputeDuration(ExtractPropertyName(() => Duration), _Duration);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Duration));
            }
        }
        private void ValidateInputeDuration(string prop, string value)
        {
            ClearErrors(prop);
            Match match = Regex.Match(value, @"^(0|[1-9]?[0-9]\d*|0)$");
            if (!match.Success)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Number"));
            }
            else if (int.Parse(match.Value) > 1801 || int.Parse(match.Value) < 1)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_InputValidDuration"));
            }
        }
        private bool _ruleName = false;
        public bool ruleName
        {
            get
            {
                return _ruleName;
            }
            set
            {
                _ruleName = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ruleName));
            }
        }
        private bool _maxSpeed = false;
        public bool maxSpeed
        {
            get
            {
                return _maxSpeed;
            }
            set
            {
                _maxSpeed = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => maxSpeed));
            }
        }
        private bool _duration = false;
        public bool duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => duration));
            }
        }

        private bool _ruleNameEnable = true;
        public bool ruleNameEnable
        {
            get
            {
                return _ruleNameEnable;
            }
            set
            {
                _ruleNameEnable = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ruleNameEnable));
            }
        }

        private bool _FinishEnabled = false;
        public bool FinishEnabled
        {
            get
            {
                return _FinishEnabled;
            }
            set
            {
                _FinishEnabled = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
            }
        }
        public string VehicleID { get; set; }
        public string Title { get; set; }
        public SpeedLimit speedInfo { get; set; }
        SpeedLimit speedTemp = new SpeedLimit();
        public ICommand FinishCommand { get; set; }
        public ICommand ReturnCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand SaveAndSettingCommand { get; set; }
        public int minSpeed = 0;
        private VehicleSelectViewModelold _VehicleSelectVM;
        public VehicleSelectViewModelold VehicleSelectVM
        {
            get { return _VehicleSelectVM; }
            set { _VehicleSelectVM = value; }
        }
        ObservableCollection<Gsafety.PTMS.ServiceReference.MessageService.SelectInfoModel> selectModels;
        public ObservableCollection<Gsafety.PTMS.ServiceReference.MessageService.SelectInfoModel> SelectModels
        {
            get { return selectModels; }
            set { selectModels = value; }
        }
        public bool IsSettingToVehcile { get; set; }
        #endregion
        TrafficManageServiceClient client = null;
        public AddspeedlimitVm()
        {
            try
            {
                client = ServiceClientFactory.Create<TrafficManageServiceClient>();

                client.AddSpeedLimitCompleted += client_AddSpeedLimitCompleted;
                client.CheckSpeedLimitidNameExistCompleted += client_CheckSpeedLimitidNameExistCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            _VehicleSelectVM = new VehicleSelectViewModelold();
            FinishCommand = new ActionCommand<object>(obj => Finish());
            ReturnCommand = new ActionCommand<object>(obj => Return("SpeedRulesView"));
            ResetCommand = new ActionCommand<object>(obj => Reset());
            SaveAndSettingCommand = new ActionCommand<object>(obj => SaveAndSetting());
            
        }


        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            _VehicleSelectVM.InitTree();
            action = viewParameters["action"].ToString();
            switch (action)
            {
                case "Add":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_Add_SpeedLimit");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                    ruleName = false;
                    maxSpeed = false;
                    duration = false;
                    ruleNameEnable = true;
                    toEmpty();
                    VehicleSelectVM.TreeViewVisible = Visibility.Visible;
                    break;
                case "toVehicle":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_SpeedRuleToVehicle");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                    speedInfo = viewParameters["toVehicle"] as Gsafety.PTMS.ServiceReference.TrafficManageService.SpeedLimit;
                    InitialPage(speedInfo);
                    ruleName = true;
                    maxSpeed = true;
                    duration = true;
                    ruleNameEnable = false;
                    break;
            }
        }


        private void Return(string name)
        {
            try
            {
                toEmpty();
                EventAggregator.Publish(new ViewNavigationArgs(TrafficName.SpeedRulesView, new Dictionary<string, object>() { { "action", name } }));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void client_AddSpeedLimitCompleted(object sender, AddSpeedLimitCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    SelectModels = VehicleSelectVM.GetSelectModel();
                    if (SelectModels.Count != 0 && IsSettingToVehcile)
                    {
                        ServiceReference.MessageService.OverSpeedSendSettingModel model = new ServiceReference.MessageService.OverSpeedSendSettingModel();
                        model.Setting = new SettingOverSpeedCMD();
                        model.Value = new ObservableCollection<Gsafety.PTMS.ServiceReference.MessageService.SelectInfoModel>();
                        model.Value = SelectModels;
                        model.Setting.RuleName = speedTemp.ID;
                        model.Setting.SendTime = DateTime.Now;
                        model.Setting.Duration = speedTemp.DURATION.ToString();
                        model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        model.Setting.MaxSpeed = speedTemp.MAX_SPEED.ToString();
                        model.Setting.MinSpeed = minSpeed.ToString();
                        model.Setting.OverSpeedID = speedTemp.ID;
                        model.Setting.OperType = 1;
                        ApplicationContext.Instance.MessageManager.SendSettingOverSpeedUploadCMD(model);
                        IsSettingToVehcile = false;
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Operation_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                    else if (SelectModels.Count == 0)
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Operation_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void client_UpdateVehicleSpeedStateBySpeedIDAndCarNumCompleted(object sender, UpdateVehicleSpeedStateBySpeedIDAndCarNumCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void client_CheckSpeedLimitNameExistCompleted(object sender, CheckSpeedLimitNameExistCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    if (e.Result.Result == true)
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_ExsitSpeedName"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                    else
                    {
                        if (action == "Add")
                        {
                            InfoToDB();
                            speedTemp.ID = Guid.NewGuid().ToString();
                            client.AddSpeedLimitAsync(speedTemp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void client_CheckSpeedLimitidNameExistCompleted(object sender, CheckSpeedLimitidNameExistCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    if (e.Result.Result)
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_ExsitSpeedName"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                    else
                    {
                        if (action == "repair")
                        {
                            InfoToDB();
                            speedTemp.ID = speedInfo.ID;
                            client.UpdateVehicleSpeedStateBySpeedIDAndCarNumAsync(speedTemp);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        public void Finish()
        {
            try
            {
                IsSettingToVehcile = false;
                if (action == "Add")
                {
                    if (IsNumber(ExtractPropertyName(() => MaxSpeed), _MaxSpeed) && IsNumberDuration(ExtractPropertyName(() => Duration), _Duration))
                    {
                        if (!string.IsNullOrEmpty(RuleName) && int.Parse(MaxSpeed) > 0 && int.Parse(MaxSpeed) < 201 && int.Parse(Duration) > 0 && int.Parse(Duration) < 1801)
                        {
                            client.CheckSpeedLimitNameExistCompleted -= client_CheckSpeedLimitNameExistCompleted;
                            client.CheckSpeedLimitNameExistCompleted += client_CheckSpeedLimitNameExistCompleted;
                            client.CheckSpeedLimitNameExistAsync(RuleName);
                        }
                        else if (string.IsNullOrEmpty(RuleName))
                        {
                            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_SettingNameISNULL"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                        }
                        else if (!string.IsNullOrEmpty(RuleName))
                        {
                            if (int.Parse(MaxSpeed) < 1 || int.Parse(MaxSpeed) > 200 || int.Parse(Duration) < 1 || int.Parse(Duration) > 1800)
                            {
                                ValidateInputeDuration(ExtractPropertyName(() => Duration), _Duration);
                                ValidateInputeSpeed(ExtractPropertyName(() => MaxSpeed), _MaxSpeed);
                                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MaxSpeed));
                                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Duration));
                                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
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
            toEmpty();
            //_VehicleSelectVM.InitTree();
            _VehicleSelectVM.FilterText = string.Empty;
            VehicleSelectVM.TreeViewVisible = Visibility.Visible;
        }

        public void SaveAndSetting()
        {
            try
            {
                IsSettingToVehcile = true;
                if (action == "Add")
                {
                    if (IsNumber(ExtractPropertyName(() => MaxSpeed), _MaxSpeed) && IsNumberDuration(ExtractPropertyName(() => Duration), _Duration))
                    {
                        SelectModels = VehicleSelectVM.GetSelectModel();
                        if (!string.IsNullOrEmpty(RuleName) && int.Parse(MaxSpeed) > 0 && int.Parse(MaxSpeed) < 201 && int.Parse(Duration) > 0 && int.Parse(Duration) < 1801)
                        {
                            client.CheckSpeedLimitNameExistCompleted -= client_CheckSpeedLimitNameExistCompleted;
                            client.CheckSpeedLimitNameExistCompleted += client_CheckSpeedLimitNameExistCompleted;
                            client.CheckSpeedLimitNameExistAsync(RuleName);
                        }
                        else if (string.IsNullOrEmpty(RuleName))
                        {
                            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_SettingNameISNULL"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                        }
                        else if (!string.IsNullOrEmpty(RuleName))
                        {
                            if (int.Parse(MaxSpeed) < 1 || int.Parse(MaxSpeed) > 200 || int.Parse(Duration) < 1 || int.Parse(Duration) > 1800)
                            {
                                ValidateInputeDuration(ExtractPropertyName(() => Duration), _Duration);
                                ValidateInputeSpeed(ExtractPropertyName(() => MaxSpeed), _MaxSpeed);
                                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MaxSpeed));
                                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Duration));
                                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }

                }
            }

            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void toEmpty()
        {
            RuleName = string.Empty;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RuleName));
            MaxSpeed = string.Empty;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MaxSpeed));
            Duration = string.Empty;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Duration));
        }
        private void InfoToDB()
        {
            speedTemp.NAME = RuleName;
            speedTemp.MAX_SPEED = int.Parse(MaxSpeed);
            speedTemp.MIN_SPEED = 0;
            speedTemp.DURATION = int.Parse(Duration);
            speedTemp.CreateTime = DateTime.Now;
            speedTemp.Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
            speedTemp.START_TIME = DateTime.Today;
            speedTemp.END_TIME = DateTime.Today;
            speedTemp.Valid = 1;
        }
        void InitialPage(SpeedLimit speedlimit)
        {
            speedInfo.ID = speedlimit.ID;
            RuleName = speedlimit.NAME;
            MaxSpeed = speedlimit.MAX_SPEED.ToString();
            Duration = speedlimit.DURATION.ToString();
        }
        #region number requirement
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
                else if (int.Parse(match.Value) > 201 || int.Parse(match.Value) < 1)
                {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_InputValidMaxSpeed"));
                    return false;
                }
                else return true;

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
                return false;
            }
        }

        private bool IsNumberDuration(string prop, string value)
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
                else if (int.Parse(match.Value) > 1801 || int.Parse(match.Value) < 1)
                {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_InputValidDuration"));
                    return false;
                }
                else return true;

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
                return false;
            }

        }
    }
        #endregion
}
