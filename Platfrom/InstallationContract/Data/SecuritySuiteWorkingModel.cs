using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Ant.Installation.Contract.Data
{
        [DataContract]
        public class SecuritySuiteWorkingModel
        {
            [DataMember]
            public string SuiteInfoID { get; set; }

            [DataMember]
            public string CompanyName { get; set; }

            [DataMember]
            public string CarNumber { get; set; }

            [DataMember]
            public string VehicleSN { get; set; }

            [DataMember]
            public string SuiteID { get; set; }

            [DataMember]
            public string MDVRSN { get; set; }

            [DataMember]
            public string MDVRCoreSN { get; set; }

            [DataMember]
            public string FactoryCode { get; set; }

            [DataMember]
            public string BatchNumber { get; set; }

            [DataMember]
            public string DeviceType { get; set; }

            [DataMember]
            public string MDVRSIM { get; set; }

            [DataMember]
            public string MDVRSIMMobile { get; set; }

            [DataMember]
            public string ANTGPSSN { get; set; }

            [DataMember]
            public string ANTSIM { get; set; }

            [DataMember]
            public string ANTSIMMobile { get; set; }

            [DataMember]
            public string ANTSN { get; set; }

            [DataMember]
            public string ANTCameraSN1 { get; set; }

            [DataMember]
            public string ANTCameraSN2 { get; set; }

            [DataMember]
            public string ANTSD { get; set; }

            [DataMember]
            public string ANTUPSSN { get; set; }

            [DataMember]
            public string ANTInfraredsensor { get; set; }

            [DataMember]
            public string CameraSN1 { get; set; }

            [DataMember]
            public string CameraSN2 { get; set; }

            [DataMember]
            public string UPSSN { get; set; }

            [DataMember]
            public string SDSN { get; set; }

            [DataMember]
            public string AlarmButtonSN { get; set; }

            [DataMember]
            public string AlarmButtonSN2 { get; set; }

            [DataMember]
            public string AlarmButtonSN3 { get; set; }

            [DataMember]
            public string DoorSwitchSensorSN { get; set; }

            [DataMember]
            public Nullable<DateTime> SetupDate { get; set; }

            [DataMember]
            public decimal Valid { get; set; }

            [DataMember]
            public string SoftwareVersion { get; set; }

            [DataMember]
            public string Singlechip { get; set; }

            [DataMember]
            public SecuritySuiteStatus Status { get; set; }

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
        }

}
