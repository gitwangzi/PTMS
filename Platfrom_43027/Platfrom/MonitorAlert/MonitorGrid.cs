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
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Manager.Repository;

namespace Gsafety.PTMS.MonitorAlert
{
    /// <summary>
    /// </summary>
    public class MonitorGrid
    {
        private Hashtable _GridCellsSpeed;
        private Hashtable _GridCellsFence;
        private Hashtable _GridCellsRoute;

        public Hashtable GridCellsSpeed
        {
            get
            {
                return _GridCellsSpeed;
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


        /// <summary>
        /// 初始化
        /// </summary>
        public MonitorGrid()
        {
           

            _GridCellsSpeed = new Hashtable(); 
            _GridCellsRoute = new Hashtable();
            _GridCellsFence = new Hashtable();
            

            Thread loadThread = new Thread(new ThreadStart(loadData));
            loadThread.Start();
          
        }
        /// <summary>
        /// 加载需要计算的数据到内存中，包括围栏、线路、以及超速
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

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["GenerateSpeedAlert"]))
                {
                    LoggerManager.Logger.Info(" LoadSpeeds Started:" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());
                    if (LoadAllSpeeds())
                    {
                        LoggerManager.Logger.Info(" LoadSpeeds Successed End:" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());
                    }
                    else
                    {
                        LoggerManager.Logger.Info(" LoadSpeeds Failed End:" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());
                    }
                }
                
                MonitorAlertMessage.DequeueMessage();
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

       

        /// <summary>
        /// 加载所有的超速
        /// </summary>
        public bool LoadAllSpeeds()
        {
            try
            {
                List<Gsafety.PTMS.Common.Data.SpeedLimit> allFence = SpeedLimitRepository.GetSpeedLimitList().Result.ToList();     
                return true;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("LoadAllRoads :" + ex.Message);
            }
            return false;
        }
       
        /// <summary>
        /// 加载所有的线路
        /// </summary>
        public bool LoadAllRoutes()
        {
            try
            {
                List<TrafficRoute> allRoute = TrafficRouteRepository.GetDeliveredTrafficRouteList().Result.ToList();
                LoadRoutes(allRoute);

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
        public void LoadRoutes( List<TrafficRoute> allRoute)
        {
            
            try
            {
                
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("LoadRoutes :" + ex.Message);
            }
           
        }
        /// <summary>
        /// 加载所有的围栏
        /// </summary>
        public bool LoadAllFences()
        {
            try
            {               
                List<TrafficFence> allFence = TrafficFenceRepository.GetDeliveredTrafficFenceList().Result.ToList();               
                LoadFences(allFence);                
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
        public void LoadFences(List<TrafficFence> allFence)
        {
           
            try
            {
               
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("LoadFences :" + ex.Message);
            }
           
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
                                   
                }
                else if (model.OperType == 2)
                {//修改线路
                   
                }
                else if (model.OperType == 3)
                {//删除线路
                    
                }
            }
            catch (Exception ex)
            {
                //日志
                LoggerManager.Logger.Error(ex);
            }

            //MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, ReplyRoute.OriginalFenceyBusinessReplyKey + model.DvId, MonitorAlertMessage.ObjectToBytes(ef));
            //LoggerManager.Logger.Info("updateroute reply");
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

                if (model.OperType == 1)
                {//新增电子围栏
                                
                }
                else if (model.OperType == 2)
                {//修改电子围栏
                   
                }
                else if (model.OperType == 3)
                {//删除电子围栏
                                 
                }
            }
            catch (Exception ex)
            {               
                //日志
                LoggerManager.Logger.Error(ex);
            }
           
            //MonitorAlertMessage.PublishMessage(Constdefine.APPEXCHANGE, ReplyRoute.OriginalFenceyBusinessReplyKey + model.DvId, MonitorAlertMessage.ObjectToBytes(ef));
            //LoggerManager.Logger.Info("updatefence reply");
        }

        /// <summary>
        /// 更新行驶计划的信息
        /// </summary>
        /// <param name="vechileID"></param>
        /// <param name="model"></param>
        public void UpdateSpeed(string vechileID, TravelPlanCMD model)
        {
           
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
