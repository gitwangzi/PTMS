using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.SIDS.Contract.Data;
using Gsafety.PTMS.SIDS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.SIDS.Repository
{
    public class VehicleRepository : BaseRepository
    {
        //1、所有在线车辆位置
        public static IList<VehicleModel> GetAllOnLineVehicleMes(PTMSEntities context, string strControlCenter)
        {
            try
            {
                var vehicles = from x in context.VEHICLE_LOCATION
                               join y in context.SECURITY_SUITE_WORKING on x.VEHICLE_ID equals y.VEHICLE_ID
                               where y.ONLINE_FLAG == 1
                               select x.VEHICLE_ID;
                List<string> lsvehicles = vehicles.ToList().Distinct().ToList();

                var result1 = from vl in context.VEHICLE_LOCATION
                              where lsvehicles.Any(n => n == vl.VEHICLE_ID)
                              group vl by vl.VEHICLE_ID into g
                              select new VehicleModel()
                              {
                                  VehicleSn = g.Key,
                                  GPS_Time = g.Max(n => n.GPS_TIME),
                              };
                List<VehicleModel> temp = result1.ToList();
                List<VEHICLE_LOCATION> locations = new List<VEHICLE_LOCATION>();
                foreach (VehicleModel item in temp)
                {
                    VEHICLE_LOCATION location = new VEHICLE_LOCATION();
                    location.VEHICLE_ID = item.VehicleSn;
                    location.GPS_TIME = item.GPS_Time;

                    locations.Add(location);
                }

                var result = from vm in locations
                             from v1 in context.VEHICLE_LOCATION
                             where vm.VEHICLE_ID == v1.VEHICLE_ID && vm.GPS_TIME == v1.GPS_TIME
                             select new VehicleModel()
                               {
                                   VehicleSn = v1.VEHICLE_ID,
                                   LONGITUDE = v1.LONGITUDE,
                                   LATITUDE = v1.LATITUDE,
                                   GPS_Time = v1.GPS_TIME,
                               };
                return result.ToList();
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        //2、报警车辆位置
        public IList<VehicleModel> GetAlarmVehicleMes(PTMSEntities context, string strControlCenter)
        {
            //关联逻辑需确认
            var idlist = from x in context.ALARM_RECORD
                         join y in context.ALARM_DISPOSE on x.ID equals y.ALARM_ID
                         where y.ALARM_FLAG == 1
                         select x.VEHICLE_ID;

            List<string> strList = idlist.ToList().Distinct().ToList();

            var result1 = from x in context.ALARM_RECORD
                          where strList.Any(n => n == x.VEHICLE_ID)
                          group x by x.VEHICLE_ID into g
                          select new VehicleModel()
                          {
                              VehicleSn = g.Key,
                              GPS_Time = g.Max(n => n.GPS_TIME),
                          };
            List<VehicleModel> vm = result1.ToList();
            List<VEHICLE_LOCATION> v_ids = new List<VEHICLE_LOCATION>();
            foreach (VehicleModel item in vm)
            {
                VEHICLE_LOCATION v_id = new VEHICLE_LOCATION();
                v_id.VEHICLE_ID = item.VehicleSn;
                v_id.GPS_TIME = item.GPS_Time;

                v_ids.Add(v_id);
            }

            var result = from x in context.ALARM_RECORD
                         from y in v_ids
                         where x.VEHICLE_ID == y.VEHICLE_ID && x.GPS_TIME == y.GPS_TIME
                         select new VehicleModel()
                         {
                             VehicleSn = x.VEHICLE_ID,
                             LONGITUDE = x.LONGITUDE,
                             LATITUDE = x.LATITUDE,
                             GPS_Time = x.GPS_TIME,
                         };
            return result.ToList();

        }

        //3、离线车辆位置
        public IList<VehicleModel> GetOutLineVehicleMes(PTMSEntities context, string strControlCenter)
        {
            var idlist = from x in context.VEHICLE_LOCATION
                         join y in context.SECURITY_SUITE_WORKING on x.VEHICLE_ID equals y.VEHICLE_ID
                         where y.ONLINE_FLAG == 0
                         orderby x.GPS_TIME descending
                         select x.VEHICLE_ID;

            List<string> strList = idlist.ToList().Distinct().ToList();
            var no_time = from x in context.VEHICLE_LOCATION
                          where strList.Any(n => n == x.VEHICLE_ID)
                          group x by x.VEHICLE_ID into g
                          select new VehicleModel()
                          {
                              VehicleSn = g.Key,
                              GPS_Time = g.Max(n => n.GPS_TIME),
                          };

            List<VehicleModel> vm = no_time.ToList();

            List<VEHICLE_LOCATION> v_ids = new List<VEHICLE_LOCATION>();
            foreach (VehicleModel item in vm)
            {
                VEHICLE_LOCATION v_id = new VEHICLE_LOCATION();
                v_id.VEHICLE_ID = item.VehicleSn;
                v_id.GPS_TIME = item.GPS_Time;

                v_ids.Add(v_id);
            }

            var result = from x in v_ids
                         from y in context.VEHICLE_LOCATION
                         where x.VEHICLE_ID == y.VEHICLE_ID && x.GPS_TIME == y.GPS_TIME
                         select new VehicleModel()
                        {
                            VehicleSn = x.VEHICLE_ID,
                            LONGITUDE = x.LONGITUDE,
                            LATITUDE = x.LATITUDE,
                            GPS_Time = x.GPS_TIME,
                        };
            return result.ToList();

        }

        //4、车辆在线情况 (测试通过)
        public Dictionary<string, int> GetAllVehicleMessage(PTMSEntities context, string strControlCenter)
        {
            var AllCount = from x in context.VEHICLE select x.VEHICLE_ID;

            var OnLineCount = from x in context.SECURITY_SUITE_WORKING
                              where x.ONLINE_FLAG == 1
                              select x.VEHICLE_ID;

            var OutLineCount = from x in context.SECURITY_SUITE_WORKING
                               where x.ONLINE_FLAG == 0
                               select x.VEHICLE_ID;

            var WarningCount = from x in context.ALARM_RECORD
                               join y in context.ALARM_DISPOSE on x.ID equals y.ALARM_ID
                               where y.ALARM_FLAG == 1
                               where x.STATUS != 4
                               select x.VEHICLE_ID;

            Dictionary<string, int> Dic = new Dictionary<string, int>();
            Dic.Add("AllVehicle", Convert.ToInt32(AllCount.ToList().Count));
            Dic.Add("OnLineVehicle", Convert.ToInt32(OnLineCount.ToList().Count));
            Dic.Add("OutLineVehicle", Convert.ToInt32(OutLineCount.ToList().Count));
            Dic.Add("AlarmVehicle", Convert.ToInt32(WarningCount.ToList().Count));
            return Dic;
        }

        //5、一键报警信息列表
        public IList<AlarmModel> GetTop10Alarm(PTMSEntities context, string strControlCenter)
        {
            var result = (from x in context.ALARM_RECORD
                          join y in context.ALARM_DISPOSE on x.ID equals y.ALARM_ID
                          where y.ALARM_FLAG == 1
                          orderby x.GPS_TIME descending
                          select new AlarmModel()
                          {
                              VehicleSn = x.VEHICLE_ID,
                              AlarmState = x.STATUS,
                              AlarmTime = x.GPS_TIME,
                              LONGITUDE = x.LONGITUDE,
                              LATITUDE = x.LATITUDE,
                              Speed = x.SPEED,
                          }).Take(10);

            if (result.ToList().Count == 0)
            {
                return null;
            }
            else
            {
                return result.ToList();
            }
        }

        //6、一键报警统计(12个月)  ALARM_RECORD
        public Dictionary<string, int> StatisticsAlarmCountByMonth(PTMSEntities context, string strControlCenter, string StartTime, string EndTime)
        {
            string DicTime = null;
            Dictionary<string, int> Dic = new Dictionary<string, int>();

            for (int i = 0; i < 12; i++)
            {
                DateTime time1 = Convert.ToDateTime(EndTime.Substring(0, 4) + "-" + EndTime.Substring(4, 2) + "-" + "01");
                DateTime time2 = time1.AddMonths(1);
                var CountNum = from x in context.ALARM_RECORD
                               where x.GPS_TIME > time1
                               where x.GPS_TIME < time2
                               select x.ID;

                DicTime = EndTime.Substring(4, 2) + "/" + EndTime.Substring(0, 4);
                Dic.Add(DicTime, Convert.ToInt32(CountNum.ToList().Count));
                EndTime = Convert.ToDateTime(EndTime.Substring(0, 4) + "-" + EndTime.Substring(4, 2) + "-" + "01").AddMonths(-1).ToString("yyyyMM");
            }

            return Dic;
        }


        //7、车辆未上线情况统计（过去7天）  SECURITY_SUITE_WORKING
        public Dictionary<string, int> StatisticsOutLineVehicleCount(PTMSEntities context, string strControlCenter, string StartTime, string EndTime)
        {
            Dictionary<string, int> Dic = new Dictionary<string, int>();

            for (int i = 0; i < 7; i++)
            {
                DateTime time1 = Convert.ToDateTime(EndTime.Substring(0, 4) + "-" + EndTime.Substring(4, 2) + "-" + EndTime.Substring(6, 2)).AddDays(-i);
                DateTime time2 = Convert.ToDateTime(EndTime.Substring(0, 4) + "-" + EndTime.Substring(4, 2) + "-" + EndTime.Substring(6, 2)).AddDays(1);
                //有上下线切换记录的车辆数，即相应时间有在线的车辆数
                //var CountNum = from x in context.SECURITY_SUITE_WORKING
                //               where x.SWITCH_TIME > time1
                //               where x.SWITCH_TIME < time2
                //               select new VehicleModel
                //               {
                //                   VehicleSn = x.VEHICLE_ID,
                //               };
                //var AllCount = from x in context.SECURITY_SUITE_WORKING
                //               select new VehicleModel
                //               {
                //                   VehicleSn = x.VEHICLE_ID,
                //               };

                var CountNum = from x in context.SUITE_ONLINE_RECORD
                               where x.GPS_TIME > time1
                               where x.GPS_TIME < time2
                               where x.STATUS == 1
                               select new VehicleModel
                               {
                                   VehicleSn = x.VEHICLE_ID,
                               };
                var AllCount = from x in context.SUITE_ONLINE_RECORD
                               select new VehicleModel
                               {
                                   VehicleSn = x.VEHICLE_ID,
                               };

                int outlineCount = 0;

                List<VehicleModel> dAllCount = AllCount.ToList().GroupBy(p => p.VehicleSn).Select(g => g.First()).ToList();
                List<VehicleModel> dCountNum = CountNum.ToList().GroupBy(p => p.VehicleSn).Select(g => g.First()).ToList();
                outlineCount = Convert.ToInt32(dAllCount.Count) - Convert.ToInt32(dCountNum.Count);

                switch (i)
                {
                    case 0:
                        Dic.Add("Fir", outlineCount);
                        break;
                    case 1:
                        Dic.Add("Sec", outlineCount);
                        break;
                    case 2:
                        Dic.Add("Thi", outlineCount);
                        break;
                    case 3:
                        Dic.Add("For", outlineCount);
                        break;
                    case 4:
                        Dic.Add("Fiv", outlineCount);
                        break;
                    case 5:
                        Dic.Add("Six", outlineCount);
                        break;
                    case 6:
                        Dic.Add("Sev", outlineCount);
                        break;
                }
            }
            return Dic;
        }

        //8、报警车辆态势展示  VEHICLE_LOCATION
        public IList<VehicleModel> GetAlarmVehicleLocation(PTMSEntities context, string strControlCenter, string WarningMark, string WarningTime, string HandleTime)
        {
            DateTime dateW = Convert.ToDateTime(WarningTime.Substring(0, 4) + "-" + WarningTime.Substring(4, 2) + "-" + WarningTime.Substring(6, 2));
            if (HandleTime.Equals(null) || HandleTime == null)
            {
                var result = from x in context.ALARM_LOCATION
                             join y in context.ALARM_RECORD on x.VEHICLE_ID equals y.VEHICLE_ID
                             join z in context.ECU911_DISPOSE on y.ID equals z.ALARM_ID
                             where z.INCIDENT_ID == WarningMark && x.GPS_TIME > dateW
                             select new VehicleModel()
                             {
                                 ID = x.ID,
                                 VehicleSn = x.VEHICLE_ID,
                                 LONGITUDE = x.LONGITUDE,
                                 LATITUDE = x.LATITUDE,
                                 GPS_Time = x.GPS_TIME,
                             };
                return result.ToList();
            }
            else
            {
                DateTime dateH = Convert.ToDateTime(HandleTime.Substring(0, 4) + "-" + HandleTime.Substring(4, 2) + "-" + HandleTime.Substring(6, 2));
                var result = from x in context.ALARM_LOCATION
                             join y in context.ALARM_RECORD on x.VEHICLE_ID equals y.VEHICLE_ID
                             join z in context.ECU911_DISPOSE on y.ID equals z.ALARM_ID
                             where z.INCIDENT_ID == WarningMark && x.GPS_TIME > dateW && x.GPS_TIME < dateH
                             select new VehicleModel()
                             {
                                 ID = x.ID,
                                 VehicleSn = x.VEHICLE_ID,
                                 LONGITUDE = x.LONGITUDE,
                                 LATITUDE = x.LATITUDE,
                                 GPS_Time = x.GPS_TIME,
                             };
                return result.ToList();
            }

        }


        //9、车辆信息展示

        public PerVehicleModel GetPerVehicleMes(PTMSEntities context, string strControlCenter, string VehicleSn)
        {
            var result = from x in context.VEHICLE
                         where x.VEHICLE_ID == VehicleSn
                         select new PerVehicleModel
                         {
                             VehicleSn = x.VEHICLE_ID,
                             Brand_Model = x.BRAND_MODEL,
                             Start_Year = x.START_YEAR,
                             Operation_Licence = x.OPERATION_LICENSE,
                             Owner = x.OWNER,
                             Owner_Phone = x.OWNER_PHONE,
                         };
            if (result.ToList().Count == 0)
            {
                return null;
            }
            else
            {
                return result.ToList()[0];
            }
        }

        //10、安全套件状态  SECURITY_SUITE_INFO
        public MDVR_Model GetMDVRMessage(PTMSEntities context, string strControlCenter, string VehicleSn)
        {
            var result = from x in context.SECURITY_SUITE_INFO
                         join y in context.SECURITY_SUITE_WORKING on x.SUITE_INFO_ID equals y.SUITE_INFO_ID
                         join z in context.INSTALLATION_DETAIL on y.VEHICLE_ID equals z.VEHICLE_ID
                         where y.VEHICLE_ID == VehicleSn
                         select new MDVR_Model
                         {
                             MDVR_SIM = x.MDVR_SIM,
                             MDVR_CORE_SN = x.MDVR_CORE_SN,
                             STATUS = x.STATUS,
                             InstallPerson = z.INSTALL_STAFF,
                             CREATE_TIME = x.CREATE_TIME,
                         };
            if (result.ToList().Count == 0)
            {
                return null;
            }
            else
            {
                return result.ToList()[0];
            }
        }


        //11、历史在线时长统计 VECHILE_ONLINE_TIME
        public Dictionary<string, decimal> GetOnLineTime(PTMSEntities context, string strControlCenter, string BusID, string StartTime, string EndTime)
        {
            Dictionary<string, decimal> Dic = new Dictionary<string, decimal>();
            DateTime dtime = Convert.ToDateTime(EndTime.Substring(0, 4) + "-" + EndTime.Substring(4, 2) + "-" + EndTime.Substring(6, 2));
            DateTime dtime1 = dtime.AddDays(1);
            for (int i = 0; i < 7; i++)
            {
                //当天上下线
                var result = from x in context.VEHICLE_ONLINE_TIME
                             where x.ONLINE_TIME > dtime && x.ONLINE_TIME < dtime1
                             where x.OFFLINE_TIME > dtime && x.OFFLINE_TIME < dtime1
                             where x.VEHICLE_ID == BusID
                             select x.ONLINE_TIMESPAN;
                //当天上线未下线
                var result1 = from x in context.VEHICLE_ONLINE_TIME
                              where x.ONLINE_TIME > dtime && x.ONLINE_TIME < dtime1
                              where x.OFFLINE_TIME > dtime1
                              where x.VEHICLE_ID == BusID
                              select x.ONLINE_TIME;

                //以前上线当天下线
                var result2 = from x in context.VEHICLE_ONLINE_TIME
                              where x.OFFLINE_TIME > dtime && x.OFFLINE_TIME < dtime1
                              where x.ONLINE_TIME < dtime
                              where x.VEHICLE_ID == BusID
                              select x.OFFLINE_TIME;

                List<decimal?> time1 = result.ToList();
                List<DateTime?> time2 = result1.ToList();
                List<DateTime?> time3 = result2.ToList();

                decimal? timespan = 0;
                for (int j = 0; j < time1.Count; j++)
                {
                    timespan += time1[j];
                }
                for (int m = 0; m < time2.Count; m++)
                {
                    TimeSpan ss = dtime1 - Convert.ToDateTime(time2[m]);
                    timespan += Convert.ToDecimal(ss.TotalSeconds);
                }
                for (int m = 0; m < time3.Count; m++)
                {
                    TimeSpan ss = Convert.ToDateTime(time3[m]) - dtime;
                    timespan += Convert.ToDecimal(ss.TotalSeconds);
                }

                string westtime = dtime.ToString("yyyyMMdd").Substring(6, 2) + "/" + dtime.ToString("yyyyMMdd").Substring(4, 2);

                Dic.Add(westtime, Convert.ToDecimal(timespan / 3600));

                dtime = dtime.AddDays(-1); dtime1 = dtime.AddDays(1);
            }
            return Dic;
        }

        //附加1
        public IList<Vehicle_MDVR_Model> GetVehicle_MDVR(PTMSEntities context, string strControlCenter)
        {
            var result = from x in context.VEHICLE
                         join y in context.SUITE_ONLINE_RECORD on x.VEHICLE_ID equals y.VEHICLE_ID
                         select new Vehicle_MDVR_Model
                         {
                             VehicleSn = x.VEHICLE_ID,
                             Region_Name = x.REGION,
                             District_Code = x.DISTRICT_CODE,
                             MDVR_Core_Sn = y.MDVR_CORE_SN,
                         };
            if (result.ToList().Count == 0)
            {
                return null;
            }
            else
            {
                return result.ToList();
            }
        }

        //附加2
        public IList<Vehicle_Video_Model> GetVehicle_Video(PTMSEntities context, string VehicleSn)
        {
            var result = from x in context.VIDEO_LOG
                         join y in context.SUITE_ONLINE_RECORD on x.MDVR_CORE_SN equals y.MDVR_CORE_SN
                         where y.VEHICLE_ID == VehicleSn
                         select new Vehicle_Video_Model
                         {
                             File_Name = x.FILE_NAME,
                             Create_Time = x.CREATE_TIME,

                         };

            if (result.ToList().Count == 0)
            {
                return null;
            }
            else
            {
                return result.ToList();
            }
        }

    }
}
