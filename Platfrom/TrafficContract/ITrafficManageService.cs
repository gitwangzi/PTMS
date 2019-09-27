/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3593e839-8678-43e8-90f3-81187fb3c499      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Contract
/////    Project Description:    
/////             Class Name: Interface1
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/1 15:39:33
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/1 15:39:33
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Traffic.Contract.Data;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.Traffic.Contract
{
    [ServiceContract]
    public interface ITrafficManageService : ITrafficFence, ITrafficRoute
    {
        #region Fence

        [OperationContract]
        SingleMessage<Boolean> CheckFenceNameExist(string strFenceName, short nType);

        [OperationContract]
        SingleMessage<Boolean> DeleteCarFenceByFenceID(string fenceID);

        [OperationContract]
        MultiMessage<Vehicle> GetVehicleByFence(string fenceID, short nState);

        [OperationContract]
        SingleMessage<bool> CheckSpeedLimitidNameExist(string strSpeedName, string id);

        [OperationContract]
        SingleMessage<Boolean> CheckSpeedLimitNameExist(string strSpeedName);
        #endregion
    }
}
