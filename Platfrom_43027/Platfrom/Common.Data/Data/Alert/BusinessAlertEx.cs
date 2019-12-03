using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    [Serializable]
    public class BusinessAlertEx
    {
        [DataMember]
        public string ClientId { get; set; }//云账号客户ID

        [DataMember]
        public string Id { get; set; }//GUID

        [DataMember]
        public string MdvrCoreId { get; set; }//芯片号

        [DataMember]
        public string SuiteInfoID { get; set; }//安全套件信息GUID

        [DataMember]
        public int? SuiteStatus { get; set; }//套件状态

        [DataMember]
        public string SuiteID { get; set; }//套件号

        [DataMember]
        public string VehicleId { get; set; }//车牌号

        [DataMember]
        public string VehicleType { get; set; }//车辆类型

        [DataMember]
        public string VehicleOwner { get; set; }//车主

        [DataMember]
        public string OwnerPhone { get; set; }//车主电话

        [DataMember]
        public string Province { get; set; }//省份

        [DataMember]
        public string City { get; set; }//城市

        [DataMember]
        public string DistrictCode { get; set; }//区域代码

        [DataMember]
        public int? AlertType { get; set; }//业务告警类型

        [DataMember]
        public string GpsValid { get; set; }//GPS是否有效

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Height { get; set; }

        [DataMember]
        public string Speed { get; set; }

        [DataMember]
        public string Direction { get; set; }

        [DataMember]
        public string OrganizationId { get; set; }

        [DataMember]
        public string OrganizationName { get; set; }

        /// <summary>
        /// 车辆告警处置内容
        /// </summary>
        [DataMember]
        public string Note { get; set; }

        DateTime? _gpstime = null;
        [DataMember]
        public DateTime? GpsTime
        {
            get
            {
                return _gpstime;
            }
            set
            {
                _gpstime = value;
            }
        }

        DateTime? _alerttime = null;
        [DataMember]
        public DateTime? AlertTime
        {
            get
            {
                return _alerttime;
            }
            set
            {
                _alerttime = value;
            }
        }

        [DataMember]
        public int Status { get; set; }


        [DataMember]
        public string AdditionalInfo { get; set; }//附加信息

        [DataMember]
        public List<string> Organizations { get; set; }

        /// <summary>
        /// 车辆告警处置时间
        /// </summary>
        [DataMember]
        public DateTime? HandleTime { get; set; }

        /// <summary>
        /// 车辆告警处置人
        /// </summary>
        [DataMember]
        public string HandlePerson { get; set; }

        [DataMember]
        public int AlertLevel { get; set; }
    }
}
