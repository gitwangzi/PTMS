/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c1ee4b24-fd86-47c8-b227-b5fa54e553c9      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Monitor.Entity
/////    Project Description:    
/////             Class Name: V30
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/21 16:00:31
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/21 16:00:31
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.Common.Util;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// GPS位置数据 V30
    /// </summary>
    [Serializable]
    [DataContract]
    public class GPS
    {

        public GPS() { }

        /// <summary>
        /// 99dc[指令长度],[MDVR芯片号],[消息序列号],指令关键字,[位置和状态],驱动标志#
        /// </summary>
        /// <param name="str"></param>
        public GPS(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] alarmArray = str.Substring(0, str.LastIndexOf("#")).Split(',');

                if (alarmArray[3].ToUpper().Equals("V30"))
                {
                    this.MdvrCoreId = alarmArray[1];          ////芯片号

                    this.GPSValid = alarmArray[5];     ////GPS有效性
                    this.Longitude = alarmArray[6];    ////经度
                    this.Latitude = alarmArray[7];     ////纬度
                    this.Speed = alarmArray[8];        ////速度
                    this.Direction = alarmArray[9];    ////方向
                    this.DriveSign = int.Parse(alarmArray[20]);

                    this.Context = str;                    ////原始文本

                    var time = ConvertHelper.ConvertStrToDate(alarmArray[4], "yyMMdd HHmmss");
                    if (time != null)
                    {
                        this.GPSTime = time.Value;     ////GPS时间
                    }
                }
            }
        }

        /// <summary>
        /// 芯片号
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember]
        public string Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember]
        public string Latitude { get; set; }

        /// <summary>
        /// 速度
        /// </summary>
        [DataMember]
        public string Speed { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        [DataMember]
        public string Direction { get; set; }

        /// <summary>
        /// GPS时间
        /// </summary>
        [DataMember]
        public Nullable<DateTime> GPSTime { get; set; }

        /// <summary>
        /// GPS有效性
        /// </summary>
        [DataMember]
        public string GPSValid { get; set; }

        /// <summary>
        /// 驱动标志	本指令是由何因素驱动其发送
        /// 0：下发的实时位置监控指令
        /// 1：固定上传参数
        /// 2：为了与正在传输中的视频流同步（无用）
        /// 3：历史GPS信息
        /// 4：一键报警上传GPS
        /// 5：历史一键报警GPS信息
        /// </summary>
        [DataMember]
        public int DriveSign { get; set; }

        /// <summary>
        /// GPS原始字符串
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [DataMember]
        public string VehicleId { get; set; }

        /// <summary>
        /// 安全套件ID
        /// </summary>
        [DataMember]
        public string SuiteInfoId { get; set; }

        /// <summary>
        /// 区域代码
        /// </summary>
        [DataMember]
        public string DistrictCode { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [DataMember]
        public short Source { get; set; }

        /// <summary>
        /// 拼接SQL语句
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("INSERT INTO ALARM_LOCATION  (ID, MDVR_CORE_SN, VEHICLE_ID, SUITE_INFO_ID, GPS_VALID, DISTRICT_CODE, LONGITUDE, LATITUDE, GPS_TIME, SPEED, DIRECTION) VALUES('");
            sbSql.Append(Guid.NewGuid().ToString());
            sbSql.Append("','");
            sbSql.Append(this.MdvrCoreId);
            sbSql.Append("','");
            sbSql.Append(this.VehicleId);
            sbSql.Append("','");
            sbSql.Append(this.SuiteInfoId);
            sbSql.Append("','");
            sbSql.Append(this.GPSValid);
            sbSql.Append("','");
            sbSql.Append(this.DistrictCode);
            sbSql.Append("','");
            sbSql.Append(this.Longitude);
            sbSql.Append("','");
            sbSql.Append(this.Latitude);
            sbSql.Append("',TO_DATE('");
            sbSql.Append(this.GPSTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            sbSql.Append("','yyyy-mm-dd hh24:mi:ss')");
            sbSql.Append(",'");
            sbSql.Append(this.Speed);
            sbSql.Append("','");
            sbSql.Append(this.Direction);
            sbSql.Append("');");

            return sbSql.ToString();
        }

        /// <summary>
        /// 拼接SQL语句
        /// </summary>
        /// <returns></returns>
        public string ToAddMonitorSql()
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("INSERT INTO VEHICLE_LOCATION  (ID, MDVR_CORE_SN, VEHICLE_ID, SUITE_INFO_ID, GPS_VALID, DISTRICT_CODE, LONGITUDE, LATITUDE, GPS_TIME, SPEED, DIRECTION) VALUES('");
            sbSql.Append(Guid.NewGuid().ToString());
            sbSql.Append("','");
            sbSql.Append(this.MdvrCoreId);
            sbSql.Append("','");
            sbSql.Append(this.VehicleId);
            sbSql.Append("','");
            sbSql.Append(this.SuiteInfoId);
            sbSql.Append("','");
            sbSql.Append(this.GPSValid);
            sbSql.Append("','");
            sbSql.Append(this.DistrictCode);
            sbSql.Append("','");
            sbSql.Append(this.Longitude);
            sbSql.Append("','");
            sbSql.Append(this.Latitude);
            sbSql.Append("',TO_DATE('");
            sbSql.Append(this.GPSTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            sbSql.Append("','yyyy-mm-dd hh24:mi:ss')");
            sbSql.Append(",'");
            sbSql.Append(this.Speed);
            sbSql.Append("','");
            sbSql.Append(this.Direction);
            sbSql.Append("');");

            return sbSql.ToString();
        }
    }
}
