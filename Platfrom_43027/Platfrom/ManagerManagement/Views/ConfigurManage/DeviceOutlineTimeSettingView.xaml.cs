using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.ConfigurManage
{
    /// <summary>
    /// 设备未上线时间设置视图
    /// </summary>
    [ExportAsView(ManagerName.AntProductDeviceOutlineTimeSettingV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.AntProductDeviceOutlineTimeSettingV, ManagerName.ManagerContainer)]
    public partial class DeviceOutlineTimeSettingView : UserControl
    {
        public DeviceOutlineTimeSettingView()
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
