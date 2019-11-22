using GisManagement.Models;
using Gsafety.Common.CommMessage;
using Gsafety.Common.CommMessage.Controls;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Traffic.Models;
using Gsafety.PTMS.Traffic.Views;
using Jounce.Core.Command;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 87103962-96bd-42a0-94ad-d2a63b17f6e9      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.ViewModels
/////    Project Description:    
/////             Class Name: TrafficMenuVm
/////          Class Version: v1.0.0.0
/////            Create Time: 8/14/2013 9:19:22 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/14/2013 9:19:22 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace Gsafety.PTMS.Traffic.ViewModels
{
    /// <summary>
    /// Traffic Management on the left menu interface
    /// </summary>
    [ExportAsViewModel(TrafficName.TrafficMenuVm)]
    public partial class TrafficMenuVm : BaseViewModel,
        //IEventSink<RefreshTrafficManagerListArgs>,
        //IEventSink<ReturnRoutePts>,
        IEventSink<RefreshTrafficSelectStatus>,
        IEventSink<RefreshRouteSelectStatus>,
        //IEventSink<UpdateTrafficMarkArgs>,
        IEventSink<AddFenceCompleteArgs>,
        IEventSink<AddRouteCompleteArgs>,
        IPartImportsSatisfiedNotification
    {
        #region Menu select events
        /// <summary>
        /// Select the menu number
        /// </summary>
        private int _Traffic_MenuSelectIndex = 0;
        public int Traffic_MenuSelectIndex
        {
            get { return _Traffic_MenuSelectIndex; }
            set
            {
                _Traffic_MenuSelectIndex = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                {
                    RaisePropertyChanged(() => Traffic_MenuSelectIndex);
                });
            }
        }
        public Visibility MenuShow
        {
            get
            {
                return Visibility.Visible;
            }
        }

        public string ManagerVisibility { get; set; }
        /// <summary>
        /// Module currently selected
        /// </summary>
        private TrafficFeature _curSelectParme = TrafficFeature.Traffic_NoFeature;
        /// <summary>
        /// Menu is selected, switch views
        /// </summary>
        /// <param name="paramter"></param>
        private void Selected(object paramter)
        {
            if (paramter == null)
                return;
            TrafficFeature strParamter = (TrafficFeature)(Convert.ToInt32(paramter));
            ///fence
            if (strParamter == TrafficFeature.Traffic_PolygonFence)
            {
                if (strParamter != _curSelectParme)
                    EventAggregator.Publish<ClearTrafficMaps>(new ClearTrafficMaps() { nType = TrafficFeature.Traffic_PolygonFence });
                if (FenceSourePage == null || FenceSourePage.ItemCount == 0)
                    QueryFence();

                SelectItem = SelectItem;
                if (SelectItem != null)
                    SelectItem.IsSelect = true;
            }
            else if (strParamter == TrafficFeature.Traffic_Route)
            {
                if (strParamter != _curSelectParme)
                    EventAggregator.Publish<ClearTrafficMaps>(new ClearTrafficMaps() { nType = TrafficFeature.Traffic_Route });

                if (RouteSourePage == null || RouteSourePage.ItemCount == 0)
                    QueryRoute();

                RouteSelectItem = RouteSelectItem;
                if (RouteSelectItem != null)
                    RouteSelectItem.IsSelect = true;
            }
            ///SpeedRule
            else if (strParamter == TrafficFeature.Traffic_SpeedLimit)
            {
                if (strParamter != _curSelectParme)
                    EventAggregator.Publish<ClearTrafficMaps>(new ClearTrafficMaps() { nType = TrafficFeature.Traffic_SpeedLimit });

                if (SpeedRuleSourcePage == null || SpeedRuleSourcePage.ItemCount == 0)
                    QuerySpeedRule();
            }

            // notify detail page to update
            EventAggregator.Publish<TrafficFeature>(strParamter);

            _curSelectParme = strParamter;
        }
        #endregion


        #region Initialization
        /// <summary>
        /// active view
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="viewParameters"></param>
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            EventAggregator.Publish<GisDisplayControlEvent>(
            new GisDisplayControlEvent()
            {
                Display = GisDisplayControlType.miTraffic
            });
            if (string.IsNullOrEmpty(selectedname))
            {
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-03-01-01"))
                {
                    selectedname = "MenuItem_Fence";
                    Selected(TrafficFeature.Traffic_PolygonFence);
                }
                else if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-03-01-02"))
                {
                    selectedname = "MenuItem_Route";
                    Selected(TrafficFeature.Traffic_Route);
                }
                else if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-03-01-02-02"))
                {
                    selectedname = "MenuItem_SpeedRule";
                    Selected(TrafficFeature.Traffic_SpeedLimit);
                }
            }
        }

        /// <summary>
        /// Select the menu command
        /// </summary>
        public IActionCommand SelectedCommand { get; private set; }
        /// <summary>
        /// Constructors
        /// </summary>
        public TrafficMenuVm()
        {
            try
            {
                SelectedCommand = new ActionCommand<object>(obj => Selected(obj));
                OpenDetailViewCommand = new ActionCommand<object>((obj) => OpenDetailViewClick_Event(obj));
                Traffic_MenuSelectIndex = 0;

                InitialFence();
                InitTrafficRoute();
                InitSpeedRule();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("TrafficMenuVm", ex);
            }
        }

        #endregion

        #region Subscribe to message
        /// <summary>
        /// Subscribe to message
        /// </summary>
        public void OnImportsSatisfied()
        {
            //EventAggregator.SubscribeOnDispatcher<RefreshTrafficManagerListArgs>(this);
            //EventAggregator.SubscribeOnDispatcher<ReturnRoutePts>(this);
            EventAggregator.SubscribeOnDispatcher<RefreshTrafficSelectStatus>(this);
            EventAggregator.SubscribeOnDispatcher<RefreshRouteSelectStatus>(this);
            //EventAggregator.SubscribeOnDispatcher<UpdateTrafficMarkArgs>(this);
            EventAggregator.SubscribeOnDispatcher<AddFenceCompleteArgs>(this);
            EventAggregator.SubscribeOnDispatcher<AddRouteCompleteArgs>(this);
        }
        #endregion

        #region Travel plan
        /// <summary>
        /// update plot of the plan
        /// </summary>
        /// <param name="publishedEvent"></param>
        //public void HandleEvent(UpdateTrafficMarkArgs publishedEvent)
        //{
        //    //
        //    if (publishedEvent.nType == TrafficFeature.Traffic_PolygonFence)
        //    {
        //        if (SelectItem == null)
        //            return;
        //        if (SelectItem.IsmarkFenceGraphic == true)
        //        {
        //            EventAggregator.Publish<MarkTrafficGraphic>(new MarkTrafficGraphic() { nType = TrafficFeature.Traffic_PolygonFence, bShow = false, MarkSymbolParm = null });
        //            SymbolParams parm = GetSymbolParm(MarkType.markFence, SelectItem.ID.ToString());
        //            if (parm == null)
        //                return;
        //            EventAggregator.Publish<MarkTrafficGraphic>(new MarkTrafficGraphic() { nType = TrafficFeature.Traffic_PolygonFence, bShow = true, MarkSymbolParm = parm });
        //        }
        //    }
        //}
        #endregion
        #region Receive messages of other modules
        /// <summary>
        /// Refresh selected state
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(RefreshTrafficSelectStatus publishedEvent)
        {
            if (SelectItem != null && _curSelectParme == TrafficFeature.Traffic_PolygonFence)
                SelectItem.IsSelect = true;
        }
        /// <summary>
        /// windows refresh
        /// </summary>
        /// <param name="publishedEvent"></param>
        //public void HandleEvent(RefreshTrafficManagerListArgs publishedEvent)
        //{
        //    if (publishedEvent.nType == TrafficFeature.Traffic_PolygonFence)
        //    {
        //        if (publishedEvent.bReQuery)
        //        {
        //            QueryFence();
        //        }
        //        else
        //        {
        //            TrafficFence fence = publishedEvent.UpdateItemInfo as TrafficFence;
        //            if (SelectItem != null && fence != null)
        //            {
        //                SelectItem.Name = fence.Name;
        //                SelectItem.Address = fence.Address;
        //                //  SelectItem.AlertType = fence.AlertType;
        //                SelectItem.Pts = fence.Pts;
        //                //  SelectItem.SpeedLimit = fence.SpeedLimit;
        //                //  SelectItem.TimeLimit = fence.TimeLimit;
        //                SelectItem.CreateTime = fence.CreateTime;
        //                SelectItem.Creator = fence.Creator;
        //                SelectItem.IsSelect = true;
        //            }
        //        }
        //    }
        //}
        #endregion
        #region Double-click interface to display detailed information
        /// <summary>
        /// show right info window
        /// </summary>
        /// <param name="obj"></param>
        internal void OpenDetailViewClick_Event(object obj)
        {
            EventAggregator.Publish<ShowTrafficeMangerDetailInfoArgs>(new ShowTrafficeMangerDetailInfoArgs() { bShow = true });
        }
        #endregion

        #region Has plot object

        private enum MarkType
        {
            markFence = 0,
            markRoute = 1,
            markLongRoute = 2,
            markStop = 3,
            markStopRout = 4,
            markStopLongRoute = 5,
            markStopScheDule = 6
        }
        /// <summary>
        /// plot object
        /// </summary>
        private class MarkElements
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
        /// <summary>
        /// has plot object list
        /// </summary>
        private List<MarkElements> _HasMarkElements = new List<MarkElements>();
        /// <summary>
        /// update list
        /// </summary>
        /// <param name="bMark"></param>
        /// <param name="nType"></param>
        /// <param name="strOID"></param>
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
        ///// <summary>
        ///// get symbol
        ///// </summary>
        ///// <param name="nType"></param>
        ///// <param name="strOID"></param>
        ///// <returns></returns>
        //private SymbolParams GetSymbolParm(MarkType nType, string strOID)
        //{
        //    for (int i = 0; i < _HasMarkElements.Count; i++)
        //    {
        //        if (_HasMarkElements[i].OBJECTID == strOID && _HasMarkElements[i].TYPE == nType)
        //        {
        //            return _HasMarkElements[i].parm;
        //        }
        //    }
        //    return null;
        //}

        #endregion


        ObservableCollection<MenuInfo> _ManagerMenuItems = null;
        public ObservableCollection<MenuInfo> SpeedMenuItems
        {
            get { return GetMenuInfo(TrafficName.SpeedMenuName); }
        }

        private ObservableCollection<MenuInfo> GetMenuInfo(string SubMenuName)
        {
            if (_ManagerMenuItems == null || _ManagerMenuItems.Count == 0)
                return null;
            var result = _ManagerMenuItems.Where(item => item.SubMenuType.Equals(SubMenuName)).OrderBy(item => item.Order);
            if (result == null || result.Count() == 0)
                return null;
            ObservableCollection<MenuInfo> menuItems = new ObservableCollection<MenuInfo>();
            foreach (var item in result)
            {
                menuItems.Add(item);
            }
            return menuItems;
        }

        protected override void InitializeVm()
        {
            base.InitializeVm();
            if (_ManagerMenuItems == null || _ManagerMenuItems.Count == 0)
            {
                _ManagerMenuItems = ApplicationContext.Instance.MenuManager.GetNavigateInfos(Router, TrafficName.CategoryName);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                {
                    RaisePropertyChanged(() => SpeedMenuItems);
                });
            }
        }

        public void HandleEvent(AddFenceCompleteArgs publishedEvent)
        {
            QueryFence();
        }

        string selectedname = string.Empty;
        public void Accordion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Accordion accordion = sender as Accordion;
            object obj = accordion.SelectedItem;
            if (obj == null)
            {
                return;
            }

            AccordionItem item = obj as AccordionItem;
            selectedname = item.Name;

            if (selectedname == "MenuItem_Fence")
            {
                if (_curSelectParme != TrafficFeature.Traffic_PolygonFence)
                {
                    EventAggregator.Publish<ClearTrafficMaps>(new ClearTrafficMaps() { nType = TrafficFeature.Traffic_PolygonFence });
                    if (FenceSourePage == null || FenceSourePage.ItemCount == 0)
                        QueryFence();

                    SelectItem = SelectItem;
                    if (SelectItem != null)
                        SelectItem.IsSelect = true;

                    EventAggregator.Publish<TrafficFeature>(TrafficFeature.Traffic_PolygonFence);
                    _curSelectParme = TrafficFeature.Traffic_PolygonFence;
                }
            }
            else if (selectedname == "MenuItem_Route")
            {
                if (_curSelectParme != TrafficFeature.Traffic_Route)
                {
                    EventAggregator.Publish<ClearTrafficMaps>(new ClearTrafficMaps() { nType = TrafficFeature.Traffic_Route });

                    if (RouteSourePage == null || RouteSourePage.ItemCount == 0)
                        QueryRoute();

                    RouteSelectItem = RouteSelectItem;
                    if (RouteSelectItem != null)
                        RouteSelectItem.IsSelect = true;

                    EventAggregator.Publish<TrafficFeature>(TrafficFeature.Traffic_Route);
                    _curSelectParme = TrafficFeature.Traffic_Route;
                }
            }
            else if (selectedname == "MenuItem_SpeedRule")
            {
                if (_curSelectParme != TrafficFeature.Traffic_SpeedLimit)
                {
                    EventAggregator.Publish<ClearTrafficMaps>(new ClearTrafficMaps() { nType = TrafficFeature.Traffic_SpeedLimit });

                    if (SpeedRuleSourcePage == null || SpeedRuleSourcePage.ItemCount == 0)
                        QuerySpeedRule();

                    EventAggregator.Publish<TrafficFeature>(TrafficFeature.Traffic_SpeedLimit);
                    _curSelectParme = TrafficFeature.Traffic_SpeedLimit;
                }
            }
        }
    }

    #region Comparison class for distict list
    /// <summary>
    /// Compare fencing class
    /// </summary>
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
    /// <summary>
    /// Comparison class speed
    /// </summary>
    public class SpeedLimitCompare : IEqualityComparer<SpeedLimit>
    {
        public bool Equals(SpeedLimit x, SpeedLimit y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(SpeedLimit s)
        {
            return s.ID.GetHashCode();
        }
    }

    #endregion
}
