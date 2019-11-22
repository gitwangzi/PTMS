using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract
{
    [ServiceContract]
    public interface IMessageGPS
    {
        [OperationContract]
        void MonitorVehicle(string usertoken, List<string> vechiles);

        [OperationContract]
        void UnMonitorVehicle(string usertoken, List<string> vehicles);
    }
}
