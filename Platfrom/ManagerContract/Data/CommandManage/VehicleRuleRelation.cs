using Gsafety.PTMS.Base.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Manager.Contract.Data.CommandManage
{
    [Serializable]
    [DataContract]
    public class VehicleRuleRelation
    {
        [DataMember]
        public List<string> Vehicles;
    }
}
