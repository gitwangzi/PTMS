/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ebe8e57b-7f1d-449c-b241-a8ae8d3d0a45      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: AlarmInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 11:07:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/24 11:07:41
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.Common.Util;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// V61
    /// </summary>
    [DataContract]
    [Serializable]
    public class AlarmInfo
    {

        public AlarmInfo() { }

        public AlarmInfo(string str)
        {
            ////99dc[Cmd Length],[MDVR Card ID],[Message ID],Cmd Key,[Location And State]
            ////Happened Location And State ,Alert UID,Send Count ID,Button ID,Button Byname#
            ////Cmd Length 99dc0194,MDVR Card ID 0108002821,Message ID,Cmd Key V61,Cmd Send Time ,130729 033627,Location A,Latitude 11422.822265, Longitude 2243.110839,Land Speed 0.00,Land Course 202.93,Part State And 0000000008080C00,Power State GPS,Module State 0000000000010000,Security Suite State 703.23,Vehicle Carriage Temperature 21.97,IO Alert State And Web State 0000E011.0008,Route Number 26,Drive Number 102,Next Station Flag 301,Stop Flag 1,People Num In Car 0,,,,,,,,,,,,,,,,,Alert UID111670853010,Send Count ID 1,Button ID 3,Button Byname EmergencyAlarm#
            if (!string.IsNullOrEmpty(str))
            {
                string[] alarmArray = str.Split(',');

                if (alarmArray[3].ToUpper().Equals("V61"))
                {
                    this.MdvrCoreId = alarmArray[1];   ////MdvrCore Id

                    ////--------------------------------------------------------------
                    ////TODO:Need Make-up Function,Comment From Here
                    //var time = ConvertHelper.ConvertStrToDate(alarmArray[4], "yyMMdd HHmmss");
                    //if (time != null)
                    //{
                    //    this.AlarmTime = time.Value;   ////AlarmTime"yymmdd hhmmss",The Time That Cmd Happened On Security Suite .
                    //    this.GpsTime = time.Value;     ////GpsTime
                    //}

                    //this.GpsValid = alarmArray[5];     ////GpsValid
                    //this.Longitude = alarmArray[6];    ////Longitude
                    //this.Latitude = alarmArray[7];     ////Latitude
                    //this.Speed = alarmArray[8];        ////Speed
                    //this.Direction = alarmArray[9];    ////Direction
                    ////--------------------------------------------------------------

                    ////--------------------------------------------------------------
                    //TODO:Make-up Message

                    if (!string.IsNullOrEmpty(alarmArray[20]))
                    {
                        var time = ConvertHelper.ConvertStrToDate(alarmArray[20], "yyMMdd HHmmss");
                        if (time != null)
                        {
                            this.AlarmTime = time.Value;   ////AlarmTime "yymmdd hhmmss",The Time That Cmd Happened On Security Suite .
                            this.GpsTime = time.Value;     ////GpsTime
                        }

                        this.GpsValid = alarmArray[21];     ////GpsValid
                        this.Longitude = alarmArray[22];    ////Longitude
                        this.Latitude = alarmArray[23];     ////Latitude
                        this.Speed = alarmArray[24];        ////Speed
                        this.Direction = alarmArray[25];    ////Direction
                    }
                    else
                    {
                        var time = ConvertHelper.ConvertStrToDate(alarmArray[4], "yyMMdd HHmmss");
                        if (time != null)
                        {
                            this.AlarmTime = time.Value;   ////AlarmTime "yymmdd hhmmss", The Time That Cmd Happened On Security Suite .
                            this.GpsTime = time.Value;     ////GpsTime
                        }

                        this.GpsValid = alarmArray[5];     ////GpsValid
                        this.Longitude = alarmArray[6];    ////Longitude
                        this.Latitude = alarmArray[7];     ////Latitude
                        this.Speed = alarmArray[8];        ////Speed
                        this.Direction = alarmArray[9];    ////Direction
                    }
                    //--------------------------------------------------------------

                    this.ButtonNum = int.Parse(alarmArray[38]);   ////ButtonNum
                    this.Context = str;                ////Context

                    this.AlarmUid = alarmArray[36];   ////UID
                    this.Id = Guid.NewGuid().ToString();
                    this.ServerTime = DateTime.Now;
                }
            }
        }


        /// <summary>
        /// Alert Id(Primary Key)
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// VehicleId
        /// </summary>
        [DataMember]
        public string VehicleId { get; set; }

        /// <summary>
        /// AlarmTime
        /// </summary>
        [DataMember]
        public DateTime AlarmTime { get; set; }

        /// <summary>
        /// ProvinceName
        /// </summary>
        [DataMember]
        public string ProvinceName { get; set; }

        /// <summary>
        /// ProvinceName
        /// </summary>
        [DataMember]
        public string ProvinceCode { get; set; }

        /// <summary>
        /// CityName
        /// </summary>
        [DataMember]
        public string CityName { get; set; }

        /// <summary>
        /// DistrictCode
        /// </summary>
        [DataMember]
        public string DistrictCode { get; set; }

        /// <summary>
        /// SuiteInfoID
        /// </summary>
        [DataMember]
        public string SuiteInfoID { get; set; }

        ///// <summary>
        ///// CompanyID
        ///// </summary>
        //[DataMember]
        //public string CompanyID { get; set; }

        ///// <summary>
        ///// CompanyName
        ///// </summary>
        //[DataMember]
        //public string CompanyName { get; set; }

        /// <summary>
        /// SuiteStatus
        /// </summary>
        [DataMember]
        public int SuiteStatus { get; set; }

        /// <summary>
        /// VehicleType
        /// </summary>
        [DataMember]
        public int VehicleType { get; set; }

        /// <summary>
        /// SuiteID
        /// </summary>
        [DataMember]
        public string SuiteID { get; set; }

        /// <summary>
        /// MdvrCoreId
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        [DataMember]
        public string Longitude { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        [DataMember]
        public string Latitude { get; set; }

        /// <summary>
        /// Speed
        /// </summary>
        [DataMember]
        public string Speed { get; set; }

        /// <summary>
        /// Direction
        /// </summary>
        [DataMember]
        public string Direction { get; set; }

        /// <summary>
        /// GpsTime
        /// </summary>
        [DataMember]
        public Nullable<DateTime> GpsTime { get; set; }

        /// <summary>
        /// GpsValid
        /// </summary>
        [DataMember]
        public string GpsValid { get; set; }

        /// <summary>
        /// AlarmStatus
        /// </summary>
        [DataMember]
        public int AlarmStatus { get; set; }

        /// <summary>
        /// IsTrueAlarm 0: fales 1:true
        /// </summary>
        [DataMember]
        public short? IsTrueAlarm { get; set; }

        /// <summary>
        /// HandleTime
        /// </summary>
        [DataMember]
        public DateTime HandleTime { get; set; }

        /// <summary>
        /// HandleResult
        /// </summary>
        [DataMember]
        public int HandleResult { get; set; }

        /// <summary>
        /// UID
        /// </summary>
        [DataMember]
        public string AlarmUid { get; set; }

        /// <summary>
        /// ButtonNum
        /// </summary>
        [DataMember]
        public int ButtonNum { get; set; }

        /// <summary>
        /// Context
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        /// <summary>
        /// AlarmAddress
        /// </summary>
        [DataMember]
        public string AlarmAddress { get; set; }

        /// <summary>
        /// AlarmAddressCode
        /// </summary>
        [DataMember]
        public string AlarmAddressCode { get; set; }

        /// <summary>
        /// VehicleSn
        /// </summary>
        [DataMember]
        public string VehicleSn { get; set; }

        /// <summary>
        /// BrandModel
        /// </summary>
        [DataMember]
        public string BrandModel { get; set; }

        /// <summary>
        /// Mobile
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }

        /// <summary>
        /// OperationLincese
        /// </summary>
        [DataMember]
        public string OperationLincese { get; set; }

        /// <summary>
        /// Note
        /// </summary>
        [DataMember]
        public string Note { get; set; }

        /// <summary>
        /// Owner
        /// </summary>
        [DataMember]
        public string Owner { get; set; }

        /// <summary>
        /// StartYear
        /// </summary>
        [DataMember]
        public string StartYear { get; set; }

        /// <summary>
        /// IncidentId
        /// </summary>
        [DataMember]
        public string IncidentId { get; set; }

        /// <summary>
        /// One-Click Alert Time,Using By Cancel
        /// </summary>
        public DateTime ServerTime { get; set; }

        //districtname
        [DataMember]
        public string DistrictName { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Id)))
            {
                builder.AppendLine("Id:" + Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmTime)))
            {
                builder.AppendLine("AlarmTime:" + AlarmTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ProvinceName)))
            {
                builder.AppendLine("ProvinceName:" + ProvinceName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ProvinceCode)))
            {
                builder.AppendLine("ProvinceCode:" + ProvinceCode.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CityName)))
            {
                builder.AppendLine("CityName:" + CityName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DistrictCode)))
            {
                builder.AppendLine("DistrictCode:" + DistrictCode.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteInfoID)))
            {
                builder.AppendLine("SuiteInfoID:" + SuiteInfoID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteStatus)))
            {
                builder.AppendLine("SuiteStatus:" + SuiteStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleType)))
            {
                builder.AppendLine("VehicleType:" + VehicleType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteID)))
            {
                builder.AppendLine("SuiteID:" + SuiteID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Longitude)))
            {
                builder.AppendLine("Longitude:" + Longitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Latitude)))
            {
                builder.AppendLine("Latitude:" + Latitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Speed)))
            {
                builder.AppendLine("Speed:" + Speed.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Direction)))
            {
                builder.AppendLine("Direction:" + Direction.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsTime)))
            {
                builder.AppendLine("GpsTime:" + GpsTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsValid)))
            {
                builder.AppendLine("GpsValid:" + GpsValid.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmStatus)))
            {
                builder.AppendLine("AlarmStatus:" + AlarmStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IsTrueAlarm)))
            {
                builder.AppendLine("IsTrueAlarm:" + IsTrueAlarm.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(HandleTime)))
            {
                builder.AppendLine("HandleTime:" + HandleTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(HandleResult)))
            {
                builder.AppendLine("HandleResult:" + HandleResult.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmUid)))
            {
                builder.AppendLine("AlarmUid:" + AlarmUid.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ButtonNum)))
            {
                builder.AppendLine("ButtonNum:" + ButtonNum.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Context)))
            {
                builder.AppendLine("Context:" + Context.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmAddress)))
            {
                builder.AppendLine("AlarmAddress:" + AlarmAddress.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmAddressCode)))
            {
                builder.AppendLine("AlarmAddressCode:" + AlarmAddressCode.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleSn)))
            {
                builder.AppendLine("VehicleSn:" + VehicleSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(BrandModel)))
            {
                builder.AppendLine("BrandModel:" + BrandModel.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Mobile)))
            {
                builder.AppendLine("Mobile:" + Mobile.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OperationLincese)))
            {
                builder.AppendLine("OperationLincese:" + OperationLincese.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Note.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Owner)))
            {
                builder.AppendLine("Owner:" + Owner.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StartYear)))
            {
                builder.AppendLine("StartYear:" + StartYear.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IncidentId)))
            {
                builder.AppendLine("IncidentId:" + IncidentId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ServerTime)))
            {
                builder.AppendLine("ServerTime:" + ServerTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DistrictName)))
            {
                builder.AppendLine("DistrictName:" + DistrictName.ToString());
            }
            return builder.ToString();
        }

    }
}
