/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3bea8ad9-42c8-4a60-a575-3478bdc9d8be      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: SuiteMaintainingVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/30 11:06:00
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/30 11:06:00
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Framework.Command;
using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.ServiceReference.MaitenanceRecordService;
using Jounce.Framework.ViewModel;
using System.Windows.Data;

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.SuiteMaintainingVm)]
    public class SuiteMaintainingVm : BaseEntityViewModel
    {

        private MaintenanceRecordServiceClient maintenaceClient;

        public ICommand QueryCommand { get; set; }
        public IActionCommand SimpleMaintenanceCommand { get; private set; }
        public IActionCommand SubstitutionMaintenanceCommand { get; private set; }
        public IActionCommand ScrappedRegistrationCommand { get; private set; }

        private ObservableCollection<SuiteMaintenance> _SuitMaitainingList;
        public ObservableCollection<SuiteMaintenance> SuitMaitainingList
        {
            get
            {
                return _SuitMaitainingList;
            }
            set
            {
                _SuitMaitainingList = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SuitMaitainingList));
            }
        }

        private string _InstallstationId = ApplicationContext.Instance.AuthenticationInfo.OrgCode;
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


        private string _VehicleNumber;
        public string VehicleNumber
        {
            get
            {
                return _VehicleNumber;
            }
            set
            {
                _VehicleNumber = value;
            }
        }

        private string _SuiteID;
        public string SuiteID
        {
            get
            {
                return _SuiteID;
            }
            set
            {
                _SuiteID = value;
            }
        }

        public SuiteMaintenance CurrentMaintainingRecord { get; set; }

        public SuiteMaintainingVm()
        {
            PageSizeList = new List<int>
           {
               20,
               40,
               80,
           };
            maintenaceClient = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();
            maintenaceClient.GetMaintenanceRecordsByStationFuzzyCompleted += maintenaceClient_GetMaintenanceRecordsByStationFuzzyCompleted;

            QueryCommand = new ActionCommand<object>(obj => QueryAction());
            SimpleMaintenanceCommand = new ActionCommand<object>(obj => SimpleMaintenance());
            SubstitutionMaintenanceCommand = new ActionCommand<object>(obj => SubstitutionMaintenance());
            ScrappedRegistrationCommand = new ActionCommand<object>(obj => ScrappedRegistration());
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            GetInfoByPageIndex();
           
        }

        private void SimpleMaintenance()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.SimpleMaintenanceV, new Dictionary<string, object>() { { "key", CurrentMaintainingRecord } }));
        }

        private void SubstitutionMaintenance()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.SubstitutionMaintenanceV, new Dictionary<string, object>() { { "key", CurrentMaintainingRecord } }));
        }

        private void ScrappedRegistration()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.ScrappedRegistrationV, new Dictionary<string, object>() { { "key", CurrentMaintainingRecord } }));
        }

        private void QueryAction()
        {
            fromQuery = true;
            Gsafety.PTMS.ServiceReference.MaitenanceRecordService.PagingInfo pageinfo = new Gsafety.PTMS.ServiceReference.MaitenanceRecordService.PagingInfo
            {
                PageIndex = 0,
                PageSize = PageSizeValue
            };
            maintenaceClient.GetMaintenanceRecordsByStationFuzzyAsync(InstallstationId, VehicleNumber,SuiteID, RepairStatusType.Repairing, pageinfo);
        }

        void maintenaceClient_GetMaintenanceRecordsByStationFuzzyCompleted(object sender, GetMaintenanceRecordsByStationFuzzyCompletedEventArgs e)
        {
            if (TotalCount != e.Result.TotalRecord)
                TotalCount = e.Result.TotalRecord;

            if (TotalCount > 0) //fromQuery && 
            {
                if (fromQuery)
                {
                    _PageIndex = 0;
                }

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageIndex));
            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"));
            }
            SuitMaitainingList = e.Result.Result;

        
        }


        private int totalCount;
        public int TotalCount
        {
            get { return totalCount; }
            set
            {
                totalCount = value;
                RaisePropertyChanged(() => TotalCount);
                initDataPager(value);
            }
        }

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
                QueryAction();
            }
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
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageIndex));
                GetInfoByPageIndex();
            }
        }

        private System.Windows.Data.PagedCollectionView _ItemCount;
        public PagedCollectionView ItemCount
        {
            get { return _ItemCount; }
            set
            {
                _ItemCount = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ItemCount));
            }
        }

         private bool fromQuery = true;

        private void initDataPager(int count)
        {
            List<int> itemCount = new List<int>();
            for (int i = 1; i <= count; i++)
            {
                itemCount.Add(i);
            }
            ItemCount = new PagedCollectionView(itemCount);
        }

        private void GetInfoByPageIndex()
        {
            fromQuery = false;
            Gsafety.PTMS.ServiceReference.MaitenanceRecordService.PagingInfo pageinfo = new Gsafety.PTMS.ServiceReference.MaitenanceRecordService.PagingInfo
            {
                PageIndex = PageIndex,
                PageSize = PageSizeValue
            };
            maintenaceClient.GetMaintenanceRecordsByStationFuzzyAsync(InstallstationId, VehicleNumber, SuiteID, RepairStatusType.Repairing, pageinfo);
        }
    }
}
