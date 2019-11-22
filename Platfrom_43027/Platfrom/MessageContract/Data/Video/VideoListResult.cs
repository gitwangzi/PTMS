using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data.Video
{
    [DataContract]
    [Serializable]
    public class VideoListResult
    {
        [DataMember]
        public List<MdvrFileListResult> lstResult { get; set; }
    }
}
