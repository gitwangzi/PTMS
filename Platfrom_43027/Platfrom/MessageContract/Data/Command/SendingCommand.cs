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
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// Sending Command
    /// </summary>
    public class SendingCommand
    {
        public int PacketSeq { get; set; }                                      //数据包序号
        public string RecordID { get; set; }                                       ////////////////RecordID,Concur with ID in Database                
        public string DeviceID { get; set; }                                       ////////////////DeviceID(MDVR_CORE_ID or Raptor GPS IMEI) 
        public string CommandType { get; set; }                                    ////////////////Command Type
        public string OperateType { get; set; }                                    ////////////////Operate Type(For Instance Add,Modify and Delete )
        public string OperationID { get; set; }                                    ////////////////OperationID,For Instance Electric Fence
        public DateTime RequestTime { get; set; }                                  ////////////////Request Time
        public DateTime SendingTime { get; set; }                                  ////////////////Sending Time     
        public DateTime SendingTimeout { get; set; }                               ////////////////Sending Timeout 
        public Action<PTMSEntities, SendingCommand> TimeoutAction { get; set; }                  ////////////////Timeout Action
        public byte[] SendCommandBytes { get; set; }
        public CommandSendStatus commandSendStatus { get; set; }
        public string RuleID { get; set; }
        public string UserName { get; set; }
        public string VehicleID { get; set; }

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
            if (!string.IsNullOrEmpty(Convert.ToString(RequestTime)))
            {
                builder.AppendLine("RequestTime:" + RequestTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SendingTime)))
            {
                builder.AppendLine("SendingTime:" + SendingTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SendingTimeout)))
            {
                builder.AppendLine("SendingTimeout:" + SendingTimeout.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(TimeoutAction)))
            {
                builder.AppendLine("TimeoutAction:" + TimeoutAction.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SendCommandBytes)))
            {
                builder.AppendLine("SendCommandBytes:" + SendCommandBytes.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(commandSendStatus)))
            {
                builder.AppendLine("commandSendStatus:" + commandSendStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RuleID)))
            {
                builder.AppendLine("RuleID:" + RuleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserName)))
            {
                builder.AppendLine("UserName:" + UserName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            return builder.ToString();
        }

    }
}
