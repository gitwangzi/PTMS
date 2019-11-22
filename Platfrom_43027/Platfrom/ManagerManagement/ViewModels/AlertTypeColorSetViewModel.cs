/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: f8e2dc4e-f451-47b7-8aa0-7aa7bb96df28      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels
/////    Project Description:    
/////             Class Name: AlertTypeColorSetViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/18 9:53:33
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/18 9:53:33
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Manager.Views;
using Gsafety.PTMS.ServiceReference.AppConfigService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Gsafety.PTMS.Manager.ViewModels
{
    [ExportAsViewModel(ManagerName.AlertTypeColorSetViewModel)]
    public class AlertTypeColorSetViewModel : BaseEntityViewModel
    {
        public IActionCommand<object> QueryCommand { get; set; }
        public IActionCommand<object> UpdateCommand { get; set; }
        public IActionCommand<object> DeleteCommand { get; set; }
        public IActionCommand<object> InitPwdCommand { get; set; }
        public IActionCommand<object> ModifyPwdCommand { get; set; }
        AppConfigManagerClient appClient = ServiceClientFactory.Create<AppConfigManagerClient>();
        private ObservableCollection<Gsafety.PTMS.ServiceReference.AppConfigService.AppConfig> _ConfigList;
        private Gsafety.PTMS.ServiceReference.AppConfigService.AppConfig _CurrentConfig;
        /// <summary>
        /// Current Config
        /// </summary>
        public Gsafety.PTMS.ServiceReference.AppConfigService.AppConfig CurrentConfig
        {
            get { return _CurrentConfig; }
            set { _CurrentConfig = value; }
        }
        public ObservableCollection<Gsafety.PTMS.ServiceReference.AppConfigService.AppConfig> ConfigList
        {
            get { return _ConfigList; }
            set { _ConfigList = value; }
        }
        public VehicleAlertType AlertType { get; set; }
        public AlertTypeColorSetViewModel()
        {
            var targetlist = new List<Gsafety.PTMS.Bases.Enums.EnumInfos>();
            if (ApplicationContext.Instance.ServerConfig.AlertType == "0")
            {
                targetlist = new EnumAdapter<BusinessAlertType>().GetEnumInfos().Where(f => f.EnumAttributeInfo.Flag == "0").ToList();
            }
            else if (ApplicationContext.Instance.ServerConfig.AlertType == "1")
            {
                targetlist = new EnumAdapter<BusinessAlertType>().GetEnumInfos().Where(f => f.EnumAttributeInfo.Flag == "0").ToList();
                targetlist.AddRange(new EnumAdapter<BusinessAlertType>().GetEnumInfos().Where(f => f.EnumAttributeInfo.Flag == "1").ToList());
            }
            VehicleAlertTypeList = new ObservableCollection<VehicleAlertType>();
            foreach (var item in targetlist)
            {
                VehicleAlertTypeList.Add(new VehicleAlertType
                {
                    Code = (short)item.Value,
                    DataName = item.Name,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString(item.Name)
                });
            }
            VehicleAlertTypeList.Insert(0, new VehicleAlertType { Code = 0, DataName = "All", Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect") });
            this.AlertType = VehicleAlertTypeList.FirstOrDefault();
            PageSizeList = new List<int> { 20, 40, 80 };
            PageSize = 20;
            QueryCommand = new ActionCommand<object>(obj => QueryAction());
            UpdateCommand = new ActionCommand<object>(obj => UpdateCommandAction(obj));
            appClient.GetConfigInfoBytypeCompleted += appClient_GetConfigInfoBytypeCompleted;
            appClient.GetConfigInfoBytypeAsync("AlertType", this.AlertType.DataName);
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleAlertTypeList));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlertType));
        }
        private void appClient_GetConfigInfoBytypeCompleted(object sender, GetConfigInfoBytypeCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            if (e.Result.Result != null)
            {
                ConfigList = e.Result.Result;
                if (ConfigList.Count == 0)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                ConfigInfoPageView = new PagedCollectionView(ConfigList);


            }
        }

        private void UpdateCommandAction(object obj)
        {
            AlertTypeColor colorWindow = new AlertTypeColor(CurrentConfig.SECTION_DESC, CurrentConfig.SECTION_VALUE, CurrentConfig.SECTION_NAME);
            colorWindow.ResultAction = (name, value) => HandleAction(name, value);
            colorWindow.Show();

        }

        private void HandleAction(string name, string value)
        {
            if (value != CurrentConfig.SECTION_VALUE)
            {
                ApplicationContext.Instance.BufferManager.AlertConfigManager.ConfigList.Where(e => e.SECTION_NAME == name).FirstOrDefault().SECTION_VALUE = value;
                EventAggregator.Publish<ColorConfigChange>(new ColorConfigChange());
            }
            QueryAction();
        }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            QueryAction();
        }

        private void QueryAction()
        {
            appClient.GetConfigInfoBytypeAsync("AlertType", this.AlertType.DataName);
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
            set
            {
                _ConfigInfoPageView = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ConfigInfoPageView));
            }
        }
        public ObservableCollection<VehicleAlertType> VehicleAlertTypeList { get; set; }
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
    }
    public class VehicleAlertType
    {
        public short Code { get; set; }
        public string Name { get; set; }
        public string DataName { get; set; }
    }
}
