using Gsafety.PTMS.ServiceReference.BscDevSuiteService;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Ant.BaseInformation.Model
{
    public class DevSuitExportModel
    {
        public string SuiteInfoID { get; set; }
        public string ClientID { get; set; }
        public string SuiteID { get; set; }
        public string MdvrSn { get; set; }
        public string MdvrCoreSn { get; set; }
        public string Model { get; set; }
        public string MdvrSim { get; set; }
        public string MdvrSimMobile { get; set; }
        public string UpsSn { get; set; }
        public string SdSn { get; set; }
        public string SoftwareVersion { get; set; }
        public ProtocolTypeEnum Protocol { get; set; }
        public string Note { get; set; }
        public short Status { get; set; }
        public InstallStatusType InstallStatus { get; set; }

        public string Camera1Sn { get; set; }
        public string Camera1Name { get; set; }
        public string Camera1Model { get; set; }
        public string Camera1ProduceTime { get; set; }

        public string Camera2Sn { get; set; }
        public string Camera2Name { get; set; }
        public string Camera2Model { get; set; }
        public string Camera2ProduceTime { get; set; }

        public string Camera3Sn { get; set; }
        public string Camera3Name { get; set; }
        public string Camera3Model { get; set; }
        public string Camera3ProduceTime { get; set; }

        public string Camera4Sn { get; set; }
        public string Camera4Name { get; set; }
        public string Camera4Model { get; set; }
        public string Camera4ProduceTime { get; set; }

        public string AlarmBtn1Sn { get; set; }
        public string AlarmBtn1Name { get; set; }
        public string AlarmBtn1Model { get; set; }
        public string AlarmBtn1ProduceTime { get; set; }

        public string AlarmBtn2Sn { get; set; }
        public string AlarmBtn2Name { get; set; }
        public string AlarmBtn2Model { get; set; }
        public string AlarmBtn2ProduceTime { get; set; }

        public string AlarmBtn3Sn { get; set; }
        public string AlarmBtn3Name { get; set; }
        public string AlarmBtn3Model { get; set; }
        public string AlarmBtn3ProduceTime { get; set; }

        public string LedSn { get; set; }
        public string LedName { get; set; }
        public string LedModel { get; set; }
        public string LedProduceTime { get; set; }
    }
}
