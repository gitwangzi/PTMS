using Gsafety.Ant.BaseInformation.Model;
using Gsafety.Ant.BaseInformation.Views.Organization;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.VehicleService;
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
using Vehicle = Gsafety.PTMS.ServiceReference.VehicleService.Vehicle;

namespace Gsafety.Ant.BaseInformation.ViewModels.OrganizationViewModel
{
    /// <summary>
    /// 车辆管理
    /// </summary>
    [ExportAsViewModel(BaseInformationName.VehicleDepartmentListVm)]
    public class VehicleDepartmentListViewModel : UploadAndExportViewModelBase<Vehicle>
    {

        #region 属性

        public ICommand BtnDetailCommand { get; set; }
        public ICommand BtnBindingCommand { get; set; }

        #region 查询条件属性

        private string searchVehicleId;
        /// <summary>
        /// 查询条件 车牌号
        /// </summary>
        public string SearchVehicleId
        {
            get { return searchVehicleId; }
            set
            {
                searchVehicleId = value;
                RaisePropertyChanged(() => SearchVehicleId);
            }
        }


        private string searchOwner;
        /// <summary>
        /// 查询条件 车主
        /// </summary>
        public string SearchOwner
        {
            get { return searchOwner; }
            set
            {
                searchOwner = value;
                RaisePropertyChanged(() => SearchOwner);
            }
        }

        private string searchVehicleType;
        /// <summary>
        /// 查询条件 车类型
        /// </summary>
        public string SearchVehicleType
        {
            get { return searchVehicleType; }
            set
            {
                searchVehicleType = value;
                RaisePropertyChanged(() => SearchVehicleType);
            }
        }

        private Vehicle vehicleObj = new Vehicle();
        /// <summary>
        /// 当前列表选中的车辆
        /// </summary>
        public Vehicle VehicleObj
        {
            get { return vehicleObj; }
            set
            {
                vehicleObj = value;
                RaisePropertyChanged(() => VehicleObj);
            }
        }
        #endregion

        private string parentId;
        /// <summary>
        /// 车辆所属的车辆组织机构编号
        /// </summary>
        public string ParentId
        {
            get { return this.parentId; }
            set
            {
                this.parentId = value;
                RaisePropertyChanged(() => this.ParentId);
            }
        }

        private string parentName;

        public string ParentName
        {
            get { return this.parentName; }
            set
            {
                this.parentName = value;
            }
        }

        List<VehicleType> vehciletypes = new List<VehicleType>();

        private List<VehicleType> bindvehicletypes = new List<VehicleType>();
        /// <summary>
        /// 所有省份
        /// </summary>
        public List<VehicleType> VehicleTypes
        {
            get { return bindvehicletypes; }
            set
            {
                bindvehicletypes = value;
                RaisePropertyChanged(() => VehicleTypes);
            }
        }

        private VehicleType vehicletype = null;

        public VehicleType VehicleType
        {
            get { return vehicletype; }
            set { vehicletype = value; }
        }

        #endregion

        #region 构造函数

