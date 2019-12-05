using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using GisManagement.Models;
using GisManagement.Views;
using Gsafety.PTMS.Share;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
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

namespace GisManagement.ViewModels
{
    public partial class GisViewModel
    {
        /// <summary>
        /// Query objects plotted
        /// </summary>
        public ESRI.ArcGIS.Client.Draw QueryDraw;
        private List<ElementLayerDefine> _RealTimeElementDisplayLst = new List<ElementLayerDefine>();
        private List<ElementLayerDefine> _MonitorAlarmElementDisplayLst = new List<ElementLayerDefine>();
        private List<ElementLayerDefine> _HistoryAlarmElementDisplayLst = new List<ElementLayerDefine>();
        private List<ElementLayerDefine> _MonitorAlertElementDisplayLst = new List<ElementLayerDefine>();
        private List<ElementLayerDefine> _HistoryAlertElementDisplayLst = new List<ElementLayerDefine>();
        private List<ElementLayerDefine> _TrafficElementDisplayLst = new List<ElementLayerDefine>();

        /// <summary>
        /// 空构造
        /// </summary>
        public GisViewModel()
        {
            try
            {
                InitSearchProperty();//地理位置查询


                _RealTimeElementDisplayLst.Add(ElementLayerDefine.miVERealLocation);
                _RealTimeElementDisplayLst.Add(ElementLayerDefine.miVETraffic);

                _MonitorAlarmElementDisplayLst.Add(ElementLayerDefine.miVERealLocation);
                _MonitorAlarmElementDisplayLst.Add(ElementLayerDefine.miVEAlarmHappenLocation);
                _MonitorAlarmElementDisplayLst.Add(ElementLayerDefine.miVETraffic);

                _MonitorAlertElementDisplayLst.Add(ElementLayerDefine.miVERealLocation);
                _MonitorAlertElementDisplayLst.Add(ElementLayerDefine.miVEAlertHappenLocation);
                _MonitorAlertElementDisplayLst.Add(ElementLayerDefine.miVETraffic);

                _HistoryAlarmElementDisplayLst.Add(ElementLayerDefine.miVEAlarmHappenLocation);
                _HistoryAlertElementDisplayLst.Add(ElementLayerDefine.miVEAlertHappenLocation);

                _TrafficElementDisplayLst.Add(ElementLayerDefine.miVETraffic);

                //Initial xaml, the default display monitor under layers
                _currentDisplay = GisDisplayControlType.miMonitor_RealTime;
                _currentVisibleLst = _RealTimeElementDisplayLst;
                //Network Analysis
                AddStopsCommand = new ActionCommand<object>(obj => _AddStops(), obj => { return true; });
                AddBarriersCommand = new ActionCommand<object>(obj => _AddBarriers(), obj => { return true; });
                SolvedCommand = new ActionCommand<object>(obj => _Solved(), obj => { return true; });
                LocationCommand = new ActionCommand<object>(obj => Location(obj, null));

                //Edit button bar geographic features
                SaveEditGeometryCommand = new ActionCommand<object>(obj => _SaveEditGeometry());
                CancelEditGeometryCommand = new ActionCommand<object>(obj => _CancelEditGeometry());

                RedoEditGeometryCommand = new ActionCommand<object>(obj => _RedoEditGeometry(), obj => _RedoEditEnabled());
                UndoEditGeometryCommand = new ActionCommand<object>(obj => _UndoEditGeometry(), obj => _UndoEditEnabled());

                // RefreshTheme1Command = new ActionCommand<object>(obj => _RefreshTheme1());
                ClearMapCommand = new ActionCommand<object>(obj => ClearMap(obj));
                QuerySelectCommand = new ActionCommand<string>(_QueryCommandFunc);
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GisViewModel", ee);
            }
        }

        private void Location(object sender, RoutedEventArgs e)
        {
            LocationWindow location = new LocationWindow();
            location.Closed += location_Closed;
            location.Show();
        }

