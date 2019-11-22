using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Tasks;
using Gsafety.PTMS.Share;
using Jounce.Core.Model;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GisManagement.ViewModels
{
    public class NetAnalyze : BaseNotify
    {
        public ObservableCollection<Graphic> RouteStops
        {
            get
            {
                return RouteAnalyze.RouteStops;
            }
        }

        public ObservableCollection<Graphic> RouteBarriers
        {
            get
            {
                return RouteAnalyze.RouteBarriers;
            }
        }


        public ObservableCollection<Graphic> Routes
        {
            get
            {
                return RouteAnalyze.Routes;
            }
        }      
    }

    public static class RouteAnalyze
    {
        public static RouteTask _routeTask;
        private static ObservableCollection<Graphic> _RouteStops = new ObservableCollection<Graphic>();
        public static ObservableCollection<Graphic> RouteStops
        {
            get
            {
                return _RouteStops;
            }
            set
            {
                _RouteStops = value;
            }
        }

        private static ObservableCollection<Graphic> _RouteBarriers = new ObservableCollection<Graphic>();
        public static ObservableCollection<Graphic> RouteBarriers
        {
            get
            {
                return _RouteBarriers;
            }
            set
            {
                _RouteBarriers = value;
            }
        }

        private static ObservableCollection<Graphic> _Routes = new ObservableCollection<Graphic>();
        public static ObservableCollection<Graphic> Routes
        {
            get
            {
                return _Routes;
            }
            set
            {
                _Routes = value;
            }
        }

        /// <summary>
        /// Temporarily do not offer this feature
        /// </summary>
        /// <param name="spl"></param>
        public static void Solved(ESRI.ArcGIS.Client.Geometry.SpatialReference spl)
        {
            try
            {
                _routeTask = new RouteTask("http://172.16.20.25:6080/ArcGIS/rest/services//NAServer/Route");
                _routeTask.SolveCompleted += routeTask_SolveCompleted;
                RouteParameters routeParameters = new RouteParameters()
                {
                    Stops = _RouteStops,
                    Barriers = _RouteBarriers,
                    OutSpatialReference = spl,
                    ReturnDirections = true,
                    FindBestSequence = true,
                    PreserveFirstStop = true,
                    PreserveLastStop = true,
                };

                if (_routeTask.IsBusy) _routeTask.CancelAsync();
                _routeTask.SolveAsync(routeParameters);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        private static void routeTask_SolveCompleted(object sender, RouteEventArgs e)
        {
            _Routes.Clear();//Empty path layers before
            RouteResult routeResult = e.RouteResults[0];//Best route query only one result, the length of the array is 1.
            _Routes.Add(routeResult.Route);//Add the path to the current map
        }
    }
}
