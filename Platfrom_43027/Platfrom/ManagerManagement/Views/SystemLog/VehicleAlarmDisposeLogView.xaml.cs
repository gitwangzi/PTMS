using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.SystemLog
{
    /// <summary>
    /// 车辆告警处理日志View
    /// </summary>
    [ExportAsView(ManagerName.AntProductVehicleAlarmDisposedLogV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.AntProductVehicleAlarmDisposedLogV, ManagerName.ManagerContainer)]
    public partial class VehicleAlarmDisposeLogView : UserControl
    {
        public VehicleAlarmDisposeLogView()
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
