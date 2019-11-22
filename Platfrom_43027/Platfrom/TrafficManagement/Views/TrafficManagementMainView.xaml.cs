using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows;
using System.Windows.Controls;

namespace Gsafety.PTMS.Traffic.Views
{
    [ExportAsView(TrafficName.TrafficManagementMainV, Category = "Navigation", MenuName = "TrafficManagement", ToolTip = "Click to view some text.")]
    [ExportViewToRegion(TrafficName.TrafficManagementMainV, "CentralMainContainer2")]
    public partial class TrafficManagementMainView : UserControl
    {
        public TrafficManagementMainView()
        {
            InitializeComponent();
        }

        private void AddLineButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Line");
        }

        private void AddElectronicFence_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Electronic");
        }


        private void AddSpeedRoleButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Speed");
        }

        private void NativeAccordion_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var nativeItem = sender as Accordion;
            //if (nativeItem != null)
            //{
            //    if (this.NativeFrame != null && (nativeItem.SelectedIndex == 0))
            //    {
            //        this.NativeFrame.Navigate(new Uri("/TrafficManagement;component/Views/Map", UriKind.RelativeOrAbsolute));
            //    }
            //    if (this.NativeFrame != null && (nativeItem.SelectedIndex == 1))
            //    {
            //        this.NativeFrame.Navigate(new Uri("/TrafficManagement;component/Views/Map", UriKind.RelativeOrAbsolute));
            //    }
            //    if (this.NativeFrame != null && (nativeItem.SelectedIndex == 2))
            //    {
            //        this.NativeFrame.Navigate(new Uri("/TrafficManagement;component/Views/SpeedRoleMangeView", UriKind.RelativeOrAbsolute));
            //    }
            //}
        }

    }
}
