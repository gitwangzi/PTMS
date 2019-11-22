using Gsafety.PTMS.ServiceReference.UpdateService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2d055053-764b-4bd9-a3ea-3d4c75ea3557      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels
/////    Project Description:    
/////             Class Name: UpgradeVersionDetailVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/22 10:45:01
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/22 10:45:01
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Maintain.ViewModels
{
    [ExportAsViewModel(MaintainName.VersionMappingVm)]
    public class VersionMappingVm : BaseViewModel
    {
        private UpdateServiceClient upgradeClient = ServiceClientFactory.Create<UpdateServiceClient>();
        private string unifyVersion;
        public SuiteVersionMap CurrentSuiteVersionMap { get; set; }

        public PagedServerCollection<SuiteVersionMap> PSC_SuiteVersionMap { get; set; }

        public string UnifyVersion { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set { pageSizeValue = value; this.PSC_SuiteVersionMap.PageSize = value; }
        }
        public List<int> PageSizeList { get; set; }
        public IActionCommand UpdateCommand { get; private set; }
        public IActionCommand DeleteCommand { get; private set; }
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand ViewCommand { get; private set; }
        public IActionCommand AddCommand { get; private set; }
        public IActionCommand AllUpgradeCommand { get; private set; }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (viewParameters.Count != 0 && viewParameters["action"].ToString() == "return")
            {
                PSC_SuiteVersionMap.ToPage(currentIndex);
            }
        }

        public VersionMappingVm()
        {
            upgradeClient.DeleteSuiteVersionCompleted += upgradeClient_DeleteSuiteVersionCompleted;
            AddCommand = new ActionCommand<object>(obj => Publish("add"));
            UpdateCommand = new ActionCommand<object>(obj => Publish("update"));
            ViewCommand = new ActionCommand<object>(obj => Publish("view"));
            DeleteCommand = new ActionCommand<object>(ojb => Delete());
            QueryCommand = new ActionCommand<object>(obj => Query());
            AllUpgradeCommand = new ActionCommand<object>(obj => AllUpgrade());
            InitPagedServerCollection();

            PageSizeList = MaintainCommon.PageSizeList; //获取翻页控件下拉列表
            PageSizeValue = PageSizeList[0];//获取默认每页显示记录的数据
        }

        private int currentIndex = 1;
        private void InitPagedServerCollection()
        {
            upgradeClient.GetSuiteVersionMapRecordsFuzzyCompleted += upgradeClient_GetSuiteVersionMapRecordsFuzzyCompleted;
            PSC_SuiteVersionMap = new PagedServerCollection<SuiteVersionMap>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                upgradeClient.GetSuiteVersionMapRecordsFuzzyAsync(unifyVersion, pagingInfo);
            });
        }

        void upgradeClient_GetSuiteVersionMapRecordsFuzzyCompleted(object sender, GetSuiteVersionMapRecordsFuzzyCompletedEventArgs e)
        {
            try
            {
                PSC_SuiteVersionMap.loader_Finished(new PagedResult<SuiteVersionMap>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex,
                });
                if (e.Result.TotalRecord == 0)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"));
                }
            }
            catch
            {
                //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"));
            }
        }

        private void Delete()
        {
            var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_UpgradeConfirm"), "", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                if (CurrentSuiteVersionMap != null)
                {
                    upgradeClient.DeleteSuiteVersionAsync(CurrentSuiteVersionMap.Id);
                }
            }
        }
        void upgradeClient_DeleteSuiteVersionCompleted(object sender, DeleteSuiteVersionCompletedEventArgs e)
        {
            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DeleteSucess"));
            PSC_SuiteVersionMap.ToPage(currentIndex);
        }

        private void Query()
        {
            unifyVersion = UnifyVersion == null ? string.Empty : UnifyVersion.Trim();

            currentIndex = 1;
            PSC_SuiteVersionMap.MoveToFirstPage();
        }

        private void AllUpgrade()
        {
            var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_UpgradeConfirm"), "", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                ApplicationContext.Instance.MessageManager.SendUpgradeNotifyMessage(new Gsafety.PTMS.ServiceReference.MessageService.UpgradeNotify() { NotifyTime = DateTime.Now });
                PSC_SuiteVersionMap.ToPage(currentIndex);
            }
        }

        private void Publish(string name)
        {
            if (name == "add")
            {
                EventAggregator.Publish(new ViewNavigationArgs(MaintainName.VersionMappingAddV, new Dictionary<string, object>() { { "action", name }, { name, CurrentSuiteVersionMap } }));
            }
            else
            {
                EventAggregator.Publish(new ViewNavigationArgs(MaintainName.VersionMappingEditV, new Dictionary<string, object>() { { "action", name }, { name, CurrentSuiteVersionMap } }));
            }
        }

    }
}
