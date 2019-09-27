using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Gsafety.PTMS.Leader.Contract.Data;



namespace Gsafety.PTMS.Leader.Contract
{
     [ServiceContract]
  public  interface ILeaderTerminalService
    {
        [OperationContract]
        List<GPSModel> getGPSList();

        [OperationContract]
        int getMdvrCount();
    }
}
