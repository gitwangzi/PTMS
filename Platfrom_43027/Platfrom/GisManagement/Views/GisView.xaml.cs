using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Actions;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Tasks;
using GisManagement.Models;
using GisManagement.ViewModels;
using Gsafety.Common.CommMessage.Controls;
using Gsafety.Common.Controls;
using Gsafety.Common.Converts;
using Gsafety.Common.Map;
using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Enums;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 104e69ad-c740-4bef-b9c0-c3c7f72a72bc      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: GisManagement.Views
/////    Project Description:    
/////             Class Name: GisView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/5 10:25:50
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/5 10:25:50
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
//using Gsafety.Ant.Alert.Models;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Printing;
using System.Windows.Threading;

namespace GisManagement.Views
{
    [ExportAsView(GisName.MonitorGisView, Category = GisName.CategoryName, MenuName = GisName.MonitorGisView)]
    [ExportViewToRegion(GisName.MonitorGisView, ViewContainer.GisContainer)]
    public partial class GisView : UserControl
    {
        public static Map myMap;
        public static bool IsDom = false;
        public static Symbol anchorPointSymbol = null;
        public static Symbol locationSymbol = null;
        public static GraphicsLayer anchorPointGraphicsLayer = null;

        private GraphicsLayer addedLayer, graphicsLayer;

        double defaultresolution, Fresolution, Fscalenum;

        //private Draw MyDrawObject;
        private Symbol _activeSymbol = null;
        GraphicsLayer mygraphicsLayer;

        List<Envelope> _extentHistory = new List<Envelope>();
        int _currentExtentIndex = 0;
        bool _newExtent = true;

        bool isDrawOver = true;

        bool isInitMap = true;

        bool isQuery = true;

        private static ESRI.ArcGIS.Client.Projection.WebMercator mercator = new ESRI.ArcGIS.Client.Projection.WebMercator();

        private Draw MyDrawQuery;

        public Graphic currentGraphic;
        //public Envelope InitMap = new Envelope(-9328568.5927, - 449942.313099999, 1.29506752081E7, 4864510.2246);

        private EditGeometry _pEditGeometryTool = null;
        public EditGeometry EditGeoemtryTool
        {
            get { return _pEditGeometryTool; }
        }

        public bool _bEditDrawGeometry = false;
        public GisView()
        {
            // CompositionInitializer.SatisfyImports(this);
            InitializeComponent();
            //isAddText = false;
            try
            {
                if (ApplicationContext.Instance.ServerConfig.BaseMapType == BaseMapTypeEnum.BingMap)
                {
                    BingMapLoad.InitBingMap(MyMap);

                    Envelope env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.MapInitExtent);
                    if (env != null)
                    {
                        MyMap.Extent = env;
                    }

                    env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.OverMapMaximumExtent);
                    if (env != null) OverViewMap.MaximumExtent = env;
                }
                else if (ApplicationContext.Instance.ServerConfig.BaseMapType == BaseMapTypeEnum.GoogleMap)
                {
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "GIS", "GoogleMap");
                    GoogleMap layer = new GoogleMap();
                    layer.BaseURL = ApplicationContext.Instance.ServerConfig.MapSubType;
                    MyMap.Layers.Insert(0, layer);

