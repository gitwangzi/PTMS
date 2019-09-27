using Gsafety.PTMS.ServiceReference.MaitenanceRecordService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: f5220830-53bd-41aa-ab2d-32982f3868a8      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels
/////    Project Description:    
/////             Class Name: ServiceLifeDetailVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/21 12:49:31
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/21 12:49:31
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Maintain.ViewModels
{

    [ExportAsViewModel(MaintainName.SuiteLifeDetailVm)]
    public class SuiteLifeDetailVm : BaseEntityViewModel
    {
        public SuiteLife CurrentItem { get; set; }

        public ICommand ReturnCommand { get; private set; }

        public ObservableCollection<SuiteLifeDetail> SuiteLifeDetailModels { get; set; }

        private MaintenanceRecordServiceClient client;

        public SuiteLifeDetailVm()
        {
            ReturnCommand = new ActionCommand<object>(obj => Return());

            client = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();
            client.GetSuiteLifeDetailCompleted += client_GetSuiteLifeDetailCompleted;
        }

        void client_GetSuiteLifeDetailCompleted(object sender, GetSuiteLifeDetailCompletedEventArgs e)
        {
            SuiteLifeDetailModels = e.Result.Result;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged("SuiteLifeDetailModels"));
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                CurrentItem = (SuiteLife)viewParameters["item"];
                client.GetSuiteLifeDetailAsync(CurrentItem.SuiteId);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged("CurrentItem"));
            }
            catch (Exception ex)
            {
            }
        }

        private void Return()
        {
            try
            {
                EventAggregator.Publish(new ViewNavigationArgs(MaintainName.SuiteLifeV, new Dictionary<string, object>()));
            }
            catch (Exception ex)
            {
            }
        }
    }
}
