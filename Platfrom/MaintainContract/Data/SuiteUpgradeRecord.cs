/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0ad4fc64-d717-46ca-9264-eaf43aecb063      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Contract.Data
/////    Project Description:    
/////             Class Name: SuiteUpgradeRecord
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/11 11:09:47
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/11 11:09:47
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Gsafety.PTMS.BaseInformation.Contract.Data;

namespace Gsafety.PTMS.Maintain.Contract.Data
{
    /// <summary>
    /// 安全套件升级详情
    /// </summary>
    [DataContract]
    class SuiteUpgradeRecord
    {
        /// <summary>
        /// 维修记录编号
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// 安全套件表外键
        /// </summary>
        [DataMember]
        public string SuiteInfoId { get; set; }

        ///<summary>
        ///MDVR芯片号
        ///</summary>
        [DataMember]
        public string MdvrCoreSn;

        ///<summary>
        ///升级结果
        ///</summary>
        [DataMember]
        public string UpdateResult;

        ///<summary>
        ///升级后版本号
        ///</summary>
        [DataMember]
        public string CurrVersion;

        ///<summary>
        ///升级前版本号
        ///</summary>
        [DataMember]
        public string LastVersion;

        ///<summary>
        ///上一次升级时间
        ///</summary>
        [DataMember]
        public DateTime? LastUpdateTime;

        ///<summary>
        ///开始升级时间
        ///</summary>
        [DataMember]
        public DateTime? UpdateStartTime;

        ///<summary>
        ///升级结束时间
        ///</summary>
        [DataMember]
        public DateTime? UpdateEndTime;

        ///<summary>
        ///操作人
        ///</summary>
        [DataMember]
        public string Operator;

        ///<summary>
        ///状态
        ///</summary>
        [DataMember]
        public DeviceSuiteStatus Status;

        ///<summary>
        ///升级xml内容
        ///</summary>
        [DataMember]
        public string Context;

        ///<summary>
        ///操作时间
        ///</summary>
        [DataMember]
        public DateTime? Oper_Time;

        ///<summary>
        ///经度
        ///</summary>
        [DataMember]
        public string Longitude;

        ///<summary>
        ///纬度
        ///</summary>
        [DataMember]
        public string Latitude;

        ///<summary>
        ///速度
        ///</summary>
        [DataMember]
        public string Speed;

        ///<summary>
        ///方向
        ///</summary>
        [DataMember]
        public string Direction;

        ///<summary>
        ///GPS时间
        ///</summary>
        [DataMember]
        public DateTime? Gps_Time;

        ///<summary>
        ///GPS是否有效
        ///</summary>
        [DataMember]
        public int GPS_VALID;

        ///<summary>
        ///原始命令文本
        ///</summary>
        [DataMember]
        public string OriginalCmd;
    }
}
