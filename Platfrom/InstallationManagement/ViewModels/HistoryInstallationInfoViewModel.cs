/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1d813fca-a5c6-46d6-88ed-72f33f5c0d63      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: HistoryInstallationInfoViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/16 16:17:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/16 16:17:43
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
using Jounce.Core.View;
using System.Collections.Generic;
using Jounce.Framework.Command;
using Jounce.Core.Command;
using Gsafety.PTMS.ServiceReference.MaitenanceRecordService;
using Gsafety.PTMS.Share;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.HistoryInstallationInfoVm)]
    public class HistoryInstallationInfoViewModel : BaseViewModel
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
            maintenanceRecordServiceClient.GetHistoryManintenancerecordsAsync(CarNumber,SuiteId, BeginDate, EndDate, pageinfo);
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
        int totalCount
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

        private ObservableCollection<HistorySuiteMaintenance> _SuiteModels_Imps;
        public ObservableCollection<HistorySuiteMaintenance> SuiteModels_Imps
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

        private HistorySuiteMaintenance _CurrentInstallRecord;
        public HistorySuiteMaintenance CurrentInstallRecord
        {
            get
            {
                return _CurrentInstallRecord;
            }
            set
            {
                _CurrentInstallRecord = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentInstallRecord));
            }
        }

        private void GetInfoByPageIndex()
        {
            PagingInfo pageinfo = new PagingInfo
            {
                PageIndex = PageIndex,
                PageSize = PageSizeValue
            };
            maintenanceRecordServiceClient.GetHistoryManintenancerecordsAsync(CarNumber,SuiteId, BeginDate, EndDate, pageinfo);
        }

        MaintenanceRecordServiceClient maintenanceRecordServiceClient = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand ViewCommand { get; private set; }
        public HistoryInstallationInfoViewModel()
        {

            PageSizeList = new List<int>
           {
               20,
               40,
               80,
           };

            QueryCommand = new ActionCommand<object>(obj => Query());
            ViewCommand = new ActionCommand<object>(obj => View());
            maintenanceRecordServiceClient.GetHistoryManintenancerecordsCompleted += maintenanceRecordServiceClient_GetHistoryManintenancerecordsCompleted;
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            PagingInfo pageinfo = new PagingInfo
            {
                PageIndex = PageIndex,
                PageSize = PageSizeValue
            };
            maintenanceRecordServiceClient.GetHistoryManintenancerecordsAsync(CarNumber,SuiteId, BeginDate, EndDate, pageinfo);
        }

        public bool PageChanged = false;
        void maintenanceRecordServiceClient_GetHistoryManintenancerecordsCompleted(object sender, GetHistoryManintenancerecordsCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogError("HistoryInstallationInfoViewModel", ex);
            }
        }

        private void View()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.HistoricalMaintenanceDetailsV, new Dictionary<string, object>() { { "HistorySuiteMaintenance", CurrentInstallRecord } }));
        }

        private void Query()
        {
            PagingInfo pageinfo = new PagingInfo
            {
                PageIndex = PageIndex,
                PageSize = PageSizeValue
            };
            maintenanceRecordServiceClient.GetHistoryManintenancerecordsAsync(CarNumber,SuiteId, BeginDate, EndDate, pageinfo);
        }
    }
}
