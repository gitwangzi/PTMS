using System;
using System.Runtime.Serialization;


namespace Gsafety.PTMS.Message.Contract.Data
{
    [DataContract]
    [Serializable]
    public class ChangeGroupVehicle
    {
        [DataMember] 
        public string MdvrCoreId;

        [DataMember] 
        public string VehicleId;

        [DataMember]
        public string TargetGroupId;

        [DataMember]
        public string CreateUser;

    }
}
