using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    [Serializable]
    public class DeviceAlertEx
    {
        [DataMember]
        public string ClientId { get; set; }//云账号客户ID

        [DataMember]
        public string Id { get; set; }//GUID

        [DataMember]
        public string MdvrCoreId { get; set; }//芯片号

        [DataMember]
        public string VehicleId { get; set; }//车牌号

        [DataMember]
        public string SuiteId { get; set; }//套件号

        [DataMember]
        public string SuiteInfoId { get; set; }//套件信息的GUID

        [DataMember]
        public int? SuiteStatus { get; set; }//套件状态

        [DataMember]
        public int? AlertType { get; set; }//设备告警类型

        [DataMember]
        public string AlertTypeName { get; set; }//设备告警类型名称

        [DataMember]
        public string Cmd { get; set; }//指令名称

        [DataMember]
        public string GpsValid { get; set; }//GPS是否有效

        [DataMember]
        public string Province { get; set; }//省份

        [DataMember]
        public string City { get; set; }//城市

        [DataMember]
        public string DistrictCode { get; set; }//区域代码

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
        public List<string> Organizations { get; set; }


        [DataMember]
        public string AdditionalInfo { get; set; }//附加信息
    }
}
