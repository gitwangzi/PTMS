using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class DelRouteRegionEx : DelRouteRegion
    {

        [DataMember]
        public string MdvrCoreId { get; set; }//芯片号

        [DataMember]
        public string SuiteInfoID { get; set; }//安全套件信息GUID

        [DataMember]
        public short? SuiteStatus { get; set; }//套件状态

        [DataMember]
        public string SuiteID { get; set; }//套件号

        [DataMember]
        public List<string> VehicleList { get; set; }//车牌号列表

        [DataMember]
        public List<string> DevList { get; set; }//芯片号列表

        [DataMember]
        public string PolygonsRegionId { get; set; }//区域Id

        [DataMember]
        public int Operation { get; set; }//操作类型

        [DataMember]
        public int FenceId { get; set; }//电子围栏

        [DataMember]
        public string MsgId { get; set; }//操作类型

        [DataMember]
        public string UserName { get; set; }//操作类型

        //[DataMember]
        //public Hashtable VehicleList { get; set; }//车牌号列表

        [DataMember]
        public DateTime SendTime { get; set; }

        [DataMember]
        public string DvId { get; set; }//芯片号
    }
}
