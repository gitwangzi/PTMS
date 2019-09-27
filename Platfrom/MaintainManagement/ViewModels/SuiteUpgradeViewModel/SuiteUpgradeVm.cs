using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a5ecf27c-ba55-4b18-8be0-a1fb8f952e88      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels
/////    Project Description:    
/////             Class Name: SuiteUpgradeVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/13 17:32:50
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/13 17:32:50
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
using System.Linq;
using Gsafety.PTMS.ServiceReference.UpdateService;
using Gsafety.PTMS.Bases.Models;

namespace Gsafety.PTMS.Maintain.ViewModels
{
    [ExportAsViewModel(MaintainName.SuiteUpgradeVm)]
    public class SuiteUpgradeVm : BaseViewModel
    {
        private UpdateServiceClient updateClient = ServiceClientFactory.Create<UpdateServiceClient>();
        public PagedServerCollection<SuiteUpgradeInfo> PSC_SuiteUpdateInfo { get; set; }
        public List<SuiteUpgradeInfo> CheckedSuites { get; set; }
        public SuiteUpgradeInfo CurrentSuiteUpdateInfo { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.PSC_SuiteUpdateInfo != null)
                {
                    this.PSC_SuiteUpdateInfo.PageSize = pageSizeValue;
                }
            }
        }

        public EnumModel CurrentUpgradeStatus { get; set; }
        public List<EnumModel> UpgradeStatusList { get; set; }

        private void InitialUpgradeStatus()
        {
            UpgradeStatusList = new List<EnumModel>();
            UpgradeStatusList.Add(new EnumModel { ShowName = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), EnumName = string.Empty });
            Enum.GetNames(typeof(SuiteUpgradeStatusType)).ToList().ForEach(x =>
            {
                if (x != SuiteUpgradeStatusType.UpgradFinish.ToString())
                {
                    EnumModel item = new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                    UpgradeStatusList.Add(item);
                }
            });
        }

        public List<int> PageSizeList { get; set; }
        public string VehicleId { get; set; }
        public string SuiteId { get; set; }
        public bool IsEnabled { get; set; }
        private FTPInfo ftpInfo { get; set; }
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand SingleUpgradeCommand { get; private set; }
        public IActionCommand AllUpgradeCommand { get; private set; }
        public IActionCommand CheckCommand { get; private set; }
        public IActionCommand CancelCommand { get; private set; }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (viewParameters.Count != 0 && viewParameters["action"].ToString() == "return")
            {
                PSC_SuiteUpdateInfo.ToPage(currentIndex);
            }
            CurrentUpgradeStatus = UpgradeStatusList[0];
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentUpgradeStatus));

        }

        public SuiteUpgradeVm()
        {
            InitialUpgradeStatus();
            updateClient.DeleteSuiteUpdateRecordCompleted += updateClient_DeleteSuiteUpdateRecordCompleted;
            updateClient.GetFTPInfoCompleted += updateClient_GetFTPInfoCompleted;
            updateClient.GetFTPInfoAsync();
            SingleUpgradeCommand = new ActionCommand<object>(obj => SingleUpgrade());
            AllUpgradeCommand = new ActionCommand<object>(obj => AllUpgrade());
            CheckCommand = new ActionCommand<object>(obj => Check());
            QueryCommand = new ActionCommand<object>(obj => Query());
            CancelCommand = new ActionCommand<object>(obj => Cancel());
            PageSizeList = MaintainCommon.PageSizeList; //获取翻页控件下拉列表
            PageSizeValue = PageSizeList[0];//获取默认每页显示记录的数据
            InitPagedServerCollection();
            CheckedSuites = new List<SuiteUpgradeInfo>();
        }

        void updateClient_DeleteSuiteUpdateRecordCompleted(object sender, DeleteSuiteUpdateRecordCompletedEventArgs e)
        {
            if (e.Result != null || !e.Result.IsSuccess)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"));
            }
            PSC_SuiteUpdateInfo.ToPage(currentIndex);
        }
        private void Cancel()
        {
            updateClient.DeleteSuiteUpdateRecordAsync(CurrentSuiteUpdateInfo.Id);
        }
        private void Check()
        {
            if (CheckedSuites.Any(x => x.SuiteId == CurrentSuiteUpdateInfo.SuiteId))
            //if (CheckedSuites.Contains(CurrentSuiteUpdateRecord))
            {
                CheckedSuites.Remove(CheckedSuites.FirstOrDefault(x => x.SuiteId == CurrentSuiteUpdateInfo.SuiteId));
            }
            else
            {
                CheckedSuites.Add(CurrentSuiteUpdateInfo);
            }
            IsEnabled = CheckedSuites.Count > 0 ? true : false;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsEnabled));
        }

        void updateClient_GetFTPInfoCompleted(object sender, GetFTPInfoCompletedEventArgs e)
        {
            if (e.Result.IsSuccess)
            {
                ftpInfo = e.Result.Result;
            }
        }

        private int currentIndex = 1;
        private void InitPagedServerCollection()
        {
            updateClient.GetSuiteUpgradeInfoFuzzyCompleted += updateClient_GetSuiteUpgradeInfoFuzzyCompleted;
            PSC_SuiteUpdateInfo = new PagedServerCollection<SuiteUpgradeInfo>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                string vehicleId = VehicleId == null ? string.Empty : VehicleId.Trim();
                Gsafety.PTMS.ServiceReference.UpdateService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.UpdateService.PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                SuiteUpgradeStatusType? upgradeStatus = null;
                if (CurrentUpgradeStatus != null && CurrentUpgradeStatus.EnumName != string.Empty)
                {
                    upgradeStatus = (SuiteUpgradeStatusType)Enum.Parse(typeof(SuiteUpgradeStatusType), CurrentUpgradeStatus.EnumName, true);
                }
                updateClient.GetSuiteUpgradeInfoFuzzyAsync(vehicleId, SuiteId, upgradeStatus, pagingInfo);
            });
        }

        void updateClient_GetSuiteUpgradeInfoFuzzyCompleted(object sender, GetSuiteUpgradeInfoFuzzyCompletedEventArgs e)
        {
            try
            {
                if (CheckedSuites.Count > 0)
                {
                    e.Result.Result.ToList().ForEach(x =>
                    {
                        if (CheckedSuites.Any(y => y.SuiteId == x.SuiteId))
                        {
                            x.SelectedFlag = true;
                        }
                    });
                }
                PSC_SuiteUpdateInfo.loader_Finished(new PagedResult<SuiteUpgradeInfo>
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

        private void VehicleClient_DeleteVehicleCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DeleteConfirm"));
            PSC_SuiteUpdateInfo.ToPage(currentIndex);
        }

        private void Query()
        {
            currentIndex = 1;
            PSC_SuiteUpdateInfo.MoveToFirstPage();
        }

        private void SingleUpgrade()
        {
            var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_UpgradeConfirm"), "", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                Uri ftpUri = new Uri(ftpInfo.FTPServerName);
                ApplicationContext.Instance.MessageManager.SendUpgradeCMD(new Gsafety.PTMS.ServiceReference.MessageService.UpgradeCMD()
                {
                    CmType = "Control",
                    DvId = CurrentSuiteUpdateInfo.MdvrCoreId,
                    UUId = Guid.NewGuid().ToString(),
                    DataPacketCount = CurrentSuiteUpdateInfo.FileSize,
                    MD5Code = "2222",  //瑞明要求
                    FTPAddress = ftpUri.Host,//"172.16.20.106",
                    Port = ftpInfo.FTPPort.ToString(),
                    UserName = ftpInfo.UserName,
                    Password = ftpInfo.Password,
                    FileName = CurrentSuiteUpdateInfo.FileName,
                    Version = CurrentSuiteUpdateInfo.Vendor,
                });
                PSC_SuiteUpdateInfo.ToPage(currentIndex);
            }

        }

        /// <summary>
        /// 数据转换并发给配置服务器
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ftpFile"></param>
        private void AllUpgrade()
        {
            var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_UpgradeConfirm"), "", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                Uri ftpUri = new Uri(ftpInfo.FTPServerName);
                foreach (SuiteUpgradeInfo item in CheckedSuites)
                {
                    ApplicationContext.Instance.MessageManager.SendUpgradeCMD(new Gsafety.PTMS.ServiceReference.MessageService.UpgradeCMD()
                    {
                        CmType = "Control",
                        DvId = item.MdvrCoreId,
                        UUId = Guid.NewGuid().ToString(),
                        DataPacketCount = item.FileSize,
                        MD5Code = "2222",  //瑞明要求
                        FTPAddress = ftpUri.Host,//"172.16.20.106",
                        Port = ftpInfo.FTPPort.ToString(),
                        UserName = ftpInfo.UserName,
                        Password = ftpInfo.Password,
                        FileName = item.FileName,
                        Version = item.CurrentVersion,
                    });
                    PSC_SuiteUpdateInfo.ToPage(currentIndex);
                }
            }
        }
    }
}
