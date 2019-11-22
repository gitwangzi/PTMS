using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.ConfigurManage
{
    /// <summary>
    /// 设备告警设置视图
    /// </summary>
    [ExportAsView(ManagerName.AntProductDeviceAlarmSettingV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.AntProductDeviceAlarmSettingV, ManagerName.ManagerContainer)]
    public partial class DeviceAlarmSettingView : UserControl
    {
        public DeviceAlarmSettingView()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
