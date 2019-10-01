using Gsafety.Ant.BaseInformation.Views;
using Gsafety.Common.Controls;
using Gsafety.Common.Converts;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.ServiceReference.DevGpsService;
//using Gsafety.PTMS.ServiceReference.InstallStationService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Spreadsheet;
using Jounce.Core.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Bases.Models;

namespace Gsafety.Ant.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.DevGpsManageViewVm)]
    public class DevGpsMangeViewModel : UploadAndExportViewModelBase<DevGps>
    {
        private DevGps currentDevGps;

        public DevGps CurrentDevGps
        {
            get { return currentDevGps; }
            set
            {
                currentDevGps = value;
                RaisePropertyChanged(() => this.CurrentDevGps);
            }
        }

        public List<EnumModel> InstallStatusTypes { get; set; }

        private string searchByName;
        /// <summary>
        /// 
        /// </summary>
        public string SearchByName
        {
            get
            {
                return searchByName;
            }
            set
            {
                this.searchByName = value;
                RaisePropertyChanged(() => this.SearchByName);
            }
        }

        private string _vehicleName = string.Empty;
        public string VehicleSn
        {
            get { return _vehicleName; }
            set
            {
                _vehicleName = value;
                RaisePropertyChanged(() => VehicleSn);
            }
        }
        private EnumModel _installState = null;
        public EnumModel SelectInstallState
        {
            get { return _installState; }
            set
            {
                _installState = value;
                RaisePropertyChanged(() => SelectInstallState);
            }
        }

         private string _mdvrSim = string.Empty;
        public string MdvrSim
        {
            get { return _mdvrSim; }
            set
            {
                _mdvrSim = value;
                RaisePropertyChanged(() => MdvrSim);
            }
        }
        /// <summary>
        /// 初始化内容
        /// </summary>
        public DevGpsMangeViewModel()
            : base()
        {
            Url = new Uri(this.GetTemplateUrl(DownLoadType.GPSDevice));
            this.UploadBtnStatus = true;
            this.ExportBtnStatus = true;
               var adapter = new EnumAdapter<Gsafety.PTMS.Bases.Enums.InstallStatusType>();
                var categorys = adapter.GetEnumInfos();
            InstallStatusTypes = new List<EnumModel>();
                InstallStatusTypes.Add(new EnumModel()
                {
                    EnumValue = -1,
                    EnumName = "",
                    ShowName = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_All"),
                });
                SelectInstallState = InstallStatusTypes[0];
                foreach (var item in categorys)
                {
                    InstallStatusTypes.Add(new EnumModel()
                    {
                        EnumValue = item.Value,
                        EnumName = item.Name,
                        ShowName = item.LocalizedString,
                    });
                }

            AddVisibility = (Visibility)converter.Convert("02-04-01-04-01", null, "02-04-01-04-01", null);
            RaisePropertyChanged(() => AddVisibility);

            ImportVisibility = (Visibility)converter.Convert("02-04-01-04-05", null, "02-04-01-04-05", null);
            RaisePropertyChanged(() => ImportVisibility);
            DownloadTemplateVisibility = (Visibility)converter.Convert("02-04-01-04-06", null, "02-04-01-04-06", null);
            RaisePropertyChanged(() => DownloadTemplateVisibility);
            ExportVisibility = (Visibility)converter.Convert("02-04-01-04-07", null, "02-04-01-04-06", null);
            RaisePropertyChanged(() => ExportVisibility);

            EditVisibility = (Visibility)converter.Convert("02-04-01-04-03", null, "02-04-01-04-03", null);
            RaisePropertyChanged(() => EditVisibility);

            ViewVisibility = (Visibility)converter.Convert("02-04-01-04-01", null, "02-04-01-04-01", null);
            RaisePropertyChanged(() => ViewVisibility);

            DeleteVisibility = (Visibility)converter.Convert("02-04-01-04-04", null, "02-04-01-04-04", null);
            RaisePropertyChanged(() => DeleteVisibility);

        }
        protected FunItemVisibilityConverter converter = new FunItemVisibilityConverter();
        Visibility _addvisiblity = Visibility.Collapsed;
        public Visibility AddVisibility
        {
            set
            {
                _addvisiblity = value;
            }
            get
            {
                return _addvisiblity;
            }
        }
        Visibility _eddvisiblity = Visibility.Collapsed;
        public Visibility EditVisibility
        {
            set
            {
                _eddvisiblity = value;
            }
            get
            {
                return _eddvisiblity;
            }
        }
        public Visibility DeleteVisibility { set; get; }
        public Visibility ViewVisibility { set; get; }
        public Visibility ExportVisibility { get; set; }
        public Visibility DownloadTemplateVisibility { get; set; }
        public Visibility ImportVisibility { get; set; }

        /// <summary>
        /// 初始化定位设备服务客户端
        /// </summary>
        /// <returns></returns>
        private DevGpsServiceClient InitialClient()
        {
            DevGpsServiceClient client = ServiceClientFactory.Create<DevGpsServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);

            client.GetByNameDevGpsListCompleted += client_GetByNameDevGpsListCompleted;
            client.DeleteDevGpsByIDCompleted += client_DeleteDevGpsByIDCompleted;
            client.BatchAddDevGpsCompleted += client_BatchAddDevGpsCompleted;
            client.CheckDevGpsExistCompleted += client_CheckDevGpsExistCompleted;

            return client;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        protected override void Update(string name)
        {
            DevGpsDetailWindow window = new DevGpsDetailWindow(string.Empty, new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentDevGps } });
            window.Closed += window_Closed;
            window.Show();

        }
        protected override void Add(string name)
        {
            DevGpsDetailWindow window = new DevGpsDetailWindow(string.Empty, new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentDevGps } });
            window.Closed += window_Closed;
            window.Show();
        }

        protected override void Delete()
        {
            var dialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
            dialogResult.Closed += dialogResult_Closed;
        }

        void window_Closed(object sender, EventArgs e)
        {
            Data.RefreshPage();
        }

        private void dialogResult_Closed(object sender, EventArgs e)
        {
            try
            {
                SelfMessageBox dialog = sender as SelfMessageBox;
                if (dialog != null)
                {
                    if (dialog.DialogResult == true)
                    {

                        if (CurrentDevGps != null)
                        {
                            DevGpsServiceClient client = InitialClient();
                            client.DeleteDevGpsByIDAsync(CurrentDevGps.ID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void ViewDetail(string name)
        {
            DevGpsDetailWindow window = new DevGpsDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentDevGps } });
            window.Show();
        }

        /// <summary>
        /// 查询page有点问题要解决
        /// </summary>
        protected override void Query()
        {
            currentIndex = 1;
            Data.MoveToFirstPage();
        }
        public Gsafety.PTMS.ServiceReference.DevGpsService.InstallStatusType? InstallStatus
        {
            get
            {
                if (SelectInstallState.EnumValue <= 0)
                {
                    return new Nullable<Gsafety.PTMS.ServiceReference.DevGpsService.InstallStatusType>();
                }
                else
                {
                    return (Gsafety.PTMS.ServiceReference.DevGpsService.InstallStatusType)SelectInstallState.EnumValue;
                }
            }
        }

        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                Data = new BaseLib.Model.PagedServerCollection<DevGps>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    PagingInfo page = new PagingInfo();
                    page.PageIndex = pageIndex;
                    page.PageSize = pageSize;

                    DevGpsServiceClient client = InitialClient();
                    client.GetByNameDevGpsListAsync(page, ApplicationContext.Instance.AuthenticationInfo.ClientID, SearchByName, this.VehicleSn,InstallStatus,MdvrSim);
                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }


        /// <summary>
        /// 删除定位设备完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void client_DeleteDevGpsByIDCompleted(object sender, DeleteDevGpsByIDCompletedEventArgs e)
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
                            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                                   e.Result.ExceptionMessage);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        }

                        return;
                    }
                    else
                    {
                        Data.RefreshPage();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_DeleteDevGpsByID", ex);
            }
            finally
            {
                DevGpsServiceClient client = sender as DevGpsServiceClient;
                CloseClient(client);
            }
        }

        private void CloseClient(DevGpsServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        private void client_GetByNameDevGpsListCompleted(object sender, GetByNameDevGpsListCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
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
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                               e.Result.ExceptionMessage);
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                    }
                }
                else
                {
                    foreach (var item in e.Result.Result)
                    {
                        item.CreateTime = item.CreateTime.ToLocalTime();
                        switch (item.InstallStatus)
                        {
                            case  Gsafety.PTMS.ServiceReference.DevGpsService.InstallStatusType.UnInstall:
                                item.DisplayInstallStatus = ApplicationContext.Instance.StringResourceReader.GetString("UnInstall");
                                break;
                            case Gsafety.PTMS.ServiceReference.DevGpsService.InstallStatusType.Installing:
                                item.DisplayInstallStatus = ApplicationContext.Instance.StringResourceReader.GetString("Installing");
                                break;
                            case Gsafety.PTMS.ServiceReference.DevGpsService.InstallStatusType.Installed:
                                item.DisplayInstallStatus = ApplicationContext.Instance.StringResourceReader.GetString("Installed");
                                break;
                            default:
                                break;
                        }
                    }
                    Data.loader_Finished(new BaseLib.Model.PagedResult<DevGps>()
                    {
                        Count = e.Result.TotalRecord,
                        Items = e.Result.Result,//数据列表
                        PageIndex = currentIndex
                    });
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_GetByNameDevGpsList", ex);
            }
            finally
            {
                DevGpsServiceClient client = sender as DevGpsServiceClient;
                CloseClient(client);
            }

        }

        private List<DevGps> uploadList = null;
        protected override void UploadAction()
        {
            try
            {
                uploadList = new List<DevGps>();
                batchIndex = 1;
                errorIndex = -1;
                errorCode = errorCode.Clear();
                System.IO.FileInfo fileInfo;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = " Files(*.xlsx)|*.xlsx|All files(*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {

                    fileInfo = openFileDialog.File;
                    if (!fileInfo.Name.EndsWith("xlsx"))
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), MessageDialogButton.Ok);
                        return;
                    }
                    FileStream fs = null;
                    try
                    {
                        fs = fileInfo.OpenRead();
                    }
                    catch
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_CloseFile"), MessageDialogButton.Ok);
                        return;
                    }

                    try
                    {
                        XLSXReader xlsxReader = new XLSXReader(fileInfo);

                        List<string> subItems = xlsxReader.GetListSubItems();

                        IEnumerable<IDictionary> datasource = xlsxReader.GetData(subItems[0]);

                        if (datasource == null)
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), MessageDialogButton.Ok);
                            return;
                        }

                        IDictionary firstRow = null;
                        firstRow = datasource.First();
                        List<object> lines = new List<object>();

                        foreach (IDictionary currentDict in datasource)
                        {
                            List<object> cells = new List<object>();
                            foreach (DictionaryEntry pair in firstRow)
                            {
                                if (currentDict.Contains(pair.Key))
                                {
                                    cells.Add(currentDict[pair.Key]);
                                }
                                else
                                {
                                    cells.Add(string.Empty);
                                }
                            }
                            lines.Add(cells);
                        }
                        if (lines.Count < 2)
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), MessageDialogButton.Ok);
                            return;
                        }

                        uploadContent = new String[lines.Count][];
                        for (int i = 0; i < uploadContent.Length; i++)
                        {
                            List<object> cells = (List<object>)lines[i];
                            uploadContent[i] = new String[cells.Count];
                            for (int j = 0; j < cells.Count; j++)
                            {
                                uploadContent[i][j] = cells[j].ToString();
                            }
                        }
                        int matchnum = 0;

                        uploadList = GetUploadList();
                        //全局变量matchnum为1时表示有重复
                        if (matchnum == 0)
                        {
                            if (errorIndex == -1)
                            {
                                setUploadBtnStatus(false);
                                DevGpsServiceClient client = InitialClient();
                                client.CheckDevGpsExistAsync(new ObservableCollection<DevGps>(uploadList));
                            }
                            else
                            {
                                MessageListBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), errorCode.ToString(), MessageDialogButton.Ok);
                                uploadContent = null;
                            }
                        }
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Close();
                        }
                    }

                }
            }
            catch
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("FAIL"), MessageDialogButton.Ok);
            }
        }

        protected override void ExportAction()
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
                    DevGpsServiceClient client = ServiceClientFactory.Create<DevGpsServiceClient>();
                    client.GetDevGpsListCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord >= 0)
                        {
                            foreach (var item in e.Result.Result)
                            {
                                item.CreateTime = item.CreateTime.ToLocalTime();
                                switch (item.InstallStatus)
                                {
                                    case Gsafety.PTMS.ServiceReference.DevGpsService.InstallStatusType.UnInstall:
                                        item.DisplayInstallStatus = ApplicationContext.Instance.StringResourceReader.GetString("UnInstall");
                                        break;
                                    case Gsafety.PTMS.ServiceReference.DevGpsService.InstallStatusType.Installing:
                                        item.DisplayInstallStatus = ApplicationContext.Instance.StringResourceReader.GetString("Installing");
                                        break;
                                    case Gsafety.PTMS.ServiceReference.DevGpsService.InstallStatusType.Installed:
                                        item.DisplayInstallStatus = ApplicationContext.Instance.StringResourceReader.GetString("Installed");
                                        break;
                                    default:
                                        break;
                                }
                            }

                            List<string> Codes = new List<string>();
                            Codes.Add("DisplayInstallStatus");
                            Codes.Add("VehicleID");
                            Codes.Add("GpsSn");
                            Codes.Add("GpsUid");
                            Codes.Add("GpsSim");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_InstallStatus"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_VehicleNumber"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("DeviceName"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("DevGPS_Uid"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DevGpsSIM"));

                            XLSXExporter xlsx = new XLSXExporter();
                            xlsx.Export(e.Result.Result.ToList(), dlg.OpenFile(), Codes, Names);
                            setExportBtnStatus(true);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("BaseInfo_ExportSucceed"), MessageDialogButton.Ok);
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("BaseInfo_ExportFaild"), MessageDialogButton.Ok);
                        }

                        CloseClient(client);
                    };
                    ServiceClientFactory.CreateMessageHeader(client.InnerChannel);

                    if (Data.TotalItemCount > 10000)
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = 1, PageSize = 10000 };
                        client.GetDevGpsListAsync(pagingInfo, ApplicationContext.Instance.AuthenticationInfo.ClientID);
                    }
                    else
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = 1, PageSize = Data.TotalItemCount };
                        client.GetDevGpsListAsync(pagingInfo, ApplicationContext.Instance.AuthenticationInfo.ClientID);
                    }
                }
            }
            catch (Exception ex)
            {
                setExportBtnStatus(true);
            }
        }

        private List<DevGps> GetUploadList()
        {
            List<DevGps> devGpsList = new List<DevGps>();
            int startIndex = batchIndex;
            int endIndex = uploadContent.Length;

            for (int i = startIndex; i < endIndex; i++)
            {
                DevGps devGps = new DevGps();
                devGps.ClientId = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                devGps.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;
                devGps.InstallStatus = Gsafety.PTMS.ServiceReference.DevGpsService.InstallStatusType.UnInstall;
                devGps.GpsSn = uploadContent[i][0].ToString().Trim();
                if (string.IsNullOrEmpty(devGps.GpsSn))
                {
                    errorIndex = i;
                    errorCode.Append("A" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty")); ;
                    errorCode.Append(";");
                }
                if (devGpsList.Any(t => t.GpsSn == devGps.GpsSn))
                {
                    errorIndex = i;
                    errorCode.Append("A" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame"));
                    errorCode.Append(";");
                }
                if (devGps.GpsSn.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode.Append("A" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                devGps.GpsUid = uploadContent[i][1].ToString().Trim();
                if (string.IsNullOrEmpty(devGps.GpsUid))
                {
                    errorIndex = i;
                    errorCode.Append("B" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty")); ;
                    errorCode.Append(";");
                }
                if (devGpsList.Any(t => t.GpsUid == devGps.GpsUid))
                {
                    errorIndex = i;
                    errorCode.Append("B" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame"));
                    errorCode.Append(";");
                }
                if (devGps.GpsUid.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode.Append("B" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                devGps.GpsSim = uploadContent[i][2].ToString().Trim();
                if (devGps.GpsSim.Trim().Length > 50)
                {
                    errorIndex = i;
                    errorCode.Append("C" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                devGpsList.Add(devGps);
            }

            return devGpsList;
        }

        void client_CheckDevGpsExistCompleted(object sender, CheckDevGpsExistCompletedEventArgs e)
        {
            try
            {
                if (e.Result.TotalRecord == 0)
                {
                    DevGpsServiceClient client = InitialClient();
                    client.BatchAddDevGpsAsync(new ObservableCollection<DevGps>(uploadList));
                }
                else
                {
                    var list = e.Result.Result.ToList();
                    foreach (var item in list)
                    {
                        errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("LocateDevice_Sn"));
                        errorCode.Append(item.GpsSn);
                        errorCode.Append(" ");
                        errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("Tip_Or"));
                        errorCode.Append(" ");
                        errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("LocateDevice_Core_Sn"));
                        errorCode.Append(item.GpsUid);
                        errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DataExisted"));
                        errorCode.Append(";");
                    }
                    MessageListBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), errorCode.ToString(), MessageDialogButton.Ok);
                    setUploadBtnStatus(true);
                }
            }
            catch (System.Exception ex)
            {
                setUploadBtnStatus(true);
            }
        }

        void client_BatchAddDevGpsCompleted(object sender, BatchAddDevGpsCompletedEventArgs e)
        {
            try
            {
                if (e.Result.Result)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"),
                        ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_LoadSucess"), MessageDialogButton.Ok);
                    setUploadBtnStatus(true);
                }
                else
                {
                    MessageBoxHelper.ShowDialog(e.Result.ExceptionMessage.ToString());
                    setUploadBtnStatus(true);
                }
            }
            catch
            {
                setUploadBtnStatus(true);
            }
            finally
            {
                DevGpsServiceClient client = sender as DevGpsServiceClient;
                CloseClient(client);
            }
        }
    }
}

