/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 19730ef0-e750-4a9a-a932-811ef9f2df37      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.ViewModels
/////    Project Description:    
/////             Class Name: ModifySpeedVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/23 13:35:30
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/23 13:35:30
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
using ESRI.ArcGIS.Client.Actions;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using Jounce.Framework.ViewModel;
using Jounce.Framework.Command;
using System.Text.RegularExpressions;
using Jounce.Core.View;
using System.Collections.Generic;
using Jounce.Core.ViewModel;
using System.Reflection;

namespace Gsafety.PTMS.Traffic.ViewModels
{
    [ExportAsViewModel(TrafficName.ModifyspeedlimitViewModel)]
    public class ModifySpeedVm : BaseEntityViewModel
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
                _RuleName = value;
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
        private void ValidateInputeSpeed(string prop, string value)
        {
            ClearErrors(prop);
            Match match = Regex.Match(value, @"^(0|[1-9]?[0-9]\d*|0)$");
            if (!match.Success)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Number"));
            }
            else
            {
                if (int.Parse(match.Value) > 200 || int.Parse(match.Value) < 1)
                {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_InputValidMaxSpeed"));
                }
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
            else
            {
                if (int.Parse(match.Value) > 1800 || int.Parse(match.Value) < 1)
                {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_InputValidDuration"));
                }
            }
        }
        public string Title { get; set; }
        public SpeedLimit speedInfo { get; set; }
        SpeedLimit speedTemp = new SpeedLimit();
        public ICommand FinishCommand { get; set; }
        public ICommand ReturnCommand { get; set; }
        #endregion
        TrafficManageServiceClient client = null;
        public ModifySpeedVm()
        {
            try
            {
                client = ServiceClientFactory.Create<TrafficManageServiceClient>();
                client.UpdateVehicleSpeedStateBySpeedIDAndCarNumCompleted += client_UpdateVehicleSpeedStateBySpeedIDAndCarNumCompleted;
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            FinishCommand = new ActionCommand<object>(obj => Finish());
            ReturnCommand = new ActionCommand<object>(obj => Return("SpeedRulesView"));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MaxSpeed));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Duration));
        }
        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();
            switch (action)
            {
                case "repair":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_SpeedRuleModify");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                    speedInfo = viewParameters["repair"] as Gsafety.PTMS.ServiceReference.TrafficManageService.SpeedLimit;
                    InitialPage(speedInfo);                  
                    break;
            }
        }
        void InitialPage(SpeedLimit speedlimit)
        {
            speedInfo.ID = speedlimit.ID;
            RuleName = speedlimit.NAME;
            MaxSpeed = speedlimit.MAX_SPEED.ToString();
            Duration = speedlimit.DURATION.ToString();
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
            speedTemp.ID = speedInfo.ID;
        }
        private void client_UpdateVehicleSpeedStateBySpeedIDAndCarNumCompleted(object sender, UpdateVehicleSpeedStateBySpeedIDAndCarNumCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ModifySuccess"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    OverSpeedSendSettingModel model =new OverSpeedSendSettingModel();
                    model.OperationType = RuleOperationType.Default;
                    model.Setting = new SettingOverSpeedCMD();
                    model.Setting.RuleName = speedTemp.ID;
                    model.Setting.SendTime = DateTime.Now;
                    model.Setting.OverSpeedID = speedTemp.ID;
                    model.Setting.OperType = 2;
                    model.Setting.MinSpeed = "0";
                    model.Setting.MaxSpeed = MaxSpeed;
                    model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                    model.Setting.Duration = speedTemp.DURATION.ToString();
                 
                    ApplicationContext.Instance.MessageManager.SendSettingOverSpeedUploadCMD(model);
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
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
                return false;
            }
        }
        public void Finish()
        {
            try
            {
                if (IsNumber(ExtractPropertyName(() => MaxSpeed), _MaxSpeed) && IsNumber(ExtractPropertyName(() => Duration), _Duration))
                {
                    if (int.Parse(MaxSpeed) > 0 && int.Parse(MaxSpeed) < 201 && int.Parse(Duration) > 0 && int.Parse(Duration) < 1801)
                    {
                        InfoToDB();
                    
                        client.UpdateVehicleSpeedStateBySpeedIDAndCarNumAsync(speedTemp);
                    }                
                    else   if (int.Parse(MaxSpeed) < 1 || int.Parse(MaxSpeed) > 200 || int.Parse(Duration) < 1 || int.Parse(Duration) > 1800)
                        {
                            ValidateInputeDuration(ExtractPropertyName(() => Duration), _Duration);
                            ValidateInputeSpeed(ExtractPropertyName(() => MaxSpeed), _MaxSpeed);
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MaxSpeed));
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Duration));
                            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                        }
                    }                 
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
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
    }
}
