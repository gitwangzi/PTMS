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
    public class SendRouteVehicleDetailViewModel : ListViewModel<RouteQueue>
    {
        TrafficRoute _selectedroute;

        public TrafficRoute SelectedRoute
        {
            get { return _selectedroute; }
            set { _selectedroute = value; }
        }

        string vehiclename = string.Empty;

        public string VehicleName
        {
            get { return vehiclename; }
            set
            {
                vehiclename = value;
            }
        }

        private RouteQueue _selectedvehicle;
        public RouteQueue SelectedVehicle
        {
            get { return _selectedvehicle; }
            set { _selectedvehicle = value; }
        }

        public ICommand BtnDeliverVechileCommand { get; set; }
        public ICommand BtnDeliverAllVechileCommand { get; set; }
        public ICommand BtnRefreshVechileCommand { get; set; }

        public new void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            SelectedRoute = viewParameters["model"] as TrafficRoute;

            PageSizeValue = 10;

            base.ActivateView(viewName, viewParameters);
        }

        private TrafficManageServiceClient InitialClient()
        {
            TrafficManageServiceClient client = ServiceClientFactory.Create<TrafficManageServiceClient>();
            client.GetRouteQueueByRouteIDCompleted += client_GetRouteQueueByRouteIDCompleted;
            client.InsertRouteQueueCompleted += client_InsertRouteQueueCompleted;
            client.DeleteRouteQueueByIDCompleted += client_DeleteRouteQueueByIDCompleted;
            client.GetAllRouteQueueByRouteIDCompleted += client_GetAllRouteQueueByRouteIDCompleted;
            client.DeliverRouteQueueToVehicleCompleted += client_DeliverRouteQueueToVehicleCompleted;

            return client;
        }

        void client_DeliverRouteQueueToVehicleCompleted(object sender, DeliverRouteQueueToVehicleCompletedEventArgs e)
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
                        Data.MoveToFirstPage();
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

        void client_GetAllRouteQueueByRouteIDCompleted(object sender, GetAllRouteQueueByRouteIDCompletedEventArgs e)
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

        void client_DeleteRouteQueueByIDCompleted(object sender, DeleteRouteQueueByIDCompletedEventArgs e)
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
                TrafficManageServiceClient client = sender as TrafficManageServiceClient;
                CloseClient(client);
            }
        }

        void client_InsertRouteQueueCompleted(object sender, InsertRouteQueueCompletedEventArgs e)
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

        void client_GetRouteQueueByRouteIDCompleted(object sender, GetRouteQueueByRouteIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess == true)
                    {
                        Data.loader_Finished(new PagedResult<RouteQueue>
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
                ObservableCollection<RouteQueue> vehicles = new ObservableCollection<RouteQueue>();
                List<VehicleEx> list = selectwindow.SelectVehicleList;
                foreach (var item in list)
                {
                    RouteQueue rq = new RouteQueue();
                    rq.ID = Guid.NewGuid().ToString();
                    rq.VehicleID = item.VehicleId;
                    rq.RouteID = SelectedRoute.ID;
                    rq.CreateTime = DateTime.Now.ToUniversalTime();
                    rq.ClientID = SelectedRoute.ClientID;
                    rq.Name = SelectedRoute.Name;
                    rq.OperType = (short)OperTypeEnum.Add;
                    rq.Pts = SelectedRoute.Pts;
                    rq.MaxSpeed = SelectedRoute.MaxSpeed;
                    rq.OverSpeedDuration = SelectedRoute.OverSpeedDuration;
                    rq.PointCount = SelectedRoute.PointCount;
                    rq.RouteProperty = SelectedRoute.RouteProperty;
                    rq.RouteSegmentProperty = SelectedRoute.RouteSegmentProperty;
                    rq.Width = SelectedRoute.Width;

                    rq.StartTime = SelectedRoute.StartTime;
                    rq.EndTime = SelectedRoute.EndTime;
                    rq.Status = (short)CommandStateEnum.UnDelivered;

                    vehicles.Add(rq);
                }
                TrafficManageServiceClient client = InitialClient();
                client.InsertRouteQueueAsync(vehicles);

            }
        }

        public SendRouteVehicleDetailViewModel()
            : base()
        {
            try
            {
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
            if (SelectedRoute != null)
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
                    ObservableCollection<RouteQueue> vehicles = new ObservableCollection<RouteQueue>();
                    foreach (RouteQueue item in Data)
                    {
                        if (item.Status == (short)CommandStateEnum.UnDelivered)
                        {
                            vehicles.Add(item);
                        }
                    }
                    TrafficManageServiceClient client = InitialClient();
                    client.DeliverRouteQueueToVehicleAsync(vehicles);
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
                        ObservableCollection<RouteQueue> vehicles = new ObservableCollection<RouteQueue>();
                        vehicles.Add(SelectedVehicle);
                        TrafficManageServiceClient client = InitialClient();
                        client.DeliverRouteQueueToVehicleAsync(vehicles);
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
            if (SelectedRoute.Valid == false)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_RouteIsAbandon"));
                return;
            }
            else
            {
                TrafficManageServiceClient client = InitialClient();
                client.GetAllRouteQueueByRouteIDAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SelectedRoute.ID);
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
                Data = new PagedServerCollection<RouteQueue>((pageIndex, pageSize) =>
                {
                    if (SelectedRoute != null)
                    {
                        pageSize = PageSizeValue;
                        System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                        TrafficManageServiceClient client = InitialClient();
                        //查询数据
                        client.GetRouteQueueByRouteIDAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SelectedRoute.ID, VehicleName, pageIndex, pageSize);
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
                    client.DeleteRouteQueueByIDAsync(SelectedVehicle.ID);
                }
            }
        }
    }
}
