using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.Bases.Librarys;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace Gsafety.Ant.BaseInformation.Views.Organization
{
    [ExportAsView(BaseInformationName.AntProductVehicleDepartmentV, Category = BaseInformationName.CategoryName, MenuName = BaseInformationName.AntProductVehicleDepartmentV)]
    [ExportViewToRegion(BaseInformationName.AntProductVehicleDepartmentV, "BaseInfoContainer")]
    public partial class VehicleDepartmentView : UserControl
    {
        public VehicleDepartmentView()
        {
            InitializeComponent();
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = sender as TreeView;
            if (treeView != null)
            {
                var selectedItem = treeView.SelectedItem;
                var target = selectedItem as TreeNode<PTMS.ServiceReference.OrganizationService.Organization>;
                if (target != null)
                {
                    ApplicationContext.Instance.EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.VehicleDepartmentListV, new Dictionary<string, object>(){
                        {"VehicleDepartmentId",target.Model.ID},
                        {
                            "VehicleDepartmentName",target.Model.Name
                        }}));
                }
                else
                {
                    ApplicationContext.Instance.EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.EmptyView));
                }
            }
        }

        private void Link1_Click(object sender, RoutedEventArgs e)
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
