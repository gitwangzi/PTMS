using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Message.Contract.Data.CommandManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data
{
    [Serializable]
    [DataContract]
    public class SendInfomationModel : BaseSettingModel
    {
        [DataMember]
        public SendInfomationCMD Setting;


    }
}