        void location_Closed(object sender, EventArgs e)
        {
            LocationWindow window = sender as LocationWindow;
            if (window.DialogResult == true)
            {
                //MapPoint mapPoint =
                //            ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(new MapPoint(Convert.ToDouble(window.Longitude), Convert.ToDouble(window.Latitude)));
                //3857地图
                MapPoint mapPoint =
                           Gsafety.Common.Transform.GeographicToWebMercator(new MapPoint(Convert.ToDouble(window.Longitude, System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(window.Latitude, System.Globalization.CultureInfo.InvariantCulture)));
                //MapPoint mapPoint = new MapPoint(Convert.ToDouble(window.Longitude, System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(window.Latitude, System.Globalization.CultureInfo.InvariantCulture));
                Graphic anchor = new Graphic()
                {
                    Geometry = mapPoint,
                    Symbol = GisView.locationSymbol //layoutRoot.Resources["AnchorPointSymbol"]
                };
                anchor.Attributes.Add("PointNumber", GisView.anchorPointGraphicsLayer.Graphics.Count + 1);
                anchor.Geometry = mapPoint;
                GisView.anchorPointGraphicsLayer.Graphics.Add(anchor);


                string strLocal = ApplicationContext.Instance.ServerConfig.AutoLocateResolution;
                strLocal = strLocal.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                if (MyMap.Resolution > Convert.ToDouble(strLocal))
                {
                    CenterAndZoom(Convert.ToDouble(strLocal), mapPoint.X, mapPoint.Y);
                }
                else
                {
                    mapPoint.SpatialReference = _MyMap.SpatialReference;
                    MyMap.PanTo(mapPoint);
                }
            }
        }

        private void ClearMap(object obj)
        {
            //MyDrawBorder.Visibility = Visibility.Collapsed;
            ////GraphicsLayer layer = MyMap.Layers["MeasureGraphicsLayer"] as GraphicsLayer;
            ////layer.Graphics.Clear();
            //if (MyClearBorder.Visibility == Visibility.Visible)
            //    MyClearBorder.Visibility = Visibility.Collapsed;
            //else
            //    MyClearBorder.Visibility = Visibility.Visible;
            try
            {

                MonitorList.VechileRealLocationGraphics.Clear();
                GraphicsLayer layer = MyMap.Layers["MeasureGraphicsLayer"] as GraphicsLayer;
                if (layer != null)
                    layer.Graphics.Clear();

                GraphicsLayer layer1 = MyMap.Layers["MyDrawGraphicsLayer"] as GraphicsLayer;
                if (layer1 != null)
                    layer1.Graphics.Clear();
                GraphicsLayer layer2 = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                if (layer2 != null)
                {
                    layer2.Graphics.Clear();
                }
                GraphicsLayer layer3 = MyMap.Layers[ConstDefine.MyDrawQueryGraphicsLayer] as GraphicsLayer;
                if (layer3 != null)
                {
                    layer3.Graphics.Clear();
                    ApplicationContext.Instance.CurrentDrawArgs = null;
                }
                GraphicsLayer pTheMapLayer = MyMap.Layers["SnapshotLayer"] as GraphicsLayer;
                if (pTheMapLayer != null)
                    pTheMapLayer.Graphics.Clear();

                GraphicsLayer pTheMapLayer3 = MyMap.Layers["FacilityThreeMinThemeGraphicLayer"] as GraphicsLayer;
                GraphicsLayer pTheMapLayer5 = MyMap.Layers["FacilityFiveMinThemeGraphicLayer"] as GraphicsLayer;
                GraphicsLayer pTheMapLayer10 = MyMap.Layers["FacilityTenMinThemeGraphicLayer"] as GraphicsLayer;
                if (pTheMapLayer3 != null)
                    pTheMapLayer3.Graphics.Clear();
                if (pTheMapLayer5 != null)
                    pTheMapLayer5.Graphics.Clear();
                if (pTheMapLayer10 != null)
                    pTheMapLayer10.Graphics.Clear();


                GraphicsLayer pTaxiMapLayer = MyMap.Layers["TaxiThemeGraphicLayer"] as GraphicsLayer;
                if (pTaxiMapLayer != null)
                    pTaxiMapLayer.Graphics.Clear();


                GraphicsLayer pDistGraphicLayer = MyMap.Layers[ConstDefine.MarkDistGraphicsLayer] as GraphicsLayer;
                if (pDistGraphicLayer != null)
                    pDistGraphicLayer.Graphics.Clear();

                GraphicsLayer pTempTrafficGraphicLayer = MyMap.Layers[ConstDefine.TrafficTempGraphicsLayer] as GraphicsLayer;
                if (pTempTrafficGraphicLayer != null)
                    pTempTrafficGraphicLayer.Graphics.Clear();

                GisManagement.ViewModels.MonitorList.GpsHisDataSingleVechileElements.Clear();

                GraphicsLayer AnchorPointGraphicsLayer = MyMap.Layers["AnchorPointGraphicsLayer"] as GraphicsLayer;
                AnchorPointGraphicsLayer.Graphics.Clear();

                GisManagement.ViewModels.MonitorList.AlertHappenLocationElements.Clear();
                GisManagement.ViewModels.MonitorList.AlarmHappenLocationElements.Clear();
                GisManagement.ViewModels.MonitorList.FenceGraphicHelp.Clear();

                EventAggregator.Publish<ClearMapIsExecuted>(new ClearMapIsExecuted());
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
