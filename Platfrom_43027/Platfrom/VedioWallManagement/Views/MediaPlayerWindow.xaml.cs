using ESRI.ArcGIS.Client;
using Gsafety.Common.Converts;
using Gsafety.PTMS.Enums;
using Gsafety.PTMS.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections;
using System.ComponentModel.Composition;
using Jounce.Core.Event;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Symbols;
using Gsafety.Common.Controls;
using GisManagement.Views;
using GisManagement.Models;
using GisManagement.ViewModels;
using Gsafety.PTMS.VideoManagement.ViewModels;
using Gsafety.PTMS.ServiceReference.MessageServiceExt;
using Gsafety.PTMS.ServiceReference.VehicleMonitorService;
using System.ServiceModel;
using Gsafety.Common.Map;

namespace Gsafety.PTMS.VideoManagement.Views
{
    public partial class MediaPlayerWindow : FloatableWindow
    {
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        private MediaPlayerContainer _playerContainer;
        private Dictionary<string, Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS> _GPSDict = new Dictionary<string, Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS>();
        private GpsCar _HisDataCar;
        private DateTime _HisCarStartTime;
        private string _VehicleId;
        private bool _showHistoryLine;

        public MediaPlayerWindow(MediaInfo info, string carNo = "", bool showHistoryLine = true)
        {
            InitializeComponent();

            _showHistoryLine = showHistoryLine;

            _playerContainer = new MediaPlayerContainer(info, false);
            LayoutRoot.Children.Add(_playerContainer);

            if (info.IsHideProgressControl == false && showHistoryLine)
            {
                mapGrid.Visibility = Visibility.Visible;

                MonitorList.VedioReplayGPSRoutelements.Clear();
                MonitorList.VedioReplayGPSRouteGraphics.Clear();

                ReplayGraphicsLayer = MyMap.Layers.FirstOrDefault(t => t.ID == "GPSReplayGraphics") as GraphicsLayer;

                mapGrid.Visibility = Visibility.Visible;
                InitMap();
                _VehicleId = carNo;
                _HisCarStartTime = _playerContainer.MinTime.Value;
                VehicleMonitorServiceClient MonitorClient = ServiceClientFactory.Create<VehicleMonitorServiceClient>();
                MonitorClient.GetMonitorGPSTrackCompleted += MonitorClient_GetMonitorGPSTrackCompleted;
                //多拿一分钟的数据
                MonitorClient.GetMonitorGPSTrackAsync(_VehicleId, _playerContainer.MinTime.Value.ToUniversalTime().AddMinutes(-1), _playerContainer.MaxTime.Value.ToUniversalTime().AddMinutes(1));

                this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;

                _playerContainer.OnProcessChanged = UpdateHisGPSCarLocation;
            }
            else
            {
                mapGrid.Visibility = Visibility.Collapsed;
            }

        }

