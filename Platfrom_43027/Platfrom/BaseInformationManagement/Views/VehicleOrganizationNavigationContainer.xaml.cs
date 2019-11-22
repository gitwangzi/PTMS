using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Gsafety.Ant.BaseInformation.Views
{
    [Export]
    public partial class VehicleOrganizationNavigationContainer : UserControl
    {
        public VehicleOrganizationNavigationContainer()
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
