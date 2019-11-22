using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.TransferLib
{
    class AppealModel
    {
        //GSEYE
        //public string alarmId { get; set; }
        //public string vehicleID { get; set; }
        //public string gpsTime { get; set; }
        //public string longitude { get; set; }
        //public string latitude { get; set; }

        //public string alarmSource { get; set; }
        //public string deviceID { get; set; }
        //public string contactPhone { get; set; }
        //public string contact { get; set; }
        //public string Direction { get; set; }

        //public string speed { get; set; }
        //public string gpsValid { get; set; }
        //public string vehicleSn { get; set; }
        //public string brandModel { get; set; }
        //public string vehicleType { get; set; }

        //public string operationLincense { get; set; }
        //public string district { get; set; }
        //public string districtName { get; set; }
        //public string clientID { get; set; }
        //public string uri { get; set; }

        //RD01
        //public string incidentAppealId { get; set; }
        //public int incidentType { get; set; }
        //public int alarmType { get; set; }
        //public string alarmPerson{get;set;}
        //public string alarmPhone{get;set;}
        //public string alarmAddress { get; set; }
        //public double longitude {get;set;}
        //public double latitude { get; set; }
        //public string alarmDescription { get; set; }
        //public string appealPersonId { get; set; }
        //public string appealPersonName { get; set; }


        //RD02
        /// <summary>
        /// 中心编码，必填
        /// </summary>
        public string centerCode { get; set; }
        /// <summary>
        /// 设备编号，必填
        /// </summary>
        public string deviceId { get; set; }
        /// <summary>
        /// 设备名称，必填
        /// </summary>
        public string deviceName { get; set; }
        /// <summary>
        /// 厂商系统名称，英文，必填
        /// </summary>
        public string systemName { get; set; }
        /// <summary>
        /// 厂商编号，英文，必填
        /// </summary>
        public string factoryCode { get; set; }
        /// <summary>
        /// 报警关联id，长度小于40，必填
        /// </summary>
        public string alarmId { get; set; }
        /// <summary>
        /// 报警类型，视频报警为6，一键报警为5，必填
        /// </summary>
        public string alarmType { get; set; }
        /// <summary>
        ///报警人编号
        /// </summary>
        public string alarmPersonId { get; set; }
        /// <summary>
        /// 报警人姓名
        /// </summary>
        public string alarmPersonName { get; set; }
        /// <summary>
        /// 报警人电话
        /// </summary>
        public string alarmPersonPhone { get; set; }
        /// <summary>
        /// 报警描述
        /// </summary>
        public string alarmDescription { get; set; }
        /// <summary>
        /// 事发时间，必填
        /// </summary>
        public string incidentTime { get; set; }
        /// <summary>
        /// 事件等级，1一般；2较大；3重大；4特别重大，必填
        /// </summary>
        public string incidentLevel { get; set; }

        public string incidentType { get; set; }
        /// <summary>
        /// 事发地址，必填
        /// </summary>
        public string incidentAddress { get; set; }
        /// <summary>
        /// 行政区划
        /// </summary>
        public string districtCode { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string latitude { get; set; }
        /// <summary>
        /// 安装人姓名
        /// </summary>
        public string installerName { get; set; }
        /// <summary>
        /// 安装人电话
        /// </summary>
        public string installerPhone { get; set; }
        /// <summary>
        /// 安装时间
        /// </summary>
        public string installationTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string creatTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public string updateTime { get; set; }
        /// <summary>
        /// 报警关联文件
        /// </summary>
       //public List<AlarmFileInfo> alarmFileInfos { get; set; }
    }
}
