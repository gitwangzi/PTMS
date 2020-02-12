/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: fbc8e551-1021-4543-910e-ddb058af1407      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGZG
/////                 Author: TEST(zhangzg)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MonitorAlert
/////    Project Description:    
/////             Class Name: AntGPSLocation
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/8 14:28:22
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/8 14:28:22
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.Common.Logging;
using Gsafety.MQ;
using RabbitMQ.Client;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Traffic.Contract.Data;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.Common.Data;

namespace Gsafety.PTMS.MonitorAlert
{
    /// <summary>
    /// 记录每一个GPS最后一次的位置等信息
    /// </summary>
    public class GPSLocation
    {
        private string _Id;
        private bool _CurrentOverSpeed;
        private bool _CurrentInFenceOverSpeed;
        private bool _CurrentInRouteOverSpeed;
        private DateTime _OverSpeedTime;
        private DateTime _InFenceOverSpeedTime;
        private DateTime _InRouteOverSpeedTime;
        private TrafficRoute _CurrentRoute;
        private TrafficFence _CurrentFence;
        private GPS _CurrentGPS;   


        public GPSLocation()
        {
        }

        /// <summary>
        /// 记录某辆车最近一次所在的路线以及围栏、速度等
        /// </summary>
        /// <param name="antgps"></param>
        /// <param name="road" ></param>
        /// <param name="route"></param>
        /// <param name="fence"></param>
        /// <param name="overspeed"></param>
        /// <param name="currentInFenceOverSpeed"></param>
        /// <param name="currentInFenceUnderSpeed"></param>
        public GPSLocation(GPS gps, TrafficRoute route, TrafficFence fence, bool overspeed, bool currentInFenceOverSpeed, bool currentInRouteOverSpeed, DateTime overspeedTime)
        {
            try
            {
                _Id = gps.UID;
                _CurrentRoute = route;
                _CurrentFence = fence;               
                _CurrentGPS = gps;
                _CurrentOverSpeed = overspeed;
                if (overspeed)
                {
                    _OverSpeedTime = overspeedTime;
                
                }
                _CurrentInFenceOverSpeed = currentInFenceOverSpeed;
                if (currentInFenceOverSpeed)
                {
                    _InFenceOverSpeedTime = overspeedTime;

                }
                _CurrentInRouteOverSpeed = currentInRouteOverSpeed;
                if (currentInRouteOverSpeed)
                {
                    _InRouteOverSpeedTime = overspeedTime;

                }

                //当第一点就是超速时，引发超速事件
                //if (overspeed) GenerateOverSpeedAlert();
                //当第一点就在线路外面时，可以不引发不遵循路线报警
                //if (route == null) GenerateOutRouteAlert();

                //当第一点在围栏内时，引发入围栏报警
                if (fence != null)
                {
                    GenerateInFenceAlert(fence);
                }

                if (currentInFenceOverSpeed) GenerateInFenceOverSpeedAlert(_CurrentFence);

                if (currentInRouteOverSpeed) GenerateInRouteOverSpeedAlert(_CurrentRoute);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("AntGPSLocation :" + ex.Message);
            }
        }
        /// <summary>
        /// 唯一标识车的信息，目前采用的是GPSID
        /// </summary>
        public string Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }       
     
        /// <summary>
        /// 当前所在围栏
        /// </summary>
        public TrafficFence CurrentFence
        {
            get
            {
                return _CurrentFence;
            }
            set
            {
                if (value != _CurrentFence)
                {
                    if (value == null)
                    {//出围栏了
                        GenerateOutFenceAlert(_CurrentFence);
                    }
                    else
                    {
                        if (_CurrentFence == null)
                        {//进围栏了
                            GenerateInFenceAlert(value);
                        }
                        else
                        {//先出一个，再进一个
                            GenerateOutFenceAlert(_CurrentFence);
                            GenerateInFenceAlert(value);
                        }
                    }
                    //换围栏后重新算围栏内超速否
                    _CurrentInFenceOverSpeed = false;
                   
                }
                _CurrentFence = value;
            }
        }
        /// <summary>
        /// 当前所在路线
        /// </summary>
        public TrafficRoute CurrentRoute
        {
            get
            {
                return _CurrentRoute;
            }
            set
            {
                if (value != _CurrentRoute)
                {
                    if (value == null) GenerateOutRouteAlert(_CurrentRoute.ID,_CurrentRoute.Name);
                    else GenerateInRouteAlert(value.ID, value.Name);
                }
                _CurrentRoute = value;
            }
        }
        /// <summary>
        /// 当前的GPS信息
        /// </summary>
        public GPS CurrentGPS
        {
            get
            {
                return _CurrentGPS;
            }
            set
            {
                _CurrentGPS = value;
            }
        }
        /// <summary>
        /// 当前是否超速
        /// </summary>
        public bool CurrentOverSpeed
        {
            get
            {
                return _CurrentOverSpeed;
            }
            set
            {
                if (value != _CurrentOverSpeed)
                {
                    if (value == true) GenerateOverSpeedAlert();
                   
                }
                _CurrentOverSpeed = value;
            }
        }

