/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c6d27323-2f46-4f22-a94b-582a2e398a07      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.Ant.Message.Contract.Data.Alarm
/////    Project Description:    
/////             Class Name: AntInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/27 11:43:18
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/27 11:43:18
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Ant.Message.Contract.Data
{
    /// <summary>
    /// VehicleAlarmInfo
    /// </summary>
    [DataContract]
    public class VehicleAlarmInfo
    {
        /// <summary>
        /// ANT报警Id
        /// </summary>
        [DataMember(IsRequired = true)]
        [Required]
        public string AlarmId { get; set; }

        /// <summary>
        /// mdvr芯片号（用于识别设备）
        /// </summary>
        [DataMember(IsRequired = true)]
        [Required]
        public string mdvr_core_sn { get; set; }
        /// <summary>
        /// 报警时间
        /// </summary>
        [DataMember]
        public DateTime AlarmTime { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember(IsRequired = true)]
        [Range(-180, 180)]
        public decimal Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember(IsRequired = true)]
        [Range(-90,90)]
        public decimal Latitude { get; set; }
      
        /// <summary>
        /// GPS时间
        /// </summary>
        [DataMember]
        public DateTime GPSTime { get; set; }
        
        /// <summary>
        /// 速度
        /// </summary>
        [DataMember]
        public decimal Speed { get; set; }
        
        /// <summary>
        /// 方向
        /// </summary>
        [DataMember]
        public string Direction { get; set; }
        
        /// <summary>
        /// 区域
        /// </summary>
        [DataMember]
        public string AssigedArea { get; set; }
        
        /// <summary>
        /// 车辆编号
        /// </summary>
        [DataMember(IsRequired = true)]
        [Required]
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
        /// 车辆类型
        /// </summary>
        [DataMember]
        public int VehicleType { get; set; }
        
        /// <summary>
        /// 所属公司
        /// </summary>
        [DataMember]
        public string Company { get; set; }
        
        /// <summary>
        /// 联系电话
        /// </summary>
        [DataMember(IsRequired = true)]
        [Required]
        public string Mobile { get; set; }
        
        /// <summary>
        /// 运营证
        /// </summary>
        [DataMember]
        public string OperationLincese { get; set; }
       
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Note { get; set; }

        /// <summary>
        /// 车主姓名
        /// </summary>
        [DataMember(IsRequired = true)]
        [Required]
        public string Owner { get; set; }

        /// <summary>
        /// <summary>
        /// 区域（所辖行政区域ECU-911和公共交通整体安全系统用同一套)
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
            return string.Format("AlarmId:[{0}]  mdvr_core_sn:[{1}]  AlarmTime:[{2}]  Longitude:[{3}]  Latitude:[{4}]  GPSTime:[{5}]  Speed:[{6}]  Direction:[{7}]  AssigedArea:[{8}]   CarNumber:[{9}]  VehicleSn:[{10}]  BrandModel:[{11}]  VehicleType:[{12}] Company:[{13}]  Mobile:[{14}]  OperationLincese:[{15}]  Note:[{16}]  Owner:[{17}]  District:[{18}]   StartYear:[{19}] ", this.AlarmId, this.mdvr_core_sn, this.AlarmTime, this.Longitude, this.Latitude, this.GPSTime, this.Speed, this.Direction, this.AssigedArea, this.CarNumber, this.VehicleSn, this.BrandModel, this.VehicleType, this.Company, this.Mobile, this.OperationLincese, this.Note, this.Owner, this.District, this.StartYear);
        }
    }
}
