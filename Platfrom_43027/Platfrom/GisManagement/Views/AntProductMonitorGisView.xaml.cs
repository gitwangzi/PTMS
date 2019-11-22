using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Actions;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Tasks;
using GisManagement.Models;
using GisManagement.ViewModels;
using Gsafety.Common.CommMessage.Controls;
using Gsafety.Common.Converts;
using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
//using Gsafety.Ant.Alert.Models;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Printing;
using System.Windows.Threading;

namespace GisManagement.Views
{
    //[ExportAsView(GisName.AntProductMonitorGisV)]
    //[ExportViewToRegion(GisName.AntProductMonitorGisV, ViewContainer.GisContainer)]
    public partial class AntProductMonitorGisView : UserControl
    {
        
    }
}
