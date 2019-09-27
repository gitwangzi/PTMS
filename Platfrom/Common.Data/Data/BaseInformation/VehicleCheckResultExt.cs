using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    public class VehicleCheckResultExt : VehicleCheckResult
    {
        string _orgnizationname;
        ///<summary>
        ///组织机构
        ///</summary>
        [DataMember]
        public string OrgnizationName
        {
            get
            {
                return _orgnizationname;
            }
            set
            {
                _orgnizationname = value;
            }
        }

        string _vehiclesn;
        ///<summary>
        ///车架号
        ///</summary>
        [DataMember]
        public string VehicleSn
        {
            get
            {
                return _vehiclesn;
            }
            set
            {
                _vehiclesn = value;
            }
        }

        string _engineid;
        ///<summary>
        ///发动机号
        ///</summary>
        [DataMember]
        public string EngineId
        {
            get
            {
                return _engineid;
            }
            set
            {
                _engineid = value;
            }
        }

        string _operationlicense;
        ///<summary>
        ///运行许可证
        ///</summary>
        [DataMember]
        public string OperationLicense
        {
            get
            {
                return _operationlicense;
            }
            set
            {
                _operationlicense = value;
            }
        }

        string _businesstype;
        ///<summary>
        ///运行许可证
        ///</summary>
        [DataMember]
        public string BusinessType
        {
            get
            {
                return _businesstype;
            }
            set
            {
                _businesstype = value;
            }
        }

        string _contactphone;
        ///<summary>
        ///联系电话
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

        string _owner;
        ///<summary>
        ///车主
        ///</summary>
        [DataMember]
        public string Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
            }
        }

        string _organizationid;
        ///<summary>
        ///车主
        ///</summary>
        [DataMember]
        public string OrganizationID
        {
            get
            {
                return _organizationid;
            }
            set
            {
                _organizationid = value;
            }
        }
    }
}
