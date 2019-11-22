using Gsafety.PTMS.ServiceReference.UpdateService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7584b009-2415-4027-9fa1-325de8943582      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels
/////    Project Description:    
/////             Class Name: UpgradeVersionEditVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/22 10:45:53
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/22 10:45:53
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
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
    [ExportAsViewModel(MaintainName.VersionMappingEditVm)]
    public class VersionMappingEditVm : BaseEntityViewModel
    {
        private UpdateServiceClient updateClient = ServiceClientFactory.Create<UpdateServiceClient>();
        public bool IsReadOnly { get; private set; }
        public string IsView { get; private set; }
        public string Title { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }
        public ICommand GetCommand { get; private set; }
        private SuiteVersionMap InitialSuiteVersionMap { get; set; }
        public SuiteVersionMap CurrentSuiteVersionMap { get; set; }

        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {

            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();

            switch (action)
            {
                case "view":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_LookSuiteVersion");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                    IsReadOnly = true;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    IsView = "Visible";
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsView));
                    InitialSuiteVersionMap = viewParameters["view"] as SuiteVersionMap;
                    break;
                case "update":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_UpdateSuiteVersion");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                    IsReadOnly = false;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    IsView = "Collapsed";
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsView));
                    InitialSuiteVersionMap = viewParameters["update"] as SuiteVersionMap;
                    break;
                default:
                    break;
            }
            Reset();
        }

        public VersionMappingEditVm()
        {
            try
            {
                updateClient.GetFileSizeCompleted += updateClient_GetFileSizeCompleted;
                updateClient.UpdateSuiteVersionCompleted += updateClient_UpdateSuiteVersionCompleted;
                ReturnCommand = new ActionCommand<object>(obj => Return());
                ResetCommand = new ActionCommand<object>(obj => Reset());
                GetCommand = new ActionCommand<object>(obj => GetSize());
            }
            catch
            {

            }
        }

        void updateClient_GetFileSizeCompleted(object sender, GetFileSizeCompletedEventArgs e)
        {
            if (e.Result.IsSuccess)
            {
                FileSize = e.Result.Result;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FileSize));
            }
        }

        private void GetSize()
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                updateClient.GetFileSizeAsync(FileName);
            }
        }

        void updateClient_UpdateSuiteVersionCompleted(object sender, UpdateSuiteVersionCompletedEventArgs e)
        {
            if (e.Result == null || !e.Result.IsSuccess)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"));
            }
            Return();
        }

        protected override void OnCommitted()
        {
            CurrentSuiteVersionMap.UnifyVersion = UnifyVersion;
            CurrentSuiteVersionMap.VendorVersion = VendorVersion;
            CurrentSuiteVersionMap.FileName=FileName;
            CurrentSuiteVersionMap.FileSize = FileSize;
            updateClient.UpdateSuiteVersionAsync(CurrentSuiteVersionMap);
        }


        private void Reset()
        {
            switch (action)
            {
                case "view":
                    CurrentSuiteVersionMap = MaintainCommon.Clone(InitialSuiteVersionMap);
                    UnifyVersion = CurrentSuiteVersionMap.UnifyVersion;
                    VendorVersion = CurrentSuiteVersionMap.VendorVersion;
                    FileName = CurrentSuiteVersionMap.FileName;
                    FileSize = CurrentSuiteVersionMap.FileSize;
                    break;
                case "update":
                    CurrentSuiteVersionMap = MaintainCommon.Clone(InitialSuiteVersionMap);
                    UnifyVersion = CurrentSuiteVersionMap.UnifyVersion;
                    VendorVersion = CurrentSuiteVersionMap.VendorVersion;
                    FileName = CurrentSuiteVersionMap.FileName;
                    FileSize = CurrentSuiteVersionMap.FileSize;
                    break;
                default:
                    break;
            }

            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentSuiteVersionMap));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UnifyVersion));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FileName));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FileSize));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VendorVersion));
        }

        private void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(MaintainName.VersionMappingV, new Dictionary<string, object>() { { "action", "return" } }));
        }

        private string unifyVersion;

        public string UnifyVersion
        {
            get { return unifyVersion; }
            set
            {
                unifyVersion = value == null ? null : value.Trim();
                Validate(ExtractPropertyName(() => UnifyVersion), value);
            }
        }

        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value == null ? null : value.Trim();
                Validate(ExtractPropertyName(() => FileName), value);
            }
        }

        private string vendorVersion;

        public string VendorVersion
        {
            get { return vendorVersion; }
            set
            {
                vendorVersion = value == null ? null : value.Trim();
                Validate(ExtractPropertyName(() => VendorVersion), value);
            }
        }

        private void Validate(string prop, object value)
        {
            ClearErrors(prop);
            if (value == null || value.ToString().Trim() == string.Empty)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MustFilled"));//格式非法
            }
        }

        private int fileSize;

        public int FileSize
        {
            get { return fileSize; }
            set
            {
                fileSize = value;
                ValidateSize(ExtractPropertyName(() => FileSize), value);
            }
        }

        private void ValidateSize(string prop, object value)
        {
            ClearErrors(prop);
            if (value == null || !(value is int) || Convert.ToInt32(value) < 0)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"));//格式非法
            }
        }
    }
}
