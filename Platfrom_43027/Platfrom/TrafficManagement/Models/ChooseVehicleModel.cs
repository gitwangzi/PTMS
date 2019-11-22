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
namespace Gsafety.PTMS.Traffic.Models
{
    public class ChooseVehicleModel
    {

        public CreateVehicleTree createVehicleTree;
        public ChooseVehicleModel()
        {
            if (createVehicleTree == null)
            {
                createVehicleTree = new CreateVehicleTree(Gsafety.PTMS.Enums.VehicleTreeType.Traffic);
            }
        }
        private TreeViewModel<IMonitorEntity> _TreeViews;

        public TreeViewModel<IMonitorEntity> TreeViews
        {
            get
            {
                if (_TreeViews == null || _TreeViews.Nodes.Count == 0)
                {
                    //ObservableCollection<Province> provinces = ApplicationContext.Instance.BufferManager.DistrictManager.Provinces;

                    //if (provinces != null && provinces.Count > 0)
                    //{
                        //_TreeViews = new TreeViewModel<MonitorEntity>(new ObservableCollection<MonitorEntity>(provinces), (MonitorEntity e) => e.GetChilds());
                    if (createVehicleTree == null) createVehicleTree = new CreateVehicleTree(Gsafety.PTMS.Enums.VehicleTreeType.Traffic);
                    _TreeViews = createVehicleTree.tree;

                    //}
                }
                return _TreeViews;
            }
        }
    }
}
