/////Copyright (C) Gsafety 2015 .All Rights Reserved.
/////======================================================================
/////                   Guid: 67ff2ec4-b9f4-4535-811f-2cca6d0187f1      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.ViewModels
/////    Project Description:    
/////             Class Name: ToVehicleVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2015/1/13 15:16:02
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2015/1/13 15:16:02
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
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Jounce.Framework.Command;
using Gsafety.PTMS.BasicPage.VehicleSelect;
using System.Collections.ObjectModel;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using System.Collections.Generic;
using System.Reflection;

namespace Gsafety.PTMS.Traffic.ViewModels
{
   [ExportAsViewModel(TrafficName.ToVehicleViewModel)]
    public class ToVehicleVm : BaseEntityViewModel
    {
        #region
       public ICommand SetCommand { get; set; }
       public ICommand ResetCommand { get; set; }
       public ICommand ReturnCommand { get; set; }
       public SpeedLimit speedInfo { get; set; }
       private string _MaxSpeed;
       public string MaxSpeed
       {
           get{
            return _MaxSpeed;
           }
           set
           {
             _MaxSpeed=value;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MaxSpeed));
           }
       }
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
       public string Title { get; set; }
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
        #endregion
       TrafficManageServiceClient client = null;
        public ToVehicleVm()
       {
           client = ServiceClientFactory.Create<TrafficManageServiceClient>();
           _VehicleSelectVM = new VehicleSelectViewModelold();
           SetCommand = new ActionCommand<object>(obj => Set());
           ResetCommand = new ActionCommand<object>(obj => Reset());
           ReturnCommand = new ActionCommand<object>(obj => Return("SpeedRulesView"));
           client.GetVehicleBySpeedIdCompleted += client_GetVehicleBySpeedIdCommpleted;
       }

        private void client_GetVehicleBySpeedIdCommpleted(object sender, GetVehicleBySpeedIdCompletedEventArgs e)
        {
            ObservableCollection<string> strList = new ObservableCollection<string>();
            if (e.Result.IsSuccess)
            {
                if (e.Result.Result != null)
                {
                    foreach (var i in e.Result.Result)
                    {
                        strList.Add(i.MDVR_CODE_SN);
                    }
                    VehicleSelectVM.InitTree(strList);
                }
            }
        }
        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            
            base.ActivateView(viewName, viewParameters);
            
            action = viewParameters["action"].ToString();
            switch (action)
            {
                case "toVehicle":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_SpeedRuleToVehicle");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                     _VehicleSelectVM.InitTree();
                     VehicleSelectVM.TreeViewVisible = Visibility.Visible;
                    speedInfo = viewParameters["toVehicle"] as Gsafety.PTMS.ServiceReference.TrafficManageService.SpeedLimit;
                    client.GetVehicleBySpeedIdAsync(speedInfo.ID);   
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
       public void Set()
       {
           try
           {
               SelectModels = VehicleSelectVM.GetSelectModel();
               if (SelectModels.Count != 0)
               {
                   ServiceReference.MessageService.OverSpeedSendSettingModel model = new ServiceReference.MessageService.OverSpeedSendSettingModel();
                   model.Setting = new SettingOverSpeedCMD();
                   model.Value = new ObservableCollection<Gsafety.PTMS.ServiceReference.MessageService.SelectInfoModel>();
                   model.Value = selectModels;
                   model.Setting.RuleName = speedInfo.ID;
                   model.Setting.SendTime = DateTime.Now;
                   model.Setting.Duration = speedInfo.DURATION.ToString();
                   model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                   model.Setting.MaxSpeed = speedInfo.MAX_SPEED.ToString();
                   model.Setting.MinSpeed = minSpeed.ToString();
                   model.Setting.OverSpeedID = speedInfo.ID;
                   model.Setting.OperType = 1;
                   ApplicationContext.Instance.MessageManager.SendSettingOverSpeedUploadCMD(model);
                   MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
               }
           }
           catch (Exception ex)
           {
               ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
           }
       }
       public void Reset()
       {
           _VehicleSelectVM.InitTree();
           VehicleSelectVM.FilterText = string.Empty;
           VehicleSelectVM.TreeViewVisible = Visibility.Visible;
       }
       public void Return(string name)
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
    }
}
