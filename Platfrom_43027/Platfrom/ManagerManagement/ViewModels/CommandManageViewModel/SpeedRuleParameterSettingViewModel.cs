using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.BasicPage.Views;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.Manager.Views.ComandManage;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
{
    [ExportAsViewModel(ManagerName.SpeedRuleParameterSettingVm)]
    public class SpeedRuleParameterSettingViewModel : ListViewModel<SpeedLimit>
    {
        #region MyRegion 超速规则列表 功能
     
        void client_GetSpeedLimitListCompleted(object sender, GetSpeedLimitListCompletedEventArgs e)
        {
            try
            {
                foreach (var item in e.Result.Result)
                {
                    item.CreateTime = item.CreateTime.ToLocalTime();
                }

                Data.loader_Finished(new PagedResult<SpeedLimit>()
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_GetSpeedLimitList", ex);
            }
        }

        void client_DeleteSpeedLimitByIDCompleted(object sender, DeleteSpeedLimitByIDCompletedEventArgs e)
        {
            if (e.Result.Result)
            {
                var res = MessageBoxHelper.ShowDialog(LProxy.Caption, LProxy.OperatedSuccessed, MessageDialogButton.Ok);
                res.Closed += (s, ex) =>
                {
                    Query();
                };
            }
        }

        private void client_GetSpeedLimitListByNameCompleted(object sender, GetSpeedLimitListByNameCompletedEventArgs e)
        {
            try
            {
                foreach (var item in e.Result.Result)
                {
                    item.CreateTime = item.CreateTime.ToLocalTime();
                }

                Data.loader_Finished(new PagedResult<SpeedLimit>()
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_GetSpeedLimitList", ex);
            }
        }

        /// <summary>
        /// 初始化内容
        /// </summary>
        public SpeedRuleParameterSettingViewModel()
            : base()
        {
            try
            {
                InitCommand();
               

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SpeedLimitMangeViewModel()", ex);
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        protected override void Update(string name)
        {
            SpeedRuleParameterSettingDetailWindow view = new SpeedRuleParameterSettingDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "view", CurrentSelected } });
            view.Show();
            view.Closed += (s, e) =>
            {
                Query();
            };
        }

        protected override void Add(string name)
        {
            SpeedRuleParameterSettingDetailWindow view = new SpeedRuleParameterSettingDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name } });
            view.Show();
            view.Closed += (s, e) =>
            {
                Query();
            };
        }

        protected override void Delete()
        {
            if (CurrentSelected != null)
            {
                var dialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                dialogResult.Closed += dialogResult_Closed;
            }
            //
        }

        private void dialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    CommandManageServiceClient client = InitialVehicleServiceClient();
                    client.DeleteSpeedLimitByIDAsync(CurrentSelected.ID);
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
                Data = new PagedServerCollection<SpeedLimit>((pageIndex, pageSize) =>
                {
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    pageSize = BaseCommon.PageSizeList[3];

                    CommandManageServiceClient client = InitialVehicleServiceClient();
                    client.GetSpeedLimitListByNameAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID,Name, pageIndex, pageSize);
                   
                });


                VehicleData = new PagedServerCollection<VehicleSpeed>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentVehicleIndex, pageIndex);

                    CommandManageServiceClient client = InitialVehicleServiceClient();
                    //查询数据
                    client.GetVehicleSpeedListBySpeedIDAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, CurrentSelected.ID, pageIndex, pageSize);
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        SpeedLimit currentSelected;
        public SpeedLimit CurrentSelected
        {
            get { return currentSelected; }
            set
            {
                if (currentSelected != value)
                {
                    currentSelected = value;
                    if (currentSelected != null)
                    {
                        VehicleData.MoveToFirstPage();
                    }
                }
                //currentSelected = value;
                RaisePropertyChanged(() => CurrentSelected);
            }
        }

     

        #endregion

        public ICommand BtnAddVechileCommand { get; set; }
        public ICommand BtnDeliverAllVechileCommand { get; set; }
        public ICommand BtnDeleteVechileCommand { get; set; }
        public ICommand BtnDeliverVechileCommand { get; set; }
        public ICommand BtnRefreshVechileCommand { get; set; }

        public void InitCommand()
        {
            BtnAddVechileCommand = new ActionCommand<object>(method => AddVechile("add"));
            BtnDeleteVechileCommand = new ActionCommand<object>(method => DeleteVechile());
            BtnDeliverAllVechileCommand = new ActionCommand<object>(method => DeliverAllVechile("update"));
            BtnDeliverVechileCommand = new ActionCommand<object>(method => DeliverVechile("update"));
            BtnRefreshVechileCommand = new ActionCommand<object>(method => RefreshVehicle());
        }

        private void RefreshVehicle()
        {
            VehicleData.RefreshPage();
        }

        private void DeliverVechile(string p)
        {
            if (CurVehicleSelected != null)
            {
                var deliverdialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("DownRule"), MessageDialogButton.OkAndCancel);
                deliverdialogResult.Closed += deliverdialogResult_Closed;
            }
        }


        void deliverdialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    ObservableCollection<VehicleSpeed> vehicles = new ObservableCollection<VehicleSpeed>();
                    vehicles.Add(CurVehicleSelected);
                    CommandManageServiceClient client = InitialVehicleServiceClient();
                    client.DeliverSpeedLimitToVehicleAsync(vehicles);
                }
            }
        }

        private void AddVechile(string p)
        {
            if (CurrentSelected != null)
            {
                CommandManageServiceClient client = InitialVehicleServiceClient();
                client.GetAllVehicleSpeedListBySpeedIDAsync(CurrentSelected.ClientID, CurrentSelected.ID);
            }
        }


        private void DeleteVechile()
        {
            if (CurVehicleSelected != null)
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
                    CommandManageServiceClient client = InitialVehicleServiceClient();
                    client.DeleteVehicleSpeedByIDAsync(CurVehicleSelected.ID);
                }
            }
        }

        private void CloseVehicleServiceClient(CommandManageServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        private void DeliverAllVechile(string p)
        {
            var sendalldialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("DownLoadAll"), MessageDialogButton.OkAndCancel);
            sendalldialogResult.Closed += sendalldialogResult_Closed;
        }

        void sendalldialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    try
                    {
                        ObservableCollection<VehicleSpeed> vehicles = new ObservableCollection<VehicleSpeed>();
                        foreach (VehicleSpeed item in VehicleData)
                        {
                            if (item.Status == (short)CommandStateEnum.UnDelivered)
                            {
                                vehicles.Add(item);
                            }
                        }
                        CommandManageServiceClient client = InitialVehicleServiceClient();
                        client.DeliverSpeedLimitToVehicleAsync(vehicles);
                    }
                    catch (Exception ex)
                    {
                        ApplicationContext.Instance.Logger.LogException("sendalldialogResult", ex);
                    }
                }
            }
        }

        private CommandManageServiceClient InitialVehicleServiceClient()
        {
            CommandManageServiceClient _client = ServiceClientFactory.Create<CommandManageServiceClient>();
            _client.GetSpeedLimitListByNameCompleted += client_GetSpeedLimitListByNameCompleted;
            _client.DeleteSpeedLimitByIDCompleted += client_DeleteSpeedLimitByIDCompleted;
            _client.GetVehicleSpeedListBySpeedIDCompleted += _client_GetVehicleSpeedListBySpeedIDCompleted;//获取规则中的车辆
            _client.DeleteVehicleSpeedByIDCompleted += _client_DeleteVehicleSpeedByIDCompleted;
            _client.GetAllVehicleSpeedListBySpeedIDCompleted += _client_GetAllVehicleSpeedListBySpeedIDCompleted;
            _client.InsertVehicleSpeedCompleted += _client_InsertVehicleSpeedCompleted;//添加车辆
            _client.DeliverSpeedLimitToVehicleCompleted += _client_DeliverSpeedLimitToVehicleCompleted;

            return _client;
        }

        void _client_DeliverSpeedLimitToVehicleCompleted(object sender, DeliverSpeedLimitToVehicleCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess == true)
                    {
                        VehicleData.RefreshPage();
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
                ApplicationContext.Instance.Logger.LogException("_client_DeliverSpeedLimitToVehicle", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseVehicleServiceClient(client);
            }
        }

        void _client_InsertVehicleSpeedCompleted(object sender, InsertVehicleSpeedCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess == true)
                    {
                        VehicleData.RefreshPage();
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
                ApplicationContext.Instance.Logger.LogException("_client_InsertHeartbeatVehicleCompleted()", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseVehicleServiceClient(client);
            }
        }

        void _client_GetAllVehicleSpeedListBySpeedIDCompleted(object sender, GetAllVehicleSpeedListBySpeedIDCompletedEventArgs e)
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
                 ApplicationContext.Instance.Logger.LogException("GetAllVehicleSpeedListBySpeedID", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseVehicleServiceClient(client);
            }
        }

        void _client_DeleteVehicleSpeedByIDCompleted(object sender, DeleteVehicleSpeedByIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.Result)
                    {
                        if (CurrentSelected != null)
                        {
                            VehicleData.MoveToFirstPage();
                        }
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
                ApplicationContext.Instance.Logger.LogException("_client_DeleteHeartbeatVehicleByIDCompleted()", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseVehicleServiceClient(client);
            }
        }

        int currentVehicleIndex = 1;
        void _client_GetVehicleSpeedListBySpeedIDCompleted(object sender, GetVehicleSpeedListBySpeedIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        foreach (VehicleSpeed ite in e.Result.Result)
                        {
                            int state = (int)ite.Status;
                            switch (state)
                            {

                                case 0:
                                    ite.ShowState = ApplicationContext.Instance.StringResourceReader.GetString("NoDown");
                                    break;
                                case 1:
                                    ite.ShowState = ApplicationContext.Instance.StringResourceReader.GetString("WaitDown");
                                    break;
                                case 2:
                                    ite.ShowState = ApplicationContext.Instance.StringResourceReader.GetString("Downing");
                                    ite.VehicleBtnEnable = false;
                                    break;
                                case 3:
                                    ite.ShowState = ApplicationContext.Instance.StringResourceReader.GetString("DownError");
                                    break;
                                case 4:
                                    ite.ShowState = ApplicationContext.Instance.StringResourceReader.GetString("DownSuccess");
                                    ite.VehicleBtnEnable = false;
                                    break;
                            }

                            ite.CreateTime = ite.CreateTime.Value.ToLocalTime();
                        }

                        VehicleData.loader_Finished(new PagedResult<VehicleSpeed>
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
                CloseVehicleServiceClient(client);
            }
        }


        void selectwindow_Closed(object sender, EventArgs e)
        {
            VehicleSelectWindow selectwindow = sender as VehicleSelectWindow;
            if (selectwindow.DialogResult == true)
            {

                ObservableCollection<VehicleSpeed> vehicles = new ObservableCollection<VehicleSpeed>();
                List<VehicleEx> list = selectwindow.SelectVehicleList;
                foreach (var item in list)
                {
                    VehicleSpeed hv = new VehicleSpeed();
                    hv.ID = Guid.NewGuid().ToString();
                    hv.VehicleID = item.VehicleId;
                    hv.SpeedID = CurrentSelected.ID;
                    hv.CreateTime = DateTime.Now.ToUniversalTime();
                    hv.Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                    hv.Status = (short)CommandStateEnum.UnDelivered;

                    vehicles.Add(hv);
                }
                CommandManageServiceClient client = InitialVehicleServiceClient();
                client.InsertVehicleSpeedAsync(vehicles);
            }
        }

        PagedServerCollection<VehicleSpeed> vehicleData;
        /// <summary>
        /// 列表数据源
        /// </summary>
        public PagedServerCollection<VehicleSpeed> VehicleData
        {
            get;
            set;
        }

        VehicleSpeed curVehicleSelected;

        public VehicleSpeed CurVehicleSelected
        {
            get { return curVehicleSelected; }
            set
            {
                curVehicleSelected = value;
                RaisePropertyChanged(() => CurVehicleSelected);
            }
        }


        private bool _vehicleBtnEnable = true;
        public bool VehicleBtnEnable
        {
            get { return _vehicleBtnEnable; }
            set
            {
                _vehicleBtnEnable = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleBtnEnable));
            }
        }
    }
}
