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
using Jounce.Core.Command;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using Jounce.Core.View;
using Jounce.Framework;
using Jounce.Framework.Command;
using Gsafety.PTMS.Share;
using System.Reflection;

namespace GisManagement.ViewModels
{
    public partial class GisViewModel
    {
        /// <summary>
        /// ActivateView
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="viewParameters"></param>
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            //EventAggregator.Publish(GisManagement.GisName.SpatialQuery.AsViewNavigationArgs());
            try
            {
                EventAggregator.Publish(GisManagement.GisName.GpsCarHisDataViewMonitor.AsViewNavigationArgs());
                base.ActivateView(viewName, viewParameters);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
