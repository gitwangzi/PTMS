/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 02543521-e083-4eca-96be-68d35499dbca      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.ViewModels
/////    Project Description:    
/////             Class Name: RuleCommandStateViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/3 14:50:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/3 14:50:14
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System.Reflection;
using BaseLib.ViewModels;

namespace Gsafety.PTMS.Traffic.ViewModels
{
    [ExportAsViewModel(TrafficName.RuleCommandStateViewModel)]
    public class RuleCommandStateViewModel : BaseViewModel
    {
        #region
        int currentIndex = 1;
        public string VehicleID { get; set; }
        public string LimitSpeedName { get; set; }
        public IActionCommand QueryCommand { get; set; }
        public PagedServerCollection<SpeedStatusFailed> LimitSpeed_List { get; set; }
        public SpeedStatusFailed speedlimit { get; set; }
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
        public RuleCommandStateViewModel()
        {
            try
            {
                trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
                trafficServiceClient.SpeedToVehicleFialStateCompleted += trafficServiceClient_SpeedToVehicleFialStateCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            PageSizeList = Gsafety.PTMS.BaseInformation.BaseInformationCommon.PageSizeList;
            QueryCommand = new ActionCommand<object>(obj => Query());
            PageSizeValue = PageSizeList[0];
            InitPagedServerCollection();
        }

        private void InitPagedServerCollection()
        {
            LimitSpeed_List = new PagedServerCollection<SpeedStatusFailed>(new Action<int, int>(InvokeServer));
        }

        private void InvokeServer(int pageIndex, int pageSize)
        {
            pageSize = PageSizeValue;
            System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
            PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
            trafficServiceClient.SpeedToVehicleFialStateAsync(VehicleID, LimitSpeedName,pagingInfo);
        }
        private void trafficServiceClient_SpeedToVehicleFialStateCompleted(object sender, SpeedToVehicleFialStateCompletedEventArgs e)
        {
            try
            {
                LimitSpeed_List.loader_Finished(new PagedResult<SpeedStatusFailed>
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
        private void Query()
        {
            LimitSpeed_List.MoveToFirstPage();
        }
    }
}

    

