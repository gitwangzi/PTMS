/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 387640c4-d69c-4863-8ae4-8d3b7c0bdd00      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views
/////    Project Description:    
/////             Class Name: ParamSetting
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/15 16:10:47
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/15 16:10:47
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
using Gsafety.PTMS.ServiceReference.AppConfigService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using System.Text.RegularExpressions;
namespace Gsafety.PTMS.Manager.Views
{
    public partial class ParamSetting : ChildWindow
    {
        AppConfigManagerClient appClient = ServiceClientFactory.Create<AppConfigManagerClient>();
        public string SecName = string.Empty;
        public ParamSetting(Gsafety.PTMS.ServiceReference.AppConfigService.AppConfig info)
        {

            InitializeComponent();

            Desc.Text = ApplicationContext.Instance.StringResourceReader.GetString(info.SECTION_DESC);

            DecValue.Text = info.SECTION_VALUE;
            DesUnit.Text = ApplicationContext.Instance.StringResourceReader.GetString(info.SECTION_UNIT);
            SecName = info.SECTION_NAME;
            appClient.UpdateConfigCompleted += appClient_UpdateConfigCompleted;

        }
        public Action<bool> ResultAction;
        void appClient_UpdateConfigCompleted(object sender, UpdateConfigCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //Fence_Edit_Faild
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Fence_Edit_Faild"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
            }
            if (!e.Result.Result)
            {

                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Fence_Edit_Faild"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
            }
            if (e.Result.Result)
            {
                ResultAction(true);
            }

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.DecValue.Text.Trim()))
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ValueNotNull"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                //ValueNotNull
                return;
            }
            Regex reg = new Regex("^[0-9]{0,8}$", RegexOptions.IgnoreCase);
            if (!reg.Match(this.DecValue.Text.Trim()).Success)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ValueBeNum"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                //   ValueBeNum
                return;
            }
            appClient.UpdateConfigAsync(SecName, int.Parse(DecValue.Text).ToString());
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

