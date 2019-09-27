using Gsafety.PTMS.Share;
using BaseLib.ViewModels;
using Jounce.Core.ViewModel;
using System.Collections.Generic;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Linq;
using Gsafety.Common.Controls;
using Gsafety.Common.CommMessage;
using Gsafety.Common.CommMessage.Controls;
using Gsafety.PTMS.Bases.Enums;
using Jounce.Core.Command;
using Jounce.Core.Event;
using Jounce.Framework.Command;
using Gsafety.Ant.Monitor.Models;
using BaseLib.Model;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using System;
using System.ComponentModel.Composition;
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

namespace Gsafety.PTMS.Monitor.ViewModels
{
    [ExportAsViewModel(MonitorName.MonitorSelectFenceViewModel)]
    public class SelectFenceViewModel : BaseViewModel, IPartImportsSatisfiedNotification, IEventSink<SelectFenceDispayArgs>
    {
        public IActionCommand MarkFenceGraphicCommand { get; private set; }
        public IActionCommand QueryFenceCommand { get; private set; }

        
        private bool m_IsOpen = true;
        public bool SelectFenceIsOpen
        {
            get
            {
                return m_IsOpen;
            }
            set
            {
                m_IsOpen = value;
              
            }
        }

        private int IncidentLevel { get; set; }

        private bool _IsVisual = true;
        public bool SelectFenceIsVisual
        {
            get
            {
                return _IsVisual;
            }
            set
            {
                _IsVisual = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectFenceIsVisual));
            }
        }


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


        private string _QueryFenceText = "";
        public string QueryFenceText
        {
            get { return _QueryFenceText; }
            set
            {
                _QueryFenceText = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => QueryFenceText));
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
                if (value != null)
                {
                    _selectItem = value;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FenceSourePage));
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectItem));                   
                }

                if (_selectItem != null)
                {
                    SelectItem.IsSelect = true;
                }
             
                //  EventAggregator.Publish<RefreshSelectCarListArgs>(new RefreshSelectCarListArgs() { });
            }
        }

        public void OnImportsSatisfied()
        {
            try
            {
               
                EventAggregator.SubscribeOnDispatcher<SelectFenceDispayArgs>(this);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void HandleEvent(SelectFenceDispayArgs publishedEvent)
        {
            SelectFenceIsVisual = publishedEvent.Show;
        }

        public SelectFenceViewModel()
        {
            MarkFenceGraphicCommand = new ActionCommand<object>(obj => MarkFenceGraphic(obj));
            QueryFenceCommand = new ActionCommand<object>(obj => QueryFence());
            //获得电子围栏列表

            TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<Gsafety.PTMS.ServiceReference.TrafficManageService.TrafficManageServiceClient>();
            trafficServiceClient.GetTrafficFenceListByVehicleIDAndFenceNameCompleted += trafficServiceClient_GetTrafficFenceListByVehicleIDAndFenceNameCompleted;
            trafficServiceClient.GetTrafficFenceListByVehicleIDAndFenceNameAsync("", "", ApplicationContext.Instance.AuthenticationInfo.ClientID);
        }

        private void QueryFence()
        {
            try
            {
                TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
                trafficServiceClient.GetTrafficFenceListByVehicleIDAndFenceNameCompleted += trafficServiceClient_GetTrafficFenceListByVehicleIDAndFenceNameCompleted;
                trafficServiceClient.GetTrafficFenceListByVehicleIDAndFenceNameAsync(QueryFenceText, "", ApplicationContext.Instance.AuthenticationInfo.ClientID);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("Gsafety.PTMS.Traffic.ViewModels.QueryFence()", ex);
            }
        }
        private void MarkFenceGraphic(object obj)
        {
            if (SelectItem == null) return;
            SelectItem.IsmarkFenceGraphic = !SelectItem.IsmarkFenceGraphic;  
            if (SelectItem.IsmarkFenceGraphic == true)
            {
                string vehicleId = obj.ToString();
               
                        SymbolParams parm = new SymbolParams();
                        SymbolStyleSet symbolSelect = new SymbolStyleSet();
                        symbolSelect.ControlTabItemVisbility(1, 0);
                        symbolSelect.ControlTabItemVisbility(2, 0);
                        symbolSelect.Closed += ((o, args) =>
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
                                return;
                            }


                            EventAggregator.Publish<MarkTrafficGraphic>(new MarkTrafficGraphic() { nType = TrafficFeature.Traffic_PolygonFence, parentId = SelectItem.ID, childId = SelectItem.ID, TrafficFence = SelectItem, bShow = true, MarkSymbolParm = parm });
                           
                            EventAggregator.Publish<ZoomGisView>(new ZoomGisView());
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectItem));

                        });
                        symbolSelect.Show();
                  
            }            
            else
            {

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectItem));
                EventAggregator.Publish<MarkTrafficGraphic>(new MarkTrafficGraphic() { nType = TrafficFeature.Traffic_PolygonFence, parentId = SelectItem.ID, childId = SelectItem.ID, TrafficFence = null, bShow = false, MarkSymbolParm = null });

            }
        }

        private void UpdateHasMarkElements(bool bMark, MarkType nType, string strOID, SymbolParams parm)
        {
            if (bMark == true)
            {
                _HasMarkElements.Add(new MarkElements() { OBJECTID = strOID, TYPE = nType, parm = parm });
            }
            else
            {
                for (int i = 0; i < _HasMarkElements.Count; i++)
                {
                    if (_HasMarkElements[i].OBJECTID == strOID && _HasMarkElements[i].TYPE == nType)
                    {
                        _HasMarkElements.RemoveAt(i);
                        return;
                    }
                }
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
        private List<MarkElements> _HasMarkElements = new List<MarkElements>();

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

    public class FenceCompare : IEqualityComparer<TrafficFence>
    {
        public bool Equals(TrafficFence x, TrafficFence y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(TrafficFence s)
        {
            return s.ID.GetHashCode();
        }
    }

    public class SelectedFenceChange
    {
        TrafficFence _selectedfence;

        public TrafficFence SelectedFence
        {
            get { return _selectedfence; }
            set { _selectedfence = value; }
        }
    }
    public class MarkElements
    {
        /// <summary>
        /// type
        /// </summary>
        public MarkType TYPE { get; set; }
        /// <summary>
        /// object OID
        /// </summary>
        public string OBJECTID { get; set; }
        /// <summary>
        /// symbol
        /// </summary>
        public SymbolParams parm { get; set; }
    }
    public enum MarkType
    {
        markFence = 0,
        markRoute = 1,
        markLongRoute = 2,
        markStop = 3,
        markStopRout = 4,
        markStopLongRoute = 5,
        markStopScheDule = 6
    }


}
