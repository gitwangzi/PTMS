/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: eea976dd-a94c-4d7c-a909-42ebfe7e21dd      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Monitor.Contract.Data
/////    Project Description:    
/////             Class Name: Online
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/27 9:52:15
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/27 9:52:15
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Gsafety.Common.Util;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// V1上线A1下线
    /// </summary>
    [DataContract]
    [Serializable]
    public class OnOffline
    {
        public OnOffline() { }

        /// <summary>
        ///  99dc0181,007201FFBC,3309,V1,130715 140233,A,11619.6616,3959.0198,
        ///  0.00,0.00,0000000000000000,00050000000b0000,40.00,999.00,00000000.8000,,,0,0,0,0.0.3.00,
        ///  1112,172.16.20.190:9876,0,2,,RM0001#
        /// </summary>
        public OnOffline(string str)
        {
            ////99dc[指令长度],[MDVR芯片号],[消息序列号],指令关键字,[位置和状态],
            ////触发时位置和状态,警情UID,发送次数序号,按钮序号,按钮别名#
            ////指令长度99dc0194,MDVR芯片号0108002821,消息序列号,指令关键字V61,指令发送时间130729 033627,定位状态A,经度11422.822265,纬度2243.110839,地面速率0.00,地面航向202.93,部件状态及警情标志0000000008080C00,电源状态与GPS模块状态0000000000010000,安全套件温度703.23,车厢内温度21.97,IO警情状态及网络状态0000E011.0008,线路号26,司机编号102,下一站编号301,停靠标志1,车上人数0,,,,,,,,,,,,,,,,,警情UID111670853010,发送次数序号1,按钮序号3,按钮别名EmergencyAlarm#
            if (!string.IsNullOrEmpty(str))
            {
                string[] alarmArray = str.Substring(0, str.LastIndexOf("#")).Split(',');

                ////上线
                if (alarmArray[3].ToUpper().Equals("V1"))
                {

                    this.GPSValid = alarmArray[5];     ////GPS有效性
                    this.Longitude = alarmArray[6];    ////经度
                    this.Latitude = alarmArray[7];     ////纬度
                    this.Speed = alarmArray[8];        ////速度
                    this.Direction = alarmArray[9];    ////方向

                    this.VehicleId = alarmArray[26];    ////车牌号
                    this.IsOnLine = 1;

                    var time = ConvertHelper.ConvertStrToDate(alarmArray[4], "yyMMdd HHmmss");
                    if (time != null)
                    {
                        this.GPSTime = time.Value;     ////GPS时间
                    }
                }
                ////关机上报
                else if (alarmArray[3].ToUpper().Equals("V20"))
                {
                    this.GPSValid = alarmArray[5];     ////GPS有效性
                    this.Longitude = alarmArray[6];    ////经度
                    this.Latitude = alarmArray[7];     ////纬度
                    this.Speed = alarmArray[8];        ////速度
                    this.Direction = alarmArray[9];    ////方向

                    ////关机类型0：取消关机
                    ////1：延时关机
                    ////2：定时关机
                    ////3：非正常关机
                    ////4：遥控器按键关机（可选）
                    ////5：远程关机
                    ////6：定时或延迟模式关机
                    ////7：定时与延迟模式关机
                    ////8：其他
                    int offType = int.Parse(alarmArray[20]);

                    this.IsOnLine = offType == 0 ? 1 : 0;

                    var time = ConvertHelper.ConvertStrToDate(alarmArray[4], "yyMMdd HHmmss");
                    if (time != null)
                    {
                        this.GPSTime = time.Value;     ////GPS时间
                    }

                }
                ////下线
                else if (alarmArray[3].ToUpper().Equals("A1")
                    || alarmArray[3].ToUpper().Equals("A2"))
                {
                    this.IsOnLine = 0;
                    var time = ConvertHelper.ConvertStrToDate(alarmArray[4], "yyyyMMdd HHmmss");
                    if (time != null)
                    {
                        this.GPSTime = time.Value;     ////GPS时间
                    }
                }

                this.MdvrCoreId = alarmArray[1];          ////芯片号
                this.Context = str;                    ////原始文本             

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
        /// 是否在线
        /// </summary>
        [DataMember]
        public int IsOnLine { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [DataMember]
        public string VehicleId { get; set; }

        /// <summary>
        /// 原始文本
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        /// <summary>
        /// 安全套件ID
        /// </summary>
        [DataMember]
        public string SuiteInfoId { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Longitude)))
            {
                builder.AppendLine("Longitude:" + Longitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Latitude)))
            {
                builder.AppendLine("Latitude:" + Latitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Speed)))
            {
                builder.AppendLine("Speed:" + Speed.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Direction)))
            {
                builder.AppendLine("Direction:" + Direction.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GPSTime)))
            {
                builder.AppendLine("GPSTime:" + GPSTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GPSValid)))
            {
                builder.AppendLine("GPSValid:" + GPSValid.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IsOnLine)))
            {
                builder.AppendLine("IsOnLine:" + IsOnLine.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Context)))
            {
                builder.AppendLine("Context:" + Context.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteInfoId)))
            {
                builder.AppendLine("SuiteInfoId:" + SuiteInfoId.ToString());
            }
            return builder.ToString();
        }

    }
}
