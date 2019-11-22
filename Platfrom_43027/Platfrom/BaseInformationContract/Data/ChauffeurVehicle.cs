using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    [DataContract]
    public class ChauffeurVehicle
    {
       private string _id;
        ///<summary>
        ///主键
        ///</summary>
        [DataMember]
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        string _vehicleid;
        ///<summary>
        ///用户ID
        ///</summary>
        [DataMember]
        public string VehicleID
        {
            get
            {
                return _vehicleid;
            }
            set
            {
                _vehicleid = value;
            }
        }

        string _chauffeurid;
        ///<summary>
        ///安装点ID
        ///</summary>
        [DataMember]
        public string ChauffeurID
        {
            get
            {
                return _chauffeurid;
            }
            set
            {
                _chauffeurid = value;
            }
        }

        DateTime _createtime;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public DateTime CreateTime
        {
            get
            {
                return _createtime;
            }
            set
            {
                _createtime = value;
            }
        }

        int _activate;
        ///<summary>
        ///安装点ID
        ///</summary>
        [DataMember]
        public int Activate
        {
            get
            {
                return _activate;
            }
            set
            {
                _activate = value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ChauffeurID)))
            {
                builder.AppendLine("ChauffeurID:" + ChauffeurID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("DistrictCode:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Activate)))
            {
                builder.AppendLine("Activate:" + Activate.ToString());
            }
            return builder.ToString();
        }
    }
}
