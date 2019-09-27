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

namespace Gsafety.PTMS.MonitorAlert
{
    /// <summary>
    /// 记录每一个GPS最后一次的位置等信息
    /// </summary>
    public class AntGPSLocation
    {
        private string _Id;
        private bool _CurrentOverSpeed;
        private bool _CurrentInFenceOverSpeed;
        private bool _CurrentInFenceUnderSpeed;
        private Route _CurrentRoute;
        private Road _CurrentRoad;
        private Fence _CurrentFence;
        private PTMSGPS _CurrentANTGPS;
    
////////////BIT0=0，关闭电子围栏，
////////////BIT0=1，使用电子围栏
////////////BIT1=1，超速报警
////////////BIT2=1，低速报警
////////////BIT3=1，入栏报警
////////////BIT4=1，出栏报警
////////////BIT5=0，时间限定无效，BIT5=1，时间限定有效

        public AntGPSLocation()
        {
        }

        /// <summary>
        /// 记录某辆车最近一次所在的道路、路线以及围栏等
        /// </summary>
        /// <param name="antgps"></param>
        /// <param name="road"></param>
        /// <param name="route"></param>
        /// <param name="fence"></param>
        /// <param name="overspeed"></param>
        /// <param name="currentInFenceOverSpeed"></param>
        /// <param name="currentInFenceUnderSpeed"></param>
        public AntGPSLocation(PTMSGPS antgps,Road road, Route route, Fence fence, bool overspeed,bool currentInFenceOverSpeed,bool currentInFenceUnderSpeed)
        {
            try
            {
                _Id = antgps.MdvrCoreId;
                _CurrentRoad = road;
                _CurrentRoute = route;
                _CurrentFence = fence;
                _CurrentOverSpeed = overspeed;
                _CurrentANTGPS = antgps;
                _CurrentInFenceOverSpeed = currentInFenceOverSpeed;
                _CurrentInFenceUnderSpeed = currentInFenceUnderSpeed;

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
             
                if (currentInFenceUnderSpeed) GenerateInFenceUnderSpeedAlert(_CurrentFence);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("AntGPSLocation :" + ex.Message);
            }
        }
        /// <summary>
        /// 唯一标识车的信息，目前采用的是ANT GPSID
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
      /// 当前所在道路
      /// </summary>
        public Road CurrentRoad
        {
            get
            {
                return _CurrentRoad;
            }
            set
            {
                _CurrentRoad = value;
            }
        }
        /// <summary>
        /// 当前所在围栏
        /// </summary>
        public Fence CurrentFence
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
                    _CurrentInFenceUnderSpeed = false;
                }
                _CurrentFence = value;
            }
        }
        /// <summary>
        /// 当前所在路线
        /// </summary>
        public Route CurrentRoute
        {
            get
            {
                return _CurrentRoute;
            }
            set
            {
                if (value != _CurrentRoute)
                {
                    if (value == null) GenerateOutRouteAlert(_CurrentRoute.Id,_CurrentRoute.Name);
                    else GenerateInRouteAlert(value.Id,value.Name);
                }
                _CurrentRoute = value;
            }
        }
        /// <summary>
        /// 当前的GPS信息
        /// </summary>
        public PTMSGPS CurrentANTGPS
        {
            get
            {
                return _CurrentANTGPS;
            }
            set
            {
                _CurrentANTGPS = value;
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
                    if (value == true) GenerateOverSpeedAlert(_CurrentRoad.Id,_CurrentRoad.Name);
                    else GenerateNormalSpeedAlert(_CurrentRoad.Id, _CurrentRoad.Name);
                }
                _CurrentOverSpeed = value;
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
                    else GenerateInFenceOverSpeed2NormalAlert(_CurrentFence);
                }
                _CurrentInFenceOverSpeed = value;
            }
        }

       
        /// <summary>
        /// 当前是否围栏内低速
        /// </summary>
        public bool CurrentInFenceUnderSpeed
        {
            get
            {
                return _CurrentInFenceUnderSpeed;
            }
            set
            {
                if (value != _CurrentInFenceUnderSpeed)
                {
                    if (value == true) GenerateInFenceUnderSpeedAlert(_CurrentFence);
                    else GenerateInFenceUnderSpeed2NormalAlert(_CurrentFence);      
                }
                _CurrentInFenceUnderSpeed = value;
            }
        }

        /// <summary>
        /// 低速恢复正常报警
        /// </summary>
        /// <param name="fence"></param>
        private void GenerateInFenceUnderSpeed2NormalAlert(Fence fence)
        {
            try
            {
                if (fence.Alert("BIT2"))
                {
                    RegionAlert alert = new RegionAlert()
                    {
                        AlertType = (int)BusinessAlertType.MonitorInFenceUnderSpeed2Normal,
                        AlertTime = DateTime.Now,
                        SubRegionAlertType = "MonitorAlert",

                        MdvrCoreSN = _CurrentANTGPS.MdvrCoreId,
                        Longitude = _CurrentANTGPS.Longitude,
                        Latitude = _CurrentANTGPS.Latitude,
                        Speed = _CurrentANTGPS.Speed,
                        Direction = _CurrentANTGPS.Direction,
                        GpsTime = _CurrentANTGPS.GPSTime,
                        GpsValid = _CurrentANTGPS.GPSValid,
                        Context = _CurrentANTGPS.Context,
                        VehicleID = _CurrentANTGPS.VehicleId,
                        SuitInfoID = _CurrentANTGPS.SuiteInfoId,
                        //围栏信息
                        FenceId = fence.Id,
                        FenceName = fence.Name,
                    };

                    MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalMonitorInFenceUnderSpeed2Normal, MonitorAlertMessage.ObjectToBytes(alert));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateInFenceUnderSpeed2NormalAlert :" + ex.Message);
            }
        }

        /// <summary>
        /// 低速告警
        /// </summary>
        /// <param name="fence"></param>
        private void GenerateInFenceUnderSpeedAlert(Fence fence)
        {
            try
            {
                if (fence.Alert("BIT2"))
                {
                    RegionAlert alert = new RegionAlert()
                    {
                        AlertType = (int)BusinessAlertType.UnderSpeedFence,
                        AlertTime = DateTime.Now,
                        SubRegionAlertType = "MonitorAlert",

                        MdvrCoreSN = _CurrentANTGPS.MdvrCoreId,
                        Longitude = _CurrentANTGPS.Longitude,
                        Latitude = _CurrentANTGPS.Latitude,
                        Speed = _CurrentANTGPS.Speed,
                        Direction = _CurrentANTGPS.Direction,
                        GpsTime = _CurrentANTGPS.GPSTime,
                        GpsValid = _CurrentANTGPS.GPSValid,
                        Context = _CurrentANTGPS.Context,
                        VehicleID = _CurrentANTGPS.VehicleId,
                        SuitInfoID = _CurrentANTGPS.SuiteInfoId,
                        //围栏信息
                        FenceId = fence.Id,
                        FenceName = fence.Name,
                    };

                    MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalMonitorInFenceUnderSpeed, MonitorAlertMessage.ObjectToBytes(alert));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateInFenceUnderSpeedAlert :" + ex.Message);
            }
        }

        /// <summary>
        /// 围栏内超速恢复正常告警
        /// </summary>
        /// <param name="fence"></param>
        private void GenerateInFenceOverSpeed2NormalAlert(Fence fence)
        {
            try
            {
                if (fence.Alert("BIT1"))
                {
                    RegionAlert alert = new RegionAlert()
                    {
                        AlertType = (int)BusinessAlertType.MonitorInFenceOverSpeed2Normal,
                        AlertTime = DateTime.Now,
                        SubRegionAlertType = "MonitorAlert",

                        MdvrCoreSN = _CurrentANTGPS.MdvrCoreId,
                        Longitude = _CurrentANTGPS.Longitude,
                        Latitude = _CurrentANTGPS.Latitude,
                        Speed = _CurrentANTGPS.Speed,
                        Direction = _CurrentANTGPS.Direction,
                        GpsTime = _CurrentANTGPS.GPSTime,
                        GpsValid = _CurrentANTGPS.GPSValid,
                        Context = _CurrentANTGPS.Context,
                        VehicleID = _CurrentANTGPS.VehicleId,
                        SuitInfoID = _CurrentANTGPS.SuiteInfoId,
                        //围栏信息
                        FenceId = fence.Id,
                        FenceName = fence.Name,
                    };

                    MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalMonitorInFenceOverSpeed2Normal, MonitorAlertMessage.ObjectToBytes(alert));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateInFenceOverSpeed2NormalAlert :" + ex.Message);
            }
        }

        /// <summary>
        /// 围栏内超速告警
        /// </summary>
        /// <param name="fence"></param>
        private void GenerateInFenceOverSpeedAlert(Fence fence)
        {
            try
            {
                if (fence.Alert("BIT1"))
                {
                    RegionAlert alert = new RegionAlert()
                    {
                        AlertType = (int)BusinessAlertType.OverSpeedFence,
                        AlertTime = DateTime.Now,
                        SubRegionAlertType = "MonitorAlert",

                        MdvrCoreSN = _CurrentANTGPS.MdvrCoreId,
                        Longitude = _CurrentANTGPS.Longitude,
                        Latitude = _CurrentANTGPS.Latitude,
                        Speed = _CurrentANTGPS.Speed,
                        Direction = _CurrentANTGPS.Direction,
                        GpsTime = _CurrentANTGPS.GPSTime,
                        GpsValid = _CurrentANTGPS.GPSValid,
                        Context = _CurrentANTGPS.Context,
                        VehicleID = _CurrentANTGPS.VehicleId,
                        SuitInfoID = _CurrentANTGPS.SuiteInfoId,
                        //围栏信息
                        FenceId = fence.Id,
                        FenceName = fence.Name,
                    };

                    MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalMonitorInFenceOverSpeed, MonitorAlertMessage.ObjectToBytes(alert));
                }
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
        private void GenerateInFenceAlert(Fence fence)
        {
            try
            {
                //if (_CurrentFence == null) return;
                if (fence.Alert("BIT3"))
                {
                    RegionAlert alert = new RegionAlert()
                    {
                        AlertType = (int)BusinessAlertType.InFence,
                        AlertTime = DateTime.Now,
                        SubRegionAlertType = "MonitorAlert",

                        MdvrCoreSN = _CurrentANTGPS.MdvrCoreId,
                        Longitude = _CurrentANTGPS.Longitude,
                        Latitude = _CurrentANTGPS.Latitude,
                        Speed = _CurrentANTGPS.Speed,
                        Direction = _CurrentANTGPS.Direction,
                        GpsTime = _CurrentANTGPS.GPSTime,
                        GpsValid = _CurrentANTGPS.GPSValid,
                        Context = _CurrentANTGPS.Context,
                        VehicleID = _CurrentANTGPS.VehicleId,
                        SuitInfoID = _CurrentANTGPS.SuiteInfoId,
                        //围栏信息
                        FenceId = fence.Id,
                        FenceName = fence.Name,
                    };

                    MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalMonitorInFence, MonitorAlertMessage.ObjectToBytes(alert));
                }
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
        private void GenerateOutFenceAlert(Fence fence)
        {
            try
            {
                //if (_CurrentFence == null) return;
                if (fence.Alert("BIT4"))
                {
                    RegionAlert alert = new RegionAlert()
                    {
                        AlertType = (int)BusinessAlertType.OutFence,
                        AlertTime = DateTime.Now,
                        SubRegionAlertType = "MonitorAlert",

                        MdvrCoreSN = _CurrentANTGPS.MdvrCoreId,
                        Longitude = _CurrentANTGPS.Longitude,
                        Latitude = _CurrentANTGPS.Latitude,
                        Speed = _CurrentANTGPS.Speed,
                        Direction = _CurrentANTGPS.Direction,
                        GpsTime = _CurrentANTGPS.GPSTime,
                        GpsValid = _CurrentANTGPS.GPSValid,
                        Context = _CurrentANTGPS.Context,
                        VehicleID = _CurrentANTGPS.VehicleId,
                        SuitInfoID = _CurrentANTGPS.SuiteInfoId,
                        //围栏信息
                        FenceId = fence.Id,
                        FenceName = fence.Name,
                    };

                    MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalMonitorOutFence, MonitorAlertMessage.ObjectToBytes(alert));
                }
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

                    MdvrCoreSN = _CurrentANTGPS.MdvrCoreId,
                    Longitude = _CurrentANTGPS.Longitude,
                    Latitude = _CurrentANTGPS.Latitude,
                    Speed = _CurrentANTGPS.Speed,
                    Direction = _CurrentANTGPS.Direction,
                    GpsTime = _CurrentANTGPS.GPSTime,
                    GpsValid = _CurrentANTGPS.GPSValid,
                    Context = _CurrentANTGPS.Context,
                    VehicleID = _CurrentANTGPS.VehicleId,
                    SuitInfoID = _CurrentANTGPS.SuiteInfoId,
                    //路线信息
                    FenceId = routeid,
                    FenceName = routename,
                };

                MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalMonitorOutRoute, MonitorAlertMessage.ObjectToBytes(alert));
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

                    MdvrCoreSN = _CurrentANTGPS.MdvrCoreId,
                    Longitude = _CurrentANTGPS.Longitude,
                    Latitude = _CurrentANTGPS.Latitude,
                    Speed = _CurrentANTGPS.Speed,
                    Direction = _CurrentANTGPS.Direction,
                    GpsTime = _CurrentANTGPS.GPSTime,
                    GpsValid = _CurrentANTGPS.GPSValid,
                    Context = _CurrentANTGPS.Context,
                    VehicleID = _CurrentANTGPS.VehicleId,
                    SuitInfoID = _CurrentANTGPS.SuiteInfoId,
                    //路线信息
                    FenceId = routeid,
                    FenceName = routename,
                };

                MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalMonitorInRoute, MonitorAlertMessage.ObjectToBytes(alert));
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
        private void GenerateOverSpeedAlert(string roadid,string roadname)
        {
            try
            {
                //if (_CurrentRoad == null) return;

                RegionAlert alert = new RegionAlert()
                {
                    AlertType = (int)BusinessAlertType.MonitorOverSpeed,
                    AlertTime = DateTime.Now,
                    SubRegionAlertType = "MonitorAlert",

                    MdvrCoreSN = _CurrentANTGPS.MdvrCoreId,
                    Longitude = _CurrentANTGPS.Longitude,
                    Latitude = _CurrentANTGPS.Latitude,
                    Speed = _CurrentANTGPS.Speed,
                    Direction = _CurrentANTGPS.Direction,
                    GpsTime = _CurrentANTGPS.GPSTime,
                    GpsValid = _CurrentANTGPS.GPSValid,
                    Context = _CurrentANTGPS.Context,
                    VehicleID = _CurrentANTGPS.VehicleId,
                    SuitInfoID = _CurrentANTGPS.SuiteInfoId,
                    //道路信息
                    FenceId = roadid,
                    FenceName = roadname,
                };

                MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalMonitorOverSpeed, MonitorAlertMessage.ObjectToBytes(alert));
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateOverSpeedAlert :" + ex.Message);
            }
        }

        /// <summary>
        /// 超速恢复告警
        /// </summary>
        /// <param name="roadid"></param>
        /// <param name="roadname"></param>
        private void GenerateNormalSpeedAlert(string roadid, string roadname)
        {
            try
            {
                //if (_CurrentRoad == null) return;

                RegionAlert alert = new RegionAlert()
                {
                    AlertType = (int)BusinessAlertType.MonitorNormalSpeed,
                    AlertTime = DateTime.Now,
                    SubRegionAlertType = "MonitorAlert",

                    MdvrCoreSN = _CurrentANTGPS.MdvrCoreId,
                    Longitude = _CurrentANTGPS.Longitude,
                    Latitude = _CurrentANTGPS.Latitude,
                    Speed = _CurrentANTGPS.Speed,
                    Direction = _CurrentANTGPS.Direction,
                    GpsTime = _CurrentANTGPS.GPSTime,
                    GpsValid = _CurrentANTGPS.GPSValid,
                    Context = _CurrentANTGPS.Context,
                    VehicleID = _CurrentANTGPS.VehicleId,
                    SuitInfoID = _CurrentANTGPS.SuiteInfoId,
                    //道路信息
                    FenceId = roadid,
                    FenceName = roadname,
                };

                MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalMonitorNormalSpeed, MonitorAlertMessage.ObjectToBytes(alert));
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateNormalSpeedAlert :" + ex.Message);
            }
        }

        /// <summary>
        /// 生成车辆开始执行行驶计划告警
        /// </summary>
        /// <param name="travelPlanId"></param>
        /// <param name="travePlanName"></param>
        public void GenerateTravelPlanBeginAlert(MonitorPlan travePlan, PTMSGPS travelGPS)
        {            
            try
            {
                RegionAlert alert = new RegionAlert()
                {
                    AlertType = (int)BusinessAlertType.MonitorTravelPlanBegin,
                    AlertTime = DateTime.Now,
                    SubRegionAlertType = "MonitorAlert",

                    MdvrCoreSN = travelGPS.MdvrCoreId,
                    Longitude = travelGPS.Longitude,
                    Latitude = travelGPS.Latitude,
                    Speed = travelGPS.Speed,
                    Direction = travelGPS.Direction,
                    GpsTime = travelGPS.GPSTime,
                    GpsValid = travelGPS.GPSValid,
                    Context = travelGPS.Context,
                    VehicleID = travePlan._vehicleID,
                    SuitInfoID = travelGPS.SuiteInfoId,
                    //行使计划计划信息
                    FenceId = travePlan._planId,
                    FenceName = travePlan._planName,
                };
                MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalMonitorTravelPlansBegin, MonitorAlertMessage.ObjectToBytes(alert));
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateTravelPlanBeginAlert :" + ex.Message);
            }
        }

        /// <summary>
        /// 生成车辆结束告警，没有AntGPS上报的情况
        /// </summary>
        /// <param name="travePlan"></param>
        public void GenerateTravelPlanEndAlert(MonitorPlan travelPlan)
        {
            try
            {
                RegionAlert alert = new RegionAlert()
                {
                    AlertType = (int)BusinessAlertType.MonitorTravelPlanEnd,
                    AlertTime = DateTime.Now,
                    SubRegionAlertType = "MonitorAlert",

                    MdvrCoreSN = travelPlan._antGpsID,
                    Longitude = "-",
                    Latitude = "-",
                    Speed = "-",
                    Direction = "-",
                    GpsTime = null,
                    GpsValid = "V",
                    Context = "null",
                    VehicleID = travelPlan._vehicleID,
                    SuitInfoID = "-",
                    //行驶计划信息
                    FenceId = travelPlan._planId,
                    FenceName = travelPlan._planName,                   
                };
                MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalMonitorTravelPlansEnd, MonitorAlertMessage.ObjectToBytes(alert));
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateTravelPlanEndAlert :" + ex.Message);
            }
        }

        /// <summary>
        /// 生成车辆结束执行行驶计划告警
        /// </summary>
        /// <param name="travelPlanId"></param>
        /// <param name="travePlanName"></param>
        public void GenerateTravelPlanEndAlert(MonitorPlan travePlan, PTMSGPS travelGPS)
        {
            try
            {
                RegionAlert alert = new RegionAlert()
                {
                    AlertType = (int)BusinessAlertType.MonitorTravelPlanEnd,
                    AlertTime = DateTime.Now,
                    SubRegionAlertType = "MonitorAlert",

                    MdvrCoreSN = travelGPS.MdvrCoreId,
                    Longitude = travelGPS.Longitude,
                    Latitude = travelGPS.Latitude,
                    Speed = travelGPS.Speed,
                    Direction = travelGPS.Direction,
                    GpsTime = travelGPS.GPSTime,
                    GpsValid = travelGPS.GPSValid,
                    Context = travelGPS.Context,
                    VehicleID = travePlan._vehicleID,
                    SuitInfoID = travelGPS.SuiteInfoId,
                    //行使计划计划信息
                    FenceId = travePlan._planId,
                    FenceName = travePlan._planName,
                };
                MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalMonitorTravelPlansEnd, MonitorAlertMessage.ObjectToBytes(alert));
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateTravelPlanEndAlert :" + ex.Message);
            }
        }


        /// <summary>
        /// 生成车辆取消执行行驶计划告警
        /// </summary>
        /// <param name="travelPlanId"></param>
        /// <param name="travePlanName"></param>
        public void GenerateTravelPlanCancelAlert(MonitorPlan travelPlan)
        {
            try
            {
                RegionAlert alert = new RegionAlert()
                {
                    AlertType = (int)BusinessAlertType.MonitorTravelPlanCancel,
                    AlertTime = DateTime.Now,
                    SubRegionAlertType = "MonitorAlert",

                    MdvrCoreSN = travelPlan._antGpsID,
                    Longitude = "-",
                    Latitude = "-",
                    Speed = "-",
                    Direction = "-",
                    GpsTime = null,
                    GpsValid = "V",
                    Context = "null",
                    VehicleID = travelPlan._vehicleID,
                    SuitInfoID = "-",
                    //行驶计划信息
                    FenceId = travelPlan._planId,
                    FenceName = travelPlan._planName,
                };
                MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalMonitorTravelPlansCancel, MonitorAlertMessage.ObjectToBytes(alert));
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateTravelPlanCancelAlert :" + ex.Message);
            }
        }

        /// <summary>
        /// 生成车辆进入监控点告警
        /// </summary>
        /// <param name="travelPlanId"></param>
        public void GenerateMonitorPointAlert(GPSFence cpoint, PTMSGPS antgps)
        {
            try
            {
                RegionAlert alert = new RegionAlert()
                {
                    AlertType = (int)BusinessAlertType.MonitorEnterPoint,
                    AlertTime = DateTime.Now,
                    SubRegionAlertType = "MonitorAlert",
                    MdvrCoreSN = antgps.MdvrCoreId,
                    Longitude = antgps.Longitude,
                    Latitude = antgps.Latitude,
                    Speed = antgps.Speed,
                    Direction = antgps.Direction,
                    GpsTime = antgps.GPSTime,
                    GpsValid = antgps.GPSValid,
                    Context = antgps.Context,
                    VehicleID = cpoint.VEHICLE_ID,
                    SuitInfoID = antgps.SuiteInfoId,
                    FenceId = cpoint.FenceID.ToString(),
                    FenceName = cpoint.FenceName
                };
                MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, AlertRoute.OriginalMonitorEnterPoint, MonitorAlertMessage.ObjectToBytes(alert));               
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GenerateMonitorPointAlert :" + ex.Message);
            }
        }

       
    }
}
