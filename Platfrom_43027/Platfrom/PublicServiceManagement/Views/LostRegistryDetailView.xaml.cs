using Jounce.Core.View;
using Jounce.Regions.Core;
using PublicServiceManagement;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gsafety.PTMS.PublicServiceManagement.Views.Views
{
    [ExportAsView(PublicServiceName.LostRegistryDetailV)]
    [ExportViewToRegion(PublicServiceName.LostRegistryDetailV, "PublicServiceContainer")]
	public partial class LostRegistryDetailView : UserControl
	{
		public LostRegistryDetailView()
		{
			InitializeComponent();
		}
	}
}

