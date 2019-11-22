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
using Gsafety.PTMS.Common.Enum;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// GPS位置数据 V30
    /// </summary>
    [Serializable]
    [DataContract]
    public class PTMSGPS
    {
        public short Source { get; set; }

        [DataMember]
        public GPSSourceEnum GPSSource { get; set; }


        public PTMSGPS() { }

        /// <summary>
        /// 99dc[指令长度],[MDVR芯片号],[消息序列号],指令关键字,[位置和状态],驱动标志#
        /// </summary>
        /// <param name="str"></param>
        public PTMSGPS(string str, int zone)
        {
            if (!string.IsNullOrEmpty(str))
            {
                ////$GPRMC,,V,,,,,,,,,,N*53\r\n,012605000689911
                ////去掉\r\n
                string[] gpsArray = str.Replace("\r\n", "").Split(',');

                this.MdvrCoreId = gpsArray[13];          ////芯片号

                this.GPSValid = gpsArray[2];     ////GPS有效性

                this.Longitude = gpsArray[5];    ////经度                
                this.Latitude = gpsArray[3];     ////纬度
                if (gpsArray[4] == "S")          ////纬度为S，取值负
                {
                    this.Latitude = "-" + this.Latitude;
                }
                if (gpsArray[6] == "W")          ////经度W ，取值负
                {
                    this.Longitude = "-" + this.Longitude;
                }

                if (!string.IsNullOrEmpty(gpsArray[7]))
                {
                    this.Speed = (Convert.ToDouble(gpsArray[7]) * 1.853).ToString();        ////速度 节转换为公里 1节=1.853公里/小时
                }
                this.Direction = gpsArray[8];    ////方向

                this.Context = str;                    ////原始文本

                var dateDmy = ConvertHelper.ConvertStrToDate(gpsArray[9], "ddMMyy");

                if (gpsArray[1].Length > 0 && dateDmy != null)
                {
                    ////当前时间为0时区，需要加上时区
                    if ((int.Parse(gpsArray[1].Substring(0, 2)) >= 0) && (int.Parse(gpsArray[1].Substring(0, 2)) < 24)
                        && (int.Parse(gpsArray[1].Substring(2, 2)) >= 0) && (int.Parse(gpsArray[1].Substring(2, 2)) < 60)
                        && (int.Parse(gpsArray[1].Substring(4, 2)) >= 0) && (int.Parse(gpsArray[1].Substring(4, 2)) < 60))
                        this.GPSTime = new DateTime(dateDmy.Value.Year, dateDmy.Value.Month, dateDmy.Value.Day, int.Parse(gpsArray[1].Substring(0, 2)), int.Parse(gpsArray[1].Substring(2, 2)), int.Parse(gpsArray[1].Substring(4, 2))).AddHours(zone);
                    else
                        this.GPSValid = "V";
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
        public DateTime GPSTime { get; set; }

        /// <summary>
        /// GPS有效性
        /// </summary>
        [DataMember]
        public string GPSValid { get; set; }

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
        /// 拼接SQL语句
        /// </summary>
        /// <returns></returns>
        public override string ToString()
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
            sbSql.Append(this.GPSTime.ToString("yyyy-MM-dd HH:mm:ss"));
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