        public VehicleDepartmentListViewModel()
        {
            try
            {
                BtnEditCommand = new ActionCommand<object>(obj => Update("update"));
                BtnDetailCommand = new ActionCommand<object>(obj => Update("view"));
                BtnDeleteCommand = new ActionCommand<object>(obj => DeleteVehicle());
                BtnSearchCommand = new ActionCommand<object>(obj => Query());
                BtnBindingCommand = new ActionCommand<object>(obj => Binding());
                Url = new Uri(this.GetTemplateUrl(DownLoadType.Vehicle));
                this.UploadBtnStatus = true;
                this.ExportBtnStatus = true;
                //InitialVehicleType();
                InitialVehicleSeviceType();

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 初始化客户端的方法
        /// </summary>
        private VehicleServiceClient InitVehicleServiceClient()
        {
            VehicleServiceClient vehicleClient = ServiceClientFactory.Create<VehicleServiceClient>();
            vehicleClient.DeleteVehicleCompleted += vehicleClient_DeleteVehicleCompleted;
            vehicleClient.BatchAddCompleted += vehicleClient_BatchAddCompleted;
            vehicleClient.CheckVehicleExistCompleted += vehicleClient_CheckVehicleExistCompleted;
            return vehicleClient;
        }

        /// <summary>
        /// 初始化车辆类别服务客户端方法
        /// </summary>
        /// <returns></returns>
        private VehicleTypeClient InitVehicleTypeClient()
        {
            VehicleTypeClient vehicleClient = ServiceClientFactory.Create<VehicleTypeClient>();
            vehicleClient.GetBscVehicleListCompleted += vehicleClient_GetBscVehicleListCompleted;
            vehicleClient.GetVehicleTypeListCompleted += vehicleClient_GetVehicleTypeListCompleted;

            return vehicleClient;
        }

        void vehicleClient_GetVehicleTypeListCompleted(object sender, GetVehicleTypeListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        vehciletypes.Clear();
                        foreach (var item in e.Result.Result)
                        {
                            vehciletypes.Add(item);
                            bindvehicletypes.Add(item);
                        }

                        RaisePropertyChanged(() => VehicleTypes);
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

                ApplicationContext.Instance.Logger.LogException("VehicleDepartmentListViewModel.vehicleClient_GetVehicleTypeListCompleted", ex);
            }
            finally
            {
                VehicleTypeClient vehicleClient = sender as VehicleTypeClient;
                vehicleClient.CloseAsync();
                vehicleClient = null;
            }

        }

        /// <summary>
        /// 激活视图
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="viewParameters"></param>
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                this.ParentId = "";
                if (viewParameters.Count > 0)
                {
                    this.ParentId = viewParameters["VehicleDepartmentId"].ToString();
                    this.ParentName = viewParameters["VehicleDepartmentName"].ToString();
                }

                bindvehicletypes.Clear();
                bindvehicletypes.Add(new VehicleType() { ID = string.Empty, Name = ApplicationContext.Instance.StringResourceReader.GetString("All") });
                VehicleType = bindvehicletypes[0];
                RaisePropertyChanged(() => VehicleType);
                VehicleTypeClient client = InitVehicleTypeClient();
                client.GetVehicleTypeListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);

                base.ActivateView(viewName, viewParameters);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// 初始化页面需加载的数据
        /// </summary>
        protected override void InitPagination()
        {

            try
            {
                Data = new BaseLib.Model.PagedServerCollection<Vehicle>((pageIndex, pageSize) =>
                {
                    if (VehicleType != null)
                    {
                        pageSize = PageSizeValue;
                        System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                        VehicleTypeClient vehicleClient = InitVehicleTypeClient();
                        vehicleClient.GetBscVehicleListAsync(currentIndex, PageSizeValue, SearchVehicleId, SearchOwner, SearchVehicleId, this.ParentId, VehicleType.ID);
                    }
                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleDepartmentViewModel().InitPagination", ex);
            }
        }

        private void CloseVehicleService(VehicleServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }

        private void CloseVehicleService(VehicleTypeClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }


        /// <summary>
        /// 查询车辆
        /// </summary>
        protected override void Query()
        {
            try
            {
                currentIndex = 1;
                Data.MoveToFirstPage();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleDepartmentViewModel()", ex);
            }
        }


        /// <summary>
        /// 添加车辆
        /// </summary>
        /// <returns></returns>
        protected override void Add(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(this.ParentName) || string.IsNullOrEmpty(this.ParentId))
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("SelectED"));
                }
                else
                {
                    VehicleObj = new Vehicle();
                    VehicleObj.OrgnizationId = this.ParentId;
                    VehicleObj.OrgnizationName = this.ParentName;

                    var addVehicleDepartmentDetailView = new AddVehicleDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { 
                { "action", name }, { "data", VehicleObj},{"OrgnizationId",this.ParentId},{"OrgnizationName",this.ParentName},{"VehicleTypes",vehciletypes}
                });
                    addVehicleDepartmentDetailView.Show();
                    addVehicleDepartmentDetailView.Closed += addVehicleDepartmentDetailView_Closed;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void addVehicleDepartmentDetailView_Closed(object sender, EventArgs e)
        {
            Data.RefreshPage();
        }

