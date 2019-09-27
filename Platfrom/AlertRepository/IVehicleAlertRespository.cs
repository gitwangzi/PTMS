using Gsafety.PTMS.Alert.Contract;
using Gsafety.PTMS.Alert.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: bc10b791-2133-4d7c-adff-b5807d3ec656      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Repository
/////    Project Description:    
/////             Class Name: IVehicleAlertRespository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/9 10:59:02
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/9 10:59:02
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Alert.Repository
{
    public interface IVehicleAlertRespository
    {
        /// <summary>
        /// 获取未处理的车辆告警数据
        /// </summary>
        /// <param name="carNumber"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="handleStatus"></param>
        /// <param name="alertType"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        MultiMessage<VehicleAlert> GetVehicleAlert(string carNumber, DateTime? startTime, DateTime? endTime, int alertType, PagingInfo pagingInfo);

        /// <summary>
        /// 获取已处理的车辆告警数据
        /// </summary>
        /// <param name="carNumber"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="handleStatus"></param>
        /// <param name="alertType"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        MultiMessage<VehicleAlertEx> GetVehicleAlertEx(string carNumber, DateTime? startTime, DateTime? endTime, int alertType, PagingInfo pagingInfo);

    }
}
