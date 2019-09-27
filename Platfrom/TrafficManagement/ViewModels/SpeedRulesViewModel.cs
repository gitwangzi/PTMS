/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 94cb2abb-1d94-4561-83df-935c94849cf7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.ViewModels
/////    Project Description:    
/////             Class Name: SpeedRulesViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/1 9:36:45
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/1 9:36:45
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
using System.Collections.ObjectModel;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Gsafety.PTMS.BaseInformation;
using System.Collections.Generic;
using Jounce.Core.Command;
using Jounce.Framework.Command;
using Jounce.Core.View;
using System.Reflection;
using Gsafety.PTMS.ServiceReference.MessageService;
using BaseLib.ViewModels;

namespace Gsafety.PTMS.Traffic.ViewModels
{
    [ExportAsViewModel(TrafficName.SpeedRulesViewModel)]
    public class SpeedRulesViewModel : BaseViewModel
    {
        #region
        int currentIndex = 1;
        int minspeed = 0;
        public string VehicleID { get; set; }
        public string LimitSpeedName { get; set; }
        public IActionCommand QueryCommand { get; set; }
        public IActionCommand AddCommand { get; set; }
        public IActionCommand ToVehicleCommand { get; set; }
        public IActionCommand DetailCommand { get; set; }
        public IActionCommand RepairCommand { get; set; }
        public IActionCommand DeleteCommand { get; set; }        
        public PagedServerCollection<SpeedLimit> LimitSpeed_List { get; set; }
        public SpeedLimit speedLimit{get;set;}
        public List<int> PageSizeList { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (LimitSpeed_List != null)
                {
                    LimitSpeed_List.PageSize = value;
                }
            }
        }
        public bool IsLatestPage { get; set; }
        ObservableCollection<Gsafety.PTMS.ServiceReference.MessageService.SelectInfoModel> selectModels;
        public ObservableCollection<Gsafety.PTMS.ServiceReference.MessageService.SelectInfoModel> SelectModels
        {
            get { return selectModels; }
            set { selectModels = value; }
        }
        #endregion

        TrafficManageServiceClient trafficServiceClient = null;
        private bool isFirst;

        public SpeedRulesViewModel()
        {
            try
            {
                isFirst = true;               
                IsLatestPage = true;                
                trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();              
                QueryCommand = new ActionCommand<object>(obj => Query());
                AddCommand = new ActionCommand<object>(obj => Publish("Add"));
                ToVehicleCommand = new ActionCommand<object>(obj => ToVehicle("toVehicle"));
                RepairCommand = new ActionCommand<object>(obj => Modify("repair"));
                DetailCommand = new ActionCommand<object>(obj => Detail("Detail"));
                DeleteCommand = new ActionCommand<object>(obj => Delete());
                PageSizeList = new List<int> { 20, 40, 80 };
                PageSizeValue = PageSizeList[0];
                InitPagedServerCollection();
                trafficServiceClient.GetSpeedLimitByNameKeyCompleted += trafficServiceClient_GetSpeedLimitByNameKeyCompleted;
                trafficServiceClient.DeleteSpeedLimitCompleted += trafficServiceClient_DeleteSpeedLimitCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void InitPagedServerCollection()
        {
            LimitSpeed_List = new PagedServerCollection<SpeedLimit>(new Action<int, int>(InvokeServer));
        }
        private void InvokeServer(int pageIndex, int pageSize)
        {
            try
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                trafficServiceClient.GetSpeedLimitByNameKeyAsync(LimitSpeedName, 1, pagingInfo);
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
                LimitSpeed_List.ToPage(currentIndex);                
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void Publish(string name)
        {
            try
            {
                IsLatestPage = false;  
                EventAggregator.Publish(new ViewNavigationArgs(TrafficName.AddspeedlimitView, new Dictionary<string, object>() { { "action", name }, { name, speedLimit}}));               
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void ToVehicle(string name)
        {
            try
            {
                IsLatestPage = false;
                EventAggregator.Publish(new ViewNavigationArgs(TrafficName.ToVehicleView, new Dictionary<string, object>() { { "action", name }, { name, speedLimit } }));
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
                IsLatestPage = false;
                EventAggregator.Publish(new ViewNavigationArgs(TrafficName.ModifyspeedlimitView, new Dictionary<string, object>() { { "action", name }, { name, speedLimit } }));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void Detail(string name)
        {
            try
            {
                IsLatestPage = false;
                EventAggregator.Publish(new ViewNavigationArgs(TrafficName.SpeedRuleDetailView, new Dictionary<string, object>() { { "action", name }, { name, speedLimit } }));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void Delete()
        {      
                var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ConfirmDelete"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(speedLimit.ID))
                        {
                            speedLimit.MIN_SPEED = minspeed;
                            ServiceReference.MessageService.OverSpeedSendSettingModel model = new ServiceReference.MessageService.OverSpeedSendSettingModel();
                            model.Setting = new SettingOverSpeedCMD();
                            model.Setting.RuleName = speedLimit.ID;
                            model.Setting.SendTime = DateTime.Now;
                            model.Setting.Duration = speedLimit.DURATION.ToString();
                            model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                            model.Setting.MaxSpeed = speedLimit.MAX_SPEED.ToString();
                            model.Setting.MinSpeed = speedLimit.MIN_SPEED.ToString();
                            model.Setting.OverSpeedID = speedLimit.ID;
                            model.OperationType = RuleOperationType.Delete;
                            model.Setting.OperType = 0;
                            ApplicationContext.Instance.MessageManager.SendSettingOverSpeedUploadCMD(model);
                            trafficServiceClient.DeleteSpeedLimitAsync(speedLimit.ID);
                        }                     
                    }
                    catch (Exception ex)
                    {
                        ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
                    }
                }     
        }     
        private void trafficServiceClient_GetSpeedLimitByNameKeyCompleted(object sender, GetSpeedLimitByNameKeyCompletedEventArgs e)
        {
            try
            {
                LimitSpeed_List.loader_Finished(new PagedResult<SpeedLimit>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex,
                });    
           
                if (e.Result.TotalRecord == 0&&!isFirst)
                {
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }

                isFirst = false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void trafficServiceClient_DeleteSpeedLimitCompleted(object sender, DeleteSpeedLimitCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {                    
                    LimitSpeed_List.ToPage(currentIndex);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void Query()
        {
            LimitSpeed_List.MoveToFirstPage();        
        }
    }
}