using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.Common.Enum;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b5875ce6-b8e0-47b0-9556-fefc4c223c36      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////    Project Description:    
/////             Class Name: Vehicle
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/7 16:09:26
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/22 16:09:26
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Runtime.Serialization;
using System.Text;

namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///车辆
    ///</summary>
    [DataContract]
    [Serializable]
    public class Vehicle
    {

        #region 非数据库字段
        /// <summary>
        /// Province Code
        /// </summary>
        [DataMember]
        public string ProvinceCode { get; set; }
        /// <summary>
        /// Province Name
        /// </summary>
        [DataMember]
        public string ProvinceName { get; set; }
        /// <summary>
        /// City Code
        /// </summary>
        [DataMember]
        public string CityCode { get; set; }
        /// <summary>
        /// City Name
        /// </summary>
        [DataMember]
        public string CityName { get; set; }
        /// <summary>
        /// DeleteFlag
        /// </summary>
        [DataMember]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// DeleteFlag
        /// </summary>
        [DataMember]
        public bool UpdateFlag { get; set; }

        /// <summary>
        /// MDVR_SN
        /// </summary>
        [DataMember]
        public string MDVR_SN { get; set; }

        /// <summary>
        /// MDVR_SN
        /// </summary>
        [DataMember]
        public string GPS_SN { get; set; }

        /// <summary>
        /// MDVR_SN
        /// </summary>
        [DataMember]
        public string Mobile_SN { get; set; }
        ///// <summary>
        ///// IsOnLine
        ///// </summary>
        //[DataMember]
        //public short? IsOnLine { get; set; }
        /// <summary>
        /// Install Status
        /// </summary>
        InstallStatusType installStatus = InstallStatusType.UnInstall;
        [DataMember]
        public InstallStatusType InstallStatus
        {
            get { return installStatus; }
            set { installStatus = value; }
        }
        /// <summary>
        /// Check Step
        /// </summary>
        public short? CheckStep { get; set; }
        #endregion

        private int? _mdvrOnline;
        /// <summary>
        /// Null 未安装MDVR
        /// True 在线
        /// False不在线
        /// </summary>
        [DataMember]
        public int? MDVROnline
        {
            get { return _mdvrOnline; }
            set { _mdvrOnline = value; }
        }

        private int? _gpsOnline;
        [DataMember]
        public int? GPSOnline
        {
            get { return _gpsOnline; }
            set { _gpsOnline = value; }
        }

        private int? _mobileOnline;
        [DataMember]
        public int? MobileOnline
        {
            get { return _mobileOnline; }
            set { _mobileOnline = value; }
        }

        string _vehicleid;
        ///<summary>
        ///车牌号
        ///</summary>
        [DataMember]
        public string VehicleId
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

        string _clientid;
        ///<summary>
        ///客户账号
        ///</summary>
        [DataMember]
        public string ClientId
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

        string _orgnizationid;
        ///<summary>
        ///组织机构
        ///</summary>
        [DataMember]
        public string OrgnizationId
        {
            get
            {
                return _orgnizationid;
            }
            set
            {
                _orgnizationid = value;
            }
        }

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

        string _brandmodel;
        ///<summary>
        ///车辆品牌与类型
        ///</summary>
        [DataMember]
        public string BrandModel
        {
            get
            {
                return _brandmodel;
            }
            set
            {
                _brandmodel = value;
            }
        }

        string _districtcode;
        ///<summary>
        ///车辆所属行政区域
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

        VehicleConditionType _vehiclestatus;
        ///<summary>
        ///车况（0：不具备；1具备；缺省为1）
        ///</summary>
        [DataMember]
        public VehicleConditionType VehicleStatus
        {
            get
            {
                return _vehiclestatus;
            }
            set
            {
                _vehiclestatus = value;
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

        string _contactaddress;
        ///<summary>
        ///车主地址
        ///</summary>
        [DataMember]
        public string ContactAddress
        {
            get
            {
                return _contactaddress;
            }
            set
            {
                _contactaddress = value;
            }
        }

        string _contactemail;
        ///<summary>
        ///车主邮箱
        ///</summary>
        [DataMember]
        public string ContactEmail
        {
            get
            {
                return _contactemail;
            }
            set
            {
                _contactemail = value;
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

        string _region;
        ///<summary>
        ///运行区域
        ///</summary>
        [DataMember]
        public string Region
        {
            get
            {
                return _region;
            }
            set
            {
                _region = value;
            }
        }

        string _startyear;
        ///<summary>
        ///年限
        ///</summary>
        [DataMember]
        public string StartYear
        {
            get
            {
                return _startyear;
            }
            set
            {
                _startyear = value;
            }
        }

        VehicleSeviceType _servicetype;
        ///<summary>
        ///服务类型（1：商用；2：公共；3：私有；99：未知）
        ///</summary>
        [DataMember]
        public VehicleSeviceType ServiceType
        {
            get
            {
                return _servicetype;
            }
            set
            {
                _servicetype = value;
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

        VehicleType _vehicletype;
        ///<summary>
        ///车辆类型
        ///出租车；
        ///公交车；
        ///长途大巴；
        ///</summary>
        [DataMember]
        public VehicleType VehicleType
        {
            get
            {
                return _vehicletype;
            }
            set
            {
                _vehicletype = value;
            }
        }


        private string vehicleTypeDescribe;
        [DataMember]
        public string VehicleTypeDescribe
        {
            get { return this.vehicleTypeDescribe; }
            set
            {
                this.vehicleTypeDescribe = value;
            }
        }

        private string vehicleTypeImage;
        [DataMember]
        public string VehicleTypeImage
        {
            get { return this.vehicleTypeImage; }
            set
            {
                this.vehicleTypeImage = value;
            }
        }


        decimal _valid;
        ///<summary>
        ///是否有效
        ///有效
        ///无效
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

        /// <summary>
        /// 是否绑定
        /// </summary>
        [DataMember]
        public bool IsBinding { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ClientId)))
            {
                builder.AppendLine("ClientId:" + ClientId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OrgnizationId)))
            {
                builder.AppendLine("OrgnizationId:" + OrgnizationId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleSn)))
            {
                builder.AppendLine("VehicleSn:" + VehicleSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(EngineId)))
            {
                builder.AppendLine("EngineId:" + EngineId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(BrandModel)))
            {
                builder.AppendLine("BrandModel:" + BrandModel.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DistrictCode)))
            {
                builder.AppendLine("DistrictCode:" + DistrictCode.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OperationLicense)))
            {
                builder.AppendLine("OperationLicense:" + OperationLicense.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleStatus)))
            {
                builder.AppendLine("VehicleStatus:" + VehicleStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Owner)))
            {
                builder.AppendLine("Owner:" + Owner.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Contact)))
            {
                builder.AppendLine("Contact:" + Contact.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ContactAddress)))
            {
                builder.AppendLine("ContactAddress:" + ContactAddress.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ContactEmail)))
            {
                builder.AppendLine("ContactEmail:" + ContactEmail.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ContactPhone)))
            {
                builder.AppendLine("ContactPhone:" + ContactPhone.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Region)))
            {
                builder.AppendLine("Region:" + Region.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StartYear)))
            {
                builder.AppendLine("StartYear:" + StartYear.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ServiceType)))
            {
                builder.AppendLine("ServiceType:" + ServiceType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Note.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Creator)))
            {
                builder.AppendLine("Creator:" + Creator.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleType)))
            {
                builder.AppendLine("VehicleType:" + VehicleType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Valid)))
            {
                builder.AppendLine("Valid:" + Valid.ToString());
            }

            return builder.ToString();
        }

    }
}
