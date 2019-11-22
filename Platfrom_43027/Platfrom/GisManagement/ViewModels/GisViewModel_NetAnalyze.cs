using ESRI.ArcGIS.Client;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using System;
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
    public partial class GisViewModel
    {
       
        public ESRI.ArcGIS.Client.Draw myDrawPoint = null;

        public IActionCommand AddStopsCommand { get; private set; }
        public IActionCommand AddBarriersCommand { get; private set; }
        public IActionCommand SolvedCommand { get; private set; }

        /// <summary>
        /// Adding parking spots
        /// </summary>
        private void _AddStops()
        {
            myDrawPoint = new ESRI.ArcGIS.Client.Draw(this.MyMap);
            myDrawPoint.DrawMode = DrawMode.Polyline;
            myDrawPoint.DrawComplete += myDrawStops_DrawComplete;
            myDrawPoint.IsEnabled = true;
        }

        /// <summary>
        /// Adding an event to complete parking spots
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myDrawStops_DrawComplete(object sender, DrawEventArgs e)
        {
            myDrawPoint.IsEnabled = false;

            try
            {

                Graphic newgraphic = new Graphic()
                {
                    Geometry = (e.Geometry as ESRI.ArcGIS.Client.Geometry.Polyline).Clone(),
                    Symbol = new ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol()
                    {
                        Style = ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol.LineStyle.Dash,
                        Width = 2,
                        Color = new SolidColorBrush(Colors.Blue),
                    }
                };
                MonitorList.GpsHisDataVechileGraphics.AddGraphic(newgraphic, "Pre-");

                // (MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer).Graphics.Add(gPhic);
                //RouteAnalyze.RouteStops.Add(gPhic);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("GisViewModel", ex);
            }
        }


        /// <summary>
        /// Add roadblock
        /// </summary>
        private void _AddBarriers()
        {
            myDrawPoint = new ESRI.ArcGIS.Client.Draw(this.MyMap);
            myDrawPoint.DrawMode = DrawMode.Point;
            myDrawPoint.DrawComplete += myDrawBarriers_DrawComplete;
            myDrawPoint.IsEnabled = true;
        }

        /// <summary>
        /// Add an  roadblocks event Completion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myDrawBarriers_DrawComplete(object sender, DrawEventArgs e)
        {
            try
            {
                myDrawPoint.IsEnabled = false;
                ESRI.ArcGIS.Client.Graphic gPhic = new Graphic();
                gPhic.Geometry = e.Geometry;
                RouteAnalyze.RouteBarriers.Add(gPhic);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// Path analysis
        /// </summary>
        private void _Solved()
        {
            RouteAnalyze.Solved(_MyMap.SpatialReference);
        }


    }
}
