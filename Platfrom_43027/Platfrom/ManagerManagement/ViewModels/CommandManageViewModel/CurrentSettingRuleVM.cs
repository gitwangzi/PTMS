/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 635ca1b5-3c89-461e-8353-898747132041      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
/////    Project Description:    
/////             Class Name: CurrentSettingRuleVM
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/10 13:46:06
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/10 13:46:06
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
using Gsafety.PTMS.ServiceReference.CommandManageService;
using System.Collections.Generic;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Framework.Command;
using System.Reflection;
using BaseLib.ViewModels;
using BaseLib.Model;
namespace Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
{
    [ExportAsViewModel(ManagerName.CurrentSettingRuleVM)]
    public class CurrentSettingRuleVM : BaseViewModel
    {
        private CommandManageServiceClient commandMangeServiceClient;
        public CurrentSettingRuleInfo CurrentSettingRuleInfos { get; set; }
        public string VehicleId { get; set; }
        public PagedServerCollection<CurrentSettingRuleInfo> PSC_CurrentInfo { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.PSC_CurrentInfo != null)
                {
                    this.PSC_CurrentInfo.PageSize = pageSizeValue;
                }
                //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageSizeValue));
            }
        }
        public List<int> PageSizeList { get; set; }
        public IActionCommand QueryCommand { get; private set; }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                if (viewParameters.Count != 0 && viewParameters["action"].ToString() == "return")
                {
                    PSC_CurrentInfo.ToPage(currentIndex);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public CurrentSettingRuleVM()
        {
            try
            {
                commandMangeServiceClient = ServiceClientFactory.Create<CommandManageServiceClient>();
                QueryCommand = new ActionCommand<object>(obj => Query());
                PageSizeList = ManagerCommon.PageSizeList;
                PageSizeValue = PageSizeList[0];
                InitPagedServerCollection();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private int currentIndex = 1;
        private void InitPagedServerCollection()
        {
            try
            {

                commandMangeServiceClient.GetAllCurrentSettingRuleInfoCompleted += commandMangeServiceClient_GetAllCurrentSettingRuleInfoCompleted;
                PSC_CurrentInfo = new PagedServerCollection<CurrentSettingRuleInfo>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    PagingInfo pageInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                    commandMangeServiceClient.GetAllCurrentSettingRuleInfoAsync(ApplicationContext.Instance.AuthenticationInfo.UserShowName.ToLower(), VehicleId, pageInfo);
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void commandMangeServiceClient_GetAllCurrentSettingRuleInfoCompleted(object sender, GetAllCurrentSettingRuleInfoCompletedEventArgs e)
        {
            try
            {
                PSC_CurrentInfo.loader_Finished(new PagedResult<CurrentSettingRuleInfo>
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
                ApplicationContext.Instance.Logger.LogException("CurrentSettingRuleVm", ex);
            }
        }

        private void Query()
        {
            currentIndex = 1;
            PSC_CurrentInfo.MoveToFirstPage();
        }
    }
}
