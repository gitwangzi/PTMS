using Jounce.Core.ViewModel;
using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PublicServiceManagement
{
    public class PublicServiceBinding
    {
        [Export]
        public ViewModelRoute MainPageBinding
        {
            get
            {
                return ViewModelRoute.Create(PublicServiceName.PublicServiceMainPageVm, PublicServiceName.PublicServiceMainPageV);
            }
        }

        [Export]
        public ViewModelRoute FoundRegistryManageBinding
        {
            get
            {
                return ViewModelRoute.Create(PublicServiceName.FoundRegistryManageVm, PublicServiceName.FoundRegistryManageV);
            }
        }

        [Export]
        public ViewModelRoute FoundRegistryDetailBinding
        {
            get
            {
                return ViewModelRoute.Create(PublicServiceName.FoundRegistryDetailVm, PublicServiceName.FoundRegistryDetailV);
            }
        }

        [Export]
        public ViewModelRoute LostRegistryManageBinding
        {
            get
            {
                return ViewModelRoute.Create(PublicServiceName.LostRegistryManageVm, PublicServiceName.LostRegistryManageV);
            }
        }

        [Export]
        public ViewModelRoute LostRegistryDetailBinding
        {
            get
            {
                return ViewModelRoute.Create(PublicServiceName.LostRegistryDetailVm, PublicServiceName.LostRegistryDetailV);
            }
        }

        [Export]
        public ViewModelRoute MdvrMsgManageBinding
        {
            get
            {
                return ViewModelRoute.Create(PublicServiceName.MdvrMsgManageVm, PublicServiceName.MdvrMsgManageV);
            }
        }

        [Export]
        public ViewModelRoute PhoneMsgManageBinding
        {
            get
            {
                return ViewModelRoute.Create(PublicServiceName.PhoneMsgManageVm, PublicServiceName.PhoneMsgManageV);
            }
        }
    }
}
