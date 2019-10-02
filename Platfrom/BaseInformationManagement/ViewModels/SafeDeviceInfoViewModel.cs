using Gsafety.Ant.BaseInformation.Views;
using Gsafety.Common.Controls;
using Gsafety.Common.Converts;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.BscDevSuiteService;
using Gsafety.PTMS.ServiceReference.DeviceAlertService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Spreadsheet;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Gsafety.Ant.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.SafeDeviceInfoVm)]
    public class SafeDeviceInfoViewModel : UploadAndExportViewModelBase<DevSuite>
    {
        public List<EnumModel> InstallStatusTypes { get; set; }
        public IActionCommand AddPartCommand { get; protected set; }
        public IActionCommand UpdatePartCommand { get; protected set; }
        public IActionCommand ViewPartCommand { get; protected set; }
        public IActionCommand DeletePartCommand { get; protected set; }

        public SafeDeviceInfoViewModel()
        {
            try
            {
                InitInstallStatusTypes();

                AddPartCommand = new ActionCommand<object>(obj => AddPart());
                UpdatePartCommand = new ActionCommand<object>(obj => UpdatePart());
                ViewPartCommand = new ActionCommand<object>(obj => ViewPart());
                DeletePartCommand = new ActionCommand<object>(obj => DeletePart());

                this.UploadBtnStatus = true;
                this.ExportBtnStatus = true;

                AddVisibility = (Visibility)converter.Convert("02-04-01-03-02", null, "02-04-01-03-02", null);
                RaisePropertyChanged(() => AddVisibility);

                ImportVisibility = (Visibility)converter.Convert("02-04-01-03-05", null, "02-04-01-03-05", null);
                RaisePropertyChanged(() => ImportVisibility);
                DownloadTemplateVisibility = (Visibility)converter.Convert("02-04-01-03-06", null, "02-04-01-03-06", null);
                RaisePropertyChanged(() => DownloadTemplateVisibility);
                ExportVisibility = (Visibility)converter.Convert("02-04-01-03-07", null, "02-04-01-03-07", null);
                RaisePropertyChanged(() => ExportVisibility);

                EditVisibility = (Visibility)converter.Convert("02-04-01-03-03", null, "02-04-01-03-03", null);
                RaisePropertyChanged(() => EditVisibility);

                ViewVisibility = (Visibility)converter.Convert("02-04-01-03-01", null, "02-04-01-03-01", null);
                RaisePropertyChanged(() => ViewVisibility);

                DeleteVisibility = (Visibility)converter.Convert("02-04-01-03-04", null, "02-04-01-03-04", null);
                RaisePropertyChanged(() => DeleteVisibility);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
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

        Visibility _addpartvisiblity = Visibility.Collapsed;
        public Visibility AddPartVisibility
        {
            set
            {
                _addpartvisiblity = value;
            }
            get
            {
                return _addpartvisiblity;
            }
        }
        Visibility _editpartvisiblity = Visibility.Collapsed;
        public Visibility EditPartVisibility
        {
            set
            {
                _editpartvisiblity = value;
            }
            get
            {
                return _editpartvisiblity;
            }
        }
        public Visibility DeletePartVisibility { set; get; }


        /// <summary>
        /// 初始化安全套件服务客户端
        /// </summary>
        /// <returns></returns>
        private BscDevSuiteServiceClient InitializeBscDevSuiteServiceClient()
        {
            BscDevSuiteServiceClient client = null;
            client = ServiceClientFactory.Create<BscDevSuiteServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);

            client.GetBscDevSuiteListCompleted += client_GetBscDevSuiteListCompleted;
            client.DeleteBscDevSuiteByIDCompleted += client_DeleteBscDevSuiteByIDCompleted;
            client.GetBscDevSuiteCompleted += client_GetBscDevSuiteCompleted;

            client.BatchAddCompleted += client_BatchAddCompleted;
            client.CheckSecuritySuiteExistCompleted += client_CheckSecuritySuiteExistCompleted;
            return client;
        }


        /// <summary>
        /// 初始化配件服务客户端
        /// </summary>
        /// <returns></returns>
        private BscDevSuitePartServiceClient InitializeBscDevSuitePartServiceClient()
        {
            BscDevSuitePartServiceClient partclient = ServiceClientFactory.Create<BscDevSuitePartServiceClient>();
            partclient.DeleteBscDevSuitePartByIDCompleted += partclient_DeleteBscDevSuitePartByIDCompleted;
            return partclient;
        }


        /// <summary>
        /// 删除配件服务完成方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void partclient_DeleteBscDevSuitePartByIDCompleted(object sender, DeleteBscDevSuitePartByIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        GetSuitInfo();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
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
                ApplicationContext.Instance.Logger.LogException("partclient_DeleteBscDevSuitePartByIDCompleted", ex);
            }
            finally
            {
                BscDevSuitePartServiceClient client = sender as BscDevSuitePartServiceClient;
                this.CloseBscDevSuitePartServiceClient(client);
            }
        }

        /// <summary>
        /// 关闭配件服务方法
        /// </summary>
        /// <param name="client"></param>
        private void CloseBscDevSuitePartServiceClient(BscDevSuitePartServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }


        private DevSuitePart _part;
        public DevSuitePart CurrentPart
        {
            get { return _part; }
            set
            {
                _part = value;
                RaisePropertyChanged(() => CurrentPart);
            }
        }

        private void DeletePart()
        {
            if (CurrentPart != null)
            {
                var dialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                dialogResult.Closed += dialogResult_Closed;
            }
        }

        void dialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    BscDevSuitePartServiceClient partclient = InitializeBscDevSuitePartServiceClient();
                    if (partclient != null && CurrentPart != null)
                        partclient.DeleteBscDevSuitePartByIDAsync(CurrentPart.ID);
                }
            }
        }

        private void ViewPart()
        {
            SafeDevicePartDetailWindow window = new SafeDevicePartDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", "view" }, { "model", CurrentPart } });
            window.Show();
        }

        private void UpdatePart()
        {
            SafeDevicePartDetailWindow window = new SafeDevicePartDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", "update" }, { "model", CurrentPart } });
            window.Closed += partwindow_Closed;
            window.Show();
        }

        private void partwindow_Closed(object sender, EventArgs e)
        {
            GetSuitInfo();
        }

        private void AddPart()
        {
            try
            {
                if (CurrentModel == null)
                {
                    MessageBoxHelper.ShowDialog(LProxy.Caption, ApplicationContext.Instance.StringResourceReader.GetString("Tip_SelectSafeDeviceElement"), MessageDialogButton.Ok);
                }
                else
                {
                    SafeDevicePartDetailWindow window = new SafeDevicePartDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", "add" }, { "model", CurrentModel } });
                    window.Closed += partwindow_Closed;
                    window.Show();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void GetSuitInfo()
        {
            BscDevSuiteServiceClient client = this.InitializeBscDevSuiteServiceClient();
            client.GetBscDevSuiteAsync(CurrentModel.SuiteInfoID);
        }

        #region Delete
        protected override void Delete()
        {
            if (CurrentModel != null)
            {
                var deletesuitedialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                deletesuitedialogResult.Closed += deletesuitedialogResult_Closed;
            }
        }

        void deletesuitedialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    BscDevSuiteServiceClient client = this.InitializeBscDevSuiteServiceClient();
                    client.DeleteBscDevSuiteByIDAsync(CurrentModel.SuiteInfoID);
                }
            }
        }

        void client_DeleteBscDevSuiteByIDCompleted(object sender, DeleteBscDevSuiteByIDCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

            finally
            {
                BscDevSuiteServiceClient client = sender as BscDevSuiteServiceClient;
                this.CloseBscDevSuiteServiceClient(client);
            }

        }

        /// <summary>
        /// 关闭安全套件服务端
        /// </summary>
        /// <param name="client"></param>
        private void CloseBscDevSuiteServiceClient(BscDevSuiteServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }


        void client_GetBscDevSuiteCompleted(object sender, GetBscDevSuiteCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == true)
                {
                    return;
                }

                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError), MessageDialogButton.Ok);

                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    return;
                }

                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(result.ErrorMsg), MessageDialogButton.Ok);
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }
                else
                {
                    if (CurrentModel.SuiteInfoID == result.Result.SuiteInfoID)
                    {
                        CurrentModel.BscDevSuiteParts.Clear();

                        foreach (var item in result.Result.BscDevSuiteParts)
                        {
                            if (item.PartType.ToString() == "AlarmButton")
                            {
                                item.ShowParttype = ApplicationContext.Instance.StringResourceReader.GetString("AlarmButton");
                            }
                            else if (item.PartType.ToString() == "Camera")
                            {
                                item.ShowParttype = ApplicationContext.Instance.StringResourceReader.GetString("Camera");
                            }
                            else if (item.PartType.ToString() == "Screen")
                            {
                                item.ShowParttype = ApplicationContext.Instance.StringResourceReader.GetString("Screen");
                            }

                            if (item.ProduceTime.HasValue)
                            {
                                item.ProduceTime = item.ProduceTime.Value.ToLocalTime();
                            }
                            CurrentModel.BscDevSuiteParts.Add(item);
                        }
                        RaisePropertyChanged(() => CurrentModel.BscDevSuiteParts);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                BscDevSuiteServiceClient client = sender as BscDevSuiteServiceClient;
                this.CloseBscDevSuiteServiceClient(client);
            }
        }
        #endregion

        private void InitInstallStatusTypes()
        {
            try
            {
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

                Url = new Uri(this.GetTemplateUrl(DownLoadType.Suite));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void ViewDetail(string actionName)
        {
            SafeDeviceInfoDetailWindow window = new SafeDeviceInfoDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", actionName }, { "model", CurrentModel } });
            window.Show();
        }

        protected override void Update(string name)
        {
            SafeDeviceInfoDetailWindow window = new SafeDeviceInfoDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentModel } });
            window.Closed += window_Closed;
            window.Show();
        }

        protected override void Add(string name)
        {
            SafeDeviceInfoDetailWindow window = new SafeDeviceInfoDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", name } });
            window.Closed += window_Closed;
            window.Show();
        }

        void window_Closed(object sender, EventArgs e)
        {
            Data.RefreshPage();
        }

        #region Query
        /// <summary>
        /// 查询
        /// </summary>
        protected override void Query()
        {
            currentIndex = 1;
            Data.MoveToFirstPage();
        }

        void client_GetBscDevSuiteListCompleted(object sender, GetBscDevSuiteListCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == true)
                {
                    return;
                }

                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError), MessageDialogButton.Ok);

                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    return;
                }

                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (!string.IsNullOrWhiteSpace(result.ErrorMsg))
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(result.ErrorMsg), MessageDialogButton.Ok);

                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError), MessageDialogButton.Ok);
                    }
                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }
                else
                {
                    foreach (var item in result.Result)
                    {
                        item.CreateTime = item.CreateTime.ToLocalTime();
                        switch (item.InstallStatus)
                        {
                            case Gsafety.PTMS.ServiceReference.BscDevSuiteService.InstallStatusType.UnInstall:
                                item.DisplayInstallStatus = ApplicationContext.Instance.StringResourceReader.GetString("UnInstall");
                                break;
                            case Gsafety.PTMS.ServiceReference.BscDevSuiteService.InstallStatusType.Installing:
                                item.DisplayInstallStatus = ApplicationContext.Instance.StringResourceReader.GetString("Installing");
                                break;
                            case Gsafety.PTMS.ServiceReference.BscDevSuiteService.InstallStatusType.Installed:
                                item.DisplayInstallStatus = ApplicationContext.Instance.StringResourceReader.GetString("Installed");
                                break;
                            default:
                                break;
                        }
                    }

                    Data.loader_Finished(new BaseLib.Model.PagedResult<DevSuite>
                    {
                        Count = result.TotalRecord,
                        Items = result.Result,
                        PageIndex = currentIndex
                    });
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                BscDevSuiteServiceClient client = sender as BscDevSuiteServiceClient;
                this.CloseBscDevSuiteServiceClient(client);
            }
        }
        #endregion

        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                Data = new BaseLib.Model.PagedServerCollection<DevSuite>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    BscDevSuiteServiceClient client = this.InitializeBscDevSuiteServiceClient();
                    client.GetBscDevSuiteListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, InstallStatus, VehicleSn, SuitID, MdvrCoreSn, MdvrSn, MdvrSim, pageIndex, pageSize);
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        #region Property


        private DevSuite _currentModel;
        public DevSuite CurrentModel
        {
            get { return _currentModel; }
            set
            {
                if (_currentModel != value)
                {
                    _currentModel = value;
                    if (CurrentModel != null)
                    {
                        GetSuitInfo();
                    }
                    RaisePropertyChanged(() => CurrentModel);
                }
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

        private string _suiteID = string.Empty;
        public string SuitID
        {
            get { return _suiteID; }
            set
            {
                _suiteID = value;
                RaisePropertyChanged(() => SuitID);
            }
        }

        private string _mdvrCoreSn = string.Empty;
        public string MdvrCoreSn
        {
            get { return _mdvrCoreSn; }
            set
            {
                _mdvrCoreSn = value;
                RaisePropertyChanged(() => MdvrCoreSn);
            }
        }

        private string _mdvrSn = string.Empty;
        public string MdvrSn
        {
            get { return _mdvrSn; }
            set
            {
                _mdvrSn = value;
                RaisePropertyChanged(() => MdvrSn);
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

        public Gsafety.PTMS.ServiceReference.BscDevSuiteService.InstallStatusType? InstallStatus
        {
            get
            {
                if (SelectInstallState.EnumValue <= 0)
                {
                    return new Nullable<Gsafety.PTMS.ServiceReference.BscDevSuiteService.InstallStatusType>();
                }
                else
                {
                    return (Gsafety.PTMS.ServiceReference.BscDevSuiteService.InstallStatusType)SelectInstallState.EnumValue;
                }
            }
        }

        #endregion

        #region Upload Download Export

        private List<DevSuite> uploadList;

        protected override void UploadAction()
        {
            try
            {
                uploadList = new List<DevSuite>();
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
                        List<IDictionary> datasource = xlsxReader.GetData(subItems[0]).ToList();
                        if (datasource.Count == 0)
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
                        uploadList = GetUploadList();
                        if (errorIndex == -1)
                        {
                            setUploadBtnStatus(false);

                            BscDevSuiteServiceClient client = this.InitializeBscDevSuiteServiceClient();
                            client.CheckSecuritySuiteExistAsync(new ObservableCollection<DevSuite>(uploadList));
                        }
                        else
                        {
                            MessageListBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), errorCode.ToString(), MessageDialogButton.Ok);
                            uploadContent = null;
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

                    BscDevSuiteServiceClient client = this.InitializeBscDevSuiteServiceClient();
                    client.GetBscDevSuiteExportListCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord > 0)
                        {
                            foreach (var item in e.Result.Result)
                            {
                                item.CreateTime = item.CreateTime.ToLocalTime();
                                foreach (var part in item.BscDevSuiteParts)
                                {
                                    part.CreateTime = part.CreateTime.ToLocalTime();
                                    if (part.ProduceTime.HasValue)
                                        part.ProduceTime = part.ProduceTime.Value.ToLocalTime();
                                }
                            }
                            DevSuite d = new DevSuite();
                            List<string> Codes = new List<string>();
                            Codes.Add("InstallStatus");
                            Codes.Add("VehicleID");
                            Codes.Add("SuiteID");
                            Codes.Add("MdvrSn");
                            Codes.Add("MdvrCoreSn");
                            Codes.Add("Model");
                            Codes.Add("MdvrSim");
                            Codes.Add("MdvrSimMobile");
                            Codes.Add("UpsSn");
                            Codes.Add("SdSn");
                            Codes.Add("SoftwareVersion");
                            Codes.Add("Protocol"); 
                            Codes.Add("Note");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_InstallStatus"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_VehicleNumber"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_SuiteID"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("barcode"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("DeviceNUm"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("SheBeiXingHao"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MdvrSimId"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MDVR_PhoneNum"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_UPS"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_SdCardId"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_SoftwareVersion"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("XieYiLEIXING"));                           
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Note"));

                            int maxPartCount = e.Result.Result.Max(t => t.BscDevSuiteParts.Count);
                            for (int i = 1; i <= maxPartCount; i++)
                            {
                                Codes.Add(string.Format("PartSn{0}", i.ToString()));
                                Codes.Add(string.Format("Name{0}", i.ToString()));
                                Codes.Add(string.Format("Model{0}", i.ToString()));
                                Codes.Add(string.Format("ProduceTime{0}", i.ToString()));
                                Codes.Add(string.Format("PartType{0}", i.ToString()));

                                Names.Add(string.Format(ApplicationContext.Instance.StringResourceReader.GetString("PartSn"), i.ToString()));
                                Names.Add(string.Format(ApplicationContext.Instance.StringResourceReader.GetString("PartName"), i.ToString()));
                                Names.Add(string.Format(ApplicationContext.Instance.StringResourceReader.GetString("PartModel"), i.ToString()));
                                Names.Add(string.Format(ApplicationContext.Instance.StringResourceReader.GetString("PartProduceTime"), i.ToString()));
                                Names.Add(string.Format(ApplicationContext.Instance.StringResourceReader.GetString("TargetProperty"), i.ToString()));
                            }

                            List<EnumsEx> eList = new List<EnumsEx>();

                            List<FieldEx> FieldEx1 = new List<FieldEx>();
                            Enum.GetNames(typeof(Gsafety.PTMS.ServiceReference.BscDevSuiteService.ProtocolTypeEnum)).ToList().ForEach(x =>
                            {
                                FieldEx item = new FieldEx { Key = x, Value = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                                FieldEx1.Add(item);
                            });
                            eList.Add(new EnumsEx { Code = "Protocol", Content = FieldEx1 });

                            List<FieldEx> FieldEx2 = new List<FieldEx>();
                            Enum.GetNames(typeof(Gsafety.PTMS.ServiceReference.BscDevSuiteService.InstallStatusType)).ToList().ForEach(x =>
                            {
                                FieldEx item = new FieldEx { Key = x, Value = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                                FieldEx2.Add(item);
                            });
                            eList.Add(new EnumsEx { Code = "InstallStatus", Content = FieldEx2 });

                      

                            List<Dictionary<string, string>> exportList = new List<Dictionary<string, string>>();

                            var adapter = new EnumAdapter<Gsafety.PTMS.Bases.Enums.ProtocolTypeEnum>();
                            var categorys = adapter.GetEnumInfos();
                            var Devadapter = new EnumAdapter<Gsafety.PTMS.Enums.BscDevSuitePartTypeEnum>();
                            var Devcategorys = Devadapter.GetEnumInfos();
                            foreach (var item in e.Result.Result)
                            {
                                Dictionary<string, string> dic = new Dictionary<string, string>();
                                dic.Add("InstallStatus", eList.Where(y => y.Code == "InstallStatus").FirstOrDefault().Content.Where(x => x.Key == item.InstallStatus.ToString()).FirstOrDefault().Value);
                                dic.Add("VehicleID", item.VehicleID);
                                dic.Add("SuiteID", item.SuiteID);
                                dic.Add("MdvrCoreSn", item.MdvrCoreSn);
                                dic.Add("MdvrSn", item.MdvrSn);
                                dic.Add("MdvrSim", item.MdvrSim);
                                dic.Add("MdvrSimMobile", item.MdvrSimMobile);
                                dic.Add("SdSn", item.SdSn);
                                dic.Add("SoftwareVersion", item.SoftwareVersion);
                                dic.Add("Model", item.Model);
                                dic.Add("Protocol", categorys.Where(t => t.Value == (short)item.Protocol).FirstOrDefault().Name);
                                dic.Add("UpsSn", item.UpsSn);
                                dic.Add("Note", item.Note);
                                for (int i = 0; i < item.BscDevSuiteParts.Count; i++)
                                {
                                    dic.Add(string.Format("PartSn{0}", (i + 1).ToString()), item.BscDevSuiteParts[i].PartSn);
                                    dic.Add(string.Format("Name{0}", (i + 1).ToString()), item.BscDevSuiteParts[i].Name);
                                    dic.Add(string.Format("Model{0}", (i + 1).ToString()), item.BscDevSuiteParts[i].Model);
                                    dic.Add(string.Format("ProduceTime{0}", (i + 1).ToString()), item.BscDevSuiteParts[i].ProduceTime.HasValue ? item.BscDevSuiteParts[i].ProduceTime.Value.ToShortDateString() : null);
                                    dic.Add(string.Format("PartType{0}", (i + 1).ToString()), Devcategorys.Where(t => t.Value == (short)item.BscDevSuiteParts[i].PartType).FirstOrDefault().Name);
                                }

                                exportList.Add(dic);
                            }

                            XLSXExporter xlsx = new XLSXExporter();
                            xlsx.Export(exportList, dlg.OpenFile(), Codes, Names);
                            setExportBtnStatus(true);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("BaseInfo_ExportSucceed"), MessageDialogButton.Ok);
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("BaseInfo_ExportFaild"), MessageDialogButton.Ok);
                        }

                        if (client != null)
                        {
                            client.CloseAsync();
                        }
                        client = null;
                    };

                    if (Data.TotalItemCount > 10000)
                    {
                        client.GetBscDevSuiteExportListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, InstallStatus, VehicleSn, SuitID, MdvrCoreSn, MdvrSn, MdvrSim, 1, 10000);
                    }
                    else
                    {
                        client.GetBscDevSuiteExportListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, InstallStatus, VehicleSn, SuitID, MdvrCoreSn, MdvrSn, MdvrSim, 1, Data.TotalItemCount);
                    }
                }
            }
            catch (Exception ex)
            {
                setExportBtnStatus(true);
            }
        }

        private List<DevSuite> GetUploadList()
        {
            List<DevSuite> tempSuiteList = new List<DevSuite>();
            int startIndex = batchIndex;
            int endIndex = uploadContent.Length;

            for (int i = startIndex; i < endIndex; i++)
            {
                DevSuite tempSuite = new DevSuite();
                tempSuite.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;

                #region check data
                tempSuite.SuiteID = uploadContent[i][0].ToString().Trim();
                if (tempSuite.SuiteID == string.Empty)
                {
                    errorIndex = i;
                    errorCode.Append("A" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty"));
                    errorCode.Append(";");
                }
                if (tempSuite.SuiteID.Trim().Length > 25)
                {
                    errorIndex = i;
                    errorCode.Append("A" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }
                if (tempSuiteList.Any(t => t.SuiteID == tempSuite.SuiteID.Trim()))
                {
                    errorIndex = i;
                    errorCode.Append("A" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame"));
                    errorCode.Append(";");
                }

                tempSuite.MdvrCoreSn = uploadContent[i][1].ToString().Trim();
                if (tempSuite.MdvrCoreSn == string.Empty)
                {
                    errorIndex = i;
                    errorCode.Append("B" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty"));
                    errorCode.Append(";");
                }
                if (tempSuite.MdvrCoreSn.Trim().Length > 20)
                {
                    //Length_Short_Warning
                    errorIndex = i;
                    errorCode.Append("B" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }
                if (tempSuiteList.Any(t => t.MdvrCoreSn == tempSuite.MdvrCoreSn.Trim()))
                {
                    errorIndex = i;
                    errorCode.Append("B" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame"));
                    errorCode.Append(";");
                }

                tempSuite.MdvrSn = uploadContent[i][2].ToString().Trim();
                if (tempSuite.MdvrSn == string.Empty)
                {
                    errorIndex = i;
                    errorCode.Append("C" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty"));
                    errorCode.Append(";");
                }
                if (tempSuite.MdvrSn.Trim().Length > 20)
                {
                    errorIndex = i;
                    errorCode.Append("C" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }
                if (tempSuiteList.Any(t => t.MdvrSn == tempSuite.MdvrSn.Trim()))
                {
                    errorIndex = i;
                    errorCode.Append("C" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame"));
                    errorCode.Append(";");
                }

                tempSuite.MdvrSim = uploadContent[i][3].ToString().Trim();
                if (tempSuite.MdvrSim.Trim().Length > 100)
                {
                    errorIndex = i;
                    errorCode.Append("D" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                tempSuite.MdvrSimMobile = uploadContent[i][4].ToString().Trim();
                if (tempSuite.MdvrSimMobile.Trim().Length > 20)
                {
                    errorIndex = i;
                    errorCode.Append("E" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                tempSuite.SdSn = uploadContent[i][5].ToString().Trim();
                if (tempSuite.SdSn.Trim().Length > 50)
                {
                    errorIndex = i;
                    errorCode.Append("F" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                tempSuite.SoftwareVersion = uploadContent[i][6].ToString().Trim();
                if (tempSuite.SoftwareVersion.Trim().Length > 100)
                {
                    errorIndex = i;
                    errorCode.Append("G" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                tempSuite.Model = uploadContent[i][7].ToString().Trim();
                if (tempSuite.Model.Length > 512)
                {
                    errorIndex = i;
                    errorCode.Append("H" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                if (uploadContent[i][8].ToString().Trim() == string.Empty)
                {
                    errorIndex = i;
                    errorCode.Append("I" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty"));
                    errorCode.Append(";");
                }
                else
                {
                    switch (uploadContent[i][8].ToString().Trim())
                    {
                        case "Stander808":
                            tempSuite.Protocol = Gsafety.PTMS.ServiceReference.BscDevSuiteService.ProtocolTypeEnum.Stander808;
                            break;
                        case "N9m":
                            tempSuite.Protocol = Gsafety.PTMS.ServiceReference.BscDevSuiteService.ProtocolTypeEnum.N9m;
                            break;
                        case "other":
                            tempSuite.Protocol = Gsafety.PTMS.ServiceReference.BscDevSuiteService.ProtocolTypeEnum.Other;
                            break;
                    }
                }

                tempSuite.UpsSn = uploadContent[i][9].ToString().Trim();
                if (tempSuite.UpsSn.Trim().Length > 50)
                {
                    errorIndex = i;
                    errorCode.Append("J" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                tempSuite.Note = uploadContent[i][10].ToString().Trim();
                if (tempSuite.Note.Trim().Length > 2000)
                {
                    errorIndex = i;
                    errorCode.Append("K" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                var camera1 = ParseDevSuitePart(tempSuiteList, i, 11);
                var camera2 = ParseDevSuitePart(tempSuiteList, i, 11 + 4 * 1);
                var camera3 = ParseDevSuitePart(tempSuiteList, i, 11 + 4 * 2);
                var camera4 = ParseDevSuitePart(tempSuiteList, i, 11 + 4 * 3);
                var alertBtn1 = ParseDevSuitePart(tempSuiteList, i, 11 + 4 * 4);
                var alertBtn2 = ParseDevSuitePart(tempSuiteList, i, 11 + 4 * 5);
                var alertBtn3 = ParseDevSuitePart(tempSuiteList, i, 11 + 4 * 6);
                var led = ParseDevSuitePart(tempSuiteList, i, 11 + 4 * 7);

                #endregion

                tempSuite.Status = (short)DeviceSuiteStatus.Initial;
                tempSuite.InstallStatus = Gsafety.PTMS.ServiceReference.BscDevSuiteService.InstallStatusType.UnInstall;
                ObservableCollection<DevSuitePart> parts = new ObservableCollection<DevSuitePart>();
                if (camera1 != null)
                {
                    parts.Add(camera1);
                }
                if (camera2 != null)
                {
                    parts.Add(camera2);
                    CheckSuitePartRule(parts, camera2, i, 11 + 4 * 1);
                }
                if (camera3 != null)
                {
                    parts.Add(camera3);
                    CheckSuitePartRule(parts, camera3, i, 11 + 4 * 2);
                }
                if (camera4 != null)
                {
                    parts.Add(camera4);
                    CheckSuitePartRule(parts, camera4, i, 11 + 4 * 3);
                }

                if (alertBtn1 != null)
                {
                    parts.Add(alertBtn1);
                    CheckSuitePartRule(parts, alertBtn1, i, 11 + 4 * 4);
                }

                if (alertBtn2 != null)
                {
                    parts.Add(alertBtn2);
                    CheckSuitePartRule(parts, alertBtn1, i, 11 + 4 * 5);
                }
                if (alertBtn3 != null)
                {
                    parts.Add(alertBtn3);
                    CheckSuitePartRule(parts, alertBtn1, i, 11 + 4 * 6);
                }
                if (led != null)
                {
                    parts.Add(led);
                    CheckSuitePartRule(parts, alertBtn1, i, 11 + 4 * 7);
                }

                tempSuite.BscDevSuiteParts = parts;
                tempSuiteList.Add(tempSuite);
            }

            return tempSuiteList;
        }

        private void CheckSuitePartRule(IEnumerable<DevSuitePart> partList, DevSuitePart target, int i, int columIndex)
        {
            foreach (var item in partList)
            {
                if (item == target)
                {
                    continue;
                }

                if (item.PartSn == target.PartSn)
                {
                    errorIndex = i;
                    errorCode.Append(ConvertColumnIndexToStr(columIndex) + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame"));
                    errorCode.Append(";");
                }
                if (item.Name == target.Name)
                {
                    errorIndex = i;
                    errorCode.Append(ConvertColumnIndexToStr(columIndex + 1) + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("SameNameExist"));
                    errorCode.Append(";");
                }
            }
        }

        private DevSuitePart ParseDevSuitePart(IEnumerable<DevSuite> tempSuiteList, int i, int columnIndex)
        {
            var suitePart = new DevSuitePart();
            suitePart.PartSn = uploadContent[i][columnIndex].ToString().Trim();
            if (suitePart.PartSn.Trim().Length > 50)
            {
                errorIndex = i;
                errorCode.Append(ConvertColumnIndexToStr(columnIndex) + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                errorCode.Append(";");
            }
            foreach (var item in tempSuiteList)
            {
                if (item.BscDevSuiteParts.Any(t => t.PartSn == suitePart.PartSn.Trim()))
                {
                    errorIndex = i;
                    errorCode.Append(ConvertColumnIndexToStr(columnIndex) + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame"));
                    errorCode.Append(";");
                }
            }

            suitePart.Name = uploadContent[i][columnIndex + 1].ToString().Trim();
            if (suitePart.Name.Trim().Length > 512)
            {
                errorIndex = i;
                errorCode.Append(ConvertColumnIndexToStr(columnIndex + 1) + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                errorCode.Append(";");
            }

            suitePart.Model = uploadContent[i][columnIndex + 2].ToString().Trim();
            if (suitePart.Model.Trim().Length > 512)
            {
                errorIndex = i;
                errorCode.Append(ConvertColumnIndexToStr(columnIndex + 2) + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                errorCode.Append(";");
            }

            var days = 0;
            if (uploadContent[i][columnIndex + 3].ToString().Trim() != string.Empty)
            {
                if (!int.TryParse(uploadContent[i][columnIndex + 3].ToString().Trim(), out days))
                {
                    errorIndex = i;
                    errorCode.Append(ConvertColumnIndexToStr(columnIndex + 3) + (i + 1).ToString() +
                                     ApplicationContext.Instance.StringResourceReader.GetString(
                                         "BASEINFO_FormatIllegal"));
                    errorCode.Append(";");
                }

                var dt = Convert.ToDateTime("1900/01/01");
                dt = dt.AddDays(days - 2);
                suitePart.ProduceTime = dt.ToUniversalTime();
            }
            else
            {
                suitePart.ProduceTime = null;
            }

            suitePart.PartType = BscDevSuitePartTypeEnum.AlarmButton;
            if (suitePart.Name != string.Empty && suitePart.PartSn != string.Empty
               && suitePart.Model != string.Empty && suitePart.ProduceTime != null)
            {
            }
            else if (suitePart.Name == string.Empty && suitePart.PartSn == string.Empty
               && suitePart.Model == string.Empty && suitePart.ProduceTime == null)
            {
                suitePart = null;
            }
            else
            {
                if (suitePart.PartSn == string.Empty)
                {
                    errorIndex = i;
                    errorCode.Append(ConvertColumnIndexToStr(columnIndex) + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty"));
                    errorCode.Append(";");
                }
                else if (suitePart.Name == string.Empty)
                {
                    errorIndex = i;
                    errorCode.Append(ConvertColumnIndexToStr(columnIndex + 1) + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty"));
                    errorCode.Append(";");
                }
                else if (suitePart.Model == string.Empty)
                {
                    errorIndex = i;
                    errorCode.Append(ConvertColumnIndexToStr(columnIndex + 2) + (i + 1).ToString() +
                                     ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty"));
                    errorCode.Append(";");
                }
                else if (suitePart.ProduceTime == null)
                {
                    errorIndex = i;
                    errorCode.Append(ConvertColumnIndexToStr(columnIndex + 3) + (i + 1).ToString() +
                                     ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty"));
                    errorCode.Append(";");
                }
            }

            return suitePart;
        }

        void client_CheckSecuritySuiteExistCompleted(object sender, CheckSecuritySuiteExistCompletedEventArgs e)
        {
            try
            {
                if (e.Result.TotalRecord == 0)
                {
                    BscDevSuiteServiceClient client = this.InitializeBscDevSuiteServiceClient();
                    client.BatchAddAsync(new ObservableCollection<DevSuite>(uploadList));
                }
                else
                {
                    var list = e.Result.Result.ToList();
                    var suit = list.Where(t => t.Note == "0").ToList();
                    var part = list.Where(t => t.Note == "1").ToList();
                    var both = list.Where(t => t.Note == "2").ToList();

                    //安全套件重复
                    if (suit.Count > 0)
                    {
                        errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("SuitDeviceExist"));
                        errorCode.Append(";");
                        foreach (var item in suit)
                        {
                            errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_SuiteID"));
                            errorCode.Append(item.SuiteID);
                            errorCode.Append(" ");
                            errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("Tip_Or"));
                            errorCode.Append(" ");
                            errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("GIS_MDVR_CORE_SN"));
                            errorCode.Append(item.MdvrSn);
                            errorCode.Append("\r\n");
                            errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("Tip_Or"));
                            errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MDVR_CORE_SN"));
                            errorCode.Append(item.MdvrSn);
                            errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DataExisted"));
                            errorCode.Append(";");
                        }
                    }

                    //部件重复
                    if (part.Count > 0)
                    {
                        errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("PartDeviceExist"));
                        errorCode.Append(";");
                        foreach (var item in part)
                        {
                            foreach (var partItem in item.BscDevSuiteParts)
                            {
                                errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_SuiteID"));
                                errorCode.Append(item.SuiteID);
                                errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString(":"));
                                errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("SuitPart_Sn"));
                                errorCode.Append(partItem.PartSn);
                                errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DataExisted"));
                                errorCode.Append(";");
                            }

                        }
                    }

                    //安全套件与部件都重复
                    if (both.Count > 0)
                    {
                        errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("SuitPartExist"));
                        errorCode.Append(";");
                        foreach (var bothItem in both)
                        {
                            errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_SuiteID"));
                            errorCode.Append(bothItem.SuiteID);
                            errorCode.Append(" ");
                            errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("Tip_Or"));
                            errorCode.Append(" ");
                            errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("GIS_MDVR_CORE_SN"));
                            errorCode.Append(bothItem.MdvrSn);
                            errorCode.Append("\r\n");
                            errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("Tip_Or"));
                            errorCode.Append(" ");
                            errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MDVR_CORE_SN"));
                            errorCode.Append(bothItem.MdvrSn);
                            errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DataExisted"));
                            errorCode.Append(";");
                            foreach (var partItem in bothItem.BscDevSuiteParts)
                            {
                                errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_SuiteID"));
                                errorCode.Append(bothItem.SuiteID);
                                errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString(":"));
                                errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("SuitPart_Sn"));
                                errorCode.Append(partItem.PartSn);
                                errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DataExisted"));
                                errorCode.Append(";");
                            }
                        }
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

        void client_BatchAddCompleted(object sender, BatchAddCompletedEventArgs e)
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
                BscDevSuiteServiceClient client = sender as BscDevSuiteServiceClient;
                this.CloseBscDevSuiteServiceClient(client);
            }
        }

        #endregion
    }
}
