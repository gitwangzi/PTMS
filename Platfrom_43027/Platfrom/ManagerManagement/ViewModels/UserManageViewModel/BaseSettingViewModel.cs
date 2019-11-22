using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Enums;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0ef7f3e2-52e9-4281-b5c1-209de831991a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels.UserManageViewModel
/////    Project Description:    
/////             Class Name: BaseSettingViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/15 16:43:31
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/15 16:43:31
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Manager.Views;
using Gsafety.PTMS.ServiceReference.AppConfigService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Manager.ViewModels.UserManageViewModel
{
    [ExportAsViewModel(ManagerName.BaseSettingViewModel)]
    public class BaseSettingViewModel : BaseEntityViewModel
    {
        public IActionCommand<object> QueryCommand { get; set; }
        public IActionCommand<object> UpdateCommand { get; set; }
        public IActionCommand<object> DeleteCommand { get; set; }
        public IActionCommand<object> InitPwdCommand { get; set; }
        public IActionCommand<object> ModifyPwdCommand { get; set; }
        public EnumModel AlertParaItem { get; set; }
        public List<EnumModel> AlertParaList { get; set; }
        AppConfigManagerClient appClient = ServiceClientFactory.Create<AppConfigManagerClient>();
        private ObservableCollection<Gsafety.PTMS.ServiceReference.AppConfigService.AppConfig> _ConfigList;
        private Gsafety.PTMS.ServiceReference.AppConfigService.AppConfig _CurrentConfig;
        /// <summary>
        /// current config
        /// </summary>
        public Gsafety.PTMS.ServiceReference.AppConfigService.AppConfig CurrentConfig
        {
            get { return _CurrentConfig; }
            set { _CurrentConfig = value; }
        }
        private string _AlertPara;
        /// <summary>
        /// search type
        /// </summary>
        public string AlertPara
        {
            get { return _AlertPara; }
            set { _AlertPara = value; }
        }
        private void QueryAction()
        {
            appClient.GetappConfigInfoAsync(AlertParaItem.EnumName);
        }
        public ObservableCollection<Gsafety.PTMS.ServiceReference.AppConfigService.AppConfig> ConfigList
        {
            get { return _ConfigList; }
            set { _ConfigList = value; }
        }

        private void AlertParaType()
        {
            AlertParaList = new List<EnumModel>();
            AlertParaList.Add(new EnumModel { ShowName = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_All"), EnumName = string.Empty, EnumValue = 0 });
            AlertParaItem = AlertParaList[0];
            foreach(int nvalue in Enum.GetValues(typeof(AlertParaEnum)))
           {
               string strName = ApplicationContext.Instance.StringResourceReader.GetString(Enum.GetName(typeof(AlertParaEnum),nvalue));
               EnumModel item = new EnumModel { EnumName=Enum.GetName(typeof(AlertParaEnum),nvalue), ShowName = strName, EnumValue = nvalue };
               AlertParaList.Add(item);
           }
        }


        public BaseSettingViewModel()
        {
            PageSizeList = new List<int> { 20,40,80 };
            PageSize = 20;
            AlertParaType();
            QueryCommand = new ActionCommand<object>(obj => QueryAction());
            UpdateCommand = new ActionCommand<object>(obj => UpdateCommandAction(obj));
            appClient.GetappConfigInfoCompleted += appClient_GetappConfigInfoCompleted;
            appClient.GetappConfigInfoAsync(AlertParaItem.EnumName);           
        }

        private void UpdateCommandAction(object obj)
        {
            ParamSetting parawindow = new ParamSetting(CurrentConfig);
            parawindow.ResultAction = R => HandleAlertWindowAction(R);
            parawindow.Show();
        }

        private void HandleAlertWindowAction(bool R)
        {
            QueryAction();
        }

       
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
        }
        private List<int> _PageSizeList;
        public List<int> PageSizeList
        {
            get
            {
                return _PageSizeList;
            }
            set
            {
                _PageSizeList = value;
            }
        }
        private int _PageSizeValue = 20;
        public int PageSizeValue
        {
            get
            {
                return _PageSizeValue;
            }
            set
            {
                _PageSizeValue = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageSizeValue));
                PageSizeChenged();
            }
        }
        private PagedCollectionView _ConfigInfoPageView;

        public PagedCollectionView ConfigInfoPageView
        {
            get { return _ConfigInfoPageView; }
            set { _ConfigInfoPageView = value;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ConfigInfoPageView));
            }
        }
        private int _PageSize;

        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }
        private void PageSizeChenged()
        {
            PageSize = PageSizeValue;

            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageSize));
            QueryAction();
        }
        private void appClient_GetappConfigInfoCompleted(object sender, GetappConfigInfoCompletedEventArgs e)
        {
            if (e.Error!=null)
            {
                
            }
            if (e.Result.Result!=null)
            {
                ConfigList = e.Result.Result;
                if (ConfigList.Count == 0)
                {
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                ConfigInfoPageView = new PagedCollectionView(ConfigList);
            }
        }
    }
}
