using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.BasicPage.Views;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.Manager.Views.ComandManage;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;

namespace Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
{
    [ExportAsViewModel(ManagerName.GPSConfigureVm)]
    public class GPSConfigureViewModel : ListViewModel<LocationReportRule>
    {
        public ICommand BtnAddVechileCommand { get; set; }
        public ICommand BtnDeliverVechileCommand { get; set; }
        public ICommand BtnDeliverAllVechileCommand { get; set; }
        public ICommand BtnDeleteVechileCommand { get; set; }
        public ICommand BtnRefreshVechileCommand { get; set; }

        /// <summary>
        /// 列表数据源
        /// </summary>
        public PagedServerCollection<LocationReportVehicle> VehicleData
        {
            get;
            set;
        }

        private LocationReportVehicle _selectedvehicle;

        public LocationReportVehicle SelectedVehicle
        {
            get { return _selectedvehicle; }
            set { _selectedvehicle = value; }
        }

        int currentVehicleIndex = 0;

        public string Name
        {
            get;
            set;
        }

        #region button...


        bool _canselectvehicle = false;
        public bool CanSelectVehicle
        {
            get
            {
                return _canselectvehicle;
            }
            set
            {
                _canselectvehicle = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CanSelectVehicle));
            }
        }

        private LocationReportRule selectedrule;

        public LocationReportRule SelectedItem
        {
            get { return selectedrule; }
            set
            {
                if (selectedrule != value)
                {
                    selectedrule = value;
                    if (selectedrule != null)
                    {
                        VehicleData.MoveToFirstPage();
                    }
                }

                RaisePropertyChanged(() => this.SelectedItem);
            }
        }

        #endregion

        /// <summary>
        /// 初始化内容
        /// </summary>
        public GPSConfigureViewModel()
            : base()
        {
            try
            {
                BtnAddVechileCommand = new ActionCommand<object>(method => AddVechile("add"));
                BtnDeliverVechileCommand = new ActionCommand<object>(method => DeliverVechile("update"));
                BtnDeleteVechileCommand = new ActionCommand<object>(method => DeleteVechile());
                BtnDeliverAllVechileCommand = new ActionCommand<object>(method => DeliverAllVechile("update"));
                BtnRefreshVechileCommand = new ActionCommand<object>(method => RefreshVehicle());

                InitialLanguage();
                InitialGPSConfigureClient();
                InitialStatus();
                PageSizeValue = 10;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("HeartbeatRuleMangeViewModel()", ex);
            }
        }

        private void RefreshVehicle()
        {
            VehicleData.RefreshPage();
        }

        private CommandManageServiceClient InitialGPSConfigureClient()
        {
            CommandManageServiceClient _client = ServiceClientFactory.Create<CommandManageServiceClient>();
            _client.GetByNameLocationReportRuleListCompleted += _client_GetByNameLocationReportRuleListCompleted;
            _client.DeleteLocationReportRuleByIDCompleted += _client_DeleteLocationReportRuleByIDCompleted;
            _client.GetLocationReportVehicleListByLocationReportIDCompleted += _client_GetLocationReportVehicleListByLocationReportIDCompleted;
            _client.DeliverLocationReportRuleToVehicleCompleted += _client_DeliverLocatipReportRuleToVehicleCompleted;
            _client.GetAllLocationReportVehicleListByLocationReportIDCompleted += _client_GetAllLocationReportVehicleListByLocationReportIDCompleted;
            _client.InsertLocationReportVehicleCompleted += _client_InsertLocationReportVehicleCompleted;
            _client.DeleteLocationReportVehicleByIDCompleted += _client_DeleteLocationReportVehicleByIDCompleted;
            return _client;
        }

        void _client_DeleteLocationReportVehicleByIDCompleted(object sender, DeleteLocationReportVehicleByIDCompletedEventArgs e)
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
                        if (SelectedItem != null)
                        {
                            VehicleData.RefreshPage();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("_client_DeleteHeartbeatVehicleByIDCompleted()", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseHeartBeatClient(client);
            }
        }

        void _client_InsertLocationReportVehicleCompleted(object sender, InsertLocationReportVehicleCompletedEventArgs e)
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
                        //Data.RefreshPage();
                        VehicleData.RefreshPage();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("_client_InsertHeartbeatVehicleCompleted()", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseHeartBeatClient(client);
            }
        }

        void _client_GetAllLocationReportVehicleListByLocationReportIDCompleted(object sender, GetAllLocationReportVehicleListByLocationReportIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        Func<Vehicle, bool> filter = t => t.MDVROnline.HasValue;
                        VehicleSelectWindow selectwindow = new VehicleSelectWindow(filter);
                        foreach (var v in e.Result.Result)
                        {
                            foreach (var item in selectwindow.VehicleList)
                            {
                                if (item.VehicleId == v.VehicleID)
                                {
                                    item.IsChecked = true;
                                    break;
                                }
                            }
                        }

                        selectwindow.Closed += selectwindow_Closed;
                        selectwindow.Show();
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
                ApplicationContext.Instance.Logger.LogException("GPSConfigureViewModel", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseHeartBeatClient(client);
            }
        }

        private void selectwindow_Closed(object sender, EventArgs e)
        {
            VehicleSelectWindow selectwindow = sender as VehicleSelectWindow;
            if (selectwindow.DialogResult == true)
            {
                ObservableCollection<LocationReportVehicle> vehicles = new ObservableCollection<LocationReportVehicle>();
                List<VehicleEx> list = selectwindow.SelectVehicleList;
                foreach (var item in list)
                {
                    LocationReportVehicle hv = new LocationReportVehicle();
                    hv.ID = Guid.NewGuid().ToString();
                    hv.VehicleID = item.VehicleId;
                    hv.LocationReportID = SelectedItem.ID;
                    hv.CreateTime = DateTime.Now.ToUniversalTime();
                    hv.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;
                    hv.Status = (short)CommandStateEnum.UnDelivered;

                    vehicles.Add(hv);
                }
                if (vehicles.Count > 0)
                {
                    CommandManageServiceClient client = InitialGPSConfigureClient();
                    client.InsertLocationReportVehicleAsync(vehicles);
                }
            }

        }

        void _client_DeliverLocatipReportRuleToVehicleCompleted(object sender, DeliverLocationReportRuleToVehicleCompletedEventArgs e)
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
                        if (SelectedItem != null)
                        {
                            VehicleData.RefreshPage();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("_client_InsertHeartbeatVehicleCompleted()", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseHeartBeatClient(client);
            }
        }

        void _client_GetLocationReportVehicleListByLocationReportIDCompleted(object sender, GetLocationReportVehicleListByLocationReportIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        foreach (LocationReportVehicle ite in e.Result.Result)
                        {
                            int state = (int)ite.Status;
                            if (state == 2 || state == 3)
                            {
                                ite.IsTrue = false;
                            }

                            ite.CreateTime = ite.CreateTime.ToLocalTime();
                        }

                        VehicleData.loader_Finished(new PagedResult<LocationReportVehicle>
                        {
                            Count = e.Result.TotalRecord,
                            Items = e.Result.Result,
                            PageIndex = currentVehicleIndex
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
                ApplicationContext.Instance.Logger.LogException("_client_GetHeartbeatVehicleListByHeartBeatIDCompleted()", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseHeartBeatClient(client);
            }
        }

        void _client_DeleteLocationReportRuleByIDCompleted(object sender, DeleteLocationReportRuleByIDCompletedEventArgs e)
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
                        SelectedItem = null;
                        Data.RefreshPage();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("_client_DeleteHeartbeatRuleByIDCompleted", ex);

            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseHeartBeatClient(client);
            }
        }

        private void AddVechile(string p)
        {
            if (SelectedItem != null)
            {
                CommandManageServiceClient client = InitialGPSConfigureClient();
                client.GetAllLocationReportVehicleListByLocationReportIDAsync(SelectedItem.ClientID, SelectedItem.ID);
            }
        }

        private void DeliverVechile(string p)
        {
            if (SelectedVehicle != null)
            {
                var deliverdialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("DownRule"), MessageDialogButton.OkAndCancel);
                deliverdialogResult.Closed += deliverdialogResult_Closed;
            }
        }

        private void deliverdialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    ObservableCollection<LocationReportVehicle> vehicles = new ObservableCollection<LocationReportVehicle>();
                    vehicles.Add(SelectedVehicle);
                    CommandManageServiceClient client = InitialGPSConfigureClient();
                    client.DeliverLocationReportRuleToVehicleAsync(vehicles);
                }
            }
        }

        private void DeleteVechile()
        {
            if (SelectedVehicle != null)
            {
                var dialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                dialogResult.Closed += dialogResult_Closed2;
            }
        }

        private void dialogResult_Closed2(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    CommandManageServiceClient client = InitialGPSConfigureClient();
                    client.DeleteLocationReportVehicleByIDAsync(SelectedVehicle.ID);
                }
            }
        }

        private void DeliverAllVechile(string p)
        {
            if (VehicleData.ItemCount != 0)
            {
                var sendalldialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("DownLoadAll"), MessageDialogButton.OkAndCancel);
                sendalldialogResult.Closed += sendalldialogResult_Closed;
            }
            else
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("SelectCar"), MessageDialogButton.Ok);
            }
        }

        void sendalldialogResult_Closed(object sender, EventArgs e)
        {
            try
            {
                SelfMessageBox dialog = sender as SelfMessageBox;
                if (dialog != null)
                {
                    if (dialog.DialogResult == true)
                    {
                        ObservableCollection<LocationReportVehicle> vehicles = new ObservableCollection<LocationReportVehicle>();
                        foreach (LocationReportVehicle item in VehicleData)
                        {
                            if (item.Status == (short)CommandStateEnum.UnDelivered)
                            {
                                vehicles.Add(item);
                            }
                        }
                        CommandManageServiceClient client = InitialGPSConfigureClient();
                        client.DeliverLocationReportRuleToVehicleAsync(vehicles);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void Delete()
        {
            if (SelectedItem != null)
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
                    CommandManageServiceClient client = InitialGPSConfigureClient();
                    client.DeleteLocationReportRuleByIDAsync(SelectedItem.ID);
                }
            }
        }

        #region completed....

        private void CloseHeartBeatClient(CommandManageServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        private void _client_GetByNameLocationReportRuleListCompleted(object sender, GetByNameLocationReportRuleListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        int index = 0;
                        List<LocationReportRule> rules = new List<LocationReportRule>();
                        foreach (var item in e.Result.Result)
                        {
                            index++;
                            item.Index = index;
                            item.CreateTime = item.CreateTime.ToLocalTime();
                            rules.Add(item);
                        }

                        Data.loader_Finished(new BaseLib.Model.PagedResult<LocationReportRule>()
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
                ApplicationContext.Instance.Logger.LogException("GPSConfigViewModel", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseHeartBeatClient(client);
            }
        }

        #endregion

        #region method....


        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        protected override void Update(string name)
        {
            if (SelectedItem != null)
            {
                LocationReportRuleDetailWindow detailWindow = new LocationReportRuleDetailWindow(string.Empty, new Dictionary<string, object>() { { "action", name }, { "model", SelectedItem } });
                detailWindow.Closed += detailWindow_Closed;
                detailWindow.Show();
            }
        }
        protected override void Add(string name)
        {
            LocationReportRuleDetailWindow detailWindow = new LocationReportRuleDetailWindow(string.Empty, new Dictionary<string, object>() { { "action", name } });
            detailWindow.Closed += detailWindow_Closed;
            detailWindow.Show();
        }

        private void detailWindow_Closed(object sender, EventArgs e)
        {
            LocationReportRuleDetailWindow window = sender as LocationReportRuleDetailWindow;
            if (window != null)
            {
                if (window.DialogResult == true)
                {
                    Data.RefreshPage();
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
                Data = new PagedServerCollection<LocationReportRule>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    Gsafety.PTMS.ServiceReference.CommandManageService.PagingInfo page = new Gsafety.PTMS.ServiceReference.CommandManageService.PagingInfo();
                    page.PageIndex = pageIndex;
                    page.PageSize = PageSizeValue;
                    CommandManageServiceClient client = InitialGPSConfigureClient();
                    //查询数据
                    client.GetByNameLocationReportRuleListAsync(page, ApplicationContext.Instance.AuthenticationInfo.ClientID, Name);
                });

                VehicleData = new PagedServerCollection<LocationReportVehicle>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentVehicleIndex, pageIndex);

                    CommandManageServiceClient client = InitialGPSConfigureClient();
                    //查询数据
                    client.GetLocationReportVehicleListByLocationReportIDAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SelectedItem.ID, pageIndex, pageSize);
                });


            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        protected void InitialStatus()
        {
            SouLRR = new List<EnumModel>();
            SouLRR.Add(new EnumModel() { EnumValue = -1, EnumName = "select", ShowName = PleaseSelect });
            SouLRR.Add(new EnumModel() { EnumValue = 0, EnumName = "time", ShowName = GIS_interval });
            SouLRR.Add(new EnumModel() { EnumValue = 1, EnumName = "length", ShowName = GIS_LengthTip });
            SouLRR.Add(new EnumModel() { EnumValue = 2, EnumName = "timelength", ShowName = TimeAndLength });
        }

        protected void InitialLanguage()
        {
            PleaseSelect = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect");
            GIS_interval = ApplicationContext.Instance.StringResourceReader.GetString("ReportStrategyEnum_ByInterval");
            GIS_LengthTip = ApplicationContext.Instance.StringResourceReader.GetString("ReportStrategyEnum_ByLength");
            TimeAndLength = ApplicationContext.Instance.StringResourceReader.GetString("ReportStrategyEnum_ByIntervalAndLength");

        }


        #endregion



        private bool _vehicleBtnEnable;
        public bool VehicleBtnEnable
        {
            get { return _vehicleBtnEnable; }
            set
            {
                _vehicleBtnEnable = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleBtnEnable));
            }
        }

        private string _pleaseSelect;
        public string PleaseSelect
        {
            get { return _pleaseSelect; }
            set
            {
                _pleaseSelect = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PleaseSelect));
            }
        }

        private string _gIS_interval;
        public string GIS_interval
        {
            get { return _gIS_interval; }
            set
            {
                _gIS_interval = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => GIS_interval));
            }
        }

        #region headListlanguage....

        private string _gIS_LengthTip;
        public string GIS_LengthTip
        {
            get { return _gIS_LengthTip; }
            set
            {
                _gIS_LengthTip = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => GIS_LengthTip));
            }
        }


        private string _timeAndLength;
        public string TimeAndLength
        {
            get { return _timeAndLength; }
            set
            {
                _timeAndLength = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => TimeAndLength));
            }
        }



        #endregion

        #region util....
        private List<EnumModel> _souLRR;
        public List<EnumModel> SouLRR
        {
            get { return _souLRR; }
            set
            {
                _souLRR = value;
                RaisePropertyChanged(() => SouLRR);

            }
        }

        private bool _isReadOnlyInterval;
        public bool IsReadOnlyInterval
        {
            get { return _isReadOnlyInterval; }
            set
            {
                _isReadOnlyInterval = value;
                RaisePropertyChanged(() => IsReadOnlyInterval);
            }
        }

        private bool _isReadOnlyLength;
        public bool IsReadOnlyLength
        {
            get { return _isReadOnlyLength; }
            set
            {
                _isReadOnlyLength = value;
                RaisePropertyChanged(() => IsReadOnlyLength);
            }
        }

        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                RaisePropertyChanged(() => Index);
            }
        }
        #endregion

        #region property.....
        private int _reportStrategy;
        public int ReportStrategy
        {
            get { return _reportStrategy; }
            set
            {
                _reportStrategy = value == 0 ? 0 : value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ReportStrategy));
                if (ReportStrategy == 0)
                {
                    IsReadOnlyInterval = false;
                    IsReadOnlyLength = true;
                }
                else if (ReportStrategy == 1)
                {
                    IsReadOnlyInterval = true;
                    IsReadOnlyLength = false;
                }
                else
                {
                    IsReadOnlyInterval = false;
                    IsReadOnlyLength = false;
                }
            }
        }

        private string _interval;
        public string Interval
        {
            get { return _interval; }
            set
            {
                _interval = value == null ? null : value.Trim();
                ValidateInterval(ExtractPropertyName(() => Interval), _interval);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Interval));
            }
        }
        private void ValidateInterval(string prop, string value)
        {
            ClearErrors(prop);
            if (value.Length > 12)
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
            }
            else
            {
                long result = 0;

                if (!long.TryParse(value, out result))
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
                }
            }
        }

        private string _length;
        public string Length
        {
            get { return _length; }
            set
            {
                _length = value == null ? null : value;
                ValidateInterval(ExtractPropertyName(() => Length), _length);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Length));
            }
        }

        protected override void ValidateAll()
        {
            // ValidateName(ExtractPropertyName(() => Name), _name);
            ValidateInterval(ExtractPropertyName(() => Interval), _interval);
        }

        #endregion

    }
}
