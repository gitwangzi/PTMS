using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///视频设置
    ///</summary>
    [DataContract]
    public class VideoRule
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

        decimal _brightness;
        ///<summary>
        ///亮度
        ///</summary>
        [DataMember]
        public decimal Brightness
        {
            get
            {
                return _brightness;
            }
            set
            {
                _brightness = value;
            }
        }

        decimal _contrast;
        ///<summary>
        ///对比度
        ///</summary>
        [DataMember]
        public decimal Contrast
        {
            get
            {
                return _contrast;
            }
            set
            {
                _contrast = value;
            }
        }

        string _creator;
        ///<summary>
        ///创建人
        ///</summary>
        [DataMember]
        public string Creator
        {
            get
            {
                return _creator;
            }
            set
            {
                _creator = value;
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

        [DataMember]
        public string Name { get; set; }


        bool _isVisible;
        [DataMember]
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
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
            if (!string.IsNullOrEmpty(Convert.ToString(Brightness)))
            {
                builder.AppendLine("Brightness:" + Brightness.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Contrast)))
            {
                builder.AppendLine("Contrast:" + Contrast.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Creator)))
            {
                builder.AppendLine("Creator:" + Creator.ToString());
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

