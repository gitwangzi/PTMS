using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.Traffic.Contract.Data;
using Gsafety.PTMS.Traffic.Repository;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.MonitorAlert
{
    class MonitorAlertPlan
    {
        private TrafficRepository trafficeServer;
        public MonitorAlertPlan()
        {
            trafficeServer = new TrafficRepository();     
        }

        /// <summary>
        /// 监控行使计划
        /// </summary>
        /// <param name="mPlan"></param>
        /// <param name="antgps"></param>
        /// <returns></returns>
        public Gsafety.PTMS.MonitorAlert.MonitorPlan.EXEStatus HandleANTGPSPlan(MonitorPlan mPlan, PTMSGPS antgps)
        {
            Gsafety.PTMS.MonitorAlert.MonitorPlan.EXEStatus _status = Gsafety.PTMS.MonitorAlert.MonitorPlan.EXEStatus.Initial;
            try
            {
                TravelPlanResult model = new TravelPlanResult();
                TravelDetail tdmodel = new TravelDetail();
                if (mPlan._isValid == false)
                {
                    if ((mPlan._startTime.AddSeconds(-mPlan._toleranceTime) <= antgps.GPSTime) && (mPlan._startTime.AddSeconds(mPlan._toleranceTime) >= antgps.GPSTime))
                    {
                        mPlan._isValid = true;
                        //mPlan._PlanPointList = trafficeServer.GetGPSStopScheDulePointList(mPlan._planId);

                        model.ID = Guid.NewGuid().ToString();
                        model.SCHEDULE_ID = mPlan._planId;
                        model.NAME = mPlan._planName;
                        model.START_TIME = antgps.GPSTime;
                        model.START_ALERT_ID = Guid.NewGuid().ToString();
                        model.STATE = 1;
                        model.VEHICLE_ID = mPlan._vehicleID;

                        mPlan._planGuid = model.ID;
                        mPlan._start_alert_id = model.START_ALERT_ID;
                        mPlan._actstartTime = DateTime.Parse(model.START_TIME.ToString());
                        //trafficeServer.AddTravelPlanResult(model);

                        //初始化计划点的状态
                        foreach (GPSStopScheDulePoint ppoint in mPlan._PlanPointList)
                        {
                            ppoint._isPunctuality = 0;                          
                            tdmodel.ID = Guid.NewGuid().ToString();
                            tdmodel.DUE_TIME = ppoint.Due_Time;
                            tdmodel.LOCATION = ppoint.Location;
                            tdmodel.LATITUDE = "";
                            tdmodel.LONGITUDE = "";
                            tdmodel.ARRIVAL_TIME = null;
                            tdmodel.POINT_NUM = ppoint.Point_NUM;
                            tdmodel.TRAVAL_ID = mPlan._planGuid;
                            tdmodel.ONTIME = 2;

                            ppoint.ID = tdmodel.ID;
                            //trafficeServer.AddTravelDetail(tdmodel);
                        }

                        _status = Gsafety.PTMS.MonitorAlert.MonitorPlan.EXEStatus.Start;
                    }
                }
                if (mPlan._isValid == true)
                {
                    foreach (GPSStopScheDulePoint ppoint in mPlan._PlanPointList)
                    {
                        DateTime dtNow = System.DateTime.Now;
                        if (IsPointInPlan(antgps.Longitude, antgps.Latitude, ppoint.PX, ppoint.PY, mPlan._radius))
                        {
                            if (ppoint._isPunctuality == 0)
                            {
                                if ((antgps.GPSTime >= DateTime.Parse(dtNow.Date.ToShortDateString() + " " + DateTime.Parse(ppoint.Due_Time).ToLongTimeString()).AddSeconds(-mPlan._toleranceTime)) &&
                                   (antgps.GPSTime <= DateTime.Parse(dtNow.Date.ToShortDateString() + " " + DateTime.Parse(ppoint.Due_Time).ToLongTimeString()).AddSeconds(mPlan._toleranceTime)))
                                {
                                    ppoint._isPunctuality = 1;
                                    tdmodel.ID = ppoint.ID;
                                    tdmodel.LATITUDE = antgps.Latitude;
                                    tdmodel.LONGITUDE = antgps.Longitude;
                                    tdmodel.LOCATION = ppoint.Location;
                                    tdmodel.ONTIME = 1;
                                    tdmodel.POINT_NUM = ppoint.Point_NUM;
                                    tdmodel.TRAVAL_ID = mPlan._planGuid;
                                    tdmodel.ARRIVAL_TIME = antgps.GPSTime;
                                    tdmodel.DUE_TIME = ppoint.Due_Time;
                                    //trafficeServer.UpdateTravelDetail(tdmodel);
                                }
                                else
                                {
                                    ppoint._isPunctuality = 2;
                                    tdmodel.ID = ppoint.ID;
                                    tdmodel.LATITUDE = antgps.Latitude;
                                    tdmodel.LONGITUDE = antgps.Longitude;
                                    tdmodel.LOCATION = ppoint.Location;
                                    tdmodel.ONTIME = 0;
                                    tdmodel.POINT_NUM = ppoint.Point_NUM;
                                    tdmodel.TRAVAL_ID = mPlan._planGuid;
                                    tdmodel.ARRIVAL_TIME = antgps.GPSTime;
                                    tdmodel.DUE_TIME = ppoint.Due_Time;
                                    //trafficeServer.UpdateTravelDetail(tdmodel);
                                }                                
                            }
                            else if (ppoint._isPunctuality == 1)
                            {
                                //continue;
                            }
                            else if (ppoint._isPunctuality == 2)
                            {
                                if ((antgps.GPSTime >= DateTime.Parse(dtNow.Date.ToShortDateString() + " " + DateTime.Parse(ppoint.Due_Time).ToLongTimeString()).AddSeconds(-mPlan._toleranceTime)) &&
                                    (antgps.GPSTime <= DateTime.Parse(dtNow.Date.ToShortDateString() + " " + DateTime.Parse(ppoint.Due_Time).ToLongTimeString()).AddSeconds(mPlan._toleranceTime)))
                                {
                                    ppoint._isPunctuality = 1;
                                    tdmodel.ID = ppoint.ID;
                                    tdmodel.LATITUDE = antgps.Latitude;
                                    tdmodel.LONGITUDE = antgps.Longitude;
                                    tdmodel.LOCATION = ppoint.Location;
                                    tdmodel.ONTIME = 1;
                                    tdmodel.POINT_NUM = ppoint.Point_NUM;
                                    tdmodel.TRAVAL_ID = mPlan._planGuid;
                                    tdmodel.ARRIVAL_TIME = antgps.GPSTime;
                                    tdmodel.DUE_TIME = ppoint.Due_Time;
                                    //trafficeServer.UpdateTravelDetail(tdmodel);
                                }
                            }
                        }               
                    }
                    if (System.DateTime.Now > mPlan._endTime.AddSeconds(mPlan._toleranceTime))//判断行驶计划是否结束
                    {
                        bool isOntime = true;
                        for (int i = 0; i < mPlan._PlanPointList.Count; i++)
                        {
                            if (mPlan._PlanPointList[i]._isPunctuality != 1)
                            {
                                isOntime = false;
                            }
                        }
                        if (isOntime == true)
                        {
                            model.ID = mPlan._planGuid;
                            model.NAME = mPlan._planName;
                            model.SCHEDULE_ID = mPlan._planId;
                            model.START_TIME = mPlan._actstartTime;
                            model.STATE = 1;
                            model.VEHICLE_ID = mPlan._vehicleID;
                            model.END_TIME = antgps.GPSTime;
                            model.END_ALERT_ID = Guid.NewGuid().ToString();
                            //trafficeServer.UpdateTravelPlanResult(model);
                        }
                        else
                        {
                            model.ID = mPlan._planGuid;
                            model.NAME = mPlan._planName;
                            model.SCHEDULE_ID = mPlan._planId;
                            model.START_TIME = mPlan._actstartTime;
                            model.STATE = 0;
                            model.VEHICLE_ID = mPlan._vehicleID;
                            model.END_TIME = antgps.GPSTime;
                            model.END_ALERT_ID = Guid.NewGuid().ToString();
                            //trafficeServer.UpdateTravelPlanResult(model);
                        }
                        mPlan._isValid = false;
                        _status = Gsafety.PTMS.MonitorAlert.MonitorPlan.EXEStatus.Finish;                     
                    }
                }                
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex.Message);
            }
            return _status;
        }

        /// <summary>
        /// 判断是否在计划点范围内
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public bool IsPointInPlan(string x1, string y1, string x2, string y2, double radius)
        {
            try
            {
                //采用地球大圆的算法
                SimplePoint ptGPS = GridCellCoord.GPS2GridCoord(x1, y1);
                double lon1 = ptGPS.X / 100;
                double lat1 = ptGPS.Y / 100;
                double lon2 = double.Parse(x2);
                double lat2 = double.Parse(y2);

                double EarthRadius = 6378.137;
                lon1 = lon1 / 180 * Math.PI;
                lon2 = lon2 / 180 * Math.PI;
                lat1 = lat1 / 180 * Math.PI;
                lat2 = lat2 / 180 * Math.PI;
                double x = Math.Sqrt(Math.Pow((Math.Sin((lat1 - lat2) / 2)), 2) +
                 Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin((lon1 - lon2) / 2), 2));
                if ((2 * Math.Asin(x) * EarthRadius * 1000) <= radius)
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

        /// <summary>
        /// 监控点告警
        /// </summary>
        /// <param name="mpoint"></param>
        /// <param name="antgps"></param>
        /// <returns></returns>
        public bool HandleControlPoint(GPSFence mpoint, PTMSGPS antgps)
        {

            bool IsAlertPoint = false;
            try
            {
                string gpstime = antgps.GPSTime.ToString("HH:mm:ss");
                string time = mpoint.TimeLimit;
                string StartTime = "00:00:00";
                string EndTime = "24:00:00";
                if ((time != null) && (time != ""))
                {
                    string[] temp = time.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    StartTime = "00:00".Substring(0, 5 - temp[0].Length) + temp[0] + ":00";
                    EndTime = "00:00".Substring(0, 5 - temp[1].Length) + temp[1] + ":00";
                }
                if ((String.Compare(StartTime, gpstime) <= 0) && (String.Compare(EndTime, gpstime) >= 0))
                {
                    string gpsloc = mpoint.CircleCenter;
                    if ((gpsloc != null) && (gpsloc != ""))
                    {
                        string[] tempgps = gpsloc.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        double gpsradius = Convert.ToDouble(mpoint.RAIDUMS.ToString());
                        if (IsPointInPlan(antgps.Longitude, antgps.Latitude, tempgps[0], tempgps[1], gpsradius))
                        {
                            if (mpoint.IsInFence == false)
                            {
                                mpoint.IsInFence = true;
                                IsAlertPoint = true;
                            }
                        }
                        else
                        {
                            mpoint.IsInFence = false;
                        }
                    }
                }
                else
                {
                    mpoint.IsInFence = false;
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex.Message);
            }
            return IsAlertPoint;
        }
    }
    

}
