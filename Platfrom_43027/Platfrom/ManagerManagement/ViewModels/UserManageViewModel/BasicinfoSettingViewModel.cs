/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d8229908-48ea-4f44-8672-216fd7f55621      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels
/////    Project Description:    
/////             Class Name: BasicinfoSettingViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/5 16:12:38
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/5 16:12:38
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Manager.Models;
using Gsafety.PTMS.ServiceReference.AppConfigService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Gsafety.PTMS.Manager.ViewModels
{
    [ExportAsViewModel(ManagerName.BasicinfoSettingViewModel)]
    public class BasicinfoSettingViewModel : BaseEntityViewModel
    {
        AppConfigManagerClient appClient = ServiceClientFactory.Create<AppConfigManagerClient>();
        private ObservableCollection<AppConfigs> _ConfigInfoList;

        public Gsafety.PTMS.ServiceReference.AppConfigService.AppConfig tempconfig;

        public ObservableCollection<AppConfigs> ConfigInfoList
        {
            get { return _ConfigInfoList; }
            set { _ConfigInfoList = value; }
        }
        private ObservableCollection<Gsafety.PTMS.ServiceReference.AppConfigService.AppConfig> _RawInfoList;

        public ObservableCollection<Gsafety.PTMS.ServiceReference.AppConfigService.AppConfig> RawInfoList
        {
            get { return _RawInfoList; }
            set { _RawInfoList = value; }
        }
        private AppConfigs _CurrentConfigInfo;
        public IActionCommand ResetCommand { get; private set; }
        public ICommand CommitCommand { get; private set; }
        public IActionCommand TextCommand { get; private set; }
        public IActionCommand GotFocusCommand { get; private set; }
        public AppConfigs CurrentConfigInfo
        {
            get { return _CurrentConfigInfo; }
            set
            {
                _CurrentConfigInfo = value;
                RaisePropertyChanged(() => CurrentConfigInfo);
            }
        }

        public string SECTION_VALUE2
        {
            get
            {
                return _CurrentConfigInfo.SECTION_VALUE;
            }
            set
            {
                _CurrentConfigInfo.SECTION_VALUE = value;
                ResetCommand.RaiseCanExecuteChanged();
            }
        }
        private void Changressetcommand()
        {
            CancleEnable = true;
            ResetCommand.RaiseCanExecuteChanged();
        }
        private string _ConfigName;
        /// <summary>
        /// config name
        /// </summary>
        public string ConfigName
        {
            get { return _ConfigName; }
            set { _ConfigName = value; }
        }
        private bool _CancleEnable = false;
        /// <summary>
        /// Cancle Button
        /// </summary>
        public bool CancleEnable
        {
            get { return _CancleEnable; }
            set { _CancleEnable = value; }
        }
        private string _ConfigValue;
        /// <summary>
        /// config value
        /// </summary>
        public string ConfigValue
        {
            get { return _ConfigValue; }
            set
            {
                _ConfigValue = value;
                Validateconfig();
                RaisePropertyChanged(() => ConfigValue);
            }
        }
        /// <summary>
        ///config value validatas
        /// </summary>
        private void Validateconfig()
        {
            var prop = ExtractPropertyName(() => ConfigValue);
            ClearErrors(prop);
            if (string.IsNullOrEmpty(ConfigValue))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));
            }
        }
        public BasicinfoSettingViewModel()
        {
            CommitCommand = new ActionCommand<AppConfigs>(obj => Commit(obj));
            ResetCommand = new ActionCommand<object>(obj => Reset());
            TextCommand = new ActionCommand<object>(obj => Datachange());
            appClient.GetappConfigInfoCompleted += appClient_GetappConfigInfoCompleted;
            appClient.UpdateConfigCompleted += appClient_UpdateConfigCompleted;
        }

        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }
        private void Datachange()
        {
            if (CurrentConfigInfo.SECTION_VALUE != rawDic[CurrentConfigInfo.SECTION_NAME])
            {
                CancleEnable = true;
                ResetCommand.RaiseCanExecuteChanged();
            }
        }
        private Dictionary<string, string> rawDic;

        private bool ResetEnabled()
        {
            return (CurrentConfigInfo.SECTION_VALUE != rawDic[CurrentConfigInfo.SECTION_NAME]);
        }

        private void Reset()
        {
            CurrentConfigInfo.SECTION_VALUE = rawDic[CurrentConfigInfo.SECTION_NAME];
            ResetCommand.RaiseCanExecuteChanged();
        }

        private void Commit(AppConfigs obj)
        {
            appClient.UpdateConfigAsync(obj.SECTION_NAME, obj.SECTION_VALUE);
            rawDic[obj.SECTION_NAME] = obj.SECTION_VALUE;
        }

        void appClient_UpdateConfigCompleted(object sender, UpdateConfigCompletedEventArgs e)
        {
            if (e.Result.Result)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("System_Setting_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);

            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("System_Setting_Faild"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);

            }
        }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {

        }
        void appClient_GetappConfigInfoCompleted(object sender, GetappConfigInfoCompletedEventArgs e)
        {
            if (e.Result.Result != null)
            {
                rawDic = new Dictionary<string, string>();
                ObservableCollection<Gsafety.PTMS.ServiceReference.AppConfigService.AppConfig> ConfigInfoLists = new ObservableCollection<Gsafety.PTMS.ServiceReference.AppConfigService.AppConfig>();
                ConfigInfoLists = e.Result.Result;
                foreach (var item in ConfigInfoLists)
                {
                    rawDic.Add(item.SECTION_NAME, item.SECTION_VALUE);
                }
                ConfigInfoList = new ObservableCollection<AppConfigs>();
                int i = 0;
                foreach (var item in ConfigInfoLists)
                {

                    ConfigInfoList.Add(new AppConfigs() { Index=i, SECTION_VALUE = item.SECTION_VALUE,SECTION_DESC = item.SECTION_DESC, SECTION_UNIT = item.SECTION_UNIT,SECTION_NAME=item.SECTION_NAME });
                    i++;
                }
                tempconfig = e.Result.Result[0];
                CurrentConfigInfo = ConfigInfoList[0];
                Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                {
                    RaisePropertyChanged(() => ConfigInfoList);
                });
            }
        }
        private void GetConfigSeting()
        {
            if (CurrentConfigInfo != null)
            {
                ConfigName = CurrentConfigInfo.SECTION_NAME;
                ConfigValue = CurrentConfigInfo.SECTION_VALUE;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                {
                    RaisePropertyChanged(() => ConfigName);
                    RaisePropertyChanged(() => ConfigValue);
                });
            }
        }
    }
}
