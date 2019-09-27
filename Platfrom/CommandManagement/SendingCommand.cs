/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: a8defadf-5521-4bad-acc2-b95292c864a7      
/////             clrversion: 4.0.30319.34003
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: CommandManagement
/////    Project Description:    
/////             Class Name: SendingCommands
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/2/7 04:19:39
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/2/7 04:19:39
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
    /// 正在下发的命令
    /// </summary>
    public class SendingCommand
    {
        public string RecordID { get; set; }              ////////////////命令发送记录ID，数据表SEND_RECORD表的ID              
        public string OPERATION_ID { get; set; }          ////////////////关联操作表的ID              
        public string VehicleID { get; set; }             ////////////////车牌号              
        public string DeviceID { get; set; }              ////////////////设备编号（MDVR_CORE_ID或Raptor GPS IMEI)              
        public DateTime SendindTime { get; set; }         ////////////////命令下发的时间     
        public DateTime SendingTimeout { get; set; }      ////////////////命令下发超时时间 
    }                                                     
}
