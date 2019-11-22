using Gsafety.PTMS.Monitor.Contract.Data;
using Gsafety.Common.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Monitor.Repository;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Analysis.Helper;

namespace Gsafety.PTMS.Analysis.MonitorTreatment
{
    public static class VehicleOnOffTimeTreatment
    {
        private const double EarthRadius = 6378.137;
        static MonitorRepository monitorRepository;
        static RedisManager<VehicleOnOffTime> OnOffRedisClient;
        static RedisManager<VehicleDisplacement> VdRedisClient;
        static object lockobj = new object();
        //static Hashtable htInfo;

        static VehicleOnOffTimeTreatment()
        {
            monitorRepository = new MonitorRepository();
            OnOffRedisClient = new RedisManager<VehicleOnOffTime>();
            VdRedisClient = new RedisManager<VehicleDisplacement>();
            //htInfo = Hashtable.Synchronized(new Hashtable()); 
        }

        public static void StatOnOffTime(byte[] bytes)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            if (!string.IsNullOrEmpty(str))
            {
                string[] alarmArray = str.Substring(0, str.LastIndexOf("#")).Split(',');
                if (alarmArray[3].ToUpper().Equals("V1"))
                {
                    LoggerManager.Logger.Info("MDVRID: " + alarmArray[1] + " OnLine");
                    if (!OnOffRedisClient.Hash_Exist(RedisKey.VehicleOnOffLineKey, alarmArray[1]))
                    {
                        VehicleOnOffTime vehicleOnOffTime = new VehicleOnOffTime();
                        vehicleOnOffTime.Vehicle_ID = alarmArray[26];
                        vehicleOnOffTime.Mdvr_Core_SN = alarmArray[1];
                        var time = ConvertHelper.ConvertStrToDate(alarmArray[4], "yyMMdd HHmmss");
                        if (time != null)
                        {
                            vehicleOnOffTime.Online_Time = time.Value;
                        }
                        else
                        {
                            vehicleOnOffTime.Online_Time = DateTime.MinValue;
                        }
                        lock (lockobj)
                        {
                            OnOffRedisClient.Hash_Set(RedisKey.VehicleOnOffLineKey, alarmArray[1], vehicleOnOffTime);
                        }
                    }
                }
                else if (alarmArray[3].ToUpper().Equals("V20") || alarmArray[3].ToUpper().Equals("A1")
                    || alarmArray[3].ToUpper().Equals("A2"))
                {
                    LoggerManager.Logger.Info("MDVRID: " + alarmArray[1] + (alarmArray[3].ToUpper().Equals("V20") ? " Close" : " OffLine"));
                    VehicleDisplacement vd = VdRedisClient.Hash_Get(RedisKey.VehicleGpsKey, alarmArray[1]);
                    if (OnOffRedisClient.Hash_Exist(RedisKey.VehicleOnOffLineKey, alarmArray[1]))
                    {
                        VehicleOnOffTime statInfo = OnOffRedisClient.Hash_Get(RedisKey.VehicleOnOffLineKey, alarmArray[1]);
                        if (statInfo.Vehicle_ID != null)
                        {
                            var time = ConvertHelper.ConvertStrToDate(alarmArray[4], "yyyyMMdd HHmmss");
                            if (time != null && statInfo.Online_Time != DateTime.MinValue)
                            {
                                TimeSpan ts = time.Value - statInfo.Online_Time;
                                decimal second = Convert.ToDecimal(ts.TotalSeconds);
                                statInfo.Offline_Time = time.Value;
                                statInfo.Online_Timespan = second;
                                statInfo.Distance = vd != null ? Convert.ToDecimal(vd.Distance) : 0;
                                int i = 0;
                                lock(lockobj)
                                {
                                    i = monitorRepository.AddVehicleOnOffTime(statInfo);
                                }
                                if (i > 0)
                                {
                                    VdRedisClient.Hash_Remove(RedisKey.VehicleGpsKey, alarmArray[1]);
                                    OnOffRedisClient.Hash_Remove(RedisKey.VehicleOnOffLineKey, alarmArray[1]);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void StatGpsDistance(byte[] bytes)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            if (!string.IsNullOrEmpty(str))
            {
                string[] alarmArray = str.Substring(0, str.LastIndexOf("#")).Split(',');
                if (alarmArray[3].ToUpper().Equals("V30"))
                {
                    LoggerManager.Logger.Info("MDVRID: " + alarmArray[1] + " Get GPSInfo");
                    bool isValid = alarmArray[5].Equals("A") ? true : false;
                    if (isValid)
                    {
                        VehicleDisplacement vd = null;
                        if (VdRedisClient.Hash_Exist(RedisKey.VehicleGpsKey, alarmArray[1]))
                        {
                            vd = VdRedisClient.Hash_Get(RedisKey.VehicleGpsKey, alarmArray[1]);
                            double dis = GetDistance(Double.Parse(vd.Latitude) / 100, Double.Parse(vd.Longitude) / 100, Double.Parse(alarmArray[7]) / 100, Double.Parse(alarmArray[6]) / 100);
                            vd.Longitude = alarmArray[6];
                            vd.Latitude = alarmArray[7];
                            vd.Distance += dis;
                            VdRedisClient.Hash_Set(RedisKey.VehicleGpsKey, alarmArray[1], vd);
                        }
                        else
                        {
                            vd = new VehicleDisplacement();
                            vd.Mdvr_Core_SN = alarmArray[1];
                            vd.Longitude = alarmArray[6];
                            vd.Latitude = alarmArray[7];
                            vd.Distance = 0.0;
                            VdRedisClient.Hash_Set(RedisKey.VehicleGpsKey, alarmArray[1], vd);
                        }
                    }
                }
            }
        }

        public static void StatAllOnOffTime()
        {
            List<VehicleDisplacement> lstVd = VdRedisClient.Hash_GetAll(RedisKey.VehicleGpsKey);
            List<VehicleOnOffTime> lstOnOff = OnOffRedisClient.Hash_GetAll(RedisKey.VehicleOnOffLineKey);
            if (lstOnOff != null && lstOnOff.Count > 0)
            {
                for (int i = 0; i < lstOnOff.Count; i++)
                {
                    VehicleOnOffTime statInfo = lstOnOff[i];
                    DateTime onLineTime = statInfo.Online_Time;
                    DateTime offLineTime = DateTime.Parse(onLineTime.Date.ToShortDateString() + " 23:59:59");
                    TimeSpan ts = offLineTime - onLineTime;
                    decimal second = Convert.ToDecimal(ts.TotalSeconds);
                    statInfo.Offline_Time = offLineTime;
                    statInfo.Online_Timespan = second;
                    string mdvrid = statInfo.Mdvr_Core_SN;
                    string vehicleid = statInfo.Vehicle_ID;
                    VehicleDisplacement vd = null;
                    if (lstVd != null)
                    {
                        vd = lstVd.FirstOrDefault(c => c.Mdvr_Core_SN.Equals(mdvrid));
                    }
                    statInfo.Distance = vd == null ? 0 : Convert.ToDecimal(vd.Distance);
                    int result = monitorRepository.AddVehicleOnOffTime(statInfo);
                    statInfo = new VehicleOnOffTime();
                    statInfo.Vehicle_ID = vehicleid;
                    statInfo.Mdvr_Core_SN = mdvrid;
                    statInfo.Online_Time = onLineTime.Date.AddDays(1);
                    OnOffRedisClient.Hash_Set(RedisKey.VehicleOnOffLineKey, mdvrid, statInfo);
                    if (vd != null)
                    {
                        vd.Distance = 0.0;
                        VdRedisClient.Hash_Set(RedisKey.VehicleGpsKey, mdvrid, vd);
                    }
                }
            }
        }

        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        private static double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lon1) - rad(lon2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EarthRadius;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }
    }
}
