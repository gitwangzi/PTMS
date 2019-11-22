using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d6843386-b55e-4e51-8783-b6ee25210aa2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: InstalledRecordViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/16 16:19:17
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/16 16:19:17
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.InstalledRecordVm)]
    public class InstalledRecordViewModel : ListViewModel<InstallationInfo>
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
            DeviceInstallServiceClient deviceInstallServiceClient = InitClient();
            deviceInstallServiceClient.GetInstallationFinishedEx1Async(ApplicationContext.Instance.AuthenticationInfo.ClientID, CarNumber, SuiteId, Installer, stations, BeginDate, EndDate, pageinfo);
        }
        #region propperty...
        private int _PageIndex = -1;
        public int PageIndex
        {
            get
            {
                return _PageIndex;
            }
            set
            {
                if (_PageIndex == -1)
                {
                    _PageIndex = value;
                }
                else
                {
                    _PageIndex = value;
                    GetInfoByPageIndex();
                }
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

        private DateTime? _BeginDate = null;
        public DateTime? BeginDate
        {
            get
            {
                return _BeginDate;
            }
            set
            {
                _BeginDate = value;
                string beg = _BeginDate == null ? null : BeginDate.ToString();
                ValidateBeginDate(ExtractPropertyName(() => BeginDate), beg);
                if (BeginDate != null && EndDate != null)
                    ValidateBeginAndEndDate(ExtractPropertyName(() => BeginDate), (DateTime)BeginDate, ExtractPropertyName(() => EndDate), (DateTime)EndDate);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => BeginDate));
            }
        }

        private void ValidateBeginDate(string prop, string value)
        {
            ClearErrors(prop);
            if (BeginDate.HasValue)
            {
                IsTrue = true;
                if (BeginDate > EndDate)
                {
                    IsTrue = false;
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("TimeError"));
                }
            }
        }

        private bool _isTrue = true;
        public bool IsTrue
        {
            get { return _isTrue; }
            set
            {
                _isTrue = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsTrue));
            }
        }

        private DateTime? _EndDate;
        public DateTime? EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                if (value.HasValue)
                {
                    _EndDate = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day, 23, 59, 59);                  
                    ValidateBeginDate(ExtractPropertyName(() => EndDate), _EndDate.ToString());
                    if (BeginDate != null && EndDate != null)
                        ValidateBeginAndEndDate(ExtractPropertyName(() => BeginDate), (DateTime)BeginDate, ExtractPropertyName(() => EndDate), (DateTime)EndDate);
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => EndDate));
                }
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

        #endregion
        private void GetInfoByPageIndex()
        {
            try
            {
                PagingInfo pageinfo = new PagingInfo
                  {
                      PageIndex = PageIndex,
                      PageSize = PageSizeValue
                  };
                DeviceInstallServiceClient deviceInstallServiceClient = InitClient();
                deviceInstallServiceClient.GetInstallationFinishedEx1Async(ApplicationContext.Instance.AuthenticationInfo.ClientID, CarNumber, SuiteId, Installer, stations, BeginDate, EndDate, pageinfo);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            try
            {
                PagingInfo pageinfo = new PagingInfo
                  {
                      PageIndex = PageIndex,
                      PageSize = PageSizeValue
                  };
                DeviceInstallServiceClient deviceInstallServiceClient = InitClient();
                deviceInstallServiceClient.GetInstallationFinishedEx1Async(ApplicationContext.Instance.AuthenticationInfo.ClientID, CarNumber, SuiteId, Installer, stations, BeginDate, EndDate, pageinfo);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        //DeviceInstallServiceClient deviceInstallServiceClient = null;
        public IActionCommand QueryCommand { get; private set; }
        ObservableCollection<string> stations = null;

        private DeviceInstallServiceClient InitClient()
        {
            DeviceInstallServiceClient deviceInstallServiceClient = ServiceClientFactory.Create<DeviceInstallServiceClient>();
            deviceInstallServiceClient.GetInstallationFinishedEx1Completed += deviceInstallServiceClient_GetInstallationFinishedEx1Completed;

            return deviceInstallServiceClient;
        }

        public InstalledRecordViewModel()
        {
            try
            {
                this.CurrentInstallRecord = new InstallationInfo();
                // deviceInstallServiceClient = ServiceClientFactory.Create<DeviceInstallServiceClient>();
                stations = new ObservableCollection<string>();
                foreach (var item in ApplicationContext.Instance.AuthenticationInfo.Stations)
                {
                    stations.Add(item.ID);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InstalledRecordViewModel()", ex);
            }
            EndDate = DateTime.Now;
            PageSizeList = new List<int>
           {
               20,
               40,
               80,
           };

            QueryCommand = new ActionCommand<object>(obj => Query());
            // deviceInstallServiceClient.GetInstallationFinishedEx1Completed += deviceInstallServiceClient_GetInstallationFinishedEx1Completed;
            PagingInfo pageinfo = new PagingInfo
            {
                PageIndex = PageIndex,
                PageSize = PageSizeValue
            };
            DeviceInstallServiceClient deviceInstallServiceClient = InitClient();
            deviceInstallServiceClient.GetInstallationFinishedEx1Async(ApplicationContext.Instance.AuthenticationInfo.ClientID, CarNumber, SuiteId, Installer, stations, BeginDate, EndDate, pageinfo);
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
                DateTime? begintime = null;
                if (BeginDate.HasValue)
                {
                    begintime = BeginDate.Value.ToUniversalTime();
                }

                DateTime? endtime = null;
                if (EndDate.HasValue)
                {
                    if (EndDate.Value > DateTime.Now.Date.AddDays(1))
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Rpt_EndTimeError"));
                        return;
                    }
                    endtime = EndDate.Value.ToUniversalTime();
                }
                DeviceInstallServiceClient deviceInstallServiceClient = InitClient();
                deviceInstallServiceClient.GetInstallationFinishedEx1Async(ApplicationContext.Instance.AuthenticationInfo.ClientID, CarNumber, SuiteId, Installer, stations, begintime, endtime, pageinfo);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        public bool PageChanged = false;

        private InstallationInfo _CurrentInstallRecord;
        public InstallationInfo CurrentInstallRecord
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


        void deviceInstallServiceClient_GetInstallationFinishedEx1Completed(object sender, GetInstallationFinishedEx1CompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        if ((totalCount != e.Result.TotalRecord) || PageChanged)
                            totalCount = e.Result.TotalRecord;
                        SuiteModels_Imps = e.Result.Result;
                        foreach (var item in SuiteModels_Imps)
                        {
                            if (item.FinishTime.HasValue)
                            {
                                item.FinishTime = item.FinishTime.Value.ToLocalTime();
                            }
                        }
                        PageChanged = false;
                        //if (totalCount == 0)
                        //{
                        //    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"));
                        //}
                        if (SuiteModels_Imps.Any())
                        {
                            CurrentInstallRecord = SuiteModels_Imps[0];
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InstalledRecordViewModel", ex);
            }
            finally
            {
                DeviceInstallServiceClient client = sender as DeviceInstallServiceClient;
                if (client != null)
                {
                    client.CloseAsync();
                    client = null;
                }
            }
        }
    }
}