        public DateTime OverSpeedTime
        {
            get
            {
                return _OverSpeedTime;
            }
            set
            {
               _OverSpeedTime = value;
            }
        }
        /// <summary>
        /// 当前是否在围栏内超速
        /// </summary>
        public bool CurrentInFenceOverSpeed
        {
            get
            {
                return _CurrentInFenceOverSpeed;
            }
            set
            {
                if (value != _CurrentInFenceOverSpeed)
                {
                    if (value == true) GenerateInFenceOverSpeedAlert(_CurrentFence);
                  
                }
                _CurrentInFenceOverSpeed = value;
            }
        }
        public DateTime InFenceOverSpeedTime
        {
            get
            {
                return _InFenceOverSpeedTime;
            }
            set
            {
                _InFenceOverSpeedTime = value;
            }
        }

        /// <summary>
        /// 当前是否在路线内超速
        /// </summary>
        public bool CurrentInRouteOverSpeed
        {
            get
            {
                return _CurrentInRouteOverSpeed;
            }
            set
            {
                if (value != _CurrentInRouteOverSpeed)
                {
                    if (value == true) GenerateInRouteOverSpeedAlert(_CurrentRoute);
                   
                }
                _CurrentInRouteOverSpeed = value;
            }
        }
        public DateTime InRouteOverSpeedTime
        {
            get
            {
                return _InRouteOverSpeedTime;
            }
            set
            {
                _InRouteOverSpeedTime = value;
            }
        }

