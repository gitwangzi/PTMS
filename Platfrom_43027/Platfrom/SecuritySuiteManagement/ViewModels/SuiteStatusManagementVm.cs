using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Core.View;
using Jounce.Framework.Command;
using System.Collections.ObjectModel;
using Gsafety.PTMS.SecuritySuite;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.ServiceReference.VehicleStatusService;
using Gsafety.PTMS.SecuritySuite.Views;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.SecuritySuite.Models;
using System.Reflection;
using BaseLib.ViewModels;
using BaseLib.Model;



namespace Gsafety.PTMS.SecuritySuite.ViewModels
{
    [ExportAsViewModel(SecuritySuiteName.SuiteStatusManagementVm)]
    public class SuiteStatusManagementVm:BaseViewModel
    {
        public List<int> PageSizeList { get; set; }
        private int pageSizeValue;
        private int currentIndex = 1;
        VehicleStatusServiceClient client;
       
        #region properties
        public PagedServerCollection<InitialSuiteMangement> SuiteStatusInfoPage { get; set; }
        public int SuiteStatus { get;set;}
        public string SuiteId { get; set; }
        public List<EnumModel> SuiteStatusList{ get ;set;}
        public IActionCommand DeleteCommand { get; private set; }
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand OpCommand { get; private set; }
        public InitialSuiteMangement CurrentSuiteStatus { get; set; }
        public ICommand InfoSuiteStatusCommand { get; private set; }
        #endregion
        public SuiteStatusManagementVm()
        {
            try
            {
                client = ServiceClientFactory.Create<VehicleStatusServiceClient>();
                client.GetSuiteStatusManagementCompleted += client_GetSuiteStatusManagementCompleted;
                QueryCommand = new ActionCommand<object>(obj => Query());
                OpCommand = new ActionCommand<object>(obj => OperaSta(obj));
                PageSizeList = Gsafety.PTMS.BaseInformation.BaseInformationCommon.PageSizeList;
                PageSizeValue = PageSizeList[0];
                InitPagedServerCollection();
                getSuiteStatusManagement();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SuiteStatusManagementVm()", ex);
            }
        }
         public Visibility MenuShow
        {
            get
            {
                return Visibility.Visible;
            }
         }  

        private void InitPagedServerCollection()
        {
            SuiteStatusInfoPage = new PagedServerCollection<InitialSuiteMangement>(new Action<int, int>(InvokServer));
            
        }
        private void OperaSta(object obj)
        {
            try
            {
                SuiteStatusControl suiteStatus = new SuiteStatusControl();
                suiteStatus.SuiteID = CurrentSuiteStatus.SuiteID;
                suiteStatus.VehicleID = CurrentSuiteStatus.VehicleID;
                suiteStatus.CurrentStatus = (short)CurrentSuiteStatus.CurrentStatus;
                suiteStatus.ChangedStatus = Convert.ToInt16(obj);
                suiteStatus.MdvrCID = CurrentSuiteStatus.MdvrCoreId;
                suiteStatus.Suite_info_id = CurrentSuiteStatus.SuiteINFOID;
                InfoSuiteStatus suiteChildWindow = new InfoSuiteStatus(suiteStatus);
                suiteChildWindow.ResultAction = (result) => InfoSuiteStatusAction(result);//test
                suiteChildWindow.Show();
                suiteChildWindow.QueryAction(suiteStatus.VehicleID, suiteStatus.SuiteID, suiteStatus.CurrentStatus, suiteStatus.ChangedStatus, suiteStatus.MdvrCID);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("OperaSta()", ex);
            }
       }
        private void InfoSuiteStatus_Closed(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InfoSuiteStatus_Closed()", ex);
            }

        }

        private void InfoSuiteStatusAction(bool result)
        {
            try
            {
                if (result)
                {
                    this.InvokServer(SuiteStatusInfoPage.PageIndex + 1, pageSizeValue);
                }
                else
                {
                    MessageBox.Show("todo");
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InfoSuiteStatusAction()", ex);
            }
        }
        //Invoke Server
        private void InvokServer(int pageIndex, int pageSize)
        {
            try
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                client.GetSuiteStatusManagementAsync(SuiteId, SuiteStatus, pagingInfo);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InvokServer()", ex);
            }
        }     
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.SuiteStatusInfoPage != null)
                {
                    this.SuiteStatusInfoPage.PageSize = value;
                }
            }
         }
        
        //Response server
        void client_GetSuiteStatusManagementCompleted(object sender, GetSuiteStatusManagementCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(GetType().FullName, e.Error);
                    return;
                }
                SuiteStatusInfoPage.loader_Finished(new PagedResult<InitialSuiteMangement>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex,
                });
                if (e.Result.TotalRecord == 0)
                {
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_GetSuiteStatusManagementCompleted()", ex);
            }
        }
        private void getSuiteStatusManagement()
        {
            try
            {
                SuiteStatusList = new List<EnumModel>();
                SuiteStatusList.Add(new EnumModel { ShowName = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_All"), EnumName = string.Empty, EnumValue = 0 });
                foreach (var item in Enum.GetNames(typeof(CurrentStatus)))
                {
                    int statusValue = (int)Enum.Parse(typeof(CurrentStatus), item, true);
                    SuiteStatusList.Add(new EnumModel { EnumName = item, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(item), EnumValue = statusValue });
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("getSuiteStatusManagement()", ex);
            }
        }
        private void Query()
        {
            try
            {
                SuiteStatusInfoPage.MoveToFirstPage();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("Query()", ex);
            }
        }
      
    }

}
