
using Gsafety.PTMS.SIDS.Contract.Data;
using Gsafety.PTMS.SIDS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Gsafety.PTMS.SIDS.Contract
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ISIDSManageService”。
    [ServiceContract]
    public interface ISIDSManageService
    {
        [OperationContract]
        IList<VehicleModel> GetAllOnLineVehicleMes(string strControlCenter);

        [OperationContract]
        IList<VehicleModel> GetAlarmVehicleMes(string strControlCenter);

        [OperationContract]
        IList<VehicleModel> GetOutLineVehicleMes(string strControlCenter);

        [OperationContract]
        Dictionary<string, int> GetAllVehicleMessage(string strControlCenter);

        [OperationContract]
        IList<AlarmModel> GetTop10Alarm(string strControlCenter);

        [OperationContract]
        Dictionary<string, int> StatisticsAlarmCountByMonth(string strControlCenter, string StartTime, string EndTime);

        [OperationContract]
        Dictionary<string, int> StatisticsOutLineVehicleCount(string strControlCenter, string StartTime, string EndTime);

        [OperationContract]
        IList<VehicleModel> GetAlarmVehicleLocation(string strControlCenter, string AlarmMark, string AlarmTime, string HandleTime);

        [OperationContract]
        PerVehicleModel GetPerVehicleMes(string strControlCenter, string VehicleSn);

        [OperationContract]
        MDVR_Model GetMDVRMessage(string strContralCenter, string VehicleSn);

        [OperationContract]
        Dictionary<string, decimal> GetOnLineTime(string strContralCenter, string VehicleSn, string StartTime, string EndTime);

        //附加1
        [OperationContract]
        IList<Vehicle_MDVR_Model> GetVehicle_MDVR(string strContralCenter);

        //附加2
        [OperationContract]
        IList<Vehicle_Video_Model> GetVehicle_Video(string VehicleSn);
    }
}
