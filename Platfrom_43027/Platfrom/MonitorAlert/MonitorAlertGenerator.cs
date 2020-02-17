/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 10142c42-8186-4e92-8f34-b44f4e56271e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGZG
/////                 Author: TEST(zhangzg)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MonitorAlert
/////    Project Description:    
/////             Class Name: MonitorAlertGenerator
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/8 14:36:48
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/8 14:36:48
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Message.Contract.Data;
using System.Collections;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Traffic.Contract.Data;
using Gsafety.PTMS.Traffic.Repository;
using Gsafety.PTMS.Common.Data;

namespace Gsafety.PTMS.MonitorAlert
{
    /// <summary>
    /// 监控GPS消息，产生相应的其他消息
    /// </summary>
    public class MonitorAlertGenerator
    {
        private Hashtable _GPSLocationHashtable;      
        private MonitorGrid _MonitorGrid;
        private TrafficRepository trafficeServer;
        private MonitorAlertPlan _MonitorAlertPlan;

        public  MonitorGrid MonitorGrid
        {
            get
            {
                return _MonitorGrid;
            }
        }

        public static Hashtable GPSSpeedHashtable;
       
        public MonitorAlertGenerator()
        {
            _GPSLocationHashtable = new Hashtable();
            GPSSpeedHashtable = new Hashtable();
            _MonitorGrid = new MonitorGrid();
            _MonitorAlertPlan = new MonitorAlertPlan();
            trafficeServer = new TrafficRepository();          
        }

