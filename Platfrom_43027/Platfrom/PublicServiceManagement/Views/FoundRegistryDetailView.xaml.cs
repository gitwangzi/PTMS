using Jounce.Core.View;
using Jounce.Regions.Core;
using PublicServiceManagement;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gsafety.PTMS.PublicServiceManagement.Views.Views
{
	[ExportAsView(PublicServiceName.FoundRegistryDetailV)]
    [ExportViewToRegion(PublicServiceName.FoundRegistryDetailV, "PublicServiceContainer")]
	public partial class FoundRegistryDetailView : UserControl
	{
		public FoundRegistryDetailView()
		{
			InitializeComponent();
            this.MouseRightButtonDown += FoundRegistryDetailView_MouseRightButtonDown;
		}

        void FoundRegistryDetailView_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
	}
}

