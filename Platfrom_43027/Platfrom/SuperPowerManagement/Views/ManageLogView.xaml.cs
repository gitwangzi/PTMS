using Gsafety.PTMS.SuperPowerManagement;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace SuperPowerManagement.Views
{
    /// <summary>
    /// 用户管理日志View
    /// </summary>
    [ExportAsView(SuperPowerName.ManageLogV)]
    [ExportViewToRegion(SuperPowerName.ManageLogV, "SuperContainer")]
    public partial class ManageLogView : UserControl
    {
        public ManageLogView()
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
