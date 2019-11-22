/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////Guid: 
///// clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
///// Machine Name: PC-SHIHS
///// Author: (ShiHongsheng)
/////======================================================================
///// Project Name: Gsafety.PTMS.Manager.ViewModels.LogManageViewModel
///// Project Description:    
/////Class Name: CarAlertLogViewModel
///// Class Version: v1.0.0.0
///// Create Time: 2013/9/17 00:00:00
/////Class Description:  
/////======================================================================
/////Modified Time: 2013/9/17 00:00:00
/////Modified by:
/////Modified Description: 
/////======================================================================
using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using Gsafety.PTMS.Share;
using Gsafety.Common.Localization.Resource;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Windows;
using FiftyNine.Ag.OpenXML.Common.Storage;
using System.Linq;
using System.Windows.Controls;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Spreadsheet;
using System.Reflection;
using BaseLib.ViewModels;
using BaseLib.Model;

namespace Gsafety.PTMS.Manager.ViewModels.LogManageViewModel
{
    [ExportAsViewModel(ManagerName.CarAlertLogViewModel)]
    public class CarAlertLogViewModel : BaseViewModel
    {
        private int currentIndex = 1;
        CarAlertDealLogServiceClient ptmsLogClient;

        #region DataProperties & CommandProperties
        private string vehicleId;
        private string username;
        private DateTime starttime;
        private DateTime endtime;
        public bool ExportBtnStatus { get; set; }
        public string VehicleId { get; set; }  //Bingding view item
        public string UserName { get; set; }  //Bingding view item
        public DateTime StartTime { get; set; } //Bingding view item
        public DateTime EndTime { get; set; }  //Bingding view item
        public CarAlertLogInfo CurrentLog { get; set; }
        public PagedServerCollection<CarAlertLogInfo> LogInfoPage { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(new string[] { "LogInfoPage", "PageSizeValue" }));
            }
        }

        public List<int> PageSizeList { get; set; }
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand ExportCommand { get; private set; }

        /// <summary>
        /// wait status
        /// </summary>
        private bool _IsBusy = false;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                _IsBusy = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsBusy));
            }
        }
        #endregion

        #region Constructions & Initralization
        //Ctor
        public CarAlertLogViewModel()
        {
            try
            {
                ptmsLogClient = ServiceClientFactory.Create<CarAlertDealLogServiceClient>();

                QueryCommand = new ActionCommand<object>(obj => Query());
                ExportCommand = new ActionCommand<object>(obj => Export());
                PageSizeValue = 20;
                PageSizeList = new List<int> { 20, 40, 80 };
                UIInit();
                starttime = StartTime;
                endtime = EndTime;
                InitPagedServerCollection();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        //Init
        private void InitPagedServerCollection()
        {
            try
            {
                ptmsLogClient.GetCarAlertDealLogCompleted += antLogClient_GetCarAlertDealLogCompleted;
                LogInfoPage = new PagedServerCollection<CarAlertLogInfo>(new Action<int, int>(InvokServer));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        //Bingding view's values
        private void UIInit()
        {
            StartTime = DateTime.Now.AddMonths(-1);
            EndTime = DateTime.Now;
        }
        #endregion

        #region Functions & Events
        //serach
        private void Query()
        {
            try
            {
                vehicleId = string.IsNullOrEmpty(VehicleId) ? string.Empty : VehicleId.Trim();
                username = string.IsNullOrEmpty(UserName) ? string.Empty : UserName.Trim();
                starttime = StartTime;
                endtime = EndTime;
                if (new LogHelper().SeearchConditionValid(StartTime, EndTime))
                {
                    currentIndex = 1;
                    LogInfoPage.MoveToFirstPage();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        //Invoke Server
        private void InvokServer(int pageIndex, int pageSize)
        {
            IsBusy = true;
            pageSize = PageSizeValue;
            System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
            string districtCode = string.Empty;
            PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
            try
            {
                //ptmsLogClient.GetCarAlertDealLogAsync(vehicleId, username, starttime, endtime, pagingInfo);
            }
            catch
            {
                new LogHelper().RomoteError();
            }
        }
        //Respose Server event
        private void antLogClient_GetCarAlertDealLogCompleted(object sender, GetCarAlertDealLogCompletedEventArgs e)
        {
            try
            {
                LogInfoPage.loader_Finished(new PagedResult<CarAlertLogInfo>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex,
                });
                if (e.Result.TotalRecord == 0)
                {
                    setExportBtnStatus(false);
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                else
                {
                    setExportBtnStatus(true);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
                //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"));
            }
            IsBusy = false;
        }
        //RefreshPage View
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                if (viewParameters.Count != 0 && viewParameters["action"].ToString() == "return")
                {
                    LogInfoPage.ToPage(currentIndex);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void setExportBtnStatus(bool Flag)
        {
            ExportBtnStatus = Flag;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ExportBtnStatus));
        }

        private void Export()
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Excel Document (.xlsx)|*.xlsx";
                dlg.DefaultExt = ".xlsx";
                bool? dialogResult = dlg.ShowDialog();
                if (dialogResult == true)
                {
                    setExportBtnStatus(false);
                    CarAlertDealLogServiceClient client = ServiceClientFactory.Create<CarAlertDealLogServiceClient>();
                    client.GetCarAlertDealLogCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord > 0)
                        {
                            List<string> Codes = new List<string>();
                            Codes.Add("CarNumber");
                            Codes.Add("VihcleType");
                            Codes.Add("DealTime");
                            Codes.Add("DealPerson");
                            Codes.Add("AlertTime");
                            Codes.Add("AlertType");
                            Codes.Add("DealContent");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_VehicleID"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_VehicleType"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_DealTime"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_DealPerson"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_AlertTime"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_AlertType"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_DealContent"));

                            List<Gsafety.PTMS.Spreadsheet.EnumsEx> eList = new List<Gsafety.PTMS.Spreadsheet.EnumsEx>();

                            List<FieldEx> FieldEx1 = new List<FieldEx>();
                            Gsafety.PTMS.Bases.Enums.VehicleConverter vc = new Gsafety.PTMS.Bases.Enums.VehicleConverter();
                            vc.EnumInfo.ToList().ForEach(x =>
                            {
                                FieldEx item = new FieldEx { Key = x.Value.ToString(), Value = x.LocalizedString };
                                FieldEx1.Add(item);
                            });
                            eList.Add(new Gsafety.PTMS.Spreadsheet.EnumsEx { Code = "VihcleType", Content = FieldEx1 });

                            List<FieldEx> FieldEx2 = new List<FieldEx>();
                            Gsafety.PTMS.Bases.Enums.BusinessAlertTypeConverter batc = new BusinessAlertTypeConverter();
                            batc.EnumInfo.ToList().ForEach(x =>
                                {
                                    FieldEx item = new FieldEx { Key = x.Value.ToString(), Value = x.LocalizedString };
                                    FieldEx2.Add(item);
                                });
                            eList.Add(new Gsafety.PTMS.Spreadsheet.EnumsEx { Code = "AlertType", Content = FieldEx2 });

                            XLSXExporter xlsx = new XLSXExporter();
                            xlsx.Export(e.Result.Result.ToList(), dlg.OpenFile(), Codes, Names, eList);
                            setExportBtnStatus(true);
                            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Manager_LogExportSucceed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                        }
                        else
                        {
                            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Manager_LogExportFailed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                        }
                    };

                    if (LogInfoPage.TotalItemCount > 10000)
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = 1, PageSize = 10000 };
                        //client.GetCarAlertDealLogAsync(vehicleId, username, starttime, endtime, pagingInfo);
                    }
                    else
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = -1, PageSize = 0 };
                        //client.GetCarAlertDealLogAsync(vehicleId, username, starttime, endtime, pagingInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        #endregion
    }
}
