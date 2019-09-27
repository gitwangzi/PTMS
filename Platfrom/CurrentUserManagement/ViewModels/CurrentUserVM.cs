using Gsafety.PTMS.ServiceReference.ADGroupService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7e1a78cf-e5f5-423a-80a5-6f4ea950157f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CurrentUserManagement.ViewModels
/////    Project Description:    
/////             Class Name: CurrentUserVM
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-12-16 14:52:03
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-12-16 14:52:03
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
using Jounce.Framework.Command;
using Jounce.Core.View;
using System.Collections.Generic;

namespace Gsafety.PTMS.CurrentUserManagement.ViewModels
{
    [ExportAsViewModel(CurrentUserName.CurrentUserModel)]
    public class CurrentUserVM : BaseViewModel
    {
      
        private string _groupName;
        public string groupName
        {
            get { return _groupName; }
            set
            {
                _groupName = value;
            }
        }


        public CurrentUserVM()
        {

            Init();
        }

        private void Init()
        {
            groupName = ApplicationContext.Instance.AuthenticationInfo.GroupName;
            if (groupName == UserGroup.SecurityAdmin || groupName == UserGroup.SecurityManager || groupName == UserGroup.AlarmFilterCommissioner)
            {
                CurrentUrl = "/TrafficUserInfoView";
                TrafficModel = new TrafficUserInfoVM();
            }
            else if (groupName == UserGroup.SiteManager)
            {
                CurrentUrl = "/CurrentUserInfo";
            }
            else if (groupName == UserGroup.SysMaintain)
            {
                CurrentUrl = "/CurrentUserInfo";
            }

        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            Init();
        }

        protected override void DeactivateView(string viewName)
        {
            base.DeactivateView(viewName);
        }



        public TrafficUserInfoVM TrafficModel { get; private set; }

        private string _currentUrl;
        public string CurrentUrl
        {
            get { return this._currentUrl; }
            set
            {
                if (_currentUrl != value)
                {
                    _currentUrl = value;
                    this.RaisePropertyChanged(() => CurrentUrl);
                }
            }
        }

        public AuthenticationInfo UserInfo
        {
            get
            {
                return ApplicationContext.Instance.AuthenticationInfo;
            }
        }
    }
}
