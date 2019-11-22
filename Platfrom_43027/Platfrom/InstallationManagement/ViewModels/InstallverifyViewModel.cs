/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7bd1493c-ca6c-434b-9613-ee765b480c05      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: InstallverifyViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/24 10:53:27
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/24 10:53:27
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
using System.Windows.Data;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System.Reflection;

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.InstallverifyVm)]
    public class InstallverifyViewModel : BaseViewModel
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
            deviceInstallServiceClient.GetInstallationAuditAsync(CarNumber, SuiteId, Installer, InstallstationId, BeginDate, EndDate, pageinfo, IsChecked);
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
            }
        }

        private string _CarNumber;
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

        private string _SuiteId;
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

        private string _Installer;
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

        private string _InstallstationId = string.Empty;
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

        private DateTime _EndDate = DateTime.Now.AddHours(1);
        public DateTime EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                _EndDate = value.AddHours(24);
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

        private bool _IsChecked = true;
        public bool IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                _IsChecked = value;
                PageIndex = 0;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked));
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

        private ObservableCollection<InstallationAuditCollect> _SuiteModels_Imps;
        public ObservableCollection<InstallationAuditCollect> SuiteModels_Imps
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
            try
            {
                PagingInfo pageinfo = new PagingInfo
                   {
                       PageIndex = PageIndex,
                       PageSize = PageSizeValue
                   };
                deviceInstallServiceClient.GetInstallationAuditAsync(CarNumber, SuiteId, Installer, InstallstationId, BeginDate, EndDate, pageinfo, IsChecked);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        DeviceInstallServiceClient deviceInstallServiceClient = null;
        public IActionCommand QueryCommand { get; private set; }
        public InstallverifyViewModel()
        {
            try
            {
                deviceInstallServiceClient = ServiceClientFactory.Create<DeviceInstallServiceClient>();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InstallverifyViewModel()", ex);
            }
            PageSizeList = new List<int>
           {
               20,
               40,
               80,
           };

            QueryCommand = new ActionCommand<object>(obj => Query());
            deviceInstallServiceClient.GetInstallationAuditCompleted += deviceInstallServiceClient_GetInstallationAuditCompleted;

        }
        public bool PageChanged = false;
        void deviceInstallServiceClient_GetInstallationAuditCompleted(object sender, GetInstallationAuditCompletedEventArgs e)
        {
            try
            {
                if ((totalCount != e.Result.TotalRecord) || PageChanged)
                    totalCount = e.Result.TotalRecord;
                SuiteModels_Imps = e.Result.Result;
                foreach (var item in SuiteModels_Imps)
                {
                    if (item.ApproverTime.HasValue)
                    {
                        item.ApproverTime = item.ApproverTime.Value.ToLocalTime();
                    }
                }
                PageChanged = false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InstallverifyViewModel", ex);
            }
        }

        private void Query()
        {
            try
            {
                PagingInfo pageinfo = new PagingInfo
                   {
                       PageIndex = PageIndex,
                       PageSize = PageSizeValue
                   };
                deviceInstallServiceClient.GetInstallationAuditAsync(CarNumber, SuiteId, Installer, InstallstationId, BeginDate, EndDate, pageinfo, IsChecked);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

    }
}
