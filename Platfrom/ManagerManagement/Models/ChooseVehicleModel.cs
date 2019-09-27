using Gsafety.PTMS.Bases.Librarys;
using Gsafety.PTMS.Share;
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
using Gsafety.PTMS.Bases.Models;
namespace Gsafety.PTMS.Manager.Models
{
    public class ChooseVehicleModel
    {
        private TreeViewModel<MonitorEntity> _TreeViews;

        public TreeViewModel<MonitorEntity> TreeViews
        {
            get
            {
                if (_TreeViews == null || _TreeViews.Nodes.Count == 0)
                {
                    ObservableCollection<Province> provinces = ApplicationContext.Instance.BufferManager.DistrictManager.Provinces;
                    if (provinces != null && provinces.Count > 0)
                    {
                        _TreeViews = new TreeViewModel<MonitorEntity>(new ObservableCollection<MonitorEntity>(provinces), (MonitorEntity e) => e.GetChilds());
                    }
                }
                return _TreeViews;
            }
        }
    }
}
