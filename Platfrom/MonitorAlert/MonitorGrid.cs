/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 56cee831-48ce-4679-a680-2dd55e9c59fd      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGZG
/////                 Author: TEST(zhangzg)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MonitorAlert
/////    Project Description:    
/////             Class Name: MonitorGrid
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/8 14:14:25
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/8 14:14:25
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using Gsafety.Common.Util;
using Gsafety.PTMS.Message.Contract.Data;
using System.Web.Script.Serialization;
using Gsafety.PTMS.Traffic.Repository;
using Gsafety.PTMS.Traffic.Contract.Data;
using System.Threading;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Analysis.Helper;
using System.Web;
using Gsafety.MQ;
using System.Configuration;
using System.Runtime.CompilerServices;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Message.Contract.Data;

namespace Gsafety.PTMS.MonitorAlert
{
    /// <summary>
    /// 将整个地图划分为若干个网格进行管理
    /// </summary>
    public class MonitorGrid
    {
        private Hashtable _GridCellsRoad;
        private Hashtable _GridCellsFence;
        private Hashtable _GridCellsRoute;
        private List<GPSStopScheDule> _allDuleList;
        private List<GPSFence> _allControlPointList;
        private const int ReadMAXCountOnce = 500;
        private double RouteBuffer = 1.0 * 200 * 100 / (60 * 1852);
        private double RoadBuffer = 1.0 * 200 * 100 / (60 * 1852);
        private double FenceBuffer = 0;

        private Object lockpoint = new object();

        public static Object lockplan = new object();

        private TrafficRepository trafficeServer;

        public Hashtable GridCellsRoad
        {
            get
            {
                return _GridCellsRoad;
            }
        }
        public Hashtable GridCellsFence
        {
            get
            {
                return _GridCellsFence;
            }
        }
        public Hashtable GridCellsRoute
        {
            get
            {
                return _GridCellsRoute;
            }
        }

        public List<GPSStopScheDule> allDuleList
        {
            get
            {
                return _allDuleList;
            }
        }

        public List<GPSFence> allControlPointList
        {
            get
            {
                return _allControlPointList;
            }
        }