                    Envelope env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.MapInitExtent);
                    if (env != null)
                    {
                        MyMap.Extent = env;
                    }
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "GIS", "XMin:" + env.XMin);
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "GIS", "XMax:" + env.XMax);
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "GIS", "YMin:" + env.YMin);
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "GIS", "YMax:" + env.YMax);

                    GoogleMap overviewlayer = new GoogleMap();
                    layer.BaseURL = ApplicationContext.Instance.ServerConfig.MapSubType;
                    OverViewMap.Layer = overviewlayer;
                    env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.OverMapMaximumExtent);
                    if (env != null) OverViewMap.MaximumExtent = env;
                }
                else if (ApplicationContext.Instance.ServerConfig.BaseMapType == BaseMapTypeEnum.TsMap)
                {
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "GIS", "TsMap");
                    TsMap layer = new TsMap();
                    layer.Visible = true;
                    layer.Opacity = 1;
                    layer.Url = ApplicationContext.Instance.ServerConfig.GisBaseMapUrl;
                    layer.MatrixSet = ApplicationContext.Instance.ServerConfig.MatrixSet;
                    layer.Layers = ApplicationContext.Instance.ServerConfig.Layers;
                    layer.TileMatrixSet = ApplicationContext.Instance.ServerConfig.TileMatrixSet;

                    MyMap.Layers.Insert(0, layer);

                    Envelope env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.MapInitExtent);
                    env.SpatialReference = new SpatialReference(4326);
                    if (env != null)
                    {
                        MyMap.Extent = env;
                    }

                    TsMap overviewlayer = new TsMap();

                    overviewlayer.Visible = true;
                    overviewlayer.Opacity = 1;
                    overviewlayer.Url = ApplicationContext.Instance.ServerConfig.GisBaseMapUrl;
                    overviewlayer.MatrixSet = ApplicationContext.Instance.ServerConfig.MatrixSet;
                    overviewlayer.Layers = ApplicationContext.Instance.ServerConfig.Layers;
                    overviewlayer.TileMatrixSet = ApplicationContext.Instance.ServerConfig.TileMatrixSet;
                    OverViewMap.Layer = overviewlayer;
                    env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.OverMapMaximumExtent);
                    //env = new Envelope(-20037508.342787, -20037508.342787, 20037508.342787, 20037508.342787);

                    env.SpatialReference = new SpatialReference(4326);
                    if (env != null) OverViewMap.MaximumExtent = env;
                }
                else if (ApplicationContext.Instance.ServerConfig.BaseMapType == BaseMapTypeEnum.BaiduMap)
                {
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "GIS", "BaiduMap");
                    BaiduMap Alayer = new BaiduMap();
                    Alayer.MapType = ApplicationContext.Instance.ServerConfig.MapSubType;
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "GIS", ApplicationContext.Instance.ServerConfig.MapSubType);
                    Alayer.Visible = true;
                    MyMap.Layers.Insert(0, Alayer);
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "GIS", "InitialComplete");

                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "GIS", "MapInitExtent:" + ApplicationContext.Instance.ServerConfig.MapInitExtent);
                    Envelope env = Gsafety.Common.Transform.GetBaiduExtent(ApplicationContext.Instance.ServerConfig.MapInitExtent);
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "GIS", "XMin:" + env.XMin);
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "GIS", "XMax:" + env.XMax);
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "GIS", "YMin:" + env.YMin);
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "GIS", "YMax:" + env.YMax);
                    env.SpatialReference = new SpatialReference(102100);
                    //Envelope FullExtent = new ESRI.ArcGIS.Client.Geometry.Envelope(9001735.65795624, 2919532.04645186, 19020489.8293508, 8346937.81802098)
                    //{
                    //    SpatialReference = new SpatialReference(102100)
                    //};
                    this.MyMap.ZoomTo(env);
                    MyMap.Extent = env;
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "GIS", "ZoomComplete");
                    env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.OverMapMaximumExtent);
                    if (env != null) OverViewMap.MaximumExtent = env;
                }
                else
                {
                    StringBuilder sb = new StringBuilder();


                    sb.Append("<esri:ArcGISTiledMapServiceLayer xmlns:esri=\"http://schemas.esri.com/arcgis/client/2009\" ID=\"DynamicMapLayer\" />");
                    ArcGISTiledMapServiceLayer GisTiledMap = (ArcGISTiledMapServiceLayer)XamlReader.Load(sb.ToString());
                    GisTiledMap.Initialized += ArcGISTiledMapServiceLayer_Initialized;
                    GisTiledMap.InitializationFailed += ArcGISServiceLayer_InitializationFailed;
                    MyMap.Layers.Insert(0, GisTiledMap);


                    //sb = new StringBuilder();
                    //sb.Append("<esri:ArcGISDynamicMapServiceLayer xmlns:esri=\"http://schemas.esri.com/arcgis/client/2009\" ID=\"DynamicMapLayer\" />");
                    //ArcGISDynamicMapServiceLayer TiledMap = (ArcGISDynamicMapServiceLayer)XamlReader.Load(sb.ToString());
                    //TiledMap.Initialized += ArcGISDynamicMapServiceLayer_Initialized;
                    //TiledMap.InitializationFailed += ArcGISServiceLayer_InitializationFailed;
                    //MyDomMap.Layers.Insert(0, TiledMap);

                    //ArcGISTiledMapServiceLayer TiledMap = (ArcGISTiledMapServiceLayer)XamlReader.Load(sb.ToString());
                    //TiledMap.Initialized += ArcGISDynamicMapServiceLayer_Initialized;
                    //TiledMap.InitializationFailed += ArcGISServiceLayer_InitializationFailed;
                    //MyMap.Layers.Insert(0, TiledMap);
                }

                myMap = MyMap;
                anchorPointGraphicsLayer = MyMap.Layers["AnchorPointGraphicsLayer"] as GraphicsLayer;
                anchorPointSymbol = LayoutRoot.Resources["AnchorPointSymbol"] as Symbol;
                locationSymbol = LayoutRoot.Resources["LocationPointSymbol"] as Symbol;

                isQuery = false;
                Fscalenum = MyMap.Scale;

                _pEditGeometryTool = this.LayoutRoot.Resources["MyEditGeometry"] as EditGeometry;
                _pEditGeometryTool.Map = MyMap;

                //MyMap.MinimumResolution = ConstDefine.MinResolution;
                //MyMap.MaximumResolution = ConstDefine.MaxResolution;
                string CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                string strMax = ApplicationContext.Instance.ServerConfig.MaxResolution;
                string strMin = ApplicationContext.Instance.ServerConfig.MinResolution;

                strMax = strMax.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                strMin = strMin.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);

                MyMap.MinimumResolution = Convert.ToDouble(strMin);
                MyMap.MaximumResolution = Convert.ToDouble(strMax);

                this.MyMap.Layers.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Layers_CollectionChanged);

                mygraphicsLayer = MyMap.Layers["MyDrawGraphicsLayer"] as GraphicsLayer;


                MyDrawBorder.Visibility = Visibility.Collapsed;
                //Theme1StackPanel.Visibility = Visibility.Collapsed;

                MyDrawQuery = new Draw(MyMap)
                {
                    LineSymbol = LayoutRoot.Resources["DefaultLineSymbol"] as SimpleLineSymbol,
                    FillSymbol = LayoutRoot.Resources["DefaultFillSymbol"] as FillSymbol
                };

                MyDrawQuery.DrawComplete += MyDrawQuery_DrawComplete;


                //默认不可以操作
                Nextpng.Opacity = 0.3;
                Nextpng.IsHitTestVisible = false;
                Previouspng.Opacity = 0.3;
                Previouspng.IsHitTestVisible = false;

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("GisView()", ex);
            }
        }

        private void MyMap_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isInitMap)
                {
                    defaultresolution = MyMap.Resolution;
                    isInitMap = false;
                }

            }
            catch (Exception)
            {

            }
        }

        private Graphic selectedPointGraphic;

        Graphic graphic;
        TimeSpan elapsedTime;
        DateTime? startTime;

        private void GraphicsLayer_MouseLeftButtonDown(object sender, GraphicMouseButtonEventArgs e)
        {
            e.Handled = true;

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

                    SymbolStyleSet symbolSelect = new SymbolStyleSet();
                    if (graphic.Geometry is Envelope)
                    {
                        symbolSelect.ControlTabItemVisbility(1, 0);
                        symbolSelect.ControlTabItemVisbility(2, 0);
                    }
                    if (graphic.Geometry is MapPoint && !(e.Graphic.Symbol is TextSymbol))
                    {
                        symbolSelect.ControlTabItemVisbility(0, 2);
                        symbolSelect.ControlTabItemVisbility(1, 2);
                    }
                    if (graphic.Geometry is ESRI.ArcGIS.Client.Geometry.Polyline)
                    {
                        symbolSelect.ControlTabItemVisbility(0, 1);
                        symbolSelect.ControlTabItemVisbility(2, 1);
                    }
                    if (graphic.Geometry is ESRI.ArcGIS.Client.Geometry.Polygon)
                    {
                        symbolSelect.ControlTabItemVisbility(1, 0);
                        symbolSelect.ControlTabItemVisbility(2, 0);
                    }
                    symbolSelect.Closed += ((selectSmbolsender, args) =>
                    {
                        if ((bool)symbolSelect.DialogResult)
                        {
                            MapPoint pt = e.Graphic.Geometry as MapPoint;
                            ESRI.ArcGIS.Client.Geometry.Polyline polyline = e.Graphic.Geometry as ESRI.ArcGIS.Client.Geometry.Polyline;
                            ESRI.ArcGIS.Client.Geometry.Polygon polygon = e.Graphic.Geometry as ESRI.ArcGIS.Client.Geometry.Polygon;
                            ESRI.ArcGIS.Client.Geometry.Envelope env = e.Graphic.Geometry as ESRI.ArcGIS.Client.Geometry.Envelope;
                            if (pt != null && !(e.Graphic.Symbol is TextSymbol))
                            {
                                e.Graphic.Symbol = new SimpleMarkerSymbol() { Color = new SolidColorBrush() { Color = symbolSelect.MarkColorParm }, Size = symbolSelect.SymbolSize };
                            }
                            if (polyline != null)
                            {
                                e.Graphic.Symbol = new LineSymbol() { Color = new SolidColorBrush() { Color = symbolSelect.LineColorParm }, Width = symbolSelect.LineWidthParm };
                            }
                            if (polygon != null || env != null)
                            {
                                e.Graphic.Symbol = new SimpleFillSymbol() { Fill = new SolidColorBrush() { Color = symbolSelect.FillColorParm, Opacity = symbolSelect.TansparentParm } };
                            }
                        }
                    });
                    symbolSelect.Show();
                }
                else
                {
                    if (e.Graphic.Geometry is MapPoint && !(e.Graphic.Symbol is TextSymbol))
                    {
                        e.Graphic.Selected = true;
                        selectedPointGraphic = e.Graphic;
                    }
                    else
                    {
                        if (_pEditGeometryTool != null && e.Graphic.Geometry != null)
                        {
                            _pEditGeometryTool.StartEdit(e.Graphic);
                            _bEditDrawGeometry = true;
                            EditGeometryTool.Visibility = Visibility.Collapsed;
                        }
                    }
                }

                graphic = null;
                startTime = null;
            }
        }

        private void MyMap_MouseClick(object sender, ESRI.ArcGIS.Client.Map.MouseEventArgs e)
        {
            if (_pEditGeometryTool != null && _bEditDrawGeometry == true)
            {
                _pEditGeometryTool.StopEdit();
                _bEditDrawGeometry = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyDrawQuery_DrawComplete(object sender, DrawEventArgs e)
        {
            if (isQuery == false)
            {
                ESRI.ArcGIS.Client.Graphic graphic = new ESRI.ArcGIS.Client.Graphic()
                {
                    Geometry = e.Geometry,
                    Symbol = _activeSymbol
                };
                //mygraphicsLayer.Graphics.Clear();
                mygraphicsLayer.Graphics.Add(graphic);

                if ((e.Geometry != null) && (e.DrawMode == DrawMode.Point) && String.IsNullOrEmpty(this.text.addtextblock.Text) == false)
                {
                    MapPoint point = e.Geometry as MapPoint;
                    //3857地图
                    MapPoint geoPt = GpsCarListViewModel.GetGeoCoord(point.X, point.Y);
                    //MapPoint geoPt = point;
                    TextSymbol textSymbol = new TextSymbol
                    {
                        FontSize = 12,
                        Text = this.text.addtextblock.Text
                    };
                    Graphic graphicText = new Graphic()
                    {
                        Geometry = mercator.FromGeographic(geoPt),
                        Symbol = textSymbol
                    };
                    mygraphicsLayer.Graphics.Add(graphicText);
                }
            }
            UnSelectTools();
            MyDrawQuery.IsEnabled = false;
            //throw new NotImplementedException();
        }

        private InputText text;

        //private void MyDrawObject_DrawComplete(object sender, DrawEventArgs e)
        //{
        //    ESRI.ArcGIS.Client.Graphic graphic = new ESRI.ArcGIS.Client.Graphic()
        //    {
        //        Geometry = e.Geometry,
        //        //Symbol = _activeSymbol
        //        Symbol = LayoutRoot.Resources["DefaultFillSymbol"] as FillSymbol
        //    };
        //    mygraphicsLayer.Graphics.Add(graphic);


        //    if ((e.Geometry!=null)&&(e.DrawMode == DrawMode.Point))
        //    {

        //        InputText text = new InputText();
        //        text.Show();

        //        MapPoint point = e.Geometry as MapPoint;

        //        MapPoint geoPt = GpsCarListViewModel.GetGeoCoord(point.X, point.Y);

        //        //if (isAddText == true)
        //        {
        //            TextSymbol textSymbol = new TextSymbol
        //            {
        //                FontSize = 12,
        //                Text = GisName.addtextwords
        //            };
        //            Graphic graphicText = new Graphic()
        //            {
        //                Geometry = mercator.FromGeographic(geoPt),
        //                Symbol = textSymbol
        //            };
        //            mygraphicsLayer.Graphics.Add(graphicText);
        //        }
        //    }           

        //   // isAddText = false;
        //    //throw new NotImplementedException();
        //}

        //private void GraphicsLayer_MouseLeftButtonUp(object sender, GraphicMouseButtonEventArgs e)
        //{
        //    if (EnableEditVerticesScaleRotate.IsChecked.Value)
        //    {
        //        MyDrawObject1.DrawMode = DrawMode.None;
        //        UnSelectTools1();
        //        Editor editor = LayoutRoot.Resources["MyEditor"] as Editor;
        //        if (e.Graphic != null && !(e.Graphic.Geometry is ESRI.ArcGIS.Client.Geometry.MapPoint))
        //            editor.EditVertices.Execute(e.Graphic);
        //    }
        //}


        private void UnSelectTools()
        {
            foreach (UIElement element in MyDrawStackPanel.Children)
                if (element is Button)
                    VisualStateManager.GoToState((element as Button), "UnSelected", false);
        }


        private void Tool_Click(object sender, RoutedEventArgs e)
        {
            UnSelectTools();

            VisualStateManager.GoToState(sender as Button, "Selected", false);

            switch ((sender as Button).Tag as string)
            {
                case "DrawPoint":
                    MyDrawQuery.DrawMode = DrawMode.Point;
                    _activeSymbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as Symbol;
                    text = new InputText();
                    this.text.addtextblock.Text = "";
                    //text.Closed += ((sender2, args) => { });
                    text.Show();
                    break;
                case "DrawPolyline":
                    MyDrawQuery.DrawMode = DrawMode.Polyline;
                    _activeSymbol = LayoutRoot.Resources["DefaultLineSymbol"] as Symbol;
                    break;
                case "DrawlineSegment":
                    MyDrawQuery.DrawMode = DrawMode.LineSegment;
                    _activeSymbol = LayoutRoot.Resources["DefaultLineSymbol"] as Symbol;
                    break;
                case "DrawPolygon":
                    MyDrawQuery.DrawMode = DrawMode.Polygon;
                    _activeSymbol = LayoutRoot.Resources["DefaultFillSymbol"] as Symbol;
                    break;
                case "DrawRectangle":
                    MyDrawQuery.DrawMode = DrawMode.Rectangle;
                    _activeSymbol = LayoutRoot.Resources["DefaultFillSymbol"] as Symbol;
                    break;
                case "DrawFreehand":
                    MyDrawQuery.DrawMode = DrawMode.Freehand;
                    _activeSymbol = LayoutRoot.Resources["DefaultLineSymbol"] as Symbol;
                    break;
                case "DrawArrow":
                    MyDrawQuery.DrawMode = DrawMode.Arrow;
                    _activeSymbol = LayoutRoot.Resources["DefaultFillSymbol"] as Symbol;
                    break;
                case "DrawTriangle":
                    MyDrawQuery.DrawMode = DrawMode.Triangle;
                    _activeSymbol = LayoutRoot.Resources["DefaultFillSymbol"] as Symbol;
                    break;
                case "DrawCircle":
                    MyDrawQuery.DrawMode = DrawMode.Circle;
                    _activeSymbol = LayoutRoot.Resources["DefaultFillSymbol"] as Symbol;
                    break;
                case "DrawEllipse":
                    MyDrawQuery.DrawMode = DrawMode.Ellipse;
                    _activeSymbol = LayoutRoot.Resources["DefaultFillSymbol"] as Symbol;
                    break;
                default:
                    MyDrawQuery.DrawMode = DrawMode.None;
                    mygraphicsLayer.Graphics.Clear();
                    break;
            }
            MyDrawQuery.IsEnabled = (MyDrawQuery.DrawMode != DrawMode.None);
            isQuery = false;
        }

        private void MyMap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (selectedPointGraphic != null)
            {
                selectedPointGraphic.Selected = false;
                selectedPointGraphic = null;
            }
        }

        private void MyMap_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(MyMap);
            MapPoint mapPoint = MyMap.ScreenToMap(pt);
            if (mapPoint != null)
            {
                //3857地图
                //MapPoint geoPt = mapPoint;
                MapPoint geoPt = GpsCarListViewModel.GetGeoCoord(mapPoint.X, mapPoint.Y);
                DisplayLonConvert loncon = new DisplayLonConvert();
                DisplayLatConvert latcon = new DisplayLatConvert();
               
                if (geoPt != null)
                {
                    CoordTip.Text = "WGS:" + loncon.ConvertToWESN(geoPt.X, null, null, null).ToString() + "  " + latcon.ConvertToWESN(geoPt.Y, null, null, null).ToString();
                }
                //  ShowUTMProjectCooor(mapPoint);
            }

            if (selectedPointGraphic != null)
                selectedPointGraphic.Geometry = MyMap.ScreenToMap(e.GetPosition(MyMap));
        }

        private void ShowUTMProjectCooor(MapPoint ptWebMercato)
        {
            if (ptWebMercato == null || ptWebMercato.SpatialReference == null)
                return;
            ESRI.ArcGIS.Client.Projection.WebMercator webm = new ESRI.ArcGIS.Client.Projection.WebMercator();
            List<Graphic> list = new List<Graphic>();
            Graphic g = new Graphic();
            g.Geometry = ptWebMercato;
            list.Add(g);
            ESRI.ArcGIS.Client.Geometry.SpatialReference spa = new SpatialReference();
            spa.WKID = ConstDefine.GisUTMProjectCoorSystemWKID;
            ESRI.ArcGIS.Client.Tasks.GeometryService geo = new ESRI.ArcGIS.Client.Tasks.GeometryService();
            geo.Url = ApplicationContext.Instance.ServerConfig.GisGeometryServiceUrl;
            geo.ProjectCompleted += geo_ProjectCompleted;
            geo.Failed += geo_Failed;
            geo.ProjectAsync(list, spa);
        }

        private void geo_Failed(object sender, TaskFailedEventArgs e)
        {
            //  ApplicationContext.Instance.Logger.LogException("showUTM", e.Error);
        }

        private void geo_ProjectCompleted(object sender, GraphicsEventArgs e)
        {
            if (e.Results.Count == 1)
            {
                MapPoint pt = e.Results[0].Geometry as MapPoint;
                if (pt != null)
                {
                    // UTMCoordTip.Text = "UTM:" + pt.X.ToString("f6") + "  " + pt.Y.ToString("f6");
                }
            }
        }

        //private void hiddenD_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    ((Image)sender).Opacity = 1;
        //}

        //private void hiddenD_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    ((Image)sender).Opacity = 0.6;
        //}


        private void Layers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {

                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    //foreach (var item in e.NewItems)
                    var item = e.NewItems[0];
                    if (item is GraphicsLayer)
                    {
                        addedLayer = item as GraphicsLayer;
                        graphicsLayer = this.MyMap.Layers["MeasureGraphicsLayer"] as GraphicsLayer;
                        graphicsLayer.Visible = true;
                        //if (addedLayer.DisplayName == "MeasureGraphicsLayer")
                        (item as GraphicsLayer).Graphics.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Graphics_CollectionChanged);
                        //graphicsLayer.Graphics.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Graphics_CollectionChanged);
                    }
                }

                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                {
                    if (addedLayer != null)
                    {
                        isDrawOver = true;
                        graphicsLayer.Visible = true;
                    }
                }

            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GisView", ee);
            }
            //throw new NotImplementedException();
        }

        private void Graphics_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    if (isDrawOver == false)
                    {
                        MyMap.Focus();
                        graphicsLayer = this.MyMap.Layers["MeasureGraphicsLayer"] as GraphicsLayer;
                        graphicsLayer.ClearGraphics();
                    }
                    else
                        isDrawOver = false;
                    //double len = 0;
                    if (graphicsLayer != null)
                    {
                        //MapPoint pt = null;
                        foreach (Graphic item in addedLayer.Graphics)
                        {
                            Graphic g = item as Graphic;
                            graphicsLayer.Graphics.Add(new Graphic() { Geometry = g.Geometry, Symbol = g.Symbol });

                            //if (item.Geometry is ESRI.ArcGIS.Client.Geometry.Polyline)
                            //{
                            //    ESRI.ArcGIS.Client.Geometry.PointCollection pts = (item.Geometry as ESRI.ArcGIS.Client.Geometry.Polyline).Paths[0];
                            //    pt = pts[pts.Count - 1];

                            //    len = len;
                            //}
                        }
                        //if (pt != null)
                        //{
                        //    ESRI.ArcGIS.Client.Symbols.TextSymbol ts = new ESRI.ArcGIS.Client.Symbols.TextSymbol();
                        //    ts.Text = len.ToString();
                        //    graphicsLayer.Graphics.Add(new Graphic() { Geometry = pt, Symbol = ts });
                        //}
                    }
                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GisView", ee);
            }
            //throw new NotImplementedException();
        }

        private void PrevExtent(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentExtentIndex != 0)
                {
                    _currentExtentIndex--;

                    if (_currentExtentIndex == 0)
                    {
                        Previouspng.Opacity = 0.3;
                        Previouspng.IsHitTestVisible = false;
                    }

                    _newExtent = false;

                    MyMap.IsHitTestVisible = false;
                    MyMap.ZoomTo(_extentHistory[_currentExtentIndex]);

                    if (Nextpng.IsHitTestVisible == false)
                    {
                        Nextpng.Opacity = 1;
                        Nextpng.IsHitTestVisible = true;
                    }
                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GisView", ee);
            }
        }

        private void NextExtent(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentExtentIndex < _extentHistory.Count - 1)
                {
                    _currentExtentIndex++;

                    if (_currentExtentIndex == (_extentHistory.Count - 1))
                    {
                        Nextpng.Opacity = 0.3;
                        Nextpng.IsHitTestVisible = false;
                    }

                    _newExtent = false;

                    MyMap.IsHitTestVisible = false;
                    MyMap.ZoomTo(_extentHistory[_currentExtentIndex]);

                    if (Previouspng.IsHitTestVisible == false)
                    {
                        Previouspng.Opacity = 1;
                        Previouspng.IsHitTestVisible = true;
                    }
                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GisView", ee);
            }
        }

        private void MyMap_ExtentChanged(object sender, ExtentEventArgs e)
        {
            try
            {
                if ((e.OldExtent != null) && (e.NewExtent != null))
                {
                    if (Math.Abs(e.OldExtent.Width - e.NewExtent.Width) < e.OldExtent.Width * ConstDefine.MinOffsetFactorToWriteHistroy)
                    {
                        if (Math.Sqrt(Math.Pow((e.OldExtent.GetCenter().X - e.NewExtent.GetCenter().X), 2) + Math.Pow((e.OldExtent.GetCenter().Y - e.NewExtent.GetCenter().Y), 2)) < e.OldExtent.Width * ConstDefine.MinOffsetFactorToWriteHistroy)
                        {
                            return;
                        }
                    }
                }
                //if (e.OldExtent != null)
                //{
                //if (MyMap.Resolution * (e.NewExtent.Width / e.OldExtent.Width) < ConstDefine.MinResolution - 0.01)
                double dWc = Convert.ToDouble("0.01".Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
                if (MyMap.Resolution < (MyMap.MinimumResolution - dWc))
                {
                    MyMap.ZoomToResolution(MyMap.MinimumResolution, e.NewExtent.GetCenter());
                    return;
                }
                //}

                if (isInitMap)
                {
                    defaultresolution = MyMap.Resolution;
                    isInitMap = false;
                }

                //scaletext.Text = MyMap.Scale.ToString("0");
                //int scaleint = (int)MyMap.Scale;
                //scaletext.Text = scaleint.ToString();

                if (e.OldExtent == null)
                {
                    _extentHistory.Add(e.NewExtent.Clone());
                    return;
                }


                if (_newExtent)
                {
                    _currentExtentIndex++;

                    if (_extentHistory.Count - _currentExtentIndex > 0)
                        _extentHistory.RemoveRange(_currentExtentIndex, (_extentHistory.Count - _currentExtentIndex));

                    if (Nextpng.IsHitTestVisible == true)
                    {
                        Nextpng.Opacity = 0.3;
                        Nextpng.IsHitTestVisible = false;
                    }

                    _extentHistory.Add(e.NewExtent.Clone());

                    if (Previouspng.IsHitTestVisible == false)
                    {
                        Previouspng.Opacity = 1;
                        Previouspng.IsHitTestVisible = true;
                    }
                }
                else
                {
                    MyMap.IsHitTestVisible = true;
                    _newExtent = true;
                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GisView", ee);
            }
        }

        private void FullMapExtent(object sender, RoutedEventArgs e)
        {
            Envelope env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.MapInitExtent);
            if (env != null) MyMap.Extent = env;

            env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.OverMapMaximumExtent);
            if (env != null) OverViewMap.MaximumExtent = env;
        }

        private void ChangeMapExtent(object sender, RoutedEventArgs e)
        {         


            MyMap.Layers.RemoveAt(0);

            if (IsDom == false)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<esri:ArcGISDynamicMapServiceLayer xmlns:esri=\"http://schemas.esri.com/arcgis/client/2009\" ID=\"DynamicMapLayer\" />");
                ArcGISDynamicMapServiceLayer GisTiledMap = (ArcGISDynamicMapServiceLayer)XamlReader.Load(sb.ToString());

                GisTiledMap.Initialized += ArcGISDynamicMapServiceLayer_Initialized;
                GisTiledMap.InitializationFailed += ArcGISServiceLayer_InitializationFailed;
                MyMap.Layers.Insert(0, GisTiledMap);
                IsDom = !IsDom;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<esri:ArcGISTiledMapServiceLayer xmlns:esri=\"http://schemas.esri.com/arcgis/client/2009\" ID=\"DynamicMapLayer\" />");
                ArcGISTiledMapServiceLayer GisTiledMap = (ArcGISTiledMapServiceLayer)XamlReader.Load(sb.ToString());

                GisTiledMap.Initialized += ArcGISTiledMapServiceLayer_Initialized;
                GisTiledMap.InitializationFailed += ArcGISServiceLayer_InitializationFailed;
                MyMap.Layers.Insert(0, GisTiledMap);
                IsDom = !IsDom;

            }
           

            //if (ApplicationContext.Instance.ServerConfig.BaseMapType == BaseMapTypeEnum.ArcGisMap)
            //{
            //    if ((MyMap.Layers["DynamicMapLayer"]) is ArcGISDynamicMapServiceLayer)
            //    {
            //        ArcGISDynamicMapServiceLayer arcgisLayer = MyMap.Layers["DynamicMapLayer"] as ArcGISDynamicMapServiceLayer;

            //        arcgisLayer.Url = ApplicationContext.Instance.ServerConfig.DomGisBaseMapUrl;
            //        (OverViewMap.Layer as ArcGISDynamicMapServiceLayer).Url = ApplicationContext.Instance.ServerConfig.DomGisBaseMapUrl;
            //        MyMap.Extent = arcgisLayer.FullExtent;
            //    }
            //    else if ((MyMap.Layers["DynamicMapLayer"]) is ArcGISTiledMapServiceLayer)
            //    {
            //        ArcGISTiledMapServiceLayer arcgisLayer = MyMap.Layers["DynamicMapLayer"] as ArcGISTiledMapServiceLayer;
            //        arcgisLayer.Url = ApplicationContext.Instance.ServerConfig.DomGisBaseMapUrl;
            //        (OverViewMap.Layer as ArcGISTiledMapServiceLayer).Url = ApplicationContext.Instance.ServerConfig.DomGisBaseMapUrl;
            //        MyMap.Extent = arcgisLayer.FullExtent;
            //    }


            //    Envelope env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.MapInitExtent);
            //    if (env != null)
            //    {
            //        MyMap.Extent = env;
            //    }

            //    env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.OverMapMaximumExtent);
            //    if (env != null) OverViewMap.MaximumExtent = env;
            //    MyMap.Focus();
               
            //}
        }


        private void MeasureLength(object sender, RoutedEventArgs e)
        {
            MyDrawQuery.IsEnabled = false;
            MyMeasureAction mc = new MyMeasureAction();
            //mc.TargetName = "MyMap";
            mc.MeasureMode = MeasureAction.Mode.Polyline;
            mc.AreaUnit = ESRI.ArcGIS.Client.Actions.AreaUnit.SquareKilometers;
            mc.DisplayTotals = true;

            mc.DistanceUnit = DistanceUnit.Kilometers;
            //mc.FillSymbol = LayoutRoot.Resources["DefaultLineSymbol"] as FillSymbol;
            //mc.FillSymbol.BorderThickness = 0.01;
            mc.Attach(MyMap);
            mc.Execute();

            //System.Windows.Interactivity.EventTrigger eventTrigger = new System.Windows.Interactivity.EventTrigger("Click");
            //eventTrigger.Actions.Add(mc);

            //System.Windows.Interactivity.Interaction.GetTriggers(BtnLength).Add(eventTrigger);
        }

        private void MeasureArea(object sender, RoutedEventArgs e)
        {
            //MyDrawObject.IsEnabled = false;
            MyDrawQuery.IsEnabled = false;
            MyMeasureAction mc = new MyMeasureAction();
            //mc.TargetName = "MyMap";
            mc.MeasureMode = MeasureAction.Mode.Polygon;
            mc.AreaUnit = ESRI.ArcGIS.Client.Actions.AreaUnit.SquareKilometers;
            mc.DisplayTotals = false;
            mc.DistanceUnit = DistanceUnit.Kilometers;
            mc.FillSymbol = LayoutRoot.Resources["DefaultFillSymbol"] as FillSymbol;
            //mc.FillSymbol = LayoutRoot.Resources["DefaultFillSymbol"] as FillSymbol;
            //mc.FillSymbol.BorderThickness = 0.01;
            mc.Attach(MyMap);
            mc.Execute();

        }


        private void scaletext_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    Regex reg = new Regex("^[0-9]+$");
                    //Match m = reg.Match(scaletext.Text);
                    Match m = reg.Match("");
                    if (m.Success)
                    {
                        //Fresolution = Convert.ToDouble(scaletext.Text.ToString()) / ConstDefine.DefaultScale * defaultresolution;
                        //Fresolution = Convert.ToDouble(scaletext.Text.ToString()) / ConstDefine.DefaultScale;
                        //Fresolution = Convert.ToDouble(0) / ConstDefine.DefaultScale;
                        if (Fresolution <= Convert.ToDouble(ApplicationContext.Instance.ServerConfig.MinResolution))
                        {
                            // scaletext.Text = "";
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), TranslateInfo.Translate(TranslateInfo.ScaleBelow), MessageDialogButton.Ok);

                        }
                        else if (Fresolution >= Convert.ToDouble(ApplicationContext.Instance.ServerConfig.MaxResolution))
                        {
                            //scaletext.Text = "";
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), TranslateInfo.Translate(TranslateInfo.ScaleAbove), MessageDialogButton.Ok);
                        }
                        else
                        {
                            MyMap.ZoomToResolution(Fresolution);

                        }
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), TranslateInfo.Translate(TranslateInfo.ScaleInValid), MessageDialogButton.Ok);
                        //scaletext.Text = "";
                    }
                }
                else
                {
                    if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.Delete) || (e.Key == Key.Back) || (e.Key >= Key.D0 && e.Key <= Key.D9))
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GisView", ee);
            }
        }

        private void OverViewClick(object sender, RoutedEventArgs e)
        {
            overviewgrid.Visibility = overviewgrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        //[ExportAsView(GisName.SpatialQuery)]
        //public UserControl SpatialQueryExport
        //{
        //    get
        //    {
        //        return spatialQuery;
        //    }
        //}


        //public UserControl GpsCarListExport
        //{
        //    get
        //    {
        //        return monitorList;
        //    }
        //}

        public GpsCarHisDataView GpsHisCarListExport
        {
            get
            {
                return gpscarhisdatalist;
            }
        }

        [ExportAsView(GisManagement.GisName.GpsCarHisDataViewMonitor)]
        public GisManagement.Views.GpsCarHisDataView GpsCarHisDataViewExport
        {
            get { return gpscarhisdatalist; }
        }



        private void ArcGISDynamicMapServiceLayer_Initialized(object sender, EventArgs e)
        {
            if (ApplicationContext.Instance.ServerConfig.BaseMapType == BaseMapTypeEnum.ArcGisMap)
            {
                if ((MyMap.Layers["DynamicMapLayer"]) is ArcGISDynamicMapServiceLayer)
                {
                    ArcGISDynamicMapServiceLayer arcgisLayer = MyMap.Layers["DynamicMapLayer"] as ArcGISDynamicMapServiceLayer;

                    arcgisLayer.Url = ApplicationContext.Instance.ServerConfig.DomGisBaseMapUrl;

                    ArcGISDynamicMapServiceLayer overLayer = new ArcGISDynamicMapServiceLayer();
                    overLayer.Url = ApplicationContext.Instance.ServerConfig.DomGisBaseMapUrl;
                    OverViewMap.Layer = overLayer;
                    //(OverViewMap.Layer as ArcGISDynamicMapServiceLayer).Url = ApplicationContext.Instance.ServerConfig.GisBaseMapUrl;
                    MyMap.Extent = arcgisLayer.FullExtent;
                }
                else if ((MyMap.Layers["DynamicMapLayer"]) is ArcGISTiledMapServiceLayer)
                {
                    ArcGISTiledMapServiceLayer arcgisLayer = MyMap.Layers["DynamicMapLayer"] as ArcGISTiledMapServiceLayer;
                    arcgisLayer.Url = ApplicationContext.Instance.ServerConfig.DomGisBaseMapUrl;
                    ArcGISTiledMapServiceLayer overLayer = new ArcGISTiledMapServiceLayer();
                    overLayer.Url = ApplicationContext.Instance.ServerConfig.DomGisBaseMapUrl;
                    OverViewMap.Layer = overLayer;
                    //(OverViewMap.Layer as ArcGISTiledMapServiceLayer).Url = ApplicationContext.Instance.ServerConfig.GisBaseMapUrl;
                    MyMap.Extent = arcgisLayer.FullExtent;
                }


                Envelope env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.MapInitExtent);
                if (env != null)
                {
                    MyMap.Extent = env;
                }

                env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.OverMapMaximumExtent);
                if (env != null) OverViewMap.MaximumExtent = env;
                MyMap.Focus();
            }
        }

        private void ArcGISTiledMapServiceLayer_Initialized(object sender, EventArgs e)
        {
            if (ApplicationContext.Instance.ServerConfig.BaseMapType == BaseMapTypeEnum.ArcGisMap)
            {
                if ((MyMap.Layers["DynamicMapLayer"]) is ArcGISDynamicMapServiceLayer)
                {
                    ArcGISDynamicMapServiceLayer arcgisLayer = MyMap.Layers["DynamicMapLayer"] as ArcGISDynamicMapServiceLayer;

                    arcgisLayer.Url = ApplicationContext.Instance.ServerConfig.GisBaseMapUrl;
                    ArcGISDynamicMapServiceLayer overLayer = new ArcGISDynamicMapServiceLayer();
                    overLayer.Url = ApplicationContext.Instance.ServerConfig.GisBaseMapUrl;
                    OverViewMap.Layer = overLayer;
                   // (OverViewMap.Layer as ArcGISDynamicMapServiceLayer).Url = ApplicationContext.Instance.ServerConfig.GisBaseMapUrl;
                    MyMap.Extent = arcgisLayer.FullExtent;
                }
                else if ((MyMap.Layers["DynamicMapLayer"]) is ArcGISTiledMapServiceLayer)
                {
                    ArcGISTiledMapServiceLayer arcgisLayer = MyMap.Layers["DynamicMapLayer"] as ArcGISTiledMapServiceLayer;
                    arcgisLayer.Url = ApplicationContext.Instance.ServerConfig.GisBaseMapUrl;
                    ArcGISTiledMapServiceLayer overLayer = new ArcGISTiledMapServiceLayer();
                    overLayer.Url = ApplicationContext.Instance.ServerConfig.GisBaseMapUrl;
                    OverViewMap.Layer = overLayer;
                    //(OverViewMap.Layer as ArcGISTiledMapServiceLayer).Url = ApplicationContext.Instance.ServerConfig.GisBaseMapUrl;
                    MyMap.Extent = arcgisLayer.FullExtent;
                }


                Envelope env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.MapInitExtent);
                if (env != null)
                {
                    MyMap.Extent = env;
                }

                env = GisViewModel.getExtent(ApplicationContext.Instance.ServerConfig.OverMapMaximumExtent);
                if (env != null) OverViewMap.MaximumExtent = env;
                MyMap.Focus();
            }
        }

        private void ArcGISServiceLayer_InitializationFailed(object sender, EventArgs e)
        {
            ESRI.ArcGIS.Client.Layer layer = sender as ESRI.ArcGIS.Client.Layer;
            ApplicationContext.Instance.Logger.LogException("MapView", layer.InitializationFailure);
        }

        private void ExportMap(object sender, RoutedEventArgs e)
        {
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += (s, ee) =>
                {
                    ee.PageVisual = MyMap;
                    ee.HasMorePages = false;
                };
            doc.Print("Map");
        }

        private void DrawMap(object sender, RoutedEventArgs e)
        {
            if (MyDrawBorder.Visibility == Visibility.Visible)
                MyDrawBorder.Visibility = Visibility.Collapsed;
            else
                MyDrawBorder.Visibility = Visibility.Visible;
        }

        private void ActivatePrintPreview(object sender, RoutedEventArgs e)
        {
            PrintPreview.Visibility = Visibility.Visible;
            //修改弹窗口上边距
            PrintPreview.Margin = new Thickness(0, -5, 0, 0);
            MyMapPrinter.Map = MyMap; // sets the Map to print and initializes the PrintMap with a cloned map (as defined in the print style)
            MyMapPrinter.RotateMap = false;
        }



        private void DesactivatePrintPreview(object sender, RoutedEventArgs e)
        {
            PrintTitle.Text = "";
            PrintPreview.Visibility = Visibility.Collapsed;
            MyMapPrinter.Map = null;  // cancel the current print and frees the cloned map
        }

        private void OnPreviewSizeChanged(object sender, SelectionChangedEventArgs e)
        {
            // Chnage the preview size of the print map.
            // Note that this size will be overwritten during the print process by the real print area of the printer (depending on print parameters: paper size, orientation,...)
            var previewSize = ((ComboBox)sender).SelectedItem as PreviewSize;
            if (previewSize != null && MyMapPrinter != null)
                MyMapPrinter.SetPrintableArea(previewSize.Height, previewSize.Width);
        }

        private void OnPrint(object sender, RoutedEventArgs e)
        {
            // Start the print process
            MyMapPrinter.Print();
        }

        private void BtnExportPng_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                ElementToPNG eTP = new ElementToPNG();
                bool f = eTP.ShowSaveDialog(MyMapPrinter);
                if (f) MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_ExportSuccessful"), MessageDialogButton.Ok);

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GraphicsLayer layer = MyMap.Layers["MeasureGraphicsLayer"] as GraphicsLayer;
            if (layer != null)
                layer.Graphics.Clear();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GraphicsLayer layer1 = MyMap.Layers["MyDrawGraphicsLayer"] as GraphicsLayer;
            if (layer1 != null)
                layer1.Graphics.Clear();
            GraphicsLayer layer2 = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            if (layer2 != null)
            {
                layer2.Graphics.Clear();
                ApplicationContext.Instance.CurrentDrawArgs = null;
            }
        }

        private void ClearTheme1_Click_1(object sender, RoutedEventArgs e)
        {
            GraphicsLayer pTheMapLayer3 = MyMap.Layers["FacilityThreeMinThemeGraphicLayer"] as GraphicsLayer;
            GraphicsLayer pTheMapLayer5 = MyMap.Layers["FacilityFiveMinThemeGraphicLayer"] as GraphicsLayer;
            GraphicsLayer pTheMapLayer10 = MyMap.Layers["FacilityTenMinThemeGraphicLayer"] as GraphicsLayer;
            if (pTheMapLayer3 != null)
                pTheMapLayer3.Graphics.Clear();
            if (pTheMapLayer5 != null)
                pTheMapLayer5.Graphics.Clear();
            if (pTheMapLayer10 != null)
                pTheMapLayer10.Graphics.Clear();
        }

        private void ClearTheme2_Click_1(object sender, RoutedEventArgs e)
        {
            GraphicsLayer pTheMapLayer = MyMap.Layers["TaxiThemeGraphicLayer"] as GraphicsLayer;
            if (pTheMapLayer != null)
                pTheMapLayer.Graphics.Clear();
        }

        private void ClearSnapshot_Click_1(object sender, RoutedEventArgs e)
        {
            GraphicsLayer pTheMapLayer = MyMap.Layers["SnapshotLayer"] as GraphicsLayer;
            if (pTheMapLayer != null)
                pTheMapLayer.Graphics.Clear();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocateInfo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            DataGrid dg = sender as DataGrid;
            int s = dg.SelectedIndex;
            var ss = dg.SelectedItem;

            MapPoint mapPoint = null;

            GisViewModel.Coordinate.TryGetValue(SearchLocateInfoData.SelectedIndex + 1, out mapPoint);

            //MapPoint mapPnt = ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(mapPoint);
            MapPoint mapPnt = Gsafety.Common.Transform.GeographicToWebMercator(mapPoint);
            MyMap.PanTo(mapPnt);
        }
    }

    public class MyMeasureAction : MeasureAction
    {
        public void Execute()
        {
            Invoke(null);
        }
    }

    // Main print control that displays the map using the print template
    public class MapPrinter : Control, INotifyPropertyChanged
    {
        public MapPrinter()
        {
            _isPrinting = false;
            DataContext = this; // simplify binding in print styles
        }

        // Executed when the print template changed
        public override void OnApplyTemplate()
        {
            var extent = PrintMap == null ? null : PrintMap.Extent; // save the current print extent that will be lost after OnApplyTemplate (since the PrintMap changes)
            base.OnApplyTemplate();
            PrintMap = GetTemplateChild("PrintMap") as ESRI.ArcGIS.Client.Map;
            PrintMap.Extent = extent ?? Map.Extent; // restore previous print extent or init it with the current map extent
        }

        // Map to print (Dependency Property)
        public ESRI.ArcGIS.Client.Map Map
        {
            get { return (ESRI.ArcGIS.Client.Map)GetValue(MapProperty); }
            set { SetValue(MapProperty, value); }
        }

        public static readonly DependencyProperty MapProperty = DependencyProperty.Register("Map", typeof(ESRI.ArcGIS.Client.Map), typeof(MapPrinter), new PropertyMetadata(null, OnMapChanged));

        private static void OnMapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapPrinter = d as MapPrinter;
            var newMap = e.NewValue as ESRI.ArcGIS.Client.Map;
            if (mapPrinter != null)
            {
                if (newMap != null && mapPrinter.PrintMap != null)
                    mapPrinter.PrintMap.Extent = newMap.Extent;
                if (newMap == null && mapPrinter.IsPrinting)
                    mapPrinter.IsCancelingPrint = true;
            }
        }

        // Title of the print document (Dependency Property)
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(MapPrinter), new PropertyMetadata("Map Document"));

        // Flag indicating that the map must be rotated 90° 
        public bool RotateMap
        {
            get { return (bool)GetValue(RotateMapProperty); }
            set { SetValue(RotateMapProperty, value); }
        }

        public static readonly DependencyProperty RotateMapProperty =
                DependencyProperty.Register("RotateMap", typeof(bool), typeof(MapPrinter), new PropertyMetadata(false, OnRotateMapChanged));

        private static void OnRotateMapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapPrinter = d as MapPrinter;
            if (mapPrinter != null)
                mapPrinter.PrintMap.Rotation = ((bool)e.NewValue ? -90 : 0);
        }

        bool _isPrinting;
        // Indicates if a print task is going on.
        public bool IsPrinting
        {
            get { return _isPrinting; }
            private set
            {
                if (value != _isPrinting)
                {
                    _isPrinting = value;
                    NotifyPropertyChanged("IsPrinting");
                }
            }
        }

        // The print map (defined in the print template)
        public ESRI.ArcGIS.Client.Map PrintMap { get; private set; }

        // Gets the current date/time.
        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        // Start the print process (by delagating either to the Silverlight print engine or to the WPF print engine)
        public void Print()
        {
            if (IsPrinting)
                return;

            // Create the print engine depending on silverlight/WPF
            var printEngine = new SilverlightPrintEngine(this);

            // Call the print engine doing the work
            try
            {
                printEngine.Print();
            }
            catch (Exception e)
            {
                EndPrint(e);
            }
        }

        // InotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        // Notifies the property changed.
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Internal methods/properties
        internal bool IsCancelingPrint { get; set; }

        internal void BeginPrint()
        {
            IsCancelingPrint = false;
            IsPrinting = true;

            NotifyPropertyChanged("Now"); // in case time is displayed
        }

        internal void EndPrint(Exception error)
        {
            //if (error != null && !IsCancelingPrint)
            //    MessageBoxHelper.GisShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), string.Format("Error during print: {0}", error.Message), MessageDialogButton.Ok);
            IsPrinting = false;
            IsCancelingPrint = false;
        }

        internal void SetPrintableArea(double printableAreaHeight, double printableAreaWidth)
        {
            // Recalculate layout in order to fit printable area
            Height = printableAreaHeight;
            Width = printableAreaWidth;

            // Update map size
            UpdateLayout();
        }
    }

    // Collection of PreviewSize (creatable in XAML)
    public class PreviewSizes : ObservableCollection<PreviewSize> { }

    // Represents a Preview Size (creatable in XAML)
    public class PreviewSize
    {
        public string Id { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }

    // Silverlight PrintEngine class: Print a map by using a SL PrintDocument
    internal class SilverlightPrintEngine
    {
        private readonly MapPrinter _mapPrinter;

        // Used during print
        private bool _isLoading;
        private bool _isReady;
        private int _tryCount;
        private MapLoader _mapLoader;

        public SilverlightPrintEngine(MapPrinter mapPrinter)
        {
            _mapPrinter = mapPrinter;
        }

        public void Print()
        {
            var doc = new PrintDocument();

            doc.BeginPrint += (s, e) => BeginPrint();
            doc.PrintPage += PrintPage;
            doc.EndPrint += (s, e) => EndPrint(e != null ? e.Error : null);

            doc.Print(string.IsNullOrEmpty(_mapPrinter.Title) ? "Map Print" : _mapPrinter.Title, null);
        }

        private void BeginPrint()
        {
            _mapPrinter.BeginPrint();

            _mapLoader = new MapLoader(_mapPrinter.PrintMap);
            _mapLoader.Loaded += OnMapLoaderLoaded;
            _isLoading = false;
            _tryCount = 0;
            _isReady = false;
        }

        void OnMapLoaderLoaded(object sender, EventArgs e)
        {
            // All layers are loaded in the map --> ready to print (next time PrintPage will be called by SL framework)
            _isLoading = false;
            _mapPrinter.UpdateLayout(); // to be sure all tiles will be displayed
        }

        private void EndPrint(Exception error)
        {
            _mapPrinter.EndPrint(error);
            _mapLoader.Loaded -= OnMapLoaderLoaded;
            _mapLoader = null;
            if (error == null)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ALARM_PrintSuccess"), MessageDialogButton.Ok);

            }
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            e.PageVisual = null;

            _tryCount++;
            if (_mapPrinter.IsCancelingPrint)
            {
                // Print has been canceled by user
                e.HasMorePages = false; //  Note that despite this setting to false, the framework will continue to call PrintPage 7 times
                return;
            }

            if (_tryCount == 1)
            {
                // Set the printable area size which is unknown before the print of the first page
                var extent = _mapPrinter.PrintMap.Extent;
                _mapPrinter.SetPrintableArea(e.PrintableArea.Height, e.PrintableArea.Width);

                // change the extent of the map and wait for all layers loaded (progress == 100)
                _isLoading = true;
                _mapLoader.WaitForLoaded();
                _mapPrinter.PrintMap.Extent = extent;
                e.HasMorePages = true; // retry later but nothing to print at this time
            }
            else
            {
                _mapPrinter.UpdateLayout();
                if (_isLoading && _tryCount <= 8)
                {
                    // Wait for loaded layers
                    e.HasMorePages = true; // retry later

                    Thread.Sleep(100 + 300 * _tryCount); // sleep to give a chance to load layers before the maximum of 7 tries
                }
                else
                {
                    if (_isReady || _tryCount > 8)
                    {
                        // Print the page
                        e.HasMorePages = false;
                        e.PageVisual = _mapPrinter;
                    }
                    else
                    {
                        // FeatureLayers in OnDemand mode need to be rendered once in order to be printable --> wait once more
                        e.HasMorePages = true;
                        _isReady = true;
                    }
                }
            }
        }
    }

    // Helper class to know when a map is loaded and so ready to print.
    // It's waiting for progress = 100 but sometimes this event never comes (e.g. with a dynamic layer when the image is in the cache)
    // So there is a timer to avoid infinite wait.
    internal class MapLoader
    {
        private readonly ESRI.ArcGIS.Client.Map _map;
        private readonly DispatcherTimer _timer;
        private bool _isProgressing; // some progress events came up 

        public MapLoader(ESRI.ArcGIS.Client.Map map)
        {
            _map = map;
            _timer = new DispatcherTimer();
            _timer.Tick += OnTimerTick;
            _isProgressing = false;
        }

        // Waits for the map loaded.
        public void WaitForLoaded()
        {
            _map.Progress += OnMapProgress; // subscribe to OnProgress event
            if (_timer.IsEnabled)
                _timer.Stop();
            _timer.Interval = TimeSpan.FromSeconds(10); // Wait 10 seconds before the first mapprogress event, after that, consider that the map was already ready
            _timer.Start();
        }

        /// Cancels the wait.
        public void CancelWait()
        {
            _timer.Stop();
            _map.Progress -= OnMapProgress;
        }

        // Occurs when the map is loaded.
        public event EventHandler<EventArgs> Loaded;
        private void OnLoaded()
        {
            CancelWait();
            var handler = Loaded;
            if (handler != null)
                handler(this, new EventArgs());
        }

        // Security timer to avoid infinite waiting (not useful with Silverlight which anyway calls PrintPage only 7 times)
        private void OnTimerTick(object sender, EventArgs e)
        {
            if (_isProgressing)
            {
                // Progress events are coming -> wait more
                _isProgressing = false;
                _timer.Interval = TimeSpan.FromSeconds(30);
            }
            else
            {
                // No progress event since last test --> stop and consider the map as loaded
                OnLoaded();
            }
        }

        private void OnMapProgress(object sender, ProgressEventArgs e)
        {
            _isProgressing = true;
            if (e.Progress == 100)
                OnLoaded(); // map is ready
        }
    }

    // Define an attached property allowing to initialize a map by cloning the layers of another map.
    public static class CloneMap
    {
        // Map to clone attached property
        public static String GetMap(DependencyObject obj)
        {
            return (String)obj.GetValue(MapProperty);
        }

        public static void SetMap(DependencyObject obj, String value)
        {
            obj.SetValue(MapProperty, value);
        }

        public static readonly DependencyProperty MapProperty = DependencyProperty.RegisterAttached("Map", typeof(ESRI.ArcGIS.Client.Map), typeof(CloneMap), new PropertyMetadata(null, OnMapChanged));

        private static void OnMapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var map = d as ESRI.ArcGIS.Client.Map;
            if (map == null)
                return;
            var mapToClone = (ESRI.ArcGIS.Client.Map)e.NewValue;

            map.Layers.Clear();
            if (mapToClone != null)
                Clone(map, mapToClone);
        }

        // Clone a Map
        private static void Clone(ESRI.ArcGIS.Client.Map map, ESRI.ArcGIS.Client.Map mapToClone)
        {
            map.MinimumResolution = mapToClone.MinimumResolution;
            map.MaximumResolution = mapToClone.MaximumResolution;
            map.TimeExtent = mapToClone.TimeExtent;
            map.WrapAround = mapToClone.WrapAround;

            // Clone layers
            foreach (var toLayer in mapToClone.Layers.Select(CloneLayer).Where(toLayer => toLayer != null))
            {
                toLayer.InitializationFailed += (s, e) => { }; // to avoid crash if bad layer
                map.Layers.Add(toLayer); // use index in order to keep existing layers after cloned layers
            }
        }

        // Clone a Layer
        private static Layer CloneLayer(Layer layer)
        {
            Layer toLayer;
            var featureLayer = layer as FeatureLayer;

            if (layer is GraphicsLayer && (featureLayer == null || featureLayer.Url == null || featureLayer.Mode != FeatureLayer.QueryMode.OnDemand))
            {
                // Clone the layer and the graphics
                var fromLayer = layer as GraphicsLayer;
                var printLayer = new GraphicsLayer
                {
                    Renderer = fromLayer.Renderer,
                    Clusterer = fromLayer.Clusterer == null ? null : fromLayer.Clusterer.Clone(),
                    ShowLegend = fromLayer.ShowLegend,
                    RendererTakesPrecedence = fromLayer.RendererTakesPrecedence,
                    ProjectionService = fromLayer.ProjectionService
                };
                toLayer = printLayer;

                var graphicCollection = new GraphicCollection();
                foreach (var graphic in fromLayer.Graphics)
                {
                    var clone = new Graphic();

                    foreach (var kvp in graphic.Attributes)
                    {
                        if (kvp.Value is DependencyObject)
                        {
                            // If the attribute is a dependency object --> clone it
                            var clonedkvp = new KeyValuePair<string, object>(kvp.Key, (kvp.Value as DependencyObject).Clone());
                            clone.Attributes.Add(clonedkvp);
                        }
                        else
                            clone.Attributes.Add(kvp);
                    }
                    clone.Geometry = graphic.Geometry;
                    clone.Symbol = graphic.Symbol;
                    clone.Selected = graphic.Selected;
                    clone.TimeExtent = graphic.TimeExtent;
                    graphicCollection.Add(clone);
                }

                printLayer.Graphics = graphicCollection;

                toLayer.ID = layer.ID;
                toLayer.Opacity = layer.Opacity;
                toLayer.Visible = layer.Visible;
                toLayer.MaximumResolution = layer.MaximumResolution;
                toLayer.MinimumResolution = layer.MinimumResolution;
            }
            else if (layer is ElementLayer)
            {
                var printLayer = new ElementLayer();
                toLayer = printLayer;

                var fromLayer = layer as ElementLayer;

                foreach (var ele in fromLayer.Children)
                {
                    GpsCar newele = (ele as GpsCar).ConleGPSCar();

                    //3857地图
                    ESRI.ArcGIS.Client.Geometry.MapPoint pt = VechileMemDataOperate.GetProjCoord(0, 0);


                    //ESRI.ArcGIS.Client.Geometry.MapPoint pt = new ESRI.ArcGIS.Client.Geometry.MapPoint(0, 0);
                    if ((newele.Lat != null) && (newele.Lon != null) && (newele.Lat != "") && (newele.Lon != ""))
                    {
                        double lslon = double.Parse(newele.Lon);
                        double lslat = double.Parse(newele.Lat);
                        //3857地图
                        pt = VechileMemDataOperate.GetProjCoord(lslon, lslat);

                        //pt = new MapPoint(lslon, lslat);
                    }
                    ElementLayer.SetEnvelope(newele, new ESRI.ArcGIS.Client.Geometry.Envelope(pt, pt));
                    printLayer.Children.Add(newele);
                }

                toLayer.ID = layer.ID;
                toLayer.Opacity = layer.Opacity;
                toLayer.Visible = layer.Visible;
                toLayer.MaximumResolution = layer.MaximumResolution;
                toLayer.MinimumResolution = layer.MinimumResolution;
            }
            else
            {
                // Clone other layer types
                toLayer = layer.Clone();

                if (layer is GroupLayerBase)
                {
                    // Clone sublayers (not cloned in Clone() to avoid issue with graphicslayer)
                    var childLayers = new LayerCollection();
                    foreach (Layer subLayer in (layer as GroupLayerBase).ChildLayers)
                    {
                        var toSubLayer = CloneLayer(subLayer);

                        if (toSubLayer != null)
                        {
                            toSubLayer.InitializationFailed += (s, e) => { }; // to avoid crash if bad layer
                            childLayers.Add(toSubLayer);
                        }
                    }
                    ((GroupLayerBase)toLayer).ChildLayers = childLayers;
                }
            }
            return toLayer;
        }
    }

    // Generic class extention for cloning recursively a dependency object
    // Very limited implementation based on CLR properties
    // Attached properties are not taken in care except specific case for this sample
    // Is used to clone Layers and Elements of ElementLayer
    public static class CloneExtension
    {
        // Clones a dependency object.
        public static T Clone<T>(this T source) where T : DependencyObject
        {
            Type t = source.GetType(); // can be different from typeof(T)
            var clone = (T)Activator.CreateInstance(t);

            // Loop on CLR properties (except name, parent and graphics)
            foreach (PropertyInfo propertyInfo in t.GetProperties())
            {
                if (propertyInfo.Name == "Name" || propertyInfo.Name == "Parent" || propertyInfo.Name == "Graphics" || propertyInfo.Name == "ChildLayers" ||
                        !propertyInfo.CanRead || propertyInfo.GetGetMethod() == null ||
                        propertyInfo.GetIndexParameters().Length > 0)
                    continue;
                try
                {
                    Object value = propertyInfo.GetValue(source, null);
                    if (value != null)
                    {
                        if (propertyInfo.PropertyType.GetInterface("IList", true) != null && !propertyInfo.PropertyType.IsArray)
                        {
                            // Collection ==> loop on items and clone them (we suppose the collection itself is already initialized!)
                            var count = (int)propertyInfo.PropertyType.InvokeMember("get_Count", BindingFlags.InvokeMethod, null, value, null);
                            propertyInfo.PropertyType.InvokeMember("Clear", BindingFlags.InvokeMethod, null, propertyInfo.GetValue(clone, null), null); // without this line, text can be duplicated due to inlines objects added after text is set

                            for (int index = 0; index < count; index++)
                            {
                                object itemValue = propertyInfo.PropertyType.InvokeMember("get_Item", BindingFlags.InvokeMethod, null, propertyInfo.GetValue(source, null), new object[] { index });
                                propertyInfo.PropertyType.InvokeMember("Add", BindingFlags.InvokeMethod, null, propertyInfo.GetValue(clone, null), new[] { CloneDependencyObject(itemValue) });
                            }
                        }
                        else if (propertyInfo.CanWrite && propertyInfo.GetSetMethod() != null)
                        {
                            propertyInfo.SetValue(clone, CloneDependencyObject(value), null);
                        }
                    }
                }
                catch (Exception) { }
            }

            // Copy some useful attached properties (not done by reflection)
            if (source is UIElement)
            {
                DependencyProperty attachedProperty = ESRI.ArcGIS.Client.ElementLayer.EnvelopeProperty; // needed for ElementLayer
                SetDependencyProperty(attachedProperty, source, clone);
            }

            return clone;
        }

        static private object CloneDependencyObject(object source)
        {
            return source is DependencyObject && !(source is ControlTemplate) ? (source as DependencyObject).Clone() : source;
        }

        static private void SetDependencyProperty(DependencyProperty dp, DependencyObject source, DependencyObject clone)
        {
            Object value = source.GetValue(dp);
            if (value != null)
                clone.SetValue(dp, CloneDependencyObject(value));
        }
    }
}
