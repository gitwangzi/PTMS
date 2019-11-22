/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: e94a5149-02e1-4239-b28a-0c1d06aac47f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
/////    Project Description:    
/////             Class Name: RuleSettingLogViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/12 14:12:00
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/12 14:12:00
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
using Gsafety.PTMS.ServiceReference.CommandManageService;
using System.Collections.Generic;
using Jounce.Core.Command;
using Gsafety.PTMS.Share;
using Jounce.Framework.Command;
using System.Reflection;
using System.Linq;
using System.Collections;
using BaseLib.ViewModels;
using BaseLib.Model;
namespace Gsafety.PTMS.Manager.ViewModels.LogManageViewModel
{
    [ExportAsViewModel(ManagerName.RuleSettingLogVM)]
    public class RuleSettingLogViewModel : BaseViewModel
    {
        private CommandManageServiceClient commandMangeServiceClient;
        public IActionCommand ExportCommand { get; private set; }
        public CurrentSettingRuleInfo RuleSettingLogInfos { get; set; }
        public string VehicleId { get; set; }
        public bool ExportBtnStatus { get; set; }
        public PagedServerCollection<CurrentSettingRuleInfo> PSC_LogInfo { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.PSC_LogInfo != null)
                {
                    this.PSC_LogInfo.PageSize = value;
                }
            }
        }
        public List<int> PageSizeList { get; set; }
        public IActionCommand QueryCommand { get; private set; }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
        }

        public RuleSettingLogViewModel()
        {
            try
            {
                commandMangeServiceClient = ServiceClientFactory.Create<CommandManageServiceClient>();
                ExportCommand = new ActionCommand<object>(obj => Export());
                PageSizeList = ManagerCommon.PageSizeList;
                QueryCommand = new ActionCommand<object>(obj => Query());
                PageSizeValue = PageSizeList[0];
                InitPagedServerCollection();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void commandMangeServiceClient_GetAllRuleSettingLogInfoCompleted(object sender, GetAllRuleSettingLogInfoCompletedEventArgs e)
        {
            try
            {
                PSC_LogInfo.loader_Finished(new PagedResult<CurrentSettingRuleInfo>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex,
                });
                if (e.Result.TotalRecord == 0)
                {
                    setExportBtnStatus(false);
                   // MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                else
                {
                    setExportBtnStatus(true);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("RuleSettingLogVm", ex);
            }
        }

        private int currentIndex = 1;
        private void InitPagedServerCollection()
        {
            commandMangeServiceClient.GetAllRuleSettingLogInfoCompleted += commandMangeServiceClient_GetAllRuleSettingLogInfoCompleted;
            PSC_LogInfo = new PagedServerCollection<CurrentSettingRuleInfo>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                PagingInfo pageInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                commandMangeServiceClient.GetAllRuleSettingLogInfoAsync(ApplicationContext.Instance.AuthenticationInfo.UserShowName.ToLower(), VehicleId, pageInfo);
            });
        }

        private void Query()
        {
            currentIndex = 1;
            PSC_LogInfo.MoveToFirstPage();
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
                    CommandManageServiceClient client = ServiceClientFactory.Create<CommandManageServiceClient>();
                    client.GetAllRuleSettingLogInfoCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord > 0)
                        {
                            List<string> Codes = new List<string>();
                            Codes.Add("RuleName");
                            Codes.Add("VehicleID");
                            Codes.Add("MdvrCoreID");
                            Codes.Add("SendTime");
                            Codes.Add("CommandType");
                            Codes.Add("CommandStatus");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_RuleName"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_VehicleNumber"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("SuiteID"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Send_Time"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Command_Type"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Command_Status"));


                            List<Spreadsheet.EnumsEx> eList = new List<Spreadsheet.EnumsEx>();

                            List<Spreadsheet.FieldEx> FieldEx1 = new List<Spreadsheet.FieldEx>();

                            FieldEx1.Add(new Spreadsheet.FieldEx { Key = "C30", Value = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_GpsSetting") });
                            FieldEx1.Add(new Spreadsheet.FieldEx { Key = "C64", Value = ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_TemperatureSetting") });
                            FieldEx1.Add(new Spreadsheet.FieldEx { Key = "C82", Value = ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_AbnormalDoor_Setting") });
                            FieldEx1.Add(new Spreadsheet.FieldEx { Key = "C78", Value = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_AlarmSetting") });
                            FieldEx1.Add(new Spreadsheet.FieldEx { Key = "C80", Value = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_AlarmSetting") });
                            FieldEx1.Add(new Spreadsheet.FieldEx { Key = "C107", Value = ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_ElectricFence") });
                            FieldEx1.Add(new Spreadsheet.FieldEx { Key = "C68", Value = ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_SpeedLimit") });

                            eList.Add(new Gsafety.PTMS.Spreadsheet.EnumsEx { Code = "CommandType", Content = FieldEx1 });

                            Spreadsheet.XLSXExporter xlsx = new Spreadsheet.XLSXExporter();
                            xlsx.Export(e.Result.Result.ToList(), dlg.OpenFile(), Codes, Names, eList);
                            setExportBtnStatus(true);
                            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Manager_LogExportSucceed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                        }
                        else
                        {
                            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Manager_LogExportFailed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                        }
                    };

                    if (PSC_LogInfo.TotalItemCount > 10000)
                    {
                        PagingInfo pageInfo = new PagingInfo() { PageIndex = 1, PageSize = 10000 };
                        client.GetAllRuleSettingLogInfoAsync(ApplicationContext.Instance.AuthenticationInfo.UserShowName.ToLower(), VehicleId, pageInfo);
                    }
                    else
                    {
                        PagingInfo pageInfo = new PagingInfo() { PageIndex = -1, PageSize = 0 };
                        client.GetAllRuleSettingLogInfoAsync(ApplicationContext.Instance.AuthenticationInfo.UserShowName.ToLower(), VehicleId, pageInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                setExportBtnStatus(true);
            }
        }

        private void setExportBtnStatus(bool Flag)
        {
            ExportBtnStatus = Flag;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ExportBtnStatus));
        }
    }
}
