using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4e2c0e3f-0409-41e6-a882-94a4e3c9a199      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels
/////    Project Description:    
/////             Class Name: SuiteHistoryRecordVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/12 14:29:20
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/12 14:29:20
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Maintain.ViewModels
{
    [ExportAsViewModel(MaintainName.SuiteHistoryRecordVm)]
    public class SuiteHistoryRecordVm : BaseViewModel
    {
        private List<int> _PageSizeList;
        public List<int> PageSizeList
        {
            get
            {
                return _PageSizeList;
            }
            set
            {
                _PageSizeList = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageSizeList));
            }
        }

        private int _PageSizeValue = 20;
        public int PageSizeValue
        {
            get
            {
                return _PageSizeValue;
            }
            set
            {
                _PageSizeValue = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageSizeValue));
                PageChanged = true;
                PageSizeChenged();
            }
        }
        private void PageSizeChenged()
        {
            PagingInfo pageinfo = new PagingInfo
            {
                PageIndex = 0,
                PageSize = PageSizeValue
            };
            deviceInstallServiceClient.GetInstallationFinishedEx1Async(CarNumber.Trim(), SuiteId.Trim(), Installer.Trim(), InstallstationId, BeginDate, EndDate, pageinfo);
        }
        private int _PageIndex = -1;
        public int PageIndex
        {
            get
            {
                return _PageIndex;
            }
            set
            {
                _PageIndex = value;
                GetInfoByPageIndex();
                //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageIndex));
            }
        }

        private string _CarNumber = string.Empty;
        public string CarNumber
        {
            get
            {
                return _CarNumber;
            }
            set
            {
                _CarNumber = value;
            }
        }

        private string _SuiteId = string.Empty;
        public string SuiteId
        {
            get
            {
                return _SuiteId;
            }
            set
            {
                _SuiteId = value;
            }
        }

        private string _Installer = string.Empty;
        public string Installer
        {
            get
            {
                return _Installer;
            }
            set
            {
                _Installer = value;
            }
        }

        private string _InstallstationId;
        public string InstallstationId
        {
            get
            {
                return _InstallstationId;
            }
            set
            {
                _InstallstationId = value;
            }
        }

        private DateTime _BeginDate = DateTime.Now.AddDays(-1);
        public DateTime BeginDate
        {
            get
            {
                return _BeginDate;
            }
            set
            {
                _BeginDate = value;
            }
        }

        private DateTime _EndDate = DateTime.Now;
        public DateTime EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                _EndDate = value.AddHours(24); ;
            }
        }
        private int _totalCount;
        public int totalCount 
        {
            get
            {
                return _totalCount;
            }
            set
            {
                _totalCount = value;
                initdatapager(_totalCount);
            }
        }

        private PagedCollectionView _ItemCount;
        public PagedCollectionView ItemCount
        {
            get { return _ItemCount; }
            set
            {
                _ItemCount = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ItemCount));
            }
        }

        private void initdatapager(int _totalCount)
        {
            List<int> itemCount = new List<int>();
            for (int i = 1; i <= _totalCount; i++)
            {
                itemCount.Add(i);
            }
            ItemCount = new PagedCollectionView(itemCount);
        }

        private ObservableCollection<InstallationInfo> _SuiteModels_Imps;
        public ObservableCollection<InstallationInfo> SuiteModels_Imps
        {
            get
            {
                return _SuiteModels_Imps;
            }
            set
            {
                _SuiteModels_Imps = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SuiteModels_Imps));
            }
        }

        private void GetInfoByPageIndex()
        {
            PagingInfo pageinfo = new PagingInfo
            {
                PageIndex = PageIndex,
                PageSize = PageSizeValue
            };
            deviceInstallServiceClient.GetInstallationFinishedEx1Async(CarNumber.Trim(), SuiteId.Trim(), Installer.Trim(), InstallstationId, BeginDate, EndDate, pageinfo);
        }

        DeviceInstallServiceClient deviceInstallServiceClient = ServiceClientFactory.Create<DeviceInstallServiceClient>();
        public IActionCommand QueryCommand { get; private set; }
        public SuiteHistoryRecordVm()
       {

           PageSizeList = new List<int>
           {
               20,
               40,
               80,
           };

           QueryCommand = new ActionCommand<object>(obj => Query());
           deviceInstallServiceClient.GetInstallationFinishedEx1Completed += deviceInstallServiceClient_GetInstallationFinishedEx1Completed;
       }

        void deviceInstallServiceClient_GetInstallationFinishedEx1Completed(object sender, GetInstallationFinishedEx1CompletedEventArgs e)
        {
            try
            {
                if ((totalCount != e.Result.TotalRecord) || PageChanged)
                    totalCount = e.Result.TotalRecord;
                SuiteModels_Imps = e.Result.Result;
                PageChanged = false;
                if (totalCount == 0)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogError("SuiteHistoryRecordVm", ex);
            }
        }
        public bool PageChanged = false;
        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            PagingInfo pageinfo = new PagingInfo
            {
                PageIndex = PageIndex,
                PageSize = PageSizeValue
            };
            deviceInstallServiceClient.GetInstallationFinishedEx1Async(CarNumber.Trim(), SuiteId.Trim(), Installer.Trim(), InstallstationId, BeginDate, EndDate, pageinfo);
        }

        private void Query()
        {
            PagingInfo pageinfo = new PagingInfo
            {
                PageIndex = PageIndex,
                PageSize = PageSizeValue
            };
            deviceInstallServiceClient.GetInstallationFinishedEx1Async(CarNumber.Trim(), SuiteId.Trim(), Installer.Trim(), InstallstationId, BeginDate, EndDate, pageinfo);
        }
    }
}
