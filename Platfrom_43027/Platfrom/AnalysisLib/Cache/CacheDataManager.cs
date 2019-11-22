using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.AnalysisLib
{
    public class CacheDataManager
    {
        public static Dictionary<string, string> Districts = new Dictionary<string, string>();
        public static Dictionary<string, SuiteCache> Suites = new Dictionary<string, SuiteCache>();
        //GPS设备在线状态
        public static Dictionary<string, RUN_GPS_WORKING> GPSs = new Dictionary<string, RUN_GPS_WORKING>();
        //手机在线状态
        public static Dictionary<string, RUN_MOBILE_WORKING> Mobiles = new Dictionary<string, RUN_MOBILE_WORKING>();
        //用户当前在线状态
        public static Dictionary<string, RUN_USER_ONLINE> Users = new Dictionary<string, RUN_USER_ONLINE>();
        /// <summary>
        /// 车辆组织结构与用户权限的对应关系
        /// key:organizationid
        /// value:users,具有报警权限的报警处理人员
        /// </summary>
        public static Dictionary<string, Dictionary<string, RUN_USER_ONLINE>> OrganizationUser = new Dictionary<string, Dictionary<string, RUN_USER_ONLINE>>();
        /// <summary>
        /// 用户分配警情的时间
        /// </summary>
        public static Dictionary<string, DateTime> AlarmTime = new Dictionary<string, DateTime>();
        /// <summary>
        /// 客户转警模式
        /// </summary>
        public static Dictionary<string, int> ClientModes = new Dictionary<string, int>();
    }
}
