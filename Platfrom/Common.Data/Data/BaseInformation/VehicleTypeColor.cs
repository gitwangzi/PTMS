using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///
    ///</summary>
    [DataContract]
    [Serializable]
    public class VehicleTypeColor
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

        string _typeid;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string TypeID
        {
            get
            {
                return _typeid;
            }
            set
            {
                _typeid = value;
            }
        }

        string _typename;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string TypeName
        {
            get
            {
                return _typename;
            }
            set
            {
                _typename = value;
            }
        }


        int _minspeed;

        [DataMember]
        public int MinSpeed
        {
            get
            {
                return _minspeed;
            }
            set
            {
                _minspeed = value;
            }
        }


        int _maxspeed;

        [DataMember]
        public int MaxSpeed
        {
            get
            {
                return _maxspeed;
            }
            set
            {
                _maxspeed = value;
            }
        }

        string _color;
        [DataMember]
        public string Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

    }
}

