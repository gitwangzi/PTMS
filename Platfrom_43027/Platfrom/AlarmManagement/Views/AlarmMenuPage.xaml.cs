/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c60d6751-932f-487c-9ed2-ae573cf06864      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Views
/////    Project Description:    
/////             Class Name: AlarmMenuPage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/19 9:27:42
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/19 9:27:42
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Jounce.Core.View;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.Alarm.ViewModels;
using Gsafety.Common.Utilities;
using Gsafety.Common.Controls;
namespace Gsafety.PTMS.Alarm.Views
{
    //[ExportAsView(AlarmName.AlarmMenuView)]
    public partial class AlarmMenuPage : UserControl
    {
        public AlarmMenuPage()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(handlegrid);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(unHandleGrid);
            handlegrid.DoubleClickHook(HandleGridDbClick_CallBack);
            unHandleGrid.DoubleClickHook(UnHandleGridDbClick_CallBack);
            Accordion.SelectedIndex = 0;
            comboStatus.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus.DropDownClosed += PopupHandler.OnDropDown;
        }

        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void HandleGridDbClick_CallBack(object source)
        {
            GridDbClickOper(handlegrid);
        }

        private void UnHandleGridDbClick_CallBack(object source)
        {
            GridDbClickOper(unHandleGrid);
        }
        private void GridDbClickOper(DataGrid dg)
        {
            if (dg.SelectedItem != null)
            {
                ((AlarmMenuPageVm)this.LayoutRoot.DataContext).OpenDetailViewClick_Event(null);
            }

        }

        private Button OldBtnMore = null;
        private void btnMore_Click(object sender, RoutedEventArgs e)
        {
            if (OldBtnMore != null)
            {
                ContextMenuService.GetContextMenu(OldBtnMore).IsOpen = false;
            }
            Button btnMore = (Button)sender;
            OldBtnMore = btnMore;
            ContextMenuService.GetContextMenu(btnMore).IsOpen = true;
        }

        private void aa_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseRightButtonDown += (s, a) =>
                {
                    (sender as DataGrid).SelectedIndex = (s as DataGridRow).GetIndex();
                    (s as DataGridRow).Focus();
                };


        }

        private void btnMore_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
