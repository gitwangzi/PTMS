using Gsafety.PTMS.Base.Entity;
using Gsafety.Common.Util;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7afcf540-5afc-48c1-9b30-44df43e738f3      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Entity
/////    Project Description:    
/////             Class Name: V61
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/2 9:49:27
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/2 9:49:27
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Alarm.Entity
{
    /// <summary>
    /// 一键报警业务实体V61
    /// </summary>
    [DataContract]
    [Serializable]
    public class V61 : IConvertModel
    {
        /// <summary>
        /// 警情ID
        /// </summary>
       [DataMember]
        public string AlarmUid { get; set; }

        /// <summary>
        /// 报警时间
        /// </summary>
        [DataMember]
        public DateTime AlarmTime { get; set; }

        /// <summary>
        /// 套件信息
        /// </summary>
        [DataMember]
        public SuiteInfo SuiteInfo { get; set; }

        /// <summary>
        /// 位置数据
        /// </summary>
        [DataMember]
        public GPSLocation GPS { get; set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        /// <summary>
        /// 将Model转换为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ConvertToString(object obj)
        {
            return null;
        }

        /// <summary>
        /// 将字符串转换为实体
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public object ConvertToModel(string str)
        {
            ////99dc[指令长度],[MDVR芯片号],[消息序列号],指令关键字,[位置和状态],
            ////触发时位置和状态,警情UID,发送次数序号,按钮序号,按钮别名#
            ////指令长度99dc0194,MDVR芯片号0108002821,消息序列号,指令关键字V61,指令发送时间130729 033627,定位状态A,经度11422.822265,纬度2243.110839,地面速率0.00,地面航向202.93,部件状态及警情标志0000000008080C00,电源状态与GPS模块状态0000000000010000,安全套件温度703.23,车厢内温度21.97,IO警情状态及网络状态0000E011.0008,线路号26,司机编号102,下一站编号301,停靠标志1,车上人数0,,,,,,,,,,,,,,,,,警情UID111670853010,发送次数序号1,按钮序号3,按钮别名EmergencyAlarm#
            if (string.IsNullOrEmpty(str)) return null;
            string[] alarmArray = str.Split(',');
            V61 alarm = new V61();
            if (alarmArray[3].ToLower().Equals("v61"))
            {
                if (alarm.SuiteInfo == null)
                    alarm.SuiteInfo = new Base.Entity.SuiteInfo();   ////安全套件信息
                alarm.SuiteInfo.MdvrCoreSN = alarmArray[1];          ////芯片号

                if (alarm.GPS == null)
                    alarm.GPS = new GPSLocation();      ////位置数据
                //alarm.GPS.Context = alarmArray[4];    ////GPS原始消息
                alarm.GPS.GpsValid = alarmArray[5];     ////GPS有效性
                alarm.GPS.Longitude = alarmArray[6];    ////经度
                alarm.GPS.Latitude = alarmArray[7];     ////纬度
                alarm.GPS.Speed = alarmArray[8];        ////速度
                alarm.GPS.Direction = alarmArray[9];    ////方向

                alarm.Context = str;                    ////原始文本

                var time = ConvertHelper.ConvertStrToDate(alarmArray[4], "yyMMdd HHmmss");
                if (time != null)
                {
                    alarm.AlarmTime = time.Value;       ////报警时间“yymmdd hhmmss”，指令发生时安全套件上的时间
                    alarm.GPS.GpsTime = time.Value;     ////GPS时间
                }

                alarm.AlarmUid = alarmArray[36];        ////UID
            }

            return alarm;
        }
    }
}
