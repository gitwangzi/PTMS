/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1a8b52f4-8dd0-41c8-8302-e4712bb1eee0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share
/////    Project Description:    
/////             Class Name: MenuManager
/////          Class Version: v1.0.0.0
/////            Create Time: 7/24/2013 5:34:43 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/24/2013 5:34:43 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
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
using Jounce.Core.ViewModel;
using Jounce.Framework.ViewModel;

namespace Gsafety.PTMS.Share
{
    public class MenuManager
    {
        IViewModelRouter _Router;

        public IViewModelRouter Router
        {
            get { return _Router; }
            set { _Router = value; }
        }

        public MenuManager()
        {
        }

        public ObservableCollection<MenuInfo> GetNavigateInfos(IViewModelRouter router, string categoryName)
        {
            if (_Router == null)
                _Router = (ViewModelRouter)router;
            ObservableCollection<MenuInfo> navigateInfos = new ObservableCollection<MenuInfo>();
            foreach (var v in from viewInfo in ((ViewModelRouter)router).Views
                              where viewInfo.Metadata.Category.Equals(categoryName)
                              select Tuple.Create(
                              viewInfo.Metadata.ExportedViewType,
                              viewInfo.Metadata.MenuName,
                              viewInfo.Metadata.MenuTitle,
                              viewInfo.Metadata.ToolTip,
                              viewInfo.Metadata.Url,
                              viewInfo.Metadata.Order))
            {
                int count = (from item in navigateInfos where item.MenuKey.Equals(v.Item1) select item.MenuKey).Count();
                if (count == 0)
                {

                    string menuTitle = ApplicationContext.Instance.StringResourceReader.GetString(v.Item3);
                    MenuInfo menuInfo = new MenuInfo(v.Item1, menuTitle, v.Item5);
                    if (!string.IsNullOrEmpty(v.Item4))
                        menuInfo.ToolTip = v.Item4;
                    if (!v.Item6.Equals(0))
                        menuInfo.Order = v.Item6;
                    if (!string.IsNullOrEmpty(v.Item2))
                        menuInfo.SubMenuType = v.Item2;
                    navigateInfos.Add(menuInfo);
                    
                }
            }

            
            return navigateInfos;
        }

        public ObservableCollection<MenuInfo> GetNavigateInfos(IViewModelRouter router,string categoryName, string subMenuName)
        {
            ObservableCollection<MenuInfo> navigateInfos = new ObservableCollection<MenuInfo>();
            foreach (var v in from viewInfo in ((ViewModelRouter)router).Views
                              where viewInfo.Metadata.Category.Equals(categoryName)
                              && viewInfo.Metadata.MenuName.Equals(subMenuName)
                              select Tuple.Create(
                              viewInfo.Metadata.ExportedViewType,
                              viewInfo.Metadata.MenuName,
                              viewInfo.Metadata.MenuTitle,
                              viewInfo.Metadata.ToolTip,
                              viewInfo.Metadata.Url,
                              viewInfo.Metadata.Order))
            {
                int count = (from item in navigateInfos where item.MenuKey.Equals(v.Item1) select item.MenuKey).Count();
                if (count == 0)
                {

                    string menuTitle = ApplicationContext.Instance.StringResourceReader.GetString(v.Item3);
                    MenuInfo menuInfo = new MenuInfo(v.Item1, menuTitle, v.Item5);
                    if (!string.IsNullOrEmpty(v.Item4))
                        menuInfo.ToolTip = v.Item4;
                    if (!v.Item6.Equals(0))
                        menuInfo.Order = v.Item6;
                    navigateInfos.Add(menuInfo);
                }
            }
            return navigateInfos;
        }
    }
}
