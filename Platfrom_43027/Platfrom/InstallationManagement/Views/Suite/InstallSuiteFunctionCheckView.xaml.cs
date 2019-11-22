/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 41ba7f93-6b26-4f34-9691-3384b0e43b04      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Views
/////    Project Description:    
/////             Class Name: CheckDeviceFunctionView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/17 14:53:56
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/17 14:53:56
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
using Jounce.Regions.Core;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Installation.ViewModels;
using Gs.PTMS.Common.Data.Enum;

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.InstallSuiteFunctionCheckV)]
    [ExportViewToRegion(InstallationName.InstallSuiteFunctionCheckV, ViewContainer.InstallContainer)]
    public partial class InstallSuiteFunctionCheckView : UserControl
    {
        private InstallSuiteFunctionCheckViewModel viewModel;
        private Dictionary<CameraInstallLocationEnum, CheckBox> CheckBoxList = new Dictionary<CameraInstallLocationEnum, CheckBox>();

        public InstallSuiteFunctionCheckView()
        {
            InitializeComponent();

            CheckBoxList[CameraInstallLocationEnum.OuterBehind] = OuterBehind;
            CheckBoxList[CameraInstallLocationEnum.OuterBefore] = OuterBefore;
            CheckBoxList[CameraInstallLocationEnum.OuterRight] = OuterRight;
            CheckBoxList[CameraInstallLocationEnum.OuterLeft] = OuterLeft;
            CheckBoxList[CameraInstallLocationEnum.InnerLeftDriver] = InnerLeftDriver;
            CheckBoxList[CameraInstallLocationEnum.InnerRightDriver] = InnerRightDriver;
            CheckBoxList[CameraInstallLocationEnum.InnerCenter] = InnerCenter;
            CheckBoxList[CameraInstallLocationEnum.InnerBehind] = InnerBehind;

            busCanvas.DataContextChanged += busCanvas_DataContextChanged;
        }

        void busCanvas_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            viewModel = e.NewValue as InstallSuiteFunctionCheckViewModel;
            if (viewModel == null)
            {
                return;
            }

            viewModel.PropertyChanged += viewModel_PropertyChanged;
        }

        void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "ChannelDictionary")
            {
                return;
            }

            SetCameraVisibility(viewModel.ChannelDictionary);
        }

        private void SetCameraVisibility(Dictionary<int, bool> dic)
        {
            if (dic == null)
            {
                return;
            }

            foreach (var item in CheckBoxList)
            {
                if (dic.ContainsKey((int)item.Key))
                {
                    item.Value.Visibility = Visibility.Visible;
                    item.Value.IsChecked = dic[(int)item.Key];
                }
                else
                {
                    item.Value.Visibility = Visibility.Collapsed;
                }
            }
        }

    }
}
