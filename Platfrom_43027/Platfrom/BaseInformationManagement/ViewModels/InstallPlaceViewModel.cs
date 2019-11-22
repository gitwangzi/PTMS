using BaseLib.ViewModels;
using Gsafety.Ant.BaseInformation.Views;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.ServiceReference.InstallStationService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Spreadsheet;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gsafety.Ant.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.InstallPlaceVm)]
    public class InstallPlaceViewModel : UploadAndExportViewModelBase<InstallStation>
    {

        private InstallStation currentInstallStation;

        public InstallStation CurrentInstallStation
        {
            get { return currentInstallStation; }
            set
            {
                currentInstallStation = value;
                RaisePropertyChanged(() => this.CurrentInstallStation);
            }
        }

        public ICommand BtnBindingCommand { get; set; }

        public ICommand BtnCopyCommand { get; set; }


        //
        private string searchByDistrictCode;
        /// <summary>
        /// 
        /// </summary>
        public string SearchByDistrictCode
        {
            get
            {

                return searchByDistrictCode;
            }
            set
            {

                this.searchByDistrictCode = value;
                Validate(ExtractPropertyName(() => SearchByDistrictCode), searchByDistrictCode, SearchByDistrictCode);
                RaisePropertyChanged(() => this.SearchByDistrictCode);
            }
        }
        private void Validate(string prop, string value, string valid)
        {
            ClearErrors(prop);
            if (valid.Length > 20)
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
            }
        }
        //
        private string searchByDirector;
        /// <summary>
        /// 
        /// </summary>
        public string SearchByDirector
        {
            get
            {
                return searchByDirector;
            }
            set
            {
                this.searchByDirector = value;
                Validate(ExtractPropertyName(() => SearchByDirector), searchByDirector, SearchByDirector);
                RaisePropertyChanged(() => this.SearchByDirector);
            }
        }

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
                Validate(ExtractPropertyName(() => SearchByName), searchByName, SearchByName);
                RaisePropertyChanged(() => this.SearchByName);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public InstallPlaceViewModel()
            : base()
        {
            try
            {
                BtnBindingCommand = new ActionCommand<object>(method => Binding());
                BtnCopyCommand = new ActionCommand<object>(method => Copy());
                Url = new Uri(this.GetTemplateUrl(DownLoadType.setupStation));
                this.UploadBtnStatus = true;
                this.ExportBtnStatus = true;

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SetupStationMangeViewModel()", ex);
            }
        }

        /// <summary>
        /// 初始化客户端信息
        /// </summary>
        private InstallStationServiceClient InilizeClient()
        {
            InstallStationServiceClient client = ServiceClientFactory.Create<InstallStationServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.DeleteInstallStationCompleted += client_DeleteInstallStationCompleted;
            client.GetInstallStationsFuzzyCompleted += client_GetInstallStationsFuzzyCompleted;
            client.BatchAddStationCompleted += client_BatchAddStationCompleted;
            client.CheckInstallStationExistCompleted += client_CheckInstallStationExistCompleted;

            return client;
        }

        /// <summary>
        /// 关闭与服务端的连接
        /// </summary>
        /// <param name="client"></param>
        private void CloseClient(InstallStationServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }


        /// <summary>
        /// 查看详细
        /// </summary>
        /// <param name="name"></param>
        protected override void ViewDetail(string name)
        {
            InstallPlaceDetailWindow window = new InstallPlaceDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentInstallStation } });
            window.Show();
        }

        void window_Closed(object sender, EventArgs e)
        {
            InstallPlaceDetailWindow window = sender as InstallPlaceDetailWindow;
            if (window.DialogResult == true)
            {
                Data.RefreshPage();
            }
        }

        private void Copy()
        {
            string name = "copy";
            InstallPlaceDetailWindow window = new InstallPlaceDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentInstallStation } });
            window.Closed += window_Closed;
            window.Show();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        protected override void Update(string name)
        {
            InstallPlaceDetailWindow window = new InstallPlaceDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentInstallStation } });
            window.Closed += window_Closed;
            window.Show();
        }
        protected override void Add(string name)
        {
            InstallPlaceDetailWindow window = new InstallPlaceDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentInstallStation } });
            window.Closed += window_Closed;
            window.Show();

        }

        private void Binding()
        {
            InstallPlaceBindingWindow bindingwindow = new InstallPlaceBindingWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", "view" }, { "model", CurrentInstallStation } });
            bindingwindow.Closed += bindingwindow_Closed;
            bindingwindow.Show();
        }

        void bindingwindow_Closed(object sender, EventArgs e)
        {
            InstallPlaceBindingWindow window = sender as InstallPlaceBindingWindow;
            if (window.DialogResult == true)
            {
                Data.RefreshPage();
            }
        }

        protected override void Delete()
        {
            if (CurrentInstallStation != null)
            {
                var dialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                dialogResult.Closed += dialogResult_Closed;
            }
        }

        private void dialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    InstallStationServiceClient client = InilizeClient();
                    client.DeleteInstallStationAsync(CurrentInstallStation.ID);
                }
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        protected override void Query()
        {
            currentIndex = 1;
            Data.MoveToFirstPage();
        }

        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                Data = new BaseLib.Model.PagedServerCollection<InstallStation>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    PagingInfo page = new PagingInfo();
                    page.PageIndex = pageIndex;
                    page.PageSize = pageSize;
                    InstallStationServiceClient client = InilizeClient();
                    client.GetInstallStationsFuzzyAsync(SearchByDistrictCode, SearchByDirector, SearchByName, page, ApplicationContext.Instance.AuthenticationInfo.ClientID);
                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        private void client_DeleteInstallStationCompleted(object sender, DeleteInstallStationCompletedEventArgs e)
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
                        CurrentInstallStation = null;
                        Data.RefreshPage();
                    }
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_DeleteInstallStationCompleted", ex);
            }
            finally
            {
                InstallStationServiceClient client = sender as InstallStationServiceClient;
                CloseClient(client);
            }
        }

        private void client_GetInstallStationsFuzzyCompleted(object sender, GetInstallStationsFuzzyCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        foreach (var item in e.Result.Result)
                        {
                            item.DistrictName = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.FirstOrDefault(x => x.Code == item.DistrictCode).Name;

                            item.CreateTime = item.CreateTime.ToLocalTime();
                        }
                        Data.loader_Finished(new BaseLib.Model.PagedResult<InstallStation>()
                        {
                            Count = e.Result.TotalRecord,
                            Items = e.Result.Result,//数据列表
                            PageIndex = currentIndex
                        });
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(
                           ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                            ApplicationContext.Instance.Logger.LogException("client_GetInstallStationsFuzzyCompleted", e.Result.ExceptionMessage);
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(
                       ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogException("client_GetInstallStationsFuzzyCompleted", e.Result.ExceptionMessage);
                        }
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(
                       ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_GetInstallStationsFuzzyCompleted", ex);
            }
            finally
            {
                InstallStationServiceClient client = sender as InstallStationServiceClient;
                CloseClient(client);
            }
        }

        private List<InstallStation> uploadList;

        protected override void UploadAction()
        {
            try
            {
                uploadList = new List<InstallStation>();
                batchIndex = 1;
                errorIndex = -1;
                errorCode = errorCode.Clear();
                System.IO.FileInfo fileInfo;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = " Files(*.xlsm)|*.xlsm|All files(*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    fileInfo = openFileDialog.File;
                    if (!fileInfo.Name.EndsWith("xlsm"))
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
                            InstallStationServiceClient client = InilizeClient();
                            client.CheckInstallStationExistAsync(new ObservableCollection<InstallStation>(uploadList));
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
                    InstallStationServiceClient client = ServiceClientFactory.Create<InstallStationServiceClient>();
                    client.GetInstallStationsFuzzyCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord >= 0)
                        {
                            List<string> Codes = new List<string>();
                            Codes.Add("Name");
                            Codes.Add("ProvinceName");
                            Codes.Add("CityName");
                            Codes.Add("Address");
                            Codes.Add("Contact");
                            Codes.Add("ContactPhone");
                            Codes.Add("Director");
                            Codes.Add("DirectorPhone");
                            Codes.Add("Email");
                            Codes.Add("Note");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_SetupStation"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Province"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_City"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Address"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_ContactPerson"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Contact"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Director"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DirecotrPhone"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_EMail"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Note"));

                            foreach (var item in e.Result.Result)
                            {
                                if (item.DistrictCode.Length == 5)
                                {
                                    string provicecode = item.DistrictCode.Substring(0, 2);
                                    var province = ApplicationContext.Instance.BufferManager.DistrictManager.Provinces.FirstOrDefault(n => n.Code == provicecode);
                                    if (province != null)
                                    {
                                        item.ProvinceName = province.Name;
                                    }

                                    var city = ApplicationContext.Instance.BufferManager.DistrictManager.Cities.FirstOrDefault(n => n.Code == item.DistrictCode);
                                    if (city != null)
                                    {
                                        item.CityName = city.Name;
                                    }

                                }
                                else if (item.DistrictCode.Length == 2)
                                {
                                    var province = ApplicationContext.Instance.BufferManager.DistrictManager.Provinces.FirstOrDefault(n => n.Code == item.DistrictCode);
                                    if (province != null)
                                    {
                                        item.ProvinceName = province.Name;
                                    }
                                }
                            }

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
                        Gsafety.PTMS.ServiceReference.InstallStationService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.InstallStationService.PagingInfo() { PageIndex = 1, PageSize = 10000 };
                        client.GetInstallStationsFuzzyAsync(SearchByDistrictCode, SearchByDirector, SearchByName, pagingInfo, ApplicationContext.Instance.AuthenticationInfo.ClientID);
                    }
                    else
                    {
                        Gsafety.PTMS.ServiceReference.InstallStationService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.InstallStationService.PagingInfo() { PageIndex = 1, PageSize = Data.TotalItemCount };
                        client.GetInstallStationsFuzzyAsync(SearchByDistrictCode, SearchByDirector, SearchByName, pagingInfo, ApplicationContext.Instance.AuthenticationInfo.ClientID);
                    }
                }

            }
            catch (Exception ex)
            {
                setExportBtnStatus(true);
            }
        }

        private List<InstallStation> GetUploadList()
        {
            List<InstallStation> tempStationList = new List<InstallStation>();

            int startIndex = batchIndex;
            int endIndex = uploadContent.Length;
            for (int i = startIndex; i < endIndex; i++)
            {
                InstallStation station = new InstallStation();
                station.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                station.Name = uploadContent[i][0].ToString().Trim();
                if (string.IsNullOrEmpty(station.Name))
                {
                    errorIndex = i;
                    errorCode.Append("A" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty")); ;
                    errorCode.Append(";");
                }
                if (tempStationList.Any(t => t.Name == station.Name))
                {
                    errorIndex = i;
                    errorCode.Append("A" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame"));
                    errorCode.Append(";");
                }
                if (station.Name.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode.Append("A" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                var province = uploadContent[i][1].ToString().Trim();
                var city = uploadContent[i][2].ToString().Trim();
                var provinceCode = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.FirstOrDefault(t => t.Code.Length == 2 && t.Name == province).Code;
                station.DistrictCode = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 5 && x.Name == city && x.Code.Substring(0, 2) == provinceCode).Select(x => x.Code).FirstOrDefault();
                if (string.IsNullOrEmpty(station.DistrictCode))
                {
                    errorIndex = i;
                    errorCode.Append("C" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("InfoisError")); ;
                    errorCode.Append(";");
                }

                station.Address = uploadContent[i][3].ToString().Trim();
                if (station.Address.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode.Append("D" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                station.Contact = uploadContent[i][4].ToString().Trim();
                if (station.Contact.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode.Append("E" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                station.ContactPhone = uploadContent[i][5].ToString().Trim();
                if (!ValidatePhone(uploadContent[i][7].ToString().Trim()))
                {
                    errorIndex = i;
                    errorCode.Append("F" + (i + 1).ToString() +
                                     ApplicationContext.Instance.StringResourceReader.GetString(
                                         "Phone_Illegal"));
                    errorCode.Append(";");
                }
                if (station.ContactPhone.Trim().Length > 20)
                {
                    errorIndex = i;
                    errorCode.Append("I" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                station.Director = uploadContent[i][6].ToString().Trim();
                if (station.Director.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode.Append("G" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                station.DirectorPhone = uploadContent[i][7].ToString().Trim();
                if (!ValidatePhone(uploadContent[i][7].ToString().Trim()))
                {
                    errorIndex = i;
                    errorCode.Append("H" + (i + 1).ToString() +
                                     ApplicationContext.Instance.StringResourceReader.GetString(
                                         "Phone_Illegal"));
                    errorCode.Append(";");
                }
                if (station.DirectorPhone.Trim().Length > 20)
                {
                    errorIndex = i;
                    errorCode.Append("H" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                station.Email = uploadContent[i][8].ToString().Trim();
                if (ValidateEmail(station.Email))
                {
                    errorIndex = i;
                    errorCode.Append("I" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_EMail") + "\n" + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"));
                    errorCode.Append(";");
                }
                if (station.Email.Trim().Length > 100)
                {
                    errorIndex = i;
                    errorCode.Append("I" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                station.Note = uploadContent[i][9].ToString();
                if (station.Note.Trim().Length > 2000)
                {
                    errorIndex = i;
                    errorCode.Append("J" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                tempStationList.Add(station);
            }

            return tempStationList;
        }

        void client_CheckInstallStationExistCompleted(object sender, CheckInstallStationExistCompletedEventArgs e)
        {
            try
            {
                if (e.Result.TotalRecord == 0)
                {
                    InstallStationServiceClient client = InilizeClient();
                    client.BatchAddStationAsync(new ObservableCollection<InstallStation>(uploadList));
                }
                else
                {
                    var list = e.Result.Result.ToList();
                    foreach (var item in list)
                    {
                        errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("InstallStation"));
                        errorCode.Append(item.Name);
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

        void client_BatchAddStationCompleted(object sender, BatchAddStationCompletedEventArgs e)
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
                InstallStationServiceClient client = sender as InstallStationServiceClient;
                CloseClient(client);
            }
        }
    }
}
