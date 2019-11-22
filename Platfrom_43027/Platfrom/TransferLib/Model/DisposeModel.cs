using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.TransferLib
{
    class DisposeModel
    {
        //public string alarmId { get; set; }
        //public string vehicleID { get; set; }
        //public string gpsTime { get; set; }
        //public string longitude { get; set; }
        //public string latitude { get; set; }

        //public string alarmSource { get; set; }
        //public string deviceID { get; set; }
        //public string contactPhone { get; set; }
        //public string contact { get; set; }
        //public string Direction { get; set; }

        //public string speed { get; set; }
        //public string gpsValid { get; set; }
        //public string vehicleSn { get; set; }
        //public string brandModel { get; set; }
        //public string vehicleType { get; set; }

        //public string operationLincense { get; set; }
        //public string district { get; set; }
        //public string districtName { get; set; }
        //public string note { get; set; }
        //public string clientID { get; set; }

        //public string uri { get; set; }

        public string incidentAppealId { get; set; }
        public int incidentType { get; set; }
        public int alarmType { get; set; }
        public string alarmPerson { get; set; }
        public string alarmPhone { get; set; }
        public string alarmAddress { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string alarmDescription { get; set; }
        public string appealPersonId { get; set; }
        public string appealPersonName { get; set; }
    }
}
