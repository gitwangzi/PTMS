/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5fcb275f-97e6-438e-9685-10263e67adaf      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.ViewModels
/////    Project Description:    
/////             Class Name: RuleToVehicleViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/3 11:28:44
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/3 11:28:44
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Linq;
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
using Gsafety.PTMS.BaseInformation;
using System.Collections.Generic;
using Jounce.Core.Command;
using Jounce.Framework.Command;
using Jounce.Core.View;
using System.Reflection;
using BaseLib.ViewModels;

namespace Gsafety.PTMS.Traffic.ViewModels
{
    [ExportAsViewModel(TrafficName.RuleToVehicleViewModel)]
    public class RuleToVehicleViewModel:BaseViewModel
    {      
        #region
        int currentIndex = 1;
        public string VehicleID { get; set; }
        public string LimitSpeedName { get; set; }
        public IActionCommand QueryCommand { get; set; }
        public IActionCommand DeleteCommand { get; set;}
        public IActionCommand DetailCommand { get; set; }
        public PagedServerCollection<SpeedLimit> LimitSpeed_List { get; set; }
        public SpeedLimit speedlimit { get; set; }
       
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
        #endregion
        TrafficManageServiceClient trafficServiceClient = null;
        public RuleToVehicleViewModel()
        {
            try
            {
                trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
                PageSizeList = Gsafety.PTMS.BaseInformation.BaseInformationCommon.PageSizeList;
                QueryCommand = new ActionCommand<object>(obj => Query());
                DeleteCommand = new ActionCommand<object>(obj => Delete());
                DetailCommand = new ActionCommand<object>(obj => Detail("Detail"));
                PageSizeValue = PageSizeList[0];
                InitPagedServerCollection();
                trafficServiceClient.GetSpeedLimitByNameKeyAndVeicleIDCompleted += trafficServiceClient_GetSpeedLimitByNameKeyAndVeicleIDCompleted;
                trafficServiceClient.DeleteVehicleToRuleCompleted += trafficServiceClient_DeleteVehicleToRuleCompleted;
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
            pageSize = PageSizeValue;
            System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
            PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
            trafficServiceClient.GetSpeedLimitByNameKeyAndVeicleIDAsync(LimitSpeedName, VehicleID, 1, pagingInfo);
        }     
        private void trafficServiceClient_GetSpeedLimitByNameKeyAndVeicleIDCompleted(object sender, GetSpeedLimitByNameKeyAndVeicleIDCompletedEventArgs e)
        {
            try
            {
                //e.Result.Result.Distinct()
                LimitSpeed_List.loader_Finished(new PagedResult<SpeedLimit>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex,
                });
                if (e.Result.TotalRecord == 0)
                {
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void trafficServiceClient_DeleteVehicleToRuleCompleted(object sender, DeleteVehicleToRuleCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    LimitSpeed_List.ToPage(currentIndex);
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

        void Delete()
        {
            SpeedLimit speedTemp = new SpeedLimit();
            speedTemp = speedlimit;
            var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ConfirmDelete"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    trafficServiceClient.DeleteVehicleToRuleAsync(speedlimit.ID);
                }
                catch (Exception ex)
                {
                    ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
                }
            }           
            
        }

        void Detail(string name)
        {
            EventAggregator.Publish(new ViewNavigationArgs(TrafficName.RuleToVehicleDetailView, new Dictionary<string, object>() { { "action", name }, { name, speedlimit } }));
        }      

        private void Query()
        {
            LimitSpeed_List.MoveToFirstPage();
        }
    }
}
