using BaseLib.ViewModels;
using Gsafety.Ant.BaseInformation.Views;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.ChauffeurService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Collections.ObjectModel;
using Gsafety.PTMS.Spreadsheet;

namespace Gsafety.Ant.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.DriverInfoVm)]
    public class DriverInfoViewModel : UploadAndExportViewModelBase<Chauffeur>
    {


        #region 属性


        #region headList...
        private string chuffeurNameH;
        /// <summary>
        /// 
        /// </summary>
        public string ChuffeurNameH
        {
            get
            {
                return chuffeurNameH;
            }
            set
            {
                this.searchByName = value;
                RaisePropertyChanged(() => this.ChuffeurNameH);
            }
        }


        private string driverLicenseH;
        /// <summary>
        /// 
        /// </summary>
        public string DriverLicenseH
        {
            get
            {
                return driverLicenseH;
            }
            set
            {
                this.searchByName = value;
                RaisePropertyChanged(() => this.DriverLicenseH);
            }
        }

        private string iCardIDH;
        /// <summary>
        /// 
        /// </summary>
        public string ICardIDH
        {
            get
            {
                return iCardIDH;
            }
            set
            {
                this.iCardIDH = value;
                RaisePropertyChanged(() => this.ICardIDH);
            }
        }

        private string phoneH;
        /// <summary>
        /// 
        /// </summary>
        public string PhoneH
        {
            get
            {
                return phoneH;
            }
            set
            {
                this.phoneH = value;
                RaisePropertyChanged(() => this.PhoneH);
            }
        }

        private string emailH;
        /// <summary>
        /// 
        /// </summary>
        public string EmailH
        {
            get
            {
                return emailH;
            }
            set
            {
                this.emailH = value;
                RaisePropertyChanged(() => this.EmailH);
            }
        }

        private string addressH;
        /// <summary>
        /// 
        /// </summary>
        public string AddressH
        {
            get
            {
                return addressH;
            }
            set
            {
                this.addressH = value;
                RaisePropertyChanged(() => this.AddressH);
            }
        }

        private string operatorH;
        /// <summary>
        /// 
        /// </summary>
        public string OperatorH
        {
            get
            {
                return operatorH;
            }
            set
            {
                this.operatorH = value;
                RaisePropertyChanged(() => this.OperatorH);
            }
        }
        #endregion

        public List<EnumModel> ChauffeurList { get; set; }

        public EnumModel CurrentInstallStatus { get; set; }

        private Chauffeur currentChauffeur;

        public Chauffeur CurrentChauffeur
        {
            get { return currentChauffeur; }
            set
            {
                currentChauffeur = value;
                RaisePropertyChanged(() => this.CurrentChauffeur);
            }
        }

        public ICommand BtnBindingCommand { get; set; }

        #region 查询条件
        //
        private string searchByICard;
        /// <summary>
        /// 
        /// </summary>
        public string SearchByICard
        {
            get
            {
                return searchByICard;
            }
            set
            {
                this.searchByICard = value;
                Validate(ExtractPropertyName(() => SearchByICard), searchByICard, SearchByICard);
                RaisePropertyChanged(() => this.SearchByICard);
            }
        }

        //
        private string searchByLicence;
        /// <summary>
        /// 
        /// </summary>
        public string SearchByLicence
        {
            get
            {
                return searchByLicence;
            }
            set
            {
                this.searchByLicence = value;
                Validate(ExtractPropertyName(() => SearchByLicence), searchByLicence, SearchByLicence);
                RaisePropertyChanged(() => this.SearchByLicence);
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

        private void Validate(string prop, string value, string valid)
        {
            ClearErrors(prop);
            if (valid.Length > 20)
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
            }
        }

        #endregion


        #endregion


        #region 构造函数

        /// <summary>
        /// 初始化内容
        /// </summary>
        public DriverInfoViewModel()
            : base()
        {
            try
            {
                BtnBindingCommand = new ActionCommand<object>(method => Binding());
                Url = new Uri(this.GetTemplateUrl(DownLoadType.Driver));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SetupStationMangeViewModel()", ex);
            }
        }
        #endregion


        #region 方法

        /// <summary>
        /// 初始化驾驶员服务客户端
        /// </summary>
        /// <returns></returns>
        private ChauffeurServiceClient InitialClient()
        {
            ChauffeurServiceClient client = ServiceClientFactory.Create<ChauffeurServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);

            client.DeleteChauffeurCompleted += client_DeleteChauffeurCompleted;
            client.GetChauffeurByConditionCompleted += client_GetChauffeurByConditionCompleted;
            client.CheckChauffeurExistCompleted += client_CheckChauffeurExistCompleted;
            client.BatchAddChauffeurCompleted += client_BatchAddChauffeurCompleted;
            return client;
        }

        protected override void ViewDetail(string name)
        {
            DriverInfoDetailWindow window = new DriverInfoDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentChauffeur } });
            window.Show();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        protected override void Update(string name)
        {
            DriverInfoDetailWindow window = new DriverInfoDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentChauffeur } });
            window.Closed += window_Closed;
            window.Show();

        }
        protected override void Add(string name)
        {

            DriverInfoDetailWindow window = new DriverInfoDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentChauffeur } });
            window.Closed += window_Closed;
            window.Show();
        }

        private void Binding()
        {
            DriverInfoBindingWindow windowbinding = new DriverInfoBindingWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", "view" }, { "model", CurrentChauffeur } });
            windowbinding.Closed += windowbinding_Closed;
            windowbinding.Show();
        }

        private void windowbinding_Closed(object sender, EventArgs e)
        {
            DriverInfoBindingWindow driverBingwindow = sender as DriverInfoBindingWindow;
            if (driverBingwindow != null)
            {
                Data.RefreshPage();
            }
        }

        void window_Closed(object sender, EventArgs e)
        {
            DriverInfoDetailWindow driverwindow = sender as DriverInfoDetailWindow;
            if (driverwindow != null)
            {
                Data.RefreshPage();
            }
        }

        protected async override void Delete()
        {

            try
            {
                var dialogResult = MessageBoxHelper.ShowDialogMessageTask(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                var result = await dialogResult;
                if (result == MessageDialogResult.OK)
                {

                    ChauffeurServiceClient client = InitialClient();
                    if (CurrentChauffeur != null)
                        client.DeleteChauffeurAsync(CurrentChauffeur.ID);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void dialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    ChauffeurServiceClient client = InitialClient();
                    client.DeleteChauffeurAsync(CurrentChauffeur.ID);
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
                Data = new BaseLib.Model.PagedServerCollection<Chauffeur>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
               
                    PagingInfo page = new PagingInfo();
                    page.PageIndex = pageIndex;
                    page.PageSize = pageSize;
                    ChauffeurServiceClient client = InitialClient();
                    client.GetChauffeurByConditionAsync(SearchByName, SearchByLicence, SearchByICard, page, ApplicationContext.Instance.AuthenticationInfo.ClientID);
                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        #region do Completed

        /// <summary>
        /// 删除驾驶员完成方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void client_DeleteChauffeurCompleted(object sender, DeleteChauffeurCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("client_DeleteChauffeurCompleted", ex);
            }
            finally
            {
                ChauffeurServiceClient client = sender as ChauffeurServiceClient;
                CloseClient(client);

            }
        }

        /// <summary>
        /// 关闭驾驶员服务客户端
        /// </summary>
        /// <param name="client"></param>
        private void CloseClient(ChauffeurServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        private void client_GetChauffeurByConditionCompleted(object sender, GetChauffeurByConditionCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        foreach (var item in e.Result.Result)
                        {
                            item.CreateTime = item.CreateTime.ToLocalTime();
                        }

                        Data.loader_Finished(new BaseLib.Model.PagedResult<Chauffeur>()
                        {
                            Count = e.Result.TotalRecord,
                            Items = e.Result.Result,//数据列表
                            PageIndex = currentIndex
                        });
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
                ApplicationContext.Instance.Logger.LogException("client_GetChauffeurByConditionCompleted", ex);
            }
            finally
            {
                ChauffeurServiceClient client = sender as ChauffeurServiceClient;
                CloseClient(client);
            }
        }

        #endregion


        private void InitialHeader()
        {
            ChuffeurNameH = ApplicationContext.Instance.StringResourceReader.GetString("ConfirmDelete");
        }

        #endregion

        #region Upload and Export
        private List<Chauffeur> uploadList;

        protected override void UploadAction()
        {
            try
            {
                uploadList = new List<Chauffeur>();
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
                            ChauffeurServiceClient client = InitialClient();
                            client.CheckChauffeurExistAsync(new ObservableCollection<Chauffeur>(uploadList));
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
                    ChauffeurServiceClient client = ServiceClientFactory.Create<ChauffeurServiceClient>();
                    client.GetChauffeurByConditionCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord >= 0)
                        {
                            List<string> Codes = new List<string>();
                            Codes.Add("Name");
                            Codes.Add("ICardID");
                            Codes.Add("DriverLicense");
                            Codes.Add("Address");
                            Codes.Add("Phone");
                            Codes.Add("CellPhone");
                            Codes.Add("Email");
                            Codes.Add("Note");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ChuffeurName"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_IdentityID"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("DRIVER_LICENSE"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_Address"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ContactPhone"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("GUser_Mobile"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_EMail"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Note"));

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
                        Gsafety.PTMS.ServiceReference.ChauffeurService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.ChauffeurService.PagingInfo() { PageIndex = 1, PageSize = 10000 };
                        client.GetChauffeurByConditionAsync(string.Empty, string.Empty, string.Empty, pagingInfo, ApplicationContext.Instance.AuthenticationInfo.ClientID);
                    }
                    else
                    {
                        Gsafety.PTMS.ServiceReference.ChauffeurService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.ChauffeurService.PagingInfo() { PageIndex = 1, PageSize = Data.TotalItemCount };
                        client.GetChauffeurByConditionAsync(string.Empty, string.Empty, string.Empty, pagingInfo, ApplicationContext.Instance.AuthenticationInfo.ClientID);
                    }
                }

            }
            catch (Exception ex)
            {
                setExportBtnStatus(true);
            }
        }

        private List<Chauffeur> GetUploadList()
        {
            List<Chauffeur> chauffeurList = new List<Chauffeur>();

            int startIndex = batchIndex;
            int endIndex = uploadContent.Length;
            for (int i = startIndex; i < endIndex; i++)
            {
                Chauffeur chauffeur = new Chauffeur();
                chauffeur.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                chauffeur.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;
                chauffeur.Name = uploadContent[i][0].ToString().Trim();
                if (string.IsNullOrEmpty(chauffeur.Name))
                {
                    errorIndex = i;
                    errorCode.Append("A" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty")); ;
                    errorCode.Append(";");
                }

                if (chauffeur.Name.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode.Append("A" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                chauffeur.ICardID = uploadContent[i][1].ToString().Trim();
                if (string.IsNullOrEmpty(chauffeur.ICardID))
                {
                    errorIndex = i;
                    errorCode.Append("B" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty")); ;
                    errorCode.Append(";");
                }
                if (chauffeur.ICardID.Trim().Length > 50)
                {
                    errorIndex = i;
                    errorCode.Append("B" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                chauffeur.DriverLicense = uploadContent[i][2].ToString().Trim();
                if (string.IsNullOrEmpty(chauffeur.DriverLicense))
                {
                    errorIndex = i;
                    errorCode.Append("C" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty")); ;
                    errorCode.Append(";");
                }
                if (chauffeur.DriverLicense.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode.Append("C" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                chauffeur.Address = uploadContent[i][3].ToString().Trim();
                if (string.IsNullOrEmpty(chauffeur.Address))
                {
                    errorIndex = i;
                    errorCode.Append("D" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty")); ;
                    errorCode.Append(";");
                }
                if (chauffeur.Address.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode.Append("D" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                chauffeur.CellPhone = uploadContent[i][4].ToString().Trim();
                if (string.IsNullOrEmpty(chauffeur.CellPhone))
                {
                    errorIndex = i;
                    errorCode.Append("E" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty")); ;
                    errorCode.Append(";");
                }
                if (!ValidatePhone(uploadContent[i][4].ToString().Trim()))
                {
                    errorIndex = i;
                    errorCode.Append("E" + (i + 1).ToString() +
                                     ApplicationContext.Instance.StringResourceReader.GetString(
                                         "Phone_Illegal"));
                    errorCode.Append(";");
                }
                if (chauffeur.CellPhone.Trim().Length > 20)
                {
                    errorIndex = i;
                    errorCode.Append("E" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                chauffeur.Phone = uploadContent[i][5].ToString().Trim();
                if (string.IsNullOrEmpty(chauffeur.Phone))
                {
                    errorIndex = i;
                    errorCode.Append("F" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty")); ;
                    errorCode.Append(";");
                }
                if (!ValidatePhone(uploadContent[i][5].ToString().Trim()))
                {
                    errorIndex = i;
                    errorCode.Append("F" + (i + 1).ToString() +
                                     ApplicationContext.Instance.StringResourceReader.GetString(
                                         "Phone_Illegal"));
                    errorCode.Append(";");
                }
                if (chauffeur.Phone.Trim().Length > 20)
                {
                    errorIndex = i;
                    errorCode.Append("F" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                chauffeur.Email = uploadContent[i][6].ToString().Trim();
                if (string.IsNullOrEmpty(chauffeur.Email))
                {
                    errorIndex = i;
                    errorCode.Append("G" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty")); ;
                    errorCode.Append(";");
                }
                if (ValidateEmail(chauffeur.Email))
                {
                    errorIndex = i;
                    errorCode.Append("G" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_EMail") + "\n" + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"));
                    errorCode.Append(";");
                }
                if (chauffeur.Email.Trim().Length > 100)
                {
                    errorIndex = i;
                    errorCode.Append("G" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                chauffeur.Note = uploadContent[i][7].ToString();
                if (chauffeur.Note.Trim().Length > 2000)
                {
                    errorIndex = i;
                    errorCode.Append("H" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                chauffeurList.Add(chauffeur);
            }

            return chauffeurList;
        }

        void client_CheckChauffeurExistCompleted(object sender, CheckChauffeurExistCompletedEventArgs e)
        {
            try
            {
                if (e.Result.TotalRecord == 0)
                {
                    ChauffeurServiceClient client = InitialClient();
                    client.BatchAddChauffeurAsync(new ObservableCollection<Chauffeur>(uploadList));
                }
                else
                {
                    var list = e.Result.Result.ToList();
                    foreach (var item in list)
                    {
                        errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("Churffure"));
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

        void client_BatchAddChauffeurCompleted(object sender, BatchAddChauffeurCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
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
                ChauffeurServiceClient client = sender as ChauffeurServiceClient;
                CloseClient(client);
            }
        }

        #endregion

    }
}
