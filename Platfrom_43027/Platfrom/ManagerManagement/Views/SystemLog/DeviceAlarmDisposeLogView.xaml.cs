using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.SystemLog
{
    /// <summary>
    /// 设备告警处理日志View
    /// </summary>
    [ExportAsView(ManagerName.AntProductDeviceAlarmDisposedLogV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.AntProductDeviceAlarmDisposedLogV, ManagerName.ManagerContainer)]
    public partial class DeviceAlarmDisposeLogView : UserControl
    {
        public DeviceAlarmDisposeLogView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

    }
}
