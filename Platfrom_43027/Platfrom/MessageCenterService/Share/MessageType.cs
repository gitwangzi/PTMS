using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCenterService.Share
{
    public enum MessageType
    {
        HeartBeat, //心跳
        Register,  //注册到消息中心
        RegisterReply, //注册回复
        UserLogin,     //用户登陆
        UserLogout,
        UnRegister,     //

        SuiteBeginInstall,
        SuiteEndInstall,
        SuiteCancelInstall,
        GPSBeginInstall,
        GPSEndInstall,
        GPSCancelInstall,
        Maintain,

        Alarm,
        BusinessAlert,
        DeviceAlert,
        GPS,
        AlarmComplete,
        AlertComplete,
        MonitorVehicle,
        CancelMonitorVehicle,
        
        HeartBeatRule,
        HeartBeatRuleReply,
        LocationStrategy,
        LocationStrategyReply,
        VideoSetting,
        VideoSettingReply,
        SoftwareUpgrade,
        SoftwareUpgradeReply,
        SpeedSetting,
        SpeedSettingReply,
        FenceSetting,
        FenceSettingReply,
        RouteSetting,
        RouteSettingReply,

        SuiteMessage,
        AppMessage,


        VehicleOnline,
        VehicleOffline,
        SuiteStatusChange,
        GPSStatusChange,
        MobileStatusChange,

        ClientStatusChange,
        ClientUserCountChange,
        ClientServiceTimeChange,
        OrganizationChange,
        RoleChange,
        FuncItemChange,
        DeleteUser,

        DownloadVideo,
        GetMDVRFileList,
        
    }
}