        /// <summary>
        /// 初始化
        /// </summary>
        public MonitorGrid()
        {
            RouteBuffer = 1.0 * Convert.ToInt32(ConfigurationManager.AppSettings["RouteBufferWidth"]) * 100 / (60 * 1852);
            RoadBuffer = 1.0 * Convert.ToInt32(ConfigurationManager.AppSettings["RoadBufferWidth"]) * 100 / (60 * 1852);

            _GridCellsRoad = new Hashtable(); //Road是靠网格来分隔
            _GridCellsRoute = new Hashtable();//是靠antgpsid来分隔
            _GridCellsFence = new Hashtable();//是靠antgpsid来分隔

            _allDuleList = new List<GPSStopScheDule>();
            _allControlPointList = new List<GPSFence>();
            trafficeServer = new TrafficRepository();

            Thread loadThread = new Thread(new ThreadStart(loadData));
            loadThread.Start();
            //loadData();
        }
        /// <summary>
        /// 加载需要计算的数据到内存中，包括监控点、围栏、线路、行使计划以及超速道路
        /// </summary>
        private void loadData()
        {
            try
            {
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["GenerateFenceAlert"]))
                {
                    LoggerManager.Logger.Info(" LoadFences Started:" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());                    
                    if (LoadAllFences())
                    {
                        LoggerManager.Logger.Info(" LoadFences Successed  End:" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());
                    }
                    else
                    {
                        LoggerManager.Logger.Info(" LoadFences Failed End:" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());
                    }
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["GenerateRouteAlert"]))
                {
                    LoggerManager.Logger.Info(" LoadRoutes Started:" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());
                    if (LoadAllRoutes())
                    {
                        LoggerManager.Logger.Info(" LoadRoutes Successed End:" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());
                    }
                    else
                    {
                        LoggerManager.Logger.Info(" LoadRoutes Failed End:" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());
                    }
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["GenerateRoadAlert"]))
                {
                    LoggerManager.Logger.Info(" LoadRoads Started:" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());
                    if (LoadAllRoads())
                    {
                        LoggerManager.Logger.Info(" LoadRoads Successed End:" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());
                    }
                    else
                    {
                        LoggerManager.Logger.Info(" LoadRoads Failed End:" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());
                    }
                }
                if(Convert.ToBoolean(ConfigurationManager.AppSettings["GenerateMonitorPointAlert"]))
                {
                    LoggerManager.Logger.Info(" LoadMonitorPoint Started:"+DateTime.Now.ToString()+""+DateTime.Now.Millisecond.ToString());
                    if (LoadAllMonitorPoint())
                    {
                        LoggerManager.Logger.Info("LoadMonitorPoint Successed End:" + DateTime.Now.ToString() + "" + DateTime.Now.Millisecond.ToString());
                    }
                    else
                    {
                        LoggerManager.Logger.Info("LoadMonitorPoint Failed End:" + DateTime.Now.ToString() + "" + DateTime.Now.Millisecond.ToString());
                    }
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["GeneratePlanAlert"]))
                {
                    LoggerManager.Logger.Info(" LoadPlans Started:" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());
                    if (LoadAllPlans())
                    {
                        LoggerManager.Logger.Info(" LoadPlans Successed End:" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());
                    }
                    else
                    {
                        LoggerManager.Logger.Info(" LoadPlans Failed End:" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());
                    }
                }
                PollingTravelPlan();
                MonitorAlertMessage.DequeueMessage();
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }              
        }

        private bool LoadAllMonitorPoint()
        {
            try
            {
                _allControlPointList = trafficeServer.GetGPSControlPoint();
                if (_allControlPointList.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return false;
            }           
        }

        //[MethodImpl(MethodImplOptions.Synchronized)]
        private void PollingTravelPlan()
        {
            System.Threading.Thread pollthread = new System.Threading.Thread(ExeTravelPlan);
            pollthread.Start();            
        }

        
        private void ExeTravelPlan()
        {
            try
            {
                MonitorPlan tempPlan = new MonitorPlan();
                DateTime dtNow = new DateTime();                
                
                List<GPSStopScheDulePoint> _pointList=new List<GPSStopScheDulePoint>();
                while (true)
                {                  
                    double tolerance=0.0;

                    //判断执行条件                   
                    for (int i = 0; i < _allDuleList.Count; i++)
                    {
                        GPSStopScheDule gpsdule = _allDuleList[i];
                        dtNow = System.DateTime.Now;
                        if (gpsdule.Tolerance != null)  tolerance = (double)gpsdule.Tolerance;
                        

                        if (gpsdule.SCHEDULE_ID != null)
                        {
                            //_pointList = trafficeServer.GetGPSStopScheDulePointList(gpsdule.SCHEDULE_ID);
                        }

                        gpsdule.Start_Time = DateTime.Parse(_pointList[0].Due_Time);
                        gpsdule.End_Time = DateTime.Parse(_pointList[_pointList.Count - 1].Due_Time);
                      
                        DateTime stime = DateTime.Parse(dtNow.Date.ToShortDateString() + " " + DateTime.Parse(gpsdule.Start_Time.ToString()).ToLongTimeString());
                        DateTime etime = DateTime.Parse(dtNow.Date.ToShortDateString() + " " + DateTime.Parse(gpsdule.End_Time.ToString()).ToLongTimeString());
                       

                        //判断开始执行行使计划的条件:1.开始日期<当前日期 结束时间>当前日期 2第一个预计到达时间-偏差）<当前时间 3当前的日期的是否在周期列表里4该计划是否在执行
                        if (((DateTime.Parse(gpsdule.Begin_date.ToString()).Date <= dtNow.Date) && (DateTime.Parse(gpsdule.End_date.ToString()).Date >= dtNow.Date))
                            && (stime.AddSeconds(-tolerance) <= dtNow) && (stime.AddSeconds(tolerance) >= dtNow)
                            && (IsOnWeekDay(gpsdule.WeekDay, dtNow)) && (gpsdule.ANTGPSID != null)
                            && (!MonitorAlertGenerator.AntGPSPlanHashtable.ContainsKey(gpsdule.ANTGPSID))
                            && (gpsdule.Status==1) &&(gpsdule.Valid==1))
                        {                            
                            tempPlan._planId = gpsdule.SCHEDULE_ID;
                            tempPlan._planName = gpsdule.Name;
                            tempPlan._antGpsID = gpsdule.ANTGPSID;
                            tempPlan._PlanPointList = _pointList;
                            tempPlan._vehicleID = gpsdule.Vechile_ID;
                            tempPlan._toleranceTime = (double)gpsdule.Tolerance;
                            tempPlan._radius = (double)gpsdule.Radius;

                            tempPlan._startTime = DateTime.Parse(gpsdule.Start_Time.ToString());
                            tempPlan._endTime = DateTime.Parse(gpsdule.End_Time.ToString());
                            tempPlan._isValid = false;

                            lock (lockplan)
                            {
                                MonitorAlertGenerator.AntGPSPlanHashtable.Add(gpsdule.ANTGPSID, tempPlan);
                            }
                        }
                    }

                    object [] keys=new object[MonitorAlertGenerator.AntGPSPlanHashtable.Keys.Count];
                    MonitorAlertGenerator.AntGPSPlanHashtable.Keys.CopyTo(keys, 0);

                    for (int i = 0; i < keys.Count(); i++)
                    {
                        tempPlan = MonitorAlertGenerator.AntGPSPlanHashtable[keys[i]] as MonitorPlan;
                        dtNow = System.DateTime.Now;
                        if ((tempPlan._startTime.AddSeconds(tempPlan._toleranceTime) < dtNow) && (tempPlan._isValid == false))//GPS数据没有上报，行使计划没有执行
                        {
                            lock (lockplan)
                            {
                                TravelPlanResult model = new TravelPlanResult();

                                model.ID = Guid.NewGuid().ToString();
                                model.SCHEDULE_ID = tempPlan._planId;
                                model.NAME = tempPlan._planName;
                                model.STATE = 2;
                                model.VEHICLE_ID = tempPlan._vehicleID;
                                model.START_TIME = dtNow;
                                model.START_ALERT_ID = Guid.NewGuid().ToString();
                                model.END_TIME = dtNow;
                                model.END_ALERT_ID = Guid.NewGuid().ToString();

                                //trafficeServer.AddTravelPlanResult(model);
                                AntGPSLocation temp = new AntGPSLocation();
                                temp.GenerateTravelPlanCancelAlert(tempPlan);

                                MonitorAlertGenerator.AntGPSPlanHashtable.Remove(keys[i]);
                            }
                        }
                        if ((tempPlan._endTime.AddSeconds(tempPlan._toleranceTime+600 ) < dtNow) && (tempPlan._isValid == true))//异常结束行驶计划的情况
                        {
                            lock (lockplan)
                            {
                                TravelPlanResult model = new TravelPlanResult();
                                model.ID = tempPlan._planGuid;
                                model.SCHEDULE_ID = tempPlan._planId;
                                model.NAME = tempPlan._planName;
                                model.STATE = 0;
                                model.VEHICLE_ID = tempPlan._vehicleID;
                                model.START_TIME = tempPlan._startTime;
                                model.START_ALERT_ID = tempPlan._start_alert_id;
                                model.END_TIME = dtNow;
                                model.END_ALERT_ID = Guid.NewGuid().ToString();
                                //trafficeServer.UpdateTravelPlanResult(model);
                                AntGPSLocation temp = new AntGPSLocation();

                                temp.GenerateTravelPlanEndAlert(tempPlan);

                                MonitorAlertGenerator.AntGPSPlanHashtable.Remove(keys[i]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        private bool IsOnWeekDay(string p, DateTime dtNow)
        {
            try
            {
                string weekday = ((int)dtNow.DayOfWeek).ToString();
                if (p.Contains(weekday))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex.Message);
                return false;
            }
        }        

        public bool LoadAllPlans()
        {
            try
            {
                ////加载所有行使计划
                //_allDuleList = trafficeServer.GetGPSStopScheDuleList();

                //if (_allDuleList.Count > 0)
                //    return true;
                //else
                    return false;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("LoadAllPlans :" + ex.Message);
                return false;
            }            
        }

        /// <summary>
        /// 获取限速道路的数量
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        private int GetCount(string Url)
        {
            int Result = 0;
            var webClient = new System.Net.WebClient();
            try
            {
                webClient.Proxy = null;
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers["Content-Type"] = "application/json";
                string result = webClient.DownloadString(new System.Uri(Url));
                var jss = new JavaScriptSerializer();
                Dictionary<string, object> arcGisResult = jss.DeserializeObject(result) as Dictionary<string, object>;
                object[] obj = arcGisResult["features"] as object[];
                foreach (Dictionary<string, object> dict in obj)
                {
                    Dictionary<string, object> roadinfo = dict["attributes"] as Dictionary<string, object>;
                    Result = (int)roadinfo["OBJECTID"];
                    break;
                }
                return Result;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("LoadCounts :" + Url + ex.Message);
                return 0;
            }
            finally
            {
                webClient.Dispose();
            }
        }

        /// <summary>
        /// 加载所有的道路
        /// </summary>
        public bool LoadAllRoads()
        {
            try
            {
                int maxobjectid = GetCount(HttpUtility.HtmlDecode(ConfigHelper.RoadCountUrl));
                string addressUrl = HttpUtility.HtmlDecode(ConfigHelper.RoadDataUrl);
                for (int i = 1; i <= maxobjectid; i = i + ReadMAXCountOnce)
                {
                    string addUrl = addressUrl.Replace("{0}", i.ToString());
                    addUrl = addUrl.Replace("{1}", (i + ReadMAXCountOnce).ToString());
                    LoadRoads(addUrl);
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("LoadAllRoads :" + ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 加载道路
        /// </summary>
        /// <param name="addressUrl"></param>
        public void LoadRoads(string addressUrl)
        {
            var webClient = new System.Net.WebClient();
            try
            {
                webClient.Proxy = null;
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers["Content-Type"] = "application/json";
                string result = webClient.DownloadString(new System.Uri(addressUrl));
                var jss = new JavaScriptSerializer();
                Dictionary<string, object> arcGisResult = jss.DeserializeObject(result) as Dictionary<string, object>;
                object[] obj = arcGisResult["features"] as object[];
                if ((obj != null) && (obj.Length > 0))
                {
                    foreach (Dictionary<string, object> dict in obj)
                    {
                        Dictionary<string, object> roadinfo = dict["attributes"] as Dictionary<string, object>;
                        Dictionary<string, object> geometry = dict["geometry"] as Dictionary<string, object>;
                        object[] paths = geometry["paths"] as object[];
                        object[] nodewhole = paths[0] as object[];
                        List<Gsafety.PTMS.MonitorAlert.Road.Node> Nodelist = new List<Gsafety.PTMS.MonitorAlert.Road.Node>();
                        foreach (object[] item in nodewhole)
                        {
                            Gsafety.PTMS.MonitorAlert.Road.Node node = new Gsafety.PTMS.MonitorAlert.Road.Node();
                            node.longitude = item[0].ToString();
                            node.latitude = item[1].ToString();
                            Nodelist.Add(node);
                        }

                        int limitedspeed = 0;
                        if (roadinfo["LIMITEDSPEED"] != null) limitedspeed = (int)roadinfo["LIMITEDSPEED"];
                        string name = "";
                        if (roadinfo["NAME"] != null) name = roadinfo["NAME"].ToString();

                        Road road = new Road(roadinfo["OBJECTID"].ToString(), name, limitedspeed, Nodelist, RoadBuffer);

                        Hashtable ht = road.Buffer.GetIntersectsCells();
                        foreach (DictionaryEntry de in ht)
                        {
                            if (_GridCellsRoad.ContainsKey(de.Key.ToString()))
                            {
                                if (!(_GridCellsRoad[de.Key.ToString()] as MonitorGridCell).GeometryList.ContainsKey(road.Id))
                                {
                                    (_GridCellsRoad[de.Key.ToString()] as MonitorGridCell).GeometryList.Add(road.Id, road);
                                }
                            }
                            else
                            {
                                MonitorGridCell cell = new MonitorGridCell();
                                cell.GeometryList.Add(road.Id, road);
                                _GridCellsRoad.Add(de.Key.ToString(), cell);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("LoadRoads :" + ex.Message);
            }
            finally
            {
                webClient.Dispose();
            }
        }
        /// <summary>
        /// 加载所有的线路
        /// </summary>
        public bool LoadAllRoutes()
        {
            try
            {
                int maxobjectid = GetCount(HttpUtility.HtmlDecode(ConfigHelper.RouteCountUrl)); ;
                string addressUrl = HttpUtility.HtmlDecode(ConfigHelper.RouteDataUrl);
                LoggerManager.Logger.Info("LoadRoutes :" + addressUrl);
                List<GPSRout> _allRoute = trafficeServer.GetGPSRoute();
                LoggerManager.Logger.Info("LoadRoutes :" + _allRoute.Count.ToString());
                for (int i = 1; i <= maxobjectid; i = i + ReadMAXCountOnce)
                {
                    string addUrl = addressUrl.Replace("{0}", i.ToString());
                    addUrl = addUrl.Replace("{1}", (i + ReadMAXCountOnce).ToString());
                    LoggerManager.Logger.Info("LoadRoutes :" + addUrl);
                    LoadRoutes(addUrl, _allRoute);
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("LoadRoutes :" + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 加载线路
        /// </summary>
        /// <param name="addressUrl"></param>
        /// <param name="allRoute"></param>
        public void LoadRoutes(string addressUrl, List<GPSRout> allRoute)
        {
            var webClient = new System.Net.WebClient();
            try
            {
                webClient.Proxy = null;
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers["Content-Type"] = "application/json";
                string result = webClient.DownloadString(new System.Uri(addressUrl));
                var jss = new JavaScriptSerializer();
                Dictionary<string, object> arcGisResult = jss.DeserializeObject(result) as Dictionary<string, object>;
                object[] obj = arcGisResult["features"] as object[];

                if ((obj != null) && (obj.Length > 0))
                {
                    foreach (Dictionary<string, object> dict in obj)
                    {
                        Dictionary<string, object> routeinfo = dict["attributes"] as Dictionary<string, object>;
                        Dictionary<string, object> geometry = dict["geometry"] as Dictionary<string, object>;
                        object[] paths = geometry["paths"] as object[];
                        object[] nodewhole = paths[0] as object[];
                        List<Gsafety.PTMS.MonitorAlert.Road.Node> Nodelist = new List<Gsafety.PTMS.MonitorAlert.Road.Node>();
                        foreach (object[] item in nodewhole)
                        {
                            Gsafety.PTMS.MonitorAlert.Road.Node node = new Gsafety.PTMS.MonitorAlert.Road.Node();
                            node.longitude = item[0].ToString();
                            node.latitude = item[1].ToString();
                            Nodelist.Add(node);
                        }
                        Route route = new Route(routeinfo["OBJECTID"].ToString(), routeinfo["ROUTE_NAME"].ToString(), Nodelist, RouteBuffer);
                        /***************************************************************/
                        foreach (var item in allRoute.Where(r => r.RoutID == System.Convert.ToDecimal(route.Id)))
                        {
                            if (item.GPSID == null)
                                continue;
                            if (!_GridCellsRoute.ContainsKey(item.GPSID))
                            {
                                MonitorGridCell cell = new MonitorGridCell();
                                cell.GeometryList.Add(item.RoutID, route);
                                _GridCellsRoute.Add(item.GPSID, cell);
                            }
                            else
                            {
                                if (!((MonitorGridCell)_GridCellsRoute[item.GPSID]).GeometryList.ContainsKey(item.RoutID))
                                {
                                    ((MonitorGridCell)_GridCellsRoute[item.GPSID]).GeometryList.Add(item.RoutID, route);
                                }
                            }
                        }
                        /********************************************************************/
                    }

                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("LoadRoutes :" + ex.Message);
            }
            finally
            {
                webClient.Dispose();
            }

        }
        /// <summary>
        /// 加载所有的围栏
        /// </summary>
        public bool LoadAllFences()
        {
            try
            {
                int maxobjectid = GetCount(HttpUtility.HtmlDecode(ConfigHelper.FenceCountUrl)); ;
                string addressUrl = HttpUtility.HtmlDecode(ConfigHelper.FenceDataUrl);
                List<GPSFence> allFence = trafficeServer.GetGPSFence();
                for (int i = 1; i <= maxobjectid; i = i + ReadMAXCountOnce)
                {
                    string addUrl = addressUrl.Replace("{0}", i.ToString());
                    addUrl = addUrl.Replace("{1}", (i + ReadMAXCountOnce).ToString());
                    LoadFences(addUrl, allFence);
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("LoadAllFences :" + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 加载围栏
        /// </summary>
        /// <param name="addressUrl"></param>
        /// <param name="allFence"></param>
        public void LoadFences(string addressUrl, List<GPSFence> allFence)
        {
            var webClient = new System.Net.WebClient();
            try
            {
                webClient.Proxy = null;
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers["Content-Type"] = "application/json";
                string result = webClient.DownloadString(new System.Uri(addressUrl));
                var jss = new JavaScriptSerializer();
                Dictionary<string, object> arcGisResult = jss.DeserializeObject(result) as Dictionary<string, object>;
                object[] obj = arcGisResult["features"] as object[];
                if ((obj != null) && (obj.Length > 0))
                {
                    foreach (Dictionary<string, object> dict in obj)
                    {
                        Dictionary<string, object> fenceinfo = dict["attributes"] as Dictionary<string, object>;
                        Dictionary<string, object> geometry = dict["geometry"] as Dictionary<string, object>;
                        object[] paths = geometry["rings"] as object[];
                        object[] nodewhole = paths[0] as object[];
                        List<Gsafety.PTMS.MonitorAlert.Road.Node> Nodelist = new List<Gsafety.PTMS.MonitorAlert.Road.Node>();
                        foreach (object[] item in nodewhole)
                        {
                            Gsafety.PTMS.MonitorAlert.Road.Node node = new Gsafety.PTMS.MonitorAlert.Road.Node();
                            node.longitude = item[0].ToString();
                            node.latitude = item[1].ToString();
                            Nodelist.Add(node);
                        }
                        string speed_limited = "";
                        if (fenceinfo["SPEED_LIMIT"] != null) speed_limited = fenceinfo["SPEED_LIMIT"].ToString();
                        string name = "";
                        if (fenceinfo["NAME"] != null) name = fenceinfo["NAME"].ToString();

                        int fencetype = 2;
                        if (fenceinfo["ALERT_TYPE"] != null) fencetype = int.Parse(fenceinfo["ALERT_TYPE"].ToString());
                        string timeLimit = "";
                        if (fenceinfo["TIME_LIMIT"] != null) timeLimit = fenceinfo["TIME_LIMIT"].ToString();

                        Fence fence = new Fence(fenceinfo["OBJECTID"].ToString(), name, speed_limited, fencetype, 0, timeLimit, Nodelist, FenceBuffer);
                        /***************************************************************/
                        foreach (var item in allFence.Where(f => f.FenceID == System.Convert.ToDecimal(fence.Id)))
                        {
                            if (!_GridCellsFence.ContainsKey(item.GPSID))
                            {
                                MonitorGridCell cell = new MonitorGridCell();
                                cell.GeometryList.Add(item.FenceID, fence);
                                _GridCellsFence.Add(item.GPSID, cell);
                            }
                            else
                            {
                                if (!((MonitorGridCell)_GridCellsFence[item.GPSID]).GeometryList.ContainsKey(item.FenceID))
                                {
                                    ((MonitorGridCell)_GridCellsFence[item.GPSID]).GeometryList.Add(item.FenceID, fence);
                                }
                            }
                        }
                        /********************************************************************/
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("LoadFences :" + ex.Message);
            }
            finally
            {
                webClient.Dispose();
            }
        }

        /// <summary>
        /// 依据道路所在的网格坐标获取网格对象
        /// </summary>
        /// <param name="cellid"></param>
        /// <returns></returns>
        public MonitorGridCell GetRoadMonitorCell(string cellid)
        {
            if (_GridCellsRoad.ContainsKey(cellid)) return (_GridCellsRoad[cellid] as MonitorGridCell);
            return null;
        }

        /// <summary>
        /// 依据GPS号获取路线所在的网格对象
        /// </summary>
        /// <param name="AntGPSID"></param>
        /// <returns></returns>
        public MonitorGridCell GetRouteMonitorCell(string AntGPSID)
        {
            if (_GridCellsRoute.ContainsKey(AntGPSID)) return (_GridCellsRoute[AntGPSID] as MonitorGridCell);
            return null;
        }

        /// <summary>
        /// 依据GPS号获取所在的围栏的网格对象
        /// </summary>
        /// <param name="AntGPSID"></param>
        /// <returns></returns>
        public MonitorGridCell GetFenceMonitorCell(string AntGPSID)
        {
            if (_GridCellsFence.ContainsKey(AntGPSID)) return (_GridCellsFence[AntGPSID] as MonitorGridCell);
            return null;
        }
        /// <summary>
        /// 更新线路
        /// </summary>
        /// <param name="gpsid"></param>
        /// <param name="mdvrcoreid"></param>
        /// <param name="model"></param>
        public void UpdateRoute(string gpsid, string mdvrcoreid, RouteCMD model)
        {
            try
            {
                if (model.OperType == 1)
                {//新增线路
                    List<Gsafety.PTMS.MonitorAlert.Road.Node> NodeLst = GetNodeList(model.Route);
                    string routename = "";
                    if (model.RouteName != null) routename = model.RouteName;

                    Route route = new Route(model.RouteId, routename, NodeLst, RouteBuffer);
                    if (!_GridCellsRoute.ContainsKey(gpsid))
                    {
                        MonitorGridCell cell = new MonitorGridCell();
                        cell.GeometryList.Add(System.Convert.ToDecimal(model.RouteId), route);
                        _GridCellsRoute.Add(gpsid, cell);
                    }
                    else
                    {
                        if (!((MonitorGridCell)_GridCellsRoute[gpsid]).GeometryList.ContainsKey(System.Convert.ToDecimal(model.RouteId)))
                        {
                            ((MonitorGridCell)_GridCellsRoute[gpsid]).GeometryList.Add(System.Convert.ToDecimal(model.RouteId), route);
                        }
                    }                   
                }
                else if (model.OperType == 2)
                {//修改线路
                    if (_GridCellsRoute.Contains(gpsid))
                    {
                        if ((_GridCellsRoute[gpsid] as MonitorGridCell).GeometryList.Contains(System.Convert.ToDecimal(model.RouteId)))
                        {
                            List<Gsafety.PTMS.MonitorAlert.Road.Node> NodeLst = GetNodeList(model.Route);
                            string routename = "";
                            if (model.RouteName != null) routename = model.RouteName;

                            Route route = new Route(model.RouteId, routename, NodeLst, RouteBuffer);
                            (_GridCellsRoute[gpsid] as MonitorGridCell).GeometryList[model.RouteId] = route;
                        }
                    }
                }
                else if (model.OperType == 3)
                {//删除线路
                    if (_GridCellsRoute.Contains(gpsid))
                    {
                        if ((_GridCellsRoute[gpsid] as MonitorGridCell).GeometryList.Contains(System.Convert.ToDecimal(model.RouteId)))
                        {
                            (_GridCellsRoute[gpsid] as MonitorGridCell).GeometryList.Remove(System.Convert.ToDecimal(model.RouteId));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //日志
                LoggerManager.Logger.Error(ex);
            }


        }
        public void UpdatePoint(ElectricFenceCMD model)
        {
            ElectricFenceReply ef = new ElectricFenceReply();
            bool isExist = false;
            string mdvrcoreid = model.DvId;
            try
            {              
                ef.MdvrCoreId = model.DvId;
                ef.GpsValid = "N";
                ef.OriginalCmd = model.CmType;
                ef.ReplyType = 1;
                ef.ReplyResult = 0;
                ef.OriginalTime = model.SendTime;
                ef.AreaType = AreaType.MonitoringPoint;

                if (model.OperType == 1)
                {//新增控制点
                    foreach (GPSFence cpoint in _allControlPointList)
                    {
                        if ((cpoint.FenceID == model.FenceId) && (cpoint.MDVR_CORE_SN == model.DvId))
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        GPSFence temp = new GPSFence();

                        temp.FenceID = model.FenceId;
                        temp.FenceName = model.FenceName;
                        temp.MDVR_CORE_SN = model.DvId;
                        temp.TimeLimit = model.ValidTime;
                        temp.FENCE_TYPE = 1;

                        //取监控点信息
                        GPSFence gpsfence = trafficeServer.GetGPSControlPointByPointID(temp.FenceID, model.DvId);
                        if (gpsfence != null)
                        {
                            temp.CircleCenter = gpsfence.CircleCenter;
                            temp.RAIDUMS = gpsfence.RAIDUMS;
                            temp.VEHICLE_ID = gpsfence.VEHICLE_ID;
                            temp.GPSID = gpsfence.GPSID;
                            if (temp.GPSID == null)
                            {
                                ef.ReplyResult = 0;
                            }
                            else
                            {
                                ef.ReplyResult = 1;
                                temp.IsInFence = false;
                                lock (lockpoint)
                                {
                                    _allControlPointList.Add(temp);
                                }
                            }                          
                        }
                        else
                        {                           
                            ef.ReplyResult = 0;
                            LoggerManager.Logger.Error("Get controloPointinfo faild" + model.FenceId + "&&" + model.DvId, new Exception());                          
                        }
                    }                   
                }
                else if (model.OperType == 2)
                {//修改控制点
                    foreach (GPSFence cpoint in _allControlPointList)
                    {
                        if ((cpoint.FenceID == model.FenceId) && (cpoint.MDVR_CORE_SN == model.DvId))
                        {                            
                            cpoint.FenceName = model.FenceName;
                            cpoint.TimeLimit = model.ValidTime;
                            GPSFence gpsfence = trafficeServer.GetGPSControlPointByPointID(cpoint.FenceID, model.DvId);
                            if (gpsfence != null)
                            {
                                cpoint.CircleCenter = gpsfence.CircleCenter;
                                cpoint.RAIDUMS = gpsfence.RAIDUMS;
                                cpoint.VEHICLE_ID = gpsfence.VEHICLE_ID;
                                //cpoint.GPSID = gpsfence.GPSID;
                                ef.ReplyResult = 1;
                            }
                            else
                            {
                                ef.ReplyResult = 0;
                                LoggerManager.Logger.Error("Get controloPointinfo faild" + model.FenceId + "&&" + model.DvId, new Exception());
                                //return;
                            }                                               
                            break;
                        }
                    }                    
                }
                else if (model.OperType == 3)
                {//删除控制点                    
                    for (int i = 0; i < _allControlPointList.Count; i++)
                    {
                        if (_allControlPointList[i].FenceID == model.FenceId && _allControlPointList[i].MDVR_CORE_SN == model.DvId)
                        {
                            lock (lockpoint)
                            {
                                _allControlPointList.RemoveAt(i);
                            }                         
                            ef.ReplyResult = 1;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
          
            MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, ReplyRoute.OriginalFenceyBusinessReplyKey + mdvrcoreid, MonitorAlertMessage.ObjectToBytes(ef));
            LoggerManager.Logger.Info("updatepoint reply");
        }

        /// <summary>
        /// 更新围栏在内存中的信息
        /// </summary>
        /// <param name="gpsid"></param>
        /// <param name="mdvrcoreid"></param>
        /// <param name="model"></param>
        public void UpdateFence(string gpsid, ElectricFenceCMD model)
        {
            ElectricFenceReply ef = new ElectricFenceReply();
            try
            {
                ef.MdvrCoreId = model.DvId;
                ef.GpsValid = "N";
                ef.OriginalCmd = model.CmType;
                ef.ReplyType = 1;
                ef.ReplyResult = 0;
                ef.OriginalTime = model.SendTime;
                ef.AreaType = AreaType.ElectronicFence;

                if (gpsid == "")
                {
                    ef.ReplyResult = 0;
                    MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, ReplyRoute.OriginalFenceyBusinessReplyKey + model.DvId, MonitorAlertMessage.ObjectToBytes(ef));
                    LoggerManager.Logger.Info("updatefence reply");
                    return;
                }

                if (model.OperType == 1)
                {//新增电子围栏
                    List<Gsafety.PTMS.MonitorAlert.Road.Node> NodeLst = GetNodeList(model.Fence);
                    string fencename = "";
                    if (model.FenceName != null) fencename = model.FenceName;

                    Fence fence = new Fence(model.FenceId.ToString(), fencename, model.Speed, -1, model.Action, model.ValidTime, NodeLst, 0);
                    if (!_GridCellsFence.ContainsKey(gpsid))
                    {
                        MonitorGridCell cell = new MonitorGridCell();
                        cell.GeometryList.Add(System.Convert.ToDecimal(model.FenceId), fence);
                        _GridCellsFence.Add(gpsid, cell);
                       
                        ef.ReplyResult = 1;
                    }
                    else
                    {
                        if (!((MonitorGridCell)_GridCellsFence[gpsid]).GeometryList.ContainsKey(System.Convert.ToDecimal(model.FenceId)))
                        {
                            ((MonitorGridCell)_GridCellsFence[gpsid]).GeometryList.Add(System.Convert.ToDecimal(model.FenceId), fence);
                          
                            ef.ReplyResult = 1;
                        }
                    }                 
                }
                else if (model.OperType == 2)
                {//修改电子围栏
                    if (_GridCellsFence.Contains(gpsid))
                    {
                        if ((_GridCellsFence[gpsid] as MonitorGridCell).GeometryList.Contains(System.Convert.ToDecimal(model.FenceId)))
                        {
                            List<Gsafety.PTMS.MonitorAlert.Road.Node> NodeLst = GetNodeList(model.Fence);
                            string fencename = "";
                            if (model.FenceName != null) fencename = model.FenceName;

                            Fence fence = new Fence(model.FenceId.ToString(), fencename, model.Speed, -1, model.Action, model.ValidTime, NodeLst, 0);
                            (_GridCellsFence[gpsid] as MonitorGridCell).GeometryList[model.FenceId] = fence;
                           
                            ef.ReplyResult = 1;
                        }
                    }
                }
                else if (model.OperType == 3)
                {//删除电子围栏
                    if (_GridCellsFence.Contains(gpsid))
                    {
                        if ((_GridCellsFence[gpsid] as MonitorGridCell).GeometryList.Contains(System.Convert.ToDecimal(model.FenceId)))
                        {
                            (_GridCellsFence[gpsid] as MonitorGridCell).GeometryList.Remove(System.Convert.ToDecimal(model.FenceId));
                            ef.ReplyResult = 1;
                        }                        
                    }                
                }
            }
            catch (Exception ex)
            {
                ef.ReplyResult = 0;
                //日志
                LoggerManager.Logger.Error(ex);
            }
           
            MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, ReplyRoute.OriginalFenceyBusinessReplyKey + model.DvId, MonitorAlertMessage.ObjectToBytes(ef));
            LoggerManager.Logger.Info("updatefence reply");
        }

        /// <summary>
        /// 更新行驶计划的信息
        /// </summary>
        /// <param name="vechileID"></param>
        /// <param name="model"></param>
        public void UpdatePlan(string vechileID, TravelPlanCMD model)
        {
            int index = -1;
            GPSStopScheDule gs = new GPSStopScheDule();
            try
            {
                if (model.OperType == 1)//新增行使计划
                {                   
                    gs.ANTGPSID = model.RaptorIMEI;
                    gs.Begin_date = model.BeginDate;
                    gs.End_date = model.EndDate;
                    gs.Radius = model.Radius;                   
                    gs.SCHEDULE_ID = model.ScheduleID;                   
                    gs.Tolerance = model.Tolerance;
                    gs.Status = 1;
                    gs.Valid = 1;
                    gs.Vechile_ID = model.VechileID;
                    gs.WeekDay = model.WeekDay;
                   
                    _allDuleList.Add(gs);
                }
                else if (model.OperType == 2)//修改行使计划
                {                 
                    
                    if (vechileID != null)
                    {
                        foreach (GPSStopScheDule es in _allDuleList)
                        {
                            if ((es.Vechile_ID == model.VechileID) && (es.SCHEDULE_ID == model.ScheduleID))
                            {
                                index = _allDuleList.IndexOf(es);
                                //List<GPSStopScheDulePoint> temppoint = trafficeServer.GetGPSStopScheDulePointList(model.ScheduleID);
                                //_allDuleList[index].ANTGPSID = model.RaptorIMEI;
                                _allDuleList[index].Valid = 1;
                                _allDuleList[index].Status = 1;
                                _allDuleList[index].Begin_date = model.BeginDate;
                                _allDuleList[index].End_date = model.EndDate;
                                _allDuleList[index].Radius = model.Radius;
                                _allDuleList[index].Tolerance = model.Tolerance;
                                _allDuleList[index].WeekDay = model.WeekDay;
                               
                                break;
                            }
                        }
                    }               
                }
                else if (model.OperType == 3)//删除行使计划
                {                
                    for(int i=0;i<_allDuleList.Count;i++) 
                    {
                        if (_allDuleList[i] != null)
                        {
                            if ((_allDuleList[i].Vechile_ID == model.VechileID) && (_allDuleList[i].SCHEDULE_ID == model.ScheduleID))
                            {
                                _allDuleList.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    //_allDuleList.RemoveAt(index);
                }
            }
            catch (Exception ex)
            {
                //日志
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// 将消息转为
        /// </summary> 
        /// <param name="cmdstr"></param>
        /// <returns></returns>
        private List<Road.Node> GetNodeList(string cmdstr)
        {
            List<Gsafety.PTMS.MonitorAlert.Road.Node> Nodelist = new List<Gsafety.PTMS.MonitorAlert.Road.Node>();
            string temp = cmdstr.Replace("E", ";").Replace("W", ";-").Replace("N", ",").Replace("S", ",-");
            string[] nodestrArr = temp.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string nodeone in nodestrArr)
            {
                Gsafety.PTMS.MonitorAlert.Road.Node node = new Gsafety.PTMS.MonitorAlert.Road.Node();
                string[] item = nodeone.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                node.longitude = LonLatStrToDouble(item[0].ToString()).ToString();
                node.latitude = LonLatStrToDouble(item[1].ToString()).ToString();
                Nodelist.Add(node);
            }
            return Nodelist;
        }

        private double LonLatStrToDouble(string value)
        {

            //string temp = value.ToString().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            int indflag = value.IndexOf("-");
            value = value.Replace("-", "");

            int du = 0;
            double fen = 0;

            int ind = value.IndexOf(".");
            if (ind == -1)
            {
                ind = value.Length;
            }

            if ((ind - 3 + 1) >= 1)
            {
                du = int.Parse(value.Substring(0, ind - 3 + 1));
                fen = double.Parse(value.Substring(ind - 2));
            }
            else
            {
                fen = double.Parse(value);
            }

            if (indflag > -1)
                return (-du - fen / 60);
            else
                return (du + fen / 60);
        }
    }

    /// <summary>
    /// 网格
    /// </summary>
    public class MonitorGridCell
    {
        private Hashtable _GeometryList = new Hashtable();
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 该风格中包含的道路列表
        /// </summary>
        public Hashtable GeometryList
        {
            get
            {
                return _GeometryList;
            }

        }

        /// <summary>
        /// 从网格中找到落入的对象
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public object GetGeometry(string lon, string lat)
        {

            foreach (DictionaryEntry de in _GeometryList)
            {
                if (de.Value is Road)
                {
                    if ((de.Value as Road).Buffer.IsPointIn(lon, lat)) return de.Value;
                }
                if (de.Value is Route)
                {
                    if ((de.Value as Route).Buffer.IsPointIn(lon, lat)) return de.Value;
                }
                if (de.Value is Fence)
                {
                    if ((de.Value as Fence).Buffer.IsPointIn(lon, lat)) return de.Value;
                }
            }
            return null;
        }
    }
}
