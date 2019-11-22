using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Tasks;
using GisManagement.Models;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.Bases.Enums;
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
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.ServiceReference.MessageService;
using ESRI.ArcGIS.Client.Symbols;
using System.Linq;
using Gsafety.Common.Controls;
using System.Reflection;

namespace GisManagement.ViewModels
{
    public partial class GisViewModel
    {
        private bool _bEditRoute = false;
        #region Add route

        /// <summary>
        /// tool of draw route
        /// </summary>
        public ESRI.ArcGIS.Client.Draw _DrawRout = null;

        /// <summary>
        /// Add a line drawn route messages
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(DrawRoutEventArgs publishedEvent)
        {
            _publishedDrawRoutEvent = publishedEvent;
            try
            {
                //
                if (_DrawRout == null)
                {
                    _DrawRout = new ESRI.ArcGIS.Client.Draw(this.MyMap);
                    //bingding map
                    _DrawRout.Map = this.MyMap;
                    //draw line
                    _DrawRout.DrawMode = DrawMode.Polyline;
                    //symbol
                    _DrawRout.LineSymbol = new SimpleLineSymbol()
                    {
                        Color = new SolidColorBrush(Colors.Blue),
                        Width = 2
                    };
                    //complete event
                    _DrawRout.DrawComplete += _DrawRout_DrawComplete;
                }
                //enable tool
                _DrawRout.IsEnabled = true;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("GisViewModel", ex);
            }
        }
        /// <summary>
        /// draw route complete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _DrawRout_DrawComplete(object sender, DrawEventArgs e)
        {
            // enable tool 
            _DrawRout.IsEnabled = false;

            var window = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_CheckAddRouteInfo"), MessageDialogButton.OkAndCancel);
            window.Closed += delegate(object sender2, EventArgs e2)
            {
                ChildWindow childwindow = sender2 as ChildWindow;
                if (childwindow.DialogResult == true)
                {
                    try
                    {
                        ESRI.ArcGIS.Client.Geometry.Polyline polyline = (e.Geometry as ESRI.ArcGIS.Client.Geometry.Polyline);

                        if (polyline == null || polyline.Paths.Count != 1)
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_FenceNotPolygon"), MessageDialogButton.Ok);
                            return;
                        }
                        //Determine whether more than 35 points        
                        ESRI.ArcGIS.Client.Geometry.PointCollection drawpoints = polyline.Paths[0];
                        if (drawpoints.Count > 35)
                        {
                            var result = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_FenceLimitPoints"), MessageDialogButton.OkAndCancel);
                            result.Closed += delegate(object send, EventArgs er)
                            {
                                ChildWindow child = send as ChildWindow;
                                if (child.DialogResult == true)
                                {
                                    StartEditDrawFence(e.Geometry);
                                    return;
                                }
                                else
                                {
                                    return;
                                }
                            };

                        }

                        SaveRouteEx(e.Geometry);
                    }
                    catch (Exception ex)
                    {
                        EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
                        ApplicationContext.Instance.Logger.LogException("GisViewModel", ex);
                    }
                }
                else
                {
                    EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
                }
            };


        }

