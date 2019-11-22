/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ba0c20fd-6d2a-447c-ab54-4edaeb403fe6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.ViewModels
/////    Project Description:    
/////             Class Name: BaseInfoMenuVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/9 11:41:40
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/9 11:41:40
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
using Jounce.Core.ViewModel;
using System.Collections.ObjectModel;
using Gsafety.PTMS.Share;
using Jounce.Framework.Command;
using System.Linq;
using Gsafety.Common.Utilities;
using System.Collections.Generic;
using System.Reflection;

namespace Gsafety.PTMS.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.BaseInfoMenuVm)]
    public class BaseInfoMenuVm : BaseViewModel
    {
        #region Fields

        ObservableCollection<MenuInfo> _BaseInfoMenuItems = null;

        #endregion

        #region Attributes

        public ObservableCollection<MenuInfo> BaseInfoMenuItems
        {
            get
            {
                return _BaseInfoMenuItems;
            }
        }
        #endregion

        public BaseInfoMenuVm()
        {
        }

        protected override void InitializeVm()
        {
            try
            {
                base.InitializeVm();
                if (_BaseInfoMenuItems == null || _BaseInfoMenuItems.Count == 0)
                {
                    _BaseInfoMenuItems = ApplicationContext.Instance.MenuManager.GetNavigateInfos(Router, BaseInformationName.CategoryName);
                    var result = _BaseInfoMenuItems.OrderBy(item => item.Order);
                    ObservableCollection<MenuInfo> menuItems = new ObservableCollection<MenuInfo>();
                    foreach (var item in result)
                    {
                        menuItems.Add(item);
                    }
                    _BaseInfoMenuItems = menuItems;

                    Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                    {
                        RaisePropertyChanged(() => BaseInfoMenuItems);
                    });
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