        /// <summary>
        /// 编辑和查看车辆明细
        ///update,view
        /// </summary>
        /// <param name="name"></param>
        protected override void Update(string name)
        {
            try
            {
                VehicleObj.OrgnizationId = this.ParentId;
                VehicleObj.OrgnizationName = this.ParentName;
                var addVehicleDepartmentDetailView = new AddVehicleDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { 
                { "action", name }, { "data", VehicleObj} ,{"VehicleTypes",vehciletypes}
                });
                addVehicleDepartmentDetailView.Show();
                addVehicleDepartmentDetailView.Closed += addVehicleDepartmentDetailView_Closed;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }

        /// <summary>
        /// 删除车辆
        /// </summary>
        private async void DeleteVehicle()
        {
            try
            {
                if (VehicleObj == null)
                {
                    string message = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect");
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), message);
                    return;
                }
                else
                {
                    var dialogResult = MessageBoxHelper.ShowDialogMessageTask(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                    var result = await dialogResult;

                    if (result == MessageDialogResult.OK)
                    {
                        VehicleServiceClient vehicleClient = InitVehicleServiceClient();
                        vehicleClient.DeleteVehicleAsync(VehicleObj);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// 删除车辆服务完成方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void vehicleClient_DeleteVehicleCompleted(object sender, DeleteVehicleCompletedEventArgs e)
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
                VehicleServiceClient vehicleClient = sender as VehicleServiceClient;
                CloseVehicleService(vehicleClient);
            }
        }

        /// <summary>
        /// 获取车辆列表服务完成方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void vehicleClient_GetBscVehicleListCompleted(object sender, GetBscVehicleListCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    return;
                }

                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        MessageBoxHelper.ShowDialog(
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(
                       ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                       ApplicationContext.Instance.StringResourceReader.GetString(result.ErrorMsg));
                    }
                }
                else
                {
                    foreach (var item in e.Result.Result)
                    {
                        item.CreateTime = item.CreateTime.ToLocalTime();
                    }
                    Data.loader_Finished(new BaseLib.Model.PagedResult<Vehicle>()
                 {
                     Count = e.Result.TotalRecord,
                     Items = e.Result.Result,
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
                VehicleTypeClient vehicleClient = sender as VehicleTypeClient;
                CloseVehicleService(vehicleClient);
            }
        }

        private void Binding()
        {
            if (VehicleObj.VehicleId != string.Empty)
            {
                VehicleBindingDriverWindow vbd = new VehicleBindingDriverWindow(VehicleObj.VehicleId);
                vbd.Show();
            }
        }
        #endregion

        #region Upload Download Export
        private List<Vehicle> uploadList;

        protected override void UploadAction()
        {
            try
            {
                uploadList = new List<Vehicle>();
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
                            VehicleServiceClient vehicleClient = InitVehicleServiceClient();
                            vehicleClient.CheckVehicleExistAsync(new ObservableCollection<Vehicle>(uploadList));
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
                    VehicleTypeClient client = ServiceClientFactory.Create<VehicleTypeClient>();
                    ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
                    client.GetBscVehicleListCompleted += (s, e) =>
                    {
                        try
                        {
                            if (e.Result != null && e.Result.TotalRecord > 0)
                            {
                                foreach (var item in e.Result.Result)
                                {
                                    item.CreateTime = item.CreateTime.ToLocalTime();
                                }
                                List<string> Codes = new List<string>();
                                Codes.Add("VehicleId");
                                Codes.Add("VehicleSn");
                                Codes.Add("EngineId");
                                Codes.Add("VehicleType");
                                Codes.Add("ProvinceName");
                                Codes.Add("CityName");
                                Codes.Add("BrandModel");
                                Codes.Add("StartYear");
                                Codes.Add("ServiceType");
                                Codes.Add("Region");
                                Codes.Add("OperationLicense");
                                Codes.Add("Owner");
                                Codes.Add("IdentityID");
                                Codes.Add("ContactPhone");
                                Codes.Add("ContactEmail");
                                Codes.Add("ContactAddress");
                                Codes.Add("Note");

                                List<string> Names = new List<string>();
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_VehicleNumber"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_VehicleSN"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_EngineID"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_VehicleType"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Province"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_City"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_VehicleBrandModel"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_StartYear"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_ServiceType"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("RunningArea"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("OperatingLicense"));
                               
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_VehicleOwner"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_IdentityID"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Phone"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Email"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Address"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Note"));

                                List<EnumsEx> eList = new List<EnumsEx>();

                                List<FieldEx> FieldEx2 = new List<FieldEx>();
                                Enum.GetNames(typeof(Gsafety.PTMS.ServiceReference.VehicleService.VehicleSeviceType)).ToList().ForEach(x =>
                                {
                                    FieldEx item = new FieldEx { Key = x, Value = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                                    FieldEx2.Add(item);
                                });
                                eList.Add(new EnumsEx { Code = "ServiceType", Content = FieldEx2 });

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

                                List<VehicleExportModel> list = new List<VehicleExportModel>();
                                foreach (var item in e.Result.Result)
                                {
                                    try
                                    {
                                        VehicleExportModel vem = new VehicleExportModel();
                                        vem.VehicleId = item.VehicleId;
                                        vem.ServiceType = item.ServiceType;
                                        vem.ProvinceName = item.ProvinceName;
                                        vem.CityName = item.CityName;
                                        vem.VehicleSn = item.VehicleSn;
                                        vem.EngineId = item.EngineId;
                                        vem.BrandModel = item.BrandModel;
                                        var vehicletype = vehciletypes.Where(t => t.ID == item.VehicleType.ID).FirstOrDefault();
                                        if (vehicletype != null)
                                            vem.VehicleType = vehicletype.Name;
                                        vem.Region = item.Region;
                                        vem.OperationLicense = item.OperationLicense;
                                        vem.StartYear = item.StartYear;
                                        vem.Owner = item.Owner;
                                        vem.Contact = item.Contact;
                                        vem.ContactPhone = item.ContactPhone;
                                        vem.ContactEmail = item.ContactEmail;
                                        vem.ContactAddress = item.ContactAddress;
                                        vem.Note = item.Note;
                                        list.Add(vem);
                                    }
                                    catch (Exception)
                                    {

                                    }

                                }

                                XLSXExporter xlsx = new XLSXExporter();
                                xlsx.Export(list, dlg.OpenFile(), Codes, Names, eList);
                                setExportBtnStatus(true);
                                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("BaseInfo_ExportSucceed"), MessageDialogButton.Ok);
                            }
                            else
                            {
                                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("BaseInfo_ExportFaild"), MessageDialogButton.Ok);
                            }

                            CloseVehicleService(client);
                        }
                        catch (Exception)
                        {

                        }
                    };


