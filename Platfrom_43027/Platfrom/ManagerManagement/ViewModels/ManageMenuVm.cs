/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 8bc119bd-233f-494b-a651-11dfbf37c8e0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels
/////    Project Description:    
/////             Class Name: ManageMenuVm
/////          Class Version: v1.0.0.0
/////            Create Time: 8/8/2013 2:08:41 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/8/2013 2:08:41 PM
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
using System.Linq;
using System.Windows.Shapes;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System.Collections.ObjectModel;
using Gsafety.PTMS.Share;
using System.ComponentModel;
using System.Reflection;

namespace Gsafety.PTMS.Manager.ViewModels
{
    [ExportAsViewModel(ManagerName.ManagerMenuVm)]
    public class ManageMenuVm : BaseViewModel
    {
        #region Fields

        ObservableCollection<MenuInfo> _ManagerMenuItems = null;
        #endregion

        #region Attributes

        public ObservableCollection<MenuInfo> UserManagerMenuItems
        {
            get { return GetMenuInfo(ManagerName.UserMangeMenuName); }
        }

        public ObservableCollection<MenuInfo> LogManageMenuItems
        {
            get { return GetMenuInfo(ManagerName.LogManageMenuName); }
        }
        public ObservableCollection<MenuInfo> SettingManageMenuItems
        {
            get { return GetMenuInfo(ManagerName.SettingManageMenuName); }
        }
        public ObservableCollection<MenuInfo> CommandManageMenuItems
        {
            get { return GetMenuInfo(ManagerName.CommandManageMenuName); }
        }
        #endregion

        public ManageMenuVm()
        {
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);

        }

        protected override void InitializeVm()
        {
            try
            {
                base.InitializeVm();
                if (_ManagerMenuItems == null || _ManagerMenuItems.Count == 0)
                {
                    _ManagerMenuItems = ApplicationContext.Instance.MenuManager.GetNavigateInfos(Router, ManagerName.CategoryName);
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                    {
                        RaisePropertyChanged(() => UserManagerMenuItems);
                        RaisePropertyChanged(() => LogManageMenuItems);
                        RaisePropertyChanged(() => SettingManageMenuItems);
                        RaisePropertyChanged(() => CommandManageMenuItems);

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
            if (_ManagerMenuItems == null || _ManagerMenuItems.Count == 0)
                return null;
            var result = _ManagerMenuItems.Where(item => item.SubMenuType.Equals(SubMenuName)).OrderBy(item => item.Order);
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
