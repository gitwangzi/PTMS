using Jounce.Core.Model;
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
using Gsafety.PTMS.ServiceReference.OrganizationService;
using System.Collections.ObjectModel;
using Gsafety.PTMS.Bases.Librarys;

namespace Gsafety.PTMS.Bases.Models
{
    public class OrganizationEx : MonityEntityBase
    {
        private Organization _organization;
        public Organization Organization
        {
            get { return _organization; }
            private set
            {
                _organization = value;
                RaisePropertyChanged(() => Organization);
            }
        }

        public OrganizationEx(Organization organization)
            : this()
        {
            _organization = organization;
        }

        public OrganizationEx()
        {
            this.IsLeaf = false;
        }

        private bool _internalCheck;
        public bool InternalCheck
        {
            get
            {
                return _internalCheck;
            }
            set
            {
                _internalCheck = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }

        private bool _IsChecked;
        public bool IsChecked
        {
            get
            {
                return InternalCheck;
            }
            set
            {
                InternalCheck = value;
                RaisePropertyChanged(() => IsChecked);

                foreach (var item in this.Children)
                {
                    if (item.Visibility == Visibility.Collapsed)
                    {
                        continue;
                    }

                    var vechicle = item as VehicleEx;
                    if (vechicle != null)
                    {
                        vechicle.IsChecked = value;
                        continue;
                    }

                    var org = item as OrganizationEx;
                    if (org != null)
                    {
                        org.IsChecked = value;
                    }
                }
            }
        }



    }

    //public class OrganizationNode : TreeNode<OrganizationEx>
    //{

    //}

    //public class VehicleExNode : TreeNode<VehicleEx>
    //{

    //}
}
