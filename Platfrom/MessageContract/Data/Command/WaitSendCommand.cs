using Gsafety.PTMS.DBEntity;
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

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// Wait Send Command
    /// </summary>
    public class WaitSendCommand
    {
        public int PacketSeq { get; set; }                                         //数据包序号
        public string RecordID { get; set; }                                          ////////////////Record ID
        public string DeviceID { get; set; }                                          ////////////////Device ID(MDVR_CORE_ID or Raptor GPS IMEI)
        public string CommandType { get; set; }                                       ////////////////Command Type 
        public string OperateType { get; set; }                                       ////////////////Operate Type(For Instance Add,Modify and Delete)
        public string OperationID { get; set; }                                       ////////////////OperationID,For Instance Electric Fence
        public string Exchange { get; set; }                                          ////////////////Exchange
        public string RouteKey { get; set; }                                          ////////////////Route Key
        public byte[] CommandContent { get; set; }                                    ////////////////Command Content
        public DateTime RequestSendTime { get; set; }                                 ////////////////Request Send Time
        public DateTime RequestTimeout { get; set; }                                  ////////////////Request Timeout
        public Action<PTMSEntities,WaitSendCommand> TimeoutAction { get; set; }                    ////////////////Time out Action
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(RecordID)))
            {
                builder.AppendLine("RecordID:" + RecordID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DeviceID)))
            {
                builder.AppendLine("DeviceID:" + DeviceID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CommandType)))
            {
                builder.AppendLine("CommandType:" + CommandType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OperateType)))
            {
                builder.AppendLine("OperateType:" + OperateType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OperationID)))
            {
                builder.AppendLine("OperationID:" + OperationID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Exchange)))
            {
                builder.AppendLine("Exchange:" + Exchange.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RouteKey)))
            {
                builder.AppendLine("RouteKey:" + RouteKey.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CommandContent)))
            {
                builder.AppendLine("CommandContent:" + CommandContent.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RequestSendTime)))
            {
                builder.AppendLine("RequestSendTime:" + RequestSendTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RequestTimeout)))
            {
                builder.AppendLine("RequestTimeout:" + RequestTimeout.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(TimeoutAction)))
            {
                builder.AppendLine("TimeoutAction:" + TimeoutAction.ToString());
            }
            return builder.ToString();
        }

    }
}
