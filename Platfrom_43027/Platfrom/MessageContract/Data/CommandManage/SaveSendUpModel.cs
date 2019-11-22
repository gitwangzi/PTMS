/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5e7aca98-a53e-41aa-92cc-ec814c2ff538      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: SaveGpsSendUpModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/28 15:42:51
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/28 15:42:51
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
    public class SaveSendUpModel
    {
        public int PacketSeq { get; set; }

        public string ID { get; set; }

        public DateTime Send_Time { get; set; }

        public string Vehicle_id { get; set; }

        public string Mdvr_core_sn { get; set; }

        public string Operation_id { get; set; }

        public string Cmd_Type { get; set; }

        public string Cmd_Desc { get; set; }

        public byte[] Cmd_content { get; set; }

        public int Status { get; set; }

        public DateTime Create_Time { get; set; }

        public string Cmd_Exchange { get; set; }

        public string Cmd_Route { get; set; }

        public DateTime Requst_time { get; set; }

        public string Cmd_Sub_Type { get; set; }

        public string RuleID { get; set; }

        public string UserName { get; set; }

        public override string ToString()
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("insert into SEND_RECORD (ID, SEND_TIME, VEHICLE_ID, MDVR_CORE_SN, OPERATION_ID, CMD_TYPE, CMD_DESC, CMD_CONTENT, STATUS, CREATE_TIME, CMD_EXCHANGE, CMD_ROUTE, REQUEST_TIME, CMD_SUB_TYPE, USER_NAME, RULE_NAME) values ('");
            sbSql.Append(this.ID.ToString());
            sbSql.Append("',TO_DATE('");
            sbSql.Append(this.Send_Time.ToString("yyyy-MM-dd HH:mm:ss"));
            sbSql.Append("','yyyy-mm-dd hh24:mi:ss')");
            sbSql.Append(",'");
            sbSql.Append(this.Vehicle_id);
            sbSql.Append("','");
            sbSql.Append(this.Mdvr_core_sn);
            sbSql.Append("','");
            sbSql.Append(this.Operation_id);
            sbSql.Append("','");
            sbSql.Append(this.Cmd_Type);
            sbSql.Append("','");
            sbSql.Append(this.Cmd_Desc);
            sbSql.Append("','");
            sbSql.Append(GetCmdContent(this.Cmd_content));
            sbSql.Append("','");
            sbSql.Append(this.Status);
            sbSql.Append("',TO_DATE('");
            sbSql.Append(this.Create_Time.ToString("yyyy-MM-dd HH:mm:ss"));
            sbSql.Append("','yyyy-mm-dd hh24:mi:ss')");
            sbSql.Append(",'");
            sbSql.Append(this.Cmd_Exchange);
            sbSql.Append("','");
            sbSql.Append(this.Cmd_Route);
            sbSql.Append("',TO_DATE('");
            sbSql.Append(this.Requst_time.ToString("yyyy-MM-dd HH:mm:ss"));
            sbSql.Append("','yyyy-mm-dd hh24:mi:ss')");
            sbSql.Append(",'");
            sbSql.Append(this.Cmd_Sub_Type);
            sbSql.Append("','");
            sbSql.Append(string.IsNullOrEmpty(this.UserName) ? "" : this.UserName.ToLower());
            sbSql.Append("','");
            sbSql.Append(this.RuleID);
            sbSql.Append("');");
            return sbSql.ToString();
        }

        private StringBuilder GetCmdContent(byte[] cmdContent)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < cmdContent.Length; i++)
            {
                sb.Append(Convert.ToString(cmdContent[i], 16));
            }
            return sb;
        }
    }
}
