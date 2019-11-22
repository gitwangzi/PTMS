
namespace Gsafety.PTMS.Common.Enum
{
    /// <summary>
    /// 系统日志类别枚举
    /// 创建人刘昌在
    /// </summary>
    public enum SystemLogTypeEnum
    {
        /// <summary>
        /// 报警处理日志
        /// </summary>
        AlarmDisposedLog = 0,
        /// <summary>
        /// 车辆告警处理日志
        /// </summary>
        VehicleAlarmDisposedLog = 1,
        /// <summary>
        /// 设备告警处理日志
        /// </summary>
        DeviceAlarmDisposed = 2,
        /// <summary>
        /// 安装日志
        /// </summary>
        InstallLog = 3,
        /// <summary>
        /// 系统登录日志
        /// </summary>
        SystemLoginLog = 4,
        /// <summary>
        /// 视频下载日志
        /// </summary>
        VedioDownloadLog = 5,
        /// <summary>
        /// 本地视频下载日志
        /// </summary>
        LocalVedioDownloadLog = 6,
        /// <summary>
        /// 视频播放日志
        /// </summary>
        VedioPlayLog = 7,
        /// <summary>
        /// 用户管理日志
        /// </summary>
        UserManageLog = 8,
        /// <summary>
        /// 角色管理日志
        /// </summary>
        RoleManageLog = 9,
        /// <summary>
        /// 协助911用户日志
        /// </summary>
        Assist911UserLog = 10,
    }
}
