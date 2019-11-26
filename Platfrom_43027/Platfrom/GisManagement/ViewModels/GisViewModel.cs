/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ab5e5a98-d0ba-4f3c-9de3-101084add563      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: GisManagement.ViewModels
/////    Project Description:    
/////             Class Name: GisViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/5 13:36:36
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/5 13:36:36
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.ServiceReference.DistrictService;
using ESRI.ArcGIS.Client.Tasks;
using System.Xml;
using Gsafety.Common.CommMessage;
using ESRI.ArcGIS.Client.Symbols;
using Gsafety.PTMS.Bases.Models;
using Gsafety.Common.Converts;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.ServiceReference.VehicleMonitorService;
using Common;
using System.Windows.Interactivity;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.Common.CommMessage.Controls;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using GisManagement.Models;
using GisManagement.Views;
using Gsafety.PTMS.ServiceReference.MessageService;
using Jounce.Core.Command;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using Jounce.Core.View;
using Jounce.Framework;
using Jounce.Framework.Command;
using BaseLib.ViewModels;
using Gsafety.PTMS.ServiceReference.RunVehicleLocationService;
using Gsafety.Common.Controls;
using System.Reflection;

namespace GisManagement.ViewModels
{
    [ExportAsViewModel(GisName.GisViewModel)]
    public partial class GisViewModel : BaseViewModel,
        IEventSink<LocateEventArgs>,
        IEventSink<RefreshEvent>,
        IEventSink<LocateGeometryEventArgs>,
        IEventSink<EditGeometryArgs>,
        IEventSink<DrawFenceEventArgs>,
        IEventSink<ShowFenceInfoArgs>,
        IEventSink<DeleteFenceArgs>,
        IEventSink<DrawRoutEventArgs>,
        IEventSink<ClearTrafficMaps>,
        IEventSink<UpdateStopArgs>,
        IEventSink<GisDisplayControlEvent>,
        IEventSink<ShowBusStopInfoArgs>,
        IEventSink<DeleteRouteArgs>,
        IEventSink<DeleteStopArgs>,
        IEventSink<MarkTrafficGraphic>,
        IEventSink<ClearTrafficFeaturelayer>,
        //IEventSink<GenerateFacilityThememapArgs>,
        IEventSink<MarkStopsByRoutID>,
        IEventSink<OpenCarHisViewState>,
        IEventSink<ZoomToDistByDistCode>,
        IEventSink<GetPointFrommap>,
        //IEventSink<MarkTrafficScheDulePointArgs>,
        //IEventSink<MarkStopScheDuleArgs>,
        //IEventSink<UpdateTrafficMarkArgs>,
        IEventSink<DisplayHistoricalRoute>,
        IEventSink<ZoomGisView>,
        IPartImportsSatisfiedNotification
    {
        #region define
        /// <summary>
        /// tool bar
        /// </summary>
        private enum ToolTypes { None, miZoomIn, miZoomOut, miPan, miSearchByPoint, miSearchByRect, miSearchByPolygon, miSearchByBuffer }


        GraphicsLayer mygraphicsLayer;
        /// <summary>
        /// Define geometric objects plotted
        /// </summary>
        Graphic currentGraphic;
        /// <summary>
        /// get map
        /// </summary>
        private ESRI.ArcGIS.Client.Map _MyMap;

        private bool _IsCarHisOpen;

        private bool _IsCarHisVisual;

        public bool IsCarHisVisual
        {
            get { return _IsCarHisVisual; }
            set
            {
                if (_IsCarHisVisual != value)
                {
                    _IsCarHisVisual = value;
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsCarHisVisual));
                }

            }
        }

        public bool IsCarHisOpen
        {
            get { return _IsCarHisOpen; }
            set
            {
                _IsCarHisVisual = _IsCarHisOpen = value;

            }
        }
        /// <summary>
        /// get map
        /// </summary>


        private List<ElementLayerDefine> _currentVisibleLst;
        Graphic graphic;
        TimeSpan elapsedTime;
        DateTime? startTime;
        #endregion

        #region GetMyMap
        public ESRI.ArcGIS.Client.Map MyMap
        {
            get
            {
                if (_MyMap == null)
                {
                    object mview = Router.ViewQuery(GisName.MonitorGisView);
                    if (mview != null)
                    {
                        _MyMap = (mview as GisView).MyMap;
                    }
                    (mview as GisView).MyMap.MouseMove += MyMap_MouseMove;
                    (mview as GisView).MyMap.MouseLeftButtonUp += MyMap_MouseLeftButtonUp;
                    if (_MyMap != null)
                    {
                        GraphicsLayer gGpsGraphicLayer = _MyMap.Layers[ConstDefine.GpsHisDataGraphicsLayerName] as GraphicsLayer;
                        if (gGpsGraphicLayer != null)
                        {
                            gGpsGraphicLayer.MouseLeftButtonDown += GpsHisGraphicsLayer_MouseLeftButtonDown;
                        }
                    }
                }
                return _MyMap;
            }

        }
        public void GpsHisGraphicsLayer_MouseLeftButtonDown(object sender, GraphicMouseButtonEventArgs e)
        {
            if (graphic == null || graphic != e.Graphic)
            {
                graphic = e.Graphic;
                startTime = DateTime.Now;
            }
            else if (graphic == e.Graphic && startTime.HasValue && e.Graphic != null && e.Graphic.Geometry != null)
            {
                elapsedTime = DateTime.Now.Subtract(startTime.Value);
                if (elapsedTime.TotalMilliseconds < 500)
                {
                    if (e.Graphic != null)
                    {
                        if (e.Graphic.Geometry is MapPoint)
                        {
                            MapPoint pt = e.Graphic.Geometry as MapPoint;

                            EventAggregator.Publish<ShowGpsHisSinglePointInfo>(new ShowGpsHisSinglePointInfo() { PX = pt.X, PY = pt.Y, Visble = false });
                        }
                    }
                    return;
                }
                else
                {
                    if (e.Graphic != null)
                    {
                        if (e.Graphic.Geometry is MapPoint)
                        {
                            MapPoint pt = e.Graphic.Geometry as MapPoint;

                            EventAggregator.Publish<ShowGpsHisSinglePointInfo>(new ShowGpsHisSinglePointInfo() { PX = pt.X, PY = pt.Y, Visble = true });
                        }
                    }
                }

                graphic = null;
                startTime = null;
            }

        }
        private void MyMap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_bEditPoint == true)
            {
                _bEditPoint = false;
            }

        }
        private void MyMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (_bEditPoint == true)
            {
                if (_bEditPointGraphic != null)
                {
                    _bEditPointGraphic.Geometry = MyMap.ScreenToMap(e.GetPosition(MyMap));
                }
            }
        }
        #endregion

        #region operate extent
        private Dictionary<GisDisplayControlType, Envelope> _currentExtentDict = new Dictionary<GisDisplayControlType, Envelope>();

        private void DisplayControlFilterByGroup(GisDisplayControlType cscurrentDisplay, string groupname)
        {
            //only display gpscar which group equal groupname
            //miVEGpsData = 1, 
            try
            {
                if ((groupname == "") || (groupname == null)) return;
                switch (cscurrentDisplay)
                {
                    case GisDisplayControlType.miMonitor_RealTime:
                        {
                            foreach (GpsCar car in MonitorList.VechileRealLocationElements.Elements)
                            {
                                car.DisplayGroupName = groupname;
                            }
                            break;
                        }
                    case GisDisplayControlType.miMonitor_Alert:
                        {
                            foreach (GpsCar car in MonitorList.AlertHappenLocationElements.Elements)
                            {
                                car.DisplayGroupName = groupname;
                            }
                            break;
                        }
                    case GisDisplayControlType.miHistory_Alert:
                        {
                            foreach (GpsCar car in MonitorList.AlertHappenLocationElements.Elements)
                            {
                                car.DisplayGroupName = groupname;
                            }
                            break;
                        }
                    case GisDisplayControlType.miMonitor_Alarm:
                        {
                            foreach (GpsCar car in MonitorList.AlarmHappenLocationElements.Elements)
                            {
                                car.DisplayGroupName = groupname;
                            }
                            break;
                        }
                    case GisDisplayControlType.miHistory_Alarm:
                        {
                            foreach (GpsCar car in MonitorList.AlarmHappenLocationElements.Elements)
                            {
                                car.DisplayGroupName = groupname;
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        private Envelope _currentExtent;
        private Envelope CurrentExtent
           {
               get
               {
                   return _currentExtent;
               }
               set
               {

                   _currentExtent = value;
               }
           }


        private GisDisplayControlType _currentDisplay;
        private GisDisplayControlType currentDisplay
        {
            get
            {
                return _currentDisplay;
            }
            set
            {
                //Save the current vision
                SaveCurrentExtent(_currentDisplay, MyMap.Extent);
                _currentDisplay = value;

                getCurrentExtent(_currentDisplay);

                GraphicsLayer layer = MyMap.Layers[ConstDefine.TrafficFeatureFenceLayerName] as GraphicsLayer;
                layer.Graphics.Clear();
                layer = MyMap.Layers[ConstDefine.TrafficFeatureRouteLayerName] as GraphicsLayer;
                layer.Graphics.Clear();


               // GisManagement.ViewModels.MonitorList.FenceGraphicHelp.Clear();
                //Return to the original vision
                switch (_currentDisplay)
                {
                    case GisDisplayControlType.miMonitor_RealTime:
                        {
                            _currentVisibleLst = _RealTimeElementDisplayLst;

                            if (_currentExtent != null)
                            {
                                MyMap.Extent = _currentExtent;
                            }
                            break;
                        }

                    case GisDisplayControlType.miMonitor_Alarm:
                        {
                            _currentVisibleLst = _MonitorAlarmElementDisplayLst;
                            if (_currentExtent != null)
                            {
                                MyMap.Extent = _currentExtent;
                            }
                            break;
                        }
                    case GisDisplayControlType.miHistory_Alarm:
                        {
                            _currentVisibleLst = _HistoryAlarmElementDisplayLst;
                            break;
                        }
                    case GisDisplayControlType.miMonitor_Alert:
                        {
                            _currentVisibleLst = _MonitorAlertElementDisplayLst;
                            if (_currentExtent != null)
                            {
                                MyMap.Extent = _currentExtent;
                            }
                            break;
                        }
                    case GisDisplayControlType.miHistory_Alert:
                        {
                            _currentVisibleLst = _HistoryAlertElementDisplayLst;
                            break;
                        }
                    case GisDisplayControlType.miTraffic:
                        {
                            _currentVisibleLst = _TrafficElementDisplayLst;
                            break;
                        }
                }

                //miVERealLocation  
                if (_currentVisibleLst.IndexOf(ElementLayerDefine.miVERealLocation) > -1)
                {
                    MyMap.Layers[ConstDefine.RealLocationElementLayerName].Visible = true;
                    MyMap.Layers[ConstDefine.RealLocationGraphicsLayerName].Visible = true;
                    SwtichEditGeometryTool();
                }
                else
                {
                    MyMap.Layers[ConstDefine.RealLocationElementLayerName].Visible = false;
                    MyMap.Layers[ConstDefine.RealLocationGraphicsLayerName].Visible = false;
                }

                //miVEAlarmHappenLocation  
                if (_currentVisibleLst.IndexOf(ElementLayerDefine.miVEAlarmHappenLocation) > -1)
                {
                    MyMap.Layers[ConstDefine.AlarmHappenLocationElementLayerName].Visible = true;
                    MyMap.Layers[ConstDefine.AlarmHappenLocationGraphicsLayerName].Visible = true;
                    SwtichEditGeometryTool();
                }
                else
                {
                    MyMap.Layers[ConstDefine.AlarmHappenLocationElementLayerName].Visible = false;
                    MyMap.Layers[ConstDefine.AlarmHappenLocationGraphicsLayerName].Visible = false;
                }

                //miVEAlertHappenLocation 
                if (_currentVisibleLst.IndexOf(ElementLayerDefine.miVEAlertHappenLocation) > -1)
                {
                    MyMap.Layers[ConstDefine.AlertHappenLocationElementLayerName].Visible = true;
                    MyMap.Layers[ConstDefine.AlertHappenLocationGraphicsLayerName].Visible = true;
                    SwtichEditGeometryTool();
                }
                else
                {
                    MyMap.Layers[ConstDefine.AlertHappenLocationElementLayerName].Visible = false;
                    MyMap.Layers[ConstDefine.AlertHappenLocationGraphicsLayerName].Visible = false;
                }

                //miVETraffic
                if (_currentVisibleLst.IndexOf(ElementLayerDefine.miVETraffic) > -1)
                {
                    MyMap.Layers[ConstDefine.TrafficFeatureStopLayerName].Visible = true;
                    MyMap.Layers[ConstDefine.TrafficFeatureRouteLayerName].Visible = true;
                    MyMap.Layers[ConstDefine.TrafficFeatureFenceLayerName].Visible = true;
                    MyMap.Layers[ConstDefine.TrafficGraphicLayerName].Visible = true;
                }
                else
                {
                    MyMap.Layers[ConstDefine.TrafficFeatureStopLayerName].Visible = false;
                    MyMap.Layers[ConstDefine.TrafficFeatureRouteLayerName].Visible = false;
                    MyMap.Layers[ConstDefine.TrafficFeatureFenceLayerName].Visible = false;
                    MyMap.Layers[ConstDefine.TrafficGraphicLayerName].Visible = false;
                }
            }
        }

        private void SaveCurrentExtent(GisDisplayControlType _currentDisplay, Envelope envelope)
        {
            switch (_currentDisplay)
            {
                case GisDisplayControlType.miMonitor_RealTime:
                    {
                        _currentExtent = MyMap.Extent;
                        break;
                    }

                case GisDisplayControlType.miMonitor_Alarm:
                    {
                        _currentExtent = MyMap.Extent;
                        break;
                    }

                case GisDisplayControlType.miMonitor_Alert:
                    {
                        _currentExtent = MyMap.Extent;
                        break;
                    }
            }

            if (_currentExtentDict.ContainsKey(_currentDisplay))
            {
                _currentExtentDict[_currentDisplay] = MyMap.Extent;
            }
            else
            {
                _currentExtentDict.Add(_currentDisplay, MyMap.Extent);
            }
        }

        public static Envelope getExtent(string str)
        {
            try
            {
                if ((str != null) && (str != ""))
                {
                    string[] temp = str.Split(";".ToCharArray());
                    if (temp.Length < 4) return null;
                    DisplayLonConvert loncon = new DisplayLonConvert();
                    DisplayLatConvert latcon = new DisplayLatConvert();
                    double xmin = double.Parse(loncon.ConvertBack(temp[0], null, null, null).ToString());
                    double ymin = double.Parse(latcon.ConvertBack(temp[1], null, null, null).ToString());
                    double xmax = double.Parse(loncon.ConvertBack(temp[2], null, null, null).ToString());
                    double ymax = double.Parse(latcon.ConvertBack(temp[3], null, null, null).ToString());
                    //3857地图
                    ESRI.ArcGIS.Client.Geometry.MapPoint pt1 = GpsCarListViewModel.GetProjCoord(xmin, ymin);
                    ESRI.ArcGIS.Client.Geometry.MapPoint pt2 = GpsCarListViewModel.GetProjCoord(xmax, ymax);
                    //ESRI.ArcGIS.Client.Geometry.MapPoint pt1 = new ESRI.ArcGIS.Client.Geometry.MapPoint(xmin, ymin);
                    //ESRI.ArcGIS.Client.Geometry.MapPoint pt2 = new ESRI.ArcGIS.Client.Geometry.MapPoint(xmax, ymax);
               


                    return new Envelope(pt1, pt2);
                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GisViewModel", ee);
            }
            return null;
        }

        private void getCurrentExtent(GisDisplayControlType _currentDisplay)
        {
            if (_currentExtentDict.ContainsKey(_currentDisplay))
            {
                MyMap.Extent = _currentExtentDict[_currentDisplay];
            }
            else
            {
                MyMap.Extent = getExtent(ApplicationContext.Instance.ServerConfig.MapInitExtent);
            }
        }
        #endregion



        /// <summary>
        /// Remove the trolley
        /// </summary>
        public IActionCommand RemoveCarCommand { get; private set; }
        /// <summary>
        /// Save the edited geographic features command
        /// </summary>
        public IActionCommand SaveEditGeometryCommand { get; private set; }
        /// <summary>
        /// Cancel edit geographic feature editing commands
        /// </summary>
        public IActionCommand CancelEditGeometryCommand { get; private set; }
        /// <summary>
        /// Redo edit geographic feature editing commands
        /// </summary>
        public IActionCommand RedoEditGeometryCommand { get; private set; }
        /// <summary>
        /// Undo edit geographic feature editing commands
        /// </summary>
        public IActionCommand UndoEditGeometryCommand { get; private set; }

        public IActionCommand LocationCommand { get; set; }
        /// <summary>
        /// Query the map command
        /// </summary>
        public IActionCommand QuerySelectCommand { get; set; }
        /// <summary>
        /// Refresh a thematic map
        /// </summary>
        // public IActionCommand RefreshTheme1Command { get; set; }
        //private void _RefreshTheme1()
        //{
        //    try
        //    {
        //        FeatureLayer pFeaLayer = MyMap.Layers[ConstDefine.TrafficFeatureStopLayerName] as FeatureLayer;
        //        if (pFeaLayer != null)
        //        {
        //            pFeaLayer.Graphics.Clear();
        //            //   QueryTask queryStopsTask = new QueryTask("http://172.16.20.25:6080/arcgis/rest/services/testStops/FeatureServer/0");
        //            QueryTask queryStopsTask = new QueryTask(pFeaLayer.Url);
        //            queryStopsTask.ExecuteCompleted += queryStopsTask_ExecuteCompleted;
        //            queryStopsTask.Failed += queryStopsTask_Failed;
        //            Query query = new Query();
        //            query.Where = "1=1";
        //            query.ReturnGeometry = true;
        //            queryStopsTask.ExecuteAsync(query);
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        ApplicationContext.Instance.Logger.LogException("_RefreshTheme1", ee);
        //    }
        //    // throw new NotImplementedException();
        //}
        public void _QueryCommandFunc(string s)
        {
            try
            {
                //if (currentGraphic != null)
                //    mygraphicsLayer.Graphics.Remove(currentGraphic);
                ApplicationContext.Instance.CurrentDrawArgs = null;
                if (QueryDraw == null)
                {
                    QueryDraw = new ESRI.ArcGIS.Client.Draw(this.MyMap);
                }
                else
                    QueryDraw.IsEnabled = true;

                if (s == "0")
                    QueryDraw.DrawMode = DrawMode.Polygon;
                if (s == "1")
                    QueryDraw.DrawMode = DrawMode.Rectangle;
                if (s == "2")
                    QueryDraw.DrawMode = DrawMode.Circle;

                QueryDraw.FillSymbol = new SimpleFillSymbol()
                {
                    BorderBrush = new SolidColorBrush(Colors.Red) { },
                    BorderThickness = 2,
                    Fill = new SolidColorBrush(Colors.Green) { Opacity = 0.5 }
                };
                QueryDraw.IsEnabled = true;
                QueryDraw.DrawComplete += QueryDraw_DrawComplete;
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("_QueryCommandFunc", ee);
            }
        }
        /// <summary>
        /// Switching module when switching to edit shapes (temporarily stop）
        /// </summary>
        private void SwtichEditGeometryTool()
        {
            //if (EditGeometryVisble == Visibility.Visible)
            //    EditGeometryVisble = Visibility.Collapsed;
            if (_pEditGeometry != null)
            {
                object mview = Router.ViewQuery(GisName.MonitorGisView);
                if ((mview as GisView)._bEditDrawGeometry == false)
                    _pEditGeometry.CancelEdit();
            }
        }
        private void QueryDraw_DrawComplete(object sender, DrawEventArgs e)
        {
            try
            {
                ApplicationContext.Instance.CurrentDrawArgs = e.Geometry;
                ESRI.ArcGIS.Client.Graphic graphic = new ESRI.ArcGIS.Client.Graphic()
                {
                    Geometry = e.Geometry,
                    Symbol = new SimpleFillSymbol()
                    {
                        BorderBrush = new SolidColorBrush(Colors.Red) { },
                        BorderThickness = 2,
                        Fill = new SolidColorBrush(Colors.Green) { Opacity = 0.5 }
                    }
                };
                mygraphicsLayer = MyMap.Layers[ConstDefine.MyDrawQueryGraphicsLayer] as GraphicsLayer;
                mygraphicsLayer.Graphics.Clear();
                currentGraphic = graphic;
                mygraphicsLayer.Graphics.Add(graphic);

                GoToVisualState("showQueryPanel", true);

                object mview = Router.ViewQuery(GisName.MonitorGisView);
                ToggleButton toggleButton = (mview as UserControl).FindName("MapQueryBtn") as ToggleButton;
                toggleButton.IsChecked = true;
                QueryDraw.IsEnabled = false;
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("QueryDraw_Final", ee);
            }
        }

        #region map locate
        /// <summary>
        /// locate point
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void LocatePoint(double dx, double dy)
        {
            if (MyMap == null) return;
            try
            {
                string strLocal = ApplicationContext.Instance.ServerConfig.AutoLocateResolution;
                strLocal = strLocal.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                if (MyMap.Resolution > Convert.ToDouble(strLocal))
                {
                    CenterAndZoom(Convert.ToDouble(strLocal), dx, dy);
                }
                else
                {
                    MapPoint pPoint = new MapPoint();
                    pPoint.X = dx;
                    pPoint.Y = dy;
                    pPoint.SpatialReference = _MyMap.SpatialReference;
                    MyMap.PanTo(pPoint);
                }

            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GisViewModel", ee);
            }
        }
        /// <summary>
        /// With a vision Location Map
        /// </summary>
        /// <param name="myResolution"></param>
        /// <param name="myMapPoint"></param>
        public void CenterAndZoom(double myResolution, double dx, double dy)
        {
            try
            {
                double ratio = 1.0;
                if (MyMap.Resolution != 0.0)
                {
                    ratio = myResolution / MyMap.Resolution;
                }
                MapPoint pPoint = new MapPoint();
                pPoint.X = dx;
                pPoint.Y = dy;
                pPoint.SpatialReference = _MyMap.SpatialReference;
                if (ratio == 1.0)
                {
                    MyMap.PanTo(pPoint);
                }
                else
                {
                    ESRI.ArcGIS.Client.Geometry.Envelope myEnvelope = MyMap.Extent;

                    ESRI.ArcGIS.Client.Geometry.MapPoint pt = myEnvelope.GetCenter();

                    double x = (pPoint.X - ratio * pt.X) / (1 - ratio);
                    double y = (pPoint.Y - ratio * pt.Y) / (1 - ratio);
                    ESRI.ArcGIS.Client.Geometry.MapPoint newpt = new ESRI.ArcGIS.Client.Geometry.MapPoint(x, y);

                    MyMap.ZoomToResolution(myResolution, newpt);

                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("CenterAndZoom_Error", ee);
            }
        }
        /// <summary>
        /// locate to the object
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(LocateGeometryEventArgs publishedEvent)
        {
            if (MyMap == null) return;
            try
            {
                string strLocal = ApplicationContext.Instance.ServerConfig.AutoLocateResolution;
                strLocal = strLocal.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                if (MyMap.Resolution > Convert.ToDouble(strLocal))
                {
                    CenterAndZoom(Convert.ToDouble(strLocal), publishedEvent.LocateGeometry.Extent.GetCenter());
                }
                else
                {
                    MyMap.PanTo(publishedEvent.LocateGeometry.Extent.GetCenter());
                    MyMap.Extent = new ESRI.ArcGIS.Client.Geometry.Envelope(MyMap.Extent.XMin, MyMap.Extent.YMin, MyMap.Extent.XMax, MyMap.Extent.YMax);
                }

            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GisViewModel", ee);
            }
        }
        /// <summary>
        /// With a vision Location Map
        /// </summary>
        /// <param name="myResolution"></param>
        /// <param name="myMapPoint"></param>
        public void CenterAndZoom(double myResolution, ESRI.ArcGIS.Client.Geometry.MapPoint myMapPoint)
        {
            try
            {
                double ratio = 1.0;
                if (MyMap.Resolution != 0.0)
                {
                    ratio = myResolution / MyMap.Resolution;
                }

                if (ratio == 1.0)
                {
                    MyMap.Extent = MyMap.Extent.Expand(1.001);
                    MyMap.PanTo(myMapPoint);
                }
                else
                {
                    ESRI.ArcGIS.Client.Geometry.Envelope myEnvelope = MyMap.Extent;

                    ESRI.ArcGIS.Client.Geometry.MapPoint pt = myEnvelope.GetCenter();

                    double x = (myMapPoint.X - ratio * pt.X) / (1 - ratio);
                    double y = (myMapPoint.Y - ratio * pt.Y) / (1 - ratio);
                    ESRI.ArcGIS.Client.Geometry.MapPoint newpt = new ESRI.ArcGIS.Client.Geometry.MapPoint(x, y);

                    MyMap.ZoomToResolution(myResolution, newpt);

                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("CenterAndZoom_Error", ee);
            }
        }

        public void HandleEvent(DisplayHistoricalRoute publishedEvent)
        {
            HistoricalRoute window = new HistoricalRoute(publishedEvent.VechileId, HisGPSDataType.MonitorGPS, false, publishedEvent.StartTime, publishedEvent.EndTime);
            window.Closed += new EventHandler(HistoricalRoute_Closed);
            window.Show();
        }

        public ICommand ClearMapCommand { get; private set; }


        /// <summary>
        /// When the vehicle historical trajectory window closes, get the query parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HistoricalRoute_Closed(object sender, EventArgs e)
        {
            HistoricalRoute window = sender as HistoricalRoute;
            window.Closed -= HistoricalRoute_Closed;
            if (window != null && window.DialogResult == true)
            {
                EventAggregator.Publish<HisTraceArgs>(window.HistraceArgs);
            }
        }
        //Locate Distinct
        public void HandleEvent(ZoomToDistByDistCode publishedEvent)
        {
            try
            {
                //GraphicsLayer pFeaLayerDist = MyMap.Layers[ConstDefine.MarkDistGraphicsLayer] as GraphicsLayer;
                //if (pFeaLayerDist != null)
                //    pFeaLayerDist.Graphics.Clear();
                //string strURL = "";
                //if (ApplicationContext.Instance.ServerConfig.GisBaseMapUrl.LastIndexOf('/') < ApplicationContext.Instance.ServerConfig.GisBaseMapUrl.Length - 1)
                //{
                //    strURL = ApplicationContext.Instance.ServerConfig.GisBaseMapUrl + "/";
                //}
                //else
                //{
                //    strURL = ApplicationContext.Instance.ServerConfig.GisBaseMapUrl;
                //}

                //string strLayerID = ApplicationContext.Instance.ServerConfig.DistQueryGisID;
                //string[] listID = strLayerID.Split(';');
                //if (publishedEvent.DISTTYPE == ZoomToDistType.ZoomToCity)
                //{
                //    strURL += listID[0];
                //    QueryTask queryConntryTask = new QueryTask(strURL);
                //    queryConntryTask.ExecuteCompleted += queryConntryTask_ExecuteCompleted;
                //    queryConntryTask.Failed += queryConntryTask_Failed;
                //    Query query = new Query();
                //    query.Where = "COD_CAN = '" + publishedEvent.DISTCODE + "'";
                //    query.ReturnGeometry = true;
                //    queryConntryTask.ExecuteAsync(query);
                //}
                //if (publishedEvent.DISTTYPE == ZoomToDistType.ZoomToProvince)
                //{
                //    strURL += listID[1];
                //    QueryTask queryConntryTask = new QueryTask(strURL);
                //    queryConntryTask.ExecuteCompleted += queryConntryTask_ExecuteCompleted;
                //    queryConntryTask.Failed += queryConntryTask_Failed;
                //    Query query = new Query();
                //    query.Where = "DISTRICTCODE  = '" + publishedEvent.DISTCODE + "'";
                //    query.ReturnGeometry = true;
                //    queryConntryTask.ExecuteAsync(query);
                //}
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("ZoomToDistByDistCode_Error", ee);
            }
        }
        /// <summary>
        ///  Query provinces task failed   
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queryConntryTask_Failed(object sender, TaskFailedEventArgs e)
        {
            ApplicationContext.Instance.Logger.LogException("HandleEvent(ZoomToDistByDistCode publishedEvent)", e.Error);
        }
        /// <summary>
        /// Query counties Completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queryConntryTask_ExecuteCompleted(object sender, QueryEventArgs e)
        {
            try
            {
                if (e.FeatureSet == null)
                    return;
                if (e.FeatureSet.Features.Count == 0)
                    return;
                if (e.FeatureSet.Features[0].Geometry == null)
                    return;
                if (e.FeatureSet.Features[0].Geometry.Extent == null)
                    return;

                ESRI.ArcGIS.Client.Geometry.Geometry geo = e.FeatureSet.Features[0].Geometry;
                //Multiple words are combined
                if (e.FeatureSet.Features.Count > 1)
                {
                    ESRI.ArcGIS.Client.Geometry.Polygon unioGeo = new ESRI.ArcGIS.Client.Geometry.Polygon();
                    for (int i = 0; i < e.FeatureSet.Features.Count; i++)
                    {
                        ESRI.ArcGIS.Client.Geometry.Polygon polygonTemp = e.FeatureSet.Features[i].Geometry as ESRI.ArcGIS.Client.Geometry.Polygon;
                        if (polygonTemp != null)
                        {
                            for (int k = 0; k < polygonTemp.Rings.Count; k++)
                            {
                                unioGeo.Rings.Add(polygonTemp.Rings[k]);
                            }
                        }
                    }
                    geo = unioGeo as ESRI.ArcGIS.Client.Geometry.Geometry;
                }
                if (geo == null)
                    return;
                MyMap.ZoomTo(geo);
                Graphic g = new Graphic();
                g.Geometry = geo;
                g.Symbol = new SimpleFillSymbol()
                {
                    BorderBrush = new SolidColorBrush(Colors.Red) { },
                    BorderThickness = 2,
                    Fill = new SolidColorBrush(Colors.Red) { Opacity = 0 }
                };

                GraphicsLayer pFeaLayerDist = MyMap.Layers[ConstDefine.MarkDistGraphicsLayer] as GraphicsLayer;
                if (pFeaLayerDist != null)
                    pFeaLayerDist.Graphics.Add(g);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        #endregion

        #region Edit geometry
        #region Edit geometry
        private double _nUndoCount = 0;
        private double _nEditCount = 0;
        private double _nRedoCount = 0;

        private bool _RedoEditEnabled()
        {
            if (_nUndoCount <= 0)
                return false;
            else
                return true;
        }

        private bool _UndoEditEnabled()
        {
            if (_nEditCount <= 0)
                return false;
            else
                return true;
        }
        /// <summary>
        /// Undo edit
        /// </summary>
        private void _UndoEditGeometry()
        {
            if (_pEditGeometry != null)
            {
                _pEditGeometry.UndoLastEdit();
            }
        }
        /// <summary>
        /// redo edit
        /// </summary>
        private void _RedoEditGeometry()
        {
            if (_pEditGeometry != null)
            {
                _pEditGeometry.RedoLastEdit();
            }
        }
        /// <summary>
        /// cancel edit
        /// </summary>
        private void _CancelEditGeometry()
        {
            _nEditCount = 0;
            _nRedoCount = 0;
            _nUndoCount = 0;

            //if (EditGeometryVisble == Visibility.Visible)
            //    EditGeometryVisble = Visibility.Collapsed;
            try
            {
                if (_pEditGeometry != null)
                {
                    _pEditGeometry.CancelEdit();
                }
                if (_bDrawFence == true)
                {
                    GraphicsLayer pGraphicLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                    if (pGraphicLayer != null)
                    {
                        pGraphicLayer.Graphics.Clear();
                    }
                    _bDrawFence = false;
                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("_CancelEditGeometry_Error", ee);
            }
        }
        /// <summary>
        /// save edit
        /// </summary>
        public void _SaveEditGeometry()
        {
            _nEditCount = 0;
            _nRedoCount = 0;
            _nUndoCount = 0;
            //if (EditGeometryVisble == Visibility.Visible)
            //    EditGeometryVisble = Visibility.Collapsed;
            try
            {
                if (_pEditGeometry != null)
                {
                    _pEditGeometry.StopEdit();
                }

                if (_editGeometrypublishedEvent == null) return;
                if (_editGeometrypublishedEvent.nType == TrafficFeature.Traffic_PolygonFence)
                {                 
                    GraphicsLayer pLayerFence = MyMap.Layers[ConstDefine.TrafficFeatureFenceLayerName] as GraphicsLayer;
                    if (pLayerFence != null)
                    {
                        //rectangle


                        //polygon
                        ESRI.ArcGIS.Client.Geometry.Polygon pFencePolygon = pLayerFence.Graphics[0].Geometry as ESRI.ArcGIS.Client.Geometry.Polygon;

                        if (pFencePolygon == null || pFencePolygon.Rings.Count != 1 || GeoneralGISFun.PolygonIsLine(pFencePolygon))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_FenceNotPolygon"), MessageDialogButton.Ok);
                            _pEditGeometry.CancelEdit();
                            return;
                        }
                        //Determine whether more than 35 points
                        ESRI.ArcGIS.Client.Geometry.PointCollection pts = pFencePolygon.Rings[0];
                        if (pts.Count > 40)
                        {
                            var childwindow = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_FenceLimitPoints"),
                                MessageDialogButton.OkAndCancel);
                            childwindow.Closed += (sender, e) =>
                            {
                                if (childwindow.DialogResult == true)
                                {
                                    _pEditGeometry.StartEdit(pLayerFence.Graphics[0]);
                                    return;
                                }
                                else
                                {
                                    _CancelEditGeometry();
                                    return;
                                }
                            };

                        }
                        //Determine whether the self-intersection
                        if (GeoneralGISFun.IsCrossSelf(pLayerFence.Graphics[0].Geometry))
                        {
                            var childwindow2 = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_FenceCross"),
                                MessageDialogButton.OkAndCancel);
                            childwindow2.Closed += (sender, e) =>
                            {
                                if (childwindow2.DialogResult == true)
                                {
                                    _pEditGeometry.StartEdit(pLayerFence.Graphics[0]);
                                    return;
                                }
                                else
                                {
                                    _CancelEditGeometry();
                                    return;
                                }
                            };
                        }

                        string newpts = FormatGeometryPoints((pLayerFence.Graphics[0].Geometry as ESRI.ArcGIS.Client.Geometry.Polygon).Rings[0]);

                        _editGeometrypublishedEvent.selectFence.Pts = newpts;
                        _editGeometrypublishedEvent.selectFence.PointCount = (pLayerFence.Graphics[0].Geometry as ESRI.ArcGIS.Client.Geometry.Polygon).Rings[0].Count;
                        pFeaLayerFence_EndSaveEdits(_editGeometrypublishedEvent.selectFence);

                    }
                }
                else if (_editGeometrypublishedEvent.nType == TrafficFeature.Traffic_Route)
                {
                    GraphicsLayer pLayerRoute = MyMap.Layers[ConstDefine.TrafficFeatureRouteLayerName] as GraphicsLayer;
                    if (pLayerRoute != null)
                    {
                        ESRI.ArcGIS.Client.Geometry.Polyline pFencePolyline = pLayerRoute.Graphics[0].Geometry as ESRI.ArcGIS.Client.Geometry.Polyline;

                        if (pFencePolyline == null || pFencePolyline.Paths.Count != 1)
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_RouteInValid"), MessageDialogButton.Ok);
                            _pEditGeometry.CancelEdit();
                            return;
                        }
                        //Determine whether more than 35 points
                        ESRI.ArcGIS.Client.Geometry.PointCollection pts = pFencePolyline.Paths[0];
                        if (pts.Count > 35)
                        {
                            var childwindow3 = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_FenceLimitPoints"),
                                MessageDialogButton.OkAndCancel);
                            childwindow3.Closed += (sender, e) =>
                            {
                                if (childwindow3.DialogResult == true)
                                {
                                    _pEditGeometry.StartEdit(pLayerRoute.Graphics[0]);
                                    return;
                                }
                                else
                                {
                                    _CancelEditGeometry();
                                    return;
                                }
                            };
                        }
                        string newpts = FormatGeometryPoints((pLayerRoute.Graphics[0].Geometry as ESRI.ArcGIS.Client.Geometry.Polyline).Paths[0]);

                        _editGeometrypublishedEvent.selectRoute.Pts = newpts;
                        _editGeometrypublishedEvent.selectRoute.PointCount = (pLayerRoute.Graphics[0].Geometry as ESRI.ArcGIS.Client.Geometry.Polyline).Paths[0].Count;
                        pFeaLayerRoute_EndSaveEdits(_editGeometrypublishedEvent.selectRoute);

                    }

                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("_SaveEditGeometry_Error", ee);
            }
        }

        /// <summary>
        /// Binding save window redo buttons availability
        /// </summary>
        private bool _IsRedoEnable = false;
        public bool IsRedoEnable
        {
            get { return _IsRedoEnable; }
            set
            {
                _IsRedoEnable = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsRedoEnable));
            }
        }
        /// <summary>
        /// Binding save window undo buttons availability
        /// </summary>
        private bool _IsUndoEnable = false;
        public bool IsUndoEnable
        {
            get { return _IsUndoEnable; }
            set
            {
                _IsUndoEnable = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsUndoEnable));
            }
        }
        /// <summary>
        /// Edit geographic feature event (the event is easy to intercept and edit window for editing state control)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _pEditGeometry_GeometryEdit(object sender, EditGeometry.GeometryEditEventArgs e)
        {
            try
            {
                object mview = Router.ViewQuery(GisName.MonitorGisView);
                if ((mview as GisView)._bEditDrawGeometry == false)
                {
                    if (e.Action == EditGeometry.Action.EditStarted && (mview as GisView)._bEditDrawGeometry == false)
                    {
                        EditGeometryVisble = Visibility.Visible;
                    }
                    if (e.Action == EditGeometry.Action.EditCanceled || e.Action == EditGeometry.Action.EditCompleted)
                    {
                        EditGeometryVisble = Visibility.Collapsed;
                    }
                }
                if (e.Action != EditGeometry.Action.EditCompleted &&
                    e.Action != EditGeometry.Action.EditRedone &&
                    e.Action != EditGeometry.Action.EditStarted &&
                    e.Action != EditGeometry.Action.EditUndone &&
                    e.Action != EditGeometry.Action.EditCanceled)
                {
                    if (e.Action == EditGeometry.Action.VertexMoved)
                    {
                        _nEditCount += 0.5;
                    }
                    else
                    {
                        _nEditCount += 1;
                    }
                }
                if (e.Action == EditGeometry.Action.EditUndone)
                {
                    _nUndoCount += 1;
                    _nEditCount -= 1;
                }
                if (e.Action == EditGeometry.Action.EditRedone)
                {
                    _nUndoCount -= 1;
                    _nEditCount += 1;
                }
                RedoEditGeometryCommand.RaiseCanExecuteChanged();
                UndoEditGeometryCommand.RaiseCanExecuteChanged();
                SaveEditGeometryCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("_pEditGeometry_GeometryEdit_Error", ee);
            }
        }
        #endregion
        /// <summary>
        /// Editing Tools
        /// </summary>
        private EditGeometry _pEditGeometry = null;
        /// <summary>
        /// Whether the state of the edit point features
        /// </summary>
        private bool _bEditPoint = false;


        /// <summary>
        /// Editing of point features
        /// </summary>
        private Graphic _bEditPointGraphic = null;
        /// <summary>
        /// Editing toolbar visibility of geographic features
        /// </summary>
        private Visibility _editGeometryVisble = Visibility.Collapsed;
        public Visibility EditGeometryVisble
        {
            get { return _editGeometryVisble; }
            set
            {
                _editGeometryVisble = value;
                RaisePropertyChanged(() => EditGeometryVisble);
            }
        }
        /// <summary>
        /// Edit geographic features command parameters 
        /// (issued by the traffic management module and other geographic features you need to edit the module)
        /// </summary>
        private EditGeometryArgs _editGeometrypublishedEvent = null;

        #endregion
        #region Get point coordinate
        /// <summary>
        /// Get point coordinates
        /// </summary>
        Draw getPoint = null;
        /// <summary>
        /// Get point coordinates
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(GetPointFrommap publishedEvent)
        {
            if (publishedEvent.Enable == true)
            {
                if (getPoint == null)
                {
                    getPoint = new Draw();
                    getPoint.Map = MyMap;
                    getPoint.DrawMode = DrawMode.Point;
                    getPoint.DrawComplete += getPoint_DrawComplete;
                }
                getPoint.IsEnabled = true;
            }
            else
            {
                if (getPoint != null)
                {
                    getPoint.IsEnabled = false;
                }
            }
        }

        private static ESRI.ArcGIS.Client.Projection.WebMercator mercator = new ESRI.ArcGIS.Client.Projection.WebMercator();
        /// <summary>
        /// Returns coordinates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getPoint_DrawComplete(object sender, DrawEventArgs e)
        {
            try
            {
                getPoint.IsEnabled = false;
                MapPoint pt = mercator.ToGeographic(e.Geometry) as MapPoint;

                if (pt != null)
                {
                    Graphic newgraphic = new Graphic()
                    {
                        Geometry = e.Geometry,
                        Symbol = new ESRI.ArcGIS.Client.Symbols.SimpleMarkerSymbol()
                        {
                            Size = 24,
                            Color = new SolidColorBrush { Color = Colors.Red },
                            Style = SimpleMarkerSymbol.SimpleMarkerStyle.Cross
                        }
                    };

                    GraphicsLayer pLayer = MyMap.Layers[ConstDefine.TrafficTempGraphicsLayer] as GraphicsLayer;
                    if (pLayer != null)
                    {
                        pLayer.Graphics.Clear();
                        pLayer.Graphics.Add(newgraphic);
                    }

                    string strReturn = pt.X.ToString("00.000000") + ";" + pt.Y.ToString("00.000000");
                    EventAggregator.Publish<ReturenMapPointString>(new ReturenMapPointString() { MapPointString = strReturn });
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("getPoint_DrawComplete", ex);
            }
        }
        #endregion

    }
}
