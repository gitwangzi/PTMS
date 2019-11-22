/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 160b0035-8371-42b8-8ed3-10a5412f3fd4      
/////             clrversion: 4.0.30319.34003
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: CommandManagement
/////    Project Description:    
/////             Class Name: WaitSendCommand
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/2/7 04:18:55
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/2/7 04:18:55
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandManagement
{
    /// <summary>
    /// 等待下发命令
    /// </summary>
    public class WaitSendCommand
    {
        public string RecordID { get; set; }                   ////////////////命令发送记录ID          
        public string OPERATION_ID { get; set; }               ////////////////关联操作表的ID
        public string VehicleID { get; set; }                  ////////////////车牌号
        public string DeviceID { get; set; }                   ////////////////设备编号（MDVR_CORE_ID或Raptor GPS IMEI)
        public string CommandContent { get; set; }             ////////////////下发命令内容
        public DateTime RequestSendTime { get; set; }          ////////////////命令请求发送时间
        public DateTime RequestTimeout { get; set; }           ////////////////命令发送超时时间
    }
}
