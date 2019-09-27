using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.BaseInformation.Contract.Models
{
    ///<summary>
    ///
    ///</summary>
    [DataContract]
    public class InstallationStaff
    {
        string _id;
        ///<summary>
        ///
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

        string _icardid;
        ///<summary>
        ///身份证
        ///</summary>
        [DataMember]
        public string IcardID
        {
            get
            {
                return _icardid;
            }
            set
            {
                _icardid = value;
            }
        }

        decimal _grade;
        ///<summary>
        ///级别
        ///</summary>
        [DataMember]
        public decimal Grade
        {
            get
            {
                return _grade;
            }
            set
            {
                _grade = value;
            }
        }

        decimal _stafftype;
        ///<summary>
        ///类型
        ///</summary>
        [DataMember]
        public decimal StaffType
        {
            get
            {
                return _stafftype;
            }
            set
            {
                _stafftype = value;
            }
        }

        string _phone;
        ///<summary>
        ///电话
        ///</summary>
        [DataMember]
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
            }
        }

        string _email;
        ///<summary>
        ///邮件
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

        string _stationid;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string StationID
        {
            get
            {
                return _stationid;
            }
            set
            {
                _stationid = value;
            }
        }

        string _note;
        ///<summary>
        ///
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

        bool _valid;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public bool Valid
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
            if (!string.IsNullOrEmpty(Convert.ToString(Name)))
            {
                builder.AppendLine("Name:" + Name.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IcardID)))
            {
                builder.AppendLine("IcardID:" + IcardID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Grade)))
            {
                builder.AppendLine("Grade:" + Grade.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StaffType)))
            {
                builder.AppendLine("StaffType:" + StaffType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Phone)))
            {
                builder.AppendLine("Phone:" + Phone.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Email)))
            {
                builder.AppendLine("Email:" + Email.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Address)))
            {
                builder.AppendLine("Address:" + Address.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StationID)))
            {
                builder.AppendLine("StationID:" + StationID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Note.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Valid)))
            {
                builder.AppendLine("Valid:" + Valid.ToString());
            }

            return builder.ToString();
        }

    }
}

