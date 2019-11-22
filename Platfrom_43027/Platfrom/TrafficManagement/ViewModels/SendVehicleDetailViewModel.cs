using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.BasicPage.Views;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;

namespace Gsafety.PTMS.Traffic.ViewModels
{
    public class SendVehicleDetailViewModel : ListViewModel<FenceQueue>
    {
        TrafficFence _selectedfence;

        string vehiclename = string.Empty;

        public string VehicleName
        {
            get { return vehiclename; }
            set
            {
                vehiclename = value;
            }
        }

        private FenceQueue _selectedvehicle;

        public FenceQueue SelectedVehicle
        {
            get { return _selectedvehicle; }
            set { _selectedvehicle = value; }
        }

        public ICommand BtnDeliverVechileCommand { get; set; }
        public ICommand BtnDeliverAllVechileCommand { get; set; }
        public ICommand BtnRefreshVechileCommand { get; set; }

        public new void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                _selectedfence = viewParameters["model"] as TrafficFence;

                base.ActivateView(viewName, viewParameters);
            }
            catch (Exception)
            {

            }
        }

        private TrafficManageServiceClient InitialClient()
        {
            TrafficManageServiceClient client = ServiceClientFactory.Create<TrafficManageServiceClient>();
            client.GetFenceQueueListByFenceIDCompleted += client_GetFenceQueueListByFenceIDCompleted;
            client.InsertFenceQueueCompleted += client_InsertFenceQueueCompleted;
            client.DeleteFenceQueueByIDCompleted += client_DeleteFenceQueueByIDCompleted;
            client.GetAllFenceQueueListByFenceIDCompleted += client_GetAllFenceQueueListByFenceIDCompleted;
            client.DeliverFenceQueueToVehicleCompleted += client_DeliverFenceQueueToVehicleCompleted;

            return client;
        }

        void client_DeliverFenceQueueToVehicleCompleted(object sender, DeliverFenceQueueToVehicleCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.Result)
                    {
                        Data.MoveToFirstPage();
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
                TrafficManageServiceClient client = sender as TrafficManageServiceClient;
                CloseClient(client);
            }
        }

        void client_GetAllFenceQueueListByFenceIDCompleted(object sender, GetAllFenceQueueListByFenceIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
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
                ApplicationContext.Instance.Logger.LogException("_client_DeliverHeartBeatRuleToVehicleCompleted()", ex);
            }
            finally
            {
                TrafficManageServiceClient client = sender as TrafficManageServiceClient;
                CloseClient(client);
            }

        }

        private void CloseClient(TrafficManageServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        private void selectwindow_Closed(object sender, EventArgs e)
        {
            VehicleSelectWindow selectwindow = sender as VehicleSelectWindow;
            if (selectwindow.DialogResult == true)
            {
                ObservableCollection<FenceQueue> vehicles = new ObservableCollection<FenceQueue>();
                List<VehicleEx> list = selectwindow.SelectVehicleList;
                foreach (var item in list)
                {
                    FenceQueue fq = new FenceQueue();
                    fq.ID = Guid.NewGuid().ToString();
                    fq.VehicleID = item.VehicleId;
                    fq.FenceID = _selectedfence.ID;
                    fq.CreateTime = DateTime.Now.ToUniversalTime();
                    fq.CircleCenter = _selectedfence.CircleCenter;
                    fq.ClientID = _selectedfence.ClientID;
                    fq.FenceType = (short)_selectedfence.FenceType;
                    fq.Name = _selectedfence.Name;
                    fq.OperType = (short)OperTypeEnum.Add;
                    fq.Pts = _selectedfence.Pts;
                    fq.Radius = _selectedfence.Radius;
                    fq.MaxSpeed = _selectedfence.MaxSpeed;
                    fq.OverSpeedDuration = _selectedfence.OverSpeedDuration;
                    fq.PointCount = _selectedfence.PointCount;
                    fq.RegionProperty = _selectedfence.RegionProperty;
                    fq.StartTime = _selectedfence.StartTime;
                    fq.EndTime = _selectedfence.EndTime;
                    fq.Status = (short)CommandStateEnum.UnDelivered;

                    vehicles.Add(fq);
                }
                TrafficManageServiceClient client = InitialClient();
                client.InsertFenceQueueAsync(vehicles);

            }
        }

        void client_DeleteFenceQueueByIDCompleted(object sender, DeleteFenceQueueByIDCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("_client_DeleteHeartbeatVehicleByIDCompleted()", ex);
            }
            finally
            {
                TrafficManageServiceClient client = sender as TrafficManageServiceClient;
                CloseClient(client);
            }
        }

        void client_InsertFenceQueueCompleted(object sender, InsertFenceQueueCompletedEventArgs e)
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
                TrafficManageServiceClient client = sender as TrafficManageServiceClient;
                CloseClient(client);
            }
        }

        void client_GetFenceQueueListByFenceIDCompleted(object sender, GetFenceQueueListByFenceIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        Data.loader_Finished(new PagedResult<FenceQueue>
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
                TrafficManageServiceClient client = sender as TrafficManageServiceClient;
                CloseClient(client);
            }
        }

        public SendVehicleDetailViewModel()
            : base()
        {
            try
            {
                PageSizeValue = 10;
                BtnDeliverVechileCommand = new ActionCommand<object>(method => DeliverVechile("update"));
                BtnDeliverAllVechileCommand = new ActionCommand<object>(method => DeliverAllVechile("update"));
                BtnRefreshVechileCommand = new ActionCommand<object>(method => RefreshVehicle());
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SendVehicleDetailViewModel()", ex);
            }
        }

        private void RefreshVehicle()
        {
            Data.RefreshPage();
        }

        private void DeliverAllVechile(string p)
        {
            if (_selectedfence != null)
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

        private void sendalldialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    ObservableCollection<FenceQueue> vehicles = new ObservableCollection<FenceQueue>();
                    foreach (FenceQueue item in Data)
                    {
                        if (item.Status == (short)CommandStateEnum.UnDelivered)
                        {
                            vehicles.Add(item);
                        }
                    }
                    TrafficManageServiceClient client = InitialClient();
                    client.DeliverFenceQueueToVehicleAsync(vehicles);
                }
            }
        }


        private void deliverdialogResult_Closed(object sender, EventArgs e)
        {
            if (SelectedVehicle != null)
            {
                SelfMessageBox dialog = sender as SelfMessageBox;
                if (dialog != null)
                {
                    if (dialog.DialogResult == true)
                    {
                        ObservableCollection<FenceQueue> vehicles = new ObservableCollection<FenceQueue>();
                        vehicles.Add(SelectedVehicle);
                        TrafficManageServiceClient client = InitialClient();
                        client.DeliverFenceQueueToVehicleAsync(vehicles);
                    }
                }
            }
        }

        private void DeliverVechile(string p)
        {
            var senddialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                ApplicationContext.Instance.StringResourceReader.GetString("DownRule"), MessageDialogButton.OkAndCancel);
            senddialogResult.Closed += deliverdialogResult_Closed;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="name"></param>
        protected override void Add(string name)
        {
            if (_selectedfence.Valid == false)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_FenceIsAbandon"));
                return;
            }
            else
            {
                TrafficManageServiceClient client = InitialClient();
                client.GetAllFenceQueueListByFenceIDAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, _selectedfence.ID);
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
                Data = new PagedServerCollection<FenceQueue>((pageIndex, pageSize) =>
                {
                    if (_selectedfence != null)
                    {
                        pageSize = PageSizeValue;
                        System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                        TrafficManageServiceClient client = InitialClient();
                        string id = _selectedfence.ID;
                        //查询数据
                        client.GetFenceQueueListByFenceIDAsync(id, ApplicationContext.Instance.AuthenticationInfo.ClientID, VehicleName, pageIndex, pageSize);
                    }
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        protected override void Delete()
        {
            if (SelectedVehicle != null)
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
                    TrafficManageServiceClient client = InitialClient();
                    client.DeleteFenceQueueByIDAsync(SelectedVehicle.ID);
                }
            }
        }
    }
}
