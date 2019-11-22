using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    /// <summary>
    /// InstallStation
    /// </summary>
    [DataContract]
    public class InstallStation
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

        string _clientid;
        ///<summary>
        ///客户账号
        ///</summary>
        [DataMember]
        public string ClientID
        {
            get
            {
                return _clientid;
            }
            set
            {
                _clientid = value;
            }
        }

        string _name;
        ///<summary>
        ///名称
        ///</summary>
        [DataMember]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        string _districtcode;
        ///<summary>
        ///行政区域
        ///</summary>
        [DataMember]
        public string DistrictCode
        {
            get
            {
                return _districtcode;
            }
            set
            {
                _districtcode = value;
            }
        }

        string _provinceName;
        ///<summary>
        ///行政区域
        ///</summary>
        [DataMember]
        public string ProvinceName
        {
            get
            {
                return _provinceName;
            }
            set
            {
                _provinceName = value;
            }
        }

        string _cityName;
        ///<summary>
        ///行政区域
        ///</summary>
        [DataMember]
        public string CityName
        {
            get
            {
                return _cityName;
            }
            set
            {
                _cityName = value;
            }
        }

        string _address;
        ///<summary>
        ///地址
        ///</summary>
        [DataMember]
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }

        string _director;
        ///<summary>
        ///负责人
        ///</summary>
        [DataMember]
        public string Director
        {
            get
            {
                return _director;
            }
            set
            {
                _director = value;
            }
        }

        string _directorphone;
        ///<summary>
        ///负责人电话
        ///</summary>
        [DataMember]
        public string DirectorPhone
        {
            get
            {
                return _directorphone;
            }
            set
            {
                _directorphone = value;
            }
        }

        string _contact;
        ///<summary>
        ///联系人
        ///</summary>
        [DataMember]
        public string Contact
        {
            get
            {
                return _contact;
            }
            set
            {
                _contact = value;
            }
        }

        string _contactphone;
        ///<summary>
        ///联系人电话
        ///</summary>
        [DataMember]
        public string ContactPhone
        {
            get
            {
                return _contactphone;
            }
            set
            {
                _contactphone = value;
            }
        }

        string _email;
        ///<summary>
        ///邮箱
        ///</summary>
        [DataMember]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        string _note;
        ///<summary>
        ///备注
        ///</summary>
        [DataMember]
        public string Note
        {
            get
            {
                return _note;
            }
            set
            {
                _note = value;
            }
        }

        DateTime _createtime;
        ///<summary>
        ///创建时间
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

        decimal _valid;
        ///<summary>
        ///是否有效

        ///</summary>
        [DataMember]
        public decimal Valid
        {
            get
            {
                return _valid;
            }
            set
            {
                _valid = value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ClientID)))
            {
                builder.AppendLine("ClientID:" + ClientID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Name)))
            {
                builder.AppendLine("Name:" + Name.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DistrictCode)))
            {
                builder.AppendLine("DistrictCode:" + DistrictCode.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Address)))
            {
                builder.AppendLine("Address:" + Address.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Director)))
            {
                builder.AppendLine("Director:" + Director.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DirectorPhone)))
            {
                builder.AppendLine("DirectorPhone:" + DirectorPhone.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Contact)))
            {
                builder.AppendLine("Contact:" + Contact.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ContactPhone)))
            {
                builder.AppendLine("ContactPhone:" + ContactPhone.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Email)))
            {
                builder.AppendLine("Email:" + Email.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Note.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Valid)))
            {
                builder.AppendLine("Valid:" + Valid.ToString());
            }

            return builder.ToString();
        }


    }
}
