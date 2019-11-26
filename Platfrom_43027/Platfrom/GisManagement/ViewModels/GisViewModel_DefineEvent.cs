using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using GisManagement.Models;
using GisManagement.Views;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.Common.Controls;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace GisManagement.ViewModels
{
    public partial class GisViewModel
    {
        #region OnImportsSatisfied
        /// <summary>
        /// OnImportsSatisfied
        /// </summary>
        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<LocateEventArgs>(this);
            EventAggregator.SubscribeOnDispatcher<LocateGeometryEventArgs>(this);
            EventAggregator.SubscribeOnDispatcher<EditGeometryArgs>(this);
            EventAggregator.SubscribeOnDispatcher<RefreshEvent>(this);
            EventAggregator.SubscribeOnDispatcher<ShowBusStopInfoArgs>(this);
            EventAggregator.SubscribeOnDispatcher<DrawFenceEventArgs>(this);
            EventAggregator.SubscribeOnDispatcher<ShowFenceInfoArgs>(this);
            EventAggregator.SubscribeOnDispatcher<DeleteFenceArgs>(this);
            EventAggregator.SubscribeOnDispatcher<DrawRoutEventArgs>(this);
            EventAggregator.SubscribeOnDispatcher<ClearTrafficMaps>(this);
            EventAggregator.SubscribeOnDispatcher<UpdateStopArgs>(this);
            EventAggregator.SubscribeOnDispatcher<GisDisplayControlEvent>(this);
            EventAggregator.SubscribeOnDispatcher<DeleteRouteArgs>(this);
            EventAggregator.SubscribeOnDispatcher<DeleteStopArgs>(this);
            EventAggregator.SubscribeOnDispatcher<MarkTrafficGraphic>(this);
            EventAggregator.SubscribeOnDispatcher<ClearTrafficFeaturelayer>(this);
            EventAggregator.SubscribeOnDispatcher<MarkStopsByRoutID>(this);
            EventAggregator.SubscribeOnDispatcher<OpenCarHisViewState>(this);
            EventAggregator.SubscribeOnDispatcher<ZoomToDistByDistCode>(this);
            EventAggregator.SubscribeOnDispatcher<GetPointFrommap>(this);
            //EventAggregator.SubscribeOnDispatcher<UpdateTrafficMarkArgs>(this);
            EventAggregator.SubscribeOnDispatcher<DisplayHistoricalRoute>(this);
            EventAggregator.SubscribeOnDispatcher<ZoomGisView>(this);


        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(GisDisplayControlEvent publishedEvent)
        {
            //control layer display
            currentDisplay = publishedEvent.Display;
            //control gpscar display
            DisplayControlFilterByGroup(currentDisplay, publishedEvent.DisplayGroupName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VE"></param>
        /// <returns></returns>
        public bool isDisplayMap(ElementLayerDefine VE)
        {
            return _currentVisibleLst.IndexOf(VE) > -1;
        }

        /// <summary>
        /// Handle a variety of location events
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(LocateEventArgs publishedEvent)
        {
            if (MyMap == null) return;
            bool isLocate = false;
            try
            {
                if (publishedEvent is LocateEventArgs)
                {
                    if (!isDisplayMap(publishedEvent.VE)) return;
                    switch (publishedEvent.Operate)
                    {
                        case MapEventArgs.MapOperatePanTo://pan to
                            {
                                //3857地图
                                ESRI.ArcGIS.Client.Geometry.MapPoint pt = VechileMemDataOperate.GetProjCoord((publishedEvent as LocateEventArgs).Lon, (publishedEvent as LocateEventArgs).Lat);

                                //ESRI.ArcGIS.Client.Geometry.MapPoint pt =new  ESRI.ArcGIS.Client.Geometry.MapPoint((publishedEvent as LocateEventArgs).Lon, (publishedEvent as LocateEventArgs).Lat);
                                
                                MyMap.PanTo(pt);
                                if (MyMap.Extent != null) MyMap.Extent = MyMap.Extent.Expand(1.001);
                                isLocate = true;
                                break;
                            }
                        case MapEventArgs.MapOperateLocate://locate 
                            {
                                //3857地图
                                ESRI.ArcGIS.Client.Geometry.MapPoint pt = VechileMemDataOperate.GetProjCoord((publishedEvent as LocateEventArgs).Lon, (publishedEvent as LocateEventArgs).Lat);
                                //If the map scale is not big enough, then zoom in to a specified percentage

                                //ESRI.ArcGIS.Client.Geometry.MapPoint pt = new ESRI.ArcGIS.Client.Geometry.MapPoint((publishedEvent as LocateEventArgs).Lon, (publishedEvent as LocateEventArgs).Lat);
                                
                                string strLocal = ApplicationContext.Instance.ServerConfig.AutoLocateResolution;
                                strLocal = strLocal.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                                if (MyMap.Resolution > Convert.ToDouble(strLocal))
                                {
                                    CenterAndZoom(Convert.ToDouble(strLocal), pt);
                                    isLocate = true;
                                }
                                else
                                {
                                    MyMap.PanTo(pt);
                                    if (MyMap.Extent != null) MyMap.Extent = MyMap.Extent.Expand(1.001);
                                    isLocate = true;
                                }


                                break;
                            }

                        case MapEventArgs.MapOperateLocateByUniqueID://lcoate
                            {
                                VechileElementLst Lst = VechileMemDataOperate.GetCarMonitor(publishedEvent.VE);
                                GpsCar gpscar = Lst.GetCarUIElementByUniqueID(publishedEvent.UniqueID);
                                if ((gpscar != null) && (gpscar.Lon != null) && (gpscar.Lat != null) && (gpscar.Lat != "") && (gpscar.Lon != ""))
                                {
                                    //3857地图
                                    ESRI.ArcGIS.Client.Geometry.MapPoint pt = VechileMemDataOperate.GetProjCoord(double.Parse(gpscar.Lon), double.Parse(gpscar.Lat));
                                    gpscar.AlertFlag = ApplicationContext.Instance.BufferManager.VehicleAlertManager.HasAlert(gpscar.CarNo);
                                    gpscar.AlarmFlag = ApplicationContext.Instance.BufferManager.AlarmManager.HasAlarm(gpscar.CarNo);
                                   // ESRI.ArcGIS.Client.Geometry.MapPoint pt = new ESRI.ArcGIS.Client.Geometry.MapPoint(double.Parse(gpscar.Lon), double.Parse(gpscar.Lat));


                                    var vehicleInfo = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.FirstOrDefault(t => t.VehicleId == gpscar.CarNo);
                                    if (vehicleInfo != null)
                                    {
                                        if (!string.IsNullOrEmpty(vehicleInfo.VehicleTypeImage))
                                        {
                                            byte[] data = Convert.FromBase64String(vehicleInfo.VehicleTypeImage);

                                            BitmapImage image = new BitmapImage();

                                            image.SetSource(new MemoryStream(data));

                                            gpscar.VehicleType = image;

                                        }
                                        if (!string.IsNullOrEmpty(vehicleInfo.VehicleTypeDescribe))
                                        {

                                            gpscar.SpeedColorList = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.SpeedColorList.Where(x => x.TypeName == vehicleInfo.VehicleTypeDescribe).ToList();
                                        }

                                    }
                                    string strLocal = ApplicationContext.Instance.ServerConfig.AutoLocateResolution;
                                    strLocal = strLocal.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                                    if (MyMap.Resolution > Convert.ToDouble(strLocal))
                                    {
                                        CenterAndZoom(Convert.ToDouble(strLocal), pt);
                                        isLocate = true;
                                    }
                                    else
                                    {
                                        if (MyMap.Extent != null) MyMap.Extent = MyMap.Extent.Expand(0.8);
                                        MyMap.PanTo(pt);
                                        isLocate = true;
                                    }
                                }
                                break;
                            }
                        case MapEventArgs.MapOperateTraceByUniqueID:
                            {
                                isLocate = true;
                                VechileElementLst Lst = VechileMemDataOperate.GetCarMonitor(publishedEvent.VE);
                                GpsCar gpscar = Lst.GetCarUIElementByUniqueID(publishedEvent.UniqueID);
                                if ((gpscar != null) && (gpscar.Lon != null) && (gpscar.Lat != null) && (gpscar.Lat != "") && (gpscar.Lon != ""))
                                {
                                    //3857地图
                                    ESRI.ArcGIS.Client.Geometry.MapPoint pt = VechileMemDataOperate.GetProjCoord(double.Parse(gpscar.Lon), double.Parse(gpscar.Lat));
                                    //ESRI.ArcGIS.Client.Geometry.MapPoint pt = new ESRI.ArcGIS.Client.Geometry.MapPoint(double.Parse(gpscar.Lon), double.Parse(gpscar.Lat));

                                    ESRI.ArcGIS.Client.Geometry.Envelope env = new ESRI.ArcGIS.Client.Geometry.Envelope(pt.X - 10, pt.Y - 10, pt.X + 10, pt.Y + 10);
                                    double dis = MyMap.Extent.Width / 2;
                                    ESRI.ArcGIS.Client.Geometry.Envelope screnv = new ESRI.ArcGIS.Client.Geometry.Envelope(pt.X + dis, pt.Y + dis, pt.X - dis, pt.Y - dis);

                                    if (!MyMap.Extent.Intersects(env))//Not within the screen, transfer
                                    {
                                        MyMap.PanTo(pt);
                                        MyMap.Extent = screnv;
                                    }
                                }

                                break;
                            }
                        case MapEventArgs.MapOperateTraceCar://trace car 
                            {
                                isLocate = true;

                                //3857地图
                                ESRI.ArcGIS.Client.Geometry.MapPoint pt = VechileMemDataOperate.GetProjCoord((publishedEvent as LocateEventArgs).Lon, (publishedEvent as LocateEventArgs).Lat);
                                //ESRI.ArcGIS.Client.Geometry.MapPoint pt = new ESRI.ArcGIS.Client.Geometry.MapPoint((publishedEvent as LocateEventArgs).Lon, (publishedEvent as LocateEventArgs).Lat);

                                ESRI.ArcGIS.Client.Geometry.Envelope env = new ESRI.ArcGIS.Client.Geometry.Envelope(pt.X - 10, pt.Y - 10, pt.X + 10, pt.Y + 10);
                                double dis = MyMap.Extent.Width / 2;
                                ESRI.ArcGIS.Client.Geometry.Envelope screnv = new ESRI.ArcGIS.Client.Geometry.Envelope(pt.X + dis, pt.Y + dis, pt.X - dis, pt.Y - dis);

                          

                                if (!MyMap.Extent.Intersects(env))//Not within the screen, transfer
                                {
                                    MyMap.PanTo(pt);
                                    MyMap.Extent = screnv;
                                }
                            }
                            break;
                    }
                    if (!isLocate)
                    {
                        //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("GIS_LocationFailed"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageBoxButton.OK);

                    }

                }

            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GisViewModel", ee);
            }
        }


        // Processing OpenCarHisViewState message
        public void HandleEvent(OpenCarHisViewState publishedEvent)
        {
            if (null != publishedEvent && publishedEvent.State)
            {
                IsCarHisVisual = true;
            }
        }


        /// <summary>
        /// Handle refresh event
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(RefreshEvent publishedEvent)
        {
            if (MyMap == null) return;
            if (MyMap.Extent == null) return;
            try
            {
                MyMap.Extent = MyMap.Extent.Expand(1.001);
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GisViewModel", ee);
            }
        }

        /// <summary>
        /// Handle Edit message  elements
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(EditGeometryArgs publishedEvent)
        {
            try
            {
                _editGeometrypublishedEvent = publishedEvent;

                object mview1 = Router.ViewQuery(GisName.MonitorGisView);
                if (mview1 != null) (mview1 as GisView)._bEditDrawGeometry = false;

                if (_pEditGeometry == null)
                {
                    object mview = Router.ViewQuery(GisName.MonitorGisView);
                    if (mview != null)
                    {
                        _pEditGeometry = (mview as GisView).EditGeoemtryTool;
                        _pEditGeometry.GeometryEdit += _pEditGeometry_GeometryEdit;
                    }
                }
                _pEditGeometry.EditVerticesEnabled = true;
                _pEditGeometry.MoveEnabled = true;
                _pEditGeometry.RotateEnabled = true;
                _pEditGeometry.ScaleEnabled = true;

                if (publishedEvent.nType == TrafficFeature.Traffic_PolygonFence)
                {
                    GraphicsLayer pLayerFence = MyMap.Layers[ConstDefine.TrafficFeatureFenceLayerName] as GraphicsLayer;
                    if (pLayerFence != null)
                    {
                        if (pLayerFence.Visible == false)
                            return;
                        if (pLayerFence.Graphics.Count != 1)
                        {
                            return;
                        }

                        _pEditGeometry.CancelEdit();
                        _pEditGeometry.StopEdit();
                        _pEditGeometry.StartEdit(pLayerFence.Graphics[0]);
                        _bEditFence = true;
                    }
                }
                else if (publishedEvent.nType == TrafficFeature.Traffic_Route)
                {
                    GraphicsLayer pLayerRoute = MyMap.Layers[ConstDefine.TrafficFeatureRouteLayerName] as GraphicsLayer;
                    if (pLayerRoute != null)
                    {
                        if (pLayerRoute.Visible == false)
                            return;
                        if (pLayerRoute.Graphics.Count != 1)
                        {
                            return;
                        }

                        _pEditGeometry.CancelEdit();
                        _pEditGeometry.StopEdit();
                        _pEditGeometry.StartEdit(pLayerRoute.Graphics[0]);
                        _bEditRoute = true;
                    }
                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("EditGeometryArgs_Error", ee);
            }
        }

        ///// <summary>
        ///// Handle draw Graphics  event
        ///// </summary>
        ///// <param name="publishedEvent"></param>
        //public void HandleEvent(DrawGraphicsEventArgs publishedEvent)
        //{
        //    if (MyMap == null) return;
        //    try
        //    {
        //        if (publishedEvent is DrawGraphicsEventArgs)
        //        {
        //            switch (publishedEvent.Operate)
        //            {
        //                case MapEventArgs.MapOperateDrawGraphics://draw a point 
        //                    {
        //                        if (publishedEvent.Graphic != null)
        //                        {
        //                            GraphicsLayer gly = MyMap.Layers[publishedEvent.LayerName] as GraphicsLayer;
        //                            Graphic g = publishedEvent.Graphic;
        //                            g.Symbol = new ESRI.ArcGIS.Client.Symbols.SimpleMarkerSymbol()
        //                            {
        //                                Color = new SolidColorBrush() { Color = Colors.Red },
        //                                Size = 20
        //                            };

        //                            gly.Graphics.Clear();
        //                            gly.Graphics.Add(publishedEvent.Graphic);
        //                        }
        //                        break;
        //                    }

        //            }
        //        }

        //    }
        //    catch (Exception ee)
        //    {
        //        ApplicationContext.Instance.Logger.LogException("GisViewModel", ee);
        //    }
        //}

        /// <summary>
        /// Control traffic management module Layer Display
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(ClearTrafficFeaturelayer publishedEvent)
        {
            try
            {
                //
                if (publishedEvent.nType == TrafficFeature.Traffic_PolygonFence)
                {
                    GraphicsLayer pLayerFence = MyMap.Layers[ConstDefine.TrafficFeatureFenceLayerName] as GraphicsLayer;
                    if (pLayerFence != null)
                    {
                        pLayerFence.Visible = publishedEvent.bLayerVisble;
                        pLayerFence.ClearSelection();
                        pLayerFence.Refresh();
                    }
                }
                if (publishedEvent.nType == TrafficFeature.Traffic_Route)
                {
                    GraphicsLayer pLayerRoute = MyMap.Layers[ConstDefine.TrafficFeatureRouteLayerName] as GraphicsLayer;
                    if (pLayerRoute != null)
                    {
                        pLayerRoute.Visible = publishedEvent.bLayerVisble;
                        pLayerRoute.ClearSelection();
                        pLayerRoute.Refresh();
                    }
                }

            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("ClearTrafficFeaturelayer_Error", ee);
            }
        }

        public void HandleEvent(ZoomGisView publishedEvent)
        {
            Envelope extent=MonitorList.FenceGraphicHelp.GetExtent();
            if (extent != null)
            {
                MyMap.ZoomTo(extent);
            }
        }
    }
}
