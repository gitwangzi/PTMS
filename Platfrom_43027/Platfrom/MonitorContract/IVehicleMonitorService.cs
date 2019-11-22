/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c2879230-e93b-439a-9ae5-6a412727ab79      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Monitor.Contract
/////    Project Description:    
/////             Class Name: IVehicleMonitorService
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/1 15:15:52
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/1 15:15:52
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Monitor.Contract.Data;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;

namespace Gsafety.PTMS.Monitor.Contract
{

    [ServiceContract]
    public interface IVehicleMonitorService
    {

        [OperationContract]
        MultiMessage<VehicleAlert> GetVehicleAlert(string vehicleId, DateTime startDate, DateTime endDate, string alertType);

        [OperationContract]
        SingleMessage<GPS> GetLastMonitorGPS(string vehicleId);

        [OperationContract]
        MultiMessage<GPS> GetMonitorGPSTrack(string vehicleId, DateTime startTime, DateTime endTime);

        [OperationContract]
        SingleMessage<bool> ValidateSuiteGPS(string vehicleId, string mdvrCoreSn);

        [OperationContract]
        SingleMessage<bool> ValidateGPSGPS(string vehicleId, string gpsSN);
    }
}
