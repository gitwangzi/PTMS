using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Manager.Contract.Data.CommandManage;
using Gsafety.PTMS.Manager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Manager.Contract
{
    [ServiceContract]
    public interface ICommandManageService : IHeartbeatRule, IHeartbeatVehicle, ILocationReportRuleService, IVideoRule, ILocationReportVehicle, IVideoRuleVehicle, ISpeedLimit, IVehicleSpeed
    {

    }
}
