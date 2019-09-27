using Gsafety.PTMS.Bases.Librarys;
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
using System.Linq;

namespace Gsafety.PTMS.Bases.Models
{
    public class MonityEntityBase : TreeNode
    {
        public Visibility DescriptionVisibility { get; set; }
        Visibility FunctionItemVisibility { get; set; }

        public MonityEntityBase()
            : base()
        {
            Children.CollectionChanged += Children_CollectionChanged;
        }

        void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Update();
        }

        private int _vehicleOnlineCount = 0;
        public int VehicleOnlineCount
        {
            get { return _vehicleOnlineCount; }
            set
            {
                if (_vehicleOnlineCount != value)
                {
                    _vehicleOnlineCount = value;

                    RaisePropertyChanged(() => VehicleOnlineCount);

                    if (Parent != null)
                    {
                        var orgEx = Parent as OrganizationEx;
                        orgEx.Update();
                    }
                }
            }
        }

        private int _vehicleCount = 0;
        public int VehicleCount
        {
            get { return _vehicleCount; }
            set
            {
                if (_vehicleCount != value)
                {
                    _vehicleCount = value;
                    RaisePropertyChanged(() => VehicleCount);

                    if (Parent != null)
                    {
                        var orgEx = Parent as OrganizationEx;
                        orgEx.Update();
                    }
                }
            }
        }

        public void Update()
        {
            VehicleCount = Children.Sum(t => (t as MonityEntityBase).VehicleCount);
            VehicleOnlineCount = Children.Sum(t => (t as MonityEntityBase).VehicleOnlineCount);
        }
    }
}
