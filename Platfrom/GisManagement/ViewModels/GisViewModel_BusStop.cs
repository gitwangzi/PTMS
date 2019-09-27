using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Tasks;
using GisManagement.Models;
using Gsafety.Common.CommMessage;
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
using System.Collections.Generic;
using Gsafety.Common.Controls;

namespace GisManagement.ViewModels
{
    public partial class GisViewModel
    {
        #region Add stop
        /// <summary>
        /// draw stop tool
        /// </summary>
        private Draw _myDrawBusPoint = null;

        /// <summary>
        /// save stop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _myDrawBusPoint_DrawComplete(object sender, DrawEventArgs e)
        {
            //try
            //{
            //    //
            //    _myDrawBusPoint.IsEnabled = false;
            //    //
            //    if (MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("GIS_CheckAddStopInfo"),
            //        ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            //    {
            //        try
            //        {
            //            FeatureLayer pFeaLayerStop = MyMap.Layers[ConstDefine.TrafficFeatureStopLayerName] as FeatureLayer;
            //            if (pFeaLayerStop != null)
            //            {
            //                EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = true });
            //                pFeaLayerStop.AutoSave = true;
            //                ESRI.ArcGIS.Client.Graphic gPhic = new Graphic();
            //                gPhic.Geometry = e.Geometry;
            //                gPhic.Attributes["STOP_NAME"] = _pulishDrawStopArgs.StopModel.Stop_Name;
            //                pFeaLayerStop.Graphics.Add(gPhic);
            //               // MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("GIS_SaveSucceed"));

            //                //
            //                //List<ElectricFence> listResult = GetFenceListFromMapService("1=1");
            //                //EventAggregator.Publish<Gsafety.Ant.Traffic.ViewModels.TrafficMenuVm.RefreshFenceListArgs>(new Gsafety.Ant.Traffic.ViewModels.TrafficMenuVm.RefreshFenceListArgs() { listFence = listResult });
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
            //            ApplicationContext.Instance.Logger.LogException("GisViewModel", ex);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
            //    ApplicationContext.Instance.Logger.LogException("GisViewModel", ex);
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        private DrawBusStopArgs _pulishDrawStopArgs = null;
        #endregion
        #region Edit stop Attributes
        /// <summary>
        /// Edit stop Attributes
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(UpdateStopArgs publishedEvent)
        {

        }
        #endregion
        #region Delete stop
        /// <summary>
        /// Delete stop
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(DeleteStopArgs publishedEvent)
        {
            try
            {
                if (publishedEvent == null)
                    return;

                FeatureLayer pFeaLayerStop = MyMap.Layers[ConstDefine.TrafficFeatureStopLayerName] as FeatureLayer;
                if (pFeaLayerStop != null)
                {
                    pFeaLayerStop.AutoSave = true;

                    if (pFeaLayerStop.Graphics.Count == 1)
                    {
                        if (publishedEvent != null)
                        {
                            MonitorList.FenceGraphicHelp.RemoveGraphicByParentID(publishedEvent.OBJECTID.ToString(), TrafficeGraphictype.BusStop);
                            MonitorList.FenceGraphicHelp.RemoveGraphicByParentID(publishedEvent.OBJECTID.ToString(), TrafficeGraphictype.StopTextInfo);
                        }
                        EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = true });

                        pFeaLayerStop.Graphics.RemoveAt(0);
                        //pFeaLayerFence.Update();
                        pFeaLayerStop.Refresh();
                    }
                }
            }
            catch (Exception ee)
            {
                EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
                ApplicationContext.Instance.Logger.LogException("DeleteStopArgs", ee);
            }
        }
        #endregion
        #region Show site feature objects
        /// <summary>
        /// Handle show stop info
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(ShowBusStopInfoArgs publishedEvent)
        {
        }
        /// <summary>
        /// update faild
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pFeaLayerStop_UpdateFailed(object sender, TaskFailedEventArgs e)
        {
            EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
            ApplicationContext.Instance.Logger.LogException("pFeaLayerStop_UpdateFailed", e.Error);
        }
        /// <summary>
        /// update stop completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pFeaLayerStop_UpdateCompleted(object sender, EventArgs e)
        {
            try
            {
                FeatureLayer pFeaLayerStop = MyMap.Layers[ConstDefine.TrafficFeatureStopLayerName] as FeatureLayer;
                if (pFeaLayerStop != null)
                {
                    pFeaLayerStop.ClearSelection();
                    if (pFeaLayerStop.Graphics.Count == 1)
                    {
                        pFeaLayerStop.ClearSelection();
                        // pFeaLayerStop.Graphics[0].Selected = true;
                        pFeaLayerStop.Refresh();
                        ESRI.ArcGIS.Client.Geometry.MapPoint pPt = pFeaLayerStop.Graphics[0].Geometry as ESRI.ArcGIS.Client.Geometry.MapPoint;
                        string strLocal = ApplicationContext.Instance.ServerConfig.AutoLocateResolution;
                        strLocal = strLocal.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                        //string CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                        //if (CurrentUICulture.ToLower().Trim() == "es-es")
                        //{
                        //    strLocal = strLocal.Replace('.', ',');
                        //}
                        if (MyMap.Resolution > Convert.ToDouble(strLocal))
                        {
                            //MyMap.ZoomToResolution(ConstDefine.AutoLocateResolution, pt);
                            CenterAndZoom(Convert.ToDouble(strLocal), pPt);
                        }
                        else
                        {
                            MyMap.PanTo(pPt);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("pFeaLayerStop_UpdateCompleted", ex);
            }
        }
        #endregion
        /// <summary>
        /// query faild
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queryStopTask_Failed(object sender, TaskFailedEventArgs e)
        {
            ApplicationContext.Instance.Logger.LogException("queryStopTask_Failed", e.Error);
            return;
        }
        /// <summary>
        /// query complete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queryStopTask_ExecuteCompleted(object sender, QueryEventArgs e)
        {
        }


        /// <summary>
        /// Layers save failed:stop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pFeaLayerStop_SaveEditsFailed(object sender, TaskFailedEventArgs e)
        {
            EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
            ApplicationContext.Instance.Logger.LogException("pFeaLayerStop_SaveEditsFailed", e.Error);
        }


        /// <summary>
        /// Layers site after data change notification interface refresh (add, edit and delete)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pFeaLayerStop_EndSaveEdits(object sender, EndEditEventArgs e)
        {
            try
            {

                EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
                if (e.Success == true)
                {
                    FeatureLayer pFeaLayerStop = MyMap.Layers["AntAppFeatureMapLayer_Stop"] as FeatureLayer;
                    if (pFeaLayerStop != null)
                    {
                        if (pFeaLayerStop.Graphics.Count == 1 && pFeaLayerStop.Graphics[0].Attributes["OBJECTID"] != null)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
                ApplicationContext.Instance.Logger.LogException("pFeaLayerStop_EndSaveEdits", ex);
            }
        }

        #region Transportation thematic maps
        /// <summary>
        /// Query  site failed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queryStopsTask_Failed(object sender, TaskFailedEventArgs e)
        {
            EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
            ApplicationContext.Instance.Logger.LogException("queryStopsTask_Failed", e.Error);
        }
        /// <summary>
        /// Queries generated three layers respectively after the bus station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queryStopsTask_ExecuteCompleted(object sender, QueryEventArgs e)
        {
            try
            {
                EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
                if (e.FeatureSet == null || e.FeatureSet.Features.Count == 0)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_NoBusStop"), MessageDialogButton.Ok);
                    return;
                }
                if (e.FeatureSet != null)
                {
                    double dSpeed = Convert.ToDouble(ApplicationContext.Instance.ServerConfig.FacilitySpeed);
                    //3
                    GraphicsLayer pTheMapLayer3 = MyMap.Layers["FacilityThreeMinThemeGraphicLayer"] as GraphicsLayer;
                    if (pTheMapLayer3 != null)
                    {
                        pTheMapLayer3.Graphics.Clear();
                        foreach (Graphic g in e.FeatureSet.Features)
                        {
                            if (g.Geometry != null)
                            {
                                ESRI.ArcGIS.Client.Geometry.MapPoint pt = g.Geometry as MapPoint;
                                Graphic gresult = GenerateCircleGraphic(pt, dSpeed * 3, Colors.Green);

                                if (pTheMapLayer3 != null)
                                {
                                    pTheMapLayer3.Graphics.Add(gresult);
                                }
                            }
                        }
                    }

                    //5
                    GraphicsLayer pTheMapLayer5 = MyMap.Layers["FacilityFiveMinThemeGraphicLayer"] as GraphicsLayer;
                    if (pTheMapLayer5 != null)
                    {
                        pTheMapLayer5.Graphics.Clear();
                        foreach (Graphic g in e.FeatureSet.Features)
                        {
                            if (g.Geometry != null)
                            {
                                ESRI.ArcGIS.Client.Geometry.MapPoint pt = g.Geometry as MapPoint;
                                Graphic gresult = GenerateCircleGraphic(pt, dSpeed * 5, Colors.Yellow);

                                if (pTheMapLayer5 != null)
                                {
                                    pTheMapLayer5.Graphics.Add(gresult);
                                }
                            }
                        }
                    }
                    //10
                    GraphicsLayer pTheMapLayer10 = MyMap.Layers["FacilityTenMinThemeGraphicLayer"] as GraphicsLayer;
                    if (pTheMapLayer10 != null)
                    {
                        pTheMapLayer10.Graphics.Clear();
                        foreach (Graphic g in e.FeatureSet.Features)
                        {
                            if (g.Geometry != null)
                            {
                                ESRI.ArcGIS.Client.Geometry.MapPoint pt = g.Geometry as MapPoint;
                                Graphic gresult = GenerateCircleGraphic(pt, dSpeed * 10, Colors.Red);

                                if (pTheMapLayer10 != null)
                                {
                                    pTheMapLayer10.Graphics.Add(gresult);
                                }
                            }
                        }
                    }

                    //Layer dynamicMapLayer = MyMap.Layers["DynamicMapLayer"] as DynamicMapServiceLayer;
                    //if (dynamicMapLayer != null)
                    //    MyMap.Extent = dynamicMapLayer.FullExtent;
                }
            }
            catch (Exception ex)
            {
                EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
                ApplicationContext.Instance.Logger.LogException("queryStopsTask_ExecuteCompleted", ex);
            }

            EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
        }
        /// <summary>
        /// Circle
        /// </summary>
        /// <param name="pCeneterPt"></param>
        /// <param name="dRadius"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public Graphic GenerateCircleGraphic(ESRI.ArcGIS.Client.Geometry.MapPoint pCeneterPt, double dRadius, Color color)
        {
            Graphic result = new Graphic();
            List<MapPoint> listPts = new List<MapPoint>();
            for (double i = 0; i <= 360; i++)
            {
                MapPoint pPtNew = new MapPoint();
                pPtNew.X = pCeneterPt.X - Math.Cos(Math.PI * i / 180) * dRadius;
                pPtNew.Y = pCeneterPt.Y - Math.Sin(Math.PI * i / 180) * dRadius;
                listPts.Add(pPtNew);
            }
            ESRI.ArcGIS.Client.Geometry.PointCollection ptCollect = new ESRI.ArcGIS.Client.Geometry.PointCollection(listPts);
            ESRI.ArcGIS.Client.Geometry.Polygon pPolygon = new ESRI.ArcGIS.Client.Geometry.Polygon();
            pPolygon.Rings.Add(ptCollect);
            result.Geometry = pPolygon;
            result.Symbol = new SimpleFillSymbol()
            {
                //BorderBrush = new SolidColorBrush(color) { },
                //BorderThickness = 2,
                Fill = new SolidColorBrush(color) { Opacity = 0.3 }
            };
            return result;
        }

        /// <summary>
        /// Draw a square
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        private ESRI.ArcGIS.Client.Geometry.Polygon GetSquareByLeftButtomPoint(double dx, double dy)
        {
            MapPoint basepoint = new MapPoint();
            if (dx > 0)
                basepoint.X = (int)dx / 100 + (dx % 100) / 60;
            else
                basepoint.X = -((int)((-dx) / 100) + ((-dx) % 100) / 60);

            if (dy > 0)
                basepoint.Y = (int)(dy / 100) + (dy % 100) / 60;
            else
                basepoint.Y = -((int)((-dy) / 100) + ((-dy) % 100) / 60);
            //The upper left corner
            MapPoint pt1 = new MapPoint();
            pt1.X = basepoint.X;
            pt1.Y = basepoint.Y + 0.01666667;

            //The upper right corner
            MapPoint pt2 = new MapPoint();
            pt2.X = basepoint.X + 0.01666667;
            pt2.Y = basepoint.Y + 0.01666667;
            //The lower right corner
            MapPoint pt3 = new MapPoint();
            pt3.X = basepoint.X + 0.01666667;
            pt3.Y = basepoint.Y;
            //The lower left corner
            MapPoint pt4 = new MapPoint();
            pt4.X = basepoint.X;
            pt4.Y = basepoint.Y;

            ESRI.ArcGIS.Client.Geometry.PointCollection ptCollect = new ESRI.ArcGIS.Client.Geometry.PointCollection();
            //ptCollect.Add(ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(pt1));
            //ptCollect.Add(ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(pt2));
            //ptCollect.Add(ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(pt3));
            //ptCollect.Add(ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(pt4));

            ptCollect.Add(Gsafety.Common.Transform.GeographicToWebMercator(pt1));
            ptCollect.Add(Gsafety.Common.Transform.GeographicToWebMercator(pt2));
            ptCollect.Add(Gsafety.Common.Transform.GeographicToWebMercator(pt3));
            ptCollect.Add(Gsafety.Common.Transform.GeographicToWebMercator(pt4));

            ESRI.ArcGIS.Client.Geometry.Polygon polygon = new ESRI.ArcGIS.Client.Geometry.Polygon();
            polygon.Rings.Add(ptCollect);
            polygon.SpatialReference = MyMap.SpatialReference;

            return polygon;
        }

        /// <summary>
        /// According to a level of color values
        /// </summary>
        /// <param name="nNum"></param>
        /// <param name="nMax"></param>
        /// <returns></returns>
        private Color GetTaxtTheMapColorByNum(int nNum, int nMax)
        {
            if (nNum == 0 || nMax == 0)
                return Colors.Green;
            int nLevel = (int)(nMax / 3);
            if (nLevel == 0)
                return Colors.Green;
            int nLevelEx = (int)(nNum / nLevel);
            if (nLevelEx == 0)
                return Colors.Green;
            if (nLevelEx == 1)
                return Colors.Yellow;
            if (nLevelEx >= 2)
                return Colors.Red;
            return Colors.White;
        }
        #endregion
    }
}
