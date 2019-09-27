using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Ant.Installation.Contract.Data
{
        [DataContract]
        public class SecuritySuiteWorkingModel2
        {
            [DataMember]
            public string ID { get; set; }

            [DataMember]
            public string CompanyName { get; set; }

            [DataMember]
            public string CarNumber { get; set; }

            [DataMember]
            public string VehicleSN { get; set; }

            [DataMember]
            public string SuiteID { get; set; }

            //[DataMember]
            //public BusinessType BusinessType;

            //public decimal? BusinessType_Aide
            //{
            //    get { return Convert.ToDecimal(this.BusinessType); }
            //    set
            //    {
            //        if (value != null)
            //        {
            //            this.BusinessType = (BusinessType)value;
            //        }
            //    }
            //}

            [DataMember]
            public string MDVR_SN { get; set; }

            [DataMember]
            public string MDVR_Type { get; set; }

            [DataMember]
            public string MDVR_Permanentassets { get; set; }

            [DataMember]
            public Nullable<DateTime> MDVR_Delivery_Date { get; set; }

            [DataMember]
            public string MDVR_GPS_SN { get; set; }

            [DataMember]
            public string MDVR_CORE_SN { get; set; }

            [DataMember]
            public string MDVR_GPRS_SN { get; set; }

            [DataMember]
            public string MDVR_SIM { get; set; }

            [DataMember]
            public string MDVR_SIM_Mobile { get; set; }

            [DataMember]
            public string ANT_GPS { get; set; }

            [DataMember]
            public string ANTGPS_Type { get; set; }

            [DataMember]
            public string ANTGPS_Permanentassets { get; set; }

            [DataMember]
            public Nullable<DateTime> ANTGPS_Delivery_Date { get; set; }

            [DataMember]
            public string ANT_SIM { get; set; }

            [DataMember]
            public string ANT_SIM_MOBILE { get; set; }

            [DataMember]
            public Nullable<decimal> ANTGPS_SETUP { get; set; }

            [DataMember]
            public decimal InfraredCamera { get; set; }

            [DataMember]
            public string Camera_SN1 { get; set; }

            [DataMember]
            public string Camera_SN2 { get; set; }

            [DataMember]
            public decimal UPS { get; set; }

            [DataMember]
            public string UPS_SN { get; set; }

            [DataMember]
            public decimal SDCard { get; set; }

            [DataMember]
            public string SD_SN { get; set; }

            [DataMember]
            public decimal Alarm_Button { get; set; }

            [DataMember]
            public string Alarm_Button_SN { get; set; }

            [DataMember]
            public string Alarm_Button_SN2 { get; set; }

            [DataMember]
            public string Alarm_Button_SN3 { get; set; }

            [DataMember]
            public decimal Door_Switch_Sensor { get; set; }

            [DataMember]
            public string Door_Switch_Sensor_SN { get; set; }

            [DataMember]
            public string Setup_Station_ID { get; set; }

            [DataMember]
            public string Setup_Station_Name { get; set; }

            [DataMember]
            public string User_ID_Record { get; set; }

            [DataMember]
            public string Setup_Staff { get; set; }

            [DataMember]
            public string Setup_Images { get; set; }

            [DataMember]
            public Nullable<DateTime> Setup_Date { get; set; }

            [DataMember]
            public string Note { get; set; }

            [DataMember]
            public decimal Valid { get; set; }

            [DataMember]
            public string Software_Version { get; set; }

            [DataMember]
            public SecuritySuiteStatus Status { get; set; }

            [DataMember]
            public decimal? Status_Aide
            {
                get { return Convert.ToDecimal(this.Status); }
                set
                {
                    if (value != null)
                    {
                        this.Status = (SecuritySuiteStatus)value;
                    }
                }
            }

            [DataMember]
            public string Protocol_Version { get; set; }

            [DataMember]
            public string Alert_Type { get; set; }

            [DataMember]
            public string ENGINE_SN { get; set; }

            [DataMember]
            public string UNDERPAN_SN { get; set; }

            [DataMember]
            public decimal? CheckStep { get; set; }
        }
}
