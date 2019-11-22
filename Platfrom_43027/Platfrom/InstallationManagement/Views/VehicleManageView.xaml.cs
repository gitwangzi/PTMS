using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Installation;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;
using System.Windows.Media;
using Gsafety.PTMS.Share;
namespace Gsafety.PTMS.Installation.Views
{

    [ExportAsView(InstallationName.VehicleManageV, Category = BaseInformationName.CategoryName)]
    [ExportViewToRegion(InstallationName.VehicleManageV, ViewContainer.InstallContainer)]
    public partial class VehicleManageView : UserControl
    {
        public VehicleManageView()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Link1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var btn = sender as HyperlinkButton;
            if (btn != null)
            {
                SolidColorBrush solidColorBrush = new SolidColorBrush();
                solidColorBrush.Color = "#1c1f23".ToColor();
                btn.Background = solidColorBrush;
            }
        }

    }
}
