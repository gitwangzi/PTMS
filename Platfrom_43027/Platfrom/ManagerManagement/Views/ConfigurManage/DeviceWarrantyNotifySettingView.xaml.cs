using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.ConfigurManage
{
    /// <summary>
    /// 质保期到期时间通知视图
    /// </summary>
    [ExportAsView(ManagerName.AntProductDeviceWarrantyNotifySettingV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.AntProductDeviceWarrantyNotifySettingV, ManagerName.ManagerContainer)]
    public partial class DeviceWarrantyNotifySettingView : UserControl
    {
        public DeviceWarrantyNotifySettingView()
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