        /// <summary>
        /// 保存路线
        /// </summary>
        /// <param name="geometry"></param>
        private void SaveRouteEx(ESRI.ArcGIS.Client.Geometry.Geometry geometry)
        {
            GraphicsLayer pLayerRoute = MyMap.Layers[ConstDefine.TrafficFeatureRouteLayerName] as GraphicsLayer;
            if (pLayerRoute != null)
            {
                _publishedDrawRoutEvent.Route.Pts = FormatGeometryPoints((geometry as ESRI.ArcGIS.Client.Geometry.Polyline).Paths[0]);
                _publishedDrawRoutEvent.Route.PointCount = (geometry as ESRI.ArcGIS.Client.Geometry.Polyline).Paths[0].Count;
                _publishedDrawRoutEvent.Route.RouteSegmentProperty = UpdateRoutePropertyWithPts(_publishedDrawRoutEvent.Route.RouteSegmentProperty, _publishedDrawRoutEvent.Route.Pts);

                TrafficRoute newe = _publishedDrawRoutEvent.Route.Clone(_publishedDrawRoutEvent.Route);
                newe.CreateTime.ToUniversalTime();

                TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
                trafficServiceClient.InsertTrafficRouteCompleted += trafficServiceClient_InsertTrafficRouteCompleted;
                trafficServiceClient.InsertTrafficRouteAsync(newe);

                ESRI.ArcGIS.Client.Graphic gPhic = new Graphic();
                gPhic.Geometry = geometry;

                gPhic.Symbol = new SimpleLineSymbol()
                {
                    Color = new SolidColorBrush(Colors.Blue),
                    Width = 2
                };

                pLayerRoute.Graphics.Add(gPhic);


                GraphicsLayer pGraphicLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                pGraphicLayer.Graphics.Clear();
            }
        }

        /// <summary>
        /// 依据pts中第一点的正负来决定regionproperty中的经纬度参数
        /// </summary>
        /// <param name="fsproperty"></param>
        /// <param name="pts"></param>
        /// <returns></returns>
        private string UpdateRoutePropertyWithPts(string fsproperty, string pts)
        {
            if (pts == "") return fsproperty;
            string[] coordsep = { ";" };
            string[] xysep = { "," };

            string[] lst = pts.Split(coordsep, StringSplitOptions.RemoveEmptyEntries);
            string coord = lst[0];
            string[] xy = coord.Split(xysep, StringSplitOptions.RemoveEmptyEntries);
            //xy[0],xy[1];
            if (xy[0].IndexOf("-") > -1) //经度为负
            {
                fsproperty = AddOrRemoveRouteSegmentProperty(fsproperty, false, Route_RouteSegmentProperty.East_Lon);
                fsproperty = AddOrRemoveRouteSegmentProperty(fsproperty, true, Route_RouteSegmentProperty.West_Lon);
            }
            else
            {
                fsproperty = AddOrRemoveRouteSegmentProperty(fsproperty, true, Route_RouteSegmentProperty.East_Lon);
                fsproperty = AddOrRemoveRouteSegmentProperty(fsproperty, false, Route_RouteSegmentProperty.West_Lon);
            }
            if (xy[1].IndexOf("-") > -1) //纬度为负
            {
                fsproperty = AddOrRemoveRouteSegmentProperty(fsproperty, false, Route_RouteSegmentProperty.North_Lat);
                fsproperty = AddOrRemoveRouteSegmentProperty(fsproperty, true, Route_RouteSegmentProperty.South_Lat);
            }
            else
            {
                fsproperty = AddOrRemoveRouteSegmentProperty(fsproperty, true, Route_RouteSegmentProperty.North_Lat);
                fsproperty = AddOrRemoveRouteSegmentProperty(fsproperty, false, Route_RouteSegmentProperty.South_Lat);
            }
            return fsproperty;
        }


