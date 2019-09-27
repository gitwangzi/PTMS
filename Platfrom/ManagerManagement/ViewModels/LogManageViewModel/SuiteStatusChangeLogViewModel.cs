using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Gsafety.PTMS.Spreadsheet;
using System.Linq;
using System.Reflection;
using BaseLib.ViewModels;
using BaseLib.Model;

namespace Gsafety.PTMS.Manager.ViewModels.LogManageViewModel
{
    [ExportAsViewModel(ManagerName.SuiteStatusChangeLogViewModel)]
    public class SuiteStatusChangeLogViewModel : BaseViewModel
    {
        GetSuiteInfoServiceClient suiteInfoClient;
        private int currentIndex = 1;
        private DateTime startTime;
        private DateTime endTime;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Suite_ID { get; set; }
        public PagedServerCollection<SuiteInfoLog> SuiteInfoPage { get; set; }
        public List<int> PageSizeList { get; set; }
        private bool isFirst;
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
        public bool ExportBtnStatus { get; set; }
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand ExportCommand { get; private set; }
        public SuiteStatusChangeLogViewModel()
        {
            try
            {
                isFirst = true;
                suiteInfoClient = ServiceClientFactory.Create<GetSuiteInfoServiceClient>();
                suiteInfoClient.GetSuitInfoCompleted += suiteInfoClient_GetSuitInfoCompleted;
                QueryCommand = new ActionCommand<object>(obj => Query());
                ExportCommand = new ActionCommand<object>(obj => Export());
                PageSizeValue = 20;
                PageSizeList = new List<int> { 20, 40, 80 };
                UInit();
                InitPagedServerCollection();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
 
        public void Export()
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
                    GetSuiteInfoServiceClient Client = ServiceClientFactory.Create<GetSuiteInfoServiceClient>();
                    Client.GetSuitInfoCompleted += (s, e) =>
                        {
                            if (e.Result != null && e.Result.TotalRecord > 0)
                            {
                                List<string> Codes = new List<string>();
                                Codes.Add("Suite_ID");
                                Codes.Add("CurrentStatus");
                                Codes.Add("ChangedStatus");
                                Codes.Add("OperatingTime");
                                Codes.Add("Operator");
                                Codes.Add("OperatingReason");

                                List<string> Names = new List<string>();
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Info_SuiteID"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("SuiteCurrentStatus"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("SuiteChangedStatus"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_OperationTime"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Info_Operator"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Info_Reason"));

                                List<Gsafety.PTMS.Spreadsheet.EnumsEx> eList = new List<Gsafety.PTMS.Spreadsheet.EnumsEx>();
                                List<FieldEx> FieldEx1 = new List<FieldEx>();

                                Gsafety.PTMS.Bases.Enums.CurrentStatusConverter convert = new Bases.Enums.CurrentStatusConverter();

                                convert.EnumInfo.ToList().ForEach(x =>
                                    {
                                        FieldEx item = new FieldEx { Key = x.Name.ToString(), Value = x.LocalizedString };
                                        FieldEx1.Add(item);
                                    });
                                eList.Add(new Gsafety.PTMS.Spreadsheet.EnumsEx { Code = "CurrentStatus", Content = FieldEx1 });


                                List<FieldEx> FieldEx2 = new List<FieldEx>();
                                convert.EnumInfo.ToList().ForEach(x =>
                                {
                                    FieldEx item = new FieldEx { Key = x.Name.ToString(), Value = x.LocalizedString };
                                    FieldEx2.Add(item);
                                });
                                eList.Add(new Gsafety.PTMS.Spreadsheet.EnumsEx { Code = "ChangedStatus", Content = FieldEx2 });

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
                    if (SuiteInfoPage.TotalItemCount > 10000)
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = 1, PageSize = 10000 };
                        Client.GetSuitInfoAsync(Suite_ID, startTime, endTime, pagingInfo);
                    }
                    else
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = -1, PageSize = 0 };
                        Client.GetSuitInfoAsync(Suite_ID, startTime, endTime, pagingInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void UInit()
        {
            try
            {
                StartTime = DateTime.Now.AddDays(-1);
                EndTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void InitPagedServerCollection()
        {                   
            SuiteInfoPage = new PagedServerCollection<SuiteInfoLog>(new Action<int, int>(InvokServer));
        }
        private void InvokServer(int pageIndex, int pageSize)
        {
            try
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                startTime = StartTime.Date;
                endTime = EndTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                suiteInfoClient.GetSuitInfoAsync(Suite_ID, startTime, endTime, pagingInfo);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        void suiteInfoClient_GetSuitInfoCompleted(object sender, GetSuitInfoCompletedEventArgs e)
        {
            try
            {
                SuiteInfoPage.loader_Finished(new PagedResult<SuiteInfoLog>
                  {
                      Count = e.Result.TotalRecord,
                      Items = e.Result.Result,
                      PageIndex = currentIndex,
                  });
                if (e.Result.TotalRecord == 0&&!isFirst)
                {
                    setExportBtnStatus(false);
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                else
                {
                    setExportBtnStatus(true);
                }
                isFirst = false;
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void setExportBtnStatus(bool Flag)
        {
            try
            {
                ExportBtnStatus = Flag;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ExportBtnStatus));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        public void Query()
        {
            try
            {
                SuiteInfoPage.MoveToFirstPage();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}

