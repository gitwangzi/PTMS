using Gsafety.PTMS.ServiceReference.UpdateService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 212987c7-49ba-4e4c-92ce-64f76338576d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels
/////    Project Description:    
/////             Class Name: UpgradeVersionAddVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/24 11:39:53
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/24 11:39:53
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
using System.Linq;

namespace Gsafety.PTMS.Maintain.ViewModels
{
    [ExportAsViewModel(MaintainName.VersionMappingAddVm)]
    public class VersionMappingAddVm : BaseEntityViewModel
    {
        private UpdateServiceClient updateClient = ServiceClientFactory.Create<UpdateServiceClient>();
        public bool IsReadOnly { get; private set; }
        public string IsView { get; private set; }
        public string Title { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }
        public ICommand RMCommand { get; private set; }
        public ICommand WKPCommand { get; private set; }
        public ICommand SCCommand { get; private set; }
        private SuiteVersionMap Initial_RM_SuiteVersionMap { get; set; }
        private SuiteVersionMap Initial_WKP_SuiteVersionMap { get; set; }
        private SuiteVersionMap Initial_SC_SuiteVersionMap { get; set; }
        public SuiteVersionMap Current_RM_SuiteVersionMap { get; set; }
        public SuiteVersionMap Current_WKP_SuiteVersionMap { get; set; }
        public SuiteVersionMap Current_SC_SuiteVersionMap { get; set; }

