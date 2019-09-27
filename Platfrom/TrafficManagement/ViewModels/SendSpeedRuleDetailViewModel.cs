using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.BasicPage.Views;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.Share;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Traffic.ViewModels
{
    public class SendSpeedRuleDetailViewModel : ListViewModel<VehicleSpeed>
    {
        public SendSpeedRuleDetailViewModel()
            : base()
        {
            try
            {
                BtnDeleteVechileCommand = new ActionCommand<object>(method => DeleteVechile());
                BtnDeliverVechileCommand = new ActionCommand<object>(method => DeliverVechile("update"));
                BtnDeliverAllVechileCommand = new ActionCommand<object>(method => DeliverAllVechile("update"));
                BtnRefreshVechileCommand = new ActionCommand<object>(method => RefreshVehicle());
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SendVehicleDetailViewModel()", ex);
            }
        }

        private string _vehicleName = string.Empty;
        public string VehicleName
        {
            get { return _vehicleName; }
            set
            {
                _vehicleName = value;
                RaisePropertyChanged(() => VehicleName);
            }
        }

        private VehicleSpeed _vehicleSpeed = new VehicleSpeed();
        public VehicleSpeed SelectedVehicleSpeed
        {
            get { return _vehicleSpeed; }
            set { _vehicleSpeed = value; }
        }

        SpeedLimit _selectSpeedLimit;
        public SpeedLimit SelectedSpeedLimit
        {
            get { return _selectSpeedLimit; }
            set { _selectSpeedLimit = value; }
        }

        public new void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                SelectedSpeedLimit = viewParameters["model"] as SpeedLimit;
                base.ActivateView(viewName, viewParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICommand BtnDeleteVechileCommand { get; set; }
        public ICommand BtnDeliverVechileCommand { get; set; }
        public ICommand BtnDeliverAllVechileCommand { get; set; }
        public ICommand BtnRefreshVechileCommand { get; set; }

        private CommandManageServiceClient InitialCommandManageServiceClient()
        {
            CommandManageServiceClient client = ServiceClientFactory.Create<CommandManageServiceClient>();
            client.GetVehicleSpeedListBySpeedIDCompleted += _client_GetVehicleSpeedListBySpeedIDCompleted;//获取规则中的车辆
            client.DeleteVehicleSpeedByIDCompleted += _client_DeleteVehicleSpeedByIDCompleted;
            client.GetAllVehicleSpeedListBySpeedIDCompleted += _client_GetAllVehicleSpeedListBySpeedIDCompleted;
            client.InsertVehicleSpeedCompleted += _client_InsertVehicleSpeedCompleted;//添加车辆
            client.DeliverSpeedLimitToVehicleCompleted += _client_DeliverSpeedLimitToVehicleCompleted;

            return client;
        }

        private void CloseCommandManageService(CommandManageServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        void _client_GetAllVehicleSpeedListBySpeedIDCompleted(object sender, GetAllVehicleSpeedListBySpeedIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess == true)
                    {
                        Func<Gsafety.PTMS.Bases.Models.Vehicle, bool> filter = t => t.MDVROnline.HasValue;
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
                ApplicationContext.Instance.Logger.LogException("_client_GetAllVehicleSpeedListBySpeedIDCompleted", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseCommandManageService(client);
            }
        }

        void _client_DeleteVehicleSpeedByIDCompleted(object sender, DeleteVehicleSpeedByIDCompletedEventArgs e)
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
                        if (e.Result.Result)
                        {
                            Data.MoveToFirstPage();
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
                CloseCommandManageService(client);
            }
        }

        void _client_GetVehicleSpeedListBySpeedIDCompleted(object sender, GetVehicleSpeedListBySpeedIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        foreach (VehicleSpeed ite in e.Result.Result)
                        {
                            int state = (int)ite.Status;
                            if (state == 2 || state == 3)
                            {
                                ite.VehicleBtnEnable = false;
                            }

                            ite.CreateTime = ite.CreateTime.Value.ToLocalTime();
                        }

                        Data.loader_Finished(new PagedResult<VehicleSpeed>
                        {
                            Count = e.Result.TotalRecord,
                            Items = e.Result.Result,
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
                ApplicationContext.Instance.Logger.LogException("_client_GetHeartbeatVehicleListByHeartBeatIDCompleted()", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseCommandManageService(client);
            }
        }

        void _client_InsertVehicleSpeedCompleted(object sender, InsertVehicleSpeedCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("_client_InsertHeartbeatVehicleCompleted()", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseCommandManageService(client);
            }
        }

        void _client_DeliverSpeedLimitToVehicleCompleted(object sender, DeliverSpeedLimitToVehicleCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("_client_DeliverSpeedLimitToVehicle", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseCommandManageService(client);
            }
        }

        private void DeleteVechile()
        {
            if (SelectedVehicleSpeed != null)
            {
                var dialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                dialogResult.Closed += dialogResult_Closed;
            }
        }

        private void DeliverVechile(string p)
        {
            var senddialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                 ApplicationContext.Instance.StringResourceReader.GetString("DownRule"), MessageDialogButton.OkAndCancel);
            senddialogResult.Closed += deliverdialogResult_Closed;
        }

        private void DeliverAllVechile(string p)
        {
            if (SelectedSpeedLimit != null)
            {
                if (Data.ItemCount != 0)
                {
                    var deliverdialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("DownLoadAll"), MessageDialogButton.OkAndCancel);
                    deliverdialogResult.Closed += sendalldialogResult_Closed;
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("SelectCar"), MessageDialogButton.Ok);
                }
            }
        }

        private void RefreshVehicle()
        {
            Data.RefreshPage();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="name"></param>
        protected override void Add(string name)
        {
            CommandManageServiceClient client = InitialCommandManageServiceClient();
            client.GetAllVehicleSpeedListBySpeedIDAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SelectedSpeedLimit.ID);
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
                Data = new PagedServerCollection<VehicleSpeed>((pageIndex, pageSize) =>
                {
                    if (SelectedSpeedLimit != null)
                    {
                        pageSize = PageSizeValue;
                        System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                        CommandManageServiceClient client = InitialCommandManageServiceClient();
                        //查询数据
                        client.GetVehicleSpeedListBySpeedIDAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SelectedSpeedLimit.ID, VehicleName, pageIndex, pageSize);
                    }
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        private void sendalldialogResult_Closed(object sender, EventArgs e)
        {
            try
            {
                SelfMessageBox dialog = sender as SelfMessageBox;
                if (dialog != null)
                {
                    if (dialog.DialogResult == true)
                    {
                        ObservableCollection<VehicleSpeed> vehicles = new ObservableCollection<VehicleSpeed>();
                        foreach (VehicleSpeed item in Data)
                        {
                            if (item.Status == (short)CommandStateEnum.UnDelivered)
                            {
                                vehicles.Add(item);
                            }
                        }

                        if (vehicles.Count != 0)
                        {
                            CommandManageServiceClient client = InitialCommandManageServiceClient();
                            client.DeliverSpeedLimitToVehicleAsync(vehicles);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void deliverdialogResult_Closed(object sender, EventArgs e)
        {
            if (SelectedVehicleSpeed != null)
            {
                SelfMessageBox dialog = sender as SelfMessageBox;
                if (dialog != null)
                {
                    if (dialog.DialogResult == true)
                    {
                        ObservableCollection<VehicleSpeed> vehicles = new ObservableCollection<VehicleSpeed>();
                        vehicles.Add(SelectedVehicleSpeed);
                        CommandManageServiceClient client = InitialCommandManageServiceClient();
                        client.DeliverSpeedLimitToVehicleAsync(vehicles);
                    }
                }
            }
        }

        private void dialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    CommandManageServiceClient client = InitialCommandManageServiceClient();
                    client.DeleteVehicleSpeedByIDAsync(SelectedVehicleSpeed.ID);
                }
            }
        }

        void selectwindow_Closed(object sender, EventArgs e)
        {
            try
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
                        hv.SpeedID = SelectedSpeedLimit.ID;
                        hv.CreateTime = DateTime.Now.ToUniversalTime();
                        hv.Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        hv.Status = (short)CommandStateEnum.UnDelivered;

                        vehicles.Add(hv);
                    }
                    CommandManageServiceClient client = InitialCommandManageServiceClient();
                    client.InsertVehicleSpeedAsync(vehicles);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