        private void ChildWindow_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        void MonitorClient_GetMonitorGPSTrackCompleted(object sender, GetMonitorGPSTrackCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("GIS_CarNotFound"));
                    return;
                }
                if (e.Result != null)
                {
                    if ((e.Result.Result != null) && (e.Result.Result.Count > 0))
                    {
                        List<Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS> gpslist = new List<ServiceReference.MessageServiceExt.GPS>();
                        foreach (Gsafety.PTMS.ServiceReference.VehicleMonitorService.GPS lsgps in e.Result.Result)
                        {
                            if (GPSState.Valid(lsgps.Valid) == false)
                            {
                                continue;
                            }

                            Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS gps = new Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS();
                            gps.Direction = lsgps.Direction;
                            gps.GpsTime = lsgps.GpsTime.Value.ToLocalTime();
                            gps.Latitude = lsgps.Latitude;
                            gps.Longitude = lsgps.Longitude;
                            gps.Valid = lsgps.Valid;
                            gps.Speed = lsgps.Speed;
                            gps.Source = lsgps.Source;
                            gpslist.Add(gps);
                        }
                        List<Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS> Sortedgpslist = gpslist.OrderBy(n => n.GpsTime.Value).ToList();
                        InitRouteAndData(Sortedgpslist, _VehicleId, Colors.Red, Colors.Green);
                        if (_GPSDict.ContainsKey(_HisCarStartTime.ToString("yyyyMMddHHmmss")))
                        {
                            DrawPreCar(_VehicleId, _HisCarStartTime);
                        }
                        else
                        {
                            if (Sortedgpslist.Count > 0) DrawPreCar(_VehicleId, Sortedgpslist[0].GpsTime.Value);
                        }
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("GIS_CarNotFound"));
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
        /// 绘制车辆初始位置
        /// </summary>
        /// <param name="VechileId"></param>
        private void DrawPreCar(string VechileId, DateTime startdt)
        {
            if (_GPSDict.Count > 0)
            {
                _HisDataCar = new GpsCar(VechileId);
                _HisDataCar.HasDraw = true;
                _HisDataCar.UniqueID = VechileId;
                _HisDataCar.ElementLayDefine = ElementLayerDefine.miVEHisData;
                _HisDataCar.Graphics = MonitorList.VedioReplayGPSRouteGraphics;
                _HisDataCar.RefreshDisplay();
                MapPoint pt = new MapPoint(0, 0);
                MonitorList.VedioReplayGPSRoutelements.AddCars(_HisDataCar, pt);
                UpdateHisGPSCarLocation(startdt);
            }
        }


        Graphic _Clickedgraphic;
        TimeSpan _ClickedElapsedTime;
        DateTime? _ClickedStartTime;
        /// <summary>
        /// 鼠标左键点击执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GpsHisGraphicsLayer_MouseLeftButtonDown(object sender, GraphicMouseButtonEventArgs e)
        {
            if (_Clickedgraphic == null || _Clickedgraphic != e.Graphic)
            {
                _Clickedgraphic = e.Graphic;
                _ClickedStartTime = DateTime.Now;
            }
            else if (_Clickedgraphic == e.Graphic && _ClickedStartTime.HasValue && e.Graphic != null && e.Graphic.Geometry != null)
            {
                _ClickedElapsedTime = DateTime.Now.Subtract(_ClickedStartTime.Value);
                if (_ClickedElapsedTime.TotalMilliseconds < 1000)//1秒内双击
                {
                    if (e.Graphic != null)
                    {
                        if (e.Graphic.Geometry is MapPoint)
                        {
                            MapPoint pt = e.Graphic.Geometry as MapPoint;
                            DateTime nowdatetime = DateTime.Parse(e.Graphic.Attributes["Datetime"].ToString());
                            UpdateHisGPSCarLocation(nowdatetime);
                            _playerContainer.SeekTarget(nowdatetime);

                            e.Handled = true;
                        }
                    }
                }

                _Clickedgraphic = null;
                _ClickedStartTime = null;
            }

        }

        /// <summary>
        /// 依据时间，更新GPS位置
        /// </summary>
        /// <param name="dt"></param>
        public void UpdateHisGPSCarLocation(DateTime dt)
        {
            string key = dt.ToString("yyyyMMddHHmmss");
            if (_GPSDict.ContainsKey(key))
            {
                if (_HisDataCar != null)
                {
                    _HisDataCar.UpdateGpsInfo(_GPSDict[key]);
                    if ((_HisDataCar.Lon != "") && (_HisDataCar.Lat != ""))
                    {
                        double lslon = double.Parse(_HisDataCar.Lon);
                        double lslat = double.Parse(_HisDataCar.Lat);

                        //3857地图
                        MapPoint pt = GetProjCoord(lslon, lslat);

                        //MapPoint pt = new MapPoint(lslon, lslat);
                        ElementLayer.SetEnvelope(_HisDataCar, new ESRI.ArcGIS.Client.Geometry.Envelope(pt, pt));
                    }
                    //MyMap.PanTo(pt);
                }
            }
        }


        protected override void OnClosed(EventArgs e)
        {
            if (_playerContainer != null)
            {
                _playerContainer.OnClosed();
                LayoutRoot.Children.Remove(_playerContainer);
                _playerContainer = null;
            }

            MonitorList.VedioReplayGPSRoutelements.Clear();
            MonitorList.VedioReplayGPSRouteGraphics.Clear();

            if (ReplayGraphicsLayer != null)
            {
                ReplayGraphicsLayer.ClearValue(GraphicsLayer.GraphicsSourceProperty);
            }

            base.OnClosed(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var chromeBorder = this.GetTemplateChild("Chrome") as Border;
            chromeBorder.Background = this.Background;

            var contentPresenter = this.GetTemplateChild("ContentPresenter") as ContentPresenter;
            var border = contentPresenter.Parent as Border;
            border.Margin = new Thickness(0);
        }

        /// <summary>
        /// 初始化图层
        /// </summary>
        private void InitMap()
        {
            ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, string.Empty, "Begin Init");
            if (ApplicationContext.Instance.ServerConfig.BaseMapType == BaseMapTypeEnum.BingMap)
            {
                BingMapLoad.InitBingMap(MyMap);

                Envelope env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.MapInitExtent);
                if (env != null)
                {
                    MyMap.Extent = env;
                }
            }
            else if (ApplicationContext.Instance.ServerConfig.BaseMapType == BaseMapTypeEnum.GoogleMap)
            {
                ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, string.Empty, "GoogleMap");
                GoogleMap layer = new GoogleMap();
                layer.BaseURL = ApplicationContext.Instance.ServerConfig.MapSubType;
                MyMap.Layers.Insert(0, layer);

                ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, string.Empty, "After Insert Base Map");
                Envelope env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.MapInitExtent);
                if (env != null)
                {
                    MyMap.Extent = env;
                }
            }
            else if (ApplicationContext.Instance.ServerConfig.BaseMapType == BaseMapTypeEnum.BaiduMap)
            {
                BaiduMap Alayer = new BaiduMap();
                Alayer.MapType = ApplicationContext.Instance.ServerConfig.MapSubType;
                Alayer.Visible = true;
                MyMap.Layers.Insert(0, Alayer);

                //中国范围  
                Envelope env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.MapInitExtent);
                env.SpatialReference = new SpatialReference(102100);
                //Envelope FullExtent = new ESRI.ArcGIS.Client.Geometry.Envelope(9001735.65795624, 2919532.04645186, 19020489.8293508, 8346937.81802098)
                //{
                //    SpatialReference = new SpatialReference(102100)
                //};
                this.MyMap.ZoomTo(env);

                if (env != null)
                {
                    MyMap.Extent = env;
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<esri:ArcGISTiledMapServiceLayer xmlns:esri=\"http://schemas.esri.com/arcgis/client/2009\" ID=\"DynamicMapLayer\" />");


                //ArcGISDynamicMapServiceLayer TiledMap = (ArcGISDynamicMapServiceLayer)XamlReader.Load(sb.ToString());
                //TiledMap.Initialized += ArcGISDynamicMapServiceLayer_Initialized;
                //TiledMap.InitializationFailed += ArcGISServiceLayer_InitializationFailed;
                //MyMap.Layers.Insert(0, TiledMap);

                ArcGISTiledMapServiceLayer TiledMap = (ArcGISTiledMapServiceLayer)XamlReader.Load(sb.ToString());
                TiledMap.Initialized += ArcGISDynamicMapServiceLayer_Initialized;
                TiledMap.InitializationFailed += ArcGISServiceLayer_InitializationFailed;
                MyMap.Layers.Insert(0, TiledMap);
            }

            string CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            string strMax = ApplicationContext.Instance.ServerConfig.MaxResolution;
            string strMin = ApplicationContext.Instance.ServerConfig.MinResolution;

            strMax = strMax.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            strMin = strMin.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);

            MyMap.MinimumResolution = Convert.ToDouble(strMin);
            MyMap.MaximumResolution = Convert.ToDouble(strMax);
        }
        /// <summary>
        /// 初始化底图图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArcGISDynamicMapServiceLayer_Initialized(object sender, EventArgs e)
        {
            if (ApplicationContext.Instance.ServerConfig.BaseMapType == BaseMapTypeEnum.ArcGisMap)
            {
                if ((MyMap.Layers["DynamicMapLayer"]) is ArcGISDynamicMapServiceLayer)
                {
                    ArcGISDynamicMapServiceLayer arcgisLayer = MyMap.Layers["DynamicMapLayer"] as ArcGISDynamicMapServiceLayer;
                    arcgisLayer.Url = ApplicationContext.Instance.ServerConfig.GisBaseMapUrl;

                }
                else if ((MyMap.Layers["DynamicMapLayer"]) is ArcGISTiledMapServiceLayer)
                {
                    ArcGISTiledMapServiceLayer arcgisLayer = MyMap.Layers["DynamicMapLayer"] as ArcGISTiledMapServiceLayer;
                    arcgisLayer.Url = ApplicationContext.Instance.ServerConfig.GisBaseMapUrl;
                    MyMap.Extent = arcgisLayer.FullExtent;

                }
            }
        }

        /// <summary>
        /// GPS是否有效
        /// </summary>
        /// <param name="csvalid"></param>
        /// <returns></returns>
        public bool GPSIsValid(string csvalid)
        {
            if (csvalid == "N") return false;
            return true;
        }
        /// <summary>
        /// 转换为投影坐标
        /// </summary>
        /// <param name="csLon"></param>
        /// <param name="csLat"></param>
        /// <returns></returns>
        public static ESRI.ArcGIS.Client.Geometry.MapPoint GetProjCoord(double csLon, double csLat)
        {
            ESRI.ArcGIS.Client.Geometry.MapPoint pt = new ESRI.ArcGIS.Client.Geometry.MapPoint(csLon, csLat);
            //return ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(pt);
            return Gsafety.Common.Transform.GeographicToWebMercator(pt);
        }

        /// <summary>
        /// 绘制路线及计算每秒的位置数据
        /// </summary>
        /// <param name="csCarHisData">要求GPS列表必须按照时间从小到大排列</param>
        /// <param name="ID"></param>
        /// <param name="LineColor"></param>
        private void InitRouteAndData(List<Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS> csCarHisData, string ID, Color LineColor, Color CircleColor)
        {
            try
            {
                ESRI.ArcGIS.Client.Geometry.PointCollection newpts;

                newpts = new ESRI.ArcGIS.Client.Geometry.PointCollection();
                for (int i = 0; i <= csCarHisData.Count - 1; i++)
                {
                    if (i < csCarHisData.Count - 1) //从第一点至最后一点
                    {
                        AddToGPSDict(csCarHisData[i], csCarHisData[i + 1]);
                    }


                    Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS args = csCarHisData[i];
                    //if (args.Valid == "A")
                    if (GPSIsValid(args.Valid))
                    {
                        DisplayLonConvert loncon = new DisplayLonConvert();
                        DisplayLatConvert latcon = new DisplayLatConvert();
                        string templon = loncon.ConvertBack(args.Longitude, null, null, null).ToString();
                        string templat = latcon.ConvertBack(args.Latitude, null, null, null).ToString();

                        //3857地图
                        ESRI.ArcGIS.Client.Geometry.MapPoint pt = GetProjCoord(double.Parse(templon), double.Parse(templat));

                        //ESRI.ArcGIS.Client.Geometry.MapPoint pt = new  ESRI.ArcGIS.Client.Geometry.MapPoint(double.Parse(templon), double.Parse(templat));

                        newpts.Add(pt);

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
                                Color = new SolidColorBrush(CircleColor),
                            }
                        };
                        newgraphic.Attributes.Add("Datetime", args.GpsTime.Value);
                        MonitorList.VedioReplayGPSRouteGraphics.AddGraphic(newgraphic, ID + "@" + args.GpsTime.Value.ToString());
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
                                Color = new SolidColorBrush(LineColor),
                            }
                        };
                        MonitorList.VedioReplayGPSRouteGraphics.AddGraphic(arrowgraphic, ID + "@" + "Arrow-" + i.ToString());
                    }
                }


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
                            Color = new SolidColorBrush(LineColor),
                        }
                    };
                    MonitorList.VedioReplayGPSRouteGraphics.AddGraphic(newgraphic, ID);
                    //MyMap.ZoomTo(line);
                    ZoomToGeometry(line);

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

        private void MyMap_ExtentChanged(object sender, ExtentEventArgs e)
        {
            double dWc = Convert.ToDouble("0.01".Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
            if (MyMap.Resolution < (MyMap.MinimumResolution - dWc))
            {
                MyMap.ZoomToResolution(MyMap.MinimumResolution, e.NewExtent.GetCenter());
                return;
            }
        }



        //private void MyMap_Changed(object sender, ExtentEventArgs e)
        //{
        //    if (ApplicationContext.Instance.ServerConfig.BaseMapType == BaseMapTypeEnum.ArcGisMap)
        //    {
        //        if ((MyMap.Layers["DynamicMapLayer"]) is ArcGISDynamicMapServiceLayer)
        //        {
        //            ArcGISDynamicMapServiceLayer arcgisLayer = MyMap.Layers["DynamicMapLayer"] as ArcGISDynamicMapServiceLayer;
        //            arcgisLayer.Url = ApplicationContext.Instance.ServerConfig.DomGisBaseMapUrl;

        //        }
        //        else if ((MyMap.Layers["DynamicMapLayer"]) is ArcGISTiledMapServiceLayer)
        //        {
        //            ArcGISTiledMapServiceLayer arcgisLayer = MyMap.Layers["DynamicMapLayer"] as ArcGISTiledMapServiceLayer;
        //            arcgisLayer.Url = ApplicationContext.Instance.ServerConfig.DomGisBaseMapUrl;
        //            MyMap.Extent = arcgisLayer.FullExtent;

        //        }
        //    }
        //}
        /// <summary>
        /// 定位到对象
        /// </summary>
        /// <param name="line"></param>
        private void ZoomToGeometry(ESRI.ArcGIS.Client.Geometry.Polyline line)
        {
            string strLocal = ApplicationContext.Instance.ServerConfig.AutoLocateResolution;
            strLocal = strLocal.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            if (MyMap.Resolution > Convert.ToDouble(strLocal))
            {
                CenterAndZoom(Convert.ToDouble(strLocal), line.Extent.GetCenter());
            }
            else
            {
                MyMap.PanTo(line.Extent.GetCenter());
                if (MyMap.Extent != null) MyMap.Extent = new ESRI.ArcGIS.Client.Geometry.Envelope(MyMap.Extent.XMin, MyMap.Extent.YMin, MyMap.Extent.XMax, MyMap.Extent.YMax);
            }
        }

        /// <summary>
        /// 伸缩地图并定位
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
                    MyMap.PanTo(myMapPoint);
                    MyMap.Extent = MyMap.Extent.Expand(1.001);
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

        /// <summary>
        ///计算每一个点的GPS
        /// </summary>
        /// <param name="gps1"></param>
        /// <param name="gps2"></param>
        private void AddToGPSDict(Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS gps1, Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS gps2)
        {
            string start = gps1.GpsTime.Value.ToString("yyyyMMddHHmmss");
            string end = gps2.GpsTime.Value.ToString("yyyyMMddHHmmss");
            if (!_GPSDict.ContainsKey(start)) _GPSDict.Add(start, gps1);


            for (long i = 1; i <= gps2.GpsTime.Value.Subtract(gps1.GpsTime.Value).TotalSeconds - 1; i++)
            {
                Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS gpsinfo = new Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS();
                gpsinfo.Valid = gps1.Valid;
                gpsinfo.Speed = gps1.Speed;
                gpsinfo.Source = gps1.Source;
                gpsinfo.GpsTime = gps1.GpsTime.Value.AddSeconds(i);
                gpsinfo.Direction = gps1.Direction;
                gpsinfo.Source = gps1.Source;

                DisplayLonConvert loncon = new DisplayLonConvert();
                DisplayLatConvert latcon = new DisplayLatConvert();
                double gps1lon = double.Parse(loncon.ConvertBack(gps1.Longitude, null, null, null).ToString());
                double gps1lat = double.Parse(latcon.ConvertBack(gps1.Latitude, null, null, null).ToString());
                double gps2lon = double.Parse(loncon.ConvertBack(gps2.Longitude, null, null, null).ToString());
                double gps2lat = double.Parse(latcon.ConvertBack(gps2.Latitude, null, null, null).ToString());


                double gpsinfolon = gps1lon + (gps2lon - gps1lon) * i / (long.Parse(end) - long.Parse(start));
                double gpsinfolat = gps1lat + (gps2lat - gps1lat) * i / (long.Parse(end) - long.Parse(start));
                gpsinfo.Longitude = loncon.ConvertToSave(gpsinfolon, null, null, null).ToString();
                gpsinfo.Latitude = latcon.ConvertToSave(gpsinfolat, null, null, null).ToString();


                string key = gpsinfo.GpsTime.Value.ToString("yyyyMMddHHmmss");
                if (!_GPSDict.ContainsKey(key)) _GPSDict.Add(key, gpsinfo);
            }
            if (!_GPSDict.ContainsKey(end)) _GPSDict.Add(end, gps2);
        }

        /// <summary>
        /// 图层初始化失败
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArcGISServiceLayer_InitializationFailed(object sender, EventArgs e)
        {
            ESRI.ArcGIS.Client.Layer layer = sender as ESRI.ArcGIS.Client.Layer;
            ApplicationContext.Instance.Logger.LogException("MapView", layer.InitializationFailure);
        }

        /// <summary>
        /// 得到测试数据,得到的数据必须按时间从小到大的顺序
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public List<Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS> GetTestHisData(DateTime startdate, DateTime enddate)
        {
            //Based QueryDateTimeBegin, QueryDateTimeEnd and QueryCarNo query historical data, write _carHisData;

            //Method One: incoming data
            List<Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS> tempdata = new List<Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS>();

            double lat = 39;
            double lon = 115;

            int dt = (int)(enddate.Subtract(startdate).TotalSeconds);
            for (int i = 0; i <= dt - 1; i = i + 2)
            {
                Random rd = new Random();
                lat = lat + 0.0001 * rd.Next(1, 10);
                lon = lon + 0.0001 * rd.Next(1, 10);
                Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS args = new Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS();
                args.Latitude = lat.ToString();
                args.Longitude = lon.ToString();
                args.Direction = "XXXXXX";
                args.Speed = "65";
                args.GpsTime = startdate.AddSeconds((double)(i));
                args.Valid = "A";
                tempdata.Add(args);
            }
            return tempdata;
        }
    }
}
