using Gsafety.Common.CommMessage;
using Gsafety.Common.CommMessage.Controls;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Traffic.Models;
using Gsafety.PTMS.Traffic.Views;
using Jounce.Core.Command;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace Gsafety.PTMS.Traffic.ViewModels
{
    public partial class TrafficMenuVm
    {
        public IActionCommand AddRouteCommand { get; private set; }
        public IActionCommand QueryRouteCommand { get; private set; }
        public IActionCommand RouteNewVersionCommand { get; private set; }
        public IActionCommand EditRouteCommand { get; private set; }
        public IActionCommand RouteOpenDetailViewCommand { get; private set; }
        public IActionCommand EditRoutePropertyCommand { get; private set; }
        public IActionCommand RouteSendToVehicleCommand { get; private set; }
        public IActionCommand AbandonRouteCommand { get; private set; }
        public IActionCommand DeleteRouteCmd { get; private set; }
        public IActionCommand MarkRouteGraphicCommand { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitTrafficRoute()
        {
            AddRouteCommand = new ActionCommand<object>(obj => AddRoute());
            QueryRouteCommand = new ActionCommand<object>(obj => QueryRoute());
            RouteNewVersionCommand = new ActionCommand<object>(obj => RouteNewVersion_Event());
            EditRouteCommand = new ActionCommand<object>(obj => EditRouteGeometry());
            EditRoutePropertyCommand = new ActionCommand<object>(obj => EditRouteProperty_Event());
            AbandonRouteCommand = new ActionCommand<object>(obj => AbandonRoute());
            DeleteRouteCmd = new ActionCommand<object>(obj => DeleteRoute_Event());
            MarkRouteGraphicCommand = new ActionCommand<object>(obj => MarkRouteGraphic());
            RouteSendToVehicleCommand = new ActionCommand<object>(obj => SendToVehicle_Event());
        }

        private void SendToVehicle_Event()
        {
            if (RouteSelectItem == null) return;

            if (RouteSelectItem != null)
            {
                SendRouteVehicleDetailView childwindow = new SendRouteVehicleDetailView(string.Empty, new Dictionary<string, object>() { { "model", RouteSelectItem } });
                childwindow.Closed += routechildwindow_Closed;
                childwindow.Show();
            }
        }

        void routechildwindow_Closed(object sender, EventArgs e)
        {
            QueryRoute();
        }

        private void MarkRouteGraphic()
        {
            if (RouteSelectItem == null) return;
            RouteSelectItem.IsmarkRouteGraphic = !RouteSelectItem.IsmarkRouteGraphic;
            SymbolParams parm = new SymbolParams();
            if (RouteSelectItem.IsmarkRouteGraphic == true)
            {
                SymbolStyleSet symbolSelect = new SymbolStyleSet();
                symbolSelect.ControlTabItemVisbility(0, 1);
                symbolSelect.ControlTabItemVisbility(2, 1);
                symbolSelect.Closed += ((sender, args) =>
                {
                    if ((bool)symbolSelect.DialogResult)
                    {
                        parm.LineColorParm = symbolSelect.LineColorParm;
                        parm.LineWidthParm = symbolSelect.LineWidthParm;
                        parm.TransparentParm = symbolSelect.TansparentParm;
                        parm.MarkColorParm = symbolSelect.MarkColorParm;
                        parm.MarkSizeParm = symbolSelect.SymbolSize;
                    }
                    else
                    {
                        //parm.FillColorParm = Colors.Red;
                        //parm.TransparentParm = 0.3;
                        if (RouteSelectItem != null) RouteSelectItem.IsSelect = true;
                        RouteSelectItem.IsmarkRouteGraphic = !RouteSelectItem.IsmarkRouteGraphic;
                        return;
                    }
                    EventAggregator.Publish<MarkTrafficGraphic>(new MarkTrafficGraphic() { nType = TrafficFeature.Traffic_Route, parentId = RouteSelectItem.ID, childId = "", TrafficRoute = RouteSelectItem, bShow = RouteSelectItem.IsmarkRouteGraphic, MarkSymbolParm = parm });
                    UpdateHasMarkElements(RouteSelectItem.IsmarkRouteGraphic, MarkType.markRoute, RouteSelectItem.ID.ToString(), parm);
                    if (RouteSelectItem != null)
                        RouteSelectItem.IsSelect = true;
                });

                symbolSelect.Show();
            }
            else
            {
                EventAggregator.Publish<MarkTrafficGraphic>(new MarkTrafficGraphic() { nType = TrafficFeature.Traffic_Route, parentId = RouteSelectItem.ID, childId = "", TrafficRoute = RouteSelectItem, bShow = RouteSelectItem.IsmarkRouteGraphic, MarkSymbolParm = null });
                UpdateHasMarkElements(RouteSelectItem.IsmarkRouteGraphic, MarkType.markRoute, RouteSelectItem.ID.ToString(), null);
            }
        }

        /// <summary>
        /// Delete fence
        /// </summary>
        private void DeleteRoute_Event()
        {
            if (RouteSelectItem == null) return;
            //if (RouteSelectItem.Valid == false)
            //{
            //    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_RouteIsAbandon"));
            //    return;
            //}

            TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();

            trafficServiceClient.IsRouteDeliveredCompleted += trafficServiceClient_IsRouteDeliveredCompleted;
            trafficServiceClient.IsRouteDeliveredAsync(RouteSelectItem.ID.ToString());

        }

        void trafficServiceClient_IsRouteDeliveredCompleted(object sender, IsRouteDeliveredCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null || e.Result.IsSuccess == false)
                {
                    ApplicationContext.Instance.Logger.LogException("TrafficMenuVm:trafficServiceClient_IsRouteDeliveredCompleted", e.Error);
                }
                if (e.Result.IsSuccess == false)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_GetCar_Failed"));
                    EventAggregator.Publish<RefreshRouteSelectStatus>(new RefreshRouteSelectStatus() { });
                    return;
                }

                if (e.Result.Result == false)//没有车使用了该围栏
                {
                    //Delete the database information space route

                    var res = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_DeleteRouteOrNot"), MessageDialogButton.OkAndCancel);
                    res.Closed += (s, o) =>
                    {
                        if (res.DialogResult == true)
                        {
                            EventAggregator.Publish<DeleteRouteArgs>(new DeleteRouteArgs() { ID = RouteSelectItem.ID.ToString() });

                            _RouteSourcePage.Remove(RouteSelectItem);

                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RouteSourePage));
                            //QueryRoute();
                        }
                        else
                        {
                            if (RouteSelectItem != null)
                            {
                                RouteSelectItem.IsSelect = true;
                            }
                        }
                    };

                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_DeleteRoute_HaveCar"));
                    EventAggregator.Publish<RefreshRouteSelectStatus>(new RefreshRouteSelectStatus() { });
                    return;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("trafficServiceClient_IsRouteDeliveredCompleted", ex);
            }
        }



        #region 废弃线路
        private void AbandonRoute()
        {
            if (RouteSelectItem == null) return;

            var res = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_RouteAbandonOrNo"), MessageDialogButton.OkAndCancel);
            res.Closed += (s, e) =>
            {
                if (res.DialogResult == true)
                {
                    RouteSelectItem.Valid = false;
                    TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
                    trafficServiceClient.ObsoleteTrafficeRouteCompleted += trafficServiceClient_ObsoleteTrafficeRouteCompleted;
                    trafficServiceClient.ObsoleteTrafficeRouteAsync(RouteSelectItem.ID);
                }

                if (RouteSelectItem != null)
                {
                    RouteSelectItem.IsSelect = true;
                }

            };
        }

        void trafficServiceClient_ObsoleteTrafficeRouteCompleted(object sender, ObsoleteTrafficeRouteCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_FenceAbandonFail"));
                    ApplicationContext.Instance.Logger.LogException("Gsafety.PTMS.Traffic.ViewModels.ObsoleteFence", e.Error);
                }
                //重新加载界面更新列表数据
                this.QueryRoute();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("trafficServiceClient_ObsoleteTrafficeRouteCompleted", ex);
            }
        }


        #endregion

        #region 编辑路线属性
        /// <summary>
        /// 编辑路线属性
        /// </summary>
        /// <returns></returns>
        private void EditRouteProperty_Event()
        {
            if (RouteSelectItem == null) return;
            if (RouteSelectItem.Valid == false)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_RouteIsAbandon"));
                return;
            }
            TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();

            trafficServiceClient.IsRouteDeliveredCompleted += ((s, e) =>
            {
                if (e.Error != null || e.Result.IsSuccess == false)
                {
                    ApplicationContext.Instance.Logger.LogException("TrafficMenuVm:trafficServiceClient_IsRouteDeliveredCompleted", e.Error);
                }
                if (e.Result.IsSuccess == false)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_GetCar_Failed"));
                    EventAggregator.Publish<RefreshRouteSelectStatus>(new RefreshRouteSelectStatus() { });
                    return;
                }

                if (e.Result.Result == true)//有车使用了该围栏，不允许编辑
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Route_InUsed"));
                    return;
                }
                AddRoute currentAddRoute = new AddRoute(RouteSelectItem.Clone(RouteSelectItem));
                currentAddRoute.New = false;
                currentAddRoute.afterUpdateRouteInfo += currentAddRoute_afterUpdatefenceInfo;
                currentAddRoute.Closed += route_Closed;
                currentAddRoute.Show();
            });

            trafficServiceClient.IsRouteDeliveredAsync(RouteSelectItem.ID.ToString());


        }

        private void route_Closed(object sender, EventArgs e)
        {
            if (RouteSelectItem != null)
            {
                RouteSelectItem.IsSelect = true;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RouteSelectItem));
            }
        }

        private void currentAddRoute_afterUpdatefenceInfo(TrafficRoute e)
        {
            RouteSelectItem.SetProperty(e);

            RouteSelectItem.IsSelect = true;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RouteSelectItem));

            TrafficRoute newe = e.Clone(e);
            newe.CreateTime = newe.CreateTime.ToUniversalTime();

            TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
            trafficServiceClient.UpdateTrafficRouteCompleted += trafficServiceClient_UpdateTrafficRouteCompleted;
            trafficServiceClient.UpdateTrafficRouteAsync(newe);
        }

        void trafficServiceClient_UpdateTrafficRouteCompleted(object sender, UpdateTrafficRouteCompletedEventArgs e)
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
                        QueryFence();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_GetSpeedLimitList", ex);
            }
            finally
            {
                TrafficManageServiceClient client = sender as TrafficManageServiceClient;
                client.CloseAsync();
                client = null;
            }
        }
        #endregion

        /// <summary>
        /// 编辑路线
        /// </summary>
        /// <returns></returns>
        private void EditRouteGeometry()
        {
            if (RouteSelectItem == null) return;
            if (RouteSelectItem.Valid == false)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_RouteIsAbandon"));
                return;
            }

            TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();

            trafficServiceClient.IsRouteDeliveredCompleted += ((s, e) =>
            {
                if (e.Error != null || e.Result.IsSuccess == false)
                {
                    ApplicationContext.Instance.Logger.LogException("TrafficMenuVm:trafficServiceClient_IsRouteDeliveredCompleted", e.Error);
                }
                if (e.Result.IsSuccess == false)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_GetCar_Failed"));
                    EventAggregator.Publish<RefreshRouteSelectStatus>(new RefreshRouteSelectStatus() { });
                    return;
                }

                if (e.Result.Result == true)//有车使用了该围栏，不允许编辑
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Route_InUsed"));
                    return;
                }
                EventAggregator.Publish<EditGeometryArgs>(new EditGeometryArgs() { nType = TrafficFeature.Traffic_Route, selectRoute = RouteSelectItem });
            });

            trafficServiceClient.IsRouteDeliveredAsync(RouteSelectItem.ID.ToString());
        }
        #region 查询路线
        /// <summary>
        /// 查询路线
        /// </summary>
        private void QueryRoute()
        {
            try
            {
                TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
                trafficServiceClient.GetTrafficRouteListByVehicleIDAndRouteNameCompleted += trafficServiceClient_GetTrafficRouteListByVehicleIDAndRouteNameCompleted;
                trafficServiceClient.GetTrafficRouteListByVehicleIDAndRouteNameAsync(QueryRouteText, QueryRouteVehicleId, ApplicationContext.Instance.AuthenticationInfo.ClientID);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("Gsafety.PTMS.Traffic.ViewModels.QueryRoute()", ex);
            }
        }

        /// <summary>
        /// 填写左边列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void trafficServiceClient_GetTrafficRouteListByVehicleIDAndRouteNameCompleted(object sender, GetTrafficRouteListByVehicleIDAndRouteNameCompletedEventArgs e)
        {
            try
            {
                RouteSelectItem = null;
                if (e.Error != null || e.Result.IsSuccess == false)
                {
                    ApplicationContext.Instance.Logger.LogException("trafficServiceClient_GetRouteByNameKeyCompleted", e.Error);
                    EventAggregator.Publish<RefreshRouteSelectStatus>(new RefreshRouteSelectStatus() { });
                    return;
                }
                ObservableCollection<TrafficRoute> listRoute = e.Result.Result;
                listRoute = new ObservableCollection<TrafficRoute>(listRoute.Distinct(new RouteCompare()));

                for (int i = 0; i < listRoute.Count; i++)
                {
                    listRoute[i].IsSelect = false;
                    listRoute[i].CreateTime = listRoute[i].CreateTime.ToLocalTime();
                    if (HasMarkElement(MarkType.markRoute, listRoute[i].ID.ToString()))
                    {
                        listRoute[i].IsmarkRouteGraphic = true;
                    }
                    else
                    {
                        listRoute[i].IsmarkRouteGraphic = false;
                    }

                }
                //In descending order by modification time
                List<TrafficRoute> sortedList = listRoute.OrderByDescending(a => a.CreateTime).ToList();

                _RouteSourcePage = new PagedCollectionView(sortedList);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RouteSourePage));
                if (sortedList != null && sortedList.Count > 0)
                {
                    RouteSelectItem = sortedList[0] as TrafficRoute;
                    RouteSelectItem.IsSelect = true;
                }
                else
                {
                    RouteSelectItem = null;
                }
                RouteCount = sortedList.Count.ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RouteSelectItem));
                //Under the Route, the control display other layers
                EventAggregator.Publish<ClearTrafficFeaturelayer>(new ClearTrafficFeaturelayer() { bLayerVisble = true, nType = TrafficFeature.Traffic_Route });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("trafficServiceClient_GetTrafficRouteListByVehicleIDAndRouteNameCompleted", ex);
            }
        }


        private string _RouteCount = "";
        public string RouteCount
        {
            get { return _RouteCount; }
            set
            {
                _RouteCount = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RouteCount));
            }
        }
        public void HandleEvent(RefreshRouteSelectStatus publishedEvent)
        {
            if (RouteSelectItem != null) RouteSelectItem.IsSelect = true;
        }
        /// <summary>
        /// Vehicle id
        /// </summary>
        private string _QueryRouteVehicleId = "";
        public string QueryRouteVehicleId
        {
            get { return _QueryRouteVehicleId; }
            set
            {
                _QueryRouteVehicleId = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => QueryRouteVehicleId));
            }
        }

        /// <summary>
        /// query keyword
        /// </summary>
        private string _queryRouteText = "";
        public string QueryRouteText
        {
            get { return _queryRouteText; }
            set
            {
                _queryRouteText = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => QueryRouteText));
            }
        }

        private TrafficRoute _RouteselectItem = null;
        public TrafficRoute RouteSelectItem
        {
            get { return _RouteselectItem; }
            set
            {
                if (_RouteselectItem != null)
                {
                    RouteSelectItem.IsSelect = false;
                }
                _RouteselectItem = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RouteSourePage));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RouteSelectItem));


                _RouteselectItem = value as TrafficRoute;
                EventAggregator.Publish<ShowFenceInfoArgs>(new ShowFenceInfoArgs() { selectRoute = _RouteselectItem, Featuetype = TrafficFeature.Traffic_Route });

                SelectedRouteChange selectedroute = new SelectedRouteChange();
                selectedroute.SelectedRoute = _RouteselectItem;
                EventAggregator.Publish<SelectedRouteChange>(selectedroute);

                if (_RouteselectItem != null)
                {
                    RouteSelectItem.IsSelect = true;
                }
                //  EventAggregator.Publish<RefreshSelectCarListArgs>(new RefreshSelectCarListArgs() { });
            }
        }

        /// <summary>
        /// Route list
        /// </summary>
        PagedCollectionView _RouteSourcePage;
        public PagedCollectionView RouteSourePage
        {
            get { return _RouteSourcePage; }

        }

        public class RouteCompare : IEqualityComparer<TrafficRoute>
        {
            public bool Equals(TrafficRoute x, TrafficRoute y)
            {
                return x.ID == y.ID;
            }

            public int GetHashCode(TrafficRoute s)
            {
                return s.ID.GetHashCode();
            }
        }
        #endregion

        #region 添加行驶路线
        /// <summary>
        /// 添加行驶线路
        /// </summary>
        /// <returns></returns>
        private void AddRoute()
        {
            TrafficRoute route = new TrafficRoute();
            route.ID = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            route.CreateTime = DateTime.Now;
            route.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;
            route.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
            route.Valid = true;

            AddRoute currentAddRoute = new AddRoute(route); ;
            currentAddRoute.New = true;
            currentAddRoute.afterAddRouteInfo += AfterAddRouteinfo;
            currentAddRoute.Closed += AddRoute_Closed;
            currentAddRoute.Show();
        }
        /// <summary>
        /// 关闭输入信息窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddRoute_Closed(object sender, EventArgs e)
        {
            if (RouteSelectItem != null)
            {
                RouteSelectItem.IsSelect = true;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RouteSelectItem));
            }
        }
        /// <summary>
        /// 开始绘制路线
        /// </summary>
        /// <param name="e"></param>
        private void AfterAddRouteinfo(TrafficRoute e)
        {
            EventAggregator.Publish<DrawRoutEventArgs>(new DrawRoutEventArgs() { Route = e });
        }
        #endregion

        #region 复制版本
        /// <summary>
        /// 复制版本
        /// </summary>
        private void RouteNewVersion_Event()
        {
            if (RouteSelectItem == null) return;
            TrafficRoute newroute = RouteSelectItem.Clone(RouteSelectItem);
            newroute.ID = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            newroute.Valid = true;
            newroute.CreateTime = DateTime.Now;
            newroute.Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;

            //object o = _fenceSourcePage.AddNew();
            //o = newfence;
            //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FenceSourePage));

            TrafficRoute newe = newroute.Clone(newroute);
            newe.CreateTime = newe.CreateTime.ToUniversalTime();

            TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
            trafficServiceClient.InsertTrafficRouteCompleted += trafficServiceClient_InsertTrafficRouteCompleted;
            trafficServiceClient.InsertTrafficRouteAsync(newe);
        }
        /// <summary>
        /// 复制版本返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void trafficServiceClient_InsertTrafficRouteCompleted(object sender, InsertTrafficRouteCompletedEventArgs e)
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
                        QueryRoute();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_GetSpeedLimitList", ex);
            }
            finally
            {
                TrafficManageServiceClient client = sender as TrafficManageServiceClient;
                client.CloseAsync();
                client = null;
            }
        }

        public void HandleEvent(AddRouteCompleteArgs publishedEvent)
        {
            QueryRoute();
        }
        #endregion



    }
}
