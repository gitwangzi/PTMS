using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using GisManagement.Models;
using GisManagement.Views;
using Gsafety.Common.CommMessage;
using Gsafety.Common.CommMessage.Controls;
using Gsafety.Common.Controls;
using Gsafety.Common.Converts;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.PTMS.ServiceReference.VehicleMonitorService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Gsafety.PTMS.Bases.Enums;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 51ff0116-da8a-4392-bb27-5057e4ebcb47      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: GisManagement.ViewModels
/////    Project Description:    
/////             Class Name: GpsCarHisDataViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/5 19:18:42
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/5 19:18:42
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Reflection;
using System.ServiceModel;
using System.Windows.Controls;
using System.Windows.Media;


namespace GisManagement.ViewModels
{

    [ExportAsViewModel(GisName.GpsCarHisDataViewModel)]
    public class GpsCarHisDataViewModel : BaseViewModel,
        IEventSink<HisTraceArgs>,
        IEventSink<ShowGpsHisSinglePointInfo>,
        IPartImportsSatisfiedNotification
    {
        private ObservableCollection<HisDataStore> _HisDataStoreLst;
        public ObservableCollection<HisDataStore> HisDataStoreLst
        {
            get { return _HisDataStoreLst; }
            set
            {
                _HisDataStoreLst = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => HisDataStoreLst));
            }
        }

        public enum PlayState { miStart, miPause, miStop };
        /// <summary>
        /// Initialization
        /// </summary>
        public GpsCarHisDataViewModel()
        {
            try
            {
                _HisDataStoreLst = new ObservableCollection<HisDataStore>();

                AddCommand = new ActionCommand<object>(obj => AddHisData());
                DeleteCommand = new ActionCommand<object>(obj => DeleteHisData());
                SelectionChangedCommand = new ActionCommand<object>(obj => SelectionChanged());

                HisDataQueryCommand = new ActionCommand<object>(obj => QueryHisData(), obj => { return (_NowState == PlayState.miStop); });
                HisDataPlayStartCommond = new ActionCommand<object>(obj => HisDataPlayStart(), obj => _HisDataPlayStartEnable());
                HisDataPlayPauseCommond = new ActionCommand<object>(obj => HisDataPlayPause(), obj => _HisDataPlayPauseEnable());
                HisDataPlayEndCommond = new ActionCommand<object>(obj => HisDataPlayEnd(), obj => _HisDataPlayEndEnable());
                MarkGraphicCommand = new ActionCommand<object>((obj) => MarkGraphic_Event(obj));
                HisPlayInterval = 1000;
                IsHisPlayAnimation = true;
                //IsHisPlayTrajectory = true;
                IsHisPlayTrajectory = false;

                AlaramClient.GetAlarmGPSTrackCompleted += AlaramClient_GetAlarmGPSTrackCompleted;
                MonitorClient.GetMonitorGPSTrackCompleted += MonitorClient_GetMonitorGPSTrackCompleted;
            }
            catch (System.Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GpsCarHisDataViewModel", ee);
            }
        }
        public IActionCommand MarkGraphicCommand { get; private set; }
        private bool _HisDataPlayEndEnable()
        {
            return ((_NowState == PlayState.miStart) || (_NowState == PlayState.miPause));
        }

        private bool _HisDataPlayPauseEnable()
        {
            return (_NowState == PlayState.miStart);
        }

        private bool _HisDataPlayStartEnable()
        {
            return ((CurrentSelectedHisDataStore != null) && (CurrentSelectedHisDataStore.GPSLst.Count > 0) && ((_NowState == PlayState.miStop) || (_NowState == PlayState.miPause)));
        }

        void MonitorClient_GetMonitorGPSTrackCompleted(object sender, GetMonitorGPSTrackCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    // MonitorClient.CloseAsync();
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("GIS_CarNotFound"));
                    return;
                }
                if (e.Result != null)
                {
                    if ((e.Result.Result != null) && (e.Result.Result.Count > 0))
                    {
                        HisDataStore his = new HisDataStore();
                        his.IsMarkGraphic = false;
                        his.hisTraceArgs = e.UserState as HisTraceArgs;
                        his.GPSLst = new ObservableCollection<Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS>();
                        foreach (Gsafety.PTMS.ServiceReference.VehicleMonitorService.GPS lsgps in e.Result.Result)
                        {
                            Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS gps = new Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS();
                            gps.Direction = lsgps.Direction;
                            gps.GpsTime = lsgps.GpsTime.Value.ToLocalTime();
                            gps.Latitude = lsgps.Latitude;
                            gps.Longitude = lsgps.Longitude;
                            gps.Valid = lsgps.Valid;
                            gps.Speed = lsgps.Speed;
                            gps.Source = lsgps.Source;
                            gps.VehicleId = lsgps.VehicleId;
                            his.GPSLst.Add(gps);
                        }
                        _HisDataStoreLst.Add(his);
                        DrawPreRoute(his.GPSLst, "Pre_" + his.hisTraceArgs.CarNo + "_" + his.hisTraceArgs.StartTime.ToString() + "_" + his.hisTraceArgs.EndTime.ToString(), his.hisTraceArgs.LineColor);
                        EventAggregator.Publish<OpenCarHisViewState>(new OpenCarHisViewState() { State = true });
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_CarNotFound"), MessageDialogButton.Ok);
                    }
                }
            }
            catch (TimeoutException timeoutException)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, timeoutException);
            }
            catch (CommunicationException communicationException)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, communicationException);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }

        }


        /// <summary>
        ///  Draw the route
        /// </summary>
        private void DrawPreRoute(ObservableCollection<Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS> csCarHisData, string ID, Color color)
        {
            try
            {
                ESRI.ArcGIS.Client.Geometry.PointCollection newpts;

                int ind = 0;            
                newpts = new ESRI.ArcGIS.Client.Geometry.PointCollection();
                //绘制点
                foreach (Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS args in csCarHisData)
                {
                 
                    //if (args.Valid == "A")
                    if (GPSState.Valid(args.Valid))
                    {
                        
                        DisplayLonConvert loncon = new DisplayLonConvert();
                        DisplayLatConvert latcon = new DisplayLatConvert();
                        string templon = loncon.ConvertBack(args.Longitude, null, null, null).ToString();
                        string templat = latcon.ConvertBack(args.Latitude, null, null, null).ToString();
                        //3857地图
                        //ESRI.ArcGIS.Client.Geometry.MapPoint pt = VechileMemDataOperate.GetProjCoord(double.Parse(templon), double.Parse(templat));

                        ESRI.ArcGIS.Client.Geometry.MapPoint pt =  new ESRI.ArcGIS.Client.Geometry.MapPoint(double.Parse(templon), double.Parse(templat));
                        newpts.Add(pt);

                        //if (ind == 0)
                        //{


                        //    Graphic textgraphic = new Graphic()
                        //    {
                        //        Geometry = pt,                               
                        //        Symbol = new ESRI.ArcGIS.Client.Symbols.TextSymbol
                        //        {
                        //            Text = ID.Replace("Pre_",""),
                        //            Foreground = new SolidColorBrush(color),
                        //            FontSize = 15
                        //        }
                        //    };

                        //    MonitorList.GpsHisDataVechileGraphics.AddGraphic(textgraphic, ID + "@" + "text-" + ind.ToString());

                        //}                      


                        Graphic newgraphic = new Graphic()
                        {
                            Geometry = pt,
                            MapTip = new TextBlock()
                            {
                                Text = args.GpsTime.Value.ToString(),
                                Foreground = new SolidColorBrush(Colors.Blue),
                            },
                            Symbol = new ESRI.ArcGIS.Client.Symbols.SimpleMarkerSymbol
                            {
                                Style = ESRI.ArcGIS.Client.Symbols.SimpleMarkerSymbol.SimpleMarkerStyle.Circle,
                                Size = 8,
                                Color = new SolidColorBrush(color),
                            }
                        };
                        ind++;
                        MonitorList.GpsHisDataVechileGraphics.AddGraphic(newgraphic, ID + "@" + ind.ToString());
                    }
                }

                //绘制箭头
                for (int i = 0; i < newpts.Count - 1; i++)
                {
                   
                    if (NeedDrawArrow(newpts[i], newpts[i + 1]))
                    {
                        double angleofline = Math.Atan2(newpts[i + 1].Y - newpts[i].Y, newpts[i + 1].X - newpts[i].X);
                        MapPoint pt = new MapPoint();
                        pt.X = (newpts[i].X + newpts[i + 1].X) / 2;
                        pt.Y = (newpts[i].Y + newpts[i + 1].Y) / 2;

                        Graphic arrowgraphic = new Graphic()
                        {
                            Geometry = GetArrowLine(pt, angleofline),
                            Symbol = new ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol()
                            {
                                Style = ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol.LineStyle.Solid,
                                Width = 2,
                                Color = new SolidColorBrush(color),
                            }
                        };
                        MonitorList.GpsHisDataVechileGraphics.AddGraphic(arrowgraphic, ID + "@" + "Arrow-" + i.ToString());
                    }
                }
                //绘制线
                if (newpts.Count > 1) //Two or more points, lines drawn
                {
                    ESRI.ArcGIS.Client.Geometry.Polyline line = new ESRI.ArcGIS.Client.Geometry.Polyline();
                    line.Paths.Add(newpts);

                    Graphic newgraphic = new Graphic()
                    {
                        Geometry = line,
                        Symbol = new ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol()
                        {
                            Style = ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol.LineStyle.Dash,
                            Width = 2,
                            Color = new SolidColorBrush(color),
                        }

                    };
                    MonitorList.GpsHisDataVechileGraphics.AddGraphic(newgraphic, ID);
                    EventAggregator.Publish<LocateGeometryEventArgs>(new LocateGeometryEventArgs() { VE = ElementLayerDefine.miVEHisData, LocateGeometry = line });
                }
            }
            catch (System.Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GpsCarHisDataViewModel", ee);
            }

        }

        private bool NeedDrawArrow(MapPoint pt1, MapPoint pt2)
        {
            //距离超过10m绘制
            if ((Math.Sqrt(Math.Pow(pt2.X - pt1.X, 2) + Math.Pow(pt2.Y - pt1.Y, 2))) > 30) return true;
            return false;
        }
        private ESRI.ArcGIS.Client.Geometry.Polyline GetArrowLine(MapPoint pt, double lineangle)
        {
            int len = 20;
            double angle = Math.PI / 6;

            ESRI.ArcGIS.Client.Geometry.PointCollection newpts = new ESRI.ArcGIS.Client.Geometry.PointCollection();

            MapPoint pt1 = new MapPoint();
            pt1.X = pt.X + len * Math.Cos(Math.PI - angle + lineangle);
            pt1.Y = pt.Y + len * Math.Sin(Math.PI - angle + lineangle);

            MapPoint pt2 = new MapPoint();
            pt2.X = pt.X + len * Math.Cos(Math.PI + angle + lineangle);
            pt2.Y = pt.Y + len * Math.Sin(Math.PI + angle + lineangle);

            newpts.Add(pt1);
            newpts.Add(pt);
            newpts.Add(pt2);
            ESRI.ArcGIS.Client.Geometry.Polyline line = new ESRI.ArcGIS.Client.Geometry.Polyline();
            line.Paths.Add(newpts);
            return line;
        }

        private void AlaramClient_GetAlarmGPSTrackCompleted(object sender, GetAlarmGPSTrackCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    // AlaramClient.CloseAsync();
                    return;
                }
                if (e.Result != null)
                {
                    if ((e.Result.Result != null) && (e.Result.Result.Count > 0))
                    {
                        HisDataStore his = new HisDataStore();
                        his.IsMarkGraphic = false;
                        his.hisTraceArgs = e.UserState as HisTraceArgs;
                        his.GPSLst = new ObservableCollection<Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS>();
                        foreach (Gsafety.PTMS.ServiceReference.VehicleAlarmService.GPS lsgps in e.Result.Result)
                        {
                            Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS gps = new Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS();
                            gps.Direction = lsgps.Direction;
                            gps.GpsTime = lsgps.GpsTime.Value.ToLocalTime();
                            gps.Latitude = lsgps.Latitude;
                            gps.Longitude = lsgps.Longitude;                                                                                                                                                                                                                                                                                                            
                            gps.Valid = lsgps.Valid; 
                            gps.Speed = lsgps.Speed;
                            gps.Source = lsgps.Source;
                            his.GPSLst.Add(gps);
                        }
                        _HisDataStoreLst.Add(his);
                        DrawPreRoute(his.GPSLst, "Pre_" + his.hisTraceArgs.CarNo + "_" + his.hisTraceArgs.StartTime.ToString() + "_" + his.hisTraceArgs.EndTime.ToString(), his.hisTraceArgs.LineColor);
                        EventAggregator.Publish<OpenCarHisViewState>(new OpenCarHisViewState() { State = true });

                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_CarNotFound"), MessageDialogButton.Ok);
                    }
                }
            }
            catch (TimeoutException timeoutException)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, timeoutException);
            }
            catch (CommunicationException communicationException)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, communicationException);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }
        }

        private VehicleAlarmServiceClient AlaramClient = ServiceClientFactory.Create<VehicleAlarmServiceClient>();
        private VehicleMonitorServiceClient MonitorClient = ServiceClientFactory.Create<VehicleMonitorServiceClient>();

        /// <summary>
        /// Deal with historical trajectory
        /// </summary>
        /// <param name="publishedevent"></param>
        public void HandleEvent(HisTraceArgs publishedEvent)
        {
            //publishedEvent.Op = HisTraceOption.Add;
            //publishedEvent.GpsDataType = HisGPSDataType.MonitorGPS;
            try
            {
                HisTraceArgs targs = publishedEvent;
                if (publishedEvent.Op == HisTraceOption.Add)
                {
                    Graphic g = MonitorList.GpsHisDataVechileGraphics.GetGraphics("Pre_" + targs.CarNo + "_" + targs.StartTime.ToString() + "_" + targs.EndTime.ToString());
                    if (g == null)
                    {
                        MonitorClient.GetMonitorGPSTrackAsync(publishedEvent.CarNo, publishedEvent.StartTime.ToUniversalTime(), publishedEvent.EndTime.ToUniversalTime(), publishedEvent);
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Has_HisData"), MessageDialogButton.Ok);
                        return;
                    }
                }
                else if (publishedEvent.Op == HisTraceOption.Delete)
                {
                    RemoveByArgs(publishedEvent);
                    MonitorList.GpsHisDataVechileGraphics.RemoveGraphics("Pre_" + targs.CarNo + "_" + targs.StartTime.ToString() + "_" + targs.EndTime.ToString());
                    MonitorList.GpsHisDataSingleVechileElements.RemoveCarsByPre(targs.CarNo);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void RemoveByArgs(HisTraceArgs publishedEvent)
        {
            foreach (HisDataStore hds in _HisDataStoreLst)
            {
                if ((hds.hisTraceArgs.CarNo == publishedEvent.CarNo) && (hds.hisTraceArgs.StartTime == publishedEvent.StartTime) && (hds.hisTraceArgs.EndTime == publishedEvent.EndTime))
                {
                    _HisDataStoreLst.Remove(hds);
                    break;
                }
            }
        }


        #region Defined attributes

        private PlayState _NowState = PlayState.miStop;
        private GpsCar _HisDataCar = null;

        private HisDataStore _CurrentSelectedHisDataStore;
        public HisDataStore CurrentSelectedHisDataStore
        {
            get
            {
                return _CurrentSelectedHisDataStore;
            }
            set
            {
                CurrentPlayIndex = -1;
                //Clear display
                ClearCarHisDisplay();
                _CurrentSelectedHisDataStore = value;

                //set data
                if (_CurrentSelectedHisDataStore != null)
                {
                    string csUniqueID = "Pre_" + _CurrentSelectedHisDataStore.hisTraceArgs.CarNo + "_" + _CurrentSelectedHisDataStore.hisTraceArgs.StartTime.ToString() + "_" + _CurrentSelectedHisDataStore.hisTraceArgs.EndTime.ToString();

                    List<string> listIDs = null;
                    List<Graphic> list = MonitorList.GpsHisDataVechileGraphics.GetGraphis(csUniqueID, out listIDs);
                    if (list != null && list.Count > 0)
                    {
                        MonitorList.GpsHisDataVechileGraphics.RemoveGraphicsEx(csUniqueID);
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i < listIDs.Count)
                            {
                                MonitorList.GpsHisDataVechileGraphics.AddGraphic(list[i], listIDs[i]);
                            }
                        }
                    }

                    Graphic g = MonitorList.GpsHisDataVechileGraphics.GetGraphics(csUniqueID);
                    if ((g != null) && (g.Geometry != null))
                    {
                        MonitorList.GpsHisDataVechileGraphics.RemoveGraphic(csUniqueID);
                        MonitorList.GpsHisDataVechileGraphics.AddGraphic(g, csUniqueID);
                        EventAggregator.Publish<LocateGeometryEventArgs>(new LocateGeometryEventArgs() { VE = ElementLayerDefine.miVEHisData, LocateGeometry = g.Geometry });
                    }

                    _HisDataCar = new GpsCar(_CurrentSelectedHisDataStore.hisTraceArgs.CarNo);
                    _HisDataCar.UniqueID = _CurrentSelectedHisDataStore.hisTraceArgs.CarNo;
                    _HisDataCar.ElementLayDefine = ElementLayerDefine.miVEHisData;
                    //_HisDataCar.CarStyle = publishedEvent.;
                    _HisDataCar.Graphics = MonitorList.GpsHisDataVechileGraphics;
                    _HisDataCar.onGetNextPointEvent = GetNextValidPoint;
                    MonitorList.GpsHisDataVechileElements.AddCars(_HisDataCar, new ESRI.ArcGIS.Client.Geometry.MapPoint(0, 0));

                }
                _NowState = PlayState.miStop;
                StateChange();
            }
        }


        private void MarkGraphic_Event(object obj)
        {
            try
            {

                if (null != obj)
                {
                    string vehicleId = obj.ToString();
                    string csUniqueID = "Plan_" + _CurrentSelectedHisDataStore.hisTraceArgs.CarNo + "_" + _CurrentSelectedHisDataStore.hisTraceArgs.StartTime.ToString() + "_" + _CurrentSelectedHisDataStore.hisTraceArgs.EndTime.ToString();

                    if (HisDataStoreLst[CurrentSelectIndex].IsMarkGraphic == false)
                    {
                        
                        Gsafety.PTMS.ServiceReference.TrafficManageService.TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<Gsafety.PTMS.ServiceReference.TrafficManageService.TrafficManageServiceClient>();
                        trafficServiceClient.GetDeliveredTrafficRouteListByVehicleIDCompleted += ((sender, e) =>
                        {
                            if (e.Error != null || e.Result.IsSuccess == false)
                            {
                                ApplicationContext.Instance.Logger.LogException("GetDeliveredTrafficRouteListByVehicleIDCompleted", e.Error);
                                return;
                            }
                            if (e.Result.Result.Count == 0)
                            {
                                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_VehicleNoFindRoute"), MessageDialogButton.Ok);
                                return;
                            }

                            SymbolParams parm = new SymbolParams();
                            SymbolStyleSet symbolSelect = new SymbolStyleSet();
                            symbolSelect.ControlTabItemVisbility(0, 1);
                            symbolSelect.ControlTabItemVisbility(2, 1);
                            symbolSelect.Closed += ((o, args) =>
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
                                    return;
                                }

                                foreach (Gsafety.PTMS.ServiceReference.TrafficManageService.TrafficRoute route in e.Result.Result)
                                {
                                    EventAggregator.Publish<MarkTrafficGraphic>(new MarkTrafficGraphic() { nType = TrafficFeature.Traffic_Route, parentId = csUniqueID, childId = route.ID, TrafficRoute = route, bShow = true, MarkSymbolParm = parm });
                                    HisDataStoreLst[CurrentSelectIndex].IsMarkGraphic = true;
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => HisDataStoreLst));
                                }

                                if (e.Result.Result.Count != 0)
                                {
                                    EventAggregator.Publish<ZoomGisView>(new ZoomGisView());
                                }

                            });
                            symbolSelect.Show();
                        });
                        trafficServiceClient.GetDeliveredTrafficRouteListByVehicleIDAsync(vehicleId, ApplicationContext.Instance.AuthenticationInfo.ClientID);
                    }
                    else
                    {

                        HisDataStoreLst[CurrentSelectIndex].IsMarkGraphic = false;
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => HisDataStoreLst));
                        EventAggregator.Publish<MarkTrafficGraphic>(new MarkTrafficGraphic() { nType = TrafficFeature.Traffic_Route, parentId = csUniqueID, childId = "", TrafficRoute = null, bShow = false, MarkSymbolParm = null });

                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private int _CurrentSelectIndex;
        public int CurrentSelectIndex
        {
            get { return _CurrentSelectIndex; }
            set
            {
                _CurrentSelectIndex = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentSelectIndex));
            }
        }

        #endregion


        #region get data
        /// <summary>
        /// Check out the historical data will be written _carHisData; data requirements is based on GPS time ascending
        /// </summary>
        public void QueryHisData()
        {
            //Based QueryDateTimeBegin, QueryDateTimeEnd and QueryCarNo query historical data, write _carHisData;

            //Method One: incoming data
            List<GPSDataEventArgs> tempdata = new List<GPSDataEventArgs>();

            double lat = 2222.222;
            double lon = 11744.22;
            for (int i = 0; i <= 59; i++)
            {
                Random rd = new Random();
                lat = lat + 0.001 * rd.Next(1, 10);
                lon = lon + 0.001 * rd.Next(1, 10);
                GPSDataEventArgs args = new GPSDataEventArgs();
                args.RecLat = lat.ToString();
                args.RecLon = lon.ToString();
                args.Dir = "XXXXXX";
                args.V = "65";
                args.GpsTime = DateTime.Now.AddDays(-1).AddSeconds(2 * i).ToString();
                args.Valid = "A";
                tempdata.Add(args);
            }

            EventAggregator.Publish<ReplayHisGpsDataEvent>(new ReplayHisGpsDataEvent() { CarNo = ApplicationContext.Instance.StringResourceReader.GetString("Ce12345"), CarStyle = Gsafety.PTMS.Bases.Enums.VehicleType.Taxi, WithGpsData = true, HisData = tempdata });
        }

        /// <summary>
        /// Historical data show clear
        /// </summary>
        private void ClearCarHisDisplay()
        {
            try
            {
                if (CurrentSelectedHisDataStore == null) return;
                GpsCar car = MonitorList.GpsHisDataVechileElements.GetCarUIElementByUniqueID(CurrentSelectedHisDataStore.hisTraceArgs.CarNo);
                if (car != null)
                {
                    car.sb.Stop();
                    MonitorList.GpsHisDataVechileGraphics.RemoveGraphic(car.CarNo);
                    MonitorList.GpsHisDataVechileElements.RemoveCars(car);
                }
            }
            catch (System.Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GpsCarHisDataViewModel", ee);
            }
        }

        #endregion


        public IActionCommand AddCommand { get; private set; }
        public IActionCommand DeleteCommand { get; private set; }
        public IActionCommand SelectionChangedCommand { get; private set; }



        //public List<>
        #region Playback Control

        /// <summary>
        /// Historical Data Query button
        /// </summary>
        public IActionCommand HisDataQueryCommand { get; private set; }

        /// <summary>
        /// play button
        /// </summary>
        public IActionCommand HisDataPlayStartCommond { get; private set; }
        /// <summary>
        /// Pause Button
        /// </summary>
        public IActionCommand HisDataPlayPauseCommond { get; private set; }
        /// <summary>
        /// End button
        /// </summary>
        public IActionCommand HisDataPlayEndCommond { get; private set; }


        private void AddHisData()
        {
            HistoricalRoute window = new HistoricalRoute("", true);
            //window.VehicleId.Text = SelectedVehicleId;
            window.Closed += new EventHandler(HistoricalRoute_Closed);
            window.Show();
            EventAggregator.Publish<RefreshTrafficSelectStatus>(new RefreshTrafficSelectStatus() { });
        }


        private void DeleteHisData()
        {
            if (_CurrentSelectedHisDataStore != null)
            {
                if (_NowState != PlayState.miStop)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_StopBeforeDelete"), MessageDialogButton.Ok);
                    return;
                }
                HisTraceArgs msg = _CurrentSelectedHisDataStore.hisTraceArgs;
                msg.Op = HisTraceOption.Delete;
                EventAggregator.Publish<HisTraceArgs>(msg);
            }
        }

        private void SelectionChanged()
        {
            //MessageBox.Show("SelectionChanged");
        }

        private void HistoricalRoute_Closed(object sender, EventArgs e)
        {
            HistoricalRoute window = sender as HistoricalRoute;
            window.Closed -= HistoricalRoute_Closed;
            if (window != null && window.DialogResult == true)
            {
                HisTraceArgs msg = window.HistraceArgs;
                msg.Op = HisTraceOption.Add;
                EventAggregator.Publish<HisTraceArgs>(msg);
            }
            EventAggregator.Publish<RefreshTrafficSelectStatus>(new RefreshTrafficSelectStatus() { });
        }

        private void HisCarDataGrid_SelectionChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Stop playing
        /// </summary>
        private void HisDataPlayEnd()
        {
            _HisDataCar.sb.Stop();
            _NowState = PlayState.miStop;
            StateChange();

            if (CurrentSelectedHisDataStore.GPSLst.Count > 0)
            {
                CurrentPlayIndex = 0;
            }
            else
            {
                CurrentPlayIndex = -1;
            }



        }
        /// <summary>
        /// Pause playback
        /// </summary>
        private void HisDataPlayPause()
        {
            _HisDataCar.sb.Pause();
            _NowState = PlayState.miPause;
            StateChange();
        }
        /// <summary>
        /// Start playing
        /// </summary>
        private void HisDataPlayStart()
        {
            if (_NowState == PlayState.miPause)
            {
                _HisDataCar.sb.Resume();
            }
            else
            {
                if (CurrentSelectedHisDataStore.GPSLst.Count == 0) return;
                CurrentPlayIndex = 0;

                ClearCarHisDisplay();
                MonitorList.GpsHisDataVechileElements.AddCars(_HisDataCar, new ESRI.ArcGIS.Client.Geometry.MapPoint(0, 0));
                _HisDataCar.HasDraw = false;

                CurrentPlayIndex = -1;
                CurrentPlayIndex = GetNextValidPointIndex(CurrentPlayIndex);
                if ((CurrentPlayIndex <= -1) || (CurrentPlayIndex > CurrentSelectedHisDataStore.GPSLst.Count - 1)) return;

                Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS args = CurrentSelectedHisDataStore.GPSLst[CurrentPlayIndex];
                //if (args.Valid == "A")
                if (GPSState.Valid(args.Valid))
                {
                    Draw(_HisDataCar, args, HisPlayInterval, IsHisPlayTrajectory, IsHisPlayAnimation);
                }
            }
            _NowState = PlayState.miStart;
            StateChange();

        }

        private void GetMouseLeftDownPoint(double dx, double dy, bool bVisble)
        {
            if (CurrentSelectedHisDataStore == null || CurrentSelectedHisDataStore.GPSLst.Count == 0) return;
            DisplayLonConvert loncon = new DisplayLonConvert();
            DisplayLatConvert latcon = new DisplayLatConvert();
            for (int i = CurrentSelectedHisDataStore.GPSLst.Count - 1; i >= 0; i--)
            {
                string templon = loncon.ConvertBack(CurrentSelectedHisDataStore.GPSLst[i].Longitude, null, null, null).ToString();
                string templat = latcon.ConvertBack(CurrentSelectedHisDataStore.GPSLst[i].Latitude, null, null, null).ToString();
                double x2 = double.Parse(templon);
                double y2 = double.Parse(templat);
                double dDist = GetDistTwoPoint(dx, dy, x2, y2);
                if (dDist < 0.00001)
                {
                    GpsCar findCar = MonitorList.GpsHisDataSingleVechileElements.GetCarUIElementByUniqueIDAndGpsTime(_CurrentSelectedHisDataStore.hisTraceArgs.CarNo, CurrentSelectedHisDataStore.GPSLst[i].GpsTime.ToString());
                    if (bVisble == true)
                    {
                        if (findCar == null)
                        {
                            GpsCar car = new GpsCar(_CurrentSelectedHisDataStore.hisTraceArgs.CarNo);
                            car.UniqueID = _CurrentSelectedHisDataStore.hisTraceArgs.CarNo;
                            car.ElementLayDefine = ElementLayerDefine.miVEHisData;
                            car.HasDraw = true;
                            //_HisDataCar.CarStyle = publishedEvent.;
                            // car.Graphics = MonitorList.GpsHisDataVechileGraphics;
                            //car.onGetNextPointEvent = GetNextValidPoint;

                            //3857地图
                            //MapPoint ptPro = VechileMemDataOperate.GetProjCoord(dx, dy);

                            MapPoint ptPro = new MapPoint(dx, dy);
                            MonitorList.GpsHisDataSingleVechileElements.AddCars(car, ptPro);

                            car.RecDateTime = CurrentSelectedHisDataStore.GPSLst[i].GpsTime.ToString();
                            car.UpdateGpsInfo(CurrentSelectedHisDataStore.GPSLst[i]);
                            //car.RecLon = CurrentSelectedHisDataStore.GPSLst[i].Longitude;
                            //car.RecLat = CurrentSelectedHisDataStore.GPSLst[i].Latitude;
                            //car.GpsTime = CurrentSelectedHisDataStore.GPSLst[i].GpsTime.ToString();
                            //car.Speed = CurrentSelectedHisDataStore.GPSLst[i].Speed;
                            //car.Dir = CurrentSelectedHisDataStore.GPSLst[i].Direction;


                            car.RefreshDisplay();
                            EventAggregator.Publish<RefreshEvent>(new RefreshEvent() { });
                        }
                    }
                    else
                    {
                        if (findCar != null)
                        {
                            MonitorList.GpsHisDataSingleVechileElements.RemoveCars(findCar);
                            EventAggregator.Publish<RefreshEvent>(new RefreshEvent() { });
                        }
                    }
                    break;
                }
            }
        }

        private double GetDistTwoPoint(double dx1, double dy1, double dx2, double dy2)
        {
            return Math.Sqrt((dy2 - dy1) * (dy2 - dy1) + (dx2 - dx1) * (dx2 - dx1));
        }
        /// <summary>
        /// Get the next available node
        /// </summary>
        /// <param name="StartIndex"></param>
        /// <returns></returns>
        private int GetNextValidPointIndex(int StartIndex)
        {
            int CurIndex = StartIndex;
            while (true)
            {
                CurIndex += 1;
                if (CurIndex > CurrentSelectedHisDataStore.GPSLst.Count - 1) break;

                //if (CarHisData[CurIndex].Valid == "A") break;
                if (GPSState.Valid(CurrentSelectedHisDataStore.GPSLst[CurIndex].Valid)) break;
            }
            return CurIndex;
        }


        private void GetNextValidPoint(out bool sucess, out MapPoint pt)
        {
            while (true)
            {
                CurrentPlayIndex += 1;
                sucess = false;
                pt = null;
                if (CurrentPlayIndex > CurrentSelectedHisDataStore.GPSLst.Count - 1)
                {
                    HisDataPlayEnd();
                    return;
                }

                //if (CarHisData[CurrentPlayIndex].Valid == "A")
                if (GPSState.Valid(CurrentSelectedHisDataStore.GPSLst[CurrentPlayIndex].Valid))
                {
                    sucess = true;

                    DisplayLonConvert loncon = new DisplayLonConvert();
                    DisplayLatConvert latcon = new DisplayLatConvert();
                    string templon = loncon.ConvertBack(CurrentSelectedHisDataStore.GPSLst[CurrentPlayIndex].Longitude, null, null, null).ToString();
                    string templat = latcon.ConvertBack(CurrentSelectedHisDataStore.GPSLst[CurrentPlayIndex].Latitude, null, null, null).ToString();
                    _HisDataCar.RecLon = CurrentSelectedHisDataStore.GPSLst[CurrentPlayIndex].Longitude;
                    _HisDataCar.RecLat = CurrentSelectedHisDataStore.GPSLst[CurrentPlayIndex].Latitude;
                    _HisDataCar.RecDateTime = CurrentSelectedHisDataStore.GPSLst[CurrentPlayIndex].GpsTime.ToString();
                    _HisDataCar.GpsTime = CurrentSelectedHisDataStore.GPSLst[CurrentPlayIndex].GpsTime.ToString();
                    _HisDataCar.Speed = CurrentSelectedHisDataStore.GPSLst[CurrentPlayIndex].Speed;
                    _HisDataCar.Dir = CurrentSelectedHisDataStore.GPSLst[CurrentPlayIndex].Direction;
                    _HisDataCar.RefreshDisplay();

                    //3857地图
                   // pt = VechileMemDataOperate.GetProjCoord(double.Parse(templon), double.Parse(templat));

                    pt = new MapPoint(double.Parse(templon), double.Parse(templat));

                    TraceCar(templon, templat);
                    return;
                }
            }
        }

        /// <summary>
        /// Change status
        /// </summary>
        private void StateChange()
        {
            HisDataPlayStartCommond.RaiseCanExecuteChanged();
            HisDataPlayPauseCommond.RaiseCanExecuteChanged();
            HisDataPlayEndCommond.RaiseCanExecuteChanged();
            //HisDataQueryCommand.RaiseCanExecuteChanged();
        }

        private int _CurrentPlayIndex;
        /// <summary>
        /// Currently playing entry
        /// </summary>
        public int CurrentPlayIndex
        {
            get
            {
                return _CurrentPlayIndex;
            }
            set
            {
                _CurrentPlayIndex = value;
                RaisePropertyChanged("CurrentPlayIndex");
            }
        }

        /// <summary>
        /// Whether the control track
        /// </summary>
        public bool IsHisPlayTrajectory { get; set; }
        /// <summary>
        /// Are animation
        /// </summary>
        public bool IsHisPlayAnimation { get; set; }


        /// <summary>
        /// Play Interval
        /// </summary>
        private int _HisPlayInterval;
        public int HisPlayInterval
        {
            get
            {
                return _HisPlayInterval;
            }
            set
            {
                _HisPlayInterval = value;
                if (_HisDataCar != null)
                {
                    _HisDataCar.Interval = value;
                }
            }
        }


        /// <summary>
        /// Positioning the vehicle on the map
        /// </summary>
        /// <param name="car"></param>
        public void Locate(string csLon, string csLat)
        {
            if ((csLon != "") && (csLat != ""))
            {
                EventAggregator.Publish<LocateEventArgs>(new LocateEventArgs() { VE = ElementLayerDefine.miVEHisData, Lat = double.Parse(csLat), Lon = double.Parse(csLon) });
            }
        }
        /// <summary>
        /// Vehicle Tracking
        /// </summary>
        /// <param name="csLon"></param>
        /// <param name="csLat"></param>
        public void TraceCar(string csLon, string csLat)
        {
            if ((csLon != "") && (csLat != ""))
            {
                EventAggregator.Publish<LocateEventArgs>(new LocateEventArgs() { VE = ElementLayerDefine.miVEHisData, Operate = LocateEventArgs.MapOperateTraceCar, Lat = double.Parse(csLat), Lon = double.Parse(csLon) });
            }
        }

        /// <summary>
        /// Subscribe to event
        /// </summary>
        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<HisTraceArgs>(this);
            EventAggregator.SubscribeOnDispatcher<ShowGpsHisSinglePointInfo>(this);
        }
        #endregion

        /// <summary>
        /// Historical data to draw
        /// </summary>
        /// <param name="car"></param>
        /// <param name="args"></param>
        /// <param name="csInterval"></param>
        /// <param name="csIsTrajectory"></param>
        /// <param name="csIsAnimation"></param>
        public void Draw(GpsCar car, Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS args, int csInterval, bool csIsTrajectory, bool csIsAnimation)
        {
            try
            {
                car.Valid = args.Valid;

                //if (car.Valid != "A") return;//If Gps coordinates invalid exit
                if (!GPSState.Valid(car.Valid)) return;

                car.UpdateGpsInfo(args);
                //car.Dir = args.Direction;
                //car.Speed = args.Speed;
                //car.RecLat = args.Latitude;
                //car.RecLon = args.Longitude;
                //car.GpsTime = args.GpsTime.ToString();

                double lslon = double.Parse(car.Lon);
                double lslat = double.Parse(car.Lat);
                //3857地图
                //ESRI.ArcGIS.Client.Geometry.MapPoint pt = VechileMemDataOperate.GetProjCoord(lslon, lslat);

                ESRI.ArcGIS.Client.Geometry.MapPoint pt = new ESRI.ArcGIS.Client.Geometry.MapPoint(lslon, lslat);


                ESRI.ArcGIS.Client.Geometry.Envelope env = ElementLayer.GetEnvelope(car);
                ESRI.ArcGIS.Client.Geometry.MapPoint _OldPosition = pt;
                if (car.HasDraw == true)
                {
                    _OldPosition = new ESRI.ArcGIS.Client.Geometry.MapPoint(env.XMin, env.YMin);
                }

                car.UpdateCarRotate(_OldPosition, pt);
                if (csIsAnimation)
                {
                    if (!car.HasDraw) ElementLayer.SetEnvelope(car, new ESRI.ArcGIS.Client.Geometry.Envelope(pt, pt));
                    car.DrawTo(_OldPosition, pt, csInterval, csIsTrajectory);
                }
                else
                {
                    if (csIsTrajectory)
                    {
                        car.DrawRoute(_OldPosition, pt, false, true);
                    }
                    ElementLayer.SetEnvelope(car, new ESRI.ArcGIS.Client.Geometry.Envelope(pt, pt));
                }

                car.HasDraw = true;
                Locate(_HisDataCar.Lon, _HisDataCar.Lat);

            }
            catch (System.Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GpsCarHisDataViewModel", ee);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(ShowGpsHisSinglePointInfo publishedEvent)
        {
            //3857地图
            //MapPoint ptNew = VechileMemDataOperate.GetGeoCoord(publishedEvent.PX, publishedEvent.PY);
            MapPoint ptNew =  new MapPoint(publishedEvent.PX, publishedEvent.PY);
            GetMouseLeftDownPoint(ptNew.X, ptNew.Y, publishedEvent.Visble);
        }
    }

    public class HisDataStore : System.ComponentModel.INotifyPropertyChanged
    {
        public HisTraceArgs hisTraceArgs { get; set; }
        public ObservableCollection<Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS> GPSLst;


        public bool isMarkGraphic = false;


        public bool IsMarkGraphic
        {
            get { return isMarkGraphic; }
            set
            {
                isMarkGraphic = value;
                this.RaisePropertyChanged("IsMarkGraphic");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

    }

    /// <summary>
    /// Open the historical trajectory of the vehicle
    /// </summary>
    public class OpenCarHisViewState
    {

        public bool State { get; set; }

    }

}