        /// <summary>
        /// 围栏内超速告警
        /// </summary>
        /// <param name="fence"></param>
        private void GenerateInFenceOverSpeedAlert(TrafficFence fence)
        {
            try
            {
                
                    RegionAlert alert = new RegionAlert()
                    {
                        AlertType = (int)BusinessAlertType.OverSpeedFence,
                        AlertTime = DateTime.Now,
                        SubRegionAlertType = "MonitorAlert",


                        MdvrCoreSN = _CurrentGPS.UID,
                        Longitude = _CurrentGPS.Longitude,
                        Latitude = _CurrentGPS.Latitude,
                        Speed = _CurrentGPS.Speed,
                        Direction = _CurrentGPS.Direction,
                        GpsTime = _CurrentGPS.GpsTime,
                        GpsValid = _CurrentGPS.Valid,
                        VehicleID = _CurrentGPS.VehicleId,
                        SuitInfoID = _CurrentGPS.UID,
                        //围栏信息
                        FenceId = fence.ID,
                        FenceName = fence.Name,
                    };

                    MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalBusinessAlertKey, MonitorAlertMessage.ObjectToBytes(alert));
                
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateInFenceOverSpeedAlert :" + ex.Message);
            }
        }

        /// <summary>
        /// 路线内超速告警
        /// </summary>
        /// <param name="fence"></param>
        private void GenerateInRouteOverSpeedAlert(TrafficRoute route)
        {
            try
            {
                
                    RegionAlert alert = new RegionAlert()
                    {
                        AlertType = (int)BusinessAlertType.OverSpeedFence,
                        AlertTime = DateTime.Now,
                        SubRegionAlertType = "MonitorAlert",

                        MdvrCoreSN = _CurrentGPS.UID,
                        Longitude = _CurrentGPS.Longitude,
                        Latitude = _CurrentGPS.Latitude,
                        Speed = _CurrentGPS.Speed,
                        Direction = _CurrentGPS.Direction,
                        GpsTime = _CurrentGPS.GpsTime,
                        GpsValid = _CurrentGPS.Valid,
                        VehicleID = _CurrentGPS.VehicleId,
                        SuitInfoID = _CurrentGPS.UID,
                        //围栏信息
                        FenceId = route.ID,
                        FenceName = route.Name,
                    };

                    MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalBusinessAlertKey, MonitorAlertMessage.ObjectToBytes(alert));
                }
            
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateInFenceOverSpeedAlert :" + ex.Message);
            }
        }
        /// <summary>
        /// 入围栏告警
        /// </summary>
        /// <param name="fence"></param>
        private void GenerateInFenceAlert(TrafficFence fence)
        {
            try
            {
               
                    RegionAlert alert = new RegionAlert()
                    {
                        AlertType = (int)BusinessAlertType.InFence,
                        AlertTime = DateTime.Now,
                        SubRegionAlertType = "MonitorAlert",


                        MdvrCoreSN = _CurrentGPS.UID,
                        Longitude = _CurrentGPS.Longitude,
                        Latitude = _CurrentGPS.Latitude,
                        Speed = _CurrentGPS.Speed,
                        Direction = _CurrentGPS.Direction,
                        GpsTime = _CurrentGPS.GpsTime,
                        GpsValid = _CurrentGPS.Valid,
                        VehicleID = _CurrentGPS.VehicleId,
                        SuitInfoID = _CurrentGPS.UID,
                        //围栏信息
                        FenceId = fence.ID,
                        FenceName = fence.Name,
                    };

                    MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalBusinessAlertKey, MonitorAlertMessage.ObjectToBytes(alert));
                
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateInFenceAlert :" + ex.Message);
            }
        }
        /// <summary>
        /// 出围栏告警
        /// </summary>
        /// <param name="fence"></param>
        private void GenerateOutFenceAlert(TrafficFence fence)
        {
            try
            {
               
                    RegionAlert alert = new RegionAlert()
                    {
                        AlertType = (int)BusinessAlertType.OutFence,
                        AlertTime = DateTime.Now,
                        SubRegionAlertType = "MonitorAlert",


                        MdvrCoreSN = _CurrentGPS.UID,
                        Longitude = _CurrentGPS.Longitude,
                        Latitude = _CurrentGPS.Latitude,
                        Speed = _CurrentGPS.Speed,
                        Direction = _CurrentGPS.Direction,
                        GpsTime = _CurrentGPS.GpsTime,
                        GpsValid = _CurrentGPS.Valid,
                        VehicleID = _CurrentGPS.VehicleId,
                        SuitInfoID = _CurrentGPS.UID,
                        //围栏信息
                        FenceId = fence.ID,
                        FenceName = fence.Name,
                    };

                    MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalBusinessAlertKey, MonitorAlertMessage.ObjectToBytes(alert));
                
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateOutFenceAlert :" + ex.Message);
            }
        }

        /// <summary>
        /// 偏离路线告警
        /// </summary>
        /// <param name="routeid"></param>
        /// <param name="routename"></param>
        private void GenerateOutRouteAlert(string routeid,string routename)
        {
            try
            {
                //if (_CurrentRoute == null) return;

                RegionAlert alert = new RegionAlert()
                {
                    AlertType = (int)BusinessAlertType.MonitorOutRoute,
                    AlertTime = DateTime.Now,
                    SubRegionAlertType = "MonitorAlert",

                    MdvrCoreSN = _CurrentGPS.UID,
                    Longitude = _CurrentGPS.Longitude,
                    Latitude = _CurrentGPS.Latitude,
                    Speed = _CurrentGPS.Speed,
                    Direction = _CurrentGPS.Direction,
                    GpsTime = _CurrentGPS.GpsTime,
                    GpsValid = _CurrentGPS.Valid,                   
                    VehicleID = _CurrentGPS.VehicleId,
                    SuitInfoID = _CurrentGPS.UID,
                    //路线信息
                    FenceId = routeid,
                    FenceName = routename,
                };

                MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalBusinessAlertKey, MonitorAlertMessage.ObjectToBytes(alert));
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateOutRouteAlert :" + ex.Message);
            }
        }

        /// <summary>
        /// 偏离路线回归告警
        /// </summary>
        /// <param name="routeid"></param>
        /// <param name="routename"></param>
        private void GenerateInRouteAlert(string routeid, string routename)
        {
            try
            {
                //if (_CurrentRoute == null) return;

                RegionAlert alert = new RegionAlert()
                {
                    AlertType = (int)BusinessAlertType.MonitorInRoute,
                    AlertTime = DateTime.Now,
                    SubRegionAlertType = "MonitorAlert",

                    MdvrCoreSN = _CurrentGPS.UID,
                    Longitude = _CurrentGPS.Longitude,
                    Latitude = _CurrentGPS.Latitude,
                    Speed = _CurrentGPS.Speed,
                    Direction = _CurrentGPS.Direction,
                    GpsTime = _CurrentGPS.GpsTime,
                    GpsValid = _CurrentGPS.Valid,
                    VehicleID = _CurrentGPS.VehicleId,
                    SuitInfoID = _CurrentGPS.UID,
                    //路线信息
                    FenceId = routeid,
                    FenceName = routename,
                };

                MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalBusinessAlertKey, MonitorAlertMessage.ObjectToBytes(alert));
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateInRouteAlert :" + ex.Message);
            }
        }

        /// <summary>
        /// 超速告警
        /// </summary>
        /// <param name="roadid"></param>
        /// <param name="roadname"></param>
        private void GenerateOverSpeedAlert()
        {
            try
            {
                //if (_CurrentRoad == null) return;

                RegionAlert alert = new RegionAlert()
                {
                    AlertType = (int)BusinessAlertType.MonitorOverSpeed,
                    AlertTime = DateTime.Now,
                    SubRegionAlertType = "MonitorAlert",


                    MdvrCoreSN = _CurrentGPS.UID,
                    Longitude = _CurrentGPS.Longitude,
                    Latitude = _CurrentGPS.Latitude,
                    Speed = _CurrentGPS.Speed,
                    Direction = _CurrentGPS.Direction,
                    GpsTime = _CurrentGPS.GpsTime,
                    GpsValid = _CurrentGPS.Valid,
                    VehicleID = _CurrentGPS.VehicleId,
                    SuitInfoID = _CurrentGPS.UID,
                  
                };

                MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalBusinessAlertKey, MonitorAlertMessage.ObjectToBytes(alert));
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateOverSpeedAlert :" + ex.Message);
            }
        }     
               
    }
}
