using Gsafety.PTMS.BaseInformation;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gsafety.Ant.BaseInformation.Views
{
    [ExportAsView(BaseInformationName.DevGpsManageViewV)]
    [ExportViewToRegion(BaseInformationName.DevGpsManageViewV, BaseInformationName.BaseInfoContainer)]
	public partial class DevGpsManageView : UserControl
	{
		public DevGpsManageView()
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

