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

namespace Gsafety.PTMS.MonitorAlert
{
    /// <summary>
    /// 监控Ant GPS消息，产生相应的其他消息
    /// </summary>
    public class MonitorAlertGenerator
    {
        private Hashtable _AntGPSLocationHashtable;      
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

        public static Hashtable AntGPSPlanHashtable;
       
        public MonitorAlertGenerator()
        {
            _AntGPSLocationHashtable = new Hashtable();
            AntGPSPlanHashtable = new Hashtable();
            _MonitorGrid = new MonitorGrid();
            _MonitorAlertPlan = new MonitorAlertPlan();
            trafficeServer = new TrafficRepository();          
        }

        /// <summary>
        ///根据GPS信息来决定生成什么样的消息
        /// </summary>
        /// <param name="antgps"></param>
        public void HandleAntGPS(PTMSGPS antgps)
        {
            //如果GPS无效则退出
            try
            {
                LoggerManager.Logger.Info("HandleAntGPS");
                if (antgps.GPSValid != "A")
                {
                    LoggerManager.Logger.Info("GPS invalid");
                    return;
                }

                Road road = GetRoad(antgps);
                bool OverSpeed = false;
                
                if (road != null)//找到了当前所在道路
                {
                    if (double.Parse(antgps.Speed) > road.LimitedSpeed)
                    {
                        OverSpeed = true;
                    }
                }
                //获取当前路线
                Route route = GetRoute(antgps);

                //获取当前围栏
                Fence fence = GetFence(antgps);   
                bool InFenceOverSpeed = false;
                bool InFenceUnderSpeed = false;
                if (fence != null)//进入围栏
                {
                    if ((double.Parse(antgps.Speed) > fence.OverSpeed) && (fence.OverSpeed != 0))
                    {
                        InFenceOverSpeed = true;
                    }
                    if (double.Parse(antgps.Speed) < fence.UnderSpeed)
                    {
                        InFenceUnderSpeed = true;
                    }
                }

                //获取当前的监控点列表
                List<GPSFence> cpoint = GetPoint(antgps);
                if (cpoint != null)//存在相关监控点信息
                {
                    foreach (GPSFence pointtemp in cpoint)
                    {
                        if (_MonitorAlertPlan.HandleControlPoint(pointtemp, antgps))
                            (_AntGPSLocationHashtable[antgps.MdvrCoreId] as AntGPSLocation).GenerateMonitorPointAlert(pointtemp, antgps);
                    }
                }           


                //更新当前集合
                if (_AntGPSLocationHashtable.ContainsKey(antgps.MdvrCoreId))//看车辆上次的位置信息
                {
                    (_AntGPSLocationHashtable[antgps.MdvrCoreId] as AntGPSLocation).CurrentANTGPS = antgps;
                    (_AntGPSLocationHashtable[antgps.MdvrCoreId] as AntGPSLocation).CurrentRoad = road;
                    (_AntGPSLocationHashtable[antgps.MdvrCoreId] as AntGPSLocation).CurrentOverSpeed = OverSpeed;
                    (_AntGPSLocationHashtable[antgps.MdvrCoreId] as AntGPSLocation).CurrentRoute = route;
                    (_AntGPSLocationHashtable[antgps.MdvrCoreId] as AntGPSLocation).CurrentFence = fence;
                    (_AntGPSLocationHashtable[antgps.MdvrCoreId] as AntGPSLocation).CurrentInFenceOverSpeed = InFenceOverSpeed;
                    (_AntGPSLocationHashtable[antgps.MdvrCoreId] as AntGPSLocation).CurrentInFenceUnderSpeed = InFenceUnderSpeed;
                }
                else
                {//第一点
                    AntGPSLocation antgpslocation = new AntGPSLocation(antgps, road, route, fence, OverSpeed, InFenceOverSpeed, InFenceUnderSpeed);
                    _AntGPSLocationHashtable.Add(antgps.MdvrCoreId, antgpslocation);
                }
               

                //监控行驶计划
                if (AntGPSPlanHashtable.ContainsKey(antgps.MdvrCoreId))//如果在正在运行的行使计划方案中
                {
                    MonitorPlan tempplan=AntGPSPlanHashtable[antgps.MdvrCoreId] as MonitorPlan;
                    //antgps.GPSTime = antgps.GPSTime.AddHours(-13);
                    Gsafety.PTMS.MonitorAlert.MonitorPlan.EXEStatus _status= _MonitorAlertPlan.HandleANTGPSPlan(tempplan, antgps);
                    if (_status == Gsafety.PTMS.MonitorAlert.MonitorPlan.EXEStatus.Start)
                    {
                        (_AntGPSLocationHashtable[antgps.MdvrCoreId] as AntGPSLocation).GenerateTravelPlanBeginAlert(tempplan, antgps);
                    }
                    else if (_status == Gsafety.PTMS.MonitorAlert.MonitorPlan.EXEStatus.Finish)
                    {
                        (_AntGPSLocationHashtable[antgps.MdvrCoreId] as AntGPSLocation).GenerateTravelPlanEndAlert(tempplan, antgps);
                        lock (MonitorGrid.lockplan)
                        {
                            AntGPSPlanHashtable.Remove(antgps.MdvrCoreId);
                        }
                    }
                }
                //LoggerManager.Logger.Info("HandleAntGPS Finished");
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// 获取ANTGPS
        /// </summary>
        /// <param name="antgps"></param>
        /// <returns></returns>
        private List<GPSFence> GetPoint(PTMSGPS antgps)
        {
            List<GPSFence> cpoint = new List<GPSFence>() ;           
            for (int i = 0; i < _MonitorGrid.allControlPointList.Count; i++)
            {
                if (_MonitorGrid.allControlPointList[i].GPSID == antgps.MdvrCoreId)
                    cpoint.Add(_MonitorGrid.allControlPointList[i]);
            }
            return cpoint;
        }       

        /// <summary>
        /// 获得当前位置所在的围栏
        /// </summary>
        /// <param name="antgps"></param>
        /// <returns></returns>
        private Fence GetFence(PTMSGPS antgps)
        {
            Fence fence = null;
            if (_AntGPSLocationHashtable.ContainsKey(antgps.MdvrCoreId))//看车辆上次的位置信息
            {
                fence = (_AntGPSLocationHashtable[antgps.MdvrCoreId] as AntGPSLocation).CurrentFence;
                if (fence != null)
                {
                    if (!(fence as Fence).Buffer.IsPointIn(antgps.Longitude, antgps.Latitude))//不在上次的围栏上
                    {
                        fence = null;
                    }
                }
            }

            if (fence == null)//没有找到或不在上次所在围栏，重新计算当前围栏
            {
                MonitorGridCell cell = _MonitorGrid.GetFenceMonitorCell(antgps.MdvrCoreId);
                if (cell != null)
                {
                    object temp = cell.GetGeometry(antgps.Longitude, antgps.Latitude);
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
        private Route GetRoute(PTMSGPS antgps)
        {
            Route route = null;
            if (_AntGPSLocationHashtable.ContainsKey(antgps.MdvrCoreId))//看车辆上次的位置信息
            {
                route = (_AntGPSLocationHashtable[antgps.MdvrCoreId] as AntGPSLocation).CurrentRoute;
                if (route != null)
                {
                    if (!(route as Route).Buffer.IsPointIn(antgps.Longitude, antgps.Latitude))//不在上次的道路上
                    {
                        route = null;
                    }
                }
            }

            if (route == null)//没有找到或不在上次所在道路，重新计算当前道路
            {
                MonitorGridCell cell = _MonitorGrid.GetRouteMonitorCell(antgps.MdvrCoreId);
                if (cell != null)
                {
                    object temp = cell.GetGeometry(antgps.Longitude, antgps.Latitude);
                    if (temp != null) route = temp as Route;
                }
            }
            return route;
        }

        /// <summary>
        /// 获得当前GPS位置所在的道路
        /// </summary>
        /// <param name="antgps"></param>
        /// <param name="LastOverSpeed"></param>
        /// <returns></returns>
        private Road GetRoad(PTMSGPS antgps)
        {
            Road road = null;
            if (_AntGPSLocationHashtable.ContainsKey(antgps.MdvrCoreId))//看车辆上次的位置信息
            {
                road = (_AntGPSLocationHashtable[antgps.MdvrCoreId] as AntGPSLocation).CurrentRoad;
                if (road != null)
                {
                    if (!road.Buffer.IsPointIn(antgps.Longitude, antgps.Latitude))//不在上次的道路上
                    {
                        road = null;
                    }
                }
            }

            if (road == null)//没有找到或不在上次所在道路，重新计算当前道路
            {
                string cellid =GridCellCoord.GetCellID(antgps.Longitude, antgps.Latitude);
                MonitorGridCell cell = _MonitorGrid.GetRoadMonitorCell(cellid);
                if (cell != null)
                {
                    object temp=cell.GetGeometry(antgps.Longitude, antgps.Latitude);
                    if (temp != null) road = temp as Road;
                }
            }
            return road;
        }
    }
}
