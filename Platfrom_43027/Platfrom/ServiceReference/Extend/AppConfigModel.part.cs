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

namespace Gsafety.PTMS.ServiceReference.AppConfigService
{
    public partial class ConfigTree
    {
        public ConfigTree()
        {
            this.Value = new ConfigItem();
            this.Children = new System.Collections.ObjectModel.ObservableCollection<ConfigTree>();
        }
    }
}
