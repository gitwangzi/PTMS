using Gsafety.Common.Controls;
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

namespace Gsafety.PTMS.BasicPage.Views
{
    public partial class HistoryVideoManageContentView : UserControl
    {
        public RoutedEventHandler CheckBoxCheckHandle { get; set; }

        public HistoryVideoManageContentView()
        {
            InitializeComponent();
            
            try
            {
                Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(dg_List);
                tp_EndTime.DropDownOpened += PopupHandler.OnTimerDropDown;
                tp_EndTime.DropDownClosed += PopupHandler.OnTimerDropDown;
                tp_StartTime.DropDownOpened += PopupHandler.OnTimerDropDown;
                tp_StartTime.DropDownClosed += PopupHandler.OnTimerDropDown;
            }
            catch (System.Exception ex)
            {

            }
        }

        private void dp_StartTime_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dp_StartTime.SelectedDate.HasValue)
            {
                dp_EndTime.SelectedDate = dp_StartTime.SelectedDate.Value.Date.Add(dp_EndTime.SelectedDate.Value.TimeOfDay);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxCheckHandle != null) 
            {
                CheckBoxCheckHandle(sender, e);
            }
        }
    }
}
