using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Gsafety.PTMS.Common.Data
{
    [Serializable]
    [DataContract]
    public class TakePictureMessageResponse
    {
        [DataMember]
        public string UserID { get; set; }

        [DataMember]
        public Photo Photo { get; set; }
    }
}
