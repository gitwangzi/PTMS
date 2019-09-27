using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0b713fbc-a5a9-4a6b-bca8-d5220fb90da8      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Views
/////    Project Description:    
/////             Class Name: VehicleAdd
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/17 11:16:18
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/17 11:16:18
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.Windows.Controls;
namespace Gsafety.PTMS.BaseInformation.Views
{
    /// <summary>
    /// lcz 车辆管理功能
    /// 实现组织机构和车辆联动
    /// </summary>
    [ExportAsView(BaseInformationName.VehicleManageV)]
    [ExportViewToRegion(BaseInformationName.VehicleManageV, BaseInformationName.BaseInfoContainer)]
    public partial class VehicleManage : UserControl
    {
        public VehicleManage()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        private void monitorTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

            //contextMenu.IsOpen = false;
            //contextMenu.IsOpen = true;
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

    }
}
