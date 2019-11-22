using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.SIDS.Contract.Data
{
    [DataContract]
    public class Vehicle_Video_Model
    {
        [DataMember]
        public string File_Name { get; set; }

        [DataMember]
        public DateTime? Create_Time { get; set; } 
    }
}
