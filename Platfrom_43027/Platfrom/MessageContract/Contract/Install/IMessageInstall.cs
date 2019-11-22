using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract
{
    [ServiceContract]
    public interface IMessageInstall
    {
        [OperationContract]
        void BeginInstallSuite(string mdvrcoresn);

        [OperationContract]
        void CompleteInstallSuite(string mdvrcoresn, string organization,string vehicleid);

        [OperationContract]
        void RemoveInstallSuite(string mdvrcoresn);

        [OperationContract]
        void BeginInstallGPS(string mdvrcoresn);

        [OperationContract]
        void CompleteInstallGPS(string mdvrcoresn, string organization, string vehicleid);

        [OperationContract]
        void RemoveInstallGPS(string mdvrcoresn);

        [OperationContract]
        void SetAlarmParaCommand(SetAlarmPara commandInfo);
    }
}
