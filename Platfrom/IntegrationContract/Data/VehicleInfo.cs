/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 71e3bb3d-109e-4aa7-b96f-3f58bf8350bf      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: GPS
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 11:12:00
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/8 11:12:00
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Integration.Contract.Data
{
    /// <summary>
    /// 车辆信息
    /// </summary>
    [DataContract]
    public class VehicleInfo
    {
        /// <summary>
        /// 芯片号（用于识别设备）
        /// </summary>]
        [DataMember]
        public string mdvr_core_sn { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [DataMember]
        public string CarNumber { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        [DataMember]
        public string VehicleSn { get; set; }

        /// <summary>
        /// 车辆品牌及型号
        /// </summary>
        [DataMember]
        public string BrandModel { get; set; }


        /// <summary>
        /// 车辆类型（1：出租车；2：公交车；3：长途巴士）
        /// </summary>
        [DataMember]
        public int VehicleType { get; set; }

        /// <summary>
        /// 所属车辆公司名称
        /// </summary>
        [DataMember]
        public string Company { get; set; }
        
        /// <summary>
        /// 联系电话
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }

        /// <summary>
        /// 车主姓名
        /// </summary>
        [DataMember]
        public string Owner { get; set; }

        /// <summary>
        /// 运营证
        /// </summary>
        [DataMember]
        public string OperationLincese { get; set; }

        /// <summary>
        /// 车况说明
        /// </summary>
        [DataMember]
        public string Note { get; set; }

        /// <summary>
        /// 区域（所辖行政区域ECU-911和公共交通整体安全系统用同一套）
        /// </summary>
        [DataMember]
        public string District { get; set; }

        /// <summary>
        /// 年限
        /// </summary>
        [DataMember]
        public string StartYear { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(mdvr_core_sn)))
            {
                builder.AppendLine("mdvr_core_sn:" + mdvr_core_sn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CarNumber)))
            {
                builder.AppendLine("CarNumber:" + CarNumber.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleSn)))
            {
                builder.AppendLine("VehicleSn:" + VehicleSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(BrandModel)))
            {
                builder.AppendLine("BrandModel:" + BrandModel.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleType)))
            {
                builder.AppendLine("VehicleType:" + VehicleType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Company)))
            {
                builder.AppendLine("Company:" + Company.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Mobile)))
            {
                builder.AppendLine("Mobile:" + Mobile.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Owner)))
            {
                builder.AppendLine("Owner:" + Owner.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OperationLincese)))
            {
                builder.AppendLine("OperationLincese:" + OperationLincese.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Note.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(District)))
            {
                builder.AppendLine("District:" + District.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StartYear)))
            {
                builder.AppendLine("StartYear:" + StartYear.ToString());
            }
            return builder.ToString();
        }

    }
}
