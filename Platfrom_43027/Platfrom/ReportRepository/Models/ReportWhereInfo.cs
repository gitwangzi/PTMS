/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: cd6de673-87a1-448d-8c53-2ec3a829f7b9      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract.Data
/////    Project Description:    
/////             Class Name: ReportWhereInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/8/5 10:52:57
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/8/5 10:52:57
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Threading;

namespace Gsafety.PTMS.Report.Repository
{
    /// <summary>
    /// for report Parameters["whereinfo"]
    /// 报表传递参数类
    /// </summary>
    public class ReportWhereInfo
    {
        public ReportWhereInfo()
        {
            DataFromat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern;
            Language = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
        }
        /// <summary>
        /// BeginTime
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// EndTime
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        public DateTime LocalTime { get; set; }

        /// <summary>
        /// 日期显示格式化样式
        /// </summary>
        public string DataFromat { get; set; }

        /// <summary>
        /// 当前语言
        /// </summary>
        public string Language { get; set; }

        public DateTime BeginTimeLocal { get; set; }

        public DateTime EndTimeLocal { get; set; }

        public int? AlarmSource { get; set; }

        public string VehicleType { get; set; }

        public string VehicleId { get; set; }

        public double TimeZone { get; set; }

        public string ClientID { get; set; }

        public int GroupMode { get; set; }

        #region Fields
        private string _Province;
        private string _City;
        private string _Region;
        private string _OrgCode;
        #endregion Fields

        #region Attributes

        public string Province
        {
            get { return _Province; }
            set { _Province = value; }
        }

        /// <summary>
        /// 城市
        /// </summary>
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }

        /// <summary>
        /// 区域
        /// </summary>
        public string Region
        {
            get { return _Region; }
            set { _Region = value; }
        }

        /// <summary>
        /// 组织机构编码
        /// </summary>
        public string OrgCode
        {
            get { return _OrgCode; }
            set { _OrgCode = value; }
        }

        #endregion Attributes
    }
}
