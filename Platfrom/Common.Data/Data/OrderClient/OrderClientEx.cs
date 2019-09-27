using System;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;


namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    [KnownType(typeof(OrderClient))]
    public class OrderClientEx : OrderClient
    {        
        int row;
        [DataMember]
        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        string _username;
        ///<summary>
        ///登陆名
        ///</summary>
        [DataMember]
        public string UserName
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }

        string statusStr;
        [DataMember]
        public string StatusStr
        {
            get { return statusStr; }
            set { statusStr = value; }
        }

        int _actualUserCount;
        ///<summary>
        ///实际用户数量
        ///</summary>
        [DataMember]
        public int ActualUserCount
        {
            get
            {
                return _actualUserCount;
            }
            set
            {
                _actualUserCount = value;
            }
        }

        int _actualDeviceCount;
        ///<summary>
        ///实际设备数量
        ///</summary>
        [DataMember]
        public int ActualDeviceCount
        {
            get
            {
                return _actualDeviceCount;
            }
            set
            {
                _actualDeviceCount = value;
            }
        }
    }
}
