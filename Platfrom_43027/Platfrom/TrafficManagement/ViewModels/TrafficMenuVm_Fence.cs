using GisManagement.Models;
using Gsafety.Common.CommMessage;
using Gsafety.Common.CommMessage.Controls;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Traffic.Models;
using Gsafety.PTMS.Traffic.Views;
using Jounce.Core.Command;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gsafety.PTMS.Traffic.ViewModels
{
    public partial class TrafficMenuVm
    {
        /// <summary>
        /// add fence command
        /// </summary>
        public IActionCommand AddFenceCommand { get; private set; }

        /// <summary>
        /// query fence command
        /// </summary>
        public IActionCommand QueryFenceCommand { get; private set; }
        /// <summary>
        /// delete fence command
        /// </summary>
        public IActionCommand DeleteFenceCmd { get; private set; }
        /// <summary>
        /// edit fence shape command
        /// </summary>
        public IActionCommand EditFenceCommand { get; private set; }
        /// <summary>
        /// Plot fence command
        /// </summary>
        public IActionCommand MarkFenceGraphicCommand { get; private set; }
        /// <summary>
        /// show detail info view
        /// </summary>
        public IActionCommand OpenDetailViewCommand { get; private set; }

        /// <summary>
        /// 创建新版本
        /// </summary>
        public IActionCommand FenceNewVersionCommand { get; private set; }

        public IActionCommand EditFencePropertyCommand { get; private set; }
        /// <summary>
        /// 下发电子围栏
        /// </summary>
        public IActionCommand FenceSendToVehicleCommand { get; private set; }
        /// <summary>
        /// 废弃电子围栏
        /// </summary>
        public IActionCommand AbandonFenceCommand { get; private set; }

        private void InitialFence()
        {
            AddFenceCommand = new ActionCommand<object>(obj => AddFence());
            QueryFenceCommand = new ActionCommand<object>(obj => QueryFence());
            DeleteFenceCmd = new ActionCommand<object>(obj => DeleteFence());
            EditFenceCommand = new ActionCommand<object>(obj => EditFenceGeometry());
            MarkFenceGraphicCommand = new ActionCommand<object>(obj => MarkFenceGraphic(obj));
            AbandonFenceCommand = new ActionCommand<object>(obj => AbandonFence());
            FenceSendToVehicleCommand = new ActionCommand<object>(obj => SendToVehicle_Event(obj));
            EditFencePropertyCommand = new ActionCommand<object>(obj => EditFenceProperty_Event(obj));
            FenceNewVersionCommand = new ActionCommand<object>(obj => FenceNewVersion_Event());
        }


        /// <summary>
        /// add fence
        /// </summary>
        private void AddFence()
        {
            TrafficFence fence = new TrafficFence();
            fence.Radius = 0;
            fence.CircleCenter = "";
            fence.FenceType = (short)TrafficDrawType.Polygon;
            fence.ID = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            fence.CreateTime = DateTime.Now;
            fence.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;
            fence.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
            fence.Valid = true;

            AddFence currentAddFence = new AddFence(fence);
            currentAddFence.New = true;
            currentAddFence.afterAddFenceInfo += AfterAddFencinfo;
            currentAddFence.Closed += fence_Closed;
            currentAddFence.Show();
        }

        /// <summary>
        /// query fence
        /// </summary>
        private void QueryFence()
        {
            try
            {
                TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
                trafficServiceClient.GetTrafficFenceListByVehicleIDAndFenceNameCompleted += trafficServiceClient_GetTrafficFenceListByVehicleIDAndFenceNameCompleted;
                trafficServiceClient.GetTrafficFenceListByVehicleIDAndFenceNameAsync(QueryFenceText, QueryFenceVehicleId, ApplicationContext.Instance.AuthenticationInfo.ClientID);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("Gsafety.PTMS.Traffic.ViewModels.QueryFence()", ex);
            }
        }

        void trafficServiceClient_GetTrafficFenceListByVehicleIDAndFenceNameCompleted(object sender, GetTrafficFenceListByVehicleIDAndFenceNameCompletedEventArgs e)
        {
            SelectItem = null;
            if (e.Error != null || e.Result.IsSuccess == false)
            {
                ApplicationContext.Instance.Logger.LogException("trafficServiceClient_GetFenceByNameKeyCompleted", e.Error);
                EventAggregator.Publish<RefreshTrafficSelectStatus>(new RefreshTrafficSelectStatus() { });
                return;
            }

            ObservableCollection<TrafficFence> listFence = e.Result.Result;
            listFence = new ObservableCollection<TrafficFence>(listFence.Distinct(new FenceCompare()));

            for (int i = 0; i < listFence.Count; i++)
            {
                listFence[i].IsSelect = false;
                listFence[i].CreateTime = listFence[i].CreateTime.ToLocalTime();

                if (HasMarkElement(MarkType.markFence, listFence[i].ID.ToString()))
                {
                    listFence[i].IsmarkFenceGraphic = true;
                }
                else
                {
                    listFence[i].IsmarkFenceGraphic = false;
                }

            }
            //In descending order by modification time
            List<TrafficFence> sortedList = listFence.OrderByDescending(a => a.CreateTime).ToList();

            _fenceSourcePage = new PagedCollectionView(sortedList);
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FenceSourePage));
            if (sortedList != null && sortedList.Count > 0)
            {
                SelectItem = sortedList[0] as TrafficFence;
                SelectItem.IsSelect = true;
            }
            else
            {
                SelectItem = null;
            }
            FenceCount = sortedList.Count.ToString();
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectItem));
            //Under the fence, the control display other layers
            EventAggregator.Publish<ClearTrafficFeaturelayer>(new ClearTrafficFeaturelayer() { bLayerVisble = true, nType = TrafficFeature.Traffic_PolygonFence });
        }

        private void FenceNewVersion_Event()
        {
            if (SelectItem == null) return;
            TrafficFence newfence = SelectItem.Clone(SelectItem);
            newfence.ID = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            newfence.Valid = true;
            newfence.CreateTime = DateTime.Now;
            newfence.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;

            //object o = _fenceSourcePage.AddNew();
            //o = newfence;
            //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FenceSourePage));

            TrafficFence newe = newfence.Clone(newfence);
            newe.CreateTime = newe.CreateTime.ToUniversalTime();
            TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
            trafficServiceClient.InsertTrafficFenceCompleted += trafficServiceClient_InsertTrafficFenceCompleted;
            trafficServiceClient.InsertTrafficFenceAsync(newe);
        }

        private void trafficServiceClient_InsertTrafficFenceCompleted(object sender, InsertTrafficFenceCompletedEventArgs e)
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


        private void EditFenceProperty_Event(object obj)
        {
            if (SelectItem == null) return;
            if (SelectItem.Valid == false)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_FenceIsAbandon"));
                return;
            }

            TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();

            trafficServiceClient.IsFenceDeliveredCompleted += ((s, e) =>
            {
                if (e.Error != null || e.Result.IsSuccess == false)
                {
                    ApplicationContext.Instance.Logger.LogException("TrafficMenuVm:trafficServiceClient_GetVehicleByFenceCompleted", e.Error);
                }
                if (e.Result.IsSuccess == false)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_GetCar_Failed"));
                    EventAggregator.Publish<RefreshTrafficSelectStatus>(new RefreshTrafficSelectStatus() { });
                    return;
                }

                if (e.Result.Result == true)//有车使用了该围栏，不允许编辑
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Fence_InUsed"));
                    return;
                }
                AddFence currentAddFence = new AddFence(SelectItem.Clone(SelectItem));
                currentAddFence.New = false;
                currentAddFence.afterUpdatefenceInfo += currentAddFence_afterUpdatefenceInfo;
                currentAddFence.Closed += fence_Closed;
                currentAddFence.Show();
            });

            trafficServiceClient.IsFenceDeliveredAsync(SelectItem.ID.ToString());
        }

        void currentAddFence_afterUpdatefenceInfo(TrafficFence e)
        {
            SelectItem.SetProperty(e);
            SelectItem.IsSelect = true;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectItem));

            TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
            trafficServiceClient.UpdateTrafficFenceCompleted += trafficServiceClient_UpdateTrafficFenceCompleted;

            TrafficFence newe = e.Clone(e);
            newe.CreateTime = newe.CreateTime.ToUniversalTime();
            trafficServiceClient.UpdateTrafficFenceAsync(newe);

        }

        private void trafficServiceClient_UpdateTrafficFenceCompleted(object sender, UpdateTrafficFenceCompletedEventArgs e)
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

        /// <summary>
        /// Delete fence
        /// </summary>
        private void DeleteFence()
        {
            if (SelectItem == null) return;
            //if (SelectItem.Valid == false)
            //{
            //    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_FenceIsAbandon"));
            //    return;
            //}

            TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();

            trafficServiceClient.IsFenceDeliveredCompleted += trafficServiceClient_IsFenceDeliveredCompleted;
            trafficServiceClient.IsFenceDeliveredAsync(SelectItem.ID.ToString());

        }

        void trafficServiceClient_IsFenceDeliveredCompleted(object sender, IsFenceDeliveredCompletedEventArgs e)
        {
            if (e.Error != null || e.Result.IsSuccess == false)
            {
                ApplicationContext.Instance.Logger.LogException("TrafficMenuVm:trafficServiceClient_GetVehicleByFenceCompleted", e.Error);
            }
            if (e.Result.IsSuccess == false)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_GetCar_Failed"));
                EventAggregator.Publish<RefreshTrafficSelectStatus>(new RefreshTrafficSelectStatus() { });
                return;
            }

            if (e.Result.Result == false)//没有车使用了该围栏
            {
                //Delete the database information space fence

                var res = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_DeleteFence"), MessageDialogButton.OkAndCancel);
                res.Closed += (s, o) =>
                {
                    if (res.DialogResult == true)
                    {
                        EventAggregator.Publish<DeleteFenceArgs>(new DeleteFenceArgs() { ID = SelectItem.ID.ToString() });
                        _fenceSourcePage.Remove(SelectItem);

                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FenceSourePage));
                        //QueryFence();
                    }
                    else
                    {
                        if (_curSelectParme == TrafficFeature.Traffic_PolygonFence)
                        {
                            if (SelectItem != null)
                            {
                                SelectItem.IsSelect = true;
                            }
                        }
                    }
                };

            }
            else
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_DeleteCarFence"));
                EventAggregator.Publish<RefreshTrafficSelectStatus>(new RefreshTrafficSelectStatus() { });
                return;
            }
        }



        private void AbandonFence()
        {
            if (SelectItem == null) return;
            if (SelectItem.Valid == false)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_FenceIsAbandon"));
                return;
            }
            var res = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_FenceAbandonOrNo"), MessageDialogButton.OkAndCancel);
            res.Closed += (s, e) =>
            {
                if (res.DialogResult == true)
                {
                    SelectItem.Valid = false;
                    TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
                    trafficServiceClient.ObsoleteFenceCompleted += trafficServiceClient_ObsoleteFenceCompleted;
                    trafficServiceClient.ObsoleteFenceAsync(SelectItem.ID);
                }

                if (SelectItem != null)
                {
                    SelectItem.IsSelect = true;
                }

            };
        }

        void trafficServiceClient_ObsoleteFenceCompleted(object sender, ObsoleteFenceCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_FenceAbandonFail"));
                ApplicationContext.Instance.Logger.LogException("Gsafety.PTMS.Traffic.ViewModels.ObsoleteFence", e.Error);
            }
            //重新加载界面更新列表数据
            this.QueryFence();

        }

        /// <summary>
        /// Vehicle id
        /// </summary>
        private string _QueryFenceVehicleId = "";
        public string QueryFenceVehicleId
        {
            get { return _QueryFenceVehicleId; }
            set
            {
                _QueryFenceVehicleId = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => QueryFenceVehicleId));
            }
        }



        /// <summary>
        /// fence list
        /// </summary>
        PagedCollectionView _fenceSourcePage;
        public PagedCollectionView FenceSourePage
        {
            get { return _fenceSourcePage; }
        }
        /// <summary>
        /// query keyword
        /// </summary>
        private string _queryFenceText = "";
        public string QueryFenceText
        {
            get { return _queryFenceText; }
            set
            {
                _queryFenceText = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => QueryFenceText));
            }
        }


        private void fence_Closed(object sender, EventArgs e)
        {
            if (SelectItem != null)
            {
                SelectItem.IsSelect = true;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectItem));
            }
        }
        /// <summary>
        /// After completion of the fence to increase the basis of information events, 
        /// notification GIS interface to start drawing fence
        /// </summary>
        /// <param name="e"></param>
        /// <param name="nType"></param>
        private void AfterAddFencinfo(TrafficFence e, TrafficDrawType nType, double dDist)
        {
            EventAggregator.Publish<DrawFenceEventArgs>(new DrawFenceEventArgs() { nType = nType, dDist = dDist, eleFence = e });
        }

        #region Select the fence, GIS graphical interface displays the fence, the fence display specific information
        /// <summary>
        /// selected fence
        /// </summary>
        private TrafficFence _selectItem = null;
        public TrafficFence SelectItem
        {
            get { return _selectItem; }
            set
            {
                if (_selectItem != null)
                {
                    SelectItem.IsSelect = false;
                }
                _selectItem = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FenceSourePage));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectItem));


                _selectItem = value as TrafficFence;
                EventAggregator.Publish<ShowFenceInfoArgs>(new ShowFenceInfoArgs() { selectEleFence = _selectItem, Featuetype = TrafficFeature.Traffic_PolygonFence });

                SelectedFenceChange selectedfence = new SelectedFenceChange();
                selectedfence.SelectedFence = _selectItem;
                EventAggregator.Publish<SelectedFenceChange>(selectedfence);

                if (_selectItem != null)
                {
                    SelectItem.IsSelect = true;
                }
                //  EventAggregator.Publish<RefreshSelectCarListArgs>(new RefreshSelectCarListArgs() { });
            }
        }
        #endregion

        #region Edit fence
        /// <summary>
        /// Edit fence shape
        /// </summary>
        private void EditFenceGeometry()
        {
            if (SelectItem == null) return;
            if (SelectItem.Valid == false)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_FenceIsAbandon"));
                return;
            }
            TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();

            trafficServiceClient.IsFenceDeliveredCompleted += ((s, e) =>
            {
                if (e.Error != null || e.Result.IsSuccess == false)
                {
                    ApplicationContext.Instance.Logger.LogException("TrafficMenuVm:trafficServiceClient_GetVehicleByFenceCompleted", e.Error);
                }
                if (e.Result.IsSuccess == false)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_GetCar_Failed"));
                    EventAggregator.Publish<RefreshTrafficSelectStatus>(new RefreshTrafficSelectStatus() { });
                    return;
                }

                if (e.Result.Result == true)//有车使用了该围栏，不允许编辑
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Fence_InUsed"));
                    return;
                }
                EventAggregator.Publish<EditGeometryArgs>(new EditGeometryArgs() { nType = TrafficFeature.Traffic_PolygonFence, selectFence = SelectItem });
            });

            trafficServiceClient.IsFenceDeliveredAsync(SelectItem.ID.ToString());

        }
        #endregion

        private void MarkFenceGraphic(object obj)
        {
            if (SelectItem == null) return;
            SelectItem.IsmarkFenceGraphic = !SelectItem.IsmarkFenceGraphic;
            SymbolParams parm = new SymbolParams();
            if (SelectItem.IsmarkFenceGraphic == true)
            {
                SymbolStyleSet symbolSelect = new SymbolStyleSet();
                symbolSelect.ControlTabItemVisbility(1, 0);
                symbolSelect.ControlTabItemVisbility(2, 0);
                symbolSelect.Closed += ((sender, args) =>
                {
                    if ((bool)symbolSelect.DialogResult)
                    {
                        parm.FillColorParm = symbolSelect.FillColorParm;
                        parm.TransparentParm = symbolSelect.TansparentParm;
                        parm.MarkColorParm = symbolSelect.MarkColorParm;
                        parm.MarkSizeParm = symbolSelect.SymbolSize;
                    }
                    else
                    {
                        //parm.FillColorParm = Colors.Red;
                        //parm.TransparentParm = 0.3;
                        if (SelectItem != null) SelectItem.IsSelect = true;
                        SelectItem.IsmarkFenceGraphic = !SelectItem.IsmarkFenceGraphic;
                        return;
                    }
                    EventAggregator.Publish<MarkTrafficGraphic>(new MarkTrafficGraphic() { nType = TrafficFeature.Traffic_PolygonFence, parentId = SelectItem.ID, childId = "", TrafficFence = SelectItem, bShow = SelectItem.IsmarkFenceGraphic, MarkSymbolParm = parm });
                    UpdateHasMarkElements(SelectItem.IsmarkFenceGraphic, MarkType.markFence, SelectItem.ID.ToString(), parm);
                    if (SelectItem != null)
                        SelectItem.IsSelect = true;
                });

                symbolSelect.Show();
            }
            else
            {
                EventAggregator.Publish<MarkTrafficGraphic>(new MarkTrafficGraphic() { nType = TrafficFeature.Traffic_PolygonFence, parentId = SelectItem.ID, childId = "", TrafficFence = SelectItem, bShow = SelectItem.IsmarkFenceGraphic, MarkSymbolParm = null });
                UpdateHasMarkElements(SelectItem.IsmarkFenceGraphic, MarkType.markFence, SelectItem.ID.ToString(), null);
            }
        }

        #region Display total number
        /// <summary>
        /// fence
        /// </summary>
        private string _FenceCount = "";
        public string FenceCount
        {
            get { return _FenceCount; }
            set
            {
                _FenceCount = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FenceCount));
            }
        }
        #endregion

        private void SendToVehicle_Event(object obj)
        {
            if (SelectItem == null) return;

            if (SelectItem != null)
            {
                SendVehicleDetailView childwindow = new SendVehicleDetailView(string.Empty, new Dictionary<string, object>() { { "model", SelectItem } });
                childwindow.Closed += fencechildwindow_Closed;
                childwindow.Show();
            }
        }

        void fencechildwindow_Closed(object sender, EventArgs e)
        {
            QueryFence();
        }

        /// <summary>
        /// check is plot
        /// </summary>
        /// <param name="nType"></param>
        /// <param name="strOID"></param>
        /// <returns></returns>
        private bool HasMarkElement(MarkType nType, string strOID)
        {
            for (int i = 0; i < _HasMarkElements.Count; i++)
            {
                if (_HasMarkElements[i].OBJECTID == strOID && _HasMarkElements[i].TYPE == nType)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
