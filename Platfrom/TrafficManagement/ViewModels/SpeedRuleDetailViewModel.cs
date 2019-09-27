/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0a4f6204-bd85-4444-8a6c-19ac56703acc      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.ViewModels
/////    Project Description:    
/////             Class Name: SpeedRuleDetailViewModelcs
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/4 17:03:20
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/4 17:03:20
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
using Gsafety.PTMS.Share;
using Jounce.Framework.Command;
using Jounce.Core.View;
using System.Collections.Generic;
using System.Reflection;

namespace Gsafety.PTMS.Traffic.ViewModels
{
    [ExportAsViewModel(TrafficName.SpeedRuleDetailViewModel)]
    public class SpeedRuleDetailViewModel:BaseViewModel
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
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Duration));
            }
        }
        private string _Creator;
        public string Creator
        {
            get
            {
                return _Creator;
            }
            set
            {
                _Creator = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Creator));
            }
        }
        private string create_time;
        public string Create_Time
        {
            get
            {
                return create_time;
            }
            set
            {
                create_time = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Create_Time));
            }
        }

        public string Title { get; set; }
        public SpeedLimit speedInfo { get; set; }
        public ICommand ReturnCommand { get; set; }
        #endregion
        public SpeedRuleDetailViewModel()
        {
            ReturnCommand = new ActionCommand<object>(obj => Return("SpeedRulesView"));
        }
        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();
            switch (action)
            {
                case "Detail":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_SpeedRuleDetail");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                    speedInfo = viewParameters["Detail"] as Gsafety.PTMS.ServiceReference.TrafficManageService.SpeedLimit;
                    InitialPage(speedInfo);
                    break;
            }
        }
         void Return(string name)
        {
            try
            {
                EventAggregator.Publish(new ViewNavigationArgs(TrafficName.SpeedRulesView, new Dictionary<string, object>() { { "action", name } }));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        void InitialPage(SpeedLimit speedlimit)
        {
            speedInfo.ID = speedlimit.ID;
            RuleName = speedlimit.NAME;
            MaxSpeed = speedlimit.MAX_SPEED.ToString();
            Duration = speedlimit.DURATION.ToString();
            Creator = speedlimit.Creator;
            Create_Time = speedlimit.CreateTime.ToString();
        }
    }
}
