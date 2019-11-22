using System;
using System.Windows;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager
{
    public partial class SystemManageMainView : UserControl
    {
        public SystemManageMainView()
        {
            InitializeComponent();
        }
        private void SystemUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/User/SystemUserManageView", UriKind.RelativeOrAbsolute));
        }

        private void UserOnlineInfoButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/User/UserOnlineInfoView", UriKind.RelativeOrAbsolute));
        }

        private void RegionAssignButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/User/RegionAssignView", UriKind.RelativeOrAbsolute));
        }

        private void MobileUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/User/MobileUserView", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// GPS命令配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetGPSInfoRoleButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/ComandManage/GPSConfigureView", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// 超速规则参数配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeedRoleParameterSettingButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/ComandManage/SpeedRoleParameterSetting", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// IP参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IpParameterSetting_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/ComandManage/IpParameterSettingView", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// 心跳规则参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetHeartBeatRoleButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/ComandManage/SetHeartBeatRuleView", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// 设置LED屏参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetLedScreenRoleButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/ComandManage/SetLEDScreenRoleView", UriKind.RelativeOrAbsolute));
        }

        private void SetLedScreenMessageParameter_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/ComandManage/SetLedScreenMessageParameterView", UriKind.RelativeOrAbsolute));
        }

        private void SearchRoleConfigureationButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/ComandManage/SearchRoleConfigurationView", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// 报警处理日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlarmDisposedLog_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/SystemLog/AlamDisposedLogView", UriKind.RelativeOrAbsolute));
        }

        private void VehicleAlarmDisposeButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/SystemLog/VehicleAlarmDisposeLogView", UriKind.RelativeOrAbsolute));
        }

        private void DeviceAlarmDisposeButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/SystemLog/DeviceAlarmDisposeLogView", UriKind.RelativeOrAbsolute));
        }

        private void SystemLoginLogButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/SystemLog/SystemLoginLogView", UriKind.RelativeOrAbsolute));
        }

        private void InstallLogButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/SystemLog/InstallLogView", UriKind.RelativeOrAbsolute));
        }

        private void VideoDownloadButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/SystemLog/VedioDownLoadLogView", UriKind.RelativeOrAbsolute));
        }

        private void LocalVedioDownloadLogButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/SystemLog/LocalVedioDownloadLogView", UriKind.RelativeOrAbsolute));
        }

        private void VedioPlayLogButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/SystemLog/VedioPlayLogView", UriKind.RelativeOrAbsolute));
        }

        private void UserManageLogButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/SystemLog/OperateLogV", UriKind.RelativeOrAbsolute));
        }

        private void Assist911UserLogButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/SystemLog/Assist911UserLogView", UriKind.RelativeOrAbsolute));
        }

        private void NativeAccordion_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var nativeAccordion = sender as Accordion;
            if (nativeAccordion != null)
            {
                if (this.NativeFrame != null && (this.NativeAccordion.SelectedIndex == 1))
                {
                    this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/Organization/OrganizationManageView", UriKind.RelativeOrAbsolute));
                }
            }
        }

        /// <summary>
        /// 参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParametersSettingButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/ConfigurManage/ParameterSettingView", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// 套件未上线时间设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceOutlineTimeSetting_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/ConfigurManage/DeviceOutlineTimeSettingView", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// 质保期通知设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WarrantyNotifySetting_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/ConfigurManage/DeviceWarrantyNotifySettingView", UriKind.RelativeOrAbsolute));

        }

        /// <summary>
        /// 告警类型颜色设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlarmTypeColorSetting_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/ConfigurManage/AlarmTypeColorSettingView", UriKind.RelativeOrAbsolute));

        }

        /// <summary>
        /// 告警类型声音设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlarmTypeSoundSetting_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/ConfigurManage/AlarmTypeSoundSettingView", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// 车辆告警设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VehicleAlarmSetting_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/ConfigurManage/VehicleAlarmSettingView", UriKind.RelativeOrAbsolute));

        }

        /// <summary>
        /// 设备告警设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceAlarmSetting_OnClick(object sender, RoutedEventArgs e)
        {
            this.NativeFrame.Navigate(new Uri("/SystemManagement;component/Views/ConfigurManage/DeviceAlarmSettingView", UriKind.RelativeOrAbsolute));

        }
    }
}
