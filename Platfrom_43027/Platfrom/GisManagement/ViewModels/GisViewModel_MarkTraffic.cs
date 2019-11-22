using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Tasks;
using GisManagement.Models;
using Gsafety.Common.CommMessage;
using Gsafety.Common.CommMessage.Controls;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using System;
using System.Collections.Generic;
using System.Net;
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
        private UpdateTrafficMarkArgs _curUpdateTrafficMarkArgs = null;

        #region Daily monitoring of vehicles shown in the corresponding fence or line

        /// <summary>
        /// Updated daily monitoring travel plans plotted
        /// </summary>
        /// <param name="publishedEvent"></param>
        //public void HandleEvent(UpdateTrafficMarkArgs publishedEvent)
        //{
        //    try
        //    {
        //        _curUpdateTrafficMarkArgs = publishedEvent;

        //    }
        //    catch (Exception ex)
        //    {
        //        ApplicationContext.Instance.Logger.LogException("UpdateTrafficMarkArgs", ex);
        //    }
        //}
        private Dictionary<string, string> _StopScheDuleIDName = new Dictionary<string, string>();
        private string GetStopScheDuleNamebyID(string strID)
        {
            try
            {
                if (_StopScheDuleIDName == null)
                    return "";
                return _StopScheDuleIDName[strID];
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        #endregion #region Daily monitoring of vehicles shown in the corresponding fence or line
        /// <summary>
        /// Mark lines Details  
        /// </summary>
        /// <param name="gRouteGraphic"></param>
        private Graphic MarkRouteTextInfo(Graphic gRouteGraphic, TrafficRoute route)
        {
            try
            {
                MapPoint pt = GetRightButtomPoint(gRouteGraphic.Geometry);
                if (pt != null)
                {
                    string strInfo = route.DisplayName;
                    TextSymbol textSymbol = new TextSymbol()
                    {
                        FontFamily = new System.Windows.Media.FontFamily("Microsoft YaHei"),
                        Foreground = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 117, 20, 99)),
                        FontSize = 15,
                        Text = strInfo
                    };
                    Graphic textInfo = new Graphic();
                    textInfo.Geometry = pt;
                    textInfo.Symbol = textSymbol;
                    return textInfo;
                }
                return null;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("MarkRouteTextInfo", ex);
                return null;
            }
        }

        /// <summary>
        /// Get marked points
        /// </summary>
        /// <param name="geo"></param>
        /// <returns></returns>
        private MapPoint GetRightButtomPoint(ESRI.ArcGIS.Client.Geometry.Geometry geo)
        {
            if (geo == null)
                return null;
            //If the surface is temporarily take the lowermost point, if the same is to take one of latitude
            ESRI.ArcGIS.Client.Geometry.Polygon polygon = geo as ESRI.ArcGIS.Client.Geometry.Polygon;
            if (polygon != null)
            {
                if (polygon.Rings.Count == 0)
                    return null;
                ESRI.ArcGIS.Client.Geometry.PointCollection ptcollect = polygon.Rings[0];
                if (ptcollect == null || ptcollect.Count == 0)
                    return null;
                MapPoint pt = ptcollect[0];
                for (int i = 1; i < ptcollect.Count; i++)
                {
                    if (ptcollect[i].Y < pt.Y)
                    {
                        pt = ptcollect[i];
                    }
                }
                return pt;
            }
            //If the line, and take the right-most
            ESRI.ArcGIS.Client.Geometry.Polyline polyline = geo as ESRI.ArcGIS.Client.Geometry.Polyline;
            if (polyline != null)
            {
                if (polyline.Paths.Count == 0)
                    return null;
                ESRI.ArcGIS.Client.Geometry.PointCollection ptcollect = polyline.Paths[0];
                if (ptcollect == null || ptcollect.Count == 0)
                    return null;
                MapPoint pt = ptcollect[0];
                for (int i = 1; i < ptcollect.Count; i++)
                {
                    if (ptcollect[i].Y < pt.Y)
                    {
                        pt = ptcollect[i];
                    }
                }
                return pt;
            }
            //If this is the point, to return to their
            if (geo is ESRI.ArcGIS.Client.Geometry.MapPoint)
            {
                return geo as MapPoint;
            }
            return null;
        }


        /// <summary>
        /// Information plotted fence
        /// </summary>
        /// <param name="gFenceGraphic"></param>
        private Graphic MarkFenceTextInfo(Graphic gFenceGraphic, TrafficFence fence)
        {
            try
            {
                MapPoint pt = GetRightButtomPoint(gFenceGraphic.Geometry);
                if (pt != null)
                {
                    string strInfo = "";
                    strInfo += fence.DisplayName;
                    strInfo += ":";

                    TextSymbol textSymbol = new TextSymbol()
                    {
                        FontFamily = new System.Windows.Media.FontFamily("Microsoft YaHei"),
                        Foreground = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 117, 20, 99)),
                        FontSize = 15,
                        Text = strInfo
                    };
                    Graphic textInfo = new Graphic();
                    textInfo.Geometry = pt;
                    textInfo.Symbol = textSymbol;
                    return textInfo;

                }
                return null;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("MarkFenceTextInfo", ex);
                return null;
            }
        }
        /// <summary>
        /// Mark monitoring point information
        /// </summary>
        /// <param name="gControlPointGraphic"></param>
        /// <param name="ptMark"></param>
        private Graphic MarkControlPointTextInfo(Graphic gControlPointGraphic, MapPoint ptMark)
        {
            try
            {
                MapPoint pt = ptMark;
                if (pt != null)
                {
                    string strInfo = "";
                    strInfo += gControlPointGraphic.Attributes["NAME"].ToString();
                    strInfo += ":";

                    string strradius = gControlPointGraphic.Attributes["RADIU"].ToString();
                    if (String.IsNullOrEmpty(strradius) == false)
                    {
                        //strInfo += ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_Radiums");
                        //strInfo += "-";
                        strInfo += strradius;
                    }
                    string strTime = gControlPointGraphic.Attributes["TIME_LIMIT"].ToString();
                    if (String.IsNullOrEmpty(strTime) == false)
                    {
                        strInfo += ",";
                        //strInfo += ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Time");
                        //strInfo += "-";
                        strInfo += strTime;
                    }
                    TextSymbol textSymbol = new TextSymbol()
                    {
                        FontFamily = new System.Windows.Media.FontFamily("Microsoft YaHei"),
                        Foreground = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 117, 20, 99)),
                        FontSize = 15,
                        Text = strInfo
                    };
                    Graphic textInfo = new Graphic();
                    textInfo.Geometry = pt;
                    textInfo.Symbol = textSymbol;
                    return textInfo;
                }
                return null;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("MarkFenceTextInfo", ex);
                return null;
            }
        }

        /// <summary>
        /// Graphic surface structure
        /// </summary>
        /// <param name="geo"></param>
        /// <param name="symbolparm"></param>
        /// <returns></returns>
        private Graphic GeneratePolygonGraphic(string pts, SymbolParams symbolparm)
        {
            if (symbolparm == null) return null;
            Graphic g = GetFencePolygon(pts);
            g.Symbol = new SimpleFillSymbol()
            {
                BorderBrush = new SolidColorBrush(symbolparm.FillColorParm) { },
                BorderThickness = 2,
                Fill = new SolidColorBrush(symbolparm.FillColorParm) { Opacity = symbolparm.TransparentParm }
            };
            return g;
        }
        /// <summary>
        /// Dot pattern structure
        /// </summary>
        /// <param name="symbolparm"></param>
        /// <param name="strCenter">Point coordinates: X; Y format</param>
        /// <returns></returns>
        private Graphic GeneratePointGraphic(SymbolParams symbolparm, string strCenter)
        {
            if (symbolparm == null)
                return null;

            if (strCenter != null && strCenter.Trim().Equals("") == false)
            {
                MapPoint pt = new MapPoint();
                string[] strlist = strCenter.Split(';');
                if (strlist != null && strlist.Length == 2)
                {
                    pt.X = Convert.ToDouble(strlist[0].Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator));
                    pt.Y = Convert.ToDouble(strlist[1].Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator));
                    pt = mercator.FromGeographic(pt) as MapPoint;
                    Graphic gPoint = new Graphic();
                    gPoint.Geometry = pt;
                    gPoint.Symbol = new SimpleMarkerSymbol() { Color = new SolidColorBrush(symbolparm.MarkColorParm) { }, Size = symbolparm.MarkSizeParm };
                    return gPoint;
                }
            }
            return null;
        }
        /// <summary>
        /// Tectonic Line Graphics
        /// </summary>
        /// <param name="geo"></param>
        /// <param name="symbolparm"></param>
        /// <returns></returns>
        private Graphic GenerateRouteGraphic(string pts, SymbolParams symbolparm)
        {
            if (symbolparm == null) return null;

            Graphic markgraphic = GetRouteLine(pts);
            markgraphic.Symbol = new SimpleLineSymbol()
            {
                Color = new SolidColorBrush(symbolparm.LineColorParm) { },
                Width = symbolparm.LineWidthParm,
            };
            return markgraphic;
        }


        /// <summary>
        /// Plotted site news (issued by the traffic management,
        /// in line module adds functionality plotted line contains the site)
        /// </summary>
        private MarkStopsByRoutID _MarkStopsByRoutpublishedEvent = null;
        /// <summary>
        /// Plotted site
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(MarkStopsByRoutID publishedEvent)
        {
            _MarkStopsByRoutpublishedEvent = publishedEvent;

            TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
        }
        /// <summary>
        /// Handle plot message
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(MarkTrafficGraphic publishedEvent)
        {
            try
            {
                //fence or controlpoint
                if (publishedEvent.nType == TrafficFeature.Traffic_PolygonFence)
                {
                    if (publishedEvent.bShow == true)
                    {
                        //make a graphic
                        Graphic gPolygon = GeneratePolygonGraphic(publishedEvent.TrafficFence.Pts, publishedEvent.MarkSymbolParm);
                        //add
                        if (gPolygon != null)
                        {
                            MonitorList.FenceGraphicHelp.AddGraphic(gPolygon, publishedEvent.parentId, publishedEvent.childId, TrafficeGraphictype.fence);

                            Graphic textInfo = MarkFenceTextInfo(gPolygon, publishedEvent.TrafficFence);
                            if (textInfo != null)
                                MonitorList.FenceGraphicHelp.AddGraphic(textInfo, publishedEvent.parentId, publishedEvent.childId, TrafficeGraphictype.FenceTextInfo);
                        }

                    }
                    else
                    {
                        MonitorList.FenceGraphicHelp.RemoveGraphicByParentID(publishedEvent.parentId, TrafficeGraphictype.fence);
                        MonitorList.FenceGraphicHelp.RemoveGraphicByParentID(publishedEvent.parentId, TrafficeGraphictype.FenceTextInfo);
                    }
                }
                else if (publishedEvent.nType == TrafficFeature.Traffic_Route)
                {
                    if (publishedEvent.bShow == true)
                    {

                        //make a graphic
                        Graphic gPolyline = GenerateRouteGraphic(publishedEvent.TrafficRoute.Pts, publishedEvent.MarkSymbolParm);
                        //add
                        if (gPolyline != null)
                        {
                            MonitorList.FenceGraphicHelp.AddGraphic(gPolyline, publishedEvent.parentId, publishedEvent.childId, TrafficeGraphictype.Route);

                            Graphic textInfo = MarkRouteTextInfo(gPolyline, publishedEvent.TrafficRoute);
                            if (textInfo != null)
                                MonitorList.FenceGraphicHelp.AddGraphic(textInfo, publishedEvent.parentId, publishedEvent.childId, TrafficeGraphictype.RouteTextInfo);
                        }
                    }
                    else
                    {
                        MonitorList.FenceGraphicHelp.RemoveGraphicByParentID(publishedEvent.parentId, TrafficeGraphictype.Route);
                        MonitorList.FenceGraphicHelp.RemoveGraphicByParentID(publishedEvent.parentId, TrafficeGraphictype.RouteTextInfo);
                    }
                }

            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("MarkTrafficGraphic_Error", ee);
            }
        }
    }
}
