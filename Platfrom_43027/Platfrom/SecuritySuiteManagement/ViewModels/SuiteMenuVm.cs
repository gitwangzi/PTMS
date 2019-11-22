/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 504ece6d-fcf9-46be-bcfc-870c8a466453      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.ViewModels
/////    Project Description:    
/////             Class Name: SuiteMenuVm
/////          Class Version: v1.0.0.0
/////            Create Time: 8/7/2013 11:54:59 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/7/2013 11:54:59 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
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
using System.Collections.ObjectModel;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Gsafety.PTMS.Share;
using System.Collections.Generic;
using System.Reflection;


namespace Gsafety.PTMS.SecuritySuite.ViewModels
{
    [ExportAsViewModel(SecuritySuiteName.SuiteMenuVm)]
    public class SuiteMenuVm:BaseViewModel
    {
        #region Fields

        ObservableCollection<MenuInfo> _SuiteMenuItems = null;

        #endregion

        #region Attributes

        public ObservableCollection<MenuInfo> InstallMenuItems
        {
            get
            {
                return GetMenuInfo(SecuritySuiteName.InstallMenuName);
            }
        }

        public ObservableCollection<MenuInfo> StatusMenuItems
        {
            get
            {
                return GetMenuInfo(SecuritySuiteName.StatusMenuName);
            }
        }

        public ObservableCollection<MenuInfo> MaintainMenuItems
        {
            get 
            {
               return GetMenuInfo(SecuritySuiteName.MaintainMenuName);
            }
        }

        public ObservableCollection<MenuInfo> VehicleTrafficItems
        {
            get {
                return GetMenuInfo(SecuritySuiteName.VehicleTrafficMenuName);
            }
        }

        public ObservableCollection<MenuInfo> VehicleEquipmentItems
        {
            get
            {
                return GetMenuInfo(SecuritySuiteName.VehicleEquipmentMenuName);
            }
        }

        #endregion

        public SuiteMenuVm()
        {
            try
            {
            }
            catch (Exception ex)
            { 
            
            }
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
            }
            catch (Exception ex)
            { 
            
            }
        }

        protected override void InitializeVm()
        {
            try
            {
                base.InitializeVm();
                if (_SuiteMenuItems == null || _SuiteMenuItems.Count == 0)
                {
                    _SuiteMenuItems = ApplicationContext.Instance.MenuManager.GetNavigateInfos(Router, SecuritySuiteName.CategoryName);
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                    {
                        RaisePropertyChanged(() => InstallMenuItems);
                        RaisePropertyChanged(() => StatusMenuItems);
                        RaisePropertyChanged(() => MaintainMenuItems);
                        RaisePropertyChanged(() => VehicleTrafficItems);
                        RaisePropertyChanged(() => VehicleEquipmentItems);
                    });
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private ObservableCollection<MenuInfo> GetMenuInfo(string SubMenuName)
        {
                if (_SuiteMenuItems == null || _SuiteMenuItems.Count == 0)
                    return null;
                var result = _SuiteMenuItems.Where(item => item.SubMenuType.Equals(SubMenuName)).OrderBy(item => item.Order);
                if (result == null || result.Count() == 0)
                    return null;
                ObservableCollection<MenuInfo> menuItems = new ObservableCollection<MenuInfo>();
                foreach (var item in result)
                {
                    menuItems.Add(item);
                }
            return menuItems;
                
        }
    }
}
