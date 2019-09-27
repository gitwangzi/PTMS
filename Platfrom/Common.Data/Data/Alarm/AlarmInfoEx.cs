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
    public class AlarmInfoEx
    {
        [DataMember]
        public string ID { get; set; }//ID

        [DataMember]
        public string ClientId { get; set; }//云账号客户ID

        [DataMember]
        public string MdvrCoreId { get; set; }//芯片号

        [DataMember]
        public string SuiteInfoID { get; set; }//安全套件信息GUID

        [DataMember]
        public int? SuiteStatus { get; set; }//套件状态

        [DataMember]
        public string SuiteID { get; set; }//套件号

        [DataMember]
        public string AlarmMobile { get; set; }//报警电话

        [DataMember]
        public string VehicleId { get; set; }//车牌号

        [DataMember]
        public string VehicleType { get; set; }//车辆类型

        [DataMember]
        public string VehicleOwner { get; set; }//车主名称

        [DataMember]
        public string OwnerPhone { get; set; }//车主电话

        [DataMember]
        public string Province { get; set; }//省份

        [DataMember]
        public string City { get; set; }//城市

        [DataMember]
        public string DistrictCode { get; set; }//区域代码

        [DataMember]
        public string GpsValid { get; set; }//gps是否有效

        [DataMember]
        public int ButtonNum { get; set; }//报警按钮编号

        [DataMember]
        public int AlarmStatus { get; set; }//报警按钮编号

        [DataMember]
        public string AlarmGuid { get; set; }

        [DataMember]
        public DateTime? AlarmTime { get; set; }

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

        [DataMember]
        public List<string> Organizations { get; set; }

        [DataMember]
        public string AdditionalInfo { get; set; }//附加信息

        [DataMember]
        public string User { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public int TransferStatus { get; set; }

        [DataMember]
        public int AppealStatus { get; set; }

        [DataMember]
        public int DisposalStatus { get; set; }

        [DataMember]
        public int Source { get; set; }

        [DataMember]
        public string VehicleSn { get; set; }

        [DataMember]
        public string BrandModel { get; set; }

        /// <summary>
        /// OperationLincese
        /// </summary>
        [DataMember]
        public string OperationLincese { get; set; }

        [DataMember]
        public string Keyword { get; set; }

        [DataMember]
        public string AlarmContent { get; set; }

        /// <summary>
        /// 事件等级 
        /// </summary>
        [DataMember]
        public string IncidentLevel { get; set; }

        /// <summary>
        /// 事发地址
        /// </summary>
        [DataMember]
        public string IncidentAddress { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        [DataMember]
        public string IncidentType { get; set; }

    }
}
