﻿using Gsafety.PTMS.Constants;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.MaintainRecordManageV)]
    [ExportViewToRegion(InstallationName.MaintainRecordManageV, ViewContainer.InstallContainer)]
	public partial class MaintainRecordManageView : UserControl
	{
		public MaintainRecordManageView()
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