        public string CurrentUnifiedVersion { get; set; }
        public string InitialUnifiedVersion { get; set; }

        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {

            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();
            if (action == "add")
            {
                Title = ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_AddSuiteSoftewareVersion");
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                IsReadOnly = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                IsView = "Collapsed";
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsView));
                Initial_RM_SuiteVersionMap = null;
                Initial_WKP_SuiteVersionMap = null;
                Initial_SC_SuiteVersionMap = null;
            }
            updateClient.GetNewSuiteVersionMapsAsync();
            
        }

        public VersionMappingAddVm()
        {
            try
            {
                updateClient.GetFileSizeCompleted += updateClient_GetFileSizeCompleted;
                updateClient.AddSuiteVersionMapsCompleted += updateClient_AddSuiteVersionMapsCompleted;
                updateClient.GetNewSuiteVersionMapsCompleted += updateClient_GetNewSuiteVersionMapsCompleted;
                ReturnCommand = new ActionCommand<object>(obj => Return());
                ResetCommand = new ActionCommand<object>(obj => Reset());
                RMCommand = new ActionCommand<object>(obj => GetFileSize("RM" ));
                WKPCommand = new ActionCommand<object>(obj => GetFileSize("WKP"));
                SCCommand = new ActionCommand<object>(obj => GetFileSize("SC"));

            }
            catch
            {

            }
        }

        void updateClient_GetFileSizeCompleted(object sender, GetFileSizeCompletedEventArgs e)
        {
            if (e.Result.IsSuccess)
            {
                switch (currentVendorName)
                {
                    case "RM":
                        Current_RM_FileSize = e.Result.Result;
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Current_RM_FileSize));
                        break;
                    case "WKP":
                        Current_WKP_FileSize = e.Result.Result;
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Current_WKP_FileSize));
                        break;
                    case "SC":
                        Current_SC_FileSize = e.Result.Result;
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Current_SC_FileSize));
                        break;
                    default:
                        break;
                }
            }
        }
        private string currentVendorName;
        private void GetFileSize(string name)
        {
            string currentFileName=string.Empty;
            switch (name)
            {
                case "RM":
                    currentFileName = Current_RM_FileName;
                    break;
                case "WKP":
                    currentFileName = Current_WKP_FileName;
                    break;
                case "SC":
                    currentFileName = Current_SC_FileName;
                    break;
                default:
                    break;
            }
            currentVendorName = name;
             updateClient.GetFileSizeAsync(currentFileName);
        }

        void updateClient_GetNewSuiteVersionMapsCompleted(object sender, GetNewSuiteVersionMapsCompletedEventArgs e)
        {
            if (e.Result != null && e.Result.IsSuccess )
            {
                if (e.Result.TotalRecord > 0)
                {
                    foreach (SuiteVersionMap item in e.Result.Result)
                    {
                        if (item != null && !string.IsNullOrEmpty(item.UnifyVersion))
                        {
                            InitialUnifiedVersion = item.UnifyVersion;
                        }
                        switch (item.Vendor)
                        {
                            case "RM":
                                Initial_RM_SuiteVersionMap = item;
                                break;
                            case "WKP":
                                Initial_WKP_SuiteVersionMap = item;
                                break;
                            case "SC":
                                Initial_SC_SuiteVersionMap = item;
                                break;
                            default:
                                break;
                        }
                    }
                    CurrentUnifiedVersion = InitialUnifiedVersion;
                    if (string.IsNullOrEmpty(InitialUnifiedVersion) || !InitialUnifiedVersion.StartsWith("V"))
                    {
                        CurrentUnifiedVersion = "V1.0";
                    }
                    else
                    {
                        int intVersionNum = int.Parse(InitialUnifiedVersion.Substring(1, InitialUnifiedVersion.Length - 3)) + 1;
                        CurrentUnifiedVersion = "V" + intVersionNum.ToString() + ".0";
                    }
                }
                else
                {
                    CurrentUnifiedVersion = "V1.0";
                }
            }
            Reset();
        }

        void updateClient_AddSuiteVersionMapsCompleted(object sender, AddSuiteVersionMapsCompletedEventArgs e)
        {
            if (e.Result == null || !e.Result.IsSuccess)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"));
            }
            Return();
        }

        protected override void OnCommitted()
        {
            ObservableCollection<SuiteVersionMap> SuiteVersionMaps = new ObservableCollection<SuiteVersionMap>();
            Current_RM_SuiteVersionMap.Vendor = "RM";
            Current_RM_SuiteVersionMap.UnifyVersion = CurrentUnifiedVersion;
            Current_RM_SuiteVersionMap.VendorVersion = Current_RM_VendorVersion;
            Current_RM_SuiteVersionMap.FileName = Current_RM_FileName;
            Current_RM_SuiteVersionMap.FileSize = Current_RM_FileSize;
            SuiteVersionMaps.Add(Current_RM_SuiteVersionMap);
            Current_WKP_SuiteVersionMap.Vendor = "WKP";
            Current_WKP_SuiteVersionMap.UnifyVersion = CurrentUnifiedVersion;
            Current_WKP_SuiteVersionMap.VendorVersion = Current_WKP_VendorVersion;
            Current_WKP_SuiteVersionMap.FileName = Current_WKP_FileName;
            Current_WKP_SuiteVersionMap.FileSize = Current_WKP_FileSize; ;
            SuiteVersionMaps.Add(Current_WKP_SuiteVersionMap);
            Current_SC_SuiteVersionMap.Vendor = "SC";
            Current_SC_SuiteVersionMap.UnifyVersion = CurrentUnifiedVersion;
            Current_SC_SuiteVersionMap.VendorVersion = Current_SC_VendorVersion;
            Current_SC_SuiteVersionMap.FileName = Current_SC_FileName;
            Current_SC_SuiteVersionMap.FileSize = Current_SC_FileSize; ;
            SuiteVersionMaps.Add(Current_SC_SuiteVersionMap);
            updateClient.AddSuiteVersionMapsAsync(SuiteVersionMaps);
        }

        private void Reset()
        {
            if (Initial_RM_SuiteVersionMap == null)
            {
                Current_RM_SuiteVersionMap = new SuiteVersionMap();
            }
            else
            {
                Current_RM_SuiteVersionMap = MaintainCommon.Clone(Initial_RM_SuiteVersionMap);
            }
            Current_RM_VendorVersion = Current_RM_SuiteVersionMap.VendorVersion;
            Current_RM_FileName = Current_RM_SuiteVersionMap.FileName;
            Current_RM_FileSize = Current_RM_SuiteVersionMap.FileSize;
            if (Initial_WKP_SuiteVersionMap == null)
            {
                Current_WKP_SuiteVersionMap = new SuiteVersionMap();
            }
            else
            {
                Current_WKP_SuiteVersionMap = MaintainCommon.Clone(Initial_WKP_SuiteVersionMap);
            }
            Current_WKP_VendorVersion = Current_WKP_SuiteVersionMap.VendorVersion;
            Current_WKP_FileName = Current_WKP_SuiteVersionMap.FileName;
            Current_WKP_FileSize = Current_WKP_SuiteVersionMap.FileSize;
            if (Initial_SC_SuiteVersionMap == null)
            {
                Current_SC_SuiteVersionMap = new SuiteVersionMap();
            }
            else
            {
                Current_SC_SuiteVersionMap = MaintainCommon.Clone(Initial_SC_SuiteVersionMap);
            }
            Current_SC_VendorVersion = Current_SC_SuiteVersionMap.VendorVersion;
            Current_SC_FileName = Current_SC_SuiteVersionMap.FileName;
            Current_SC_FileSize = Current_SC_SuiteVersionMap.FileSize;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Current_RM_VendorVersion));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Current_WKP_VendorVersion));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Current_SC_VendorVersion));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Current_RM_FileName));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Current_WKP_FileName));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Current_SC_FileName));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Current_RM_FileSize));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Current_WKP_FileSize));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Current_SC_FileSize));
            //CurrentUnifiedVersion = string.Empty;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentUnifiedVersion));
        }

        #region 数据校验

        private string _Current_RM_VendorVersion;
        public string Current_RM_VendorVersion
        {
            get { return _Current_RM_VendorVersion; }
            set
            {
                _Current_RM_VendorVersion = value == null ? null : value.Trim();
                Validate(ExtractPropertyName(() => Current_RM_VendorVersion), value);
            }
        }

        private string _Current_WKP_VendorVersion;
        public string Current_WKP_VendorVersion
        {
            get { return _Current_WKP_VendorVersion; }
            set
            {
                _Current_WKP_VendorVersion = value == null ? null : value.Trim();
                Validate(ExtractPropertyName(() => Current_WKP_VendorVersion), value);
            }
        }

        private string _Current_SC_VendorVersion;
        public string Current_SC_VendorVersion
        {
            get { return _Current_SC_VendorVersion; }
            set
            {
                _Current_SC_VendorVersion = value == null ? null : value.Trim();
                Validate(ExtractPropertyName(() => Current_SC_VendorVersion), value);
            }
        }

        private string _Current_RM_FileName;
        public string Current_RM_FileName
        {
            get { return _Current_RM_FileName; }
            set
            {
                _Current_RM_FileName = value == null ? null : value.Trim();
                Validate(ExtractPropertyName(() => Current_RM_FileName), value);
            }
        }

        private string _Current_WKP_FileName;
        public string Current_WKP_FileName
        {
            get { return _Current_WKP_FileName; }
            set
            {
                _Current_WKP_FileName = value == null ? null : value.Trim();
                Validate(ExtractPropertyName(() => Current_WKP_FileName), value);
            }
        }

        private string _Current_SC_FileName;
        public string Current_SC_FileName
        {
            get { return _Current_SC_FileName; }
            set
            {
                _Current_SC_FileName = value == null ? null : value.Trim();
                Validate(ExtractPropertyName(() => Current_SC_FileName), value);
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
        private int _Current_RM_FileSize;
        public int Current_RM_FileSize
        {
            get { return _Current_RM_FileSize; }
            set
            {
                _Current_RM_FileSize = value;
                Validate(ExtractPropertyName(() => Current_RM_FileSize), value);
            }
        }

        private int _Current_WKP_FileSize;
        public int Current_WKP_FileSize
        {
            get { return _Current_WKP_FileSize; }
            set
            {
                _Current_WKP_FileSize = value;
                Validate(ExtractPropertyName(() => Current_WKP_FileSize), value);
            }
        }

        private int _Current_SC_FileSize;
        public int Current_SC_FileSize
        {
            get { return _Current_SC_FileSize; }
            set
            {
                _Current_SC_FileSize = value;
                Validate(ExtractPropertyName(() => Current_SC_FileSize), value);
            }
        }

        private void ValidateLength(string prop, object value)
        {
            ClearErrors(prop);
            if (value == null || !(value is int) || Convert.ToInt32(value) < 0)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"));//格式非法
            }
        }

        #endregion

        private void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(MaintainName.VersionMappingV, new Dictionary<string, object>() { { "action", "return" } }));
        }
    }
}