        private string AddOrRemoveRouteSegmentProperty(string rsproperty, bool isAdd, Route_RouteSegmentProperty ftype)
        {
            List<string> lst = new List<string>();
            if (rsproperty != null)
            {
                lst = rsproperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
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
            string newfsProperty = "";
            foreach (string str in lst)
            {
                if (newfsProperty == "")
                {
                    newfsProperty = str;
                }
                else
                {
                    newfsProperty = newfsProperty + "," + str;
                }
            }
            rsproperty = newfsProperty;
            return rsproperty;
        }

        void trafficServiceClient_InsertTrafficRouteCompleted(object sender, InsertTrafficRouteCompletedEventArgs e)
        {
            if (!((e.Error == null) && (e.Result.Result == true)))
            {//添加失败
                MessageBoxHelper.ShowDialog(e.Error.Message);
                ApplicationContext.Instance.Logger.LogException("GisViewModel", e.Error);
            }
            else
            {
                EventAggregator.Publish<AddRouteCompleteArgs>(new AddRouteCompleteArgs());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private DrawRoutEventArgs _publishedDrawRoutEvent = null;
        #endregion
        #region Delete Route
        /// <summary>
        /// Delete current Route
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(DeleteRouteArgs publishedEvent)
        {
            try
            {
                GraphicsLayer pLayerRoute = MyMap.Layers[ConstDefine.TrafficFeatureRouteLayerName] as GraphicsLayer;
                if (pLayerRoute != null) pLayerRoute.Graphics.Clear();

                _editGeometrypublishedEvent = null;
                EditGeometryVisble = Visibility.Collapsed;


                TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
                trafficServiceClient.DeleteTrafficRouteByIDCompleted += trafficServiceClient_DeleteTrafficRouteByIDCompleted;
                trafficServiceClient.DeleteTrafficRouteByIDAsync(publishedEvent.ID);

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
                EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
                ApplicationContext.Instance.Logger.LogException("DeleteRouteArgs", ee);
            }
        }

        void trafficServiceClient_DeleteTrafficRouteByIDCompleted(object sender, DeleteTrafficRouteByIDCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ApplicationContext.Instance.Logger.LogException("DeleteFenceArgs_Error", e.Error);
            }
        }
        #endregion

        #region Show Line Geometry object, select a line interface, map display of the line, hide other lines
        /// <summary>
        /// Layers route update fails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pFeaLayerRout_UpdateFailed(object sender, TaskFailedEventArgs e)
        {
            EventAggregator.Publish<SetTrafficePageBusyArgs>(new SetTrafficePageBusyArgs() { bBusy = false });
            ApplicationContext.Instance.Logger.LogException("pFeaLayerRout_UpdateFailed", e.Error);
        }

        #endregion
        #region Query route

        /// <summary>
        /// Empty traffic management GIS map
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(ClearTrafficMaps publishedEvent)
        {
            try
            {
                if (_pEditGeometry != null)
                    _pEditGeometry.CancelEdit();
                GraphicsLayer pFeaLayerRout = MyMap.Layers[ConstDefine.TrafficFeatureRouteLayerName] as GraphicsLayer;
                GraphicsLayer pFeaLayerFence = MyMap.Layers[ConstDefine.TrafficFeatureFenceLayerName] as GraphicsLayer;
                GraphicsLayer pFeaLayerStop = MyMap.Layers[ConstDefine.TrafficFeatureStopLayerName] as GraphicsLayer;
                GraphicsLayer pTheMapLayer3 = MyMap.Layers["FacilityThreeMinThemeGraphicLayer"] as GraphicsLayer;
                GraphicsLayer pTheMapLayer5 = MyMap.Layers["FacilityFiveMinThemeGraphicLayer"] as GraphicsLayer;
                GraphicsLayer pTheMapLayer10 = MyMap.Layers["FacilityTenMinThemeGraphicLayer"] as GraphicsLayer;
                //
                if (publishedEvent.nType == TrafficFeature.Traffic_PolygonFence)
                {
                    if (pFeaLayerRout != null)
                    {
                        pFeaLayerRout.Visible = false;
                    }
                    if (pFeaLayerFence != null)
                    {
                        pFeaLayerFence.Visible = true;
                    }
                    if (pFeaLayerStop != null)
                    {
                        pFeaLayerStop.Visible = false;
                    }
                    if (pTheMapLayer3 != null)
                    {
                        pTheMapLayer3.Visible = false;
                    }
                    if (pTheMapLayer5 != null)
                    {
                        pTheMapLayer5.Visible = false;
                    }
                    if (pTheMapLayer10 != null)
                    {
                        pTheMapLayer10.Visible = false;
                    }
                }
                else if (publishedEvent.nType == TrafficFeature.Traffic_Route)
                {
                    if (pFeaLayerRout != null)
                    {
                        pFeaLayerRout.Visible = true;
                    }
                    if (pFeaLayerFence != null)
                    {
                        pFeaLayerFence.Visible = false;
                    }
                    if (pFeaLayerStop != null)
                    {
                        pFeaLayerStop.Visible = false;
                    }
                    if (pTheMapLayer3 != null)
                    {
                        pTheMapLayer3.Visible = false;
                    }
                    if (pTheMapLayer5 != null)
                    {
                        pTheMapLayer5.Visible = false;
                    }
                    if (pTheMapLayer10 != null)
                    {
                        pTheMapLayer10.Visible = false;
                    }
                }
                else if (publishedEvent.nType == TrafficFeature.Traffic_ThemeMap)
                {
                    if (pFeaLayerRout != null)
                    {
                        pFeaLayerRout.Visible = false;
                    }
                    if (pFeaLayerFence != null)
                    {
                        pFeaLayerFence.Visible = false;
                    }
                    if (pFeaLayerStop != null)
                    {
                        pFeaLayerStop.Visible = false;
                    }
                    if (pTheMapLayer3 != null)
                    {
                        pTheMapLayer3.Visible = true;
                    }
                    if (pTheMapLayer5 != null)
                    {
                        pTheMapLayer5.Visible = true;
                    }
                    if (pTheMapLayer10 != null)
                    {
                        pTheMapLayer10.Visible = true;
                    }
                }
                else if (publishedEvent.nType == TrafficFeature.Traffic_SpeedLimit)
                {
                    if (pFeaLayerRout != null)
                    {
                        pFeaLayerRout.Visible = false;
                    }
                    if (pFeaLayerFence != null)
                    {
                        pFeaLayerFence.Visible = false;
                    }
                    if (pFeaLayerStop != null)
                    {
                        pFeaLayerStop.Visible = false;
                    }
                    if (pTheMapLayer3 != null)
                    {
                        pTheMapLayer3.Visible = true;
                    }
                    if (pTheMapLayer5 != null)
                    {
                        pTheMapLayer5.Visible = true;
                    }
                    if (pTheMapLayer10 != null)
                    {
                        pTheMapLayer10.Visible = true;
                    }
                }
            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("ClearTrafficMaps", ee);
            }
        }
        #endregion


        /// <summary>
        /// Layers site after data change notification interface refresh (add, edit and delete)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pFeaLayerRoute_EndSaveEdits(TrafficRoute csUpdateCompleteArgs)
        {
            try
            {
                EventAggregator.Publish<RefreshRouteSelectStatus>(new RefreshRouteSelectStatus() { });
                //If modified, the shape may be modified, may be is the modification information 
                //EventAggregator.Publish<RefreshTrafficManagerListArgs>(new RefreshTrafficManagerListArgs() { nType = 0 });
                if (_bEditRoute == true && csUpdateCompleteArgs != null)
                {
                    csUpdateCompleteArgs.RouteSegmentProperty = UpdateRoutePropertyWithPts(csUpdateCompleteArgs.RouteSegmentProperty, csUpdateCompleteArgs.Pts);

                    TrafficRoute newe = csUpdateCompleteArgs.Clone(csUpdateCompleteArgs);
                    newe.CreateTime = newe.CreateTime.ToUniversalTime();

                    TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
                    trafficServiceClient.UpdateTrafficRouteCompleted += trafficServiceClient_UpdateTrafficRouteCompleted;
                    trafficServiceClient.UpdateTrafficRouteAsync(newe);
                    _bEditRoute = false;

                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("pFeaLayerRout_EndSaveEdits", ex);
            }
        }

        void trafficServiceClient_UpdateTrafficRouteCompleted(object sender, UpdateTrafficRouteCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException("pFeaLayerFence_SaveEditsFailed", e.Error);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Faild"), MessageDialogButton.Ok);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

    }
}
