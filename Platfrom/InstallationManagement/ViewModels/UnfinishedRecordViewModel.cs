using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Constants;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: fd28a301-dc4a-4358-abc4-f2192830cf36      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: UnfinishedRecordViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/16 16:21:31
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/16 16:21:31
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;


namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.UnfinishedRecordVm)]
    public class UnfinishedRecordViewModel : ListViewModel<InstallationInfo>
    {
        #region
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
            Gsafety.PTMS.ServiceReference.DeviceInstallService.PagingInfo pageinfo = new Gsafety.PTMS.ServiceReference.DeviceInstallService.PagingInfo
            {
                PageIndex = 0,
                PageSize = PageSizeValue
            };
            deviceInstallServiceClient.GetInstallationInProgressExAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, CarNumber, SuiteId, Installer, InstallstationId, BeginDate, EndDate, pageinfo);
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

        public ObservableCollection<string> InstallstationId
        {
            get
            {
                ObservableCollection<string> ids = new ObservableCollection<string>();
                foreach (var item in ApplicationContext.Instance.AuthenticationInfo.Stations)
                {
                    ids.Add(item.ID);
                }
                return ids;
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
                {
                    ValidateBeginAndEndDate(ExtractPropertyName(() => BeginDate), (DateTime)BeginDate, ExtractPropertyName(() => EndDate), EndDate);
                }
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

        private DateTime _EndDate;
        public DateTime EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                _EndDate = new DateTime(value.Year, value.Month, value.Day, 23, 59, 59);
                ValidateBeginDate(ExtractPropertyName(() => EndDate), _EndDate.ToString());
                if (BeginDate != null && EndDate != null)
                {
                    ValidateBeginAndEndDate(ExtractPropertyName(() => BeginDate), (DateTime)BeginDate, ExtractPropertyName(() => EndDate), EndDate);
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => EndDate));
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

        private void GetInfoByPageIndex()
        {
            Gsafety.PTMS.ServiceReference.DeviceInstallService.PagingInfo pageinfo = new Gsafety.PTMS.ServiceReference.DeviceInstallService.PagingInfo
            {
                PageIndex = PageIndex,
                PageSize = PageSizeValue
            };
            deviceInstallServiceClient.GetInstallationInProgressExAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, CarNumber, SuiteId, Installer, InstallstationId, BeginDate, EndDate, pageinfo);
        }

        DeviceInstallServiceClient deviceInstallServiceClient = null;
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand ViewCommand { get; private set; }
        public IActionCommand DeleteCommand { get; private set; }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            Gsafety.PTMS.ServiceReference.DeviceInstallService.PagingInfo pageinfo = new Gsafety.PTMS.ServiceReference.DeviceInstallService.PagingInfo
            {
                PageIndex = PageIndex,
                PageSize = PageSizeValue
            };
            deviceInstallServiceClient.GetInstallationInProgressExAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, CarNumber, SuiteId, Installer, InstallstationId, BeginDate, EndDate, pageinfo);
        }
        #endregion

        public UnfinishedRecordViewModel()
        {
            try
            {
                deviceInstallServiceClient = ServiceClientFactory.Create<DeviceInstallServiceClient>();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UnfinishedRecordViewModel()", ex);
            }
            EndDate = DateTime.Now;
            PageSizeList = new List<int>
            {
               20,
               40,
               80,
            };
            QueryCommand = new ActionCommand<object>(obj => Query());
            ViewCommand = new ActionCommand<object>(obj => View());
            DeleteCommand = new ActionCommand<object>(obj => delete());
            deviceInstallServiceClient.GetInstallationInProgressExCompleted += deviceInstallServiceClient_GetInstallationInProgressExCompleted;
            deviceInstallServiceClient.SubmitForDeleteCompleted += deviceInstallServiceClient_SubmitForDeleteCompleted;
            Gsafety.PTMS.ServiceReference.DeviceInstallService.PagingInfo pageinfo = new Gsafety.PTMS.ServiceReference.DeviceInstallService.PagingInfo
            {
                PageIndex = PageIndex,
                PageSize = PageSizeValue
            };
            deviceInstallServiceClient.GetInstallationInProgressExAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, CarNumber, SuiteId, Installer, InstallstationId, BeginDate, EndDate, pageinfo);
        }

        private void delete()
        {
            var dialog = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_IfDelete"), MessageDialogButton.OkAndCancel);
            dialog.Closed += dialog_Closed;
        }

        void dialog_Closed(object sender, EventArgs e)
        {
            var dialog = sender as ChildWindow;
            if (dialog.DialogResult == true)
            {
                DeletePackage package = new DeletePackage();
                package.SuiteKey = CurrentInstallRecord.DeviceKey;
                package.InstallID = CurrentInstallRecord.Id;

                deviceInstallServiceClient.SubmitForDeleteAsync(package);
            }
        }

        void deviceInstallServiceClient_SubmitForDeleteCompleted(object sender, SubmitForDeleteCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
                else
                {
                    if (e.Result.IsSuccess == false)
                    {
                        if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ErrorMsg);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ExceptionMessage);
                        }
                    }
                    else
                    {

                        if (string.IsNullOrEmpty(e.Result.Result))
                        {
                            ApplicationContext.Instance.MessageClient.RemoveInstallSuite(CurrentInstallRecord.DeviceCoreId);
                            Query();
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_DeleteFailed"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UnfinishedRecordViewModel", ex);
            }
        }

        public bool PageChanged = false;
        void deviceInstallServiceClient_GetInstallationInProgressExCompleted(object sender, GetInstallationInProgressExCompletedEventArgs e)
        {
            try
            {
                if ((totalCount != e.Result.TotalRecord) || PageChanged)
                    totalCount = e.Result.TotalRecord;
                SuiteModels_Imps = e.Result.Result;
                foreach (var item in SuiteModels_Imps)
                {
                    item.CreateTime = item.CreateTime.Value.ToLocalTime();
                    if (item.FinishTime.HasValue)
                    {
                        item.FinishTime.Value.ToLocalTime();
                    }

                    item.CheckStep = item.CheckStep + 1;
                }
                if (SuiteModels_Imps.Count != 0)
                {
                    CurrentInstallRecord = SuiteModels_Imps[0];
                }

                PageChanged = false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UnfinishedRecordViewModel", ex);
            }
        }

        private void View()
        {
            try
            {
                string view = string.Empty;
                switch (CurrentInstallRecord.CheckStep)
                {
                    case 2:
                        view = InstallationName.InstallSuiteCheckV;
                        break;
                    case 3:
                        view = InstallationName.InstallVehcileSuiteCheckV;
                        break;
                    case 4:
                        view = InstallationName.InstallInitiateSuiteV;
                        break;
                    case 5:
                        view = InstallationName.InstallSuiteFunctionCheckV;
                        break;
                    case 6:
                        view = InstallationName.InstallConfirmV;
                        break;
                    default:
                        view = InstallationName.InstallVehicleCheckV;
                        break;
                }
                ApplicationContext.Instance.NavigateManager.Navigate(view, NavigationFrame.InstallContentFrame);
                EventAggregator.Publish(new ViewNavigationArgs(view, new Dictionary<string, object>() { { "ID", CurrentInstallRecord.Id } }));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void Query()
        {
            try
            {
                if (EndDate != null)
                {
                    if (_EndDate > DateTime.Now.Date.AddDays(1))
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Rpt_EndTimeError"));
                        return;
                    }
                }

                Gsafety.PTMS.ServiceReference.DeviceInstallService.PagingInfo pageinfo = new Gsafety.PTMS.ServiceReference.DeviceInstallService.PagingInfo
                   {
                       PageIndex = PageIndex,
                       PageSize = PageSizeValue
                   };
                deviceInstallServiceClient.GetInstallationInProgressExAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, CarNumber, SuiteId, Installer, InstallstationId, BeginDate, EndDate, pageinfo);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

    }
}
