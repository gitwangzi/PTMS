using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class Point
    {
        [DataMember]
        public string Longitude { get; set; }//经度

        [DataMember]
        public string Latitude { get; set; }//纬度
    }
}
