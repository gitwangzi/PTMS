using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.User
{
    [ExportAsView(ManagerName.RegionAssignV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.RegionAssignV, ManagerName.ManagerContainer)]
    public partial class RegionAssignView : UserControl
    {
        public RegionAssignView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
        }
    }
}