                    if (Data.TotalItemCount > 10000)
                    {
                        Gsafety.PTMS.ServiceReference.VehicleService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.VehicleService.PagingInfo() { PageIndex = 1, PageSize = 10000 };
                        client.GetBscVehicleListAsync(currentIndex, 10000, SearchVehicleId, SearchOwner, SearchVehicleId, this.ParentId, VehicleType.ID);
                    }
                    else
                    {
                        client.GetBscVehicleListAsync(1, Data.TotalItemCount, SearchVehicleId, SearchOwner, SearchVehicleId, this.ParentId, VehicleType.ID);
                    }
                }
            }
            catch (Exception ex)
            {
                setExportBtnStatus(true);
            }
        }

        private List<Vehicle> GetUploadList()
        {
            List<Vehicle> tempVehicleList = new List<Vehicle>();
            int startIndex = batchIndex;
            int endIndex = uploadContent.Length;
            for (int i = startIndex; i < endIndex; i++)
            {
                Vehicle tempVehicle = new Vehicle();
                tempVehicle.ClientId = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                tempVehicle.VehicleId = uploadContent[i][0].ToString().Trim();
                tempVehicle.InstallStatus = InstallStatusType.UnInstall;
                if (tempVehicle.VehicleId == string.Empty)
                {
                    errorIndex = i;
                    errorCode.Append("A" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty"));
                    errorCode.Append(";");
                }
                if (tempVehicle.VehicleId.Trim().Length > 20)
                {
                    errorIndex = i;
                    errorCode.Append("A" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }
                if (tempVehicleList.Any(t => t.VehicleId == tempVehicle.VehicleId.Trim()))
                {
                    errorIndex = i;
                    errorCode.Append("A" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame"));
                    errorCode.Append(";");
                }

                tempVehicle.VehicleSn = uploadContent[i][1].ToString().Trim();
                if (tempVehicle.VehicleSn == string.Empty)
                {
                    errorIndex = i;
                    errorCode.Append("B" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty"));
                    errorCode.Append(";");
                }
                if (tempVehicleList.Any(t => t.VehicleSn == tempVehicle.VehicleSn.Trim()))
                {
                    errorIndex = i;
                    errorCode.Append("B" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame"));
                    errorCode.Append(";");
                }

                if (tempVehicle.VehicleSn.Trim().Length > 20)
                {
                    errorIndex = i;
                    errorCode.Append("B" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                tempVehicle.EngineId = uploadContent[i][2].ToString().Trim();
                if (tempVehicle.EngineId == string.Empty)
                {
                    errorIndex = i;
                    errorCode.Append("C" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty"));
                    errorCode.Append(";");
                }
                if (tempVehicle.EngineId.Trim().Length > 50)
                {
                    errorIndex = i;
                    errorCode.Append("C" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }
                if (tempVehicleList.Any(t => t.EngineId == tempVehicle.EngineId.Trim()))
                {
                    errorIndex = i;
                    errorCode.Append("C" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame"));
                    errorCode.Append(";");
                }

                string typeName = uploadContent[i][3].ToString().Trim();
                if (string.IsNullOrEmpty(typeName))
                {
                    errorIndex = i;
                    errorCode.Append("D" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty"));
                    errorCode.Append(";");
                }
                VehicleType vtype = vehciletypes.Where(t => t.Name == typeName).FirstOrDefault();
                if (vtype == null)
                {
                    errorIndex = i;
                    errorCode.Append("D" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("VehicleTypeNotExist"));
                    errorCode.Append(";");
                }
                tempVehicle.VehicleType = vtype;

                var province = uploadContent[i][4].ToString().Trim();
                var city = uploadContent[i][5].ToString().Trim();
                var provinceCode = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.FirstOrDefault(t => t.Code.Length == 2 && t.Name == province).Code;
                tempVehicle.CityCode = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 5 && x.Name == city && x.Code.Substring(0, 2) == provinceCode).Select(x => x.Code).FirstOrDefault();
                if (tempVehicle.CityCode == null)
                {
                    errorIndex = i;
                    errorCode.Append("F" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty"));
                    errorCode.Append(";");

                }

                tempVehicle.BrandModel = uploadContent[i][6].ToString().Trim();
                if (tempVehicle.BrandModel.Trim().Length > 100)
                {
                    errorIndex = i;
                    errorCode.Append("G" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                tempVehicle.StartYear = uploadContent[i][7].ToString().Trim();

                EnumModel vehicleSeviceType = VehicleSeviceTypeList.Where(x => x.ShowName == uploadContent[i][8].ToString().Trim()).FirstOrDefault();
                if (vehicleSeviceType == null)
                {
                    errorIndex = i;
                    errorCode.Append("H" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty"));
                    errorCode.Append(";");
                }
                else
                {
                    tempVehicle.ServiceType = (VehicleSeviceType)Enum.Parse(typeof(VehicleSeviceType), vehicleSeviceType.EnumName, true);
                }

                tempVehicle.Region = uploadContent[i][9].ToString().Trim();
                if (tempVehicle.Region.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode.Append("I" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                tempVehicle.OperationLicense = uploadContent[i][10].ToString().Trim();
                if (tempVehicle.OperationLicense.Trim().Length > 100)
                {
                    errorIndex = i;
                    errorCode.Append("J" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }


                tempVehicle.Owner = uploadContent[i][11].ToString().Trim();
                if (tempVehicle.Owner.Trim().Length > 100)
                {
                    errorIndex = i;
                    errorCode.Append("L" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                tempVehicle.Contact = uploadContent[i][12].ToString().Trim();
                if (tempVehicle.Contact.Trim().Length > 50)
                {
                    errorIndex = i;
                    errorCode.Append("M" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                tempVehicle.ContactPhone = uploadContent[i][13].ToString().Trim();
                if (tempVehicle.ContactPhone.Trim().Length > 20)
                {
                    errorIndex = i;
                    errorCode.Append("N" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }
                if (!ValidatePhone(uploadContent[i][13].ToString().Trim()))
                {
                    errorIndex = i;
                    errorCode.Append("N" + (i + 1).ToString() +
                                     ApplicationContext.Instance.StringResourceReader.GetString(
                                         "Phone_Illegal"));
                    errorCode.Append(";");
                }

                tempVehicle.ContactEmail = uploadContent[i][14].ToString().Trim();
                if (ValidateEmail(tempVehicle.ContactEmail))
                {
                    errorIndex = i;
                    errorCode.Append("O" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_EMail") + "\n" + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"));
                    errorCode.Append(";");
                }
                if (tempVehicle.ContactEmail.Trim().Length > 100)
                {
                    errorIndex = i;
                    errorCode.Append("O" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                tempVehicle.ContactAddress = uploadContent[i][15].ToString().Trim();
                if (tempVehicle.ContactAddress.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode.Append("P" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning"));
                    errorCode.Append(";");
                }

                tempVehicle.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;
                tempVehicle.OrgnizationId = this.ParentId;
                tempVehicleList.Add(tempVehicle);
            }

            return tempVehicleList;
        }

        void vehicleClient_CheckVehicleExistCompleted(object sender, CheckVehicleExistCompletedEventArgs e)
        {
            try
            {
                if (e.Result.TotalRecord == 0)
                {
                    VehicleServiceClient vehicleClient = InitVehicleServiceClient();
                    vehicleClient.BatchAddAsync(new ObservableCollection<Vehicle>(uploadList));
                }
                else
                {
                    var list = e.Result.Result.ToList();
                    foreach (var item in list)
                    {
                        errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_VehicleId"));
                        errorCode.Append(item.VehicleId);
                        errorCode.Append(" ");
                        errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("Tip_Or"));
                        errorCode.Append(" ");
                        errorCode.Append(ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_VehicleZceShelf"));
                        errorCode.Append(item.VehicleSn);
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

        void vehicleClient_BatchAddCompleted(object sender, BatchAddCompletedEventArgs e)
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
                VehicleTypeClient vehicleClient = sender as VehicleTypeClient;
                CloseVehicleService(vehicleClient);
            }
        }

        //private List<EnumModel> VehicleTypeList { get; set; }
        private List<EnumModel> VehicleSeviceTypeList { get; set; }

        //private void InitialVehicleType()
        //{
        //    VehicleTypeList = new List<EnumModel>();
        //    Enum.GetNames(typeof(VehicleType)).ToList().ForEach(x =>
        //    {
        //        EnumModel item = new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) };
        //        VehicleTypeList.Add(item);
        //    });
        //}

        private void InitialVehicleSeviceType()
        {
            VehicleSeviceTypeList = new List<EnumModel>();
            Enum.GetNames(typeof(VehicleSeviceType)).ToList().ForEach(x =>
            {
                EnumModel item = new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                VehicleSeviceTypeList.Add(item);
            });
        }
        #endregion
    }
}
