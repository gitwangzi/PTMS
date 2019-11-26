using ESRI.ArcGIS.Client;
using GisManagement.Models;
using GisManagement.Views;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.ServiceReference.VehicleMonitorService;
using Gsafety.PTMS.Share;
using Gsafety.Common.CommMessage;
using Jounce.Core.Event;
using Jounce.Core.Model;
using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gs.PTMS.Common.Data.Enum;
using Gsafety.Common.Controls;
using System.Reflection;

namespace GisManagement.ViewModels
{
    public class VechileMemDataOperate : BaseNotify,
        //IEventSink<AlertAddRemoveArgs>,
        IEventSink<AlarmLocationAddRemoveArgs>,
        IEventSink<AlertAddRemoveCurrentPosition>,
        IEventSink<TrackCarArgs>,
        IEventSink<AlarmGisArgs>,
        IEventSink<Gsafety.PTMS.ServiceReference.VehicleAlarmService.AlarmInfoEx>,
        IEventSink<DisplayCurrentPositionArgs>,
        IEventSink<Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS>,
        IEventSink<RequestVehicleMonitorArgs>,
        IEventSink<AlertGisArgs>,
        IPartImportsSatisfiedNotification
    {
        [Import]
        public IEventAggregator _EventAggregator { get; set; }

        public VechileMemDataOperate()
        {
            try
            {
                IsAnimation = false;
                IsTrajectory = false;
                Interval = 1000;
                CompositionInitializer.SatisfyImports(this);
                vehicleMonitorServiceClient.GetLastMonitorGPSCompleted += vehicleMonitorServiceClient_GetLastMonitorGPSCompleted;

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }

        /// <summary>
        /// Subscribe to event
        /// </summary>
        /// 
        public void OnImportsSatisfied()
        {
            _EventAggregator.SubscribeOnDispatcher<AlertAddRemoveCurrentPosition>(this);
            _EventAggregator.SubscribeOnDispatcher<AlarmLocationAddRemoveArgs>(this);

            _EventAggregator.SubscribeOnDispatcher<Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS>(this);
            _EventAggregator.SubscribeOnDispatcher<DisplayCurrentPositionArgs>(this);

            _EventAggregator.SubscribeOnDispatcher<RequestVehicleMonitorArgs>(this);
            _EventAggregator.SubscribeOnDispatcher<AlarmGisArgs>(this);
            _EventAggregator.SubscribeOnDispatcher<AlertGisArgs>(this);
            _EventAggregator.SubscribeOnDispatcher<TrackCarArgs>(this);

        }



        /// <summary>
        /// Determine whether the vehicle has been monitoring them
        /// </summary>
        /// <param name="csCarNo"></param>
        /// <param name="csUniqueID"></param>
        /// <param name="csANTGPSID"></param>
        /// <returns></returns>
        private bool CarExists(ElementLayerDefine Ved, string csUniqueID)
        {
            VechileElementLst Lst = VechileMemDataOperate.GetCarMonitor(Ved);
            foreach (GpsCar car in Lst.Elements)
            {
                if (car.UniqueID == csUniqueID)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Where the list of vehicles to be monitored
        /// </summary>
        /// <param name="ved"></param>
        /// <returns></returns>
        public static VechileElementLst GetCarMonitor(ElementLayerDefine ved)
        {
            switch (ved)
            {
                case ElementLayerDefine.miVERealLocation://gps定位
                    {
                        return MonitorList.VechileRealLocationElements;
                    }
                case ElementLayerDefine.miVEAlarmHappenLocation://解除一键报警
                    {
                        return MonitorList.AlarmHappenLocationElements;
                    }

                case ElementLayerDefine.miVEAlertHappenLocation://解除告警
                    {
                        return MonitorList.AlertHappenLocationElements;
                    }
                default:
                    {
                        return MonitorList.VechileRealLocationElements;//请求车辆gps数据
                    }
            }
        }

        /// <summary>
        /// Cancellation monitor vehicle
        /// </summary>
        /// <param name="ved"></param>
        /// <param name="MDVRID"></param>
        private void RemoveCarByUniqueID(ElementLayerDefine Ved, string csUniqueID)
        {
            try
            {
                VechileElementLst Lst = GetCarMonitor(Ved);
                GpsCar car = Lst.GetCarUIElementByUniqueID(csUniqueID);
                //Remove the vehicle
                if (car != null)
                {
                    VechileGrapicLst glst = GetCarMonitorGraphics(car.ElementLayDefine);
                    glst.RemoveGraphic(csUniqueID);
                    Lst.RemoveCars(car);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        /// <summary>
        /// Get a list of where the vehicle trajectory
        /// </summary>
        /// <param name="ved"></param>
        /// <returns></returns>
        private VechileGrapicLst GetCarMonitorGraphics(ElementLayerDefine ved)
        {
            switch (ved)
            {
                case ElementLayerDefine.miVERealLocation:
                    {
                        return MonitorList.VechileRealLocationGraphics;
                    }

                case ElementLayerDefine.miVEAlarmHappenLocation:
                    {
                        return MonitorList.AlarmHappenLocationGraphics;
                    }

                case ElementLayerDefine.miVEAlertHappenLocation:
                    {
                        return MonitorList.AlertHappenLocationGraphics;
                    }
                default:
                    {
                        return MonitorList.VechileRealLocationGraphics;
                    }
            }
        }

        /// <summary>
        ///Add monitor vehicle
        /// </summary>
        /// <param name="csCarNo"></param>
        /// <param name="csUniqueID">MDVRID</param>
        /// <param name="csANTGPSID">ANTGPSID</param>
        /// <returns></returns>
        private GpsCar AddCar(ElementLayerDefine ved, string csCarNo, string csUniqueID)
        {
            GpsCar car = new GpsCar(csCarNo, csUniqueID);
            car.ElementLayDefine = ved;

            car.Graphics = GetCarMonitorGraphics(ved);
            ESRI.ArcGIS.Client.Geometry.MapPoint pt = new ESRI.ArcGIS.Client.Geometry.MapPoint(0, 0);
            GetCarMonitor(ved).AddCars(car, pt);

            //RaisePropertyChanged("ViewCars");
            return car;
        }
        /// <summary>
        /// Receive news updates GPS vehicle location, only a concern when automatic positioning
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS publishedEvent)
        {
            try
            {
                if (CarExists(ElementLayerDefine.miVERealLocation, publishedEvent.VehicleId))
                {
                    GpsCar car = MonitorList.VechileRealLocationElements.GetCarUIElementByUniqueID(publishedEvent.VehicleId) as GpsCar;

                    if (car.IsTracked == true)
                    {
                        Draw(car, publishedEvent, 0, true, false, ElementLayerDefine.miVERealLocation);
                    }
                    else
                    {
                        Draw(car, publishedEvent, 0, false, false, ElementLayerDefine.miVERealLocation);
                    }
                   
                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("VechileMemDataOperate", ee);
            }
        }

        /// <summary>
        /// Not all of the received data should show that this strategy can be developed to display the
        /// </summary>
        /// <param name="car"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool FilterDrawData(GpsCar car, Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS e)
        {
            //If the reception time is shorter than discarding Interval。
            if ((car.GpsTime == null) || (car.GpsTime == "")) return true;
            if (DateTime.Parse(car.GpsTime).AddMilliseconds(Interval).CompareTo(e.GpsTime) <= 0) return true;
            return false;
        }


        /// <summary>
        /// Drawn carriage position
        /// </summary>
        /// <param name="e"></param>
        public void Draw(GpsCar car, Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS args, int csInterval, bool csIsTrajectory, bool csIsAnimation, ElementLayerDefine csVE)
        {
            try
            {
                if (!FilterDrawData(car, args)) return; //Filtering the data plotted。

                car.Valid = args.Valid;
                car.RecDateTime = DateTime.Now.ToString(ApplicationContext.Instance.ServerConfig.LongDateFormat);
                //if (car.Valid != "A") return;//If Gps coordinates invalid exit
                if (!GPSState.Valid(car.Valid)) return;
                car.UpdateGpsInfo(args);
                car.ElementLayDefine = csVE;
                car.GPSSource = (GPSSourceEnum)args.Source;

                ESRI.ArcGIS.Client.Geometry.MapPoint pt = new ESRI.ArcGIS.Client.Geometry.MapPoint();
                if ((car.Lat != null) && (car.Lon != null))
                {
                    double lslon = double.Parse(car.Lon);
                    double lslat = double.Parse(car.Lat);

                    //3857地图
                    pt = GetProjCoord(lslon, lslat);

                    //pt = new ESRI.ArcGIS.Client.Geometry.MapPoint(lslon, lslat);
                }
                else
                {
                    return;
                }
                if (car.HasDraw == false)
                {
                    ElementLayer.SetEnvelope(car, new ESRI.ArcGIS.Client.Geometry.Envelope(pt, pt));
                    car.HasDraw = true;
                    //string ID = Guid.NewGuid().ToString();
                    //Graphic newgraphic = new Graphic()
                    //{
                    //    Geometry = pt,
                    //    Symbol = new ESRI.ArcGIS.Client.Symbols.SimpleMarkerSymbol
                    //    {
                    //        Style = ESRI.ArcGIS.Client.Symbols.SimpleMarkerSymbol.SimpleMarkerStyle.Circle,
                    //        Size = 8,
                    //        Color = new SolidColorBrush(Colors.Red),
                    //    }
                    //};

                    //MonitorList.VechileRealLocationGraphics.AddGraphic(newgraphic, ID + "@" + ID.ToString());
                    _EventAggregator.Publish<RefreshEvent>(new RefreshEvent() { });
                    //If the current concern, and is the first to draw out, and is currently displayed mode, the positioning
                    if (car.IsTracked)
                    {
                        _EventAggregator.Publish<LocateEventArgs>(new LocateEventArgs() { VE = csVE, Lat = double.Parse(car.Lat), Lon = double.Parse(car.Lon) });
                    }
                }
                else
                {
                    ESRI.ArcGIS.Client.Geometry.Envelope env = ElementLayer.GetEnvelope(car);
                    ESRI.ArcGIS.Client.Geometry.MapPoint _OldPosition = new ESRI.ArcGIS.Client.Geometry.MapPoint(env.XMin, env.YMin);
                    car.UpdateCarRotate(_OldPosition, pt);
                    if (csIsAnimation)
                    {
                        car.DrawTo(_OldPosition, pt, csInterval, csIsTrajectory);
                    }
                    else
                    {
                        if (csIsTrajectory)
                        {
                            //string ID = Guid.NewGuid().ToString();
                            //Graphic newgraphic = new Graphic()
                            //{
                            //    Geometry = pt,
                            //    Symbol = new ESRI.ArcGIS.Client.Symbols.SimpleMarkerSymbol
                            //    {
                            //        Style = ESRI.ArcGIS.Client.Symbols.SimpleMarkerSymbol.SimpleMarkerStyle.Circle,
                            //        Size = 8,
                            //        Color = new SolidColorBrush(Colors.Red),
                            //    }
                            //};

                            //MonitorList.VechileRealLocationGraphics.AddGraphic(newgraphic, ID + "@" + ID.ToString());
                            car.DrawRoute(_OldPosition, pt, false, true);
                            
                        }
                        else
                        {
                            MonitorList.VechileRealLocationGraphics.Clear();
                        
                        }
                        ElementLayer.SetEnvelope(car, new ESRI.ArcGIS.Client.Geometry.Envelope(pt, pt));
                    }

                    car.HasDraw = true;
                    //If the current concern, and is currently displayed mode, the track
                    //if (car.IsTracked)
                    //{
                    //    _EventAggregator.Publish<LocateEventArgs>(new LocateEventArgs() { VE = csVE, Operate = MapEventArgs.MapOperateTraceCar, Lat = double.Parse(car.Lat), Lon = double.Parse(car.Lon) });
                    //}
                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("VechileMemDataOperate", ee);
            }
        }


        /// <summary>
        /// Latitude and longitude coordinates into projected coordinates
        /// </summary>
        /// <returns></returns>
        public static ESRI.ArcGIS.Client.Geometry.MapPoint GetProjCoord(double csLon, double csLat)
        {
            ESRI.ArcGIS.Client.Geometry.MapPoint pt = new ESRI.ArcGIS.Client.Geometry.MapPoint(csLon, csLat);
            //return ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(pt);
            return Gsafety.Common.Transform.GeographicToWebMercator(pt);
        }

        /// <summary>
        /// Projection coordinates into latitude and longitude coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static ESRI.ArcGIS.Client.Geometry.MapPoint GetGeoCoord(double x, double y)
        {
            ESRI.ArcGIS.Client.Geometry.MapPoint pt = new ESRI.ArcGIS.Client.Geometry.MapPoint(x, y);
            //return ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(pt);
            return Gsafety.Common.Transform.WebMercatorToGeographic(pt);
        }

        private bool _IsTrajectory;
        /// <summary>
        /// Whether to draw the trajectory
        /// </summary>
        public bool IsTrajectory
        {
            get
            {
                return _IsTrajectory;
                // return ((_IsTrajectory) && (!IsAnimation));
            }
            set
            {
                _IsTrajectory = value;
                MonitorList.VechileRealLocationGraphics.Clear();
            }
        }

        /// <summary>
        /// Play the animation speed
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// Is Animation 
        /// </summary>
        public bool IsAnimation { get; set; }



        /// <summary>
        /// Taken out of a database key alarm
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(Gsafety.PTMS.ServiceReference.VehicleAlarmService.AlarmInfoEx publishedEvent)
        {
            try
            {
                if (!CarExists(ElementLayerDefine.miVERealLocation, publishedEvent.VehicleId))
                {
                    GpsCar car = AddCar(ElementLayerDefine.miVERealLocation, publishedEvent.VehicleId, publishedEvent.VehicleId);//添加到本机上
                    //car.Company = publishedEvent.CompanyName;
                    car.Prov = publishedEvent.City;

                    Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS gps = new Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS();
                    gps.VehicleId = publishedEvent.VehicleId;
                    gps.Direction = publishedEvent.Direction;
                    gps.GpsTime = publishedEvent.AlarmTime;
                    gps.Latitude = publishedEvent.Latitude;
                    gps.Longitude = publishedEvent.Longitude;
                    gps.Speed = publishedEvent.Speed;
                    gps.Valid = publishedEvent.GpsValid;
                    gps.Source = publishedEvent.Source;
                    //Draw the alarm location

                    Draw(car, gps, 0, false, false, ElementLayerDefine.miVERealLocation);

                    if (MonitorList.VechileRealLocationElements.Elements.Count == 1)//If only one, it is automatically selected concern
                    {
                        //SetSelectedCar(car);
                        if (GPSState.Valid(gps.Valid))
                        {
                            _EventAggregator.Publish<LocateEventArgs>(new LocateEventArgs() { VE = ElementLayerDefine.miVERealLocation, Lat = double.Parse(car.Lat), Lon = double.Parse(car.Lon) });
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("VechileMemDataOperate", ee);
            }
        }

        /// <summary>
        /// clear trace
        /// </summary>
        /// <param name="VE"></param>
        private void ClearSelectedCar(ElementLayerDefine VE)
        {
            switch (VE)
            {
                case ElementLayerDefine.miVERealLocation:
                    foreach (GpsCar car in MonitorList.VechileRealLocationElements.Elements)
                    {
                        car.IsTracked = false;
                    }
                    break;
                case ElementLayerDefine.miVEAlarmHappenLocation:
                    foreach (GpsCar car in MonitorList.AlarmHappenLocationElements.Elements)
                    {
                        car.IsTracked = false;
                    }
                    break;
                case ElementLayerDefine.miVEAlertHappenLocation:

                    foreach (GpsCar car in MonitorList.AlertHappenLocationElements.Elements)
                    {
                        car.IsTracked = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// A view is selected to ensure that only one vehicle is currently located. General practice should be to judge the program in the same layer of a ElementLst, only one is selected
        /// </summary>
        /// <param name="cscar"></param>
        private void SetLocatedCar(GpsCar cscar)
        {
            try
            {
                switch (cscar.ElementLayDefine)
                {
                    case ElementLayerDefine.miVERealLocation:
                        foreach (GpsCar car in MonitorList.VechileRealLocationElements.Elements)
                        {
                            if (car == cscar)
                                car.IsSelected = true;
                            else
                            {
                                if ((car.IsSelected))
                                {
                                    car.IsSelected = false;
                                }
                            }
                        }
                        break;
                    case ElementLayerDefine.miVEAlarmHappenLocation:
                        foreach (GpsCar car in MonitorList.AlarmHappenLocationElements.Elements)
                        {
                            if (car == cscar)
                                car.IsSelected = true;
                            else
                                car.IsSelected = false;
                        }
                        break;


                    case ElementLayerDefine.miVEAlertHappenLocation:

                        foreach (GpsCar car in MonitorList.AlertHappenLocationElements.Elements)
                        {
                            if (car == cscar)
                                car.IsSelected = true;
                            else
                                car.IsSelected = false;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// A view is selected to ensure that only one is selected vehicle. General practice should be to judge the program in the same layer of a ElementLst, only one is selected
        /// </summary>
        /// <param name="cscar"></param>
        private void SetTrackedCar(GpsCar cscar)
        {
            switch (cscar.ElementLayDefine)
            {
                case ElementLayerDefine.miVERealLocation:
                    foreach (GpsCar car in MonitorList.VechileRealLocationElements.Elements)
                    {
                        if (car == cscar)
                            car.IsTracked = true;
                        else
                            car.IsTracked = false;
                    }
                    break;
                case ElementLayerDefine.miVEAlarmHappenLocation:

                    foreach (GpsCar car in MonitorList.AlarmHappenLocationElements.Elements)
                    {
                        if (car == cscar)
                            car.IsTracked = true;
                        else
                            car.IsTracked = false;
                    }
                    break;


                case ElementLayerDefine.miVEAlertHappenLocation:

                    foreach (GpsCar car in MonitorList.AlertHappenLocationElements.Elements)
                    {
                        if (car == cscar)
                            car.IsTracked = true;
                        else
                            car.IsTracked = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// Manual positioning of the current vehicle
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(AlertAddRemoveCurrentPosition publishedEvent)
        {
            if (publishedEvent.Op == 1)
            {
                if (!CarExists(ElementLayerDefine.miVEAlertHappenLocation, publishedEvent.VehicleId + "@" + publishedEvent.AlertTime.ToString()))
                {
                    GpsCar car = AddCar(ElementLayerDefine.miVEAlertHappenLocation, publishedEvent.VehicleId, publishedEvent.VehicleId + "@" + publishedEvent.AlertTime.ToString());
                    car.Prov = publishedEvent.CityName;
                    Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS gps = new Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS();
                    gps.VehicleId = publishedEvent.VehicleId;
                    gps.Direction = publishedEvent.Direction;
                    gps.GpsTime = publishedEvent.GpsTime;
                    gps.Latitude = publishedEvent.Latitude;
                    gps.Longitude = publishedEvent.Longitude;
                    gps.Speed = publishedEvent.Speed;
                    gps.Valid = publishedEvent.Valid;
                    gps.Source = publishedEvent.Source;

                    Draw(car, gps, 0, false, false, ElementLayerDefine.miVEAlertHappenLocation);
                    SetLocatedCar(car);
                    if (GPSState.Valid(car.Valid))
                    {
                        _EventAggregator.Publish<LocateEventArgs>(new LocateEventArgs() { VE = ElementLayerDefine.miVEAlertHappenLocation, Lat = double.Parse(car.Lat), Lon = double.Parse(car.Lon) });
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("GIS_GpsInValid"));
                    }
                }
                else
                {
                    VechileElementLst mlst = GetCarMonitor(ElementLayerDefine.miVEAlertHappenLocation);
                    GpsCar car = mlst.GetCarUIElementByUniqueID(publishedEvent.VehicleId + "@" + publishedEvent.AlertTime.ToString());
                    SetLocatedCar(car);
                    if (GPSState.Valid(car.Valid))
                    {
                        _EventAggregator.Publish<LocateEventArgs>(new LocateEventArgs() { VE = ElementLayerDefine.miVEAlertHappenLocation, Lat = double.Parse(car.Lat), Lon = double.Parse(car.Lon) });
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("GIS_GpsInValid"));
                    }
                }

            }
            else
            {
                if (CarExists(ElementLayerDefine.miVEAlertHappenLocation, publishedEvent.VehicleId + "@" + publishedEvent.AlertTime.ToString()))
                {
                    RemoveCarByUniqueID(ElementLayerDefine.miVEAlertHappenLocation, publishedEvent.VehicleId + "@" + publishedEvent.AlertTime.ToString());
                }
            }
        }

        /// <summary>
        /// A key alarm manually locate the incident
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(AlarmLocationAddRemoveArgs publishedEvent)
        {
            if (publishedEvent.Op == 1)
            {
                if (!CarExists(ElementLayerDefine.miVEAlarmHappenLocation, publishedEvent.VehicleId + "@" + publishedEvent.AlarmTime.ToString()))//绘制告警点
                {
                    GpsCar car = AddCar(ElementLayerDefine.miVEAlarmHappenLocation, publishedEvent.VehicleId, publishedEvent.VehicleId + "@" + publishedEvent.AlarmTime.ToString());//添加到本机上
                    car.Prov = publishedEvent.City;

                    Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS gps = new Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS();
                    gps.VehicleId = publishedEvent.VehicleId;
                    gps.Direction = publishedEvent.Direction;
                    gps.GpsTime = publishedEvent.AlarmTime;
                    gps.Latitude = publishedEvent.Latitude;
                    gps.Longitude = publishedEvent.Longitude;
                    gps.Speed = publishedEvent.Speed;
                    gps.Valid = publishedEvent.GpsValid;
                    gps.Source = publishedEvent.Source;
                    //Draw the alarm location
                    Draw(car, gps, 0, false, false, ElementLayerDefine.miVEAlarmHappenLocation);
                    SetLocatedCar(car);
                    if (GPSState.Valid(car.Valid))
                    {
                        _EventAggregator.Publish<LocateEventArgs>(new LocateEventArgs() { VE = ElementLayerDefine.miVEAlarmHappenLocation, Lat = double.Parse(car.Lat), Lon = double.Parse(car.Lon) });
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("GIS_GpsInValid"));
                    }
                }
                else
                {
                    VechileElementLst mlst = GetCarMonitor(ElementLayerDefine.miVEAlarmHappenLocation);
                    GpsCar car = mlst.GetCarUIElementByUniqueID(publishedEvent.VehicleId + "@" + publishedEvent.AlarmTime.ToString());
                    SetLocatedCar(car);
                    if (GPSState.Valid(car.Valid))
                    {
                        _EventAggregator.Publish<LocateEventArgs>(new LocateEventArgs() { VE = ElementLayerDefine.miVEAlarmHappenLocation, Lat = double.Parse(car.Lat), Lon = double.Parse(car.Lon) });
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("GIS_GpsInValid"));
                    }
                }
            }
            else
            {
                if (CarExists(ElementLayerDefine.miVEAlarmHappenLocation, publishedEvent.MdvrCoreId + "@" + publishedEvent.AlarmTime.ToString()))
                {
                    RemoveCarByUniqueID(ElementLayerDefine.miVEAlarmHappenLocation, publishedEvent.MdvrCoreId + "@" + publishedEvent.AlarmTime.ToString());
                }
            }
        }



        /// <summary>
        /// Treatment on demand
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(RequestVehicleMonitorArgs publishedEvent)
        {
            try
            {
                if (publishedEvent.Op == 1)//Application for registration
                {
                    GpsCar car = null;
                    if (!CarExists(ElementLayerDefine.miVERealLocation, publishedEvent.UniqueId))
                    {
                        car = AddCar(ElementLayerDefine.miVERealLocation, publishedEvent.CarNo, publishedEvent.UniqueId);
                        if (publishedEvent.IsGetLastGPS)
                        {
                            vehicleMonitorServiceClient.GetLastMonitorGPSAsync(publishedEvent.CarNo, publishedEvent);//去查每一个车的最近的GPS位置
                        }
                    }
                    //++++++16-07-19添加该逻辑，已定位，并点击关注，
                    else
                    {
                        VechileElementLst mlst = GetCarMonitor(ElementLayerDefine.miVERealLocation);
                        car = mlst.GetCarUIElementByUniqueID(publishedEvent.CarNo);
                    }
                    car.AlarmFlag = publishedEvent.IsAlarm;
                    car.AlertFlag = publishedEvent.IsAlert;
                    car.Prov = publishedEvent.Department;
                    SetLocatedCar(car);
                }
                else if (publishedEvent.Op == 0)
                {
                    //Cancel Vehicle Monitoring
                    //Create a message format。
                    if ((publishedEvent.UniqueId != null) && (publishedEvent.UniqueId != ""))
                    {
                        if (CarExists(ElementLayerDefine.miVERealLocation, publishedEvent.UniqueId))
                        {
                            RemoveCarByUniqueID(ElementLayerDefine.miVERealLocation, publishedEvent.UniqueId);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("VechileMemDataOperate", ee);
            }
        }

        /// <summary>
        /// track car
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(TrackCarArgs publishedEvent)
        {
            //TrackCarArsg
            try
            {
                VechileElementLst mlst = GetCarMonitor(publishedEvent.VE);
                if (publishedEvent.UniqueID == "")
                {
                    ClearSelectedCar(publishedEvent.VE);
                }
                else
                {
                    GpsCar car = mlst.GetCarUIElementByUniqueID(publishedEvent.UniqueID);
                    if (car != null)
                    {
                        SetTrackedCar(car);
                        if (!GPSState.Valid(car.Valid))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_UNValidXYWait"), MessageDialogButton.Ok);
                            return;
                        }
                        double dx = 0;
                        double dy = 0;
                        try
                        {
                            dx = double.Parse(car.Lon);
                            dy = double.Parse(car.Lat);
                        }
                        catch (Exception ex)
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_UNValidXYWait"), MessageDialogButton.Ok);
                            return;
                        }
                        _EventAggregator.Publish<LocateEventArgs>(new LocateEventArgs() { VE = publishedEvent.VE, Lat = dy, Lon = dx });
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_UNValidXYWait"), MessageDialogButton.Ok);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// Show newest location specified MDVR, if memory does not take the last one from the database
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(DisplayCurrentPositionArgs publishedEvent)
        {
            //TrackCarArgs
            try
            {
                VechileElementLst mlst = GetCarMonitor((ElementLayerDefine)publishedEvent.VE);
                if (publishedEvent.CarNo != "")
                {
                    GpsCar car = mlst.GetCarUIElementByUniqueID(publishedEvent.CarNo);
                    if ((car != null) && (GPSState.Valid(car.Valid)))//否则 根据状态定位车辆
                    {
                        SetLocatedCar(car);//标记车辆定位状态 IsLocated
                        _EventAggregator.Publish<LocateEventArgs>(new LocateEventArgs() { Operate = MapEventArgs.MapOperateLocateByUniqueID, VE = (ElementLayerDefine)publishedEvent.VE, UniqueID = publishedEvent.CarNo });
                    }
                    else
                    {
                        vehicleMonitorServiceClient.GetLastMonitorGPSAsync(publishedEvent.CarNo, publishedEvent);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        private VehicleMonitorServiceClient vehicleMonitorServiceClient = ServiceClientFactory.Create<VehicleMonitorServiceClient>();
        /// <summary>
        /// GPS coordinates obtained finally filled into the memory, and positioning
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vehicleMonitorServiceClient_GetLastMonitorGPSCompleted(object sender, GetLastMonitorGPSCompletedEventArgs e)
        {
            string vehicleId = "";
            ElementLayerDefine Ve = ElementLayerDefine.miVERealLocation;
            string prov = "";
            if (e.UserState is DisplayCurrentPositionArgs)
            {
                DisplayCurrentPositionArgs dargs = e.UserState as DisplayCurrentPositionArgs;
                vehicleId = dargs.CarNo;
                prov = dargs.Prov;
                Ve = (ElementLayerDefine)dargs.VE;
            }
            else if (e.UserState is RequestVehicleMonitorArgs)
            {
                RequestVehicleMonitorArgs dargs = e.UserState as RequestVehicleMonitorArgs;
                vehicleId = (e.UserState as RequestVehicleMonitorArgs).CarNo;
            }
            else//只处理以上两个消息
            {
                return;
            }

            if ((e.Result != null) && (e.Result.Result != null))
            {
                Gsafety.PTMS.ServiceReference.VehicleMonitorService.GPS antgps = e.Result.Result;
                if (antgps != null)
                {
                    VechileElementLst mlst = GetCarMonitor(Ve);
                    GpsCar car = mlst.GetCarUIElementByUniqueID(vehicleId);
                    if (car == null)
                    {
                        car = AddCar(Ve, vehicleId, vehicleId);
                        car.Prov = prov;
                    }

                    if (!GPSState.Valid(car.Valid))
                    {
                        Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS gps = new Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS();
                        gps.VehicleId = antgps.VehicleId;
                        gps.Direction = antgps.Direction;
                        gps.GpsTime = antgps.GpsTime.Value.ToLocalTime();
                        gps.Latitude = antgps.Latitude;
                        gps.Longitude = antgps.Longitude;
                        gps.Speed = antgps.Speed;
                        gps.Valid = antgps.Valid;
                        gps.Source = antgps.Source;
                        Draw(car, gps, 0, false, false, Ve);
                        //car.IsMonitor = dargs.IsMonitor;
                        SetLocatedCar(car);
                    }
                }
            }

            _EventAggregator.Publish<LocateEventArgs>(new LocateEventArgs() { Operate = MapEventArgs.MapOperateLocateByUniqueID, VE = Ve, UniqueID = vehicleId });
        }


        public void HandleEvent(AlarmGisArgs publishedEvent)
        {
            foreach (GpsCar car in MonitorList.VechileRealLocationElements.Elements)
            {
                if (car.UniqueID == publishedEvent.VehicleID)
                {
                    car.AlarmFlag = (publishedEvent.Alarm > 0);
                    break;
                }
            }
        }

        public void HandleEvent(AlertGisArgs publishedEvent)
        {
            foreach (GpsCar car in MonitorList.VechileRealLocationElements.Elements)
            {
                if (car.UniqueID == publishedEvent.VehicleID)
                {
                    car.AlertFlag = (publishedEvent.Alert > 0);
                    break;
                }
            }
        }
    }
}
