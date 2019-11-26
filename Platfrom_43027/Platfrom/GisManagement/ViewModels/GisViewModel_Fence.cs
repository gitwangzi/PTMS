using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Tasks;
using GisManagement.Models;
using GisManagement.Views;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Converts;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using Gsafety.Common.Controls;
using System.Reflection;

namespace GisManagement.ViewModels
{
    public partial class GisViewModel
    {
        #region Add fence
        /// <summary>
        /// draw fence tool
        /// </summary>
        public ESRI.ArcGIS.Client.Draw myDrawFence = null;
        private bool _bDrawFence = false;
        /// <summary>
        ///Remember that the received message parameters drawing fence (save by)
        /// </summary>
        private DrawFenceEventArgs _publishedEventDrawFence = null;

        /// <summary>
        /// Editing fence attribute information parameter information
        /// </summary>
        private bool _bEditFence = false;
        /// <summary>
        /// Increase acceptance fence message 
        /// (from the view button to add the fence to send: message with parameters (drawing type, radius, and fences information entities)
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(DrawFenceEventArgs publishedEvent)
        {
            try
            {
                _publishedEventDrawFence = publishedEvent;

                if (myDrawFence == null)
                {
                    myDrawFence = new ESRI.ArcGIS.Client.Draw(this.MyMap);
                    myDrawFence.DrawComplete += DrawFence_DrawComplete;
                }

                //draw polygon
                if (publishedEvent.nType == TrafficDrawType.Polygon)
                {
                    myDrawFence.DrawMode = DrawMode.Polygon;
                    myDrawFence.FillSymbol = new SimpleFillSymbol()
                    {
                        BorderBrush = new SolidColorBrush(Colors.Red) { },
                        BorderThickness = 2,
                        Fill = new SolidColorBrush(Colors.Green) { Opacity = 0.2 }
                    };
                    myDrawFence.IsEnabled = true;
                }
                //draw point
                if (publishedEvent.nType == TrafficDrawType.Point)
                {
                    myDrawFence.DrawMode = DrawMode.Point;
                    myDrawFence.IsEnabled = true;
                }
                //draw line
                if (publishedEvent.nType == TrafficDrawType.Line)
                {
                    myDrawFence.DrawMode = DrawMode.Polyline;
                    myDrawFence.LineSymbol = new SimpleLineSymbol()
                    {
                        Color = new SolidColorBrush(Colors.Red),
                        Width = 2
                    };
                    myDrawFence.IsEnabled = true;
                }
                //draw circule
                if(publishedEvent.nType == TrafficDrawType.Circular)
                {
                    myDrawFence.DrawMode = DrawMode.Circle;
                    myDrawFence.FillSymbol = new SimpleFillSymbol()
                    {
                        BorderBrush = new SolidColorBrush(Colors.Red) { },
                        BorderThickness = 2,
                        Fill = new SolidColorBrush(Colors.Green) { Opacity = 0.2 }
                    };
                    myDrawFence.IsEnabled = true;
                }
                //draw rectangle
                if(publishedEvent.nType == TrafficDrawType.Rectangle)
                {
                    myDrawFence.DrawMode = DrawMode.Rectangle;
                    myDrawFence.FillSymbol = new SimpleFillSymbol()
                    {
                        BorderBrush = new SolidColorBrush(Colors.Red) { },
                        BorderThickness = 2,
                        Fill = new SolidColorBrush(Colors.Green) { Opacity = 0.2 }
                    };
                    myDrawFence.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("GisViewModel", ex);
            }
        }
        /// <summary>
        /// Draw complete event:
        /// Draw fence (point, line, surface) plotted lines (bus and coach), draw site (and long-distance bus lines)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DrawFence_DrawComplete(object sender, ESRI.ArcGIS.Client.DrawEventArgs args)
        {
            try
            {
                if (_publishedEventDrawFence == null)
                {
                    myDrawFence.IsEnabled = false;
                    return;
                }
                //draw polygon
                if (_publishedEventDrawFence.nType == TrafficDrawType.Polygon)
                {
                    SaveFence(args.Geometry, null);
                }

                //Generate 30 points based on points plotted to construct approximate circle polygon
                if (_publishedEventDrawFence.nType == TrafficDrawType.Point)
                {
                    Graphic pCircleGraphic = new Graphic();
                    List<MapPoint> listPts = new List<MapPoint>();
                    MapPoint pPtCenter = args.Geometry as MapPoint;
                    for (double i = 0; i <= 360; i += 12)
                    {
                        MapPoint pPtNew = new MapPoint();
                        pPtNew.X = pPtCenter.X - Math.Cos(Math.PI * i / 180) * _publishedEventDrawFence.dDist;
                        pPtNew.Y = pPtCenter.Y - Math.Sin(Math.PI * i / 180) * _publishedEventDrawFence.dDist;
                        listPts.Add(pPtNew);
                    }
                    ESRI.ArcGIS.Client.Geometry.PointCollection pPtColl = new ESRI.ArcGIS.Client.Geometry.PointCollection(listPts);
                    ESRI.ArcGIS.Client.Geometry.Polygon pCircle = new ESRI.ArcGIS.Client.Geometry.Polygon();
                    pCircle.Rings.Add(pPtColl);
                    pCircleGraphic.Geometry = pCircle;
                    pCircleGraphic.Symbol = new SimpleFillSymbol()
                    {
                        BorderBrush = new SolidColorBrush(Colors.Red) { },
                        BorderThickness = 2,
                        Fill = new SolidColorBrush(Colors.Green) { Opacity = 0 }
                    };

                    Graphic newgraphic = new Graphic()
                    {
                        Geometry = args.Geometry,
                        Symbol = new ESRI.ArcGIS.Client.Symbols.SimpleMarkerSymbol()
                        {
                            Size = 18,
                            Color = new SolidColorBrush { Color = Colors.Red },
                            Style = SimpleMarkerSymbol.SimpleMarkerStyle.Cross
                        }
                    };

                    GraphicsLayer pLayer = MyMap.Layers[ConstDefine.TempDrawLayer] as GraphicsLayer;
                    if (pLayer != null)
                    {
                        pLayer.Graphics.Add(newgraphic);
                    }

                    MyMap.Extent = pCircle.Extent.Expand(3);
                    SaveFence(pCircle, pPtCenter);
                }
                //Buffer according to point generating a line drawn, but the drawing does not exceed <= 17
                if (_publishedEventDrawFence.nType == TrafficDrawType.Line)
                {
                    Graphic g = new Graphic();
                    GraphicsLayer pGraphicLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                    g.Symbol = new SimpleFillSymbol()
                    {
                        BorderBrush = new SolidColorBrush(Colors.Red) { },
                        BorderThickness = 2,
                        Fill = new SolidColorBrush(Colors.Green) { Opacity = 0.5 }
                    };

                    g.Geometry = GeoneralGISFun.GetLineBuffer(args.Geometry as ESRI.ArcGIS.Client.Geometry.Polyline, _publishedEventDrawFence.dDist);
                    g.Geometry.SpatialReference = MyMap.SpatialReference;
                    pGraphicLayer.Graphics.Add(g);
                    MyMap.Extent = g.Geometry.Extent.Expand(1.5);
                    pGraphicLayer.Visible = true;
                    ESRI.ArcGIS.Client.Geometry.PointCollection pts = (g.Geometry as ESRI.ArcGIS.Client.Geometry.Polygon).Rings[0];
                    SaveFence(g.Geometry, null);
                }

                if(_publishedEventDrawFence.nType == TrafficDrawType.Circular)
                {
                    _publishedEventDrawFence.eleFence.FenceType = (short)TrafficDrawType.Circular;
                    SaveFence(args.Geometry, null);
                    //ESRI.ArcGIS.Client.Geometry.Polygon polygon = args.Geometry as ESRI.ArcGIS.Client.Geometry.Polygon;
                    //ESRI.ArcGIS.Client.Geometry.PointCollection pts = polygon.Rings[0]; //37个点
                    //double xmax = polygon.Extent.XMax;
                    //double xmin = polygon.Extent.XMin;
                    //MapPoint xhigh = new MapPoint();
                    //MapPoint xlow = new MapPoint();
                    //foreach(var item  in pts)
                    //{
                    //    if(item.X == xmax)
                    //    {
                    //        xhigh = item;
                    //    }

                    //    if(item.X == xmin)
                    //    {
                    //        xlow = item;
                    //    }
                    //}

                    //MapPoint pCenter = new MapPoint((xhigh.X + xlow.X) / 2, (xhigh.Y + xlow.Y) / 2);
                    //double r = CalculateRadius(xhigh, xlow);

                    //List<MapPoint> listPts = new List<MapPoint>();
                    //listPts.Add(pCenter);
                    //ESRI.ArcGIS.Client.Geometry.PointCollection pc = new ESRI.ArcGIS.Client.Geometry.PointCollection(listPts);
                    //SaveCircle(args.Geometry, pc, Convert.ToInt32(r));
                }

                if (_publishedEventDrawFence.nType == TrafficDrawType.Rectangle)
                {
                    ESRI.ArcGIS.Client.Geometry.Envelope pFenceRectangle = args.Geometry as Envelope;
                    string dia = pFenceRectangle.Extent.ToString();  //原始对角线点经纬度 左下--右上
                    string[] coordsep = { "," };
                    List<string> poi = new List<string>(dia.Split(coordsep, StringSplitOptions.RemoveEmptyEntries));
                    double leftup_j = Convert.ToDouble(poi[0], System.Globalization.CultureInfo.InvariantCulture);
                    double leftup_w = Convert.ToDouble(poi[1], System.Globalization.CultureInfo.InvariantCulture) + pFenceRectangle.Height;
                    double rightdown_j = Convert.ToDouble(poi[2], System.Globalization.CultureInfo.InvariantCulture);
                    double rightdowm_w = Convert.ToDouble(poi[3], System.Globalization.CultureInfo.InvariantCulture) - pFenceRectangle.Height;

                    List<MapPoint> listPts = new List<MapPoint>();
                    listPts.Add(new MapPoint(leftup_j, Convert.ToDouble(poi[1], System.Globalization.CultureInfo.InvariantCulture)));
                    listPts.Add(new MapPoint(leftup_j, leftup_w));
                    listPts.Add(new MapPoint(rightdown_j, Convert.ToDouble(poi[3], System.Globalization.CultureInfo.InvariantCulture)));
                    listPts.Add(new MapPoint(rightdown_j, rightdowm_w));
                    listPts.Add(new MapPoint(leftup_j, Convert.ToDouble(poi[1], System.Globalization.CultureInfo.InvariantCulture)));
                    ESRI.ArcGIS.Client.Geometry.PointCollection pPtColl = new ESRI.ArcGIS.Client.Geometry.PointCollection(listPts);
                    SaveRectangleFence(args.Geometry, pPtColl);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("GisViewModel", ex);
            }
        }

        private void SaveCircle(ESRI.ArcGIS.Client.Geometry.Geometry geometry, ESRI.ArcGIS.Client.Geometry.PointCollection pCenter,int radius)
        {
            var window = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_CheckAddControlPointInfo"), MessageDialogButton.OkAndCancel);
            window.Closed += delegate (object sender, EventArgs e)
            {
                ChildWindow childwindow = sender as ChildWindow;
                if (childwindow.DialogResult == true)
                {
                    try
                    {

                        GraphicsLayer pLayerFence = MyMap.Layers[ConstDefine.TrafficFeatureFenceLayerName] as GraphicsLayer;
                        if (pLayerFence != null)
                        {
                            _publishedEventDrawFence.eleFence.Pts = FormatGeometryPoints(pCenter);
                            _publishedEventDrawFence.eleFence.PointCount = pCenter.Count;
                            _publishedEventDrawFence.eleFence.FenceType = (short)TrafficDrawType.Circular;
                            _publishedEventDrawFence.eleFence.Radius = radius;
                            _publishedEventDrawFence.eleFence.RegionProperty = UpdateFencePropertyWithPts(_publishedEventDrawFence.eleFence.RegionProperty, _publishedEventDrawFence.eleFence.Pts);
                            TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
                            trafficServiceClient.InsertTrafficFenceCompleted += trafficServiceClient_InsertTrafficFenceCompleted;

                            TrafficFence newe = _publishedEventDrawFence.eleFence.Clone(_publishedEventDrawFence.eleFence);
                            newe.CreateTime = newe.CreateTime.ToUniversalTime();
                            trafficServiceClient.InsertTrafficFenceAsync(newe);

                            ESRI.ArcGIS.Client.Graphic gPhic = new Graphic();
                            gPhic.Geometry = geometry;

                            gPhic.Symbol = new SimpleFillSymbol()
                            {
                                BorderBrush = new SolidColorBrush(Colors.Red) { },
                                BorderThickness = 2,
                                Fill = new SolidColorBrush(Colors.Green) { Opacity = 0.2 }

                            };
                            pLayerFence.Graphics.Add(gPhic);


                            GraphicsLayer pGraphicLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                            pGraphicLayer.Graphics.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        ApplicationContext.Instance.Logger.LogException("GisViewModel", ex);
                    }
                }
            };
        }

        private const double EARTH_RADIUS = 6378137;

        private static double Rad(double d)
        {

            return (double)d * Math.PI / 180d;

        }
        private double CalculateRadius(MapPoint xh,MapPoint xl)
        {
            double radlat1 = Rad(xh.Y);
            double radlng1 = Rad(xh.X);
            double radlat2 = Rad(xl.Y);
            double radlng2 = Rad(xl.X);

            double a = radlat1 - radlat2;
            double b = radlng1 - radlng2;

            double result =( 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radlat1) * Math.Cos(radlat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS)/2.0;

            double radius = Math.Round(result * 10000.0) / 10000.0;
            return radius;
        }

        private void SaveRectangleFence(ESRI.ArcGIS.Client.Geometry.Geometry geometry, ESRI.ArcGIS.Client.Geometry.PointCollection ptColl)
        {
            _bDrawFence = false;
            myDrawFence.IsEnabled = false;

            var window = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_CheckAddControlPointInfo"), MessageDialogButton.OkAndCancel);
            window.Closed += delegate (object sender, EventArgs e)
            {
                ChildWindow childwindow = sender as ChildWindow;
                if (childwindow.DialogResult == true)
                {
                    try
                    {
                        GraphicsLayer pLayerFence = MyMap.Layers[ConstDefine.TrafficFeatureFenceLayerName] as GraphicsLayer;
                        if (pLayerFence != null)
                        {
                            _publishedEventDrawFence.eleFence.Pts = FormatGeometryPoints(ptColl);
                            _publishedEventDrawFence.eleFence.FenceType = (short)TrafficDrawType.Rectangle;
                            _publishedEventDrawFence.eleFence.PointCount = ptColl.Count;
                            _publishedEventDrawFence.eleFence.RegionProperty = UpdateFencePropertyWithPts(_publishedEventDrawFence.eleFence.RegionProperty, _publishedEventDrawFence.eleFence.Pts);
                            TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
                            trafficServiceClient.InsertTrafficFenceCompleted += trafficServiceClient_InsertTrafficFenceCompleted;

                            TrafficFence newe = _publishedEventDrawFence.eleFence.Clone(_publishedEventDrawFence.eleFence);
                            newe.CreateTime = newe.CreateTime.ToUniversalTime();
                            trafficServiceClient.InsertTrafficFenceAsync(newe);

                            ESRI.ArcGIS.Client.Graphic gPhic = new Graphic();
                            gPhic.Geometry = geometry;

                            gPhic.Symbol = new SimpleFillSymbol()
                            {
                                BorderBrush = new SolidColorBrush(Colors.Red) { },
                                BorderThickness = 2,
                                Fill = new SolidColorBrush(Colors.Green) { Opacity = 0.2 }

                            };
                            pLayerFence.Graphics.Add(gPhic);


                            GraphicsLayer pGraphicLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                            pGraphicLayer.Graphics.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        ApplicationContext.Instance.Logger.LogException("GisViewModel", ex);
                    }

                }
            };

        }

        /// <summary>
        /// save Information of Fence
        /// </summary>
        /// <param name="geometry"></param>
        private void SaveFenceEx(ESRI.ArcGIS.Client.Geometry.Geometry geometry, ESRI.ArcGIS.Client.Geometry.MapPoint pCenter)
        {
            try
            {

                GraphicsLayer pLayerFence = MyMap.Layers[ConstDefine.TrafficFeatureFenceLayerName] as GraphicsLayer;
                if (pLayerFence != null)
                {
                    _publishedEventDrawFence.eleFence.Pts = FormatGeometryPoints((geometry as ESRI.ArcGIS.Client.Geometry.Polygon).Rings[0]);
                    _publishedEventDrawFence.eleFence.PointCount = (geometry as ESRI.ArcGIS.Client.Geometry.Polygon).Rings[0].Count;
                    _publishedEventDrawFence.eleFence.RegionProperty = UpdateFencePropertyWithPts(_publishedEventDrawFence.eleFence.RegionProperty, _publishedEventDrawFence.eleFence.Pts);
                    TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
                    trafficServiceClient.InsertTrafficFenceCompleted += trafficServiceClient_InsertTrafficFenceCompleted;

                    TrafficFence newe = _publishedEventDrawFence.eleFence.Clone(_publishedEventDrawFence.eleFence);
                    newe.CreateTime = newe.CreateTime.ToUniversalTime();
                    trafficServiceClient.InsertTrafficFenceAsync(newe);

                    ESRI.ArcGIS.Client.Graphic gPhic = new Graphic();
                    gPhic.Geometry = geometry;

                    gPhic.Symbol = new SimpleFillSymbol()
                    {
                        BorderBrush = new SolidColorBrush(Colors.Red) { },
                        BorderThickness = 2,
                        Fill = new SolidColorBrush(Colors.Green) { Opacity = 0.2 }

                    };
                    pLayerFence.Graphics.Add(gPhic);


                    GraphicsLayer pGraphicLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                    pGraphicLayer.Graphics.Clear();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("GisViewModel", ex);
            }
        }

        /// <summary>
        /// 依据pts中第一点的正负来决定regionproperty中的经纬度参数
        /// </summary>
        /// <param name="regionproperty"></param>
        /// <param name="pts"></param>
        /// <returns></returns>
        private string UpdateFencePropertyWithPts(string regionproperty, string pts)
        {
            if (pts == "") return regionproperty;
            string[] coordsep = { ";" };
            string[] xysep = { "," };

            string[] lst = pts.Split(coordsep, StringSplitOptions.RemoveEmptyEntries);
            string coord = lst[0];
            string[] xy = coord.Split(xysep, StringSplitOptions.RemoveEmptyEntries);
            //xy[0],xy[1];
            if (xy[0].IndexOf("-") > -1) //经度为负
            {
                regionproperty = AddOrRemoveProperty(regionproperty, false, FENCE_RegionProperty.East_Lon);
                regionproperty = AddOrRemoveProperty(regionproperty, true, FENCE_RegionProperty.West_Lon);
            }
            else
            {
                regionproperty = AddOrRemoveProperty(regionproperty, true, FENCE_RegionProperty.East_Lon);
                regionproperty = AddOrRemoveProperty(regionproperty, false, FENCE_RegionProperty.West_Lon);
            }
            if (xy[1].IndexOf("-") > -1) //纬度为负
            {
                regionproperty = AddOrRemoveProperty(regionproperty, false, FENCE_RegionProperty.North_Lat);
                regionproperty = AddOrRemoveProperty(regionproperty, true, FENCE_RegionProperty.South_Lat);
            }
            else
            {
                regionproperty = AddOrRemoveProperty(regionproperty, true, FENCE_RegionProperty.North_Lat);
                regionproperty = AddOrRemoveProperty(regionproperty, false, FENCE_RegionProperty.South_Lat);
            }
            return regionproperty;
        }

        private string AddOrRemoveProperty(string regionproperty, bool isAdd, FENCE_RegionProperty ftype)
        {
            List<string> lst = new List<string>();
            if (regionproperty != null)
            {
                lst = regionproperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            }

            if (isAdd == true)
            {
                if (lst.IndexOf(((int)(ftype)).ToString()) == -1)
                {
                    lst.Add(((int)(ftype)).ToString());
                }
            }
            else
            {
                if (lst.IndexOf(((int)(ftype)).ToString()) > -1)
                {
                    lst.Remove(((int)(ftype)).ToString());
                }
            }
            string newRegionProperty = "";
            foreach (string str in lst)
            {
                if (newRegionProperty == "")
                {
                    newRegionProperty = str;
                }
                else
                {
                    newRegionProperty = newRegionProperty + "," + str;
                }
            }
            regionproperty = newRegionProperty;
            return regionproperty;
        }

        void trafficServiceClient_InsertTrafficFenceCompleted(object sender, InsertTrafficFenceCompletedEventArgs e)
        {
            if (!((e.Error == null) && (e.Result.Result == true)))
            {//添加失败
                MessageBoxHelper.ShowDialog(e.Error.Message);
                ApplicationContext.Instance.Logger.LogException("GisViewModel", e.Error);
            }
            else
            {
                EventAggregator.Publish<AddFenceCompleteArgs>(new AddFenceCompleteArgs());
            }
        }
        /// <summary>
        /// save fence
        /// </summary>
        /// <param name="geometry"></param>
        private void SaveFence(ESRI.ArcGIS.Client.Geometry.Geometry geometry, ESRI.ArcGIS.Client.Geometry.Geometry pCenter)
        {
            _bDrawFence = false;
            myDrawFence.IsEnabled = false;

            var window = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_CheckAddControlPointInfo"), MessageDialogButton.OkAndCancel);
            window.Closed += delegate(object sender, EventArgs e)
            {
                ChildWindow childwindow = sender as ChildWindow;
                if (childwindow.DialogResult == true)
                {

                    try
                    {
                        ESRI.ArcGIS.Client.Geometry.Polygon pFencePolygon = geometry as ESRI.ArcGIS.Client.Geometry.Polygon;

                        if (pFencePolygon == null || pFencePolygon.Rings.Count != 1 || GeoneralGISFun.PolygonIsLine(pFencePolygon))
                        {
                            //  MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("GIS_FenceNotPolygon"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageBoxButton.OK);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_FenceNotPolygon"), MessageDialogButton.Ok);

                            return;
                        }
                        //Determine whether more than 35 points
                        ESRI.ArcGIS.Client.Geometry.PointCollection pts = pFencePolygon.Rings[0];
                        if (pts.Count > 40)
                        {

                            var result = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_FenceLimitPoints"), MessageDialogButton.OkAndCancel);
                            result.Closed += delegate(object send, EventArgs er)
                            {
                                ChildWindow child = send as ChildWindow;
                                if (child.DialogResult == true)
                                {
                                    StartEditDrawFence(geometry);
                                    return;
                                }
                                else
                                {
                                    return;
                                }
                            };
                        }
                        //Determine whether the self-intersection
                        if (GeoneralGISFun.IsCrossSelf(geometry))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_FenceCross"),
                                 MessageDialogButton.Ok);
                            return;
                        }
                        SaveFenceEx(geometry, pCenter as MapPoint);
                    }
                    catch (Exception ex)
                    {
                        ApplicationContext.Instance.Logger.LogException("GisViewModel", ex);
                    }

                }
            };

        }



        /// <summary>
        /// Start editing drawing does not meet the requirements of the fence
        /// </summary>
        /// <param name="geo"></param>
        private void StartEditDrawFence(ESRI.ArcGIS.Client.Geometry.Geometry geo)
        {
            //if (EditGeometryVisble == Visibility.Collapsed)
            //    EditGeometryVisble = Visibility.Visible;
            try
            {
                if (_pEditGeometry == null)
                {
                    object mview = Router.ViewQuery(GisName.MonitorGisView);
                    if (mview != null)
                    {
                        _pEditGeometry = (mview as GisView).EditGeoemtryTool;
                        _pEditGeometry.GeometryEdit += _pEditGeometry_GeometryEdit;
                    }
                }
                GraphicsLayer pGraphicLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                if (pGraphicLayer != null)
                {
                    pGraphicLayer.Visible = true;
                    pGraphicLayer.Graphics.Clear();
                    Graphic g = new Graphic();
                    g.Geometry = geo;
                    g.Symbol = new SimpleFillSymbol()
                    {
                        BorderBrush = new SolidColorBrush(Colors.Red) { },
                        BorderThickness = 2,
                        Fill = new SolidColorBrush(Colors.Green) { Opacity = 0 }
                    };
                    pGraphicLayer.Graphics.Add(g);
                    _pEditGeometry.StartEdit(g);
                }
                _bDrawFence = true;
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("StartEditDrawFence_Error", ee);
            }
        }

        private Graphic GetFenceRectangle(string fencepts)
        {
            if (fencepts == "") return null;
            string[] coordsep = { ";" };
            string[] xysep = { "," };
            string[] list = fencepts.Split(coordsep, StringSplitOptions.RemoveEmptyEntries);

            ESRI.ArcGIS.Client.Geometry.PointCollection pts = new ESRI.ArcGIS.Client.Geometry.PointCollection();
            DisplayLatConvert Latcon = new DisplayLatConvert();
            DisplayLonConvert Loncon = new DisplayLonConvert();
            foreach (string coord in list)
            {
                string[] xy = coord.Split(xysep, StringSplitOptions.RemoveEmptyEntries);
                double lslon = double.Parse(Loncon.ConvertBack(xy[0], null, null, null).ToString());
                double lslat = double.Parse(Latcon.ConvertBack(xy[1], null, null, null).ToString());

                //3857地图
                MapPoint pt = GpsCarListViewModel.GetProjCoord(lslon, lslat);

                //MapPoint pt = new MapPoint(lslon, lslat);

                pts.Add(pt);
            }

            ESRI.ArcGIS.Client.Geometry.Envelope envelope = new ESRI.ArcGIS.Client.Geometry.Envelope(pts[0], pts[1]);


            ESRI.ArcGIS.Client.Graphic graphic = new ESRI.ArcGIS.Client.Graphic()
            {
                Geometry = envelope,
                Symbol = new SimpleFillSymbol()
                {
                    BorderBrush = new SolidColorBrush(Colors.Red) { },
                    BorderThickness = 2,
                    Fill = new SolidColorBrush(Colors.Green) { Opacity = 0.5 }
                }
            };
            return graphic;
        }

        private Graphic GetFencePolygon(string fencepts)
        {
            if (fencepts == "") return null;
            string[] coordsep = { ";" };
            string[] xysep = { "," };
            string[] list = fencepts.Split(coordsep, StringSplitOptions.RemoveEmptyEntries);

            ESRI.ArcGIS.Client.Geometry.PointCollection pts = new ESRI.ArcGIS.Client.Geometry.PointCollection();
            DisplayLatConvert Latcon = new DisplayLatConvert();
            DisplayLonConvert Loncon = new DisplayLonConvert();
            foreach (string coord in list)
            {
                string[] xy = coord.Split(xysep, StringSplitOptions.RemoveEmptyEntries);
                double lslon = double.Parse(Loncon.ConvertBack(xy[0], null, null, null).ToString());
                double lslat = double.Parse(Latcon.ConvertBack(xy[1], null, null, null).ToString());
                
                //3857地图
                MapPoint pt = GpsCarListViewModel.GetProjCoord(lslon, lslat);

                //MapPoint pt = new MapPoint(lslon, lslat);

                pts.Add(pt);
            }

            ESRI.ArcGIS.Client.Geometry.Polygon polygon = new ESRI.ArcGIS.Client.Geometry.Polygon();
            polygon.Rings.Add(pts);


            ESRI.ArcGIS.Client.Graphic graphic = new ESRI.ArcGIS.Client.Graphic()
            {
                Geometry = polygon,
                Symbol = new SimpleFillSymbol()
                {
                    BorderBrush = new SolidColorBrush(Colors.Red) { },
                    BorderThickness = 2,
                    Fill = new SolidColorBrush(Colors.Green) { Opacity = 0.5 }
                }
            };
            return graphic;
        }
        private Graphic GetRouteLine(string routepts)
        {
            if (routepts == "") return null;
            string[] coordsep = { ";" };
            string[] xysep = { "," };
            string[] list = routepts.Split(coordsep, StringSplitOptions.RemoveEmptyEntries);

            ESRI.ArcGIS.Client.Geometry.PointCollection pts = new ESRI.ArcGIS.Client.Geometry.PointCollection();
            DisplayLatConvert Latcon = new DisplayLatConvert();
            DisplayLonConvert Loncon = new DisplayLonConvert();
            foreach (string coord in list)
            {
                string[] xy = coord.Split(xysep, StringSplitOptions.RemoveEmptyEntries);
                double lslon = double.Parse(Loncon.ConvertBack(xy[0], null, null, null).ToString());
                double lslat = double.Parse(Latcon.ConvertBack(xy[1], null, null, null).ToString());
                //3857地图
                MapPoint pt = GpsCarListViewModel.GetProjCoord(lslon, lslat);

               // MapPoint pt = new MapPoint(lslon, lslat);

                pts.Add(pt);
            }

            ESRI.ArcGIS.Client.Geometry.Polyline line = new ESRI.ArcGIS.Client.Geometry.Polyline();
            line.Paths.Add(pts);


            ESRI.ArcGIS.Client.Graphic graphic = new ESRI.ArcGIS.Client.Graphic()
            {
                Geometry = line,
                Symbol = new SimpleLineSymbol()
                {
                    Color = new SolidColorBrush(Colors.Blue),
                    Width = 2
                }
            };
            return graphic;
        }
        private string FormatGeometryPoints(ESRI.ArcGIS.Client.Geometry.PointCollection ppts)
        {
            DisplayLatConvert Latcon = new DisplayLatConvert();
            DisplayLonConvert Loncon = new DisplayLonConvert();
            string ptsstring = "";
            foreach (ESRI.ArcGIS.Client.Geometry.MapPoint pt in ppts)
            {
                //3857地图
                ESRI.ArcGIS.Client.Geometry.MapPoint geopt = GpsCarListViewModel.GetGeoCoord(pt.X, pt.Y);

                //ESRI.ArcGIS.Client.Geometry.MapPoint geopt = pt;
                ptsstring = ptsstring + Loncon.ConvertToSave(geopt.X, null, null, null).ToString() + "," + Latcon.ConvertToSave(geopt.Y, null, null, null).ToString() + ";";
            }
            return ptsstring;
        }

        #endregion
        #region Delete Fence
        /// <summary>
        /// Delete Fence
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(DeleteFenceArgs publishedEvent)
        {
            try
            {
                GraphicsLayer pLayerFence = MyMap.Layers[ConstDefine.TrafficFeatureFenceLayerName] as GraphicsLayer;
                if (pLayerFence != null) pLayerFence.Graphics.Clear();

                _editGeometrypublishedEvent = null;
                EditGeometryVisble = Visibility.Collapsed;


                TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
                trafficServiceClient.DeleteTrafficFenceByIDCompleted += trafficServiceClient_DeleteTrafficFenceByIDCompleted;
                trafficServiceClient.DeleteTrafficFenceByIDAsync(publishedEvent.ID);

                if (publishedEvent != null)
                {
                    MonitorList.FenceGraphicHelp.RemoveGraphicByParentID(publishedEvent.ID, TrafficeGraphictype.fence);
                    MonitorList.FenceGraphicHelp.RemoveGraphicByParentID(publishedEvent.ID, TrafficeGraphictype.ControlPoint);
                    MonitorList.FenceGraphicHelp.RemoveGraphicByParentID(publishedEvent.ID, TrafficeGraphictype.FenceTextInfo);
                    MonitorList.FenceGraphicHelp.RemoveGraphicByParentID(publishedEvent.ID, TrafficeGraphictype.ControlPointTextInfo);

                    MonitorList.MonitorGraphicHelp.RemoveGraphicByChildID(publishedEvent.ID, TrafficeGraphictype.fence);
                    MonitorList.MonitorGraphicHelp.RemoveGraphicByChildID(publishedEvent.ID, TrafficeGraphictype.ControlPoint);
                    MonitorList.MonitorGraphicHelp.RemoveGraphicByChildID(publishedEvent.ID, TrafficeGraphictype.FenceTextInfo);
                    MonitorList.MonitorGraphicHelp.RemoveGraphicByChildID(publishedEvent.ID, TrafficeGraphictype.ControlPointTextInfo);
                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("DeleteFenceArgs_Error", ee);
            }
        }

        void trafficServiceClient_DeleteTrafficFenceByIDCompleted(object sender, DeleteTrafficFenceByIDCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ApplicationContext.Instance.Logger.LogException("DeleteFenceArgs_Error", e.Error);
            }
        }

        #endregion

        #region Show fence Geometry object interface select a fence, the fence map display, hiding the other fence
        /// <summary>
        /// Show fence messages issued by the interface, where the reception
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(ShowFenceInfoArgs publishedEvent)
        {
            _editGeometrypublishedEvent = null;
            if ((publishedEvent.selectEleFence == null) && (publishedEvent.selectRoute == null))
            {
                return;
            }
            try
            {
                if (publishedEvent.Featuetype == TrafficFeature.Traffic_PolygonFence)
                {
                    GraphicsLayer pLayerFence = MyMap.Layers[ConstDefine.TrafficFeatureFenceLayerName] as GraphicsLayer;
                    if (pLayerFence != null)
                    {
                        pLayerFence.Graphics.Clear();
                        Graphic g = new Graphic();
                        if (publishedEvent.selectEleFence.FenceType == (short)TrafficDrawType.Polygon)
                        {
                             g = GetFencePolygon(publishedEvent.selectEleFence.Pts);
                        }
                        else if(publishedEvent.selectEleFence.FenceType == (short)TrafficDrawType.Rectangle)
                        {
                            g = GetFencePolygon(publishedEvent.selectEleFence.Pts);
                        }
                        else if(publishedEvent.selectEleFence.FenceType == (short)TrafficDrawType.Circular)
                        {
                            g = GetFencePolygon(publishedEvent.selectEleFence.Pts);
                        }
                        
                        pLayerFence.Graphics.Add(g);
                        pLayerFence.Visible = true;

                        _bEditFence = false;

                        MyMap.ZoomTo(g.Geometry);
                    }
                }
                else if (publishedEvent.Featuetype == TrafficFeature.Traffic_Route)
                {
                    GraphicsLayer pLayerRoute = MyMap.Layers[ConstDefine.TrafficFeatureRouteLayerName] as GraphicsLayer;
                    if (pLayerRoute != null)
                    {
                        pLayerRoute.Graphics.Clear();
                        Graphic g = GetRouteLine(publishedEvent.selectRoute.Pts);
                        pLayerRoute.Graphics.Add(g);
                        pLayerRoute.Visible = true;

                        _bEditRoute = false;

                        MyMap.ZoomTo(g.Geometry);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("GisViewModel", ex);
            }

        }

        private Graphic GetFenceCircle(string fencepts,decimal radius)
        {
            if (fencepts == "") return null;
            string[] coordsep = { ";" };
            string[] xysep = { "," };
            string[] list = fencepts.Split(coordsep, StringSplitOptions.RemoveEmptyEntries);

            ESRI.ArcGIS.Client.Geometry.PointCollection pts = new ESRI.ArcGIS.Client.Geometry.PointCollection();
            DisplayLatConvert Latcon = new DisplayLatConvert();
            DisplayLonConvert Loncon = new DisplayLonConvert();
            foreach (string coord in list)
            {
                string[] xy = coord.Split(xysep, StringSplitOptions.RemoveEmptyEntries);
                double lslon = double.Parse(Loncon.ConvertBack(xy[0], null, null, null).ToString());
                double lslat = double.Parse(Latcon.ConvertBack(xy[1], null, null, null).ToString());

                //3857地图
                MapPoint pt = GpsCarListViewModel.GetProjCoord(lslon, lslat);

               // MapPoint pt = new MapPoint(lslon, lslat);

                pts.Add(pt);
            }

            ESRI.ArcGIS.Client.Geometry.Envelope envelope = new ESRI.ArcGIS.Client.Geometry.Envelope(pts[0], pts[1]);


            ESRI.ArcGIS.Client.Graphic graphic = new ESRI.ArcGIS.Client.Graphic()
            {
                Geometry = envelope,
                Symbol = new SimpleFillSymbol()
                {
                    BorderBrush = new SolidColorBrush(Colors.Red) { },
                    BorderThickness = 2,
                    Fill = new SolidColorBrush(Colors.Green) { Opacity = 0.5 }
                }
            };
            return graphic;
        }

        /// <summary>
        /// Layers failed update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pFeaLayerFence_UpdateFailed(object sender, TaskFailedEventArgs e)
        {
            ApplicationContext.Instance.Logger.LogException("pFeaLayerFence_UpdateFailed", e.Error);
        }
        /// <summary>
        /// Update successful event selected layers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void pFeaLayerFence_UpdateCompleted(object sender, EventArgs e)
        {
            try
            {
                FeatureLayer pFeaLayerFence = MyMap.Layers[ConstDefine.TrafficFeatureFenceLayerName] as FeatureLayer;
                if (pFeaLayerFence != null)
                {
                    pFeaLayerFence.ClearSelection();
                    if (pFeaLayerFence.Graphics.Count == 1)
                    {
                        pFeaLayerFence.ClearSelection();
                        pFeaLayerFence.Refresh();
                        this.MyMap.Extent = pFeaLayerFence.Graphics[0].Geometry.Extent.Expand(2);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("pFeaLayerFence_UpdateCompleted", ex);
            }
        }
        #endregion


        /// <summary>
        /// After the fence layer data change notification interface refresh (add, edit and delete)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pFeaLayerFence_EndSaveEdits(TrafficFence csUpdateCompleteArgs)
        {
            try
            {
                EventAggregator.Publish<RefreshTrafficSelectStatus>(new RefreshTrafficSelectStatus() { });
                //If modified, the shape may be modified, may be is the modification information 
                //EventAggregator.Publish<RefreshTrafficManagerListArgs>(new RefreshTrafficManagerListArgs() { nType = 0 });
                if (_bEditFence == true && csUpdateCompleteArgs != null)
                {
                    csUpdateCompleteArgs.RegionProperty = UpdateFencePropertyWithPts(csUpdateCompleteArgs.RegionProperty, csUpdateCompleteArgs.Pts);

                    TrafficFence newe = csUpdateCompleteArgs.Clone(csUpdateCompleteArgs);
                    newe.CreateTime.ToUniversalTime();

                    TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
                    trafficServiceClient.UpdateTrafficFenceCompleted += trafficServiceClient_UpdateTrafficFenceCompleted;
                    trafficServiceClient.UpdateTrafficFenceAsync(newe);
                    _bEditFence = false;

                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("pFeaLayerFence_SaveEditsFailed", ex);
            }
        }

        void trafficServiceClient_UpdateTrafficFenceCompleted(object sender, UpdateTrafficFenceCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Faild"), MessageDialogButton.Ok);
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
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                               e.Result.ExceptionMessage);
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                    }

                    return;
                }
            }
        }

    }
}
