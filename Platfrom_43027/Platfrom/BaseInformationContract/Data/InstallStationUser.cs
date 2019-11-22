using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    [DataContract]
    public class InstallStationUser
    {
        string _id;
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

        string _userid;
        ///<summary>
        ///用户ID
        ///</summary>
        [DataMember]
        public string UserID
        {
            get
            {
                return _userid;
            }
            set
            {
                _userid = value;
            }
        }

        string _installstationid;
        ///<summary>
        ///安装点ID
        ///</summary>
        [DataMember]
        public string InstallStationID
        {
            get
            {
                return _installstationid;
            }
            set
            {
                _installstationid = value;
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


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserID)))
            {
                builder.AppendLine("ClientID:" + UserID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(InstallStationID)))
            {
                builder.AppendLine("Name:" + InstallStationID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("DistrictCode:" + CreateTime.ToString());
            }
            return builder.ToString();
        }

    }
}
