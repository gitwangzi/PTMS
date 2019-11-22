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
namespace Gsafety.PTMS.Manager.ViewModels
{
    [ExportAsViewModel(ManagerName.SetHeartBeatRuleVm)]
    public class HeartbeatRuleMangeViewModel : ListViewModel<HeartbeatRule>
    {
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

        public ICommand BtnAddVechileCommand { get; set; }
        public ICommand BtnDeliverVechileCommand { get; set; }
        public ICommand BtnDeliverAllVechileCommand { get; set; }
        public ICommand BtnDeleteVechileCommand { get; set; }
        public ICommand BtnRefreshVechileCommand { get; set; }
        /// <summary>
        /// 列表数据源
        /// </summary>
        public PagedServerCollection<HeartbeatVehicle> VehicleData
        {
            get;
            set;
        }

        private HeartbeatVehicle _selectedvehicle;

        public HeartbeatVehicle SelectedVehicle
        {
            get { return _selectedvehicle; }
            set { _selectedvehicle = value; }
        }
        /// <summary>
        /// 初始化内容
        /// </summary>
        public HeartbeatRuleMangeViewModel()
            : base()
        {
            try
            {
                BtnAddVechileCommand = new ActionCommand<object>(method => AddVechile("add"));
                BtnDeliverVechileCommand = new ActionCommand<object>(method => DeliverVechile("update"));
                BtnDeliverAllVechileCommand = new ActionCommand<object>(method => DeliverAllVechile("update"));
                BtnDeleteVechileCommand = new ActionCommand<object>(method => DeleteVechile());
                BtnRefreshVechileCommand = new ActionCommand<object>(method => RefreshVehicle());
                CanSelectVehicle = true;

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
                        ObservableCollection<HeartbeatVehicle> vehicles = new ObservableCollection<HeartbeatVehicle>();
                        foreach (HeartbeatVehicle item in VehicleData)
                        {
                            if (item.Status == (short)CommandStateEnum.UnDelivered)
                            {
                                vehicles.Add(item);
                            }
                        }
                        CommandManageServiceClient client = InitialHeartBeatClient();
                        client.DeliverHeartBeatRuleToVehicleAsync(vehicles);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
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
                    CommandManageServiceClient client = InitialHeartBeatClient();
                    client.DeleteHeartbeatVehicleByIDAsync(SelectedVehicle.ID);
                }
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

        void deliverdialogResult_Closed(object sender, EventArgs e)
        {
            try
            {
                SelfMessageBox dialog = sender as SelfMessageBox;
                if (dialog != null)
                {
                    if (dialog.DialogResult == true)
                    {
                        ObservableCollection<HeartbeatVehicle> vehicles = new ObservableCollection<HeartbeatVehicle>();
                        vehicles.Add(SelectedVehicle);
                        CommandManageServiceClient client = InitialHeartBeatClient();
                        client.DeliverHeartBeatRuleToVehicleAsync(vehicles);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void AddVechile(string p)
        {
            if (SelectedItem != null)
            {
                CommandManageServiceClient client = InitialHeartBeatClient();
                client.GetAllHeartbeatVehicleListByHeartBeatIDAsync(SelectedItem.ClientID, SelectedItem.ID);
            }
        }

        void selectwindow_Closed(object sender, EventArgs e)
        {
            try
            {
                VehicleSelectWindow selectwindow = sender as VehicleSelectWindow;
                if (selectwindow.DialogResult == true)
                {

                    ObservableCollection<HeartbeatVehicle> vehicles = new ObservableCollection<HeartbeatVehicle>();
                    List<VehicleEx> list = selectwindow.SelectVehicleList;
                    foreach (var item in list)
                    {
                        HeartbeatVehicle hv = new HeartbeatVehicle();
                        hv.ID = Guid.NewGuid().ToString();
                        hv.VehicleID = item.VehicleId;
                        hv.HeartbeatID = SelectedItem.ID;
                        hv.CreateTime = DateTime.Now.ToUniversalTime();
                        hv.Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        hv.Status = (short)CommandStateEnum.UnDelivered;

                        vehicles.Add(hv);
                    }
                    CommandManageServiceClient client = InitialHeartBeatClient();
                    client.InsertHeartbeatVehicleAsync(vehicles);

                }
               // Data.RefreshPage();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private CommandManageServiceClient InitialHeartBeatClient()
        {
            CommandManageServiceClient _client = ServiceClientFactory.Create<CommandManageServiceClient>();
            _client.GetHeartbeatRuleListCompleted += _client_GetHeartbeatRuleListCompleted;
            _client.DeleteHeartbeatRuleByIDCompleted += _client_DeleteHeartbeatRuleByIDCompleted;
            _client.GetHeartbeatVehicleListByHeartBeatIDCompleted += _client_GetHeartbeatVehicleListByHeartBeatIDCompleted;
            _client.DeleteHeartbeatVehicleByIDCompleted += _client_DeleteHeartbeatVehicleByIDCompleted;
            _client.DeliverHeartBeatRuleToVehicleCompleted += _client_DeliverHeartBeatRuleToVehicleCompleted;
            _client.GetAllHeartbeatVehicleListByHeartBeatIDCompleted += _client_GetAllHeartbeatVehicleListByHeartBeatIDCompleted;
            _client.InsertHeartbeatVehicleCompleted += _client_InsertHeartbeatVehicleCompleted;

            return _client;
        }

        void _client_InsertHeartbeatVehicleCompleted(object sender, InsertHeartbeatVehicleCompletedEventArgs e)
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
                       // Data.RefreshPage();
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

        void _client_GetAllHeartbeatVehicleListByHeartBeatIDCompleted(object sender, GetAllHeartbeatVehicleListByHeartBeatIDCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("_client_GetAllHeartbeatVehicleListByHeartBeatIDCompleted()", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseHeartBeatClient(client);
            }
        }

        void _client_DeliverHeartBeatRuleToVehicleCompleted(object sender, DeliverHeartBeatRuleToVehicleCompletedEventArgs e)
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
                        VehicleData.RefreshPage();
                    }
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("_client_DeliverHeartBeatRuleToVehicleCompleted()", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseHeartBeatClient(client);
            }
        }

        void _client_DeleteHeartbeatVehicleByIDCompleted(object sender, DeleteHeartbeatVehicleByIDCompletedEventArgs e)
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

        void _client_GetHeartbeatVehicleListByHeartBeatIDCompleted(object sender, GetHeartbeatVehicleListByHeartBeatIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {

                        foreach (HeartbeatVehicle ite in e.Result.Result)
                        {
                            switch (ite.Status)
                            {
                                case 0:
                                case 4:
                                    ite.VehicleBtnEnable = true;
                                    break;
                                case 1:
                                case 2:
                                case 3:
                                    ite.VehicleBtnEnable = false;
                                    break;
                            }
                            ite.CreateTime = ite.CreateTime.ToLocalTime();
                        }

                        VehicleData.loader_Finished(new PagedResult<HeartbeatVehicle>
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

        void _client_DeleteHeartbeatRuleByIDCompleted(object sender, DeleteHeartbeatRuleByIDCompletedEventArgs e)
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

        private void CloseHeartBeatClient(CommandManageServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        void _client_GetHeartbeatRuleListCompleted(object sender, GetHeartbeatRuleListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        int index = 0;
                        List<HeartbeatRule> rules = new List<HeartbeatRule>();
                        foreach (var item in e.Result.Result)
                        {
                            index++;
                            item.Index = index;
                            item.CreateTime = item.CreateTime.ToLocalTime();
                            rules.Add(item);
                        }
                        Data.loader_Finished(new PagedResult<HeartbeatRule>
                        {
                            Count = e.Result.TotalRecord,
                            Items = rules,
                            PageIndex = currentIndex
                        });

                        if (e.Result.Result.Count != 0)
                        {
                            SelectedItem = e.Result.Result[0];
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
                ApplicationContext.Instance.Logger.LogException("_client_GetHeartbeatRuleListCompleted()", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseHeartBeatClient(client);
            }
        }


        protected override void Add(string name)
        {
            HeartbeatRuleDetailWindow detailWindow = new HeartbeatRuleDetailWindow(string.Empty, new Dictionary<string, object>() { { "action", name } });
            detailWindow.Closed += detailWindow_Closed;
            detailWindow.Show();
        }

        void detailWindow_Closed(object sender, EventArgs e)
        {
            HeartbeatRuleDetailWindow window = sender as HeartbeatRuleDetailWindow;
            if (window != null)
            {
                if (window.DialogResult == true)
                {
                    Data.RefreshPage();
                }
            }
        }

        protected override void Update(string actionName)
        {
            if (SelectedItem != null)
            {
                HeartbeatRuleDetailWindow detailWindow = new HeartbeatRuleDetailWindow(string.Empty, new Dictionary<string, object>() { { "action", actionName }, { "model", SelectedItem } });
                detailWindow.Closed += detailWindow_Closed;
                detailWindow.Show();
            }
        }

        private HeartbeatRule selectedrule;

        public HeartbeatRule SelectedItem
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
                    CommandManageServiceClient client = InitialHeartBeatClient();
                    client.DeleteHeartbeatRuleByIDAsync(SelectedItem.ID);
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
                Data = new PagedServerCollection<HeartbeatRule>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    CommandManageServiceClient client = InitialHeartBeatClient();
                    //查询数据
                    client.GetHeartbeatRuleListAsync(pageIndex, pageSize, ApplicationContext.Instance.AuthenticationInfo.ClientID, Name);
                });

                VehicleData = new PagedServerCollection<HeartbeatVehicle>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentVehicleIndex, pageIndex);

                    CommandManageServiceClient client = InitialHeartBeatClient();
                    //查询数据
                    client.GetHeartbeatVehicleListByHeartBeatIDAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SelectedItem.ID, pageIndex, pageSize);
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagination()", ex);
            }
        }

        int currentVehicleIndex = 0;

        public string Name
        {
            get;
            set;
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

