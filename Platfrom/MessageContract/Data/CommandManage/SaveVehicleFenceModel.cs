/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 594ac945-ebc4-48e8-8356-7ca8647fd79e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: SaveVehicleFenceModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/1 11:19:09
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/1 11:19:09
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
    public class SaveVehicleFenceModel
    {
        public string ID { get; set; }

        public string Vehicle_ID { get; set; }

        public int Fence_ID { get; set; }

        public string Mdvr_Core_ID { get; set; }

        public int Status { get; set; }

        public string Creator { get; set; }

        public DateTime Create_Time { get; set; }

        public override string ToString()
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("insert into VEHICLE_FENCE (ID, VEHICLE_ID, FENCE_ID, MDVR_CORE_SN, STATUS, CREATOR, CREATE_TIME) values ('");
            sbSql.Append(this.ID.ToString());
            sbSql.Append("','");
            sbSql.Append(this.Vehicle_ID);
            sbSql.Append("','");
            sbSql.Append(this.Fence_ID);
            sbSql.Append("','");
            sbSql.Append(this.Mdvr_Core_ID);
            sbSql.Append("','");
            sbSql.Append(this.Status);
            sbSql.Append("','");
            sbSql.Append(this.Creator);
            sbSql.Append("',TO_DATE('");
            sbSql.Append(this.Create_Time.ToString("yyyy-MM-dd HH:mm:ss"));
            sbSql.Append("','yyyy-mm-dd hh24:mi:ss'));");
            return sbSql.ToString();
        }
    }
}