        /// <summary>
        ///根据GPS信息来决定生成什么样的消息
        /// </summary>
        /// <param name="antgps"></param>
        public void HandleGPS(GPS gps)
        {
            //如果GPS无效则退出
            try
            {
                LoggerManager.Logger.Info("HandleGPS");
                if (gps.Valid != "A")
                {
                    LoggerManager.Logger.Info("GPS invalid");
                    return;
                }


                //获取当前路线
                Route route = GetRoute(gps);
                bool InRouteOverSpeed = false;
                if (route != null)//进入路线
                {
                    if ((double.Parse(gps.Speed) > route.Route.MaxSpeed) && (route.Route.MaxSpeed != 0))
                    {
                        InRouteOverSpeed = true;
                    }

                }
                //获取当前围栏
                Fence fence = GetFence(gps);
                bool InFenceOverSpeed = false;

                if (fence != null)//进入围栏
                {
                    if ((double.Parse(gps.Speed) > fence.Fence.MaxSpeed) && (fence.Fence.MaxSpeed != 0))
                    {
                        InFenceOverSpeed = true;
                    }

                }
                bool OverSpeed = false;
                if (!InFenceOverSpeed && !InRouteOverSpeed)
                {
                    Gsafety.PTMS.Common.Data.VehicleSpeedLimit limit = GetSpeedLimit(gps);


                    if (limit != null)//找到了当前所在道路
                    {
                        if (double.Parse(gps.Speed) > limit.MaxSpeed)
                        {
                            OverSpeed = true;
                        }
                    }
                }               //更新当前集合
                if (_GPSLocationHashtable.ContainsKey(gps.UID))//看车辆上次的位置信息
                {
                    (_GPSLocationHashtable[gps.UID] as GPSLocation).CurrentGPS = gps;
                    (_GPSLocationHashtable[gps.UID] as GPSLocation).CurrentRoute = route;
                    (_GPSLocationHashtable[gps.UID] as GPSLocation).CurrentFence = fence;

                    if (!(_GPSLocationHashtable[gps.UID] as GPSLocation).CurrentOverSpeed && OverSpeed)
                    {
                        (_GPSLocationHashtable[gps.UID] as GPSLocation).OverSpeedTime = gps.GpsTime.Value;
                    }
                    (_GPSLocationHashtable[gps.UID] as GPSLocation).CurrentOverSpeed = OverSpeed;

                    if (!(_GPSLocationHashtable[gps.UID] as GPSLocation).CurrentInFenceOverSpeed && InFenceOverSpeed)
                    {
                        (_GPSLocationHashtable[gps.UID] as GPSLocation).InFenceOverSpeedTime = gps.GpsTime.Value;
                    }
                    (_GPSLocationHashtable[gps.UID] as GPSLocation).CurrentInFenceOverSpeed = InFenceOverSpeed;

                    if (!(_GPSLocationHashtable[gps.UID] as GPSLocation).CurrentInRouteOverSpeed && InRouteOverSpeed)
                    {
                        (_GPSLocationHashtable[gps.UID] as GPSLocation).InRouteOverSpeedTime = gps.GpsTime.Value;
                    }
                    (_GPSLocationHashtable[gps.UID] as GPSLocation).CurrentInRouteOverSpeed = InRouteOverSpeed;
                }
                else
                {//第一点
                    GPSLocation gpslocation = new GPSLocation(gps, route, fence, OverSpeed, InFenceOverSpeed, InRouteOverSpeed, gps.GpsTime.Value);
                    _GPSLocationHashtable.Add(gps.UID, gpslocation);
                }

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        } 

        /// <summary>
        /// 获得当前车辆所属超速规则
        /// </summary>
        /// <param name="antgps"></param>
        /// <returns></returns>
        private Gsafety.PTMS.Common.Data.VehicleSpeedLimit GetSpeedLimit(GPS gps)
        {

            Gsafety.PTMS.Common.Data.VehicleSpeedLimit limit = null;
            if (_MonitorGrid.GridCellsSpeed.ContainsKey(gps.UID))
            {
                limit = _MonitorGrid.GridCellsSpeed[gps.UID] as VehicleSpeedLimit;
            }


            return limit;
        
        
        }

        /// <summary>
        /// 获得当前位置所在的围栏
        /// </summary>
        /// <param name="antgps"></param>
        /// <returns></returns>
        private Fence GetFence(GPS gps)
        {
            Fence fence = null;
            if (_GPSLocationHashtable.ContainsKey(gps.UID))//看车辆上次的位置信息
            {
                fence = (_GPSLocationHashtable[gps.UID] as GPSLocation).CurrentFence;
                if (fence != null)
                {
                    if (!(fence as Fence).Buffer.IsPointIn(gps.Longitude, gps.Latitude))//不在上次的围栏上
                    {
                        fence = null;
                    }
                }
            }

            if (fence == null)//没有找到或不在上次所在围栏，重新计算当前围栏
            {
                if (_MonitorGrid.GridCellsFence.ContainsKey(gps.UID))
                {
                    Hashtable Geometrylist = _MonitorGrid.GridCellsFence[gps.UID] as Hashtable;

                    object temp = _MonitorGrid.GetGeometry(gps.Longitude, gps.Latitude, Geometrylist);
                    if (temp != null) fence = temp as Fence;
                }
            }
            return fence;
        }

        /// <summary>
        /// 获得当前GPS位置所在的线路
        /// </summary>
        /// <param name="antgps"></param>
        /// <returns></returns>
        private Route GetRoute(GPS gps)
        {
            Route route = null;
            if (_GPSLocationHashtable.ContainsKey(gps.UID))//看车辆上次的位置信息
            {
                route = (_GPSLocationHashtable[gps.UID] as GPSLocation).CurrentRoute;
                if (route != null)
                {
                    if (!(route as Route).Buffer.IsPointIn(gps.Longitude, gps.Latitude))//不在上次的道路上
                    {
                        route = null;
                    }
                }
            }

            if (route == null)//没有找到或不在上次所在道路，重新计算当前道路
            {
                if( _MonitorGrid.GridCellsRoute.ContainsKey(gps.UID))
                {
                    Hashtable Geometrylist = _MonitorGrid.GridCellsRoute[gps.UID] as Hashtable;

                    object temp = _MonitorGrid.GetGeometry(gps.Longitude, gps.Latitude, Geometrylist);
                    if (temp != null) route = temp as Route;
                }
            }
            return route;
        }

      
    }
}
